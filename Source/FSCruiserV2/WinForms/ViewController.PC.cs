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

        public override bool ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public override void ShowManageCruisers()
        {
            throw new NotImplementedException();
        }

        public override void ShowBackupUtil()
        {
            throw new NotImplementedException();
        }

        public override bool ShowOpenCruiseFileDialog(out string fileName)
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
                return result == DialogResult.OK;
            }
        }

        #endregion IViewController Members
    }
}