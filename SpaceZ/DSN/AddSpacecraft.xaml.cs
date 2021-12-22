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
using System.Text.RegularExpressions;

namespace DSN
{
    public partial class AddSpacecraft : Window
    {
        SqlConnection cnn;
        // Change the connection string of the database
        string connetionString = "Data Source=DESKTOP-IKE2NLC;Database=spacez;Integrated Security = True";
        SqlCommand cmd;
        SqlDataAdapter adapter = new SqlDataAdapter();
        public AddSpacecraft()
        {
            // All the componentes are intialized 
            InitializeComponent();
            payloadCombo.Items.Add("Scientific");
            payloadCombo.Items.Add("Communication");
            payloadCombo.Items.Add("Spy");
        }

        // This button add the new spacecraft details  
        private void btnAddSpacecraft_Click(object sender, RoutedEventArgs e)
        {
            cnn = new SqlConnection(connetionString);

            if (orbitRadiusTxt.Text != "" && spacecraftNameTxt.Text != "" && payloadNameTxt.Text != "" && payloadCombo.SelectedItem.ToString() != "")
            {
                Regex rx = new Regex(@"^([1-9]\d*(\.)\d*|0?(\.)\d*[1-9]\d*|[1-9]\d*)$");
                string text = "12.0";
                var result = rx.IsMatch(text);
                Console.WriteLine(result);
                if (rx.IsMatch(orbitRadiusTxt.Text))
                {
                    try
                    {
                        string spacecraftName = spacecraftNameTxt.Text;
                        string orbitRadius = orbitRadiusTxt.Text;
                        string payloadName = payloadNameTxt.Text;
                        string payloadType = payloadCombo.SelectedItem.ToString();
                        string launchStatus = "inactive";
                        cnn.Open();
                        string insertQuery = "Insert Into spacecraftinfo (spacecraftName, orbitRadius, payloadName, payloadType, launchStatus) values('" + spacecraftName + "','" + orbitRadius + "','" + payloadName + "','" + payloadType + "', '" + launchStatus + "')";
                        cmd = new SqlCommand(insertQuery, cnn);
                        adapter.InsertCommand = cmd;
                        adapter.InsertCommand.ExecuteNonQuery();
                        cnn.Close();
                        MessageBox.Show("Aircraft Added.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Duplicate Entry Of Spacecraft Or Payload Name ");
                    }
                }
                else
                {
                    MessageBox.Show("Orbit Radius Should be in numbers.");
                }
            }
            else
            {
                MessageBox.Show("Fill all the data.");
            }
            cnn.Close();
        }

        // Go back to mission control window
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem();
            missionControlSystem.Show();
            this.Close();
        }
    }
}
