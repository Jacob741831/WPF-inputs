
# Ankieta WPF – System zbierania odpowiedzi i statystyk

## Opis projektu

Aplikacja desktopowa napisana w języku C# z wykorzystaniem technologii WPF (Windows Presentation Foundation).

Program umożliwia zbieranie odpowiedzi ankietowych od użytkowników, zapisywanie ich do pliku tekstowego oraz generowanie statystyk na podstawie wszystkich zgromadzonych danych.

Dane są przechowywane w pliku `ankieta.txt`, dzięki czemu odpowiedzi pozostają dostępne również po zamknięciu i ponownym uruchomieniu aplikacji.

## Technologie

- **C#** – język programowania.
- **WPF** – technologia tworzenia interfejsu graficznego.
- **XAML** – definiowanie wyglądu okien aplikacji.
- **LINQ** – przetwarzanie danych i obliczanie statystyk.
- **System.IO** – odczyt i zapis plików.
- **Microsoft.Win32.SaveFileDialog** – okno dialogowe zapisu wyników.

## Funkcjonalności

### 1. Wybór płci

Pierwsze okno umożliwia wybór jednej z trzech opcji:

- M
- K
- I

Wybrana wartość jest przekazywana do kolejnego okna.

### 2. Formularz ankiety

Drugie okno umożliwia wprowadzenie danych:

- **Wiek** – liczba całkowita z zakresu od 1 do 120.
- **Kolor** – wybór jednej z dostępnych opcji.
- **Zgoda** – pole wyboru typu CheckBox.

Po kliknięciu przycisku „Zapisz i pokaż wyniki” aplikacja sprawdza poprawność danych, zapisuje odpowiedź i otwiera okno statystyk.

### 3. Statystyki

Trzecie okno prezentuje wyniki obliczone na podstawie zapisanych odpowiedzi:

- Liczba wszystkich odpowiedzi.
- Średnia wieku.
- Najczęściej wybierany kolor.
- Liczba osób, które wybrały M.
- Liczba osób, które wybrały K.
- Liczba osób, które wybrały I.
- Liczba osób, które wyraziły zgodę.

W przypadku remisu w liczbie wskazań kolorów wybierany jest alfabetycznie pierwszy kolor.

### 4. Eksport wyników

Aplikacja umożliwia zapisanie raportu ze statystykami do pliku `.txt`.

Użytkownik wybiera nazwę pliku oraz lokalizację zapisu za pomocą standardowego okna dialogowego systemu Windows.

### 5. Dodawanie kolejnych osób

Przycisk „Dodaj kolejną osobę” umożliwia powrót do formularza ankiety.

Nowe odpowiedzi są dopisywane do istniejącego pliku, a statystyki uwzględniają również wcześniej zapisane dane.

### 6. Zerowanie danych

Aplikacja może zostać rozszerzona o funkcję usuwania wszystkich odpowiedzi.

Operacja ta czyści listę w pamięci oraz zawartość pliku `ankieta.txt`. Powinna być poprzedzona potwierdzeniem użytkownika.

## Struktura projektu

Przykładowa struktura plików:

```text
WpfApp1/
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── SecondWindow.xaml
├── SecondWindow.xaml.cs
├── ThirdWindow.xaml
├── ThirdWindow.xaml.cs
├── SurveyData.cs
├── App.xaml
├── App.xaml.cs
└── README.md
```

### Opis najważniejszych plików

| Plik | Odpowiedzialność |
|---|---|
| `MainWindow.xaml` | Interfejs pierwszego okna – wybór płci. |
| `MainWindow.xaml.cs` | Obsługa wyboru płci i przejścia do formularza. |
| `SecondWindow.xaml` | Interfejs formularza ankiety. |
| `SecondWindow.xaml.cs` | Walidacja danych i zapis odpowiedzi. |
| `ThirdWindow.xaml` | Interfejs prezentujący statystyki. |
| `ThirdWindow.xaml.cs` | Obliczanie statystyk i eksport raportu. |
| `SurveyData.cs` | Model odpowiedzi oraz odczyt, zapis i usuwanie danych. |
| `App.xaml` | Konfiguracja aplikacji WPF. |
| `App.xaml.cs` | Kod uruchomieniowy aplikacji. |

## Format danych

Odpowiedzi są zapisywane w pliku `ankieta.txt`.

Każda linia reprezentuje jedną odpowiedź, a pola są rozdzielone średnikami.

Format:

```text
Plec;Wiek;Kolor;Zgoda
```

Przykładowe dane:

```text
M;20;Czerwony;True
K;30;Niebieski;False
I;40;Czerwony;True
M;50;Zielony;True
```

Znaczenie pól:

| Pole | Typ | Opis |
|---|---|---|
| `Plec` | `string` | Wybrana płeć: M, K lub I. |
| `Wiek` | `int` | Wiek respondenta. |
| `Kolor` | `string` | Wybrany kolor. |
| `Zgoda` | `bool` | Informacja o wyrażeniu zgody. |

## Obliczanie statystyk

### Średnia wieku

Średnia jest obliczana za pomocą LINQ:

```csharp
double sredniaWieku = odpowiedzi.Average(x => x.Wiek);
```

Metoda `Average()` oblicza sumę wszystkich wieków podzieloną przez liczbę odpowiedzi.

### Najczęściej wybierany kolor

```csharp
string najczestszyKolor = odpowiedzi
    .GroupBy(x => x.Kolor)
    .OrderByDescending(g => g.Count())
    .ThenBy(g => g.Key)
    .First().Key;
```

Odpowiedzi są grupowane według koloru, a następnie sortowane według liczby wystąpień.

### Liczba odpowiedzi

Przykład zliczania osób, które wybrały M:

```csharp
int liczbaM = odpowiedzi.Count(x => x.Plec == "M");
```

Analogicznie obliczane są liczby odpowiedzi K, I oraz zgód.

## Przechowywanie danych

Plik `ankieta.txt` jest lokalnym plikiem tekstowym używanym do przechowywania odpowiedzi.

Jego lokalizacja wynika z instrukcji:

```csharp
Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "ankieta.txt");
```

Oznacza to, że plik jest tworzony w katalogu uruchomionej aplikacji, np. w katalogu kompilacji `bin/Debug/...`.

**Uwaga:** przed przeniesieniem aplikacji lub zmianą konfiguracji uruchomieniowej warto sprawdzić rzeczywistą ścieżkę pliku.

## Wymagania

- System operacyjny Windows.
- Visual Studio z obsługą programowania desktopowego .NET.
- Zainstalowany odpowiedni zestaw SDK .NET zgodny z konfiguracją projektu.
- Projekt WPF w języku C#.

## Uruchomienie projektu

1. Otwórz rozwiązanie w Visual Studio.
2. Sprawdź, czy projekt WPF jest projektem startowym.
3. Skompiluj projekt za pomocą `Build > Build Solution`.
4. Uruchom aplikację klawiszem `F5` lub `Ctrl + F5`.
5. Wypełnij formularz, aby zapisać pierwszą odpowiedź i wyświetlić statystyki.

## Możliwe kierunki rozwoju

- Wybór dowolnego koloru za pomocą kontrolki podobnej do HTML `input type="color"`.
- Zapis danych w formacie CSV lub JSON.
- Przechowywanie odpowiedzi w bazie danych SQLite.
- Dodanie wykresów prezentujących wyniki.
- Rozbudowanie walidacji danych.
- Dodanie potwierdzenia przed usunięciem wszystkich odpowiedzi.
- Rozdzielenie logiki aplikacji od interfejsu za pomocą wzorca MVVM.

## Autor

Projekt edukacyjny demonstrujący tworzenie aplikacji desktopowej w C# i WPF, obsługę kontrolek XAML, walidację danych, pracę z plikami oraz obliczanie statystyk za pomocą LINQ.
```

**Ważne:** jeśli dodana przez Ciebie funkcja `Wyzeruj()` jest już w `SurveyData.cs`, opis zerowania danych odpowiada aktualnej implementacji. Jeśli nie dodałeś jeszcze przycisku zerowania do `ThirdWindow`, README opisuje tę funkcję jako możliwość rozszerzenia aplikacji, a nie jako działający już element interfejsu.
