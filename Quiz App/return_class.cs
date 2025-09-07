using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;


namespace Quiz_App
{
    class return_class
    {


        private string conn_string = ConfigurationManager.ConnectionStrings["quiz"].ConnectionString;


        public string scalerReturn(string q)
        {
            string s;

            SqlConnection conn = connection_class.GetConnection();
            conn.Open();

            try
            {


                SqlCommand cmd = new SqlCommand(q, conn);
                s = cmd.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                s = "";
                //throw;
            }




            return s;
        }

        public DataTable tableReturn(string query)
        {
            DataTable dt = new DataTable();
            SqlConnection con = connection_class.GetConnection();
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }





        public int GetExamDuration(int examId)
        {
            int duration = 0;
            string query = "SELECT duration_minutes FROM tbl_exam_settings WHERE ex_id = @examId";

            SqlConnection con = connection_class.GetConnection();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@examId", examId);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    duration = Convert.ToInt32(result);
            }
            return duration;
        }

        public DataTable GetDataTable(string query)
        {
            SqlConnection con = connection_class.GetConnection();
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void execQuery(string query)
        {
            SqlConnection conn = connection_class.GetConnection();
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SetExamTotalQuestions(int examId, int totalQuestions)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM tbl_exam_settings WHERE ex_id = @examId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@examId", examId);
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    string updateQuery = "UPDATE tbl_exam_settings SET total_questions = @total WHERE ex_id = @examId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@total", totalQuestions);
                    updateCmd.Parameters.AddWithValue("@examId", examId);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = "INSERT INTO tbl_exam_settings (ex_id, total_questions) VALUES (@examId, @total)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@examId", examId);
                    insertCmd.Parameters.AddWithValue("@total", totalQuestions);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

    }
}
