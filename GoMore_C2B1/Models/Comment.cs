using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace GoMore_C2B1.Models
{
    public class CommentModel
    {
        public string ModelName { get; set; }
        public string CommentUser { get; set; }
        public string CommentUserEmail { get; set; }
        public string CommentMsg { get; set; }
        public string CommentTime { get; set; }

    }
    public class Comment
    {


        public void UploadComment(CommentModel C_Info, string host, string port, string database, string Username, string password)
        {

            string DBconfig = "Host = " + host + ";Port = " + port + ";Database = " + database + ";Username = " + Username + ";Password = " + password + ";";
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Comment_List(ModelName,CommentUser,CommentUserEmail,CommentTime,CommentMsg) VALUES (:ModelName, :CommentUser, :CommentUserEmail, :CommentTime, :CommentMsg)";
            cmd.Parameters.Add(new NpgsqlParameter("ModelName", C_Info.ModelName));
            cmd.Parameters.Add(new NpgsqlParameter("CommentUser", C_Info.CommentUser));
            cmd.Parameters.Add(new NpgsqlParameter("CommentUserEmail", C_Info.CommentUserEmail));
            cmd.Parameters.Add(new NpgsqlParameter("CommentMsg", C_Info.CommentMsg));
            cmd.Parameters.Add(new NpgsqlParameter("CommentTime", C_Info.CommentTime));
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public List<CommentModel> GetComment(CommentModel C_Info, string host, string port, string database, string Username, string password)
        {
            string DBconfig = "Host = " + host + ";Port = " + port + ";Database = " + database + ";Username = " + Username + ";Password = " + password + ";";
            NpgsqlConnection conn = new NpgsqlConnection(DBconfig);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Comment_List where ModelName ='" + C_Info.ModelName + "' ";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            List<CommentModel> Comments = new List<CommentModel>();
            while (reader.Read())
            {
                var comment = new CommentModel();
                comment.CommentUser = reader["CommentUser"].ToString();
                comment.CommentTime = reader["CommentTime"].ToString();
                comment.CommentMsg = reader["CommentMsg"].ToString();
                comment.CommentUserEmail = reader["CommentUserEmail"].ToString();
                Comments.Add(comment);

            }
            conn.Close();

            return Comments;
        }
    }
}