using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using static System.Console;
using Task6.Entities;

namespace Task6
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunPushConversation();
            //RunAsTimeGoesBy();
            RunReturnObjects();
        }

        #region Ping Pong Actor Model with ISubject<T1, T2>
        private static void RunPushConversation()
        {
            var husband = new Man("John", "Doe");
            var wife = new Woman("Jane", "Doe");

            WriteLine("Press any key to stop the conversation ...");
            WriteLine();

            var manSubscription = wife.Subscribe(husband);
            var womanSubscription = husband.Subscribe(wife);

            ReadKey();

            manSubscription.Dispose();
            womanSubscription.Dispose();

            WriteLine();
            WriteLine("The conversation is over.");
            WriteLine();
        }
        #endregion Ping Pong Actor Model with ISubject<T1, T2>

        #region Filter and Select - Simple
        private static void RunAsTimeGoesBy()
        {
            WriteLine("As time goes by ... Press any key to stop aging ...");
            WriteLine();

            var man = new Man("John", "Doe");
            WriteLine($"{man.Name} is born.");
            man.Age++;
            WriteLine("... A year has passed ...");
            man.PrintAge();

            var oneYearPerSecond = Observable.Interval(TimeSpan.FromSeconds(1));

            var agingByInterval = oneYearPerSecond
                                        .Where(n => n > 1)
                                        .Select(n => man.Age = (int)n);

            agingByInterval.Subscribe(age =>
            {
                WriteLine("... Another year has passed ...");
                man.PrintAge();
            });

            ReadKey();
        }
        #endregion

        private static void RunReturnObjects()
        {
            WriteLine("Addition to the family ... Press any key to stop it ...");
            WriteLine();

            var childrenCount = Children.Count();
            var producer = new Subject<Child>();

            var sub = producer.Subscribe(ch => ch.Talk());

            for (var i = 0; i < childrenCount; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                producer.OnNext(Children[i]);
            }

            ReadKey();
            producer.OnCompleted();

            var c = NonBlocking();
            var subscription = c.Subscribe(ch => ch.Talk());
            
            ReadKey();
            subscription.Dispose();
            sub.Dispose();
        }

        private static IObservable<Child> NonBlocking()
        {
            return Observable.Create<Child>(
                (IObserver<Child> observer) =>
            {
                observer.OnNext(new Child("Lillith", "Doe"));
                observer.OnNext(new Child("Adam", "Doe"));
                observer.OnNext(new Child("Eva", "Doe"));
                observer.OnCompleted();
                Thread.Sleep(3000);
                return Disposable.Create(() => WriteLine("Observer has unsubcribed"));
            });
        }

        private static Child[] Children => new[]
        {
            new Child("Adam", "Doe"),
            new Child("Eva", "Doe"),
            new Child("Lillith", "Doe"),
            new Child("Rincewind", "Doe"),
            new Child("Cohen", "Doe"),
            new Child("John", "Skeet"),
            new Child("Jack", "Doe"),
            new Child("Sebastian", "Doe"),
            new Child("Sarah", "Doe"),
            new Child("Serenity", "Doe")
        };
    }
}
