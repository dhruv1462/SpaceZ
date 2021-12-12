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
using System.Windows.Shapes;

namespace DSN
{
    /// <summary>
    /// Interaction logic for MissionControlSystem.xaml
    /// </summary>
    public partial class MissionControlSystem : Window
    {
        public MissionControlSystem()
        {
            InitializeComponent();
        }


        private void addSpaceCraftBtn_Click(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem(); //create your new form.
            communicationSystem.Show(); //show the new form.
            this.Close();
        }
    }
}
