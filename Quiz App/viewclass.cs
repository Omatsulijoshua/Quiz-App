using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_App
{
    class viewclass
    {


        private string conn_string = ConfigurationManager.ConnectionStrings["quiz"].ConnectionString;

        string query;

        public viewclass(string q)
        {
            this.query = q;

        }

        public DataTable showrecord()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(conn_string);
            SqlCommand cmd = new SqlCommand(query, conn);


            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("No Record Found............");
            }

            finally
            {
                conn.Close();
            }

            return dt;

        }


    }
}
