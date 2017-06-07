using System;
using static System.Console;
using Task6.Interfaces;

namespace Task6.Entities
{
    public class Child : IPerson
    {
        private string _firstName;
        private string _lastName;

        #region properties
        public string FirstName
        {
            get {return _firstName;}
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name must not be null or whitespace.", nameof(value));
                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name must not be null or whitespace.", nameof(value));
                _lastName = value;
            }
        }

        public int Age { get; set; } = 0;
        
        public string Name => string.Concat(FirstName, ' ', LastName);

        #endregion

        /// <summary>
        /// Creates a new child object - Finally, this seems to be political correct...
        /// </summary>
        /// <param name="firstName">The first name of the child</param>
        /// <param name="lastName">The last name of the child</param>
        public Child(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        
        public void Talk()
        {
            WriteLine($"{Name}: Gugu gaga ...");
        }

        public void PrintAge()
        {
            WriteLine($"{Name} is currently {Age} {(Age > 1 ? "years" : "year")} old.");
            WriteLine();
        }

    }
}
