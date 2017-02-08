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

        private List<KeyValuePair<string, object>> transactions = new List<KeyValuePair<string, object>>();

        public void Add(object item, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            string itemName = item.GetType().Name;

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

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Insert Into[{ itemName}] ({ propertyNames}) Values({ propertyParamaters})", item);
            transactions.Add(command);
        }

        public void Add(List<object> items, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            string itemName = items.First().GetType().Name;

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

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Insert Into [{itemName}] ({propertyNames}) Values ({propertyParamaters})", items);
            transactions.Add(command);
        }

        public void Edit(object item, string[] paramaterNames)
        {
            string properties = "";
            string itemName = item.GetType().Name;

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

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Update [{itemName}] SET {properties} Where Id = @Id", item);
            transactions.Add(command);
        }

        public void Edit(List<object> items, string[] paramaterNames)
        {
            string properties = "";
            string itemName = items.First().GetType().Name;

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

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Update [{itemName}] SET {properties} Where Id = @Id", items);
            transactions.Add(command);
        }

        public void Delete(object item)
        {
            string itemName = item.GetType().Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"DELETE FROM [{itemName}] Where Id = @Id", item);
            transactions.Add(command);
        }

        public void Delete(List<object> items)
        {
            string itemName = items.First().GetType().Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"DELETE FROM [{itemName}] Where Id = @Id", items);
            transactions.Add(command);
        }

        public void SaveChanges()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (KeyValuePair<string, object> command in transactions)
                        {
                            sqlConnection.Execute(command.Key, command.Value, sqlTransaction);
                        }

                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        sqlConnection.Close();
                        transactions.Clear();
                    }
                }
                sqlConnection.Close();
            }

            transactions.Clear();
        }

        public void SaveChangesAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    foreach (KeyValuePair<string, object> command in transactions)
                    {
                        sqlConnection.ExecuteAsync(command.Key, command.Value);
                    }
                    sqlConnection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    sqlConnection.Close();
                    transactions.Clear();
                }
            }

            transactions.Clear();
        }

        public void CancelChanges()
        {
            transactions.Clear();
        }

        public void ForceAdd(object item, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            string itemName = item.GetType().Name;

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
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"Insert Into [{itemName}] ({propertyNames}) Values ({propertyParamaters})", item, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        public void ForceAdd(List<object> items, string[] paramaterNames)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            string itemName = items.First().GetType().Name;

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
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"Insert Into [{itemName}] ({propertyNames}) Values ({propertyParamaters})", items, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        public void ForceEdit(object item, string[] paramaterNames)
        {
            string properties = "";
            string itemName = item.GetType().Name;

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
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"Update [{itemName}] SET {properties} Where Id = @Id", item, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        public void ForceEdit(List<object> items, string[] paramaterNames)
        {
            string properties = "";
            string itemName = items.First().GetType().Name;

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
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"Update [{itemName}] SET {properties} Where Id = @Id", items, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        public void ForceDelete(object item)
        {
            string itemName = item.GetType().Name;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"DELETE FROM [{itemName}] Where Id = @Id", item, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
                sqlConnection.Close();
            }
        }

        public void ForceDelete(List<object> items)
        {
            string itemName = items.First().GetType().Name;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"DELETE FROM [{itemName}] Where Id = @Id", items, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
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
