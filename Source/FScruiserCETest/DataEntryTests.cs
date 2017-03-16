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
using FScruiser.Core.Services;
using FMSC.ORM.SQLite;
using CruiseDAL;
using CruiseDAL.Schema;

namespace FSCruiserV2.Test
{
    public class DataEntryTests
    {
        ApplicationControllerMock _controller = new ApplicationControllerMock();
        DAL _dataStore;
        IDialogService _dialogService;
        ISoundService _soundService;

        IApplicationController Controller { get { return _controller; } }

        IDataEntryView _view = new DataEntryViewMock();
        FormDataEntryLogic _de;

        public DataEntryTests()
        {

            _dataStore = SetupDataStore();
            _dialogService = new DialogServiceMock();
            _soundService = new SoundServiceMock();
            var dataService = new IDataEntryDataService("01", _dataStore);
            var appSettings = new ApplicationSettings();

            _de = new FormDataEntryLogic(_controller
                , _dialogService
                , _soundService
                , dataService
                , appSettings
                , _view);
        }

        DAL SetupDataStore()
        {
            var dataStore = new DAL();

            var sale = new SaleDO(dataStore)
            {
                LogGradingEnabled = false,
                SaleNumber = "12345",
                Region = "01",
                Forest = "01",
                District = "01"
            };
            sale.Save();

            var unit = new CuttingUnitDO(dataStore) { 
                Code = "01"
            };

            unit.Save();

            var stratum = new StratumDO(dataStore) { Code = "01", 
                Method = CruiseMethods.STR };
            stratum.Save();
            unit.Strata.Add(stratum);
            unit.Strata.Save();

            var sg = new SampleGroupDO(dataStore) { Code = "01", 
                CutLeave = "C", 
                UOM = "1",
                PrimaryProduct = "01"
            };
            sg.Stratum = stratum;
            sg.SamplingFrequency = 5;
            sg.InsuranceFrequency = 0;

            sg.Save();

            var countTree = new CountTreeDO(dataStore)
            {
                SampleGroup = sg,
                CuttingUnit = unit
            };

            countTree.Save();

            return dataStore;
        }

        public void TestTreeTally()
        {
            var st = _dataStore.From<Stratum>().Read().First();

            var sg = _dataStore.From<SampleGroup>().Read().First();
            sg.SampleSelectorType = CruiseDAL.Enums.SampleSelectorType.Block.ToString();

            CountTree count = _dataStore.From<CountTree>().Read().First();

            int numSamples = 500;
            for (int i = 0; i < numSamples; i++)
            {
                _de.OnTally(count);
            }

            System.Diagnostics.Debug.WriteLine("Sample Count = " + _controller.SampleCount.ToString());
            System.Diagnostics.Debug.WriteLine("ISample Count = " + _controller.ISampleCount.ToString());
        }
    }
}