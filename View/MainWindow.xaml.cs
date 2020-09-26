using System.Globalization;
using System.Threading;
using System.Windows;

namespace WPFApp.View
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>

    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string codeLanguage)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(name: codeLanguage);
            InitializeComponent();
        }
    }
}