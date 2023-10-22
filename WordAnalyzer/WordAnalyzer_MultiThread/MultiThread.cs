using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Diagnostics;

class MultiThread
{

	static void Main(string[] args)
	{
		string text = File.ReadAllText("TheWitcher.txt", Encoding.UTF8);
		Stopwatch stopwatchX = new Stopwatch();
		stopwatchX.Start();
		string[] words = GetWordsFromText(text);
		stopwatchX.Stop();
		//PrintAllWords(words); // Print all the words
		Console.WriteLine($"Time taken for operation X: {stopwatchX.Elapsed.TotalMilliseconds} ms");
		Console.WriteLine();
		
		// 1. Number of words
		Stopwatch stopwatch1 = new Stopwatch();
		stopwatch1.Start();
		int wordCount = words.Length;
		stopwatch1.Stop();
		Console.WriteLine($"1. Number of words: {wordCount}");
		Console.WriteLine($"Time taken for operation 1: {stopwatch1.Elapsed.TotalMilliseconds} ms");
		Console.WriteLine();

		if (wordCount > 0)
		{
			Thread shortestThread = new Thread(() =>
			{
				
				
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					string shortestWord = FindShortestWord(words);
					stopwatch.Stop();
					Console.WriteLine($"2. Shortest word: {shortestWord}");
					Console.WriteLine($"Time taken for operation 2: {stopwatch.Elapsed.TotalMilliseconds} ms");
					Console.WriteLine();
				
			});

			Thread longestThread = new Thread(() =>
			{
				
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					string longestWord = FindLongestWord(words);
					stopwatch.Stop();
					Console.WriteLine($"3. Longest word: {longestWord}");
					Console.WriteLine($"Time taken for operation 3: {stopwatch.Elapsed.TotalMilliseconds} ms");
					Console.WriteLine();
			});

			Thread averageThread = new Thread(() =>
			{
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					int averageWordLength = CalculateAverageWordLength(words, wordCount);
					stopwatch.Stop();
					Console.WriteLine($"4. Average word length: {averageWordLength}");
					Console.WriteLine($"Time taken for operation 4: {stopwatch.Elapsed.TotalMilliseconds} ms");
					Console.WriteLine();
			});

			Thread mostCommonThread = new Thread(() =>
			{
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					string[] mostCommonWords = FindMostCommonWords(words, 5);
					stopwatch.Stop();
					Console.Write("5. Five most common words: ");
					Console.WriteLine(string.Join(", ", mostCommonWords));
					Console.WriteLine($"Time taken for operation 5: {stopwatch.Elapsed.TotalMilliseconds} ms");
					Console.WriteLine();
			});

			Thread leastCommonThread = new Thread(() =>
			{
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					string[] leastCommonWords = FindLeastCommonWords(words, 5);
					stopwatch.Stop();
					Console.Write("6. Five least common words: ");
					Console.WriteLine(string.Join(", ", leastCommonWords));
					Console.WriteLine($"Time taken for operation 6: {stopwatch.Elapsed.TotalMilliseconds} ms");
					Console.WriteLine();

			});

			shortestThread.Start();
			longestThread.Start();
			averageThread.Start();
			mostCommonThread.Start();
			leastCommonThread.Start();

			shortestThread.Join();
			longestThread.Join();
			averageThread.Join();
			mostCommonThread.Join();
			leastCommonThread.Join();
		}

		Console.WriteLine("Press any key to exit...");
		Console.ReadKey();
	}

	static string[] GetWordsFromText(string text)
	{
		string[] words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		List<string> validWords = new List<string>();

		foreach (string word in words)
		{
			string cleanWord = Regex.Replace(word, "[^а-яА-Я]", ""); // Remove non-alphabet characters
			cleanWord = cleanWord.Trim(); // Remove leading/trailing spaces
			if (!string.IsNullOrEmpty(cleanWord) && cleanWord.Length >= 3) // Check for word length and non-empty
			{
				validWords.Add(cleanWord);
			}
		}

		return validWords.ToArray();
	}



	static string FindShortestWord(string[] words)
	{
		string shortestWord = words[0];
		foreach (string word in words)
		{
			if (word.Length < shortestWord.Length)
			{
				shortestWord = word;
			}
		}
		return shortestWord;
	}

	static string FindLongestWord(string[] words)
	{
		string longestWord = words[0];
		foreach (string word in words)
		{
			if (word.Length > longestWord.Length)
			{
				longestWord = word;
			}
		}
		return longestWord;
	}

	static int CalculateAverageWordLength(string[] words, int wordCount)
	{
		int totalLength = 0;
		int averageLength;
		foreach (string word in words)
		{
			totalLength += word.Length;
		}
		averageLength = totalLength / wordCount;
		return averageLength;
	}

	static string[] FindMostCommonWords(string[] words, int count)
	{
		Dictionary<string, int> wordCount = new Dictionary<string, int>();
		foreach (string word in words)
		{
			if (wordCount.ContainsKey(word))
			{
				wordCount[word]++;
			}
			else
			{
				wordCount[word] = 1;
			}
		}

		string[] mostCommonWords = new string[count];
		for (int i = 0; i < count; i++)
		{
			mostCommonWords[i] = FindMaxWord(wordCount);
			wordCount.Remove(mostCommonWords[i]);
		}

		return mostCommonWords;
	}

	static string[] FindLeastCommonWords(string[] words, int count)
	{
		Dictionary<string, int> wordCount = new Dictionary<string, int>();
		foreach (string word in words)
		{
			if (wordCount.ContainsKey(word))
			{
				wordCount[word]++;
			}
			else
			{
				wordCount[word] = 1;
			}
		}

		string[] leastCommonWords = new string[count];
		for (int i = 0; i < count; i++)
		{
			leastCommonWords[i] = FindMinWord(wordCount);
			wordCount.Remove(leastCommonWords[i]);
		}

		return leastCommonWords;
	}

	static string FindMaxWord(Dictionary<string, int> wordCount)
	{
		string maxWord = null;
		int maxCount = 0;
		foreach (var pair in wordCount)
		{
			if (pair.Value > maxCount)
			{
				maxCount = pair.Value;
				maxWord = pair.Key;
			}
		}
		return maxWord;
	}

	static string FindMinWord(Dictionary<string, int> wordCount)
	{
		string minWord = null;
		int minCount = 2;
		foreach (var pair in wordCount)
		{
			if (pair.Value < minCount)
			{
				minCount = pair.Value;
				minWord = pair.Key;
			}
		}
		return minWord;
	}

	static void PrintAllWords(string[] words)
	{
		foreach (string word in words)
		{
			Console.WriteLine(word);
		}
	}

}