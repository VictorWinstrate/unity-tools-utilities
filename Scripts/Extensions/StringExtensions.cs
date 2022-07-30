// RESOURCES:
// https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity5.html
// https://youtu.be/2ogqPWJSftE
public static class StringExtensions
{
	public static bool StartsWithOptimized(this string text, string prefix)
	{
		int textIndex = 0;
		int prefixIndex = 0;

		while (textIndex < text.Length && prefixIndex < prefix.Length && text[textIndex] == prefix[prefixIndex])
		{
			textIndex++;
			prefixIndex++;
		}

		return prefixIndex == prefix.Length;
	}

	public static bool EndsWithOptimized(this string text, string suffix)
	{
		int textIndex = text.Length - 1;
		int prefixIndex = suffix.Length - 1;

		while (textIndex >= 0 && prefixIndex >= 0 && text[textIndex] == suffix[prefixIndex])
		{
			textIndex--;
			prefixIndex--;
		}

		return prefixIndex < 0;
	}

	public static bool EndsWithOptimized(this string text, char lastCharacter) =>
		!string.IsNullOrEmpty(text) && text[text.Length - 1] == lastCharacter;

	// This ContainsOptimized method utilizes the
	// Knuth-Morris-Pratt (KMP) string matching algorithm.
	public static bool ContainsOptimized(this string text, string substring)
	{
		int i = 0;
		int j = 0;

		int[] prefix = GetPrefix(substring);

		while (i < text.Length)
		{
			if (text[i] == substring[j])
			{
				i++;
				j++;
			}

			if (j == substring.Length)
			{
				return true;
			}
			else if (i < text.Length && substring[j] != text[i])
			{
				if (j != 0)
					j = prefix[j - 1];
				else
					i++;
			}
		}

		return false;
	}

	public static bool ContainsOptimized(this string text, char targetChar)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == targetChar)
				return true;
		}

		return false;
	}

	public static string Substring(this string text, int startIndex, string stopString)
	{
		if (string.IsNullOrEmpty(text))
			return string.Empty;

		if (text.ContainsOptimized(stopString))
		{
			int stopStringLocation = text.IndexOf(stopString);
			return text.Substring(startIndex, stopStringLocation);
		}

		return text.Substring(startIndex);
	}

	public static string Substring(this string text, int startIndex, char stopChar)
	{
		if (string.IsNullOrEmpty(text))
			return string.Empty;

		if (text.ContainsOptimized(stopChar))
		{
			int stopCharLocation = text.IndexOf(stopChar);
			return text.Substring(startIndex, stopCharLocation);
		}

		return text.Substring(startIndex);
	}

	// The GetPrefix method utilizes
	// the first step of KMP string matching algorithm.
	static int[] GetPrefix(string pattern)
	{
		int[] prefix = new int[pattern.Length];
		int i = 0;

		for (int j = 1; j < pattern.Length; j++)
		{
			while (i > 0 && pattern[i] != pattern[j])
				i = prefix[i];

			if (pattern[i] == pattern[j])
				i++;
		}

		return prefix;
	}
}