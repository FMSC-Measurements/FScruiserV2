using System;
using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public partial class FormTallyHistory : Form
    {
        ApplicationController Controller { get; set; }

        public FormTallyHistory(ApplicationController controller)
        {
            this.Controller = controller;
            InitializeComponent();
            _BS_TallyActions.DataSource = Controller.TallyHistory;
        }

        private void _untallyButton_Click(object sender, EventArgs e)
        {
            TallyAction selectedAction = _BS_TallyActions.Current as TallyAction;
            if (selectedAction != null)
            {
                selectedAction.Count.TreeCount -= 1;
                selectedAction.Sampler.Count -= 1;
                if (selectedAction.TreeRecord != null)
                {
                    Controller.TreeList.Remove(selectedAction.TreeRecord);
                }
                Controller.TallyHistory.Remove(selectedAction);
            }

        
        }
    }
}