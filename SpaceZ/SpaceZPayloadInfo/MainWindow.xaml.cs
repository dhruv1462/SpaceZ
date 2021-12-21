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

namespace SpaceZPayloadInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private string spaceCraftName = "";
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
            string selectQuery = "Select payloadName from vehicleInOrbit Where payLoadStatus = 'Deployed'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBoxPayLoad.Items.Add(reader.GetValue(0));
            }
            reader.Close();
            cnn.Close();
        }

        private void startDataButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string payloadType = "";
            string selectQuery = "Select payloadType, spaceCraftName, orbitRadius from spacecraftinfo Where payloadName = '"+ comboBoxPayLoad.SelectedItem.ToString() +"'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payloadType = reader.GetString(0);
                spaceCraftName = reader.GetString(1);
                orbitRadius = reader.GetDouble(2);

            }

            reader.Close();
            cnn.Close();

            if (payloadType.Equals("Communication")) 
            {
                timer.Tick += new EventHandler(timerTickCommunication);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();
            }
            else if (payloadType.Equals("Spy"))
            {
                timer.Tick += new EventHandler(timerTickSpy);
                timer.Interval = new TimeSpan(0, 0, 30);
                timer.Start();
            }
            else if (payloadType.Equals("Scientific"))
            {
                timer.Tick += new EventHandler(timerTickScientific);
                timer.Interval = new TimeSpan(0, 0, 5);
                timer.Start();
                
            }
        }
        public void timerTickScientific(object sender, EventArgs e) {
            Random rand = new Random();
            double rainfall = rand.NextDouble();
            double humidity = rand.NextDouble();
            double snow = rand.NextDouble();
            textData.Text = " Launch Vehicle Config\n Name: "+spaceCraftName+"\n Orbit: "+orbitRadius+"\n Payload Config\n Name: "+ comboBoxPayLoad.SelectedItem.ToString() + "\n Type: Scientific\n Weather Data\n Rainfall: " + rainfall+ " mm\n Humidity: " + humidity + " % snow: " + snow + " inch";
        }
        public void timerTickCommunication(object sender, EventArgs e) {
            Random rand = new Random();
            double uplink = rand.NextDouble();
            double downlink = rand.NextDouble();
            double transponder = rand.NextDouble();
            textData.Text = " Launch Vehicle Config\n Name: " + spaceCraftName + "\n Orbit: " + orbitRadius + "\n Payload Config\n Name: " + comboBoxPayLoad.SelectedItem.ToString() + "\n Type: Communication\n Communication Data\n Uplink: " + uplink+" MBps\n Downlink: "+downlink+" MBps% ActiveTransponder: "+transponder+" inch";
        }
        public void timerTickSpy(object sender, EventArgs e) {
            Random rand = new Random();
            double bytes = rand.NextDouble();
            textData.Text = " Launch Vehicle Config\n Name: " + spaceCraftName + "\n Orbit: " + orbitRadius + "\n Payload Config\n Name: " + comboBoxPayLoad.SelectedItem.ToString() + "\n Type: Spy\n Image Data\n Image: "+ bytes + " bytes ";
        }

        private void stopDataButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void startTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string selectQuery = "Select orbitRadius from spacecraftinfo Where payloadName = '" + comboBoxPayLoad.SelectedItem.ToString() + "'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orbitRadius = reader.GetDouble(0);
            }

            reader.Close();
            cnn.Close();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        public void timerTick(object sender, EventArgs e) {
            Random rand = new Random();
            double longitude = rand.NextDouble() * Math.PI * 2;
            double latitude = Math.Acos(rand.NextDouble() * 2 - 1);
            double temperature = rand.Next();
            textTelemetry.Text = "{\n altitude : " + orbitRadius + "\n latitude : " + latitude + "\n longitude : " + longitude + "\n temperature : " + temperature + "\n timeToOrbit : " + 0 + "\n}";

        }


        private void stopTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }

}
