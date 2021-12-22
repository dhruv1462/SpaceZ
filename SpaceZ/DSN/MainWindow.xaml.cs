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

namespace DSN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {

        SqlConnection cnn;
        // If you are running on ur system add ur connection string to server
        string connetionString = "Data Source=DESKTOP-IKE2NLC;Integrated Security = True";
        public MainWindow()
        {
            InitializeComponent();
            bool check = CheckDatabaseExists("spacez");
            Console.WriteLine(check);
            if (check == false)
            {
                string createDatabase = "Create Database spacez";
                cnn = new SqlConnection(connetionString);
                SqlCommand myCommand = new SqlCommand(createDatabase, cnn);
                cnn.Open();
                myCommand.ExecuteNonQuery();
               
                // Add your connection string with the name of database as spacez
                string connetionStringdatabase = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
                SqlConnection connetionConnection = new SqlConnection(connetionStringdatabase);
                connetionConnection.Open();
                string createQuery = "Create Table spacecraftinfo (spacecraftName varchar(255) PRIMARY KEY, orbitRadius float(50), payloadName varchar(255) UNIQUE, payloadType varchar(255), launchStatus varchar(255));" +
                                     "Create Table timeToOrbit (spacecraftName varchar(255) PRIMARY KEY, timespan float(50),  FOREIGN KEY (spacecraftName) REFERENCES spacecraftinfo(spacecraftName));" +
                                     "Create Table vehicleInOrbit (spacecraftName varchar(255) PRIMARY KEY, payloadName varchar(255) UNIQUE, payLoadStatus varchar(255));";

                SqlCommand connetionCommand = new SqlCommand(createQuery, connetionConnection);
                connetionCommand.ExecuteNonQuery();
                connetionConnection.Close();

            }
        }

        // This button will redirect you to Communication System Window
        private void communicationSystemBtn_Click(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show(); 
            this.Close();
        }

        // This button will redirect you to Mission Control Window
        private void missionControlSystemBtn_Click(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); 
            missionControlSystem.Show(); 
            this.Close();
        }

        // Function to check if database exist or not
        private static bool CheckDatabaseExists( string databaseName)
        {
            SqlConnection tmpConn;
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                tmpConn = new SqlConnection("Data Source=DESKTOP-IKE2NLC;Integrated Security = True");

                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
        
        using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
