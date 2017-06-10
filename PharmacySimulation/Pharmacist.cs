using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySimulation
{
    class Pharmacist
    {
        public Boolean isIdle { get; set; }
        public int serviceTime { get; set; }
        public Customer currentCustomer { get; set; }

        public Pharmacist()
        {
            this.isIdle = true;
            this.serviceTime = 0;
        }

        public void serve(Customer customer)
        {
            this.currentCustomer = customer;
            this.isIdle = false;
            customer.beginService(serviceTime);
        }

    }
}
