using System;
using System.IO;
using System.Text.RegularExpressions;

interface ISmartTextReader
{
    char[][] ReadFile(string path);
}

class SmartTextReader : ISmartTextReader
{
    public char[][] ReadFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        char[][] result = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }
}

// ПРОКСІ З ЛОГУВАННЯМ 
class SmartTextChecker : ISmartTextReader
{
    private SmartTextReader reader = new SmartTextReader();

    public char[][] ReadFile(string path)
    {
        Console.WriteLine("Opening file: " + path);

        char[][] data = reader.ReadFile(path);

        Console.WriteLine("File read successfully");

        int lines = data.Length;
        int chars = 0;

        foreach (var line in data)
            chars += line.Length;

        Console.WriteLine($"Lines: {lines}, Characters: {chars}");
        Console.WriteLine("Closing file: " + path);

        return data;
    }
}

// ПРОКСІ З БЛОКУВАННЯМ 
class SmartTextReaderLocker : ISmartTextReader
{
    private SmartTextReader reader = new SmartTextReader();
    private Regex regex;

    public SmartTextReaderLocker(string pattern)
    {
        regex = new Regex(pattern);
    }

    public char[][] ReadFile(string path)
    {
        if (regex.IsMatch(path))
        {
            Console.WriteLine("Access denied!");
            return null;
        }

        return reader.ReadFile(path);
    }
}

class Program
{
    static void Main()
    {
        string path = "test.txt";


        // 1. Звичайне читання
        Console.WriteLine("=== Simple Reader ===");
        ISmartTextReader reader = new SmartTextReader();
        reader.ReadFile(path);

        // 2. Проксі з логуванням
        Console.WriteLine("\n=== Checker Proxy ===");
        ISmartTextReader checker = new SmartTextChecker();
        checker.ReadFile(path);

        // 3. Проксі з блокуванням
        Console.WriteLine("\n=== Locker Proxy ===");
        ISmartTextReader locker = new SmartTextReaderLocker("test");

        locker.ReadFile(path); 
    }
}