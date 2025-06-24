namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\MyStore\Emphasis\IoTDevicesEmulation\TestFiles";

            // Check if directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist!");
                return;
            }

            // Get all files in the directory
            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Console.WriteLine("No files found in the directory.");
                return;
            }


            // Iterate through each file
            foreach (string filePath in files)
            {
                Console.WriteLine($"\nReading file: {filePath}");
                try
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(filePath);
                    int lineNumber = 1;

                    // Print each line with line number
                    foreach (string line in lines)
                    {
                        if (line[5] == '1')
                            Console.WriteLine(line);
                        lineNumber++;
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Access denied to file {filePath}: {ex.Message}");
                }
            }

            }
    }
}
