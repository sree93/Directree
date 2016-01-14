using System;
using System.Collections.Generic;
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

using System.Collections.ObjectModel;
using Directree.WPFPieChart.Shapes;
using Directree.Logic.Analytics;
using Directree.Logic.Cleaner;

namespace Directree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ObservableCollection<AssetClass> classes;/////////////////
        //private ObservableCollection<AssetClass> classes1;////////////////
        public MainWindow()
        {
            InitializeComponent();

            Logic.Data.DbHelper db = new Logic.Data.DbHelper();

            //classes = new ObservableCollection<AssetClass>(AssetClass.ConstructTestData());///////////
            //piePlotterC.DataContext = classes;////////////

            //classes1 = new ObservableCollection<AssetClass>(AssetClass.ConstructTestData1());/////////////
            //piePlotterD.DataContext = classes1;/////////////////
        }

        /// <summary>
        /// Close the entire application
        /// </summary>
        private void closeApp()
        {
            App.Current.Shutdown();
        }

        ///////////////////////// Events ////////////////////////////////
        private void menu_file_exit_Click(object sender, RoutedEventArgs e)
        {
            closeApp();
        }

        private void menu_cleaner_add_Click(object sender, RoutedEventArgs e)
        {
            CleanerAddWindow cleanerAdd = new CleanerAddWindow();
            cleanerAdd.Show();
        }
    }
}
