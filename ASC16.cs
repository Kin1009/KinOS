if (Console.KeyAvailable)
{
    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        Console.WriteLine("Choose 1, 2, 3 or 4:");
        Console.WriteLine("1. Shutdown");
        Console.WriteLine("2. Restart");
        Console.WriteLine("3. Boot normally");
        Console.WriteLine("4. Boot without disk access");
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
            {
                Sys.Power.Shutdown();
            }
            else if (key == ConsoleKey.D2)
            {
                Sys.Power.Reboot();
            }
            else if (key == ConsoleKey.D3)
            {
                return true;
            }
            else if (key == ConsoleKey.D4)
            {
                return false;
            }
        }
    }
}