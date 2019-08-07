using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Jollypop_Injector.Classes;
using Jollypop_Injector.ConfigContainer;
using Jollypop_Injector.Injectors;
using static Jollypop_Injector.Injectors.UnmanagedInjector;
using SharpUtils.MiscUtils;
using Registrar;

namespace Jollypop_Injector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static Config config = new Config();
        private static UnmanagedInjector unmanagedInjector = new UnmanagedInjector();
        private static ManagedInjector managedInjector = new ManagedInjector();
        private Bitness currentUnmanagedBitness = Bitness.BIT_32;

        public MainWindow()
        {
            DoAdminCheck();
            DataContext = this;
            InitializeComponent();
            Closing += MainWindow_Closing;
            LoadSettings();
            currentUnmanagedBitness = (Bitness)config.Settings.GetOption<int>("UnmanagedBitness");
            SetUnmanagedFields();
            SetManagedFields();
            // Set up auto scrolling to the last added item in the output list boxes
            // Taken from https://bit.ly/2Gk13a0
            ((INotifyCollectionChanged)UnmanagedOutput.Items).CollectionChanged += OutputList_CollectionChanged;
            ((INotifyCollectionChanged)ManagedOutput.Items).CollectionChanged += OutputList_CollectionChanged;
        }

        private void LoadSettings()
        {
            try
            {
                config.Settings.LoadSettings();
            }
            catch (RegLoadException)
            {
                try
                {
                    config.Settings.SaveSettings();
                }
                catch (RegSaveException ex)
                {
                    MessageBox.Show($"Failed to save default settings. Error message: {ex.Message}", "Error while saving settings to Registry.");
                }
            }
        }

        private void SetUnmanagedFields()
        {
            Bit32RadioButton.IsChecked = (currentUnmanagedBitness == Bitness.BIT_32);
            Bit64RadioButton.IsChecked = (currentUnmanagedBitness == Bitness.BIT_64);
            UnmanagedTargetTextBox.Text = config.Settings.GetOption<string>("UnmanagedTarget");
            UnmanagedDLLPathTextBox.Text = config.Settings.GetOption<string>("UnmanagedDLLPath");
        }

        private void SetManagedFields()
        {
            ManagedTargetTextBox.Text = config.Settings.GetOption<string>("ManagedTarget");
            ManagedDLLPathTextBox.Text = config.Settings.GetOption<string>("ManagedDLLPath");
            ManagedNamespaceTextBox.Text = config.Settings.GetOption<string>("ManagedNamespace");
            ManagedClassnameTextBox.Text = config.Settings.GetOption<string>("ManagedClassname");
            ManagedMethodnameTextBox.Text = config.Settings.GetOption<string>("ManagedMethodname");
        }

        private void DoAdminCheck()
        {
            if (!AdminCheckHelper.IsRunningAsAdmin())
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("This application requires you to run it as an administrator to work properly. " +
                    "Please re-run as administrator.", "Not Admin!");
                Application.Current.Shutdown();
            }
        }

        private void OutputList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                UnmanagedOutput.ScrollIntoView(e.NewItems[0]);
            }
        }

        public ObservableCollection<string> UnmanagedOutputList
        {
            get { return unmanagedInjector.InjectorOutput; }
        }

        public ObservableCollection<string> ManagedOutputList
        {
            get { return managedInjector.InjectorOutput; }
        }

        private void UnmanagedInjectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int targetPID = Utils.GetPid(UnmanagedTargetTextBox.Text);
                string dllLocation = UnmanagedDLLPathTextBox.Text;
                unmanagedInjector.Inject(targetPID, dllLocation, currentUnmanagedBitness);
            }
            catch (Exception ex)
            {
                if (ex is ProcessNotFoundException || ex is FileNotFoundException)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                else throw;
            }
        }

        private void ManagedInjectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string targetName = ManagedTargetTextBox.Text;
                string dllLocation = ManagedDLLPathTextBox.Text;
                string nameSpace = ManagedNamespaceTextBox.Text;
                string className = ManagedClassnameTextBox.Text;
                string methodName = ManagedMethodnameTextBox.Text;
                managedInjector.Inject(targetName, dllLocation, nameSpace, className, methodName);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ManagedDLLPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] dllPath = SharpUtils.FileUtils.DialogHelpers.SelectFilesDialog("Payload DLL File", "DLL files (*.dll)|*.dll", false);
            if (dllPath.Length != 0)
                ManagedDLLPathTextBox.Text = dllPath[0];
        }

        private void UnmanagedDLLPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] dllPath = SharpUtils.FileUtils.DialogHelpers.SelectFilesDialog("Payload DLL File", "DLL files (*.dll)|*.dll", false);
            if (dllPath.Length != 0)
                UnmanagedDLLPathTextBox.Text = dllPath[0];
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GithubBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/AWilliams17/Jollypop-Injector");
        }

        private void Bit32RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentUnmanagedBitness = Bitness.BIT_32;
        }

        private void Bit64RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentUnmanagedBitness = Bitness.BIT_64;
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            SaveAllSettings();
            Application.Current.Shutdown();
        }

        private void SaveAllSettings()
        {
            config.Settings.SetOption("UnmanagedBitness", (int)currentUnmanagedBitness);
            config.Settings.SetOption("UnmanagedTarget", UnmanagedTargetTextBox.Text);
            config.Settings.SetOption("UnmanagedDLLPath", UnmanagedDLLPathTextBox.Text);
            config.Settings.SetOption("ManagedTarget", ManagedTargetTextBox.Text);
            config.Settings.SetOption("ManagedDLLPath", ManagedDLLPathTextBox.Text);
            config.Settings.SetOption("ManagedNamespace", ManagedNamespaceTextBox.Text);
            config.Settings.SetOption("ManagedClassname", ManagedClassnameTextBox.Text);
            config.Settings.SetOption("ManagedMethodname", ManagedMethodnameTextBox.Text);
            config.Settings.SaveSettings();
        }
    }
}
