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
    public partial class make_subscription : BaseForm
    {
        private const decimal PricePerMonth = 1000m; // ?1000 per month
       
        public make_subscription()
        {
            InitializeComponent();

            // Add durations to ComboBox
            cmbDuration.Items.AddRange(new object[]
            {
                "1 Month", "2 Months", "3 Months", "6 Months", "12 Months (1 Year)", "24 Months (2 Years)"
            });

            // Make sure amount label starts blank
            lblAmount.Text = "?0";



        }


        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;

        public static void ScaleForm(Form form)
        {
            // Get current screen resolution
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate scale factors
            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;

            // Apply scaling to form and controls
            form.Scale(new SizeF(scaleX, scaleY));

            // Adjust font scaling (optional, but makes UI balanced)
            foreach (Control c in form.Controls)
            {
                c.Font = new Font(c.Font.FontFamily, c.Font.Size * Math.Min(scaleX, scaleY));
            }

            // Center form
            form.StartPosition = FormStartPosition.CenterScreen;
        }
        private void cmbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }



        private int GetMonthsFromSelection(string selection)
        {
            switch (selection)
            {
                case "1 Month": return 1;
                case "2 Months": return 2;
                case "3 Months": return 3;
                case "6 Months": return 6;
                case "12 Months (1 Year)": return 12;
                case "24 Months (2 Years)": return 24;
                default: return 1;
            }
        }

        private void make_subscription_Load(object sender, EventArgs e)
        {
            make_subscription.ScaleForm(this);
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (cmbDuration.SelectedItem == null || string.IsNullOrWhiteSpace(txtDepositor.Text))
            {
                MessageBox.Show("Please select duration and enter depositor name.");
                return;
            }

            int months = GetMonthsFromSelection(cmbDuration.SelectedItem.ToString());
            decimal amount = months * PricePerMonth;
            string depositor = txtDepositor.Text.Trim();

            using (SqlConnection conn = connection_class.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO subscription (depositor_name, duration_months, amount, status) VALUES (@name, @months, @amount, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", depositor);
                cmd.Parameters.AddWithValue("@months", months);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Subscription request submitted.\nStatus: Pending.\nCheck subscription history for updates.");
        
        }

        private void btnCheckStatus_Click(object sender, EventArgs e)
        {
            subscription_history subscription_History = new subscription_history();
            subscription_History.Show();
            //this.Hide();
        }

        private void cmbDuration_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbDuration.SelectedItem != null)
            {
                int months = GetMonthsFromSelection(cmbDuration.SelectedItem.ToString());
                decimal totalAmount = months * PricePerMonth;
                lblAmount.Text = "?" + totalAmount.ToString("N0");
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            subscription_select subscription_Select = new subscription_select();
            subscription_Select.Show();
        }
    }
}

