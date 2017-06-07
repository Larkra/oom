using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Task6.Entities;
using Task6.Extensions;

namespace Task6.Factories
{
    /*
     * An adaption of
     * https://msdn.microsoft.com/en-us/library/hh229114(v=vs.103).aspx
     *          
     */
    public class ChildFactory : IDisposable
    {
        private bool _additionToTheFamily = true;

        #region FirstNames
        private static IEnumerable<string> FirstNames => new[]
        {
            "Adam",
            "Julia",
            "Tayel",
            "Eva",
            "Rainhard",
            "Raphael",
            "Lillith",
            "Rincewind",
            "Magda",
            "Harald",
            "Cohen",
            "Hannes",
            "John",
            "Mathias",
            "Emanuel",
            "Jack",
            "Carlos",
            "Stefan",
            "Sebastian",
            "Sarah",
            "Claudia",
            "Daniel",
            "Gabriel",
            "Serenity"
        };
        #endregion

        internal ChildFactory(object childObserver)
        {
            Task.Factory.StartNew(ChildGenerator, childObserver);
        }

        public void Dispose()
        {
            _additionToTheFamily = false;
        }

        private void ChildGenerator(object observer)
        {
            var childObserver = (IObserver<Child>)observer;

            while (_additionToTheFamily)
            {
                var randomFirstName = FirstNames.RandomElement(new Random());
                
                childObserver.OnNext(new Child(randomFirstName, "Doe"));
                Thread.Sleep(2000);
            }
        }

        public static IDisposable ChildSubscribe(object childObserver)
        {
            return new ChildFactory(childObserver);
        }
        
    }
    
}
