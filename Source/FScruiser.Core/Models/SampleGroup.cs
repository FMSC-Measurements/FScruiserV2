using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class SampleGroup : SampleGroupDO
    {
        public SampleGroup()
            : base()
        { }

        [IgnoreField]
        public new Stratum Stratum
        {
            get
            {
                return (Stratum)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        ISampleSelector _sampler;

        [IgnoreField]
        public ISampleSelector Sampler
        {
            get
            {
                if (_sampler == null)
                {
                    _sampler = MakeSampleSelecter();
                }
                return _sampler;
            }
        }

        public IEnumerable<CountTree> Counts { get; set; }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<Stratum>(this.Stratum_CN);
        }

        public void LoadCounts(CuttingUnitDO unit)
        {
            var counts = new List<CountTree>();
            var tallySettings = DAL.From<TallySettings>()
                .Where("SampleGroup_CN = ?")
                .GroupBy("CountTree.SampleGroup_CN", "CountTree.TreeDefaultValue_CN", "CountTree.Tally_CN")
                .Read(SampleGroup_CN);

            foreach (TallySettings ts in tallySettings)
            {
                CountTree count = DAL.From<CountTree>()
                    .Where("CuttingUnit_CN = ? AND SampleGroup_CN = ? AND Tally_CN = ?")
                    .Read(unit.CuttingUnit_CN
                    , ts.SampleGroup_CN
                    , ts.Tally_CN).FirstOrDefault();
                if (count == null)
                {
                    count = new CountTree(DAL);
                    count.CuttingUnit = unit;
                    count.SampleGroup_CN = ts.SampleGroup_CN;
                    count.TreeDefaultValue_CN = ts.TreeDefaultValue_CN;
                    count.Tally_CN = ts.Tally_CN;

                    count.Save();
                }

                count.SampleGroup = this;

                counts.Add(count);
            }

            Counts = counts;
        }

        public bool HasTreeDefault(TreeDefaultValueDO tdv)
        {
            return DAL.ExecuteScalar<bool>("SELECT count(1) " +
                    "FROM SampleGroupTreeDefaultValue " +
                    "WHERE TreeDefaultValue_CN = ? AND SampleGroup_CN = ?"
                    , tdv.TreeDefaultValue_CN, SampleGroup_CN);
        }

        #region Sample Selecter Methods

        // Exceptions:
        //   System.InvalidOperationException:
        //     An error occurred during serialization. The original exception is available
        //     using the System.Exception.InnerException property.
        public void SerializeSamplerState()
        {
            if (_sampler == null) { return; }
            var selector = _sampler;
            if (selector != null && (selector is BlockSelecter || selector is SystematicSelecter))
            {
                XmlSerializer serializer = new XmlSerializer(selector.GetType());
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, selector);
                this.SampleSelectorState = writer.ToString();
                this.SampleSelectorType = selector.GetType().Name;
            }
        }

        // Exceptions:
        //   System.InvalidOperationException:
        //     An error occurred during deserialization. The original exception is available
        //     using the System.Exception.InnerException property.
        public ISampleSelector DeserializeSamplerState()
        {
            XmlSerializer serializer = null;

            switch (this.SampleSelectorType)
            {
                case "Block":
                case "BlockSelecter":
                    {
                        serializer = new XmlSerializer(typeof(BlockSelecter));
                        break;
                    }
                case "SRS":
                case "SRSSelecter":
                    {
                        return new SRSSelecter((int)this.SamplingFrequency, (int)this.InsuranceFrequency);
                    }
                case "Systematic":
                case "SystematicSelecter":
                    {
                        serializer = new XmlSerializer(typeof(SystematicSelecter));
                        break;
                    }
                case "ThreeP":
                case "ThreePSelecter":
                    {
                        return new ThreePSelecter((int)this.KZ, (int)this.InsuranceFrequency);
                    }
            }
            if (serializer != null)
            {
                using (StringReader reader = new StringReader(this.SampleSelectorState))
                {
                    SampleSelecter sampler = (SampleSelecter)serializer.Deserialize(reader);
                    return sampler;
                }
            }

            return null;
        }

        public ISampleSelector MakeSampleSelecter()
        {
            var method = Stratum.Method;

            if (method == "100"
                || method == "FIX"
                || method == "PNT"
                || method == "FIXCNT")
            { return null; }

            if (!string.IsNullOrEmpty(this.SampleSelectorState))
            {
                var selector = InitializePersistedSampler();
                if (selector != null)
                { return selector; }
            }

            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    return null;
            //}

            switch (method)
            {
                case "STR":
                    {
                        if (String.Equals(SampleSelectorType, CruiseMethods.SYSTEMATIC_SAMPLER_TYPE))
                        {
                            return MakeSystematicSampleSelector();
                        }
                        else if (String.Equals(SampleSelectorType, CruiseMethods.CLICKER_SAMPLER_TYPE))
                        {
                            return new ClickerSelecter();
                        }
                        else
                        {
                            return MakeBlockSampleSelector();
                        }
                    }
                case "3P":
                case "S3P":
                case "P3P":
                case "F3P":
                    {
                        return MakeThreePSampleSelector();
                    }
                case "FCM":
                case "PCM":
                    {
                        return MakeSystematicSampleSelector();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        ISampleSelector InitializePersistedSampler()
        {
            ISampleSelector selecter;
            try
            {
                selecter = DeserializeSamplerState();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e, "Exception");
                /*keep calm and carry on*/
                return null;
            }

            if (selecter != null && selecter is IFrequencyBasedSelecter)
            {
                if (!ValidateFreqSelecter((IFrequencyBasedSelecter)selecter))
                {
                    ((CruiseDatastore)DAL).LogMessage(string.Format("Frequency missmatch on SG:{0} Sf={1} If={2}; SelectorState: Sf={3} If={4};",
                            this.Code,
                            this.SamplingFrequency,
                            this.InsuranceFrequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency), "I");

                    if (CanEditSampleGroup())
                    {
                        SampleSelectorState = null;
                        return null;
                    }
                    else
                    {
                        throw new UserFacingException("Oops! Sample Frequency on sample group " +
                            this.Code + " has been modified.\r\n If you are trying to change the sample freqency during a cruise, you should create a new sample group.", (Exception)null);
                    }
                }
            }
            return selecter;
        }

        bool ValidateFreqSelecter(IFrequencyBasedSelecter freqSelecter)
        {
            //ensure sampler frequency matches sample group freqency
            if (freqSelecter != null
                && freqSelecter.Frequency != this.SamplingFrequency
                || freqSelecter.ITreeFrequency != this.InsuranceFrequency)
            {
                //older versions of FMSC.Sampling would use -1 instead of 0 if InsuranceFrequency was 0
                if (freqSelecter.ITreeFrequency == -1
                    && this.InsuranceFrequency == 0)
                { return true; }

                return false;
            }
            else { return true; }
        }

        private ISampleSelector MakeThreePSampleSelector()
        {
            int iFrequency = (int)this.InsuranceFrequency;
            int kz = (int)this.KZ;
            return new FMSC.Sampling.ThreePSelecter(kz, iFrequency);
        }

        private IFrequencyBasedSelecter MakeSystematicSampleSelector()
        {
            int iFrequency = (int)this.InsuranceFrequency;
            int frequency = (int)this.SamplingFrequency;
            if (frequency == 0) { return (IFrequencyBasedSelecter)null; }
            else
            {
                return new FMSC.Sampling.SystematicSelecter(frequency, iFrequency, true);
            }
        }

        private IFrequencyBasedSelecter MakeBlockSampleSelector()
        {
            int iFrequency = (int)this.InsuranceFrequency;
            int frequency = (int)this.SamplingFrequency;
            if (frequency == 0) { return (IFrequencyBasedSelecter)null; }
            else
            {
                return new FMSC.Sampling.BlockSelecter(frequency, iFrequency);
            }
        }

        public override string ToString()
        {
            return Code;
        }

        #endregion Sample Selecter Methods
    }
}