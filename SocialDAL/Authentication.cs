using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SocialDAL
{
    public class Authentication
    {

        public static SocialBOL.Authentication GetEmail(String Email, String ConnectionString)
        {
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();

            SocialBOL.Authentication data = null;

            try
            {
                string sql = "";

                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);
                sql += " select * from  Login ";
                sql += $" where  email1 = '{Email}' ";

                comm.CommandText = sql;

                conn.Open();

                SqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    //DALLogger.Logger.Error("login successful for :" + username);
                    dr.Read();
                    data = new SocialBOL.Authentication();
                    MakeBEFromDr(dr, data, sql);
                }
            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;

            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return data;
        }


        public static SocialBOL.Authentication GetName(String Email, String FirstName, String LastName, String ConnectionString)
        {
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();

            SocialBOL.Authentication data = null;

            try
            {
                string sql = "";

                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);
                sql += " select firstName,lastName from  Login ";
                sql += $" where  email1 = '{Email}' ";

                    comm.CommandText = sql;
                
                conn.Open();

                SqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    //DALLogger.Logger.Error("login successful for :" + username);
                    dr.Read();
                    data = new SocialBOL.Authentication();
                    data.FirstName = DALHelper.ConvertToString(dr["firstName"]);
                    data.LastName = DALHelper.ConvertToString(dr["lastName"]);
                }
            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;

            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return data;
        }


        public static int AddUserByNameAndIdentifier(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
           
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();
            int result;
            try
            {
                string sql = "";

                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);

                if (provider == "Google")
                {
                    sql += " INSERT INTO [dbo].[Login] ([status],[firstName],[lastName],[enrollmentDate],[googleId]) ";
                    sql += $" VALUES('{A.Status}','{A.FirstName}','{A.LastName}',getDate(),'{A.GoogleId}') ";

                    comm.CommandText = sql;
                }
                if (provider == "Facebook")
                {
                    sql += " INSERT INTO [dbo].[Login] ([status],[firstName],[lastName],[enrollmentDate],[facebookId]) ";
                    sql += $" VALUES('{A.Status}','{A.FirstName}','{A.LastName}',getDate(),'{A.FacebookId}') ";

                    comm.CommandText = sql;
                }
                conn.Open();

                result = comm.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;
                return -1;
            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return result;
        }


        public static int AddUserByNameIdentifierAndEmail(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
            A.OID = DALHelper.GenerateOID();
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();
            int result;
            try
            {
                string sql = "";

                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);

                if (provider == "Google")
                {
                    sql += " INSERT INTO [dbo].[Login] ([OID],[status],[firstName],[lastName],[enrollmentDate],[email1],[googleId]) ";
                    sql += $" VALUES('{A.OID}','{A.Status}','{A.FirstName}','{A.LastName}',getDate(),'{A.Email1}','{A.GoogleId}') ";

                    comm.CommandText = sql;
                }
                if (provider == "Facebook")
                {
                    sql += " INSERT INTO [dbo].[Login] ([OID],[status],[firstName],[lastName],[enrollmentDate],[email1],[facebookId]) ";
                    sql += $" VALUES('{A.OID}','{A.Status}','{A.FirstName}','{A.LastName}',getDate(),'{A.Email1}','{A.FacebookId}') ";

                    comm.CommandText = sql;
                }
                conn.Open();

                result = comm.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;
                return -1;
            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return result;
        }


        public static int AddUserByIdentifier(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();
            int result;
            try
            {
                string sql = "";
                
                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);

                if (provider == "Google")
                {
                    sql += $" UPDATE [dbo].[Login] SET [googleId] = '{A.GoogleId}'";
  
                    sql += $" WHERE[email1] = '{A.Email1}' ";

                    comm.CommandText = sql;
                }
                if (provider == "Facebook")
                {
                    sql += $" UPDATE [dbo].[Login] SET [facebookId] = '{A.FacebookId}'";

                    sql += $" WHERE[email1] = '{A.Email1}' ";

                    comm.CommandText = sql;
                }
                conn.Open();

                result = comm.ExecuteNonQuery();

                
            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;
                return -1;
            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return result;
        }
        public static SocialBOL.Authentication GetLoginByIdentifierAndEmail(string provider,string ID, string Email, string ConnectionString)
        {
            SqlConnection conn = new(ConnectionString);
            SqlCommand comm = new();

            SocialBOL.Authentication data = null;

            try
            {
                string sql = "";

                comm.Connection = conn;
                comm.CommandType = (System.Data.CommandType.Text);

                if(provider == "Google")
                {
                    sql += " select * from  Login ";
                    sql += " where googleID = @googleID and email1 = @email1 ";

                    comm.CommandText = sql;
                    comm.Parameters.AddWithValue("@googleID", ID);
                    comm.Parameters.AddWithValue("@email1", Email);
                }
                if(provider == "Facebook")
                {
                    sql += " select * from  Login ";
                    sql += " where facebookID = @facebookID and email1 = @email1 ";

                    comm.CommandText = sql;
                    comm.Parameters.AddWithValue("@facebookID", ID);
                    comm.Parameters.AddWithValue("@email1", Email);
                }
                conn.Open();

                SqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    //DALLogger.Logger.Error("login successful for :" + username);
                    dr.Read();
                    data = new SocialBOL.Authentication();
                    MakeBEFromDr(dr, data, sql);
                }
            }
            catch (Exception ex)
            {
                //Log the error, give it a unique ID, re-throw so it can bubble up.
                //DALLogger.LogThis(ex);
                //throw ex;

            }
            finally
            {
                //Close the connection and data reader

                conn.Close();
                conn.Dispose();
            }

            return data;
        }

        static void MakeBEFromDr(SqlDataReader dr, SocialBOL.Authentication be, string sql)
        {
            be.OID = DALHelper.ConvertToString(dr["OID"]);
            be.Id = DALHelper.ConvertToString(dr["id"]);
            be.Password = DALHelper.ConvertToString(dr["password"]);
            be.LoginType = DALHelper.ConvertToString(dr["loginType"]);
            be.Status = DALHelper.ConvertToString(dr["status"]);
            be.FirstName = DALHelper.ConvertToString(dr["firstName"]);
            be.LastName = DALHelper.ConvertToString(dr["lastName"]);
            be.EnrollmentDate = DALHelper.ConvertToDatetime(dr["enrollmentDate"]);
            be.Email1 = DALHelper.ConvertToString(dr["email1"]);
            be.Email2 = DALHelper.ConvertToString(dr["email2"]);
            be.SecurityQuestion = DALHelper.ConvertToString(dr["securityQuestion"]);
            be.SecurityAnswer = DALHelper.ConvertToString(dr["securityAnswer"]);
            be.GoogleId = DALHelper.ConvertToString(dr["googleId"]);
            be.FacebookId = DALHelper.ConvertToString(dr["facebookId"]);
        }
    }
}
