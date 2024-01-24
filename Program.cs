
using System.Data;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;

namespace readytogo;
class Program
{
    // you MUST fill in your anme(s) and student number(s) here
    private static readonly string studentname1 = "Thom Veldhuis";
    private static readonly string studentnum1 = "1055805";
    private static readonly string studentname2 = "Maruf Rodjan";
    private static readonly string studentnum2 = "1052505";

    // do not add more variables after this comment.
    // feel free to change the values of the variables below to test your code
    private static readonly int total_clients = 500; // this needs to be the same as the number of cooks
    private static int total_coocks = 500; // this needs to be the same as the number of clients

    // variables for concurrency?
    // add the variables you need for concurrency here in case of need
    public static Semaphore cook_sem = new Semaphore(0, total_coocks);
    public static Semaphore client_sem = new Semaphore(0, total_clients);
    // twee mutexes voor de orders en pickup omdat het 2 verschillende acties zijn voor de threads
    public static readonly Mutex orderMutex = new Mutex();
    public static readonly Mutex pickupMutex = new Mutex();

    // do not change the code below
    public static LinkedList<Order> orders = new();
    public static LinkedList<Order> pickups = new();

    private static readonly Client[] clients = new Client[total_clients];
    private static readonly Cook[] cooks = new Cook[total_coocks];

    static void Main() //this method is not working properly
    {
        Console.WriteLine("Hello, we are starting our new pickup restaurant!");
        // the following code will create the clients and cooks DO NOT CHANGE THIS CODE
        // create many threads as clients,
        CreateClients();
        // create many coocks that can prepare only one dish per time
        CreateCooks();
        // each cook thread will start
        StartCooks();
        // each client thead will start 
        StartClients();
        // DO NOT CHANGE THE CODE ABOVE
        // use the space below to add your code if needed

        foreach(var _k in cooks) {
            _k.Thread?.Join();
        }

        foreach(var _c in clients) {
            _c.Thread?.Join();
        }

        // DO NOT CHANGE THE CODE BELOW
        // print the number of orders placed and the number of orders consumed left in the lists
        Console.WriteLine("Orders left to work: " + orders.Count);
        Console.WriteLine("Orders left and not picked up: " + pickups.Count);
        // the lists should be empty
        Console.WriteLine("Name: " + studentname1 + " Student number: " + studentnum1);
        Console.WriteLine("Name: " + studentname2 + " Student number: " + studentnum2);
        //Console.WriteLine("Press any key to exit");
        //Console.ReadKey(); // this lines can be used to stop the program from exiting
    }

    private static void StartCooks() // this method is not working properly
    {   // feel free to change the code in this method if needed
        for (int i = 0; i < cooks.Length; i++)
        {
            cooks[i].Thread?.Start();
        }
    }

    private static void StartClients() // this method is not working properly
    {   // feel free to change the code in this method if needed
        for (int i = 0; i < clients.Length; i++)
        {
            clients[i].Thread?.Start();
        }
    }

    private static void CreateCooks()
    {   // feel free to change the code in this method if needed but not the signature
        for (int i = 0; i < total_coocks; i++)
        {
            cooks[i] = new Cook(i); // Properly initialize Cook instance with required arguments
            cooks[i].Thread = new Thread(cooks[i].DoWork);
        }
    }

    private static void CreateClients()
    {   // feel free to change the code in this method if needed but not the signature
        for (int i = 0; i < total_clients; i++)
        {
            clients[i] = new Client(i); // Properly initialize Client instance with required arguments
            clients[i].Thread = new Thread(clients[i].DoWork);
        }
    }
}

public class Order  //do not change this class
{
    private bool ready;

    public Order()
    {
        ready = false;
    }

    public void Done()
    {
        ready = true;

    }
    public bool isReady()
    {
        return ready;
    }
}
