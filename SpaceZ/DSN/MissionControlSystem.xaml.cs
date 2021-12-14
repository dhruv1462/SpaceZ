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
    /// Interaction logic for MissionControlSystem.xaml
    /// </summary>
    public partial class MissionControlSystem : Window
    {
        public MissionControlSystem()
        {
            InitializeComponent();
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select spacecraftName from spacecraftinfo where launchStatus = 'inactive'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                launchSpacecraftCombo.Items.Add(reader.GetValue(0));
            }

            reader.Close();

            selectQuery = "Select spacecraftName from spacecraftinfo";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listOfSpacecrafts.Items.Add(reader.GetValue(0));
            }
            cnn.Close();
        }


        private void addSpaceCraftBtn_Click(object sender, RoutedEventArgs e)
        {
            var addSpacecraft = new AddSpacecraft();
            addSpacecraft.Show(); 
            this.Close();
        }

        private void btnLaunchSpacecraft_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string updateQuery = "Update spacecraftinfo set launchStatus = 'active' where spacecraftName = '" + launchSpacecraftCombo.SelectedItem.ToString() + "'";
            cmd = new SqlCommand(updateQuery, cnn);
            adapter.UpdateCommand = cmd;
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
            var missionControlSystem = new MissionControlSystem();
            missionControlSystem.Show();
            this.Close();
        }

        private void comboNewSpacecrafts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void listOfSpacecrafts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string spacecraftName = listOfSpacecrafts.SelectedItem.ToString();
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select spacecraftName, launchStatus from spacecraftinfo Where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MessageBox.Show(reader.GetString(1), reader.GetString(0));
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
