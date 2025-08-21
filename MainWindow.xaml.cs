using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace KoishiApp;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly ObservableCollection<IconPair> iconPairs;

    public MainWindow()
    {
        InitializeComponent();
        CheckVersion();
        iconPairs =
            new
                ObservableCollection<IconPair> // да это определенное отличная идея делать вместо норм переменных обращение по позиции :)
                {
                    new() { Icon1 = "Wifi", Icon2 = "Close" },
                    new() { Icon1 = "Dns", Icon2 = "Close" },
                    new() { Icon1 = "Dns", Icon2 = "Close" },
                    new() { Icon1 = "Web", Icon2 = "Close" }
                };

        IconList.ItemsSource = iconPairs;
        Title += RandomTitle();
    }

    private string RandomTitle()
    {
        List<string> titles = new()
        {
            "Turbo-Crash-Bandicoot",
            "Penis-Penis-Penis-Penis",
            "Trojans version",
            "No handholding :)",
            "Donate me 10000000$",
            "There's more titles?!",
            "vo vsem vinovat rock",
            "Yes title is changing :3",
            "ᗜ˰ᗜ"
        };

        return titles[new Random().Next(0, titles.Count - 1)];
    }

    private void GithubButton(object sender, RoutedEventArgs e)
    {
        var url = "https://github.com/unicode11/Koishi-App";
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }

    private void RestartDebil()
    {
        foreach (var icon in iconPairs) icon.Icon2 = "Close";
        Prelaucnh.Visibility = Visibility.Collapsed;
        Postlaunch.Visibility = Visibility.Visible;
        GotMessage.Visibility = Visibility.Collapsed;
        Main();
    }

    private void RestartButton(object sender, RoutedEventArgs e)
    {
        RestartDebil();
    }

    private void CirnoButton(object sender, RoutedEventArgs e)
    {
        var url = "https://i.pinimg.com/1200x/93/8d/a4/938da45d026f9677331a7333b211b25f.jpg";
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }

    private void StartButton(object sender, RoutedEventArgs e)
    {
        RestartDebil();
    }

    private void CopyThat(object sender, MouseButtonEventArgs e)
    {
        var textToCopy = Cons.SelectedItem.ToString();
        Clipboard.SetText(textToCopy);
    }

    public async Task CheckVersion()
    {
        var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        Log.Message($"USER version: {currentVersion}");
        VersionText.Text = currentVersion;

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");

            var url = "https://api.github.com/repos/unicode11/Koishi-App/releases/latest";
            var json = await client.GetStringAsync(url);

            var release = JsonSerializer.Deserialize<GithubRelease>(json);
            var latestVersion = release?.tag_name?.TrimStart('v') ?? "0.0.0";

            Log.Message($"SERVER version: {latestVersion}");

            if (currentVersion != latestVersion)
                MessageBox.Show(
                    $"Доступна новая версия {latestVersion} (у вас {currentVersion}).\nЗайдите на GitHub для обновления.",
                    "Охайо :)",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
        }
        catch (Exception ex)
        {
            Log.Message("Got an error while checking for version:\n" + ex.Message, 1);
        }
    }

    public class GithubRelease
    {
        public string tag_name { get; set; }
    }
}

public class IconPair : INotifyPropertyChanged
{
    private string icon1;

    private string icon2;

    public string Icon1
    {
        get => icon1;
        set
        {
            if (icon1 != value)
            {
                icon1 = value;
                OnPropertyChanged(nameof(Icon1));
            }
        }
    }

    public string Icon2
    {
        get => icon2;
        set
        {
            if (icon2 != value)
            {
                icon2 = value;
                OnPropertyChanged(nameof(Icon2));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // totalno spizzeno
    }
}