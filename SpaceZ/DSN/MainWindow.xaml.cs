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

namespace DSN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void communicationSystemBtn_Click(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show(); 
            this.Close();
        }

        private void missionControlSystemBtn_Click(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); 
            missionControlSystem.Show(); 
            this.Close();
        }
    }
}
