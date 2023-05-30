
using BAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                parameters.Add("ID", DBNull.Value, DbType.String);
                parameters.Add("first_name", model.FirstName, DbType.String);
                parameters.Add("last_name",model.LastName , DbType.String);
                parameters.Add("email", model.Email, DbType.String);
                parameters.Add("phone_number",model.PhoneNUmber , DbType.String);
                parameters.Add("country", model.Country, DbType.String);
                parameters.Add("city",model.City , DbType.String);
                parameters.Add("pincode", model.PinCode, DbType.Int32);
                parameters.Add("dob", model.DAteOfBirth.ToString("MM/dd/yyyy"), DbType.String);
                parameters.Add("address",model.Address , DbType.String);
                parameters.Add("queryType", "Insert", DbType.String);

                connection.Query<UserViewModel>("sp_UserInsertUpdate", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public List<BAL.Models.UserViewModel> GetUserDetails(int pageNumber, int pageSize, string searchValue, int sortColumn, string sortDir)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            List<BAL.Models.UserViewModel> UserList = new List<BAL.Models.UserViewModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", DBNull.Value, DbType.String);
                parameters.Add("PageNo", pageNumber, DbType.String);
                parameters.Add("PageSize", pageSize, DbType.String);
                parameters.Add("SortColumn", sortColumn, DbType.String);
                parameters.Add("SortDirection", sortDir, DbType.String);
               // parameters.Add("country", DBNull.Value, DbType.String);
                //parameters.Add("city", DBNull.Value, DbType.String);
                //parameters.Add("pincode", DBNull.Value, DbType.Int32);
                //parameters.Add("dob", DBNull.Value, DbType.String);
                //parameters.Add("address", DBNull.Value, DbType.String);
                //parameters.Add("queryType", "Select", DbType.String);

                UserList = connection.Query<UserViewModel>("sp_UserGetOpertaion", parameters, commandType: CommandType.StoredProcedure).ToList();
            }


            return UserList;
        }
    }
}

