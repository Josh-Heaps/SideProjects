namespace ServiceCollectionButWorse.TestTypes;

public class EquationExecutor(FirstNumber firstNumber, SecondNumber secondNumber)
{
    public int ExecuteEquation(Func<int, int, int> func) => func(firstNumber.Value, secondNumber.Value);
    public FirstNumber FirstNumber => firstNumber;
    public SecondNumber SecondNumber => secondNumber;
}
