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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select spacecraftName from timeToOrbit";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBoxSpacecrafts.Items.Add(reader.GetValue(0));
            }

            reader.Close();
            cnn.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void comboBoxSpacecrafts_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonStartTelemetry_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select timeToOrbit from timeToOrbit";
            cmd = new SqlCommand(selectQuery, cnn);
            double timeStamp = 0;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               timeStamp = reader.GetDouble(0);
            }
            double currenttimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            double counter = timeStamp - currenttimestamp; 
            reader.Close();
            cnn.Close();
        }

    private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
