namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"D:\gmyl\PRD_LOGS\EmphasisServer_LAFARGEHOLCIM\ComServer\DeviceWorkers\IoTWorker\Logs";

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
                Console.WriteLine($"Reading file: {filePath}");
                try
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(filePath);
                    int lineNumber = 1;

                    // Print each line with line number
                    foreach (string line in lines)
                    {
                        if (line == null)
                            continue;
                        if (line.Length < 6)
                            continue;
                        if (line.Contains("[Recv]") == false)
                            continue;

                        try
                        {

                            var idx = line.IndexOf("[Recv]\t[");
                            var packet = line.Substring(idx + 8);
                            packet = packet.Remove(packet.Length-1, 1);
                            if (packet[5] == 'A' && packet.Length >97 )
                                Console.WriteLine(packet);
                            lineNumber++;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
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
