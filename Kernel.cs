using System;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using CommandRun;
using Bootloader;
/* GOALS
 * 1. Add struct User
 * 2. Add users command
 * 3. Add a fcking usable language (hadnt find one yet)
 * 4. Add sudo
 */
namespace OS
{
    public class Kernel : Sys.Kernel
    {
        public string filepath = @"0:\";
        public string command_;
        Sys.FileSystem.CosmosVFS fs = new();
        CosmosVFS FAT = new CosmosVFS();
        bool usedisks = true;
        protected override void BeforeRun()
        {
            usedisks = Boot.Show();
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                VFSManager.RegisterVFS(FAT);
                FAT.Initialize(true);
                Console.WriteLine("Initialized file systems.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't initialize file systems: " + e.Message);
                Console.WriteLine("File systems will be disabled!");
                usedisks = false;
            }
            if (usedisks)
            {
                Console.WriteLine("Available commands: cd mkdir touch edit rm ren mv basic shutdown reboot");
            } else
            {
                Console.WriteLine("Available commands: basic shutdown reboot");
            }
        }

        protected override void Run()
        {
            Console.Write(filepath + " $ ");
            command_ = Console.ReadLine();
            Runner interpreter = new Runner();
            try
            {
                string newfilepath = interpreter.ExecuteCommand(usedisks, fs, filepath, command_);
                filepath = newfilepath;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
