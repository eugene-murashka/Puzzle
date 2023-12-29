using System.Diagnostics;

internal class Program
{
    private static readonly List<(bool, long)> Statistic = new List<(bool, long)> ();

    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var r = new Random();
        var index = 0;
        var isCorrectAnswer = false;

        while (true)
        {
            if(index > 10)
            {
                break;
            }

            var nam1 = r.Next(2, 10);
            var nam2 = r.Next(2, 10);

            Console.WriteLine($"Решите задачу #{++index}");
            Console.WriteLine($"{nam1} * {nam2} = ?");

            var task = new Task<string>(() => Console.ReadLine());

            var sw = Stopwatch.StartNew();
            task.Start();
            var answer = await task;
            sw.Stop();

            if (answer == "q")
            {
                break;
            }

            isCorrectAnswer = answer == (nam1 * nam2).ToString();

            if (isCorrectAnswer)
            {
                Console.WriteLine("Правильно!");
            }
            else
            {
                Console.WriteLine($"Правильный ответ: {nam1 * nam2}");
            }

            Statistic.Add((isCorrectAnswer, sw.ElapsedMilliseconds));

            Console.WriteLine();
        }

        // Статистика правильных ответов
        // Статистика среднего времени на ответ

        Console.WriteLine("Ваша статистика:\n" +
            $"Правильные ответы: {Statistic.Count(s => s.Item1)}/{Statistic.Count} - {(float)(Statistic.Count(s => s.Item1))/(float)(Statistic.Count)}%\n" +
            $"Среднее время на ответ: {Statistic.Sum(s => s.Item2)/Statistic.Count/1000.00}s");

        Console.WriteLine();
        Console.WriteLine("Спасибо за работу!");
    }
}