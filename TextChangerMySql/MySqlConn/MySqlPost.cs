using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace TextChangerMySql.MySqlConn
{
    public class MySqlPost
    {
        private List<PostData> messages;

        public void Worker()
        {
            GetData();
            SaveToFile();
            UpdateData();
            SaveData();
        }

        private void SaveData()
        {
            var dbCon = MySqlConnector.Instance();
            
            dbCon.Connection.Close();

            foreach (PostData postD in messages)
            {
            
                if (dbCon.IsConnect())
                {
                    MySqlCommand command = dbCon.Connection.CreateCommand();
                    command.CommandText = $"UPDATE XF_POST SET message = @data WHERE Post_Id =@id";
                    command.Parameters.AddWithValue("@data", postD.Message);
                    command.Parameters.AddWithValue("@id", postD.PostId);

                    command.ExecuteNonQuery();

                }
                //dbCon.Connection.Close();

            }
        }

        private void UpdateData()
        {
            Regex rg = new Regex(@"\[img\]http:\/\/img\.photobucket\.com.+\[\/img\]");
            foreach (PostData postData in messages)
            {
                postData.Message = rg.Replace(postData.Message, string.Empty).Trim();
            }
        }

        private void GetData()
        {
            var dbCon = MySqlConnector.Instance();
            if (dbCon.IsConnect())
            {
                //suppose col0 and col1 are defined as VARCHAR in the DB
                string query = @"SELECT post_id,     message FROM xf_post where message like '%[img]http://test.suineg.org%';";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                messages = ConvertData(reader).ToList();
            }
        }

        private IEnumerable<PostData> ConvertData(MySqlDataReader reader)
        {
            while (reader.Read())
            {
                yield return new PostData
                {
                    PostId = reader.GetInt32(0),
                    Message = reader.GetString(1)
                };
            }
        }

        private void SaveToFile()
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter($"fileBackup_{DateTime.UtcNow.Ticks}.log"))
            {
                string json = JsonConvert.SerializeObject(messages);

                file.Write(json);
                file.Close();
            }


        }
    }
    [Serializable]
    public class PostData
    {
        public int PostId
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{PostId}:{Message}";
        }
    }
}

