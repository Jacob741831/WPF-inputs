using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Dalej_Click(object sender, RoutedEventArgs e)
        {
            string plec;

            if (RadioM.IsChecked == true)
                plec = "M";
            else if (RadioK.IsChecked == true)
                plec = "K";
            else
                plec = "I";

            SecondWindow okno = new SecondWindow(plec);
            okno.Show();

            this.Close();
        }
    }
}