using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static System.Console;
using Task6.Enums;
using Task6.Interfaces;

namespace Task6.Entities
{
    public class Man : IPerson, ISubject<Woman, Man>
    {
        private string _firstName;
        private string _lastName;
        
        #region properties
        public string FirstName
        {
            get { return _firstName; }
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
        
        public Salutation Salutation { get; }

        public string Name => string.Concat(FirstName, ' ', LastName);
        #endregion
        
        /// <summary>
        /// Creates a new man object - which may be wrong, but nobody really cares...
        /// </summary>
        /// <param name="firstName">The first name of the man</param>
        /// <param name="lastName">The last name of the man</param>
        public Man(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            
            Salutation = Salutation.Sir;
        }

        public void Talk()
        {
            WriteLine($"{Salutation} {Name}: ...");
        }

        public void Talk(string to)
        {
            WriteLine($"{FirstName} to {to}: ...");
        }

        public void PrintAge()
        {
            WriteLine($"{Name} is currently {Age} {(Age > 1 ? "years" : "year")} old.");
            WriteLine();
        }
        
        #region IObserver<Man>
        public void OnNext(Woman value)
        {
            Talk(value.FirstName);
        }

        public void OnError(Exception error)
        {
            WriteLine("An error occured and killed the man.");
        }

        public void OnCompleted()
        {
            WriteLine("I am going to the pub.");
        }

        public IDisposable Subscribe(IObserver<Man> observer)
        {
            return Observable.Interval(TimeSpan.FromSeconds(2))
                .Where(n => n < 10)
                .Select(n => this)
                .Subscribe(observer);
        }
        #endregion

        public void Dispose()
        {
            OnCompleted();
        }

    }

}
