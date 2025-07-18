using System;
using System.Threading.Tasks;
using GeoScope;

class MainClass
{
    static async Task Main(string[] args)
    {
        Console.Title = "GeoScope by WYN.GG Studios";
        bool running = true;

        while (running)
        {
            Console.Clear();
            Credits.Show();

            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. View My Location (IP-Based)");
            Console.WriteLine("2. Geolocate Another IP");
            Console.WriteLine("3. Reverse Geocode Coordinates");
            Console.WriteLine("4. Calculate Distance Between Two Locations");
            Console.WriteLine("5. Credits");
            Console.WriteLine("6. Exit");

            Console.Write("\nSelect an option [1-6]: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await Geolocator.ViewMyLocation();
                    break;
                case "2":
                    await Geolocator.GeolocateIP();
                    break;
                case "3":
                    await Geolocator.ReverseGeocode();
                    break;
                case "4":
                    Geolocator.CalculateDistance();
                    break;
                case "5":
                    Credits.ShowPause();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Exiting GeoScope...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
