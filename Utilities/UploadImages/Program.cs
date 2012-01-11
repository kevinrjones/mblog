using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace UploadImages
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> images = new Dictionary<string, string>();
            images.Add("TeamCity", "../../Images/TeamCity.png");
            images.Add("InitialBlog", "../../Images/InitialBlog.png");

            foreach (var image in images)
            {
                FileStream str = File.Open(image.Value, FileMode.Open);

                byte[] imageData = new byte[str.Length];

                str.Read(imageData, 0, imageData.Length);

                using (
                    var connection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString))
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        cmd.CommandText =
                            "INSERT INTO [media]([title],[file_name]," +
                            "[year], [month], [day],[mime_type],[alignment],[size],[user_id],[bytes])" +
                            "VALUES(@title, @file_name,  @year,  @month,  @day, @mime_type, @alignment, @size, @user_id, @bytes)";
                        cmd.Parameters.AddWithValue("@title", image.Key);
                        cmd.Parameters.AddWithValue("@file_name", image.Key);
                        cmd.Parameters.AddWithValue("@year", 2012);
                        cmd.Parameters.AddWithValue("@month", 12);
                        cmd.Parameters.AddWithValue("@day", 18);
                        cmd.Parameters.AddWithValue("@mime_type", "image/png");
                        cmd.Parameters.AddWithValue("@alignment", 1);
                        cmd.Parameters.AddWithValue("@size", 2);
                        cmd.Parameters.AddWithValue("@user_id", 1);
                        cmd.Parameters.AddWithValue("@bytes", imageData);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
