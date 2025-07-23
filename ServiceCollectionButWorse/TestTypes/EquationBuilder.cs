namespace ServiceCollectionButWorse.TestTypes;

public class EquationBuilder(Operator @operator)
{
    private readonly Dictionary<char, Func<int, int, int>> operations = new()
    {
        { '+', (x, y) => x + y },
        { '-', (x, y) => x - y },
        { '/', (x, y) => x / y },
        { '*', (x, y) => x * y },
    };

    public Func<int, int, int> GetEquationDelegate() => operations[@operator.Value];

    private Operator? _value;
    public Operator Operator { get => _value is null ? @operator : _value; set => _value = value; }
}
