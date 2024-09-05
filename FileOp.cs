using Cosmos.System.FileSystem;
using System.IO;
using System;
using System.Reflection.Metadata;
namespace FileOP
{ 
	public class FileOperations
	{
		public static void CreateFile(string path)
		{
			path = path.Substring(0, path.Length - 1);
			try
			{
				using var file = File.Create(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating file at {path}: {ex.Message}");
			}
		}
        public static void EditFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    // Read the file content
                    string content = ReadFromFile(path);
                    string[] lines = content.Split('\n');
                    int idx = 0;

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Editing file: " + path);
                        Console.WriteLine("-----------------------------");

                        // Display lines
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i == idx)
                                Console.BackgroundColor = ConsoleColor.Gray;

                            Console.WriteLine(lines[i]);
                            Console.ResetColor();
                        }

                        // Navigate and edit lines
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.UpArrow && idx > 0)
                        {
                            idx--;
                        }
                        else if (keyInfo.Key == ConsoleKey.DownArrow && idx < lines.Length - 1)
                        {
                            idx++;
                        }
                        else if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            Console.WriteLine("Edit line (current): " + lines[idx]);
                            Console.Write("New content: ");
                            lines[idx] = Console.ReadLine();
                        }
                        else if (keyInfo.Key == ConsoleKey.S)
                        {
                            // Save changes
                            File.WriteAllText(path, string.Join("\n", lines));
                            Console.WriteLine("Changes saved!");
                        }
                        else if (keyInfo.Key == ConsoleKey.Q)
                        {
                            // Exit editor
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"File does not exist: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing file at {path}: {ex.Message}");
            }
        }
		public static void WriteToFile(string path, string content)
		{
			try
			{
				File.WriteAllText(path, content);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error writing to file at {path}: {ex.Message}");
			}
		}

		public static string ReadFromFile(string path)
		{
			try
			{
				return File.ReadAllText(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading file at {path}: {ex.Message}");
				return "";
			}
		}

		public static void DeleteFile(string path)
		{
			try
			{
				File.Delete(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting file at {path}: {ex.Message}");
			}
		}

		public static void CreateDirectory(string path)
		{
			try
			{
				Directory.CreateDirectory(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating directory at {path}: {ex.Message}");
			}
		}

		public static void DeleteDirectory(string path)
		{
			try
			{
				Directory.Delete(path, true);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting directory at {path}: {ex.Message}");
			}
		}

		public static bool FileExists(string path)
		{
			try
			{
				return File.Exists(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error checking if file exists at {path}: {ex.Message}");
				return false;
			}
		}

		public static bool DirectoryExists(string path)
		{
			try
			{
				return Directory.Exists(path);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error checking if directory exists at {path}: {ex.Message}");
				return false;
			}
		}

		public static void RenameFile(string sourcePath, string destPath)
		{
			try
			{
				MoveFile(sourcePath, destPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error renaming file from {sourcePath} to {destPath}: {ex.Message}");
			}
		}

		public static void CopyFile(string sourcePath, string destPath)
		{
			try
			{
				File.Copy(sourcePath, destPath, true);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error copying file from {sourcePath} to {destPath}: {ex.Message}");
			}
		}

		public static void MoveFile(string file, string newpath)
		{
			try
			{
				File.Copy(file, newpath);
				File.Delete(file);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error moving file from {file} to {newpath}: {ex.Message}");
			}
		}

		public static void ListFilesAndDirectories(string path)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					Console.WriteLine($"Directory '{path}' not found.");
					return;
				}

				var directories = Directory.GetDirectories(path);
				var files = Directory.GetFiles(path);
				foreach (var dir in directories)
				{
					Console.WriteLine($"DIR {Path.GetFileName(dir)}");
				}
				Console.WriteLine("DIR ..");
				foreach (var file in files)
				{
					Console.WriteLine($"FILE {Path.GetFileName(file)}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error listing files and directories at {path}: {ex.Message}");
			}
		}
		public static void Format(CosmosVFS fat, int diskIndex, int partitionIndex)
		{
			fat.Disks[diskIndex].FormatPartition(partitionIndex, "FAT32");
		}
	}
}