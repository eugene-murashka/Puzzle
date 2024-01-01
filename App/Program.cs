using System.Diagnostics;
using System.Text;

internal static class Program
{
    private static void Main(string[] args)
    {
        new PuzzleApplication(Encoding.UTF8)
            .Run();
    }
}

internal class PuzzleApplication
{
    internal PuzzleApplication()
    {
    }

    internal PuzzleApplication(Encoding encoding)
    {
        Console.OutputEncoding = encoding;
    }

    internal void Run()
    {
        string _answer = "";
        while (_index < _maxOfNumberOfTasks)
        {
            _answer = GetAnswer();

            if (_answer == "q")
            {
                break;
            }

            MakeAReactionToAnAnswer(_answer);

            Console.WriteLine();
        }
        SayGoodbye();
    }

    #region private
    private int _index = 0;
    private int _maxOfNumberOfTasks = 10;
    private bool _isCorrectAnswer = false;

    private Statistic _statistic = Statistic.Instance;
    private (bool, long) _statisticItem;
    private Stopwatch _stopwatch = new Stopwatch();

    private IArithmeticOperation _ariOper;

    private string GetAnswer()
    {
        _ariOper = _index % 2 == 0 ? new Addition() : new Division();

        Console.WriteLine($"Решите задачу #{++_index}");
        Console.WriteLine($"{_ariOper.GetExpression()} = ?");

        var task = new Task<string>(() => Console.ReadLine());

        _stopwatch.Restart();
        task.Start();
        var answer = task.Result;
        _stopwatch.Stop();

        return answer;
    }
    private void MakeAReactionToAnAnswer(string answer)
    {
        _isCorrectAnswer = _ariOper.CheckAnswer(answer);

        if (_isCorrectAnswer)
        {
            Console.WriteLine("Правильно!");
        }
        else
        {
            Console.WriteLine($"Правильный ответ: {_ariOper.Answer}");
        }

        _statisticItem = (_isCorrectAnswer, _stopwatch.ElapsedMilliseconds);
        _statistic.Items.Add(_statisticItem);
    }
    private void SayGoodbye()
    {
        Console.WriteLine("Ваша статистика:\n" +
            $"Правильные ответы: {_statistic.PosResCount}/{_statistic.Count} - {_statistic.Grade} баллов\n" +
            $"Среднее время на ответ: {_statistic.AwerageTimer}s");

        Console.WriteLine();
        Console.WriteLine("Спасибо за работу!");
    }
    #endregion
}

internal class Statistic
{
    internal readonly List<(bool isCorrectAnswer, long timer)> Items = new List<(bool isCorrectAnswer, long timer)>();

    internal int Grade => 100 * Items.Count(s => s.Item1) / Items.Count;
    internal int Count => Items.Count;
    internal int PosResCount => Items.Count(s => s.Item1);
    internal float AwerageTimer => (float)Items.Sum(s => s.Item2) / (float)Items.Count / 1000.00F;

    internal static Statistic Instance => new Statistic();

    private Statistic() { }
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
