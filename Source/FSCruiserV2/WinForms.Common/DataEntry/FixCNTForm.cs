using System.Windows.Forms;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCNTForm : Form
    {
        IPlotDataService _dataService;
        IPlotDataService DataService 
        {
            get { return _dataService; }
            set
            {
                _dataService = value;
                OnDataServiceChanged();
            }
        }

        FixCNTForm()
        {
            InitializeComponent();
#if !NetCF
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
#else
            this.Menu = new MainMenu();
#endif
        }

        public FixCNTForm(IFixCNTTallyPopulationProvider populationProvider
            , IPlotDataService dataService)
            : this()
        {
            DataService = dataService;
            _tallyControl.PopulationProvider = populationProvider;
        }

        private void OnDataServiceChanged()
        {
            _tallyControl.DataService = DataService;
        }

        public DialogResult ShowDialog(FixCNTPlot plot)
        {
            _tallyControl.TallyCountProvider = plot;
            this.Text = string.Format("Stratum:{0} Plot:{1}", plot.Stratum.Code, plot.PlotNumber);
            return this.ShowDialog();
        }
    }
}