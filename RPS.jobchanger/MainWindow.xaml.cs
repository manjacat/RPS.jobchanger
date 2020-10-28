using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Oracle.ManagedDataAccess.Client;
using RPS.jobchanger.Constant;
using RPS.jobchanger.Database;
using RPS.jobchanger.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RPS.jobchanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        BackgroundWorker worker = new BackgroundWorker();
        public string jobName_old { get; set; }
        public string jobName_new { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PrintHelper.Trace(Messages.StartProgram);
            
            // Set the window theme to Dark.Red
            ThemeManager.Current.ChangeTheme(this, "Dark.Red");
            AdditionalInitialization();
        }
        public void AdditionalInitialization()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //TODO: update progress
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //initiate report progress at 0%
            (sender as BackgroundWorker).ReportProgress(0);
            //disable button
            btnStart.IsEnabled = false;

            DudeHelper helper = new DudeHelper();
            helper.UpdateJobName(jobName_old, jobName_new);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Job complete
            btnStart.IsEnabled = true;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            jobName_new = txt_new_job.Text;
            jobName_old = txt_old_job.Text;
            worker.RunWorkerAsync();
        }
    }
}
