using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var (salesTotal, salesDetails) = CalculateSalesTotalAndDetails(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesDetails, salesTotal, salesTotalDir);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

// Updated helper method to return both the grand total and individual file details
(double GrandTotal, Dictionary<string, double> Details) CalculateSalesTotalAndDetails(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    var details = new Dictionary<string, double>();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        double fileTotal = data?.Total ?? 0;
        salesTotal += fileTotal;

        // Store the file name and its corresponding sales total
        string fileName = Path.GetFileName(file);
        details[fileName] = fileTotal;
    }

    return (salesTotal, details);
}

// New function to generate the sales summary report file 
void GenerateSalesSummaryReport(Dictionary<string, double> salesDetails, double grandTotal, string outputDir)
{
    var reportPath = Path.Combine(outputDir, "sales-summary.txt");

    using var writer = new StreamWriter(reportPath);
    writer.WriteLine("Sales Summary");
    writer.WriteLine("------------------------");
    writer.WriteLine($"Total Sales: ${grandTotal:N2}");
    writer.WriteLine();
    writer.WriteLine("Details:");

    foreach (var detail in salesDetails)
    {
        writer.WriteLine($"  {detail.Key}: ${detail.Value:N2}");
    }

    Console.WriteLine($"Sales summary report written to {reportPath}");
}

record SalesData(double Total);