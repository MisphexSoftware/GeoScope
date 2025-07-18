using System;
using System.Threading;

namespace GeoScope
{
    public static class Credits
    {
        public static void Show(bool delay = false)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            string[] logo =
            {
                " ██████╗ ███████╗ ██████╗ ███████╗ ██████╗ ██████╗ ██████╗ ███████╗",
                "██╔════╝ ██╔════╝██╔═══██╗██╔════╝██╔════╝██╔═══██╗██╔══██╗██╔════╝",
                "██║  ███╗█████╗  ██║   ██║███████╗██║     ██║   ██║██████╔╝█████╗  ",
                "██║   ██║██╔══╝  ██║   ██║╚════██║██║     ██║   ██║██╔═══╝ ██╔══╝  ",
                "╚██████╔╝███████╗╚██████╔╝███████║╚██████╗╚██████╔╝██║     ███████╗",
                " ╚═════╝ ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═════╝ ╚═╝     ╚══════╝",
                "                                                                   "
            };

            foreach (string line in logo)
            {
                Console.WriteLine(line);
                if (delay) Thread.Sleep(50); // Delay between lines (optional)
            }

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nGeoScope v1.0 - Advanced Geolocation Tool");
            Console.WriteLine("Developed by: WYN.GG Studios");
            Console.WriteLine("Co-Owner: 𝕷𝖔𝖓𝖊𝖑𝖞_𝕾𝖍𝖆𝖉𝖔𝖜9215");
            Console.WriteLine("Website: https://wyn.gg");
            Console.WriteLine("© 2025 WYN.GG Studios. All rights reserved.");
            Console.WriteLine("------------------------------------------------------------\n");
            Console.ResetColor();
        }

        public static void ShowPause()
        {
            Show();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}
