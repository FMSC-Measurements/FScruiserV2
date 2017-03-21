using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms
{
    public partial class FormCruiserSelection : Form, ICruiserSelectionView
    {
        private FormCruiserSelectionLogic _logicController;
        KeysConverter _keyConverter = new KeysConverter();

        public FormCruiserSelection()
        {
            _logicController = new FormCruiserSelectionLogic(ApplicationSettings.Instance, this);
            _logicController.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_logicController_PropertyChanged);

            this.KeyPreview = true;
            InitializeComponent();
            UpdateCruiserList();
        }

        void _logicController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Tree":
                    {
                        UpdateTree();
                        break;
                    }
                case "Cruisers":
                    {
                        UpdateCruiserList();
                        break;
                    }
            }
        }

        void UpdateTree()
        {
            var tree = _logicController.Tree;
            if (tree != null)
            {
                _treeNumLBL.Text = "Tree #:" + tree.TreeNumber;
                _stratumLBL.Text = "Stratum: " + tree.Stratum.GetDescriptionShort();
                _sampleGroupLBL.Text = "Sg: " + tree.SampleGroup.GetDescriptionShort();
            }
            else
            {
                _treeNumLBL.Text = String.Empty;
                _stratumLBL.Text = String.Empty;
                _sampleGroupLBL.Text = String.Empty;
            }

            _cmLBL.Text = _logicController.TreeCountMeasure;
            _cmLBL.Visible = !string.IsNullOrEmpty(_cmLBL.Text);
        }

        void UpdateCruiserList()
        {
            this._crusierSelectPanel.Controls.Clear();

            foreach (var c in _logicController.Cruisers)
            {
                Button b = new Button();
                b.Dock = DockStyle.Top;
                b.Size = new System.Drawing.Size(240, 20);
                if (c.Key != null)
                {
                    b.Text = String.Format("[&{0}] {1}", c.Key, c.Value.Initials);
                }
                else
                {
                    b.Text = String.Format(" {0}", c.Value.Initials);
                }
                b.Click += new EventHandler(button_Click);
                b.Tag = c.Value;
#if NetCF
                FMSC.Controls.DpiHelper.AdjustControl(b);
#endif
                b.Parent = this._crusierSelectPanel;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Cruiser cruiser = (Cruiser)b.Tag;
            _logicController.HandleCruiserSelected(cruiser);
        }

        public void ShowDialog(IWin32Window owner, Tree tree)
        {
            _logicController.Tree = tree;

            ShowDialog(owner);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            var key = _keyConverter.ConvertToString(e.KeyCode);
            _logicController.HandleKeyDown(key);
        }
    }
}