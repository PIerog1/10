using System;

public class KontoBankowe
{
    public decimal Saldo { get; private set; }
    public string NumerKonta { get; }
    private List<string> Historia { get; } = new List<string>();

    public KontoBankowe()
    {
        NumerKonta = GenerujNumerKonta();
        Saldo = 0;
        ZapiszOperacje("Utworzono konto bankowe");
    }

    private string GenerujNumerKonta()
    {
        Random rnd = new Random();
        return $"PL{rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)} {rnd.Next(1000, 9999)}";
    }


    public bool Wplata(decimal kwota)
    {
        return Wplata(kwota, "Wpłata gotówki");
    }

    public bool Wplata(decimal kwota, string opis)
    {
        if (kwota <= 0)
            return false;

        Saldo += kwota;
        ZapiszOperacje($"{opis}: +{kwota} zł, Saldo: {Saldo} zł");
        return true;
    }

    public bool Wyplata(decimal kwota)
    {
        return Wyplata(kwota, "Wypłata gotówki");
    }

    public bool Wyplata(decimal kwota, string opis)
    {
        if (kwota <= 0 || kwota > Saldo)
            return false;

        Saldo -= kwota;
        ZapiszOperacje($"{opis}: -{kwota} zł, Saldo: {Saldo} zł");
        return true;
    }

    public List<string> PobierzOstatnieOperacje(int liczba)
    {
        if (Historia.Count == 0)
            return new List<string>();

        int start = Math.Max(0, Historia.Count - liczba);
        return Historia.GetRange(start, Historia.Count - start);
    }

    private void ZapiszOperacje(string opis)
    {
        string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        Historia.Add($"[{data}] {opis}");
    }
}

public class Program
{
    public static void Main()
    {
        var konto = new KontoBankowe();

        Console.WriteLine("=== Symulacja konta bankowego ===");
        Console.WriteLine($"Numer konta: {konto.NumerKonta}");
        Console.WriteLine($"Aktualne saldo: {konto.Saldo} zł\n");

        while (true)
        {
            Console.WriteLine("Wybierz operację:");
            Console.WriteLine("1 - Wpłać");
            Console.WriteLine("2 - Wypłać");
            Console.WriteLine("3 - Historia (ostatnie 5)");
            Console.WriteLine("4 - Wyjście");
            Console.Write("Twój wybór: ");
            string wybor = Console.ReadLine();
            Console.WriteLine();

            if (wybor == "1")
            {
                Console.Write("Podaj kwotę do wpłaty: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota))
                {
                    if (konto.Wplata(kwota))
                        Console.WriteLine(" Wpłata zakończona sukcesem.");
                    else
                        Console.WriteLine(" Niepoprawna kwota!");
                }
                else
                {
                    Console.WriteLine(" Wprowadź poprawną liczbę!");
                }
            }
            else if (wybor == "2")
            {
                Console.Write("Podaj kwotę do wypłaty: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota))
                {
                    if (konto.Wyplata(kwota))
                        Console.WriteLine(" Wypłata zakończona sukcesem.");
                    else
                        Console.WriteLine(" Brak środków lub niepoprawna kwota!");
                }
                else
                {
                    Console.WriteLine(" Wprowadź poprawną liczbę!");
                }
            }
            else if (wybor == "3")
            {
                var operacje = konto.PobierzOstatnieOperacje(5);
                Console.WriteLine("=== Ostatnie operacje ===");
                if (operacje.Count == 0)
                    Console.WriteLine("Brak historii operacji.");
                else
                {
                    foreach (var op in operacje)
                        Console.WriteLine(op);
                }
            }
            else if (wybor == "4")
            {
                Console.WriteLine("\nDziękujemy za skorzystanie z aplikacji! ");
                break;
            }
            else
            {
                Console.WriteLine(" Nieprawidłowy wybór!");
            }

            Console.WriteLine($"\nAktualne saldo: {konto.Saldo} zł\n");
        }
    }
}