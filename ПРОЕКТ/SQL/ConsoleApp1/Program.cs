using System;
using System.Text;
using System.Data.SqlClient;

namespace TestSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                
                // Data Source=127.0.0.1;Initial Catalog=TestSharp;Integrated Security=True
                builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
                builder.InitialCatalog = "TestSharp";
                builder.IntegratedSecurity = true;

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");


                    String sql;
                    //Console.Write("Dropping and creating database 'SampleDB' ... ");
                    //String sql = "DROP DATABASE IF EXISTS SampleDB; CREATE DATABASE SampleDB";
                    //using (SqlCommand command = new SqlCommand(sql, connection))
                    //{
                    //    command.ExecuteNonQuery();
                    //    Console.WriteLine("Done.");
                    //}


                    Console.Write("Creating sample table with data, press any key to continue...");
                    Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("USE SampleDB; ");

                    sb.Append(" CREATE TABLE Persons  ( ");
                    sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" Name NVARCHAR(50), ");
                    sb.Append(" Initials NVARCHAR(50) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Persons (Name, Initials) VALUES ");
                    sb.Append("(N'Jared', N'V.V'), ");
                    sb.Append("(N'Nikita', N'M.M'), ");
                    sb.Append("(N'Tom', N'G.A'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // INSERT demo
                    Console.WriteLine("How much do you want to add");
                    int index;
                    string name;
                    string initials;
                    index = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < index; i++)
                    {
                        Console.WriteLine("Name:");
                        name = Console.ReadLine();
                        Console.WriteLine("Initials:");
                        initials= Console.ReadLine();
                        Program p = new Program();
                        p.INSERT(name,initials);
                    }


                    // UPDATE demo
                    String userToUpdate = "Nikita";
                    Console.Write("Updating 'Location' for user '" + userToUpdate + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Persons SET Initials = N'S.A' WHERE Name = @name");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userToUpdate);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) updated");
                    }

                    // DELETE demo
                    String userToDelete = "Jared";
                    Console.Write("Deleting user '" + userToDelete + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Persons WHERE Name = @name;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) deleted");
                    }

                    // READ demo
                    Console.WriteLine("Reading data from table, press any key to continue...");
                    Console.ReadKey(true);
                    sql = "SELECT Id, Name, Initials FROM Persons;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }

       void  INSERT(string Name, string initials)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
                builder.InitialCatalog = "TestSharp";
                builder.IntegratedSecurity = true;
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open(); 
        
            
                String sql;
                StringBuilder sb = new StringBuilder();
                Console.Write("Inserting a new row into table, press any key to continue...");
                Console.ReadKey(true);
                sb.Clear();
                sb.Append("INSERT Persons (Name, Initials) ");
                sb.Append("VALUES (@name, @Initials);");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@name", Name);
                    command.Parameters.AddWithValue($"@initials", initials);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) inserted");
                }
        }
    }
}

   

