using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SeanSpace.Tool
{
    public class SqlHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["db1"].ToString();

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] param)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(param);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, SqlParameter[] param)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(param);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string sql, SqlParameter[] param)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteReader();
            }
        }
    }

    public static class Sql
    {
        public static string GetNote =
            @"Select
                Sid,
                Title,
                Content,
                ModifiedDate
            FROM
                Notepad
            WHERE
                UserSid = @UserSid
                AND Sid = @Sid
                AND MarkForDelete = 0";

        public static string GetNotes =
            @"Select
                Sid,
                Title,
                Content,
                ModifiedDate
            FROM
                Notepad
            WHERE
                UserSid = @UserSid
                AND MarkForDelete = 0
            ORDER BY
                ModifiedDate DESC";

        public static string AddNote =
            @"INSERT INTO Notepad
            (
                Title,
                [Content],
                UserSid
            )
            VALUES
            (
                @Title,
                @Content,
                @UserSid
            )";

        public static string UpdateNote =
            @"UPDATE Notepad
            SET
                Title = @Title,
                [Content] = @Content
            WHERE
                UserSid = @UserSid
                AND Sid = @Sid";

        public static string DeleteNote =
            @"UPDATE Notepad
            SET
                MarkForDelete = 1
            WHERE
                UserSid = @UserSid
                AND Sid = @Sid";

        public static string SignIn =
            @"SELECT
                Sid
            FROM
                [User]
            WHERE
                LoginName = @UserName
                AND Password = @Password";
    }
}