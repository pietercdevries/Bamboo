using Dapper;
using System;
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

        public void newConnection(string connection)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = connection;
            }
        }

        private List<KeyValuePair<string, object>> transactions = new List<KeyValuePair<string, object>>();


        public void Add(object item)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = item.GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Insert Into[{ itemName}] ({ propertyNames}) Values({ propertyParamaters})", item);
            transactions.Add(command);
        }

        public void Add(List<object> items)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = items.First().GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Insert Into [{itemName}] ({propertyNames}) Values ({propertyParamaters})", items);
            transactions.Add(command);
        }

        public void Edit(object item)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = item.GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Update [{itemName}] SET {properties} Where Id = @Id", item);
            transactions.Add(command);
        }

        public void Edit(List<object> items)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = items.First().GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;
            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"Update [{itemName}] SET {properties} Where Id = @Id", items);
            transactions.Add(command);
        }

        public void Delete(object item)
        {
            Type itemtype = item.GetType();
            string itemName = itemtype.Name;
            string primaryKey = "";

            if (itemtype.GetProperty("Id") != null)
            {
                primaryKey = "Id";
            }
            else if (itemtype.GetProperty("AutoId") != null)
            {
                primaryKey = "AutoId";
            }

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"DELETE FROM [{itemName}] Where {primaryKey} = @{primaryKey}", item);
            transactions.Add(command);
        }

        public void Delete(List<object> items)
        {
            Type itemtype = items.First().GetType();
            string itemName = itemtype.Name;
            string primaryKey = "";

            if (itemtype.GetProperty("Id") != null)
            {
                primaryKey = "Id";
            }
            else if (itemtype.GetProperty("AutoId") != null)
            {
                primaryKey = "AutoId";
            }

            KeyValuePair<string, object> command = new KeyValuePair<string, object>($"DELETE FROM [{itemName}] Where {primaryKey} = @{primaryKey}", items);
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


        public void QuickAdd(object item)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = item.GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;

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

        public void QuickAdd(List<object> items)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = items.First().GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;

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

        public void QuickEdit(object item)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = item.GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;

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

        public void QuickEdit(List<object> items)
        {
            string propertyNames = "";
            string propertyParamaters = "";
            Type itemType = items.First().GetType();

            System.Reflection.PropertyInfo[] properties = itemType.GetProperties();

            for (int I = 0; I < properties.Count(); I++)
            {
                if (properties[I].Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) || properties[I].Name.Equals("AutoId", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                if (I == properties.Count() - 1)
                {
                    propertyNames += "[" + properties[I].Name + "]";
                    propertyParamaters += "@" + properties[I].Name;
                }
                else
                {
                    propertyNames += "[" + properties[I].Name + "],";
                    propertyParamaters += "@" + properties[I].Name + ",";
                }
            }

            string itemName = itemType.Name;

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

        public void QuickDelete(object item)
        {
            Type itemtype = item.GetType();
            string itemName = itemtype.Name;
            string primaryKey = "";

            if (itemtype.GetProperty("Id") != null)
            {
                primaryKey = "Id";
            }
            else if (itemtype.GetProperty("AutoId") != null)
            {
                primaryKey = "AutoId";
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"DELETE FROM [{itemName}] Where {primaryKey} = @{primaryKey}", item, sqlTransaction);
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

        public void QuickDelete(List<object> items)
        {
            Type itemtype = items.First().GetType();
            string itemName = itemtype.Name;
            string primaryKey = "";

            if (itemtype.GetProperty("Id") != null)
            {
                primaryKey = "Id";
            }
            else if (itemtype.GetProperty("AutoId") != null)
            {
                primaryKey = "AutoId";
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute($"DELETE FROM [{itemName}] Where {primaryKey} = @{primaryKey}", items, sqlTransaction);
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
    }
}
