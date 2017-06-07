using System;
using System.Reactive.Linq;
using static System.Console;
using Task6.Entities;
using Task6.Factories;

namespace Task6
{
    class Program
    {
        static void Main(string[] args)
        {
            IntroduceMethod(nameof(RunPushConversation));
            RunPushConversation();

            IntroduceMethod(nameof(RunAsTimeGoesBy));
            RunAsTimeGoesBy();

            IntroduceMethod(nameof(RunCreateChildren));
            RunCreateChildren();
        }

        #region Ping Pong Actor Model with ISubject<T1, T2>
        private static void RunPushConversation()
        {
            var husband = new Man("John", "Doe");
            var wife = new Woman("Jane", "Doe");

            using (var manSubscription = wife.Subscribe(husband))
            {
                using (var womanSubscription = husband.Subscribe(wife))
                {
                    WriteLine("Press any key to stop the conversation ...");
                    WriteLine();
                    ReadKey();
                }
            }

            WriteLine();
            WriteLine("The conversation is over.");
            WriteLine();
        }
        #endregion Ping Pong Actor Model with ISubject<T1, T2>

        #region Filter and Select - Simple
        private static void RunAsTimeGoesBy()
        {
            WriteLine("As time goes by ...");
            WriteLine();

            var man = new Man("John", "Doe");
            WriteLine($"{man.Name} is born.");
            man.Age++;
            WriteLine("... A year has passed ...");
            man.PrintAge();

            var oneYearPerSecond = Observable.Interval(TimeSpan.FromSeconds(1));

            var agingByInterval = oneYearPerSecond
                                        .Where(n => n > 1 && n < 100)
                                        .Select(n => man.Age = (int)n);

            using (var handle = agingByInterval.Subscribe(age =>
            {
                WriteLine("... Another year has passed ...");
                man.PrintAge();
            }))
            {
                WriteLine("Press any key to stop aging ...");
                WriteLine();
                ReadKey();
            }
        }
        #endregion

        #region Push objects with Observable Create
        private static void RunCreateChildren()
        {
            WriteLine("Addition to the family ...");
            WriteLine();

            var childObservable = Observable.Create((Func<IObserver<Child>, IDisposable>)ChildFactory.ChildSubscribe);

            using (var handle = childObservable.Subscribe(child => WriteLine($"{child.Name} is born ...")))
            {
                WriteLine("Press any key to stop ...");
                WriteLine();
                ReadKey();
            }
        }
        #endregion

        private static void IntroduceMethod(string methodName)
        {
            WriteLine($"Press any key to run {methodName}");
            ReadKey();
            Clear();
        }

    }
}
