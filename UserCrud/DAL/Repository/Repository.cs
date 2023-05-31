
using BAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;


namespace DAL.Repository
{
    public class Repository : IRepository
    {
        public void deleteUserDetails(long userID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            List<BAL.Models.UserViewModel> UserList = new List<BAL.Models.UserViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", userID, DbType.Int64);

                UserList = connection.Query<UserViewModel>("sp_UserDeleteOpertaion", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public List<UserViewModel> GetUSerDetails()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
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

        public void SaveUserDetails(UserViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            

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

                connection.Execute("sp_UserInsertUpdate", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public List<UserViewModel> GetUserDetails(FilterOptions filterOptions)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
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
                
                userList = connection.Query<UserViewModel>("sp_UserFiltering", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return userList;
        }

        public UserViewModel GetUserById(long id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            UserViewModel userByID = new UserViewModel();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int64);
                userByID = connection.QueryFirstOrDefault<UserViewModel>("GetUserById", parameters, commandType: CommandType.StoredProcedure);
            }

            return userByID;
        }

        public void GeCSVFile()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            List<UserViewModel> userList;

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                userList = connection.Query<UserViewModel>("sp_getCSVData", commandType: CommandType.StoredProcedure).ToList();
            }

            StringBuilder csvData = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            foreach (UserViewModel user in userList)
            {
                headers = new StringBuilder();
                var type = typeof(UserViewModel);
                var properties = type.GetProperties();
                foreach (PropertyInfo prop in typeof(UserViewModel).GetProperties())
                {
                    var props = prop;
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
    }
}

