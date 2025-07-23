namespace ServiceCollectionButWorse.TestTypes;

public class EquationRunner(EquationBuilder builder, EquationExecutor executor)
{
    public void Run()
    {
        var equation = builder.GetEquationDelegate();

        Console.WriteLine($"{executor.FirstNumber.Value} {builder.Operator.Value} {executor.SecondNumber.Value} = " +
            $"{executor.ExecuteEquation(equation)}");
    }
}
