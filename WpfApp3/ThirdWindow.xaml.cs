
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Klasa reprezentująca trzecie okno aplikacji.
    /// Wyświetla statystyki ankiety, umożliwia zapisanie wyników
    /// do pliku TXT oraz rozpoczęcie ankiety dla kolejnej osoby.
    /// </summary>
    public partial class ThirdWindow : Window
    {
        /// <summary>
        /// Konstruktor okna wyników.
        /// Inicjalizuje kontrolki, wczytuje zapisane odpowiedzi
        /// z pliku i wyświetla obliczone statystyki.
        /// </summary>
        public ThirdWindow()
        {
            // Wczytuje elementy interfejsu z pliku ThirdWindow.xaml.
            InitializeComponent();

            // Odczytuje odpowiedzi z pliku ankieta.txt
            // i umieszcza je na liście SurveyData.Odpowiedzi.
            SurveyData.Wczytaj();

            // Oblicza statystyki i wyświetla je w oknie.
            PokazWyniki();
        }

        /// <summary>
        /// Oblicza statystyki na podstawie wszystkich zapisanych odpowiedzi.
        /// Zwraca jeden tekst zawierający wyniki ankiety.
        /// Nie przyjmuje parametrów.
        /// </summary>
        /// <returns>
        /// Tekst ze statystykami albo komunikat o braku odpowiedzi.
        /// </returns>
        private string GenerujWyniki()
        {
            // Pobiera listę wszystkich odpowiedzi przechowywanych w pamięci.
            // var pozwala kompilatorowi samodzielnie ustalić typ zmiennej.
            var odpowiedzi = SurveyData.Odpowiedzi;

            // Jeśli lista jest pusta, nie można obliczyć statystyk.
            if (odpowiedzi.Count == 0)
                return "Brak zapisanych odpowiedzi.";

            // Oblicza średnią arytmetyczną wieku wszystkich osób.
            // Average() sumuje wartości i dzieli sumę przez ich liczbę.
            double sredniaWieku =
                odpowiedzi.Average(x => x.Wiek);

            // Grupuje odpowiedzi według wybranego koloru.
            // GroupBy() tworzy grupy osób, które wybrały ten sam kolor.
            // OrderByDescending() sortuje grupy od największej liczby
            // wystąpień do najmniejszej.
            // ThenBy() rozstrzyga remis, sortując kolory alfabetycznie.
            // First() pobiera pierwszą grupę, a Key zwraca jej kolor.
            string najczestszyKolor = odpowiedzi
                .GroupBy(x => x.Kolor)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .First().Key;

            // Zlicza odpowiedzi osób, które wybrały płeć M.
            // x oznacza pojedynczą odpowiedź z listy.
            // Count() zlicza tylko elementy spełniające warunek.
            int liczbaM = odpowiedzi.Count(x => x.Plec == "M");

            // Zlicza odpowiedzi osób, które wybrały płeć K.
            int liczbaK = odpowiedzi.Count(x => x.Plec == "K");

            // Zlicza odpowiedzi osób, które wybrały płeć I.
            int liczbaI = odpowiedzi.Count(x => x.Plec == "I");

            // Zlicza osoby, które zaznaczyły zgodę.
            // Warunek x.Zgoda oznacza, że wartość musi być true.
            int liczbaZgod = odpowiedzi.Count(x => x.Zgoda);

            // Zwraca wszystkie wyniki jako jeden tekst.
            // \n oznacza przejście do nowej linii.
            // \n\n dodaje pusty wiersz.
            // F2 formatuje średnią do dwóch miejsc po przecinku.
            return
                $"Liczba odpowiedzi: {odpowiedzi.Count}\n\n" +
                $"Średnia wieku: {sredniaWieku:F2}\n" +
                $"Najczęstszy kolor: {najczestszyKolor}\n\n" +
                $"Liczba M: {liczbaM}\n" +
                $"Liczba K: {liczbaK}\n" +
                $"Liczba I: {liczbaI}\n\n" +
                $"Liczba zgód: {liczbaZgod}";
        }

        /// <summary>
        /// Wyświetla statystyki w kontrolce WynikiBlock.
        /// Pobiera gotowy tekst z metody GenerujWyniki().
        /// </summary>
        private void PokazWyniki()
        {
            // Ustawia tekst kontrolki TextBlock na obliczone wyniki.
            WynikiBlock.Text = GenerujWyniki();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku pobierania wyników.
        /// Otwiera okno wyboru lokalizacji pliku i zapisuje statystyki
        /// w pliku tekstowym UTF-8.
        /// </summary>
        /// <param name="sender">
        /// Obiekt, który wywołał zdarzenie, czyli przycisk.
        /// </param>
        /// <param name="e">
        /// Argumenty zdarzenia kliknięcia.
        /// </param>
        private void Pobierz_Click(object sender, RoutedEventArgs e)
        {
            // Tworzy okno dialogowe zapisu pliku.
            SaveFileDialog dialog = new SaveFileDialog
            {
                // Tytuł wyświetlany w oknie dialogowym.
                Title = "Zapisz wyniki ankiety",

                // Domyślna nazwa zapisywanego pliku.
                FileName = "wyniki_ankiety.txt",

                // Domyślne rozszerzenie pliku.
                DefaultExt = ".txt",

                // Filtr ograniczający wyświetlane pliki do TXT.
                Filter = "Pliki tekstowe (*.txt)|*.txt"
            };

            // Wyświetla okno zapisu.
            // true oznacza, że użytkownik wybrał lokalizację
            // i zatwierdził zapis.
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Zapisuje tekst statystyk do wybranego pliku.
                    // dialog.FileName zawiera pełną ścieżkę pliku.
                    // GenerujWyniki() ponownie oblicza aktualne wyniki.
                    // Encoding.UTF8 umożliwia poprawny zapis polskich znaków.
                    File.WriteAllText(
                        dialog.FileName,
                        GenerujWyniki(),
                        Encoding.UTF8);

                    // Informuje użytkownika o poprawnym zapisaniu pliku.
                    MessageBox.Show(
                        "Wyniki zapisano do pliku.",
                        "Zapis zakończony",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                // Obsługuje błędy operacji wejścia i wyjścia,
                // np. problemy z dostępem do pliku.
                catch (IOException ex)
                {
                    // Wyświetla informację o błędzie.
                    // ex.Message zawiera opis błędu.
                    MessageBox.Show(
                        $"Nie udało się zapisać pliku:\n{ex.Message}",
                        "Błąd zapisu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku dodawania kolejnej osoby.
        /// Otwiera pierwsze okno ankiety i zamyka okno wyników.
        /// Wcześniej zapisane odpowiedzi pozostają w pliku.
        /// </summary>
        /// <param name="sender">
        /// Obiekt, który wywołał zdarzenie kliknięcia.
        /// </param>
        /// <param name="e">
        /// Argumenty zdarzenia kliknięcia.
        /// </param>
        private void NowaOsoba_Click(object sender, RoutedEventArgs e)
        {
            // Tworzy nowe pierwsze okno ankiety.
            MainWindow okno = new MainWindow();

            // Wyświetla pierwsze okno.
            okno.Show();

            // Zamyka aktualne okno wyników.
            Close();
        }
    }
}
