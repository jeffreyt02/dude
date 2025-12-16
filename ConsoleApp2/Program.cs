using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // STAP 1: Zoek eerst waar SQL Server en AnimalDB zich bevinden
            DatabaseLocator.FindDatabase();
            
            Animal bowser = new Animal("Bowser", 150, 50);
            
            // Serialiseer naar JSON en sla op
            string jsonString = JsonSerializer.Serialize(bowser);
            File.WriteAllText("AnimalData.json", jsonString);
            
            Console.WriteLine("Animal geserialiseerd naar JSON:");
            Console.WriteLine(jsonString);

            bowser = null;
            
            // Deserialiseer van JSON
            string jsonFromFile = File.ReadAllText("AnimalData.json");
            bowser = JsonSerializer.Deserialize<Animal>(jsonFromFile);
            
            Console.WriteLine("\nAnimal gedeserialiseerd:");
            Console.WriteLine(bowser);
            
            // XML Serialization
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Animal));
            using (TextWriter tw = new StreamWriter("bowser.xml"))
            {
                xmlSerializer.Serialize(tw, bowser);
            }
            
            Console.WriteLine("\nAnimal geserialiseerd naar XML: bowser.xml");
            Console.WriteLine("Pad: " + Path.GetFullPath("bowser.xml"));
            
            // ============================================
            // DATABASE CONNECTIE VOORBEELD
            // ============================================
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("DATABASE CONNECTIE");
            Console.WriteLine(new string('=', 50));
            
            // Maak een DatabaseHelper aan met jouw SQL Server gegevens
            // GEVONDEN: Jouw SQL Server instance is (localdb)\MSSQLLocalDB
            string server = "(localdb)\\MSSQLLocalDB";  // LocalDB instance
            string database = "AnimalDB";                // Jouw database naam
            
            // Optie 1: Windows Authentication (aanbevolen voor lokaal)
            DatabaseHelper db = new DatabaseHelper(server, database, integratedSecurity: true);
            
            // Optie 2: SQL Server Authentication (comment de regel hierboven uit en uncomment deze)
            // DatabaseHelper db = new DatabaseHelper(server, database, integratedSecurity: false, username: "sa", password: "jouwwachtwoord");
            
            // Test de connectie
            Console.WriteLine("\n1. Connectie testen...");
            db.TestConnection();
            
            // Insert een animal in de database
            Console.WriteLine("\n2. Animal toevoegen aan database...");
            Animal mario = new Animal("Mario", 80, 40) { Age = 5 };
            db.InsertAnimal(mario);
            
            // Haal alle animals op
            Console.WriteLine("\n3. Alle animals ophalen...");
            List<Animal> animals = db.GetAllAnimals();
            foreach (var animal in animals)
            {
                Console.WriteLine(animal);
            }
            
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("Druk op een toets om af te sluiten...");
            Console.ReadKey();
        }
    }
}

