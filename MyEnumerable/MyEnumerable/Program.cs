using System;
using System.Collections;
using System.Collections.Generic;

namespace MyEnumerable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var myEnumerable = new MyEnumerable();
            myEnumerable.Add("chen");
            myEnumerable.Add("qiang");
            myEnumerable.Add("dennis");

            foreach (var item in myEnumerable)
            {
                Console.WriteLine($"value => {(item as Order).Name}");
            }

            var asResult = MyCast(myEnumerable);

            foreach (var item in asResult)
            {
                Console.WriteLine($"value1 => {item.Name}");
            }

            Console.ReadKey();
        }


        static IEnumerable<Order> MyCast(MyEnumerable orders)
        {
            IEnumerable<Order> typedSource = orders as IEnumerable<Order>;
            if (typedSource != null)
                return typedSource;

            if (orders == null)
                throw new Exception("orders == null");

            return CastIterator<Order>(orders);
        }


        static IEnumerable<TResult> CastIterator<TResult>(IEnumerable source)
        {
            foreach (object obj in source)
            {
                yield return (TResult)obj;
            }
        }
    }

    class MyEnumerable : IEnumerable
    {
        Order[] orders = new Order[3];
        private int currentIndex = 0;

        public void Add(string name)
        {
            orders[this.currentIndex] = new Order(name);
            this.currentIndex++;
        }

        public IEnumerator GetEnumerator()
        {
            return new myEnumerator(this.orders);

            //foreach (var item in this.orders)
            //{
            //    yield return item;
            //}
        }
    }

    class myEnumerator : IEnumerator
    {
        Order[] orders;
        public myEnumerator(Order[] orders)
        {
            this.orders = orders;
        }

        private int current = -1;

        object IEnumerator.Current => this.GetCurrent();

        public object GetCurrent()
        {
            if(this.current < 0 || this.current >= this.orders.Length)
            {
                return null;
            }

            return this.orders[current];
        }

        public bool MoveNext()
        {
            this.current++;
            if(this.current < this.orders.Length && this.orders[this.current] != null)
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.current = 0;
        }
    }

    class Order
    {
        public Order(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
