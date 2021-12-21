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
        [OperationContract]
        [FaultContract(typeof(Telemetry))]
        Telemetry getTelemetry();
        [OperationContract]
        string getTelmetry(string spacecraftName);
    }

    public partial class CommunicationSystem : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
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

        }

        private void startTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Intel Internship\\Project\\SpaceZ\\SpaceZ\\SpaceZ\\bin\\Debug\\SpaceZ.exe");
            p.Start();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            string uri = "net.tcp://localhost:6565/MainWindow";
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IMessageService>(binding);
            var endpoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endpoint);
            var result = proxy?.getTelmetry(comboBoxSpaceCraft.SelectedItem.ToString());
            if (result != null)
            {
                Console.WriteLine(result);
                textTelemetry.Text = result;

            }
        }

        private void stopTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            textTelemetry.Clear();
        }
    }
}
