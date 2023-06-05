
using BAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace DAL.Repository
{
    public class Repository : IRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        public void deleteUserDetails(long userID)
        {
           
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", userID, DbType.Int64);

                 connection.Execute("sp_UserDeleteOpertaion", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<UserViewModel> GetUSerDetails()
        {

           
            List<BAL.Models.UserViewModel> UserList = new List<BAL.Models.UserViewModel>();

            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", DBNull.Value, DbType.String);
                //parameters.Add("first_name", DBNull.Value, DbType.String);
                //parameters.Add("last_name", DBNull.Value, DbType.String);
                //parameters.Add("email", DBNull.Value, DbType.String);
                //parameters.Add("phone_number", DBNull.Value, DbType.String);
                //parameters.Add("country", DBNull.Value, DbType.String);
                //parameters.Add("city", DBNull.Value, DbType.String);
                //parameters.Add("pincode", DBNull.Value, DbType.Int32);
                //parameters.Add("dob", DBNull.Value, DbType.String);
                //parameters.Add("address", DBNull.Value, DbType.String);
                //parameters.Add("queryType", "Select", DbType.String);
                
                UserList = connection.Query<UserViewModel>("sp_UserGetOpertaion", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
          

            return UserList;
        }

        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
           
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }
        public void SaveUserDetails(UserViewModel model)
        {

            var passwordHash = encryption(model.Password);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", model.ID, DbType.Int64);
                parameters.Add("first_name", model.FirstName, DbType.String);
                parameters.Add("last_name",model.LastName , DbType.String);
                parameters.Add("email", model.Email, DbType.String);
                parameters.Add("phone_number",model.PhoneNUmber , DbType.String);
                parameters.Add("country", model.Country, DbType.String);
                parameters.Add("city",model.City , DbType.String);
                parameters.Add("pincode", model.PinCode, DbType.Int32);
                parameters.Add("dob", model.DAteOfBirth, DbType.String);
                parameters.Add("address",model.Address , DbType.String);
                parameters.Add("password", passwordHash, DbType.String);
                parameters.Add("confirmPassword", passwordHash, DbType.String);

                connection.Execute("sp_UserInsertUpdate", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<UserViewModel> GetUserDetails(FilterOptions filterOptions)
        {
           
            List<UserViewModel> userList = new List<UserViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", DBNull.Value, DbType.String);
                parameters.Add("@FirstName", filterOptions.FirstName, DbType.String);
                parameters.Add("@LastName", filterOptions.LastName, DbType.String);
                parameters.Add("@Country", filterOptions.Country, DbType.String);
                parameters.Add("@City", filterOptions.City, DbType.String);
                parameters.Add("@FromDate", filterOptions.FromDate, DbType.DateTime);
                parameters.Add("@ToDate", filterOptions.ToDate, DbType.DateTime);
                parameters.Add("@SortColumn", filterOptions.SortColumn, DbType.String);
                parameters.Add("@SortOrder", filterOptions.SortDirection, DbType.String);
                parameters.Add("@PageNumber", filterOptions.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", filterOptions.PageSize, DbType.Int32);
                parameters.Add("@export", filterOptions.export, DbType.Int32);

                userList = connection.Query<UserViewModel>("sp_UserFiltering", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

           
            
            return userList;
        }

        public UserViewModel GetUserById(long id)
        {
            
            UserViewModel userByID = new UserViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int64);
                userByID = connection.QueryFirstOrDefault<UserViewModel>("GetUserById", parameters, commandType: CommandType.StoredProcedure);
            }

            return userByID;
        }

        public void GeCSVFile(FilterOptions filterOptions)
        {
            List<ExportViewModel> userList = new List<ExportViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", DBNull.Value, DbType.String);
                parameters.Add("@FirstName", filterOptions.FirstName, DbType.String);
                parameters.Add("@LastName", filterOptions.LastName, DbType.String);
                parameters.Add("@Country", filterOptions.Country, DbType.String);
                parameters.Add("@City", filterOptions.City, DbType.String);
                parameters.Add("@FromDate", filterOptions.FromDate, DbType.DateTime);
                parameters.Add("@ToDate", filterOptions.ToDate, DbType.DateTime);
                parameters.Add("@SortColumn", filterOptions.SortColumn, DbType.String);
                parameters.Add("@SortOrder", filterOptions.SortDirection, DbType.String);
                parameters.Add("@PageNumber", filterOptions.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", filterOptions.PageSize, DbType.Int32);
                parameters.Add("@export", filterOptions.export, DbType.Int32);

                userList = connection.Query<ExportViewModel>("sp_UserFiltering", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            StringBuilder csvData = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            foreach (ExportViewModel user in userList)
            {
                headers = new StringBuilder();
                var type = typeof(ExportViewModel);
                var properties = type.GetProperties();
                foreach (PropertyInfo prop in typeof(ExportViewModel).GetProperties())
                {
                    var props = prop;
                    var users = prop.GetValue(user)?.ToString();
                    csvData.Append(prop.GetValue(user)?.ToString() + ",");
                    headers.Append(prop.Name + ",");
                }

                csvData.Append("\r\n");
                headers.Append("\r\n");
            }

            string contentToExport = headers.Append(csvData.ToString()).ToString();
            string attachment = "attachment; filename=export.csv";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.Write(contentToExport);
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        public UserViewModel checkUser(string email, string password)
        {
            UserViewModel userExist;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var modelPassword = encryption(password);
                var parameters = new DynamicParameters();
                parameters.Add("@email", email, DbType.String);
                parameters.Add("@password", modelPassword, DbType.String);
                userExist = connection.QueryFirstOrDefault<UserViewModel>("sp_checkUser", parameters, commandType: CommandType.StoredProcedure);
            }
              return userExist;
            
           
           
        }

        public ProfileViewModel GetProfileById(long id)
        {
            ProfileViewModel profileByID = new ProfileViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int64);
                var reader = connection.QueryMultiple("sp_getUserProfile", parameters, commandType: CommandType.StoredProcedure);
                profileByID.UserDetail = reader.ReadFirst<UserViewModel>();
                profileByID.Suggestions = reader.ReadFirst<Suggestion>();
            }

            return profileByID;
        }
    }
}

