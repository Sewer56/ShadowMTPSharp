using System;
using System.IO;
using Microsoft.Win32;
using MTPLib;

namespace MTPConsole
{
    public class Program
    {
        private const string MtpExtension = "MTP";

        static void Main(string[] args)
        {
            Console.WriteLine("MTPConsole v1.1.0");
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    var fullPath = Path.GetFullPath(arg);

                    if (IsMtp(fullPath))
                        ExtractMtp(fullPath);
                    else if (IsDirectory(fullPath))
                        BuildMtp(fullPath);
                }
            } else {
                Console.WriteLine("Usage: MTPConsole.exe <paths to files to extract or directories to pack>");
                Console.WriteLine("Example: MTPConsole.exe BkChaos.mtp BkLarva.mtp");
                Console.WriteLine("\nRunning with no arguments registers the .mtp extension to allow double-clicking .mtp files for extraction");

                try
                {
                    var classesKey = Registry.CurrentUser.OpenSubKey("Software", true)?.OpenSubKey("Classes", true);
                    var oneKey = classesKey.CreateSubKey(".mtp");
                    string myExecutable = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    string command = $"\"{myExecutable}\" \"%1\"";
                    var commandKey = oneKey.CreateSubKey("shell\\Open\\command");
                    commandKey.SetValue("", command);
                }
                catch (Exception) { /* Might fail on Wine */ }
            }
        }

        private static void ExtractMtp(string fullPath)
        {
            var outputDirectory = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
            InitializeDirectory(outputDirectory);

            var package = MotionPackage.FromMtp(File.ReadAllBytes(fullPath));
            package.ToDirectory(outputDirectory);

            Console.WriteLine($"Extracted MTP To: {outputDirectory}");
        }

        private static void BuildMtp(string fullPath)
        {
            var motionPackage = MotionPackage.FromDirectory(fullPath);
            var filePath      = Path.Combine(Path.GetDirectoryName(fullPath), $"{Path.GetFileNameWithoutExtension(fullPath)}.{MtpExtension}");

            File.WriteAllBytes(filePath, motionPackage.ToMtp());
            Console.WriteLine($"Written MTP To: {filePath}");
        }

        /* Utility */
        private static bool IsMtp(string fullPath)       => File.Exists(fullPath);
        private static bool IsDirectory(string fullPath) => Directory.Exists(fullPath);
        private static string InitializeDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }
    }
}
