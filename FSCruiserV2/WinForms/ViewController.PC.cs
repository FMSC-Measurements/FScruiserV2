using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.WinForms
{


    public class ViewController : WinFormsViewControllerBase
    {

        public ViewController()
        {
            
        }

        #region IViewController Members

        public override DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, TreeVM optTree, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public override void ShowLogsView(CruiseDAL.DataObjects.StratumDO stratum, TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public override void ShowManageCruisers()
        {
            throw new NotImplementedException();
        }

        public override void ShowDataEntry(CruiseDAL.DataObjects.CuttingUnitDO unit)
        {
            _dataEntryView = null;

            _dataEntryView = new FormDataEntry(this.ApplicationController, unit);
            _dataEntryView.Owner = this.MainView; 
            _dataEntryView.ShowDialog();

        }

        public override DialogResult ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public override DialogResult ShowEditTreeDefault(TreeDefaultValueDO tdv)
        {
            throw new NotImplementedException();
        }





        public override TreeDefaultValueDO ShowAddPopulation()
        {
            throw new NotImplementedException();
        }

        public override TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg)
        {
            throw new NotImplementedException();
        }

        public override void ShowBackupUtil()
        {
            throw new NotImplementedException();
        }

        public override System.Windows.Forms.DialogResult ShowOpenCruiseFileDialog(out string fileName)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "cruise files (*.cruise)|*.cruise";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                }
                else
                {
                    fileName = null;
                }
                return result;
            }
        }

        public override void SignalMeasureTree(bool showMessage)
        {
            System.Media.SystemSounds.Beep.Play();
        }

        public override void SignalInsuranceTree()
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        public override void SignalInvalidAction()
        {
            System.Media.SystemSounds.Exclamation.Play();
        }


        #endregion

    }
}
