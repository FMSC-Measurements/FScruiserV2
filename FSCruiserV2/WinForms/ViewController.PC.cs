using System;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.Common;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.WinForms
{
    public class ViewController : WinFormsViewControllerBase
    {
        public ViewController()
        {
        }

        #region IViewController Members

        public override void BeginShowSplash()
        {
            //using (FormAbout a = new FormAbout())
            //{
            //    a.StartPosition = FormStartPosition.CenterScreen;
            //    a.Show();
            //    //Application.Run(a);
            //}
        }

        public override DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public override void ShowManageCruisers()
        {
            throw new NotImplementedException();
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
            System.Media.SystemSounds.Exclamation.Play();
            if (showMessage)
            {
                MessageBox.Show("Measure Tree");
            }
        }

        public override void SignalInsuranceTree()
        {
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show("Insurance Tree");
        }

        public override void SignalInvalidAction()
        {
            System.Media.SystemSounds.Beep.Play();
        }

        public override void SignalPageChanged()
        {
            
        }

        public override void SignalTally()
        {
            
        }

        #endregion IViewController Members
    }
}