
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SocialDAL
{
    public class DALHelper
    {
        //private static IConfiguration _config;

        //public static void Configure(IConfiguration config)
        //{
        //    _config = config;
        //}

        ///// <summary>
        ///// It will return connection string of the DB
        ///// </summary>
        //public static string Connectionstring
        //{
        //    get
        //    {
        //        string ConnectionString = _config["ConnectionStrings:DefaultConnection"];
        //        return ConnectionString;
        //    }
        //}

        /// <summary>
        /// This function ngenerate New OID for the table
        /// </summary>
        /// <returns></returns>
        public static string GenerateOID()
        {
            string oid = "";
            System.Random random = new Random();
            string guid = System.Guid.NewGuid().ToString("N");
            DateTime d = DateTime.Now;
            oid = String.Format("{0}-{1}", d.ToString("yyyyMMddHHmmss"), Convert.ToString(guid).Substring(0, 16).PadLeft(7, '0'));
            return oid;
        }

        /// <summary>
        /// It will return CommandTimeout of the DB
        /// </summary>
        //public static int CommandTimeout
        //{
        //    get
        //    {
        //        int timeout = 0;
        //        try
        //        {
        //            timeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);
        //        }
        //        catch (Exception)
        //        {
        //            //DALLogger.LogThis(ex);
        //        }
        //        return timeout;
        //    }
        //}
        /// <summary>
        /// Add string type parameter to command
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="paramName"></param>
        /// <param name="paramvalue"></param>
        public static void AddParameterString(System.Data.SqlClient.SqlCommand comm, string paramName, string paramvalue)
        {
            comm.Parameters.AddWithValue(paramName, CheckNullStringForParameter(paramvalue));
        }

        /// <summary>
        /// Add boolean type parameter to command
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="paramName"></param>
        /// <param name="paramvalue"></param>
        public static void AddParameterBool(System.Data.SqlClient.SqlCommand comm, string paramName, bool paramvalue)
        {
            comm.Parameters.AddWithValue(paramName, CheckNullForParameter(paramvalue));
        }

        /// <summary>
        /// Add datetime type parameter to command
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="paramName"></param>
        /// <param name="paramvalue"></param>
        public static void AddParameterDateTime(System.Data.SqlClient.SqlCommand comm, string paramName, DateTime paramvalue)
        {
            comm.Parameters.AddWithValue(paramName, CheckNullDateForParameter(paramvalue));
        }

        /// <summary>
        /// Add datetime type parameter to command
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="paramName"></param>
        /// <param name="paramvalue"></param>
        public static void AddParameterDouble(System.Data.SqlClient.SqlCommand comm, string paramName, double paramvalue)
        {
            comm.Parameters.AddWithValue(paramName, CheckNullForParameter(paramvalue));
        }

        /// <summary>
        /// Add datetime type parameter to command
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="paramName"></param>
        /// <param name="paramvalue"></param>
        public static void AddParameterInteger(System.Data.SqlClient.SqlCommand comm, string paramName, int paramvalue)
        {
            comm.Parameters.AddWithValue(paramName, CheckNullForParameter(paramvalue));
        }

        /// <summary>
        /// Check parameter for DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CheckNullForParameter(Object obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// Check datetype parameter for DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CheckNullDateForParameter(DateTime obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }
            else if (obj == DateTime.MinValue)
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// Check string type parameter for DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CheckNullStringForParameter(string obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }
            else if (obj == "")
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToString(object obj)
        {
            string rslt = "";
            try
            {
                rslt = Convert.ToString(obj).Trim();
            }
            catch { }
            return rslt;
        }

        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ConvertToInteger(object obj)
        {
            int rslt = 0;
            try
            {
                rslt = Convert.ToInt32(obj);
            }
            catch { }
            return rslt;
        }

        /// <summary>
        /// Convert object to double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ConvertToDouble(object obj)
        {
            double rslt = 0;
            try
            {
                rslt = Convert.ToDouble(obj);
            }
            catch { }
            return rslt;
        }

        public static bool ConvertToBool(object obj)
        {
            bool rslt = false;
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return false;


                rslt = Convert.ToBoolean(obj);
            }
            catch { }
            return rslt;
        }


        /// <summary>
        /// Convert object to DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ConvertToDatetime(object obj)
        {
            DateTime rslt = Convert.ToDateTime("1900-01-01");
            try
            {
                rslt = (DateTime)obj;
            }
            catch { }

            return rslt;
        }

        /// <summary>
        /// Convert object to DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ConvertToDatetimeMinValue(object obj)
        {
            DateTime rslt = DateTime.MinValue; ;
            try
            {
                rslt = (DateTime)obj;
            }
            catch { }

            return rslt;
        }

        /// <summary>
        /// replace apostopee from the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceApostopee(string str)
        {
            return str.Replace("'", "''");
        }
    }
}
