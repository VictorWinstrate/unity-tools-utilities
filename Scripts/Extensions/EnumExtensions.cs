using System;

public static class EnumExtensions
{
	const string _argumentExceptionFormat = "{0} must be an enumerated type";

	public static bool HasNext<T>(this T src) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException(
				string.Format(_argumentExceptionFormat, typeof(T)));
		}

		var array = (T[]) Enum.GetValues(src.GetType());
		int j = Array.IndexOf(array, src) + 1;

		return j < array.Length;
	}

	public static T Next<T>(this T src) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException(
				string.Format(_argumentExceptionFormat, typeof(T)));
		}

		var array = (T[]) Enum.GetValues(src.GetType());
		int j = Array.IndexOf(array, src) + 1;
		return (array.Length == j) ? array[0] : array[j];
	}

	public static bool HasPrev<T>(this T src) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException(
				string.Format(_argumentExceptionFormat, typeof(T)));
		}

		var array = (T[]) Enum.GetValues(src.GetType());
		int j = Array.IndexOf(array, src) - 1;

		return j >= 0;
	}

	public static T Prev<T>(this T src) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException(
				string.Format(_argumentExceptionFormat, typeof(T)));
		}

		var array = (T[]) Enum.GetValues(src.GetType());
		int j = Array.IndexOf(array, src) - 1;

		return (j == -1)
			? array[array.Length - 1]
			: array[j];
	}
}