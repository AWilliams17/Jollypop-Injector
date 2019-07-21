using Jollypop_Injector.Injectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
using Jollypop_Injector.Classes;
using SharpUtils.MiscUtils;
using Microsoft.Win32;

namespace Jollypop_Injector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private UnmanagedInjector.Bitness currentUnmanagedBitness = UnmanagedInjector.Bitness.BIT_32;
        private static UnmanagedInjector unmanagedInjector = new UnmanagedInjector();
        private static ManagedInjector managedInjector = new ManagedInjector();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Closing += MainWindow_Closing;
            


            // Set up auto scrolling to the last added item in the output list boxes
            // Taken from https://bit.ly/2Gk13a0
            ((INotifyCollectionChanged)UnmanagedOutput.Items).CollectionChanged += OutputList_CollectionChanged;
            ((INotifyCollectionChanged)ManagedOutput.Items).CollectionChanged += OutputList_CollectionChanged;
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

        }

        private void ManagedDLLPathBtn_Click(object sender, RoutedEventArgs e)
        {

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

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GithubBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            // Save config
            Application.Current.Shutdown();
        }

        private void Bit32RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentUnmanagedBitness = UnmanagedInjector.Bitness.BIT_32;
        }

        private void Bit64RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentUnmanagedBitness = UnmanagedInjector.Bitness.BIT_64;
        }
    }
}
