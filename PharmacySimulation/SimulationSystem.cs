using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PharmacySimulation
{
    public class SimulationSystem
    {
        private List<Pharmacist> listPharmacist;
        private Queue<Customer> tempCustomersInQueue;
        private Queue<Customer> customersInQueue;
        private List<Customer> customersHasBeenServed;
        private RandomNumbersGenerator randomNumbersGenerator;
        private int lastDepartureTime = 0;
        private readonly int NUMBER_OF_CUSTOMERS = 15;
        private readonly int NUMBER_OF_PHARMACHIST = 6;
        private Boolean resultHasBeenPrinted = true;
        public Timer Timer { get; set; }
        public int CurrentTime { get; set; }

        public SimulationSystem()
        {
            randomNumbersGenerator = new RandomNumbersGenerator();
            listPharmacist = new List<Pharmacist>();
            tempCustomersInQueue = new Queue<Customer>();
            customersInQueue = new Queue<Customer>();
            customersHasBeenServed = new List<Customer>();
            Timer = new Timer();
            Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Timer.Interval = 1;
            createCustomers(NUMBER_OF_CUSTOMERS);
            createPharmacist(NUMBER_OF_PHARMACHIST);
            checkCustomers();
        }

        public List<Customer> getCustomersHasBeenServed()
        {
            return customersHasBeenServed;
        }

        private void createPharmacist(int numberOfPharmacists)
        {
            for (int i = 0; i < numberOfPharmacists; i++)
            {
                listPharmacist.Add(new Pharmacist());
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            isCustomerArriveNow();
            serveIfPharmacistIdle();
            leaveIfDoneAndServeAnotherCustomer();

            //MainWindow.main.tableRefresh();
            //MainWindow.main.Data = customersHasBeenServed;
            MainWindow.main.Time = "Time : " + CurrentTime++.ToString();
            Console.WriteLine("t : " + CurrentTime);

            if (customersHasBeenServed.Count == NUMBER_OF_CUSTOMERS)
            {
                Timer.Stop();
                Timer.Dispose();
                Timer.Close();
                if (resultHasBeenPrinted)
                {
                    countAvarage();
                    resultHasBeenPrinted = false;
                }
            }
        }

        private void isCustomerArriveNow()
        {
            if (tempCustomersInQueue.Count > 0)
            {
                if (CurrentTime == tempCustomersInQueue.First().arrivalTime)
                {
                    Console.WriteLine("Time : " + CurrentTime + " -> Customer " + tempCustomersInQueue.First().id + " arrive");
                    MainWindow.main.StatusTextBlock = "Time : " + CurrentTime + " -> Customer " + tempCustomersInQueue.First().id + " arrive";
                    customersInQueue.Enqueue(tempCustomersInQueue.First());
                    tempCustomersInQueue.Dequeue();
                }
            }
        }

        private void leaveIfDoneAndServeAnotherCustomer()
        {
            foreach (Pharmacist pharmacist in listPharmacist)
            {
                if (!pharmacist.isIdle)
                {
                    if (pharmacist.currentCustomer.departureTime == CurrentTime)
                    {
                        customersHasBeenServed.Add(pharmacist.currentCustomer);
                        pharmacist.currentCustomer.departureTime = CurrentTime;
                        if (lastDepartureTime < CurrentTime)
                        {
                            pharmacist.currentCustomer.systemIdleTime = CurrentTime - lastDepartureTime;
                        }
                        else
                        {
                            pharmacist.currentCustomer.systemIdleTime = 0;
                        }
                        Console.WriteLine("Time : " + CurrentTime + " -> Customer " + pharmacist.currentCustomer.id + " leave");
                        MainWindow.main.StatusTextBlock = "Time : " + CurrentTime + " -> Customer " + pharmacist.currentCustomer.id + " leave";
                        pharmacist.isIdle = true;
                        pharmacist.currentCustomer = null;
                        pharmacist.serviceTime = 0;
                        serveIfPharmacistIdle();

                        MainWindow.main.Data = customersHasBeenServed;
                        MainWindow.main.tableRefresh();
                    }
                }
            }
        }
        private void serveIfPharmacistIdle()
        {
            if (customersInQueue.Count > 0)
            {
                foreach (Pharmacist pharmacist in listPharmacist)
                {
                    if (pharmacist.isIdle)
                    {
                        Customer currentCustomer = customersInQueue.First();
                        customersInQueue.Dequeue();
                        currentCustomer.beginServiceTime = CurrentTime;
                        pharmacist.serviceTime = randomNumbersGenerator.getRandomServiceTime(currentCustomer.typeOfRecipe);
                        pharmacist.serve(currentCustomer);
                        Console.WriteLine("Time : " + CurrentTime + " -> Serving customer " + currentCustomer.id);
                        MainWindow.main.StatusTextBlock = "Time : " + CurrentTime + " -> Serving customer " + currentCustomer.id;
                        currentCustomer.print();
                        break;
                    }
                }
            }
        }

        private void createCustomers(int numberOfClients)
        {
            if (numberOfClients > 0)
            {
                Customer newCustomer = new Customer();
                newCustomer.id = 1;
                newCustomer.arrivalTime = randomNumbersGenerator.getRandomArrivalTime();
                newCustomer.typeOfRecipe = randomNumbersGenerator.getRandomTypeOfRecipe();
                tempCustomersInQueue.Enqueue(newCustomer);

                for (int i = 1; i < numberOfClients; i++)
                {
                    int lastCustomerArrivalTime = tempCustomersInQueue.Last().arrivalTime;
                    newCustomer = new Customer();
                    newCustomer.id = i + 1;
                    newCustomer.arrivalTime = lastCustomerArrivalTime + randomNumbersGenerator.getRandomArrivalTime();
                    newCustomer.typeOfRecipe = randomNumbersGenerator.getRandomTypeOfRecipe();
                    tempCustomersInQueue.Enqueue(newCustomer);
                }
            }
        }

        private void checkCustomers()
        {
            Console.WriteLine("Customers Arrival Time");
            foreach (Customer customer in customersHasBeenServed)
            {
                customer.print();
                Console.WriteLine();
            }
            Console.WriteLine("----------------------");
        }

        public void countAvarage()
        {
            double totalTimeInQueue = 0;
            double totalTimeInSystem = 0;
            double totalSystemIdle = 0;

            foreach (Customer customer in customersHasBeenServed)
            {
                totalTimeInQueue += customer.timeInQueue;
                totalTimeInSystem += customer.timeInSystem;
                totalSystemIdle += customer.systemIdleTime;
            }

            double avgTimeInQueue = totalTimeInQueue / NUMBER_OF_CUSTOMERS;
            double avgTimeInSystem = totalTimeInSystem / NUMBER_OF_CUSTOMERS;
            double avgSystemIdle = totalSystemIdle / NUMBER_OF_CUSTOMERS;

            Console.WriteLine("Total Time In Queue  = " + totalTimeInQueue);
            Console.WriteLine("Total Time In System = " + totalTimeInSystem);
            Console.WriteLine("Total System Idle    = " + totalSystemIdle);

            Console.WriteLine("Avarage Time In Queue  = " + avgTimeInQueue);
            Console.WriteLine("Avarage Time In System = " + avgTimeInSystem);
            Console.WriteLine("Avarage System Idle    = " + avgSystemIdle);

            MainWindow.main.StatusTextBlock = "Total Time In Queue  = " + totalTimeInQueue;
            MainWindow.main.StatusTextBlock = "Total Time In System = " + totalTimeInSystem;
            MainWindow.main.StatusTextBlock = "Total System Idle    = " + totalSystemIdle;

            MainWindow.main.StatusTextBlock = "Avarage Time In Queue  = " + avgTimeInQueue;
            MainWindow.main.StatusTextBlock = "Avarage Time In System = " + avgTimeInSystem;
            MainWindow.main.StatusTextBlock = "Avarage System Idle    = " + avgSystemIdle;
        }

    }
}
