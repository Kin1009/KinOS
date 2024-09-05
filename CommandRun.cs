using System;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using BASIC;
using FileOP;

namespace CommandRun
{
    public class Runner
    {
        string ParsePath(string filepath, string path)
        {
            string res;
            if (path.Length > 1 && path[1] == ':')
            {
                res = path + "\\";
            }
            else
            {
                res = filepath + path + "\\";
            }
            if (path == "..")
            {
                if (!filepath.EndsWith(":\\"))
                {
                    res = Path.GetDirectoryName(filepath);
                }
                else
                {
                    res = filepath;
                }
            }
            return res;
        }

        // Method to split command arguments, preserving quoted regions
        private string[] SplitArguments(string command)
        {
            var args = new System.Collections.Generic.List<string>();
            var currentArg = "";
            var inQuotes = false;

            foreach (var ch in command)
            {
                if (ch == ' ' && !inQuotes)
                {
                    if (!string.IsNullOrWhiteSpace(currentArg))
                    {
                        args.Add(currentArg);
                        currentArg = "";
                    }
                }
                else if (ch == '"')
                {
                    inQuotes = !inQuotes;
                }
                else
                {
                    currentArg += ch;
                }
            }

            if (!string.IsNullOrWhiteSpace(currentArg))
            {
                args.Add(currentArg);
            }

            return args.ToArray();
        }

        public string ExecuteCommand(bool usedisks, CosmosVFS FAT, string filepath, string command)
        {
            var parts = SplitArguments(command);
            if (parts.Length == 0) return filepath;

            string cmd = parts[0].ToLower();
            string[] args = parts.Length > 1 ? parts[1..] : new string[] { };

            if (usedisks)
            {
                if (cmd == "mkdir")
                {
                    if (args.Length > 0)
                    {
                        FileOperations.CreateDirectory(ParsePath(filepath, args[0]));
                        Console.WriteLine($"Directory '{args[0]}' created.");
                    }
                    else
                    {
                        Console.WriteLine("Usage: mkdir <directory>");
                    }
                }
                else if (cmd == "basic")
                {
                    Basic.Main();
                }
                else if (cmd == "touch")
                {
                    if (args.Length > 0)
                    {
                        FileOperations.CreateFile(ParsePath(filepath, args[0]));
                        Console.WriteLine($"File '{args[0]}' created.");
                    }
                    else
                    {
                        Console.WriteLine("Usage: touch <filename>");
                    }
                }
                else if (cmd == "read")
                {
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Usage: read <file>");
                        return filepath;
                    }
                    string filePath = ParsePath(filepath, args[0]);
                    if (FileOperations.FileExists(filePath))
                    {
                        Console.WriteLine(FileOperations.ReadFromFile(filePath));
                    }
                    else
                    {
                        Console.WriteLine($"File '{args[0]}' not found.");
                    }
                }
                else if (cmd == "write")
                {
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Usage: write <file>");
                        return filepath;
                    }
                    string filePath = ParsePath(filepath, args[0]);
                    if (FileOperations.FileExists(filePath))
                    {
                        FileOperations.WriteToFile(filepath, filePath);
                    }
                    else
                    {
                        Console.WriteLine($"File '{args[0]}' not found.");
                    }
                }
                else if (cmd == "edit")
                {
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Usage: edit <file>");
                        return filepath;
                    }
                    string filePath = ParsePath(filepath, args[0]);
                    if (FileOperations.FileExists(filePath))
                    {
                        FileOperations.EditFile(filepath);
                    }
                    else
                    {
                        Console.WriteLine($"File '{args[0]}' not found.");
                    }
                }
                else if (cmd == "format")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: format <disk> <par>");
                        return filepath;
                    }
                    FileOperations.Format(FAT, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
                }
                else if (cmd == "cd")
                {
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Usage: cd <directory>");
                        return filepath;
                    }

                    string targetDirectory = ParsePath(filepath, args[0]);
                    if ((FileOperations.DirectoryExists(targetDirectory)) || (targetDirectory.EndsWith("..\\")))
                    {
                        filepath = targetDirectory;
                    }
                    else
                    {
                        Console.WriteLine($"Directory '{args[0]}' not found.");
                    }
                }
                else if (cmd == "ls")
                {
                    FileOperations.ListFilesAndDirectories(filepath);
                }
                else if (cmd == "rm")
                {
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Usage: rm <file|directory>");
                        return filepath;
                    }
                    string target = ParsePath(filepath, args[0]);
                    if (FileOperations.FileExists(target))
                    {
                        FileOperations.DeleteFile(target);
                        Console.WriteLine($"File '{args[0]}' deleted.");
                    }
                    else if (FileOperations.DirectoryExists(target))
                    {
                        FileOperations.DeleteDirectory(target);
                        Console.WriteLine($"Directory '{args[0]}' deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"File or directory '{args[0]}' not found.");
                    }
                }
                else if (cmd == "ren")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: ren <oldname> <newname>");
                        return filepath;
                    }
                    string oldName = ParsePath(filepath, args[0]);
                    string newName = ParsePath(filepath, args[1]);
                    FileOperations.RenameFile(oldName, newName);
                    Console.WriteLine($"Renamed '{args[0]}' to '{args[1]}'.");
                }
                else if (cmd == "cp")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: cp <source> <destination>");
                        return filepath;
                    }
                    string source = ParsePath(filepath, args[0]);
                    string destination = ParsePath(filepath, args[1]);
                    FileOperations.CopyFile(source, destination);
                    Console.WriteLine($"Copied '{args[0]}' to '{args[1]}'.");
                }
                else if (cmd == "mv")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: mv <source> <destination>");
                        return filepath;
                    }
                    string source = ParsePath(filepath, args[0]);
                    string destination = ParsePath(filepath, args[1]);
                    FileOperations.MoveFile(source, destination);
                    Console.WriteLine($"Moved '{args[0]}' to '{args[1]}'.");
                }
                else if (cmd == "reboot")
                {
                    Sys.Power.Reboot();
                }
                else if (cmd == "shutdown")
                {
                    Sys.Power.Shutdown();
                }
                else
                {
                    Console.WriteLine($"Command '{cmd}' not recognized.");
                }
                return filepath;
            }
            else
            {
                Console.WriteLine("You can't use disks.");
                if (cmd == "reboot")
                {
                    Sys.Power.Reboot();
                }
                else if (cmd == "shutdown")
                {
                    Sys.Power.Shutdown();
                }
                else if (cmd == "basic")
                {
                    Basic.Main();
                }
                else
                {
                    Console.WriteLine($"Command '{cmd}' not recognized.");
                }
            }
            return filepath;
        }
    }
}
