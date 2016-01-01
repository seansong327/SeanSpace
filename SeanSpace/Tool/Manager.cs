using SeanSpace.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace SeanSpace.Tool
{
    public class Manager
    {
        public List<Notepad> GetNotepadList(long userSid)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("UserSid", SqlDbType.BigInt) { Value = userSid }
                };

            List<Notepad> result = new List<Notepad>();

            using(SqlConnection conn = SqlHelper.GetConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(conn, Sql.GetNotes, param))
                {
                    while (reader.Read())
                    {
                        result.Add(new Notepad
                            {
                                Sid = (long)reader["Sid"],
                                Title = (string)reader["Title"],
                                Content = (string)reader["Content"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            });
                    }
                }
            }

            return result;
        }

        public void DeleteNote(long sid, long userSid)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("Sid", SqlDbType.BigInt) { Value = sid },
                    new SqlParameter("UserSid", SqlDbType.BigInt) { Value = userSid }
                };

            SqlHelper.ExecuteNonQuery(Sql.DeleteNote, param);
        }

        public void AddNote(long userSid, string title, string content)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("Title", SqlDbType.NVarChar) { Value = title },
                    new SqlParameter("Content", SqlDbType.NVarChar) { Value = content },
                    new SqlParameter("UserSid", SqlDbType.BigInt) { Value = userSid }
                };

            SqlHelper.ExecuteNonQuery(Sql.AddNote, param);
        }

        public void UpdateNote(long sid, long userSid, string title, string content)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("Sid", SqlDbType.BigInt) { Value = sid },
                    new SqlParameter("Title", SqlDbType.NVarChar) { Value = title },
                    new SqlParameter("Content", SqlDbType.NVarChar) { Value = content },
                    new SqlParameter("UserSid", SqlDbType.BigInt) { Value = userSid }
                };

            SqlHelper.ExecuteNonQuery(Sql.UpdateNote, param);
        }

        public bool SignIn(string userName, string password)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("UserName", SqlDbType.VarChar) { Value = userName },
                    new SqlParameter("Password", SqlDbType.VarChar) { Value = MD5Encrypt(password) }
                };

            object result = SqlHelper.ExecuteScalar(Sql.SignIn, param);
            if (result != null)
            {
                FormsAuthentication.SetAuthCookie(result.ToString(), true);
            }

            return result != null;
        }

        private string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(strText));
            return BitConverter.ToString(result).Replace("-", null);
        }
    }
}