using Jollypop_Injector.Injectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace Jollypop_Injector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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
            get { return unmanagedInjector.UnmanagedInjectorOutput; }
        }

        private void UnmanagedInjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManagedInjectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManagedDLLPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnmanagedDLLPathBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
