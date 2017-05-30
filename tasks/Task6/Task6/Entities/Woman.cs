using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static System.Console;
using Task6.Enums;
using Task6.Interfaces;

namespace Task6.Entities
{
    public class Woman : IPerson, ISubject<Man, Woman>
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
        /// Creates a new woman object - which is really, really wrong...
        /// </summary>
        /// <param name="firstName">The first name of the woman</param>
        /// <param name="lastName">The last name of the woman</param>
        public Woman(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            
            Salutation = Salutation.Sir;
        }
        
        public void Talk()
        {
            WriteLine($"{Salutation} {Name}: Nag nag nag ...");
        }

        public void Talk(string to)
        {
            WriteLine($"{FirstName} to {to}: Nag nag nag ...");
        }

        public void PrintAge()
        {
            WriteLine($"{Name} is currently {Age} {(Age > 1 ? "years" : "year")} old.");
            WriteLine();
        }

        #region IObserver<Man>
        public void OnNext(Man value)
        {
            Talk(value.FirstName);
        }

        public void OnError(Exception error)
        {
            WriteLine("An error occured and killed the woman.");
        }

        public void OnCompleted()
        {
            WriteLine("I am going shopping.");
        }

        public IDisposable Subscribe(IObserver<Woman> observer)
        {
            return Observable.Interval(TimeSpan.FromSeconds(3))
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
