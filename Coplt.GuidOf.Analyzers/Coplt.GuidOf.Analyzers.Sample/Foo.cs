using System;
using System.Runtime.InteropServices;
using Coplt.GuidOfs;

namespace Coplt.GuidOf.Analyzers.Sample;

[Guid("9B055807-B4EB-4E17-8C7A-A34C97B68F85")]
public class Foo<T, U> where T : U
{
    public static void Some()
    {
        var r = GuidType.Of<Foo<T, U>>().Get();
        Console.WriteLine(r);
    }
}
