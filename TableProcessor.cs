using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TableLibrary
{
    public partial class TableProcessor
    {
        /// <summary>
        /// Parsuje tabele z podanego ciągu znaków. Każda tabela powinna być w formacie podobnym do `[element1, element2, ...]`.
        /// </summary>
        /// <param name="input">Ciąg znaków zawierający tabele.</param>
        /// <returns>Lista tabel, gdzie każda tabela jest reprezentowana jako lista obiektów. Zwraca null w przypadku wystąpienia wyjątku.</returns>
        public static List<List<object>>? ParseTables(string input)
        {
            var tables = new List<List<object>>();

            try
            {
                // Znajdź wszystkie pasujące tabele w podanym ciągu znaków przy użyciu zdefiniowanego wzorca regex.
                var tableMatches = MyRegex().Matches(input);
                foreach (Match tableMatch in tableMatches.Cast<Match>())
                {
                    // Wyciągnij zawartość tabeli z grupy pasującego wzorca.
                    var tableContent = tableMatch.Groups[1].Value;

                    // Podziel zawartość tabeli na pojedyncze elementy.
                    var elements = tableContent.Split(',');

                    var table = new List<object>();
                    foreach (var element in elements)
                    {
                        // Usuń niepotrzebne znaki z każdego elementu.
                        var trimmedElement = element.Trim('\'', '"', ' ');

                        // Spróbuj zrzutować element na typ całkowity.
                        if (int.TryParse(trimmedElement, out int intValue))
                        {
                            table.Add(intValue);
                        }
                        // Jeśli nie jest liczbą całkowitą, spróbuj zrzutować na typ zmiennoprzecinkowy.
                        else if (double.TryParse(trimmedElement, out double doubleValue))
                        {
                            table.Add(doubleValue);
                        }
                        // Jeśli nie jest liczbą, dodaj jako ciąg znaków.
                        else if (!string.IsNullOrEmpty(trimmedElement))
                        {
                            table.Add(trimmedElement);
                        }
                    }

                    // Dodaj tabelę do listy, jeśli zawiera jakiekolwiek elementy.
                    if (table.Count > 0)
                    {
                        tables.Add(table);
                    }
                }
            }
            catch (Exception ex)
            {
                // Wyświetl komunikat o błędzie i zwróć null w przypadku wystąpienia wyjątku.
                Console.WriteLine($"Błąd parsowania danych wejściowych: {ex.Message}");
                return null;
            }

            return tables;
        }

        /// <summary>
        /// Sortuje tabele według kilku kryteriów:
        /// 1. Sortuje elementy każdej tabeli według długości ciągu znaków, a następnie według ich wartości.
        /// 2. Sortuje tabele według sumy długości elementów (dla ciągów znaków) lub ich wartości (dla liczb).
        /// 3. Sortuje według rozmiaru tabeli.
        /// 4. Sortuje według tekstowej reprezentacji tabeli.
        /// </summary>
        /// <param name="tables">Lista tabel do posortowania.</param>
        /// <returns>Posortowana lista tabel.</returns>
        public static List<List<object>> SortTables(List<List<object>> tables)
        {
            return [.. tables
                .Select(table => table
                    // Sortuje elementy wewnątrz tabeli najpierw według długości ciągu znaków, a następnie według wartości.
                    .OrderBy(e => e is string v ? v.Length : 0)
                    .ThenBy(e => e is IComparable ? e : e.ToString())
                    .ToList())
                // Sortuje tabele według sumy elementów (długość ciągu znaków lub wartość liczbowa).
                .OrderBy(table => table.Sum(e => e is string v ? v.Length : Convert.ToDouble(e)))
                // Sortuje według liczby elementów w tabeli.
                .ThenBy(table => table.Count)
                // Sortuje według reprezentacji tekstowej tabeli.
                .ThenBy(table => string.Join(",", table))];
        }

        /// <summary>
        /// Wyświetla wszystkie tabele w konsoli.
        /// </summary>
        /// <param name="tables">Lista tabel do wyświetlenia.</param>
        public static void DisplayTables(List<List<object>> tables)
        {
            foreach (var table in tables)
            {
                // Formatuje elementy tabeli do wyświetlenia jako ciągi znaków.
                var formattedElements = table.Select(e => e is string ? $"\"{e}\"" : e.ToString());
                Console.WriteLine($"[{string.Join(", ", formattedElements)}]");
            }
        }

        /// <summary>
        /// Definiuje wyrażenie regularne do rozpoznawania tabel w ciągu znaków.
        /// </summary>
        /// <returns>Obiekt Regex do wyszukiwania tabel w formacie `[element1, element2, ...]`.</returns>
        [GeneratedRegex(@"\[(.*?)\]")]
        private static partial Regex MyRegex();
    }
}