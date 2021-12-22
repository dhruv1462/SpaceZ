using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
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
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string updateQuery = "Update spacecraftinfo set launchStatus = 'active' where spacecraftName = '" + launchSpacecraftCombo.SelectedItem.ToString() + "'";
            cmd = new SqlCommand(updateQuery, cnn);
            adapter.UpdateCommand = cmd;
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            adapter.Dispose();
            var missionControlSystem = new MissionControlSystem();
            missionControlSystem.Show();
            this.Close();
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Users\\dpatel81\\Desktop\\Intel\\SpaceZ\\SpaceZ\\SpaceZ\\bin\\Debug\\SpaceZ.exe");
            p.Start();
            SqlDataReader reader;
            double orbitRadius = 0;
            string selectQuery = "select orbitRadius from spacecraftinfo where spacecraftName = '" + launchSpacecraftCombo.SelectedItem.ToString() + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orbitRadius = reader.GetDouble(0);
            }
            double timeToOrbit = (double)orbitRadius / 3600 + 10;
            reader.Close();
            cmd.Dispose();
            double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            timestamp = timestamp + timeToOrbit;
            string insertQuery = "Insert Into timeToOrbit (spacecraftName, timespan) values('" + launchSpacecraftCombo.SelectedItem.ToString() + "','" + timestamp + "')";
            cmd = new SqlCommand(insertQuery, cnn);
            adapter = new SqlDataAdapter();
            adapter.InsertCommand = cmd;
            adapter.InsertCommand.ExecuteNonQuery();
            cnn.Close();
        }

        private void comboNewSpacecrafts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void listOfSpacecrafts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string spacecraftName = listOfSpacecrafts.SelectedItem.ToString();
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select * from spacecraftinfo Where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string information = "\nSpacecraft Name: " + reader.GetString(0) + "\nOrbit Radius: " + reader.GetDouble(1) + "\nPayload Name: " + reader.GetString(2) + "\nPayload Type: " + reader.GetString(3) + "\nLaunch Status: " + reader.GetString(4);
                MessageBox.Show(information, reader.GetString(0));
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
