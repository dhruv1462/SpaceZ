using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for AddSpacecraft.xaml
    /// </summary>
    public partial class AddSpacecraft : Window
    {
        public AddSpacecraft()
        {
            InitializeComponent();
            payloadCombo.Items.Add("Scientific");
            payloadCombo.Items.Add("Communication");
            payloadCombo.Items.Add("Spy");
        }

        private void btnAddSpacecraft_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cnn;
            string connetionString;
            connetionString = @"Server=tcp:spacez.database.windows.net,1433;Initial Catalog=SpaceZ;Persist Security Info=False;User ID=dpatel81;Password=Dilip_1462!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cnn = new SqlConnection(connetionString);
            SqlCommand cmd;
            SqlDataReader reader;
            /*
            cnn.Open();
            string result = "";
            string selectQuery = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'spacecraftinfo')) BEGIN PRINT 'Database Table Exists' END; ELSE BEGIN PRINT 'No Table in database' END; ";
            cmd = new SqlCommand(selectQuery, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               result = reader.GetString(0);
            }
            MessageBox.Show(result);
            cnn.Close();
            if (result.Equals("No Table in database"))
            {
                cnn.Open();
                string createQuery = "Create Table spacecraftinfo (spacecraftName varchar(255) PRIMARY KEY, orbitRadius varchar(255), payloadName varchar(255), playloadType varchar(255))";
                cmd = new SqlCommand(createQuery, cnn);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            */
            if (orbitRadiusTxt.Text != "" && spacecraftNameTxt.Text !="" && payloadNameTxt.Text !="" && payloadCombo.SelectedItem.ToString() != "")
            {
                string spacecraftName = spacecraftNameTxt.Text;
                string orbitRadius = orbitRadiusTxt.Text;
                string payloadName = payloadNameTxt.Text;
                string playloadType = payloadCombo.SelectedItem.ToString();
                string launchStatus = "inactive";
                cnn.Open();
                string insertQuery = "Insert Into spacecraftinfo (spacecraftName, orbitRadius, payloadName, playloadType, launchStatus) values('" + spacecraftName+"','"+orbitRadius + "','" +payloadName + "','" +playloadType+"', '"+ launchStatus +"')";
                cmd = new SqlCommand(insertQuery, cnn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                adapter.InsertCommand.ExecuteNonQuery();
                cnn.Close();
                MessageBox.Show("Aircraft Added.");
            }
            cnn.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem();
            missionControlSystem.Show();
            this.Close();
        }
    }
}
