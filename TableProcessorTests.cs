using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TableLibrary;

namespace TableLibrary.Tests
{
    [TestFixture] // Atrybut NUnit, który oznacza klasę jako zestaw testów
    public class TableProcessorTests
    {
        /// <summary>
        /// Testuje metodę ParseTables, sprawdzając, czy rzuca wyjątek ArgumentException w przypadku nieprawidłowego formatu wejściowego.
        /// </summary>
        [Test]
        public void ParseTables_InvalidInput_ThrowsException()
        {
            // Nieprawidłowy format wejściowy (brak zamykającego nawiasu klamrowego)
            string input = "[[5, 3, 9], [\"Apple\", \"Banana\"]";

            // Oczekujemy, że metoda ParseTables rzuci wyjątek typu ArgumentException
            Assert.Throws<ArgumentException>(() => TableProcessor.ParseTables(input));
        }

        /// <summary>
        /// Testuje metodę ParseTables, sprawdzając, czy poprawnie parsuje dane wejściowe w poprawnym formacie.
        /// </summary>
        [Test]
        public void ParseTables_ValidInput_ReturnsParsedTables()
        {
            // Poprawny format wejściowy
            string input = "[[5, 3, 9], [\"Apple\", \"Banana\"]]";

            // Oczekiwany wynik po parsowaniu
            var expected = new List<List<object>>
            {
                new() { 5, 3, 9 },
                new() { "Apple", "Banana" }
            };

            // Parsowanie danych wejściowych
            var result = TableProcessor.ParseTables(input);

            // Sprawdzenie, czy wynik nie jest null
            Assert.That(result, Is.Not.Null);
            // Sprawdzenie, czy wynikowa liczba tabel jest taka sama jak oczekiwana
            Assert.That(result, Has.Count.EqualTo(expected.Count));

            // Porównanie każdej tabeli z wynikiem
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.That(result[i], Is.EqualTo(expected[i]));
            }
        }

        /// <summary>
        /// Testuje metodę ParseTables, sprawdzając, czy w przypadku nieprawidłowego formatu wejściowego zwraca pustą listę.
        /// </summary>
        [Test]
        public void ParseTables_InvalidInput_ReturnsEmptyList()
        {
            // Nieprawidłowy format wejściowy (brak zamykającego nawiasu klamrowego)
            string input = "[[5, 3, 9], [\"Apple\", \"Banana\"]";

            // Parsowanie danych wejściowych
            var result = TableProcessor.ParseTables(input);

            // Sprawdzenie, czy wynikowa lista jest pusta
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Testuje metodę SortTables, sprawdzając, czy poprawnie sortuje tabele.
        /// </summary>
        [Test]
        public void SortTables_ValidInput_ReturnsSortedTables()
        {
            // Lista tabel przed sortowaniem
            var tables = new List<List<object>>
            {
                new() { 5, 3, 9 },
                new() { "Apple", "Banana" }
            };

            // Oczekiwana lista tabel po sortowaniu
            var expected = new List<List<object>>
            {
                new() { "Apple", "Banana" },
                new() { 3, 5, 9 }
            };

            // Sortowanie tabel
            var result = TableProcessor.SortTables(tables);

            // Sprawdzenie, czy liczba tabel jest zgodna z oczekiwaną
            Assert.That(result, Has.Count.EqualTo(expected.Count));

            // Porównanie każdej tabeli z wynikiem
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.That(result[i], Is.EqualTo(expected[i]));
            }
        }

        // Implementacja metody ParseTables
        /// <summary>
        /// Parsuje ciąg wejściowy JSON do listy list obiektów.
        /// </summary>
        /// <param name="input">Ciąg wejściowy w formacie JSON, zawierający tabele.</param>
        /// <returns>Lista tabel jako lista list obiektów. Jeśli format jest nieprawidłowy, rzuca ArgumentException.</returns>
        public static List<List<object>> ParseTables(string input)
        {
            try
            {
                // Próbujemy deserializować wejście JSON do listy list obiektów
                var result = JsonConvert.DeserializeObject<List<List<object>>>(input);

                // Jeśli deserializacja się powiedzie, zwróć wynik
                if (result != null)
                {
                    return result;
                }

                // Jeśli wynik jest nullem, zwróć pustą listę
                return new List<List<object>>();
            }
            catch (JsonException)
            {
                // Jeśli wejście jest nieprawidłowe (np. niekompletne lub nieprawidłowy JSON), rzuć wyjątek ArgumentException
                throw new ArgumentException("Invalid input format.");
            }
        }
    }
}
