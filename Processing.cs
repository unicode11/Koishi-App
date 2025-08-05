using System.Net;
using System.Net.NetworkInformation;
using System.Windows;

namespace KoishiApp;

public partial class MainWindow : Window
{
    private async void Main()
    {
        SStatusBar.Text = "Проверяю соединение к интернету";
        await CheckForConnection("https://www.google.com/");
        
        SStatusBar.Text = "Проверяю доступ к обоим серверам";
        await CheckForRussianServer();
        await CheckForNetherlandsServer();
    }

    private Task CheckForConnection(string url)
    {
        Log.Message("Trying to connect to google.com");
        
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Timeout = 10000; // ms
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Log.Message("Successfull.");
                SStatusBar.Text = $"Интернет присутствует";
                iconPairs[0].Icon2 = "Check";
            }
        }
        catch (Exception ex)
        {
            SStatusBar.Text = $"Интернета не присутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
        }

        return Task.CompletedTask;
    }
    
    private Task CheckForRussianServer()
    {
        Log.Message("Trying to connect to Russian server");
        try
        {
            using (Ping pinger = new Ping())
            {
                PingReply reply = pinger.Send("2.56.178.91");
                if (reply.Status == IPStatus.Success)
                {
                    Log.Message("Successfull.");
                    SStatusBar.Text = $"Ответ получен";
                    iconPairs[1].Icon2 = "Check";
                }
            }
        }
        catch  (Exception ex) 
        {
            SStatusBar.Text = $"Соединение с русским сервером отсутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
        }

        return Task.CompletedTask;
    }
    
    private Task CheckForNetherlandsServer()
    {
        Log.Message("Trying to connect to Netherlands server");
        try
        {
            using (Ping pinger = new Ping())
            {
                PingReply reply = pinger.Send("45.12.142.107");
                if (reply.Status == IPStatus.Success)
                {
                Log.Message("Successfull.");
                    SStatusBar.Text = $"Ответ получен";
                    iconPairs[2].Icon2 = "Check";
                }
            }
        }
        catch (Exception ex) 
        {
            SStatusBar.Text = $"Соединение с нидерландским сервером отсутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
        }

        return Task.CompletedTask;
    }
}