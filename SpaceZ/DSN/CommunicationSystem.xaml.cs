using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for CommunicationSystem.xaml
    /// </summary>
    public partial class CommunicationSystem : Window
    {
        private string selectedSpacecraftForInfo = "";
        public CommunicationSystem()
        {
            InitializeComponent();
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select spacecraftName from spacecraftinfo";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allSpacecraftCombo.Items.Add(reader.GetValue(0));
            }

            reader.Close();
            cnn.Close();
        }

        private void btnShowSpacecraft_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            string information = "";
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select * from spacecraftinfo where spacecraftName = '" + selectedSpacecraftForInfo + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                information = "\nSpacecraft Name: " + reader.GetString(0) + "\nOrbit Radius: " + reader.GetString(1) + "\nPayload Name: " + reader.GetString(2) + "\nPayload Type: " + reader.GetString(3) + "\nLaunch Status: " + reader.GetString(4);
            }

            reader.Close();
            cnn.Close();
            txtSpacecraftData.Text = information;
        }

        private void allSpacecraftCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSpacecraftForInfo = allSpacecraftCombo.SelectedItem.ToString();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void txtSpacecraftData_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
