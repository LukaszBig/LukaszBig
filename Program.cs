using System;
using System.Collections.Generic;
using TableLibrary; 

// Wyświetla instrukcje dla użytkownika dotyczące formatu danych wejściowych
Console.WriteLine("Proszę wprowadzić dane w formacie: [[element1, element2], [element3, element4]]");

#pragma warning disable CS8600 // Wyłącza ostrzeżenie dotyczące konwersji wartości null na typ nienullowalny.
// Odczyt danych wejściowych od użytkownika. Funkcja ReadLine() zwraca string? (nullable), dlatego wyłączamy ostrzeżenie.
string input = Console.ReadLine();
#pragma warning restore CS8600 // Przywraca ostrzeżenie dotyczące konwersji wartości null na typ nienullowalny.

// Parsuje dane wejściowe przy użyciu metody ParseTables z biblioteki TableLibrary.
var tables = TableProcessor.ParseTables(input);

// Sprawdza, czy wynik parsowania jest nullem lub lista jest pusta (czyli dane wejściowe były nieprawidłowe).
if (tables == null || tables.Count == 0)
{
    // Wyświetla komunikat o błędzie, jeśli dane wejściowe były nieprawidłowe, i kończy program.
    Console.WriteLine("Nieprawidłowe dane wejściowe.");
    return;
}

// Sortuje tabele przy użyciu metody SortTables z biblioteki TableLibrary.
var sortedTables = TableProcessor.SortTables(tables);

// Wyświetla posortowane tabele.
Console.WriteLine("Posortowane tabele:");
TableProcessor.DisplayTables(sortedTables);
