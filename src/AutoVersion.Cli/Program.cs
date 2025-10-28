using AutoVersion.Core;

namespace AutoVersion.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== AutoVersion CLI ===");
            Console.WriteLine($"Current Version: {VersionInfo.GetVersion()}");
            Console.WriteLine("Usage: bump [major|minor|patch]");

            if (args.Length > 0 && args[0] == "bump")
            {
                Console.WriteLine("Bumping version (stub)...");
            }
        }
    }
}
