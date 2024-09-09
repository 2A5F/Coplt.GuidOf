using System.Runtime.CompilerServices;

namespace Coplt.GuidOfs;

public readonly record struct GuidType<T>;
public readonly record struct GuidOf<T>(Guid Guid);

public static class GuidType
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GuidType<T> Of<T>() => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid Get<T>(this GuidType<T> type) => typeof(T).GUID;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GuidOf<T> AsOf<T>(this GuidType<T> type) => new(typeof(T).GUID);
}
