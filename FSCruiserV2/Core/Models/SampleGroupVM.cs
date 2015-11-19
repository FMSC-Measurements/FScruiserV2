using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.Sampling;
using System.Xml.Serialization;
using System.IO;

namespace FSCruiser.Core.Models
{
    public class SampleGroupVM : SampleGroupDO
    {
        public SampleGroupVM()
            : base()
        { }

        public new StratumVM Stratum
        {
            get
            {
                return (StratumVM)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<StratumVM>(STRATUM._NAME, this.Stratum_CN);
        }

        public SampleSelecter Sampler
        {
            get;
            set;
        }

        #region Sample Selecter Methods
        public void SerializeSamplerState()
        {
            SampleSelecter selector = this.Sampler;
            if (selector != null && (selector is BlockSelecter || selector is SystematicSelecter))
            {
                XmlSerializer serializer = new XmlSerializer(selector.GetType());
                StringWriter writer = new StringWriter();
                //selector.Count = (int)count.TreeCount;
                serializer.Serialize(writer, selector);
                this.SampleSelectorState = writer.ToString();
                this.SampleSelectorType = selector.GetType().Name;
                //count.TreeCount = selector.Count;
            }
        }

        public SampleSelecter LoadSamplerState()
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
                        return new ThreePSelecter((int)this.KZ, 10000, (int)this.InsuranceFrequency);
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

        public SampleSelecter MakeSampleSelecter()
        {
            SampleSelecter selecter = null;
            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    return null;
            //}

            if (string.IsNullOrEmpty(this.SampleSelectorState))
            {
                switch (this.Stratum.Method)
                {
                    case "100":
                        {
                            selecter = null;
                            break;
                        }
                    case "STR":
                        {
                            if (this.SampleSelectorType == "SystematicSelecter" && Constants.ALLOW_STR_SYSTEMATIC)
                            {
                                selecter = MakeSystematicSampleSelector();
                            }
                            else
                            {
                                selecter = MakeBlockSampleSelector();
                            }
                            break;
                        }
                    case "3P":
                    case "F3P":
                    case "P3P":
                        {
                            selecter = MakeThreePSampleSelector();
                            break;
                        }
                    case "FIX":
                    case "PNT":
                        {
                            selecter = null;
                            break;
                        }
                    case "FCM":
                    case "PCM":
                        {
                            selecter = MakeSystematicSampleSelector();
                            break;
                        }
                }
            }
            else
            {
                selecter = LoadSamplerState();

                //ensure sampler frequency matches sample group freqency 
                if (selecter != null && selecter is FMSC.Sampling.IFrequencyBasedSelecter
                    && (((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency != this.SamplingFrequency
                    || ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency != this.InsuranceFrequency))
                {
                    //HACK older versions of FMSC.Sampling would use -1 instead of 0 if InsuranceFrequency was 0
                    if (((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency == -1
                        && this.InsuranceFrequency == 0)
                    {
                        return selecter;
                    }

                    if (this.CanEditSampleGroup())
                    {
                        this.DAL.LogMessage(string.Format("Frequency missmatch on SG:{0} Sf={1} If={2}; SelectorState: Sf={3} If={4}; SelectorState reset",
                            this.Code,
                            this.SamplingFrequency,
                            this.InsuranceFrequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency), "I");

                        this.SampleSelectorState = string.Empty;
                        selecter = MakeSampleSelecter();
                    }
                    else
                    {
                        this.DAL.LogMessage(string.Format("Frequency missmatch on SG:{0} Sf={1} If={2}; SelectorState: Sf={3} If={4}; changes reverted",
                            this.Code,
                            this.SamplingFrequency,
                            this.InsuranceFrequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency,
                            ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency), "I");


                        this.ViewController.ShowMessage("Oops! Sample Frequency on sample group " +
                            this.Code + " has been modified.\r\n If you are trying to change the sample freqency during a cruise, you should create a new sample group.",
                            "Error", MessageBoxIcon.Exclamation);
                        this.SamplingFrequency = ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency;
                        this.InsuranceFrequency = ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency;
                    }

                }
            }

            return selecter;
        }

        private SampleSelecter MakeThreePSampleSelector()
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)this.InsuranceFrequency;
            int KZ = (int)this.KZ;
            int maxKPI = 100000;
            selecter = new FMSC.Sampling.ThreePSelecter(KZ, maxKPI, iFrequency);
            return selecter;
        }

        private SampleSelecter MakeSystematicSampleSelector()
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)this.InsuranceFrequency;
            int frequency = (int)this.SamplingFrequency;
            if (frequency == 0) { selecter = null; }
            else
            {
                selecter = new FMSC.Sampling.SystematicSelecter(frequency, iFrequency, true);
            }
            return selecter;
        }

        private SampleSelecter MakeBlockSampleSelector()
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)this.InsuranceFrequency;
            int frequency = (int)this.SamplingFrequency;
            if (frequency == 0) { selecter = null; }
            else
            {
                selecter = new FMSC.Sampling.BlockSelecter(frequency, iFrequency);
            }
            return selecter;
        }
        #endregion


    }
}
