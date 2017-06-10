using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySimulation
{
    public class Customer
    {
        public int id {set; get;}
        public int arrivalTime { set; get; }
        public int beginServiceTime { set; get; }
        public int serviceTime { set; get; }
        public int departureTime { set; get; }
        public int typeOfRecipe { set; get; }
        public int systemIdleTime { set; get; }
        public int timeInQueue
        {
            get
            {
                return beginServiceTime-arrivalTime;
            }
        }
        public int timeInSystem {
            get 
            {
                return departureTime - arrivalTime;
            }
        }

        public Customer(int id, int arrivalTime, int typeOfRecipe)
        {
            this.id = id;
            this.arrivalTime = arrivalTime;
            this.typeOfRecipe = typeOfRecipe;
        }

        public Customer()
        {

        }

        public void print()
        {
            Console.WriteLine("Customer " + id);
            Console.WriteLine("Type of Recipe       = " + typeOfRecipe);
            Console.WriteLine("Arrival Time         = " + arrivalTime);
            Console.WriteLine("Begin Service Time   = " + beginServiceTime);
            Console.WriteLine("Service Time         = " + serviceTime);
            Console.WriteLine("Departure Time       = " + departureTime);
            Console.WriteLine("Time in Queue        = " + timeInQueue);
            Console.WriteLine("Time in System       = " + timeInSystem);
        }

        public void beginService(int serviceTime)
        {
            this.serviceTime = serviceTime;
            departureTime = beginServiceTime + serviceTime;
        }

    }
}
