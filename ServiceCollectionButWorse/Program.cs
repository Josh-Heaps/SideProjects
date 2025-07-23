using ServiceCollectionButWorse;

var collection = new WorseServiceCollection();

collection.AddService<A>();
collection.AddService<B>();
collection.AddService<C>();
collection.AddService<D>();
collection.AddService<E>();
collection.AddService<F>();
collection.AddService<G>();
collection.AddService<H>();
collection.AddService<I>();
collection.AddService<J>();

var h = collection.GetService<H>();
h.Print();

try
{
    var i = collection.GetService<I>();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}