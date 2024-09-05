using System;
using Sys = Cosmos.System;
namespace Bootloader
{
	public class Boot
	{
		static public bool Show()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine(@"		 /$$   /$$ /$$            /$$$$$$   /$$$$$$ 
		| $$  /$$/|__/           /$$__  $$ /$$__  $$
		| $$ /$$/  /$$ /$$$$$$$ | $$  \ $$| $$  \__/
		| $$$$$/  | $$| $$__  $$| $$  | $$|  $$$$$$ 
		| $$  $$  | $$| $$  \ $$| $$  | $$ \____  $$
		| $$\  $$ | $$| $$  | $$| $$  | $$ /$$  \ $$
		| $$ \  $$| $$| $$  | $$|  $$$$$$/|  $$$$$$/
		|__/  \__/|__/|__/  |__/ \______/  \______/ 
											");
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("    Hold Escape for boot options");
			Console.Beep(400, 200);
			Console.Beep(500, 400);
			Console.Beep(600, 200);
			Console.Clear();
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
						Console.ForegroundColor = ConsoleColor.White;
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
			return true;
		}
	}
}
