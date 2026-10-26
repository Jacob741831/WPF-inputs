
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApp1
{
    /// <summary>
    /// Reprezentuje jedną odpowiedź udzieloną w ankiecie.
    /// Przechowuje płeć, wiek, wybrany kolor i informację o zgodzie.
    /// </summary>
    public class SurveyEntry
    {
        /// <summary>
        /// Płeć wybrana przez użytkownika: M, K lub I.
        /// </summary>
        public string Plec { get; set; } = "";

        /// <summary>
        /// Wiek osoby w pełnych latach.
        /// </summary>
        public int Wiek { get; set; }

        /// <summary>
        /// Kolor wybrany przez użytkownika.
        /// </summary>
        public string Kolor { get; set; } = "";

        /// <summary>
        /// Określa, czy użytkownik zaznaczył zgodę.
        /// True oznacza zgodę, false jej brak.
        /// </summary>
        public bool Zgoda { get; set; }
    }

    /// <summary>
    /// Zarządza odpowiedziami ankiety:
    /// wczytuje je z pliku, dodaje nowe i usuwa wszystkie dane.
    /// </summary>
    public static class SurveyData
    {
        /// <summary>
        /// Lista odpowiedzi aktualnie przechowywanych w pamięci.
        /// Wykorzystywana do obliczania statystyk.
        /// </summary>
        public static List<SurveyEntry> Odpowiedzi { get; }
            = new List<SurveyEntry>();

        /// <summary>
        /// Pełna ścieżka do pliku z zapisanymi odpowiedziami.
        /// Plik znajduje się w katalogu uruchomionej aplikacji.
        /// readonly oznacza, że zmiennej nie można później przypisać
        /// nowej wartości.
        /// </summary>
        private static readonly string Sciezka =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ankieta.txt");

        /// <summary>
        /// Wczytuje odpowiedzi z pliku do listy Odpowiedzi.
        /// Nie przyjmuje parametrów i nie zwraca wyniku.
        /// Niepoprawne linie pomija.
        /// </summary>
        public static void Wczytaj()
        {
            // Usuwa poprzednią zawartość listy,
            // aby nie wczytać tych samych odpowiedzi ponownie.
            Odpowiedzi.Clear();

            // Jeżeli plik nie istnieje, kończy działanie metody.
            if (!File.Exists(Sciezka))
                return;

            // Odczytuje wszystkie linie z pliku.
            foreach (string linia in File.ReadAllLines(Sciezka))
            {
                // Rozdziela linię na pola oddzielone średnikami.
                string[] dane = linia.Split(';');

                // Każda odpowiedź powinna mieć dokładnie 4 pola.
                if (dane.Length != 4)
                    continue;

                // Zamienia tekst wieku na liczbę całkowitą.
                // Jeśli konwersja się nie powiedzie, pomija linię.
                if (!int.TryParse(dane[1], out int wiek))
                    continue;

                // Zamienia tekst zgody na wartość true lub false.
                if (!bool.TryParse(dane[3], out bool zgoda))
                    continue;

                // Tworzy obiekt odpowiedzi i dodaje go do listy.
                Odpowiedzi.Add(new SurveyEntry
                {
                    Plec = dane[0],
                    Wiek = wiek,
                    Kolor = dane[2],
                    Zgoda = zgoda
                });
            }
        }

        /// <summary>
        /// Dodaje odpowiedź do listy w pamięci i dopisuje ją do pliku.
        /// </summary>
        /// <param name="odpowiedz">
        /// Obiekt zawierający płeć, wiek, kolor i zgodę użytkownika.
        /// </param>
        public static void Dodaj(SurveyEntry odpowiedz)
        {
            // Dodaje odpowiedź do listy w pamięci.
            Odpowiedzi.Add(odpowiedz);

            // Przygotowuje jeden wiersz w formacie:
            // Plec;Wiek;Kolor;Zgoda
            string linia =
                $"{odpowiedz.Plec};{odpowiedz.Wiek};" +
                $"{odpowiedz.Kolor};{odpowiedz.Zgoda}";

            // Dopisuje odpowiedź na końcu pliku.
            // Environment.NewLine rozpoczyna kolejny wiersz.
            // Encoding.UTF8 zapewnia obsługę polskich znaków.
            File.AppendAllText(
                Sciezka,
                linia + Environment.NewLine,
                Encoding.UTF8);
        }

        /// <summary>
        /// Usuwa wszystkie odpowiedzi z pamięci oraz czyści plik.
        /// Po wykonaniu tej metody statystyki będą liczone od zera.
        /// </summary>
        public static void Wyzeruj()
        {
            // Usuwa wszystkie odpowiedzi z listy w pamięci.
            Odpowiedzi.Clear();

            // Tworzy pusty plik, jeśli go nie ma,
            // lub usuwa jego dotychczasową zawartość.
            File.WriteAllText(Sciezka, string.Empty, Encoding.UTF8);
        }
    }
}
