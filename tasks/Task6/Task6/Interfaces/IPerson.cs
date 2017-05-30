namespace Task6.Interfaces
{
    public interface IPerson
    {
        int Age { get; set; }
        string FirstName { get; }
        string LastName { get; }
        
        void Talk();
        void PrintAge();
    }
}
