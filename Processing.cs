using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Windows;

namespace KoishiApp;

public partial class MainWindow : Window
{
    public UserPidor userData;

    private async void Main()
    {
        SStatusBar.Text = "Проверяю соединение к интернету";
        await CheckForConnection("https://www.google.com/");

        SStatusBar.Text = "Проверяю доступ к обоим серверам";
        await CheckForRussianServer();
        await CheckForNetherlandsServer();

        SStatusBar.Text = "Логинься в дискорде";
        ConnectToDiscord();

        // postpreps вынес в connecttodiscord а то хули
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
                SStatusBar.Text = "Интернет присутствует";
                iconPairs[0].Icon2 = "Check";
            }
        }
        catch (Exception ex)
        {
            SStatusBar.Text = "Интернета не присутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
            PostPreps();
        }

        return Task.CompletedTask;
    }

    private Task CheckForRussianServer()
    {
        Log.Message("Trying to connect to Russian server");
        try
        {
            using (var pinger = new Ping())
            {
                var reply = pinger.Send("2.56.178.91");
                if (reply.Status == IPStatus.Success)
                {
                    Log.Message("Successfull.");
                    SStatusBar.Text = "Ответ получен";
                    iconPairs[1].Icon2 = "Check";
                }
            }
        }
        catch (Exception ex)
        {
            SStatusBar.Text = "Соединение с русским сервером отсутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
            PostPreps();
        }

        return Task.CompletedTask;
    }

    private Task CheckForNetherlandsServer()
    {
        Log.Message("Trying to connect to Netherlands server");
        try
        {
            using (var pinger = new Ping())
            {
                var reply = pinger.Send("45.12.142.107");
                if (reply.Status == IPStatus.Success)
                {
                    Log.Message("Successfull.");
                    SStatusBar.Text = "Ответ получен";
                    iconPairs[2].Icon2 = "Check";
                }
            }
        }
        catch (Exception ex)
        {
            SStatusBar.Text = "Соединение с нидерландским сервером отсутствует.";
            Log.Message($"Got an error.\n{ex}", 2);
            PostPreps();
        }

        return Task.CompletedTask;
    }

    private async void ConnectToDiscord()
    {
        Log.Message("Trying to connect to Discord");
        try
        {
            Log.Message("OAUTH");
            var clientId = "1319748052881244210"; // секретник теперь хранится прямо на сервере потому что код публичный
            var redirectUri = "http://localhost:5000/callback";

            var authUrl =
                $"https://discord.com/oauth2/authorize?client_id={clientId}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&response_type=code&scope=identify";

            Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });

            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/callback/");
            listener.Start();

            SStatusBar.Text = "Отсылаю сообщение";

            var context = await listener.GetContextAsync();
            var code = context.Request.QueryString["code"];
            Log.Message($"Got OAuth: {code}");

            using var client = new HttpClient();

            var payload = new Dictionary<string, string>
            {
                { "code", code },
                { "redirect_uri", redirectUri }
            };

            var content = new FormUrlEncodedContent(payload);

            Log.Message("Waiting for server...");
            var tokenResp = await client.PostAsync("http://45.12.142.107/oauth/callback", content);
            // если тут кинет ошибку то на сервере голландии прокинь 
            // uvicorn main:app --host 0.0.0.0 --port 
            var serverJson = await tokenResp.Content.ReadAsStringAsync();
            Log.Message($"Server responded: {serverJson}");
            SStatusBar.Text = "Закончила!";
            File.WriteAllText("user_data.json", serverJson);

            iconPairs[3].Icon2 = "Check";
            PostPreps();
        }
        catch (Exception e)
        {
            Log.Message($"Got an error.\n{e}", 2);
            PostPreps();
        }
    }

    private async void PostPreps()
    {
        var msg = "Похоже, произошло критическое недоразумение!\n" +
                  "В корневой папке программы найди папку Logs и отыщи последнюю сессию.\n" +
                  "Сперва убедись, что версия с Github и твоя совпадает.\n" +
                  "Далее обратись к администратору с прикрепленными логами.";

        Log.Message("Prepping UI for post..");
        SStatusBar.Text = "Вот твои ссылки, подключайся! Рада была помочь. :3";
        Postlaunch.Visibility = Visibility.Collapsed;
        GotMessage.Visibility = Visibility.Visible;
        try
        {
            var json = File.ReadAllText("user_data.json");
            var user = JsonSerializer.Deserialize<UserPidor>(json);

            Cons.Items.Add(user.message.proxy1);
            Cons.Items.Add(user.message.proxy2);
        }
        catch (Exception e)
        {
            Log.Message($"Got an error.\n{e}", 2);
            MessageBox.Show(
                msg, "pizdec",
                MessageBoxButton.OK, MessageBoxImage.Error
            );
            Cons.Items.Add(msg);
        }
    }
}

public class UserPidor
{
    public string discord_id { get; set; }
    public string username { get; set; }
    public MessagePidor message { get; set; }
}

public class MessagePidor
{
    public string login { get; set; }
    public string proxy1 { get; set; }
    public string proxy2 { get; set; }
}