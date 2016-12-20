using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FSCruiser.Core.Models;
using FSCruiser.WinForms;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiserV2.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public Exception Exception { get; set; }

        private void _editTDV_BTN_Click(object sender, EventArgs e)
        {
            TreeDefaultValueDO tdv = new TreeDefaultValueDO();

            using (var view = new FormEditTreeDefault((FMSC.ORM.Core.DatastoreRedux)null))
            {
                view.Load += (Object sndr, EventArgs args) =>
                    {
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.SPECIES).Text = "test";
                        view.Controls.Find<ComboBox>(TREEDEFAULTVALUE.LIVEDEAD).Text = "D";
                        view.Controls.Find<ComboBox>(TREEDEFAULTVALUE.PRIMARYPRODUCT).Text = "02";
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.FIACODE).Text = 1.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.AVERAGEZ).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.BARKTHICKNESSRATIO).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.CONTRACTSPECIES).Text = "test";
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.CULLPRIMARY).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.CULLSECONDARY).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.FORMCLASS).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.HIDDENPRIMARY).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.HIDDENSECONDARY).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.MERCHHEIGHTLOGLENGTH).Text = 1.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.MERCHHEIGHTTYPE).Text = "test";
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.RECOVERABLE).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.REFERENCEHEIGHTPERCENT).Text = .2f.ToString();
                        view.Controls.Find<TextBox>(TREEDEFAULTVALUE.TREEGRADE).Text = "test";
                    };
                view.ShowDialog(tdv);

                Debug.Assert(tdv.Species == "test");
                Debug.Assert(tdv.LiveDead == "D");
                Debug.Assert(tdv.PrimaryProduct == "02");
                Debug.Assert(tdv.FIAcode == 1);

                Debug.Assert(tdv.AverageZ == .2f);
                Debug.Assert(tdv.BarkThicknessRatio == .2f);
                Debug.Assert(tdv.ContractSpecies == "test");
                Debug.Assert(tdv.CullPrimary == .2f);
                Debug.Assert(tdv.CullSecondary == .2f);
                Debug.Assert(tdv.FormClass == .2f);
                Debug.Assert(tdv.HiddenPrimary == .2f);
                Debug.Assert(tdv.HiddenSecondary == .2f);
                Debug.Assert(tdv.MerchHeightLogLength == 1);
                Debug.Assert(tdv.MerchHeightType == "test");
                Debug.Assert(tdv.Recoverable == .2f);
                Debug.Assert(tdv.ReferenceHeightPercent == .2f);
                Debug.Assert(tdv.TreeGrade == "test");
            }
        }

        private void STRSampplingTest_BTN_Click(object sender, EventArgs e)
        {
            OnTallyTestForm view = new OnTallyTestForm();
            view.ShowDialog();
        }

        private void _plotInfo_BTN_Click(object sender, EventArgs e)
        {
            var stratum = new PlotStratum();
            var plot = new Plot() { Stratum = stratum };

            using (var view = new FormPlotInfo())
            {
                view.ShowDialog(plot, stratum, false);
            }
        }

        private void LimitingDistance_Click(object sender, EventArgs e)
        {
            var fpsOrBaf = 20;
            var isVariableRadious = true;
            using (var view = new FormLimitingDistance(fpsOrBaf, isVariableRadious))
            {
                view.Closed += (object obj, EventArgs ea) =>
                {
                    if (!String.IsNullOrEmpty(view.Report))
                    {
                        MessageBox.Show(view.Report);
                    }
                };

                view.Activated += (obj, ea) =>
                    {
                        view.Controls.FindByDataMember("DBH").Text = "10";
                        view.Controls.FindByDataMember("SlopePCT").Text = "0";
                        //view.Controls.FindByDataMember("BAForFPSize").Text = "20";
                        view.Controls.FindByDataMember("MeasureTo").Text = FSCruiser.Core.DataEntry.LimitingDistanceCalculator.MEASURE_TO_FACE;
                    };

                view.ShowDialog();

                var ldValue = view.Controls.Find("LimitingDistance").Text;
                Debug.Assert(ldValue == "19.03");
            }
        }
    }
}