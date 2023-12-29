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

            IArithmeticOperation ariOper = index % 2 == 0 ? new Addition() : new Division();

            Console.WriteLine($"Решите задачу #{++index}");
            Console.WriteLine($"{ariOper.GetExpression()} = ?");

            var task = new Task<string>(() => Console.ReadLine());

            var sw = Stopwatch.StartNew();
            task.Start();
            var answer = await task;
            sw.Stop();

            if (answer == "q")
            {
                break;
            }

            isCorrectAnswer = ariOper.CheckAnswer(answer);

            if (isCorrectAnswer)
            {
                Console.WriteLine("Правильно!");
            }
            else
            {
                Console.WriteLine($"Правильный ответ: {ariOper.Answer}");
            }

            Statistic.Add((isCorrectAnswer, sw.ElapsedMilliseconds));

            Console.WriteLine();
        }

        // Статистика правильных ответов
        // Статистика среднего времени на ответ

        Console.WriteLine("Ваша статистика:\n" +
            $"Правильные ответы: {Statistic.Count(s => s.Item1)}/{Statistic.Count} - {(float)(Statistic.Count(s => s.Item1))/(float)(Statistic.Count)*100}%\n" +
            $"Среднее время на ответ: {Statistic.Sum(s => s.Item2)/Statistic.Count/1000.00}s");

        Console.WriteLine();
        Console.WriteLine("Спасибо за работу!");
    }

    public interface IArithmeticOperation
    {
        string GetExpression();
        bool CheckAnswer(string answer);
        int Answer { get; }
    }

    internal abstract class ArithmeticOperation
    {
        internal ArithmeticOperation()
        {
            var r = new Random();
            nam1 = r.Next(2, 10);
            nam2 = r.Next(2, 10);
            product = nam1 * nam2;
        }

        protected readonly int nam1;
        protected readonly int nam2;
        protected readonly int product;
    }

    internal class Addition : ArithmeticOperation, IArithmeticOperation
    {
        public int Answer => product;

        public bool CheckAnswer(string answer)
        {
            return answer == (nam1 * nam2).ToString();
        }

        public string GetExpression()
        {
            return $"{nam1} * {nam2}";
        }
    }

    internal class Division : ArithmeticOperation, IArithmeticOperation
    {
        public int Answer => nam2;

        public bool CheckAnswer(string answer)
        {
            return answer == (product / nam1).ToString();
        }

        public string GetExpression()
        {
            return $"{product} / {nam1}";
        }
    }
}