
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Klasa reprezentująca drugie okno aplikacji.
    /// Umożliwia wprowadzenie wieku, wybór koloru
    /// oraz zaznaczenie zgody użytkownika.
    /// </summary>
    public partial class SecondWindow : Window
    {
        /// <summary>
        /// Przechowuje płeć wybraną w pierwszym oknie.
        /// readonly oznacza, że wartość można przypisać
        /// tylko podczas deklaracji lub w konstruktorze.
        /// </summary>
        private readonly string _plec;

        /// <summary>
        /// Konstruktor okna SecondWindow.
        /// Inicjalizuje kontrolki i zapisuje przekazaną płeć.
        /// </summary>
        /// <param name="plec">
        /// Płeć wybrana w pierwszym oknie: M, K lub I.
        /// </param>
        public SecondWindow(string plec)
        {
            // Wczytuje elementy interfejsu z pliku SecondWindow.xaml.
            InitializeComponent();

            // Zapisuje płeć przekazaną z pierwszego okna.
            // Będzie ona częścią zapisywanej odpowiedzi.
            _plec = plec;
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku Zapisz.
        /// Sprawdza poprawność wieku, pobiera wybrany kolor,
        /// zapisuje odpowiedź i otwiera okno wyników.
        /// </summary>
        /// <param name="sender">
        /// Obiekt, który wywołał zdarzenie, czyli przycisk Zapisz.
        /// </param>
        /// <param name="e">
        /// Argumenty zdarzenia kliknięcia.
        /// </param>
        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdza, czy tekst z pola WiekBox można zamienić
            // na liczbę całkowitą i czy wiek mieści się w zakresie 1-120.
            if (!int.TryParse(WiekBox.Text, out int wiek)
                || wiek < 1 || wiek > 120)
            {
                // Wyświetla komunikat, jeśli wiek jest niepoprawny.
                MessageBox.Show(
                    "Wprowadź poprawny wiek (1-120).",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                // Kończy wykonywanie metody.
                // Niepoprawna odpowiedź nie zostanie zapisana.
                return;
            }

            // Sprawdza, czy użytkownik wybrał kolor z listy.
            // SelectedItem zwraca aktualnie wybrany element ComboBox.
            // 'is not' sprawdza, czy element nie jest typu ComboBoxItem.
            if (KolorBox.SelectedItem is not ComboBoxItem wybranyKolor)
            {
                // Informuje użytkownika, że musi wybrać kolor.
                MessageBox.Show("Wybierz kolor.");

                // Przerywa działanie metody, jeśli kolor nie został wybrany.
                return;
            }

            // Tworzy nowy obiekt reprezentujący odpowiedź ankietową.
            SurveyEntry odpowiedz = new SurveyEntry
            {
                // Przypisuje płeć zapamiętaną z pierwszego okna.
                Plec = _plec,

                // Przypisuje poprawny wiek odczytany z pola tekstowego.
                Wiek = wiek,

                // Pobiera tekst wybranego koloru z elementu ComboBox.
                // ToString() zamienia wartość na tekst.
                Kolor = wybranyKolor.Content.ToString(),

                // Odczytuje stan pola zgody.
                // true oznacza zaznaczenie, false oznacza brak zgody.
                Zgoda = ZgodaCheck.IsChecked == true
            };

            // Przekazuje odpowiedź do klasy SurveyData.
            // Metoda Dodaj zapisuje ją w pamięci oraz w pliku ankieta.txt.
            SurveyData.Dodaj(odpowiedz);

            // Tworzy nowe okno, w którym wyświetlane są statystyki.
            ThirdWindow wyniki = new ThirdWindow();

            // Wyświetla okno wyników.
            wyniki.Show();

            // Zamyka bieżące okno formularza.
            this.Close();
        }
    }
}
