using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoScope
{
    public static class Geolocator
    {
        private static readonly HttpClient _httpClient = new();

        // View location based on your current IP
        public static async Task ViewMyLocation()
        {
            Console.Clear();
            Console.WriteLine("Fetching your IP-based location...");
            var loc = await GetLocationFromIP();

            if (loc != null)
            {
                Console.WriteLine($"\nYour IP: {loc.QueryIP}");
                Console.WriteLine($"Location: {loc}");
                Console.WriteLine($"ISP: {loc.ISP}");
                Console.WriteLine($"Zip: {loc.Zip}");
                Console.WriteLine($"Timezone: {loc.Timezone}");

                var address = await ReverseGeocodeApi(loc.Latitude, loc.Longitude);
                Console.WriteLine($"Approximate Address: {address}");
            }
            else
            {
                Console.WriteLine("Failed to retrieve location.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // New method: Geolocate any IP entered by user
        public static async Task GeolocateIP()
        {
            Console.Clear();
            Console.WriteLine("Geolocate Another IP");

            Console.Write("Enter an IP address (IPv4 or IPv6): ");
            string ip = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(ip))
            {
                Console.WriteLine("Invalid IP input.");
                Console.ReadKey();
                return;
            }

            var loc = await GetLocationFromIP(ip);

            if (loc != null)
            {
                Console.WriteLine($"\nIP: {loc.QueryIP}");
                Console.WriteLine($"Location: {loc}");
                Console.WriteLine($"ISP: {loc.ISP}");
                Console.WriteLine($"Zip: {loc.Zip}");
                Console.WriteLine($"Timezone: {loc.Timezone}");

                var address = await ReverseGeocodeApi(loc.Latitude, loc.Longitude);
                Console.WriteLine($"Approximate Address: {address}");
            }
            else
            {
                Console.WriteLine("Failed to retrieve location for that IP.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Reverse geocode latitude & longitude to address
        public static async Task ReverseGeocode()
        {
            Console.Clear();
            Console.WriteLine("Reverse Geocode Coordinates");

            Console.Write("Enter Latitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lat))
            {
                Console.WriteLine("Invalid latitude.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Longitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lon))
            {
                Console.WriteLine("Invalid longitude.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nLooking up address...");
            var address = await ReverseGeocodeApi(lat, lon);
            Console.WriteLine($"Result: {address}");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Calculate distance between two coordinates in kilometers
        public static void CalculateDistance()
        {
            Console.Clear();
            Console.WriteLine("Distance Calculator (in kilometers)");

            Console.Write("Start Latitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lat1))
            {
                Console.WriteLine("Invalid latitude.");
                Console.ReadKey();
                return;
            }

            Console.Write("Start Longitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lon1))
            {
                Console.WriteLine("Invalid longitude.");
                Console.ReadKey();
                return;
            }

            Console.Write("End Latitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lat2))
            {
                Console.WriteLine("Invalid latitude.");
                Console.ReadKey();
                return;
            }

            Console.Write("End Longitude: ");
            if (!double.TryParse(Console.ReadLine(), out double lon2))
            {
                Console.WriteLine("Invalid longitude.");
                Console.ReadKey();
                return;
            }

            double km = CalculateDistanceKm(lat1, lon1, lat2, lon2);
            Console.WriteLine($"\nDistance: {km:F2} km");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Internal location class for JSON mapping
        private class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public string Zip { get; set; }
            public string Timezone { get; set; }
            public string ISP { get; set; }
            public string QueryIP { get; set; }

            public override string ToString()
            {
                return $"{City}, {Region}, {Country} ({Latitude}, {Longitude})";
            }
        }

        // Fetch location from IP, optionally for a specific IP or own IP if null
        private static async Task<Location> GetLocationFromIP(string ip = null)
        {
            try
            {
                string url = ip == null
                    ? "http://ip-api.com/json"
                    : $"http://ip-api.com/json/{ip}";

                var response = await _httpClient.GetStringAsync(url);
                var json = JsonDocument.Parse(response).RootElement;

                if (json.GetProperty("status").GetString() != "success") return null;

                return new Location
                {
                    Latitude = json.GetProperty("lat").GetDouble(),
                    Longitude = json.GetProperty("lon").GetDouble(),
                    City = json.GetProperty("city").GetString(),
                    Region = json.GetProperty("regionName").GetString(),
                    Country = json.GetProperty("country").GetString(),
                    Zip = json.GetProperty("zip").GetString(),
                    Timezone = json.GetProperty("timezone").GetString(),
                    ISP = json.GetProperty("isp").GetString(),
                    QueryIP = json.GetProperty("query").GetString()
                };
            }
            catch
            {
                return null;
            }
        }

        // Reverse geocode latitude and longitude using OpenStreetMap Nominatim
        private static async Task<string> ReverseGeocodeApi(double lat, double lon)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GeoScopeApp/1.0");

                var response = await _httpClient.GetStringAsync(
                    $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}"
                );

                var json = JsonDocument.Parse(response).RootElement;
                return json.TryGetProperty("display_name", out var addr) ? addr.GetString() : "Unknown Address";
            }
            catch
            {
                return "Error occurred during reverse geocoding.";
            }
        }

        // Calculate great-circle distance between two coordinates (Haversine formula)
        private static double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double deg) => deg * (Math.PI / 180);
    }
}
