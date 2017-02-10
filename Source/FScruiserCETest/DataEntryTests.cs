using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiserV2.Test.Mocks;

namespace FSCruiserV2.Test
{
    public class DataEntryTests
    {
        ApplicationControllerMock _controller = new ApplicationControllerMock();

        IApplicationController Controller { get { return _controller; } }

        IDataEntryView _view = new DataEntryViewMock();
        FormDataEntryLogic _de;

        public DataEntryTests()
        {
            var cu = new CuttingUnit();

            _de = new FormDataEntryLogic(cu, _controller, _view);
        }

        public void TestTreeTally()
        {
            var st = new Stratum();
            st.Method = CruiseDAL.Schema.CruiseMethods.STR;

            var sg = new SampleGroup();
            sg.Stratum = st;
            sg.SamplingFrequency = 5;
            sg.InsuranceFrequency = 0;
            sg.SampleSelectorType = CruiseDAL.Enums.SampleSelectorType.Block.ToString();

            CountTree count = new CountTree();
            count.SampleGroup = sg;

            int numSamples = 10000;
            for (int i = 0; i < numSamples; i++)
            {
                _de.OnTally(count);
            }

            System.Diagnostics.Debug.WriteLine("Sample Count = " + _controller.SampleCount.ToString());
            System.Diagnostics.Debug.WriteLine("ISample Count = " + _controller.ISampleCount.ToString());
        }
    }
}