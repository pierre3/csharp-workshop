using ConsoleAppFramework;
namespace CSharp14;
class MyApp
{
    [Command("extension")]
    public void ExtensionMembers()
    {
        var str = "Hello, World!";
        Console.WriteLine(str.Quoted);

        IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine(numbers.IsEmpty);
        Console.WriteLine(IEnumerable<int>.CountUp(5).Take(50).Dump());
    }

    [Command("bk-field")]
    public void BackingField() 
    { 
        var sample = new BkFieldSample();
        sample.Name = "taro";
        sample.Name2 = "jiro";
        Console.WriteLine(sample.Name);
        Console.WriteLine(sample.Name2);
    }

    [Command("lambda-params")]
    public void LambdaParams() 
    { 
        //これは出来た
        Delegate1 func1 = (ref string arg1, in string arg2, out string arg3) => arg3= arg1 + arg2;
        //これができるようになった
        Delegate1 func2 = (ref arg1, in arg2, out arg3) => arg3 = arg1 + arg2;
        var arg1 = "Hello, ";
        var arg2 = "World!";

        Console.WriteLine(func2(ref arg1, in arg2, out var arg3) + " " + arg3);
    }

    [Command("null-conditional-assignment")]
    public void NullConditionalAssignment()
    {
        var bk = new BkFieldSample();
        if(bk is not null)
        {
            bk.Name = "saburo";
        }
        //これができるようになった
        bk?.Name2 = "shiro";

        Console.WriteLine(bk.Name + " " + bk.Name2);
    }
}


delegate string Delegate1(ref string arg1, in string arg2, out string arg3);
