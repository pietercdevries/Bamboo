using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Bamboo
{
    public class Bamboo
    {
        public Bamboo()
        {
        }

        private string connectionString { get; set; }

        public void Add(object item, string objectName, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (I == propertyCount - 1)
                {
                    propertyNames += "[" + paramaterNames[I] + "]";
                    propertyParamaters += "@" + paramaterNames[I];
                }
                else
                {
                    propertyNames += "[" + paramaterNames[I] + "],";
                    propertyParamaters += "@" + paramaterNames[I] + ",";
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"Insert Into [{objectName}] ({propertyNames}) Values ({propertyParamaters})", item, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void AddAsync(object item, string objectName, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (I == propertyCount - 1)
                {
                    propertyNames += "[" + paramaterNames[I] + "]";
                    propertyParamaters += "@" + paramaterNames[I];
                }
                else
                {
                    propertyNames += "[" + paramaterNames[I] + "],";
                    propertyParamaters += "@" + paramaterNames[I] + ",";
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"Insert Into [{objectName}] ({propertyNames}) Values ({propertyParamaters})", item);
                sqlConnection.Close();
            }
        }

        public void Add(List<object> items, string objectName, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (I == propertyCount - 1)
                {
                    propertyNames += "[" + paramaterNames[I] + "]";
                    propertyParamaters += "@" + paramaterNames[I];
                }
                else
                {
                    propertyNames += "[" + paramaterNames[I] + "],";
                    propertyParamaters += "@" + paramaterNames[I] + ",";
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"Insert Into [{objectName}] ({propertyNames}) Values ({propertyParamaters})", items, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void AddAsync(List<object> items, string objectName, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (I == propertyCount - 1)
                {
                    propertyNames += "[" + paramaterNames[I] + "]";
                    propertyParamaters += "@" + paramaterNames[I];
                }
                else
                {
                    propertyNames += "[" + paramaterNames[I] + "],";
                    propertyParamaters += "@" + paramaterNames[I] + ",";
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"Insert Into [{objectName}] ({propertyNames}) Values ({propertyParamaters})", items);
                sqlConnection.Close();
            }
        }

        public void Edit(object item, string objectName, string[] paramaterNames)
        {
            string properties = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (paramaterNames[I] != null)
                {
                    if (I == propertyCount - 1)
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I]; ;
                    }
                    else
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I] + ", ";
                    }
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"Update [{objectName}] SET {properties} Where Id = @Id", item, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void EditAsync(object item, string objectName, string[] paramaterNames)
        {
            string properties = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (paramaterNames[I] != null)
                {
                    if (I == propertyCount - 1)
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I]; ;
                    }
                    else
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I] + ", ";
                    }
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"Update [{objectName}] SET {properties} Where Id = @Id", item);
                sqlConnection.Close();
            }
        }

        public void Edit(List<object> items, string objectName, string[] paramaterNames)
        {
            string properties = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (paramaterNames[I] != null)
                {
                    if (I == propertyCount - 1)
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I]; ;
                    }
                    else
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I] + ", ";
                    }
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"Update [{objectName}] SET {properties} Where Id = @Id", items, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void EditAsync(List<object> items, string objectName, string[] paramaterNames)
        {
            string properties = "";

            int propertyCount = paramaterNames.Count();
            for (int I = 0; I < propertyCount; I++)
            {
                if (paramaterNames[I] != null)
                {
                    if (I == propertyCount - 1)
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I]; ;
                    }
                    else
                    {
                        properties += "[" + paramaterNames[I] + "] = " + "@" + paramaterNames[I] + ", ";
                    }
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"Update [{objectName}] SET {properties} Where Id = @Id", items);
                sqlConnection.Close();
            }
        }

        public void Delete(object item, string objectName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"DELETE FROM [{objectName}] Where Id = @Id", item, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void DeleteAsync(object item, string objectName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"DELETE FROM [{objectName}] Where Id = @Id", item);
                sqlConnection.Close();
            }
        }

        public void Delete(List<object> items, string objectName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute($"DELETE FROM [{objectName}] Where Id = @Id", items, transaction);
                    transaction.Commit();
                }
                sqlConnection.Close();
            }
        }

        public void DeleteAsync(List<object> items, string objectName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                sqlConnection.ExecuteAsync($"DELETE FROM [{objectName}] Where Id = @Id", items);
                sqlConnection.Close();
            }
        }

        public void newConnection(string connection)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = connection;
            }
        }
    }
}
