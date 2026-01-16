using System.Text;

namespace ApKt05;

public static class Logic
{
	public static async Task RunLogic()
	{
		await RunFibonacciLogicAsync();

		Console.Write(Environment.NewLine);

		await RunFileReaderAsync();
	}

	private static async Task RunFibonacciLogicAsync()
	{
		const int toGenerateNumbers = 60;
		var counter = 0;
		Console.WriteLine($"Generating {toGenerateNumbers} Fibonacci numbers");
		await foreach (var num in FibonacciSequenceLazy.GenerateNumbersWaitingAsync(toGenerateNumbers))
		{
			Console.WriteLine($"{counter++}. {num}");
		}
	}

	private static async Task RunFileReaderAsync()
	{
		var tempFilePath = Path.GetTempFileName();

		Console.WriteLine($"Writing into: {tempFilePath}");
		await PrepareFileAsync(tempFilePath);

		Console.WriteLine("Parsing it...");
		await ParseFileAsync(tempFilePath);
	}

	private static async Task ParseFileAsync(string path)
	{
		if (!File.Exists(path))
		{
			throw new FileNotFoundException($"File does not exists at path: {path}");
		}

		await using var fileReader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		using var streamReader = new StreamReader(fileReader);

		const char splitChar = '|';

		await foreach (var line in GetLinesAsync(streamReader))
		{
			if (!line.Contains(splitChar))
			{
				Console.WriteLine($"Line does not contain spilt character: NO '{splitChar}' inside \"{line}\"");
				return;
			}

			var numbers = line.AsSpan().Split(splitChar);
			var total = 0L;
			foreach (var range in numbers)
			{
				var numStr = line.AsSpan().Slice(range.Start.Value, range.End.Value - range.Start.Value);
				if (int.TryParse(numStr, out var number))
				{
					total += number;
				}
				else
				{
					Console.WriteLine($"Failed converting to number: {numStr}");
				}
			}

			Console.WriteLine($"Sum of the line: \"{line}\" = {total}");
		}
	}

	private static async IAsyncEnumerable<string> GetLinesAsync(StreamReader reader)
	{
		while (reader.Peek() != -1)
		{
			var line = await reader.ReadLineAsync();
			if (!string.IsNullOrEmpty(line))
			{
				yield return line;
			}
		}
	}

	private static readonly StringBuilder Builder = new();

	private static async Task PrepareFileAsync(string path)
	{
		const int linesToGenerate = 30;
		for (var i = 0; i < linesToGenerate; i++)
		{
			Builder.Append($"{i}|{i + Random.Shared.Next(0, 10)}{Environment.NewLine}");
		}

		await File.WriteAllTextAsync(path, Builder.ToString());
	}
}
