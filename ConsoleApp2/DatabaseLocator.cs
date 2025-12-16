using Microsoft.Data.SqlClient;
using System;

namespace ConsoleApp2
{
    class DatabaseLocator
    {
        public static void FindDatabase()
        {
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("SQL SERVER DATABASE LOCATOR");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            // Lijst van veelvoorkomende SQL Server instances
            string[] commonInstances = new string[]
            {
                ".",
                "localhost",
                "localhost\\SQLEXPRESS",
                "(localdb)\\MSSQLLocalDB",
                "(localdb)\\v11.0",
                Environment.MachineName,
                Environment.MachineName + "\\SQLEXPRESS"
            };

            Console.WriteLine("Zoeken naar SQL Server instances...\n");

            foreach (string instance in commonInstances)
            {
                Console.Write($"Proberen: {instance,-40} ");
                
                try
                {
                    string connectionString = $"Server={instance};Database=master;Integrated Security=true;Connection Timeout=3;TrustServerCertificate=true;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✓ WERKT!");
                        Console.ResetColor();
                        
                        // Check of AnimalDB bestaat op deze instance
                        CheckForAnimalDB(connection, instance);
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ Niet beschikbaar");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static void CheckForAnimalDB(SqlConnection connection, string instance)
        {
            try
            {
                string query = "SELECT name FROM sys.databases WHERE name = 'AnimalDB'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"  └─> AnimalDB GEVONDEN op: {instance}");
                        Console.ResetColor();
                        
                        // Toon locatie van de database bestanden
                        ShowDatabaseFiles(connection);
                    }
                }
            }
            catch { }
        }

        private static void ShowDatabaseFiles(SqlConnection connection)
        {
            try
            {
                string query = @"SELECT physical_name 
                               FROM sys.master_files 
                               WHERE database_id = DB_ID('AnimalDB')";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"      Bestandslocatie: {reader.GetString(0)}");
                        Console.ResetColor();
                    }
                }
            }
            catch { }
        }
    }
}

