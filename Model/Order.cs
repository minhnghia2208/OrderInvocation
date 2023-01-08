using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderInvocation.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public void toString()
        {
            Console.WriteLine("ID: " + Id);
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Amount: " + Amount);
        }
    }
}
