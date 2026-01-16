namespace ApKt05;

public static class FibonacciSequenceLazy
{
	public static async IAsyncEnumerable<ulong> GenerateNumbersWaitingAsync(int count)
	{
		if (count >= 1)
		{
			yield return 0;
		}

		if (count >= 2)
		{
			yield return 1;
		}

		var prev = 0UL;
		var cur = 1UL;

		for (var i = 2; i <= count; i++)
		{
			var newCur = prev + cur;
			await Task.Delay(TimeSpan.FromMilliseconds(1 * i));
			yield return newCur;
			prev = cur;
			cur = newCur;
		}
	}
}
