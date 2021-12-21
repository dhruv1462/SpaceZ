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
            string selectQuery = "Select spacecraftName, payloadName  from spacecraftinfo";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBoxSpaceCraft.Items.Add(reader.GetValue(0));
                comboBoxPayload.Items.Add(reader.GetValue(1));
            }
            reader.Close();
            cnn.Close();
        }
        private void startDataButton_Click(object sender, RoutedEventArgs e)
        {
           Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Intel Internship\\Project\\SpaceZ\\SpaceZ\\SpaceZPayloadInfo\\bin\\Debug\\SpaceZPayloadInfo.exe");
            p.Start();
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand cmd;
            SqlDataReader reader;
            string payloadType = "";
            string selectQuery = "Select payloadType, spaceCraftName, orbitRadius from spacecraftinfo Where payloadName = '" + payloadName + "'";
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
            p.StartInfo = new ProcessStartInfo("C:\\Intel Internship\\Project\\SpaceZ\\SpaceZ\\SpaceZ\\bin\\Debug\\SpaceZ.exe");
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
    }
}
