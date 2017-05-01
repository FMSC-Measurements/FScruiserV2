using FSCruiser.Core;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSCruiser.WinForms
{
    public partial class FormCruiserSelection
    {
        KeysConverter _keyConverter = new KeysConverter();

        #region ViewModel

        private FormCruiserSelectionLogic _viewModel;

        public FormCruiserSelectionLogic ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                OnViewModelChanging();
                _viewModel = value;
                OnViewModelChanged();
            }
        }

        private void OnViewModelChanging()
        {
            var vm = ViewModel;
            if (vm != null)
            {
                vm.PropertyChanged -= ViewModel_PropertyChanged;
            }
        }

        private void OnViewModelChanged()
        {
            var vm = ViewModel;
            if (vm != null)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        #endregion ViewModel

        public Tree Tree
        {
            get { return ViewModel.Tree; }
            set { ViewModel.Tree = value; }
        }

        public FormCruiserSelection()
        {
            ViewModel = new FormCruiserSelectionLogic(ApplicationSettings.Instance, this);

            InitializeComponent();

#if !NetCF
            StartPosition = FormStartPosition.CenterParent;
#endif
            KeyPreview = true;
            UpdateCruiserList();
        }

        void UpdateTree()
        {
            var tree = ViewModel.Tree;
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

            _cmLBL.Text = ViewModel.TreeCountMeasure;
            _cmLBL.Visible = !string.IsNullOrEmpty(_cmLBL.Text);
        }

        void UpdateCruiserList()
        {
            this._crusierSelectPanel.Controls.Clear();

            foreach (var c in ViewModel.Cruisers)
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
            ViewModel.HandleCruiserSelected(cruiser);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            var key = _keyConverter.ConvertToString(e.KeyCode);
            ViewModel.HandleKeyDown(key);
        }
    }
}