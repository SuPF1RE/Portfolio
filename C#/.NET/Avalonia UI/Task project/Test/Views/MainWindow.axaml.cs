using Avalonia.Controls;
using Test.ViewModels;

namespace Test.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}