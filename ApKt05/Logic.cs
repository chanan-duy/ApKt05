namespace ApKt05;

public static class Logic
{
	public static async Task RunLogic()
	{
		const int toGenerateNumbers = 60;
		var counter = 0;
		Console.WriteLine($"Generating {toGenerateNumbers} Fibonacci numbers");
		await foreach (var num in FibonacciSequenceLazy.GenerateNumbersWaitingAsync(toGenerateNumbers))
		{
			Console.WriteLine($"{counter++}. {num}");
		}
	}
}
