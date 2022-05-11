using System;
using System.Collections.Generic;
using System.Text;

namespace SocialBLL
{
    public class Authentication
    {
        public static SocialBOL.Authentication GetEmail(String Email, String ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.GetEmail(Email, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return null;
            }
        }


        public static SocialBOL.Authentication GetName( String Email, String FirstName, String LastName, String ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.GetName( Email, FirstName, LastName, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return null;
            }
        }


        public static int AddUserByNameIdentifierAndEmail(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.AddUserByNameIdentifierAndEmail(provider, A, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return -1;
            }
        }

        public static int AddUserByNameAndIdentifie(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.AddUserByNameAndIdentifier(provider, A, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return -1;
            }
        }

        public static int AddUserByIdentifier(string provider, SocialBOL.Authentication A, string ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.AddUserByIdentifier(provider, A, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return -1;
            }
        }
        public static SocialBOL.Authentication GetLoginByIdentifierAndEmail(string provider,string ID, string Email, string ConnectionString)
        {
            try
            {
                return SocialDAL.Authentication.GetLoginByIdentifierAndEmail(provider,ID, Email, ConnectionString);
            }
            catch (Exception ex)
            {
                //BLLLogger.LogThis(ex);
                return null;
            }
        }
    }
}
