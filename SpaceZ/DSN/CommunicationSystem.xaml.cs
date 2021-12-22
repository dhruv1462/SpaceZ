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
    
    [ServiceContract]
    public interface ITelemetryService
    {
        [OperationContract]
        string getTelemetry(string spacecraftName);
    }

   [ServiceContract]
    public interface IPayloadDataService
    {
        [OperationContract]
        string getPayLoadInfo(string payloadName);
    }

    public partial class CommunicationSystem : Window
    {
        private DispatcherTimer timerTelemetry = new DispatcherTimer();
        private DispatcherTimer timerPayload = new DispatcherTimer();
        SqlConnection cnn;
        string connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;;Integrated Security = True";
        SqlCommand cmd;
        SqlDataReader reader;
        public CommunicationSystem()
        {
            
            InitializeComponent();
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            string selectQuery = "Select spacecraftName from timeToOrbit";
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


        private void startDataButton_Click(object sender, RoutedEventArgs e)
        {
           Process p = new Process();
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
            timerPayload.Tick += new EventHandler(payloadData);
            timerPayload.Interval = new TimeSpan(0, 0, 1);
            timerPayload.Start();
        }

        private void payloadData(object sender, EventArgs e)
        {
            string uri = "net.tcp://localhost:6565/MainWindow";
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
       
        private void startTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Users\\dpatel81\\Desktop\\Intel\\SpaceZ\\SpaceZ\\SpaceZ\\bin\\Debug\\SpaceZ.exe");
            p.Start();
            timerTelemetry.Tick += new EventHandler(telemeteryInfo);
            timerTelemetry.Interval = new TimeSpan(0, 0, 1);
            timerTelemetry.Start();
        }

        private void telemeteryInfo(object sender, EventArgs e)
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

        private void stopDataButton_Click(object sender, RoutedEventArgs e)
        {
            timerPayload.Stop();
            payloadDataText.Clear();
            foreach (var process in Process.GetProcessesByName("SpaceZPayloadInfo"))
            {
                process.Kill();
            }
        }

        private void launchPayloadbtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
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

        private void comboBoxSpaceCraft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
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

        private void listOfPayLoad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spaceg;Integrated Security = True";
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
    }
}
