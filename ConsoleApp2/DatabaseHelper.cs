using Microsoft.Data.SqlClient;

namespace ConsoleApp2;

public class DatabaseHelper
{
    private readonly string _connectionString;

    // Constructor met connection string
    public DatabaseHelper(string server, string database, bool integratedSecurity = true, string? username = null, string? password = null)
    {
        if (integratedSecurity)
        {
            // Windows Authentication
            _connectionString = $"Server={server};Database={database};Integrated Security=true;TrustServerCertificate=true;";
        }
        else
        {
            // SQL Server Authentication
            _connectionString = $"Server={server};Database={database};User Id={username};Password={password};TrustServerCertificate=true;";
        }
    }

    // Test de connectie
    public bool TestConnection()
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("✓ Connectie met database succesvol!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Fout bij connectie: {ex.Message}");
            return false;
        }
    }

    // Voer een query uit die data retourneert (SELECT)
    public void ExecuteReader(string query)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            
            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            
            // Print kolomnamen
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 50));
            
            // Print rijen
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + "\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Voer een query uit die data wijzigt (INSERT, UPDATE, DELETE)
    public int ExecuteNonQuery(string query)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            
            using SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            
            Console.WriteLine($"{rowsAffected} rij(en) beïnvloed.");
            return rowsAffected;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }

    // Insert een Animal object in de database
    public void InsertAnimal(Animal animal)
    {
        string query = @"INSERT INTO Animals (Name, Age, Weight, Height) 
                        VALUES (@Name, @Age, @Weight, @Height)";
        
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", animal.Name);
            command.Parameters.AddWithValue("@Age", animal.Age);
            command.Parameters.AddWithValue("@Weight", animal.Weight);
            command.Parameters.AddWithValue("@Height", animal.Height);
            
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"✓ Animal '{animal.Name}' toegevoegd aan database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Fout bij toevoegen animal: {ex.Message}");
        }
    }

    // Haal alle Animals op uit de database
    public List<Animal> GetAllAnimals()
    {
        List<Animal> animals = new List<Animal>();
        string query = "SELECT AnimalID, Name, Age, Weight, Height FROM Animals";
        
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            
            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Animal animal = new Animal
                {
                    AnimalID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    Weight = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    Height = reader.IsDBNull(4) ? 0 : reader.GetDouble(4)
                };
                animals.Add(animal);
            }
            
            Console.WriteLine($"✓ {animals.Count} animal(s) opgehaald uit database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Fout bij ophalen animals: {ex.Message}");
        }
        
        return animals;
    }
}

