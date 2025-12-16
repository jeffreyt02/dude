-- ============================================
-- SQL Script om de AnimalDB database en Animals tabel aan te maken
-- ============================================

-- Maak de database aan (als deze nog niet bestaat)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AnimalDB')
BEGIN
    CREATE DATABASE AnimalDB;
    PRINT 'Database AnimalDB aangemaakt.';
END
ELSE
BEGIN
    PRINT 'Database AnimalDB bestaat al.';
END
GO

-- Gebruik de AnimalDB database
USE AnimalDB;
GO

-- Maak de Animals tabel aan
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Animals]') AND type in (N'U'))
BEGIN
    CREATE TABLE Animals (
        AnimalID INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Age INT NULL,
        Weight FLOAT NULL,
        Height FLOAT NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Tabel Animals aangemaakt.';
END
ELSE
BEGIN
    PRINT 'Tabel Animals bestaat al.';
END
GO

-- Voeg wat testdata toe (optioneel)
IF NOT EXISTS (SELECT * FROM Animals WHERE Name = 'Bowser')
BEGIN
    INSERT INTO Animals (Name, Age, Weight, Height) 
    VALUES 
        ('Bowser', 10, 150.5, 50.2),
        ('Mario', 5, 80.3, 40.1),
        ('Luigi', 4, 75.2, 38.5);
    PRINT 'Test data toegevoegd.';
END
GO

-- Selecteer alle animals om te controleren
SELECT * FROM Animals;
GO

