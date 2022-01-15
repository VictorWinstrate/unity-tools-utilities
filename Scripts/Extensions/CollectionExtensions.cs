using System.Collections.Generic;

public static class CollectionExtensions
{
    static readonly System.Random _random = new System.Random();

    public static void Shuffle<T>(this T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + _random.Next(n - i);
            var t = array[r];

            array[r] = array[i];
            array[i] = t;
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + _random.Next(n - i);
            var t = list[r];

            list[r] = list[i];
            list[i] = t;
        }
    }

    public static T GetRandomElement<T>(this T[] array)
    {
        return array == null || array.Length == 0
            ? default
            : array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list == null || list.Count == 0
            ? default
            : list[UnityEngine.Random.Range(0, list.Count)];
    }
}