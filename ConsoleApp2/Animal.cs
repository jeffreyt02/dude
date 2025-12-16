// See https://aka.ms/new-console-template for more information

namespace ConsoleApp2;

public class Animal
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public int AnimalID { get; set; }
    
    public Animal()
    {
    }
    
    public Animal(string name = "No Name",
        double weight = 0,
        double height = 0)
    {
        Name = name; 
        Weight = weight;
        Height = height;
    }

    public override string ToString()
    {
        return string.Format("AnimalID: {0}, Name: {1}, Weight: {2}, Height: {3}", AnimalID, Name, Weight, Height);
    }

}