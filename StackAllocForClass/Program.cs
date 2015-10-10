using System;

public abstract class HelloClassOnStackBase
{
    private int valueBase;
    private Random random;
    private object unusedButCheckForGC1;
    private object unusedButCheckForGC2;

    protected HelloClassOnStackBase(Random random)
    {
        this.random = random;
        valueBase = (byte)random.Next();
    }

    public virtual int Compute(int x)
    {
        return valueBase + x;
    }
}

public class HelloClassOnStack : HelloClassOnStackBase
{
    private int addValue;

    public HelloClassOnStack(Random random) : base(random)
    {
        addValue = (byte)random.Next();
    }

    public override int Compute(int x)
    {
        var result = base.Compute(x);
        result += 1;
        return result;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Don't use Stopwatch as it is not in mscorlib.dll
        var startTime = DateTime.Now;
        Console.WriteLine("Mode: {0}{1}", args.Length == 0 ? "StackAlloc" : "HeapAlloc",
            args.Length == 0 ? " . To switch to HeapAlloc, simply pass an argument to this exe" : string.Empty);

        Console.WriteLine("[before] GC gen0 collect: {0}", GC.CollectionCount(0));
        Console.WriteLine("[before] GC gen1 collect: {0}", GC.CollectionCount(1));
        Console.WriteLine("[before] GC gen2 collect: {0}", GC.CollectionCount(2));

        var random = new Random(0);
        int result = 0;
        const int Count = 10000000;

        if (args.Length > 0)
        {
            for (int i = 0; i < Count; i++)
            {
                // Alloc class on heap
                var hello = new HelloClassOnStack(random);
                result += hello.Compute(i);
            }
        }
        else
        {
            for (int i = 0; i < Count; i++)
            {
                // Alloc class on stack
                var hello = stackalloc HelloClassOnStack(random);
                result += hello.Compute(i);
            }
        }

        Console.WriteLine("Result: {0}", result);

        Console.WriteLine("[after] GC gen0 collect: {0}", GC.CollectionCount(0));
        Console.WriteLine("[after] GC gen1 collect: {0}", GC.CollectionCount(1));
        Console.WriteLine("[after] GC gen2 collect: {0}", GC.CollectionCount(2));

        Console.WriteLine("Elapsed: {0}ms", (DateTime.Now - startTime).TotalMilliseconds);
    }
}