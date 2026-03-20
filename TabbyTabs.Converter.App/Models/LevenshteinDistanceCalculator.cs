using System;

namespace TabbyTabs.Converter.App.Models;

internal class LevenshteinDistanceCalculator : IEditDistanceCalculator
{
    // Code from: https://www.dotnetperls.com/levenshtein
    public int CalculateDistance(string first, string second)
    {
        int n = first.Length;
        int m = second.Length;
        int[,] d = new int[n + 1, m + 1];

        // Verify arguments.
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Initialize arrays.
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Begin looping.
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = (second[j - 1] == first[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        // Return cost.
        return d[n, m];
    }
}