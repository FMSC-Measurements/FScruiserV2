using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class SampleGroupRow : UserControl
    {
        public delegate void SpeciesClickedEventHandler(object sender, SubPop sp);

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this._expand_BTN.Text = value;
                base.Text = value;
            }
        }

        public bool IsExpanded 
        {
            get { return this._speciesContainer.Visible; }
            set { this._speciesContainer.Visible = value; }
        }

        List<SubPop> _subPopList = new List<SubPop>();

        public event SpeciesClickedEventHandler SpeciesClicked;


        public SampleGroupRow()
        {
            InitializeComponent();
            this._expand_BTN.Click += new EventHandler(_expand_BTN_Click);
        }

        void _expand_BTN_Click(object sender, EventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        void HandleSpeciesClicked(object sender, EventArgs e)
        {
            var spButton = sender as SpeciesRow;
            System.Diagnostics.Debug.Assert(spButton != null);
            if (sender == null) { return; }
            var subPop = spButton.SubPopulation;
            System.Diagnostics.Debug.Assert(subPop != null);

            if (SpeciesClicked != null)
            {
                SpeciesClicked(spButton, subPop);
            }
        }

        
        void AddSubPop(SubPop subPop)
        {
            var spRow = new SpeciesRow()
            {
                SubPopulation = subPop,
                Dock = DockStyle.Top
            };
            spRow.MinimumSize = new Size(0, 23);

            spRow.Click += HandleSpeciesClicked;

            this._speciesContainer.Controls.Add(spRow);
        }

        public void AddSupPops(IEnumerable<SubPop> pops)
        {
            this._speciesContainer.SuspendLayout();
            foreach(SubPop sp in pops.Reverse())
            {
                this.AddSubPop(sp);
            }
            this._speciesContainer.ResumeLayout(false);
        }

    }
}
