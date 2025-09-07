using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class subscription_history : Form
    {
        public subscription_history()
        {
            InitializeComponent();
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            LoadHistoryByDate(DateTime.Today); // default load today
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadHistoryByDate(dateTimePicker1.Value);
        }

        private void LoadHistoryByDate(DateTime selectedDate)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    string query = @"SELECT subscription_id, depositor_name, duration_months, amount, 
                                            status, request_date, start_date, end_date
                                     FROM subscription
                                     WHERE CAST(request_date AS DATE) = @selectedDate
                                     ORDER BY request_date DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate.Date);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Subscription ID");
                    dt.Columns.Add("Depositor");
                    dt.Columns.Add("Duration (Months)");
                    dt.Columns.Add("Amount");
                    dt.Columns.Add("Status");
                    dt.Columns.Add("Request Date");
                    dt.Columns.Add("Start Date");
                    dt.Columns.Add("End Date");

                    while (reader.Read())
                    {
                        string status = string.IsNullOrEmpty(reader["status"].ToString()) ? "Pending" : reader["status"].ToString();

                        string startDate = reader["start_date"] == DBNull.Value
                            ? "Not set"
                            : Convert.ToDateTime(reader["start_date"]).ToShortDateString();

                        string endDate = reader["end_date"] == DBNull.Value
                            ? "Not set"
                            : Convert.ToDateTime(reader["end_date"]).ToShortDateString();

                        dt.Rows.Add(
                            reader["subscription_id"].ToString(),
                            reader["depositor_name"].ToString(),
                            reader["duration_months"].ToString(),
                            reader["amount"].ToString(),
                            status,
                            Convert.ToDateTime(reader["request_date"]).ToShortDateString(),
                            startDate,
                            endDate
                        );
                    }

                    dataGridView1.DataSource = dt;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading history: " + ex.Message);
            }
        }




        private void subscription_history_Load(object sender, EventArgs e)
        {

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_refresh_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT TOP 1 * FROM subscription ORDER BY subscription_id DESC"; // ✅ fixed column
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string status = reader["status"].ToString();
                        string startDate = reader["start_date"] == DBNull.Value ? "Not set" : Convert.ToDateTime(reader["start_date"]).ToShortDateString();
                        string endDate = reader["end_date"] == DBNull.Value ? "Not set" : Convert.ToDateTime(reader["end_date"]).ToShortDateString();

                        MessageBox.Show($"Status: {status}\nStart: {startDate}\nEnd: {endDate}");
                    }
                    else
                    {
                        MessageBox.Show("No subscription found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing: " + ex.Message);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            subscription_select subscription_Select = new subscription_select();
            subscription_Select.Show();
        }
    }
}
    
    

