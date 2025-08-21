using System.IO;
using System.Windows;

namespace KoishiApp;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        var jsonPath = "user_data.json"; // небольшая очисточка

        try
        {
            if (File.Exists(jsonPath)) File.Delete(jsonPath);

            Log.Message("Trying to collect govno");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Log.Message("Collected govno");
        }

        catch (Exception ex)
        {
            Log.Message($"CRITICAL FAILURE:\n{ex}", 2);
        }

        Log.Message("Goodbye! :)");
    }
}