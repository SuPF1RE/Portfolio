using Avalonia.Controls;

namespace Test;

public partial class DialogWindow : Window
{
    public DialogWindow()
    {
        InitializeComponent();
        YesButton.Click += (_, _) => Close(true);
        NoButton.Click += (_, _) => Close(false);
    }
}