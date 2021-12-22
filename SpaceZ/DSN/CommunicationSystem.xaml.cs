using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Windows.Threading;

namespace DSN
{
    /// <summary>
    /// Interaction logic for CommunicationSystem.xaml
    /// </summary>
    /// [DataContract]
    
    // Interface defined for telemetry service inter process communication using wcf architecture
    [ServiceContract]
    public interface ITelemetryService
    {
        [OperationContract]
        string getTelemetry(string spacecraftName);
    }

    // Interface defined for payloadData service inter process communication using wcf architecture

    [ServiceContract]
    public interface IPayloadDataService
    {
        [OperationContract]
        string getPayLoadInfo(string payloadName);
    }

    // All the values and componenets are Initialized
    public partial class CommunicationSystem : Window
    {
        private DispatcherTimer timerTelemetry = new DispatcherTimer();
        private DispatcherTimer timerPayload = new DispatcherTimer();
        SqlConnection cnn;
        string connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;;Integrated Security = True";
        SqlCommand cmd;
        SqlDataReader reader;
        public CommunicationSystem()
        {
            
            InitializeComponent();
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            string selectQuery = "Select spacecraftName from spacecraftinfo where launchStatus = 'active'";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBoxSpaceCraft.Items.Add(reader.GetValue(0));
            }

            reader.Close();

            string updatePayloadStatus = "Select payloadName from vehicleInOrbit";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listOfPayLoad.Items.Add(reader.GetValue(0));
            }
            reader.Close();

            string selectPayloadName = "Select payloadName from vehicleInOrbit Where payLoadStatus = 'Deployed'";
            cmd = new SqlCommand(selectPayloadName, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBoxPayload.Items.Add(reader.GetValue(0));
            }
            
            reader.Close();
            
            launchPayloadbtn.IsEnabled = false;
        }


        // Get the data from payload process
        private void startDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxPayload.SelectedItem != null)
            {
                Process p = new Process();
                // Change the path of ur file accordingly
                p.StartInfo = new ProcessStartInfo("C:\\Users\\dpatel81\\Desktop\\Intel\\SpaceZ\\SpaceZ\\SpaceZPayloadInfo\\bin\\Debug\\SpaceZPayloadInfo.exe");
                p.Start();
                cnn = new SqlConnection(connetionString);
                cnn.Open();
                string payloadType = "";
                string selectQuery = "Select payloadType from spacecraftinfo Where payloadName = '" + comboBoxPayload.SelectedItem.ToString() + "'";
                cmd = new SqlCommand(selectQuery, cnn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    payloadType = reader.GetString(0);

                }
                if (payloadType == "Scientific")
                {
                    timerPayload.Tick += new EventHandler(payloadData);
                    timerPayload.Interval = new TimeSpan(0, 0, 5);
                    timerPayload.Start();
                }
                else if (payloadType == "Communication")
                {
                    timerPayload.Tick += new EventHandler(payloadData);
                    timerPayload.Interval = new TimeSpan(0, 0, 10);
                    timerPayload.Start();
                }
                else if (payloadType == "Spy")
                {
                    timerPayload.Tick += new EventHandler(payloadData);
                    timerPayload.Interval = new TimeSpan(0, 0, 30);
                    timerPayload.Start();
                }
                
            }
            else
            {
                MessageBox.Show("Select the payload for you want the data");
            }
        }

        // tcp connection established to get data for payload
        private void payloadData(object sender, EventArgs e)
        {
            try
            {
                string uri = "net.tcp://localhost:8080/MainWindow";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                var channel = new ChannelFactory<IPayloadDataService>(binding);
                var endpoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endpoint);
                var result = proxy?.getPayLoadInfo(comboBoxPayload.SelectedItem.ToString());
                if (result != null)
                {
                    Console.WriteLine(result);
                    payloadDataText.Text = result;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payload Information Process Was Closed ");
                timerPayload.Stop();
                payloadDataText.Clear();
            }
        }

        // Get telemetry data
        private void startTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (comboBoxSpaceCraft.SelectedItem != null)
            { 
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Users\\dpatel81\\Desktop\\Intel\\SpaceZ\\SpaceZ\\SpaceZ\\bin\\Debug\\SpaceZ.exe");
            p.Start();
           
            timerTelemetry.Tick += new EventHandler(telemeteryInfo);
            timerTelemetry.Interval = new TimeSpan(0, 0, 1);
            timerTelemetry.Start();
                
            }
            else
            {
                MessageBox.Show("Select the spacecraft you want to get telemetry data");
            }
        }

        // tcp connection established to get telemetry data
        private void telemeteryInfo(object sender, EventArgs e)
        {
            try
            {
                string uri = "net.tcp://localhost:6565/MainWindow";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                var channel = new ChannelFactory<ITelemetryService>(binding);
                var endpoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endpoint);
                var result = proxy?.getTelemetry(comboBoxSpaceCraft.SelectedItem.ToString());
                if (result != null)
                {
                    Console.WriteLine(result);
                    textTelemetry.Text = result;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Telemetry Information Was Closed");
                timerTelemetry.Stop();
                textTelemetry.Clear();
            }
        }

        // stop telemetry data
        private void stopTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            timerTelemetry.Stop();
            textTelemetry.Clear();
            foreach (var process in Process.GetProcessesByName("SpaceZ"))
            {
                process.Kill();
            }

        }

        
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // stop payload data
        private void stopDataButton_Click(object sender, RoutedEventArgs e)
        {
            timerPayload.Stop();
            payloadDataText.Clear();
            foreach (var process in Process.GetProcessesByName("SpaceZPayloadInfo"))
            {
                process.Kill();
            }
        }

        // Launch Payload when spacecraft is in orbit
        private void launchPayloadbtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string spacecraftName = comboBoxSpaceCraft.SelectedItem.ToString();
            string payloadStatus = "Deployed";
            cnn.Open();
            string updatePayloadStatus = "Update vehicleInOrbit Set payLoadStatus ='" + payloadStatus + "' where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
            launchPayloadbtn.IsEnabled = false;
        }

       // enabling the launch button if payload not yet launched
        private void comboBoxSpaceCraft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            string spacecraftName = comboBoxSpaceCraft.SelectedItem.ToString();
            string payloadStatus = "Deployed";
            cnn.Open();
            string updatePayloadStatus = "Select payloadName from vehicleInOrbit where payLoadStatus ='"+payloadStatus+"' and spacecraftName = '"+spacecraftName+"'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            
            if(reader.Read() == false)
            {
                launchPayloadbtn.IsEnabled= true;
            }
            else
            {
                launchPayloadbtn.IsEnabled = false;
            }
            cnn.Close();

        }

        private void refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show();
            this.Close();
        }

        // get details of payload and payload type
        private void listOfPayLoad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            string payloadName = listOfPayLoad.SelectedItem.ToString();
            cnn.Open();
            string updatePayloadStatus = "Select * from spacecraftinfo where payloadName ='" + payloadName + "'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string information = "\nSpacecraft Name: " + reader.GetString(0) + "\nPayload Name: " + reader.GetString(2) + "\nPayload Type: " + reader.GetString(3);
                MessageBox.Show(information,reader.GetString(2));
            }
        }

        // de-orbit spacecraft
        private void deOrbitButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string spacecraftName = comboBoxSpaceCraft.SelectedItem.ToString();
            string launchStatus = "DeOrbit";
            cnn.Open();
            string updatePayloadStatus = "Update spacecraftInfo Set launchStatus ='" + launchStatus + "' where spacecraftName = '" + spacecraftName + "'";
            cmd = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand = new SqlCommand(updatePayloadStatus, cnn);
            adapter.UpdateCommand.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
            launchPayloadbtn.IsEnabled = false;
        }
    }
}
