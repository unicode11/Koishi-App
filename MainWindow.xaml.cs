using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KoishiApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string RandomSmile()
    {
        List<string> smiles = new()
        {
            ":3", ":)", "c:"
        };
        
        return smiles[new Random().Next(0, smiles.Count - 1)];
    }

    private string RandomTitle()
    {
        List<string> Titles = new()
        {
            "Turbo-Crash-Bandicoot", "Penis-Penis-Penis-Penis", "Trojans version",
            "No handholding :)"
        };
        
        return Titles[new Random().Next(0, Titles.Count - 1)];
    }
    
    private ObservableCollection<IconPair> iconPairs;

    public MainWindow()
    {
        InitializeComponent();

        iconPairs = new ObservableCollection<IconPair> // да это определенное отличная идея делать вместо норм переменных обращение по позиции :)
        {
            new() { Icon1 = "Wifi", Icon2 = "Close" },
            new() { Icon1 = "Dns", Icon2 = "Close" },
            new() { Icon1 = "Dns", Icon2 = "Close" },
        };

        IconList.ItemsSource = iconPairs;
        Title += RandomTitle();
        SStatusBar.Text+= " " + RandomSmile();
    }

    private void GithubButton(object sender, RoutedEventArgs e)
    {
        string url = "https://github.com/unicode11/Koishi-App";
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }
    
    
    private void RestartButton(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void CirnoButton(object sender, RoutedEventArgs e)
    {
        string url = "https://i.pinimg.com/1200x/93/8d/a4/938da45d026f9677331a7333b211b25f.jpg";
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }

    private void StartButton(object sender, RoutedEventArgs e)
    {
        Prelaucnh.Visibility = Visibility.Collapsed;
        Postlaunch.Visibility = Visibility.Visible;
        Main();
    }
}

public class IconPair : INotifyPropertyChanged
{
    private string icon1;
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

    private string icon2;
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
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
