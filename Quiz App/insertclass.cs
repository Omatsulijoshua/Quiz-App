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
    class insertclass
    {
        private string conn_string = ConfigurationManager.ConnectionStrings["quiz"].ConnectionString;

        // inserting questions...

        public string insert_records(ques_id q)


        // inserting questions...


        {
            string msg = "";

            SqlConnection conn = new SqlConnection(conn_string);

            try
            {
                SqlCommand cmd = new SqlCommand("insert_questions", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@q_title", SqlDbType.NVarChar, 200).Value = q.q_title;
                cmd.Parameters.Add("@q_opA", SqlDbType.NVarChar, 200).Value = q.q_opA;
                cmd.Parameters.Add("@q_opB", SqlDbType.NVarChar, 200).Value = q.q_opB;
                cmd.Parameters.Add("@q_opC", SqlDbType.NVarChar, 200).Value = q.q_opC;
                cmd.Parameters.Add("@q_opD", SqlDbType.NVarChar, 200).Value = q.q_opD;
                cmd.Parameters.Add("@q_correctOpn", SqlDbType.NVarChar, 200).Value = q.q_correctOpn;

                cmd.Parameters.Add("@q_correctDate", SqlDbType.NVarChar, 200).Value = q.q_correctDate;
                cmd.Parameters.Add("@ad_id_fk", SqlDbType.Int).Value = q.ad_id_fk;
                cmd.Parameters.Add("@ex_id_fk", SqlDbType.Int).Value = q.ex_id_fk;


                conn.Open();

                cmd.ExecuteNonQuery();

                msg = "Data successfully inserted";

            }
             catch (Exception)
            {

                msg = "Data is not successfully inserted";
            }


            finally
            {
                conn.Close();
            }

            return msg;
        }


        public string insert_setexam(string date, string stid, string exid)


        // inserting questions...


        {
            string msg = "";

            SqlConnection conn = new SqlConnection(conn_string);

            try
            {
                SqlCommand cmd = new SqlCommand("insert_set_exam", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@set_exam_date", SqlDbType.NVarChar, 200).Value = date;
                cmd.Parameters.Add("stud_id_fk", SqlDbType.Int, 200).Value = stid;
                cmd.Parameters.Add("@exam_id_fk", SqlDbType.Int, 200).Value = exid;


                conn.Open();

                cmd.ExecuteNonQuery();

                msg = "Data successfully inserted";

            }
            catch (Exception)
            {

                msg = "Data is not successfully inserted";
            }


            finally
            {
                conn.Close();
            }

            return msg;




        }

        public string insert_score(string score, string stid, string exid, string per)
        {
            string msg = "";
            SqlConnection conn = new SqlConnection(conn_string);

            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO score (score, percentage, stud_fk_id, exam_fk_id) VALUES (@score, @percentage, @studId, @examId)", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@score", Convert.ToInt32(score));
                cmd.Parameters.AddWithValue("@percentage", Convert.ToDouble(per));
                cmd.Parameters.AddWithValue("@studId", Convert.ToInt32(stid));
                cmd.Parameters.AddWithValue("@examId", Convert.ToInt32(exid));

                conn.Open();
                cmd.ExecuteNonQuery();
                msg = "Score successfully inserted";
            }
            catch (Exception ex)
            {
                msg = "Score is not successfully inserted: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return msg;
        }




        public void nonquery(string query)
        {
            SqlConnection con = connection_class.GetConnection();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void InsertExamDuration(int examId, int duration)
        {
            string query = $"INSERT INTO tbl_exam_settings (ex_id, duration_minutes) VALUES ({examId}, {duration})";
            nonquery(query); // assuming this executes a non-query
        }

        public void UpsertExamDuration(int examId, int duration)
        {
            string query = $@"
        IF EXISTS (SELECT 1 FROM tbl_exam_settings WHERE ex_id = {examId})
            UPDATE tbl_exam_settings SET duration_minutes = {duration} WHERE ex_id = {examId}
        ELSE
            INSERT INTO tbl_exam_settings (ex_id, duration_minutes) VALUES ({examId}, {duration})";
            nonquery(query);
        }



        public void UpsertTheoryDuration(int examId, int theoryDuration)
        {
            using (SqlConnection con = connection_class.GetConnection())
            {
                con.Open();

                string theoryColumn = ResolveTheoryDurationColumn(con);
                if (string.IsNullOrWhiteSpace(theoryColumn))
                {
                    using (SqlCommand ensureCmd = new SqlCommand(
                        "IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_duration_minutes') IS NULL ALTER TABLE dbo.tbl_exam_settings ADD theory_duration_minutes INT NULL;",
                        con))
                    {
                        ensureCmd.ExecuteNonQuery();
                    }

                    theoryColumn = ResolveTheoryDurationColumn(con);
                    if (string.IsNullOrWhiteSpace(theoryColumn))
                    {
                        throw new InvalidOperationException("Theory duration column was not found in tbl_exam_settings.");
                    }
                }

                string query = @"
        IF EXISTS (SELECT 1 FROM tbl_exam_settings WHERE ex_id = @examId)
            UPDATE tbl_exam_settings
            SET " + theoryColumn + @" = @theoryDuration
            WHERE ex_id = @examId;
        ELSE
            INSERT INTO tbl_exam_settings (ex_id, " + theoryColumn + @")
            VALUES (@examId, @theoryDuration);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@examId", examId);
                    cmd.Parameters.AddWithValue("@theoryDuration", theoryDuration);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string ResolveTheoryDurationColumn(SqlConnection connection)
        {
            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration_minutes"))
            {
                return "theory_duration_minutes";
            }

            if (ColumnExists(connection, "tbl_exam_settings", "theory_duration"))
            {
                return "theory_duration";
            }

            return null;
        }

        private bool ColumnExists(SqlConnection connection, string tableName, string columnName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column",
                connection))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }



    }







}
        
    







