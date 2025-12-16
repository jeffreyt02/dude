// See https://aka.ms/new-console-template for more information


using System.Runtime.Serialization;

[Serializable()]

public class Animal : ISerializable
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public int AnimalID { get; set; }
    public Animal()
    {
    }
    public Animal(string name="Mo Name",
        double weight= 0,
        double  height= 0)
    {
        Name = name; 
        Weight = weight;
        Height = height;
    }

    public override string ToString()
    {
       return string.Format("AnimalID: {0}, Name: {1}, Weight: {2}, Height: {3}", AnimalID, Name, Weight, Height);
    }


    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
       info.AddValue("Name", Name);
         info.AddValue("Age", Age);
            info.AddValue("Weight", Weight);
                info.AddValue("Height", Height);
                    info.AddValue("AnimalID", AnimalID);
    }


}