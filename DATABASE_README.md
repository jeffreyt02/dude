# SQL Database Connectie - ConsoleApp2

## 📋 Wat je nodig hebt

1. **Microsoft SQL Server** geïnstalleerd (bijvoorbeeld SQL Server Express)
2. **SQL Server Management Studio (SSMS)** (optioneel, maar handig)

## 🚀 Stappen om te connecten met je SQL database

### Stap 1: Database aanmaken

Er zijn twee manieren om de database aan te maken:

#### Optie A: Via SQL Server Management Studio (SSMS)
1. Open **SQL Server Management Studio**
2. Connect met je SQL Server instance
3. Open het bestand `CreateDatabase.sql`
4. Klik op **Execute** (of druk op F5)
5. De database `AnimalDB` en tabel `Animals` worden aangemaakt

#### Optie B: Via Command Line
```powershell
sqlcmd -S localhost\SQLEXPRESS -i "CreateDatabase.sql"
```

### Stap 2: Connection String aanpassen in Program.cs

Open `Program.cs` en pas de volgende regels aan (regel ~48):

```csharp
string server = "localhost\\SQLEXPRESS";  // Jouw SQL Server instance
string database = "AnimalDB";              // Database naam
```

**Veelgebruikte SQL Server instances:**
- `localhost\SQLEXPRESS` - SQL Server Express
- `(localdb)\MSSQLLocalDB` - LocalDB
- `localhost` of `.` - Default instance
- `jouw-computer-naam\SQLEXPRESS` - Remote instance

### Stap 3: Kies authenticatie methode

#### Windows Authentication (Aanbevolen voor lokaal)
```csharp
DatabaseHelper db = new DatabaseHelper(server, database, integratedSecurity: true);
```

#### SQL Server Authentication
```csharp
DatabaseHelper db = new DatabaseHelper(server, database, integratedSecurity: false, 
                                       username: "sa", password: "jouwwachtwoord");
```

### Stap 4: Run de applicatie

```powershell
dotnet run --project ConsoleApp2/ConsoleApp2.csproj
```

## 📦 Database Schema

### Animals Tabel
| Kolom | Type | Beschrijving |
|-------|------|--------------|
| AnimalID | INT | Primary Key (Auto-increment) |
| Name | NVARCHAR(100) | Naam van het dier |
| Age | INT | Leeftijd |
| Weight | FLOAT | Gewicht in kg |
| Height | FLOAT | Hoogte in meters |
| CreatedDate | DATETIME | Aanmaakdatum |

## 🛠️ DatabaseHelper Methodes

### TestConnection()
Test of de connectie met de database werkt.

```csharp
db.TestConnection();
```

### InsertAnimal(Animal animal)
Voeg een nieuw Animal toe aan de database.

```csharp
Animal mario = new Animal("Mario", 80, 40) { Age = 5 };
db.InsertAnimal(mario);
```

### GetAllAnimals()
Haal alle animals op uit de database.

```csharp
List<Animal> animals = db.GetAllAnimals();
foreach (var animal in animals)
{
    Console.WriteLine(animal);
}
```

### ExecuteReader(string query)
Voer een SELECT query uit en print de resultaten.

```csharp
db.ExecuteReader("SELECT * FROM Animals WHERE Age > 5");
```

### ExecuteNonQuery(string query)
Voer een INSERT, UPDATE of DELETE query uit.

```csharp
db.ExecuteNonQuery("UPDATE Animals SET Age = 6 WHERE Name = 'Mario'");
```

## ❗ Veelvoorkomende Problemen

### "Cannot open database 'AnimalDB'"
- Zorg dat je eerst het `CreateDatabase.sql` script hebt uitgevoerd
- Check of je de juiste server name gebruikt

### "Login failed for user"
- Check of je de juiste authenticatie methode gebruikt
- Voor SQL Authentication: zorg dat de user bestaat en het wachtwoord klopt
- Voor Windows Authentication: zorg dat je Windows user rechten heeft

### "A network-related or instance-specific error"
- Check of SQL Server draait: open **SQL Server Configuration Manager**
- Check of TCP/IP enabled is in SQL Server Configuration
- Probeer een andere server name (zie Stap 2)

### TrustServerCertificate error
- Dit is al opgelost in de connection string met `TrustServerCertificate=true;`

## 📚 Meer Informatie

**Connection String formaten:**
```
Windows Auth: Server=localhost\SQLEXPRESS;Database=AnimalDB;Integrated Security=true;TrustServerCertificate=true;
SQL Auth:     Server=localhost\SQLEXPRESS;Database=AnimalDB;User Id=sa;Password=yourpassword;TrustServerCertificate=true;
```

**NuGet Package:**
- Microsoft.Data.SqlClient (versie 6.1.3) - Al geïnstalleerd! ✓

## 🎯 Voorbeeld Output

```
==================================================
DATABASE CONNECTIE
==================================================

1. Connectie testen...
✓ Connectie met database succesvol!

2. Animal toevoegen aan database...
✓ Animal 'Mario' toegevoegd aan database.

3. Alle animals ophalen...
✓ 4 animal(s) opgehaald uit database.
AnimalID: 1, Name: Bowser, Weight: 150.5, Height: 50.2
AnimalID: 2, Name: Mario, Weight: 80.3, Height: 40.1
AnimalID: 3, Name: Luigi, Weight: 75.2, Height: 38.5
AnimalID: 4, Name: Mario, Weight: 80, Height: 40
```

Good luck! 🚀

