using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SpaceZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double counter = 0;
        private double orbitRadius = 0;
        private DispatcherTimer timer = new DispatcherTimer();

        [DataContract]
        public class Telemetry
        {
            private string spacecraftName;
            private double altitude;
            private double latitude;
            private double longitude;
            private double timeToOrbit;
            private double temperature;
            private double counter;

            [DataMember]
            public string SpacecraftName { get { return spacecraftName; } set { spacecraftName = value; } }
            [DataMember]
            public double Altitude { get { return altitude; } set { altitude = value; } }
            [DataMember]
            public double Latitude { get { return latitude; } set { latitude = value; } }
            [DataMember]
            public double Longitude { get { return longitude; } set { longitude = value; } }
            [DataMember]
            public double TimeToOrbit { get { return timeToOrbit; } set { timeToOrbit = value; } }
            [DataMember]
            public double Temperature { get { return temperature; } set { temperature = value; } }
            [DataMember]
            public double Counter { get { return counter; } set { counter = value; } }

        }

        [ServiceContract]
        public interface IMessageService
        {

            /* [OperationContract]
             [FaultContract(typeof(Telemetry))]
             Telemetry getTelemetry();*/
            [OperationContract]
            string getTelmetry();
         }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class MessageService : IMessageService
        {

            /* public Telemetry getTelemetry()
             {
                 Telemetry data = new Telemetry();
                 data.SpacecraftName = "efvefv";
                 data.Temperature = 78;
                 data.Altitude = 88;
                 data.Longitude = 55;
                 data.Latitude = 98;
                 data.TimeToOrbit = 67;
                 data.Counter = 56;
                 return data;        
             } */
            public string getTelmetry()
            {
                string abc = "wassup";
                return(abc + "hiiii from spaceZ");
            }
        }

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

            
            var uris = new Uri[1];
            string address = "net.tcp://localhost:6565/MessageService";
            uris[0] = new Uri(address);
            IMessageService message = new MessageService();
            ServiceHost host = new ServiceHost(message, uris);
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IMessageService), binding, "");
            host.Opened += Host_Opened;
            host.Open(); 

        }

        private void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("message service started");
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
            if (counter <= 0)
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
                textTelemetry.Text = "{\n altitude : " + altitude + "\n latitude : " + latitude + "\n longitude : " + longitude + "\n temperature : " + temperature + "\n timeToOrbit : " + counter + "\n}";

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
            string updatePayloadStatus = "Update vehicleInOrbit Set payLoadStatus ='" + payloadStatus + "' where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Intel Internship\\Project\\SpaceZ\\SpaceZ\\SpaceZPayloadInfo\\bin\\Debug\\SpaceZPayloadInfo.exe");
            p.Start();
            MessageBox.Show("PayLoad Deployed");
        }
    }
}