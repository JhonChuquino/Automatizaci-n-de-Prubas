using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using OpenQA.Selenium;

public class ExtentReport
{
    public static ExtentReports _extentReports;
    public static ExtentTest _feature;
    public static ExtentTest _scenario;

    // Raíz de resultados
    public static string dir = AppDomain.CurrentDomain.BaseDirectory;
    public static string testResultRoot = dir.Replace("bin\\Debug\\net8.0", "TestResults");

    // Contexto de la ejecución
    public static string runTimestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    public static string runFolder = Path.Combine(testResultRoot, runTimestamp);
    public static string screenshotsFolder = Path.Combine(runFolder, "Screenshots");

    public static void ExtentReportInit()
    {
        Directory.CreateDirectory(runFolder);
        Directory.CreateDirectory(screenshotsFolder);

        string reportFile = Path.Combine(runFolder, $"ExtentReport_{runTimestamp}.html");

        var spark = new ExtentSparkReporter(reportFile);
        spark.Config.ReportName = "Automation Status Report";
        spark.Config.DocumentTitle = "Automation Status Report";
        spark.Config.Theme = Theme.Standard;

        _extentReports = new ExtentReports();
        _extentReports.AttachReporter(spark);
        _extentReports.AddSystemInfo("Application", "Automatización POM");
        _extentReports.AddSystemInfo("Browser", "Chrome");
        _extentReports.AddSystemInfo("OS", "Windows");
    }

    public static void ExtentReportTearDown()
    {
        if (_extentReports != null)
        {
            _extentReports.Flush();
            Console.WriteLine($"Reporte generado correctamente en: {runFolder}");
        }
        else
        {
            Console.WriteLine("ExtentReports no fue inicializado correctamente.");
        }
    }

    public static string addScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
    {
        ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
        Screenshot screenshot = takesScreenshot.GetScreenshot();

        string fileName = $"{SanitizeFileName(scenarioContext.ScenarioInfo.Title)}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        string screenshotLocation = Path.Combine(screenshotsFolder, fileName);

        screenshot.SaveAsFile(screenshotLocation);
        return screenshotLocation;
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }


}