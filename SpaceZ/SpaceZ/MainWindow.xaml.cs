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
using System.Windows.Threading;

namespace SpaceZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double counter = 0;
        private DispatcherTimer timer = new DispatcherTimer();
        private double orbitRadius = 0;
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
            payloadLaunchBtn.IsEnabled = false;
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
            string selectQuery = "Select timespan from timeToOrbit Where spaceCraftName = '" + comboBoxSpacecrafts.SelectedItem.ToString() + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            double timeStamp = 0;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                timeStamp = reader.GetDouble(0);
            }
            double currenttimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            counter = (timeStamp - currenttimestamp);
            reader.Close();
            cmd.Dispose();

            string spacecraftName = comboBoxSpacecrafts.SelectedItem.ToString();
            string getOrbitRadius = "Select orbitRadius from spacecraftinfo where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(getOrbitRadius, cnn);
            reader = cmd.ExecuteReader();
            string payLoadName = "";
            while (reader.Read())
            {
                orbitRadius = reader.GetDouble(0);
            }
            reader.Close();
            cmd.Dispose();

            cnn.Close();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

        }

        public void timerTick(object sender, EventArgs e)
        {
            counter--;
            if(counter <= 0)
            {
                SqlConnection cnn;
                string connetionString;
                connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                cnn = new SqlConnection(connetionString);
                SqlCommand cmd;
                SqlDataReader reader;
                string spacecraftName = comboBoxSpacecrafts.SelectedItem.ToString();
                string payloadStatus = "NotDeployed";
                cnn.Open();
                string getPayloadName = "Select payloadName from spacecraftinfo where spacecraftName = '" + spacecraftName + "'";
                cmd = new SqlCommand(getPayloadName, cnn);
                reader = cmd.ExecuteReader();
                string payLoadName = "";
                while (reader.Read())
                {
                    payLoadName = reader.GetString(0);
                }
                reader.Close();
                cmd.Dispose();

                string getVehicleNameAlreadyInOrbit = "Select payloadName from vehicleInOrbit where spacecraftName = '" + spacecraftName + "'";
                cmd = new SqlCommand(getVehicleNameAlreadyInOrbit, cnn);
                reader = cmd.ExecuteReader();
                string vehicleNameAlreadyInOrbit = "";
                while (reader.Read())
                {
                    vehicleNameAlreadyInOrbit = reader.GetString(0);
                }
                reader.Close();
                cmd.Dispose();
                if (vehicleNameAlreadyInOrbit != payLoadName)
                {
                    string insertQuery = "Insert Into vehicleInOrbit (spacecraftName, payloadName, payLoadStatus) values('" + spacecraftName + "','" + payLoadName + "','" + payloadStatus + "')";
                    cmd = new SqlCommand(insertQuery, cnn);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = cmd;
                    adapter.InsertCommand.ExecuteNonQuery();
                    cnn.Close();

                }
                payloadLaunchBtn.IsEnabled = true;
                textTelemetry.Text = "[ORBIT REACHED FOR : " + spacecraftName + "]";
                timer.Stop();
            }
            else
            {
                Random rand = new Random();
                double altitude = rand.NextDouble();
                double longitude = rand.NextDouble() * Math.PI * 2;
                double latitude = Math.Acos(rand.NextDouble() * 2 - 1);
                double temperature = rand.Next();
                textTelemetry.Text = "{\n altitude : "+altitude+ "\n latitude : " + latitude + "\n longitude : " + longitude + "\n temperature : " + temperature + "\n timeToOrbit : " + counter + "\n}";

            }
        }
        private void payloadLaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string spacecraftName = comboBoxSpacecrafts.SelectedItem.ToString();
            string payloadStatus = "Deployed";
            cnn.Open();
            string updatePayloadStatus = "Update vehicleInOrbit Set payLoadStatus ='"+ payloadStatus + "' where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
            MessageBox.Show("PayLoad Deployed");

        }
    }
}
