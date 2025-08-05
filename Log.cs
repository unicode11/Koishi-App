using System.IO;
using System.Windows.Media;

namespace KoishiApp;

public class Log
{
    private static string logFilePath;

    static Log()
    {
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(logDirectory, $"log_{timestamp}.txt");

        File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] --- SESSION STARTED ---{Environment.NewLine}");
    }

    public static void Message(string message, int level=0)
    {
        Console.WriteLine(message);
        switch (level)
        {
            case 1: WriteLog($"[WARNING] {message}"); break; // WARNING
            case 2: WriteLog($"[!! ERROR !!] {message}"); break; // ERROR
            default: WriteLog($"{message}"); break;
        }
    }

    private static void WriteLog(string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        File.AppendAllText(logFilePath, line + Environment.NewLine);
    }
}
    