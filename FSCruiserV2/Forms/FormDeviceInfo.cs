using System;
//using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FSCruiserV2.Forms
{
    public partial class FormDeviceInfo : Form
    {
        public FormDeviceInfo()
        {
            InitializeComponent();

            if (FSCruiserV2.Logic.ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void FormDeviceInfo_Load(object sender, EventArgs e)
        {
            FMSC.Utility.MobileDeviceInfo DeviceInfo = new FMSC.Utility.MobileDeviceInfo();
            string DeviceInfoString = " Manufacturer:\t" + DeviceInfo.platformManufacturer + "\r\n";
            DeviceInfoString += " Model:\t\t" + DeviceInfo.deviceModel + "\r\n";
            DeviceInfoString += " Identification:\t" + DeviceInfo.serialNumber + "\r\n";
            DeviceInfoString += " Win CE Version:\t" + Environment.OSVersion.Version.ToString() + "\r\n";
            DeviceInfoString += " Platform Type:\t" + DeviceInfo.betterPlatformType + "\r\n";
            DeviceInfoString += " Owner Name:\t" + DeviceInfo.userName + "\r\n";
            DeviceInfoString += " Device Name:\t" + DeviceInfo.machineName + "\r\n";
            DeviceInfoString += " Device Description:\t" + DeviceInfo.machineDescription + "\r\n";

       //     DeviceInfoString += "Memory Load:\t" + DeviceInfo.physicalMemoryLoadPercent.ToString() + "%\r\n";

       //     DeviceInfoString += "Main Battery Charge:\t" + DeviceInfo.mainBatteryLifePercent.ToString() + "%\r\n";

       //     DeviceInfoString += "Backup Battery Charge:\t" + DeviceInfo.backupBatteryLifePercent.ToString() + "%\r\n";

            textBoxDeviceInfo.Text = DeviceInfoString;

            // The progress bars for memory load and battery charge
            this.progressBarMemoryLoad.Value = (int)DeviceInfo.physicalMemoryLoadPercent;

            int mainBatteryPercent   = 100;
            int backupBatteryPercent = 0;

            if (DeviceInfo.deviceModel.IndexOf("Emulator") == -1)// not contains "emulator"
            {
                mainBatteryPercent = (int)DeviceInfo.mainBatteryLifePercent;
                backupBatteryPercent = (int)DeviceInfo.backupBatteryLifePercent;
            }

            if (backupBatteryPercent > 100)
                backupBatteryPercent = 0;

            this.progressBarMainBatteryCharge.Value = mainBatteryPercent;
            this.progressBarBackupBatteryCharge.Value = backupBatteryPercent;            
        }

        private void progressBarMainBatteryCharge_ParentChanged(object sender, EventArgs e)
        {

        }
    }
}