namespace ServiceCollectionButWorse;

public class A
{
    public void Print() { Console.WriteLine("A"); }
}

public class B
{
    public void Print() { Console.WriteLine("B"); }
}

public class C
{
    public void Print() { Console.WriteLine("C"); }
}

public class D(A a, C c)
{
    public void Print()
    {
        Console.WriteLine("D");
        a.Print();
        c.Print();
        Console.WriteLine();
    }
}
public class E(A a, B b)
{
    public void Print()
    {
        Console.WriteLine("E");
        a.Print();
        b.Print();
        Console.WriteLine();
    }
}
public class F(B b, C c)
{
    public void Print()
    {
        Console.WriteLine("F");
        b.Print();
        c.Print();
        Console.WriteLine();
    }
}
public class G(A a, B b, C c)
{
    public void Print()
    {
        Console.WriteLine("G");
        a.Print();
        b.Print();
        c.Print();
        Console.WriteLine();
    }
}
public class H(D d, E e, F f, G g)
{
    public void Print()
    {
        Console.WriteLine("H");
        d.Print();
        e.Print();
        f.Print();
        g.Print();
        Console.WriteLine();
    }
}

public class I(J j) { }

public class J(I i) { }