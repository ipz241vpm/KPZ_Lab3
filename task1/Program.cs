using System;
using System.IO;

// 1. Базовий Logger (працює з консоллю)
class Logger
{
    public virtual void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[LOG] " + message);
        Console.ResetColor();
    }

    public virtual void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERROR] " + message);
        Console.ResetColor();
    }

    public virtual void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN] " + message);
        Console.ResetColor();
    }
}

// 2. Клас для запису у файл
class FileWriter
{
    private string path;

    public FileWriter(string path)
    {
        this.path = path;
    }

    public void Write(string text)
    {
        File.AppendAllText(path, text);
    }

    public void WriteLine(string text)
    {
        File.AppendAllText(path, text + Environment.NewLine);
    }
}

// 3. Adapter (робить з FileWriter Logger)
class FileLoggerAdapter : Logger
{
    private FileWriter fileWriter;

    public FileLoggerAdapter(FileWriter fileWriter)
    {
        this.fileWriter = fileWriter;
    }

    public override void Log(string message)
    {
        fileWriter.WriteLine("[LOG] " + message);
    }

    public override void Error(string message)
    {
        fileWriter.WriteLine("[ERROR] " + message);
    }

    public override void Warn(string message)
    {
        fileWriter.WriteLine("[WARN] " + message);
    }
}

class Program
{
    static void Main()
    {
        // Звичайний логер (консоль)
        Logger consoleLogger = new Logger();
        consoleLogger.Log("Це звичайний лог");
        consoleLogger.Error("Це помилка");
        consoleLogger.Warn("Це попередження");

        Console.WriteLine("\n--- Запис у файл ---\n");

        // Файловий логер через адаптер
        FileWriter writer = new FileWriter("log.txt");
        Logger fileLogger = new FileLoggerAdapter(writer);

        fileLogger.Log("Лог у файл");
        fileLogger.Error("Помилка у файл");
        fileLogger.Warn("Попередження у файл");

        Console.WriteLine("Запис завершено. Перевір файл log.txt");
    }
}