using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Logic;
using FSCruiserV2.Forms;
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
            _de = new FormDataEntryLogic(_controller, _view);
        }


        public void TestTreeTally()
        {
            StratumVM st = new StratumVM();
            st.Method = CruiseDAL.Schema.Constants.CruiseMethods.STR;

            SampleGroupVM sg = new SampleGroupVM();
            sg.Stratum = st;
            sg.SamplingFrequency = 5;
            sg.InsuranceFrequency = 0;
            sg.SampleSelectorType = CruiseDAL.Enums.SampleSelectorType.Block.ToString();
            sg.Sampler = _controller.MakeSampleSelecter(sg);

            CountTreeVM count = new CountTreeVM();
            count.SampleGroup = sg;

            int numSamples = 10000;
            for(int i = 0; i < numSamples; i++)
            {
                _de.OnTally(count); 
            }

            System.Diagnostics.Trace.WriteLine("Sample Count = " + _controller.SampleCount.ToString());
            System.Diagnostics.Trace.WriteLine("ISample Count = " + _controller.ISampleCount.ToString());


        }

    }
}
