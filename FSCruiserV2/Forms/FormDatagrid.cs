using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using CruiseDAL.DataObjects;

namespace FSCruiserV2.Forms
{
    public partial class FormDatagrid : Form
    {
        public ApplicationController Controller { get; set; }
        public BindingList<TreeDO> TreeList { get; set; }


        public FormDatagrid(ApplicationController controller)
        {
            this.Controller = controller;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TreeBindingSource.DataSource = Controller.TreeList;
        }



        public DialogResult ShowDialog(CountTreeDO count, List<TreeDO> treeList)
        {
            this.TreeBindingSource.DataSource = treeList;
            this.editableDataGrid1.DataSource = this.TreeBindingSource;
            return this.ShowDialog();
        }
    }
}