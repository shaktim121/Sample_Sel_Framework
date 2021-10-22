using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Data.OleDb;

namespace Sel.TestAuto
{
    public static class GlobalDB
    {
        public static string dataSource = string.Empty;
        public static string dbName = string.Empty;
        public static string dbUser = string.Empty;
        public static string dbPwd = string.Empty;
        public static string connectionString = string.Empty;

        //Function to create sql connection
        public static string CreateConnectionString(string DBSrv, string DB, string user, string pwd, bool winAuth)
        {
            string cnn;
            try
            {
                if (winAuth)
                {
                    cnn = @"Data Source=" + DBSrv + ";Initial Catalog=" + DB + ";Integrated Security=SSPI;";
                    //cnn = @"Data Source=" + DBSrv + ";Initial Catalog=" + DB + ";User ID=" + user + ";Integrated Security=SSPI;";
                }
                else if(user!="" && pwd!="")
                {
                    cnn = @"Data Source=" + DBSrv + ";Initial Catalog=" + DB + ";User ID=" + user + ";Password=" + pwd + "";
                }
                else
                {
                    throw new Exception("User Name and Password are required for SQL Server Authentication");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        //Connect to Automation DB and Return the SQLConnection
        public static SqlConnection DBConnect(string dataSource, string dbName, string dbUser, string dbPwd, bool winAuth)
        {
            SqlConnection cnn;
            try
            {
                connectionString = CreateConnectionString(dataSource, dbName, dbUser, dbPwd, winAuth);
                cnn = new SqlConnection(connectionString);
                cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("DB Connection Failed: " + ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        public static SqlConnection DBConnect(string connectionString)
        {
            SqlConnection cnn;
            try
            {
                cnn = new SqlConnection(connectionString);
                cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("DB Connection Failed: " + ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        //Disposes the SQLConnection
        public static void DBDispose(this SqlConnection cnn)
        {
            try
            {
                cnn.Close();
                cnn.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Close DB Connection: " + ex.Message + " " + ex.StackTrace);
            }
        }

        //Function to execute any SQL Query
        public static DataSet ExecuteSQLQuery(string sqlQuery, string connectionString)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;

            string sql;

            try
            {
                cnn = new SqlConnection(connectionString);
                sql = sqlQuery;
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }

                adapter.Dispose();
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return data;
        }

        //Function to update using sql query
        public static int ExecuteNonSQLQuery(string sqlQuery, string connectionString)
        {
            SqlConnection cnn;
            SqlCommand command;
            string sql; int rows;

            try
            {
                cnn = DBConnect(connectionString);
                sql = sqlQuery;
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    rows = command.ExecuteNonQuery();
                }
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return rows;
        }

        //Execute StoredProc
        //DataSet ds = GlobalDB.ExecuteStoredProc("EmployeeAnniversaryListing_report", "@databaseName:WLAT;@securityRoleId:1001;@securityUserId:58;@languageCode:EN");
        public static DataSet ExecuteStoredProc(string storedProc, string parameters, string connectionString)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            string[] parameter = parameters.Split(';');

            try
            {
                cnn = new SqlConnection(connectionString);
                command = new SqlCommand(storedProc, cnn);
                command.CommandType = CommandType.StoredProcedure;

                foreach (string str in parameter)
                {
                    if(str.Split(':')[1].ToString().ToLower()=="null")
                    {
                        command.Parameters.Add(new SqlParameter(str.Split(':')[0], DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter(str.Split(':')[0], str.Split(':')[1]));
                    }
                    
                }

                adapter = new SqlDataAdapter(command);
                adapter.Fill(data);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return data;
        }

        //Verify if Stored Proc Exists
        public static bool IsStoredProcedureExists(string cnn, string storedProc)
        {
            bool flag = false;
            try
            {
                string chkSql = "select * from sys.objects where type_desc = 'SQL_STORED_PROCEDURE' AND name = '" + storedProc + "'";
                if (ExecuteSQLQuery(chkSql, cnn).Tables[0].Rows.Count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return flag;
        }

        //Function to load all the table data together
        public static DataSet LoadTestData(this string testName)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            string sql;
            dataSource = "dataSource".AppSettings();
            dbName = "dbName".AppSettings();
            dbUser = "dbUser".AppSettings();
            dbPwd = "dbPwd".AppSettings();

            try
            {
                cnn = DBConnect(dataSource, dbName, dbUser, dbPwd, true);
                var cred = cnn.Credential;
                sql = "select * from [" + "dbName".AppSettings() + "].[dbo].[" + testName + "];";
                //sql = "select TOP (1) ["+key_name+"] from ["+dbName+"].[dbo].["+testName+"];";
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }

                //command = new SqlCommand(sql, cnn);
                //dataReader = command.ExecuteReader();
                /*while (dataReader.Read())
                {
                    data = dataReader.GetValue(0).ToString();
                    break;
                }*/

                //dataReader.Close();
                adapter.Dispose();
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return data;
        }

        //Function to fetch individual test data value from a dataSet
        public static string GetTestData(this DataSet ds, string key, int Row=0)
        {
            string sval = string.Empty;
            var cols = ds.Tables[0].Columns;
            try
            {
                foreach(DataColumn col in cols)
                {
                    if(col.ColumnName.Trim().Equals(key.Trim()))
                    {
                        sval = Convert.ToString(ds.Tables[0].Rows[Row][col.ColumnName]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return sval;
        }

        /// <summary>
        /// Set Data back to Excel with Excel Path, Sheet and Column Name (Only sets in the first row)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheetName"></param>
        /// <param name="colName"></param>
        /// <param name="cellVal"></param>
        public static void SetExcelData(string path, string sheetName, string colName, string cellVal)
        {
            var connString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Extended Properties='Excel 8.0;ReadOnly=False;HDR=YES;TypeGuessRows=0;ImportMixedTypes=Text'";
            if (Path.GetExtension(path).ToLower() == ".xlsx")
            {
                connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties='Excel 12.0'";
            }

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    var sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [" + sheetName + "$] SET " + colName + " = '" + cellVal + "';";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Load Excel Data with Path and Sheet Name in DataSet format
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataSet LoadExcelData(string path, string sheetName)
        {
            var ds = new DataSet();

            var connString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Extended Properties='Excel 8.0;ReadOnly=False;HDR=YES;TypeGuessRows=0;ImportMixedTypes=Text'";
            if (Path.GetExtension(path).ToLower() == ".xlsx")
            {
                connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties='Excel 12.0'";
            }

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    var sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select * from [" + sheetName + "$];";
                        var adapter = new OleDbDataAdapter(cmd);
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return ds;
        }


    }
}
