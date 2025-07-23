using ServiceCollectionButWorse;
using ServiceCollectionButWorse.TestTypes;

var collection = new WorseServiceCollection();

collection.AddService<EquationBuilder>();
collection.AddService<EquationExecutor>();
collection.AddService<EquationRunner>();
collection.AddService<FirstNumber>();
collection.AddService<Operator>();
collection.AddService<SecondNumber>();

var firstNumber = collection.GetService<FirstNumber>();
firstNumber.Value = 2;
var runner = collection.GetService<EquationRunner>();
runner.Run();