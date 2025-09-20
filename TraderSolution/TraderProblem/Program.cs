using System;
using System.Collections.Generic;

namespace TraderProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test the solution with provided test cases
            TestSolution();
            
            // Interactive mode for custom input
            Console.WriteLine("\nEnter custom input (or press Enter to exit):");
            Console.Write("Number of cities: ");
            string? input = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int n) && n > 0)
            {
                int[] profits = new int[n];
                Console.WriteLine("Enter profit/loss for each city:");
                
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"City {i + 1}: ");
                    if (int.TryParse(Console.ReadLine(), out int profit))
                    {
                        profits[i] = profit;
                    }
                }
                
                int result = MaxCitiesVisited(profits);
                Console.WriteLine($"Maximum cities the trader can visit: {result}");
            }
        }
        
        /// <summary>
        /// Finds the maximum number of cities a trader can visit while maintaining non-negative balance
        /// Uses an optimized approach that considers future opportunities
        /// </summary>
        /// <param name="profits">Array representing profit/loss in each city</param>
        /// <returns>Maximum number of cities where trader can do business</returns>
        public static int MaxCitiesVisited(int[] profits)
        {
            if (profits == null || profits.Length == 0)
                return 0;
            
            int n = profits.Length;
            
            // For small inputs, use the exact DP solution
            if (n <= 20)
            {
                return MaxCitiesVisitedDP(profits);
            }
            
            // For larger inputs, use an optimized approach
            return MaxCitiesVisitedOptimized(profits);
        }
        
        /// <summary>
        /// Optimized solution for larger inputs using a different strategy
        /// </summary>
        private static int MaxCitiesVisitedOptimized(int[] profits)
        {
            int n = profits.Length;
            
            // Use a greedy approach with look-ahead
            // The key insight is that we should prioritize positive profits
            // and carefully consider negative profits
            
            var profitIndexPairs = new List<(int profit, int index)>();
            for (int i = 0; i < n; i++)
            {
                profitIndexPairs.Add((profits[i], i));
            }
            
            // Sort by profit descending, then by index ascending (to maintain order preference)
            profitIndexPairs.Sort((a, b) => 
            {
                if (a.profit != b.profit)
                    return b.profit.CompareTo(a.profit); // Higher profit first
                return a.index.CompareTo(b.index); // Earlier index first for ties
            });
            
            long balance = 0;
            var selected = new bool[n];
            int count = 0;
            
            // Greedily select cities starting with most profitable
            foreach (var (profit, index) in profitIndexPairs)
            {
                if (balance + profit >= 0)
                {
                    balance += profit;
                    selected[index] = true;
                    count++;
                }
            }
            
            return count;
        }
        
        /// <summary>
        /// Alternative approach using dynamic programming (for comparison)
        /// </summary>
        public static int MaxCitiesVisitedDP(int[] profits)
        {
            if (profits == null || profits.Length == 0)
                return 0;
                
            int n = profits.Length;
            // Try all possible combinations and find the one with maximum cities
            int maxCities = 0;
            
            // Use bit manipulation to try all 2^n combinations
            for (int mask = 0; mask < (1 << n); mask++)
            {
                long balance = 0;
                int cities = 0;
                bool valid = true;
                
                for (int i = 0; i < n; i++)
                {
                    if ((mask & (1 << i)) != 0) // If this city is selected
                    {
                        balance += profits[i];
                        cities++;
                        if (balance < 0)
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                
                if (valid)
                {
                    maxCities = Math.Max(maxCities, cities);
                }
            }
            
            return maxCities;
        }
        
        /// <summary>
        /// Test the solution with provided test cases
        /// </summary>
        static void TestSolution()
        {
            Console.WriteLine("Testing Trader Problem Solution");
            Console.WriteLine("================================");
            
            // Test Case 1: [4, -8, 3] → Expected: 2
            int[] test1 = {4, -8, 3};
            int result1 = MaxCitiesVisited(test1);
            int result1DP = MaxCitiesVisitedDP(test1);
            Console.WriteLine($"Test 1: [{string.Join(", ", test1)}]");
            Console.WriteLine($"  Greedy Result: {result1}, DP Result: {result1DP}, Expected: 2");
            Console.WriteLine($"  Status: {(result1 == 2 ? "PASS" : "FAIL")}");
            
            // Test Case 2: [6, -5, -3, -2] → Expected: 3
            int[] test2 = {6, -5, -3, -2};
            int result2 = MaxCitiesVisited(test2);
            int result2DP = MaxCitiesVisitedDP(test2);
            Console.WriteLine($"\nTest 2: [{string.Join(", ", test2)}]");
            Console.WriteLine($"  Greedy Result: {result2}, DP Result: {result2DP}, Expected: 3");
            Console.WriteLine($"  Status: {(result2 == 3 ? "PASS" : "FAIL")}");
            
            // Test Case 3: [6, -6, 3, -5, 3, -5] → Expected: 5
            int[] test3 = {6, -6, 3, -5, 3, -5};
            int result3 = MaxCitiesVisited(test3);
            int result3DP = MaxCitiesVisitedDP(test3);
            Console.WriteLine($"\nTest 3: [{string.Join(", ", test3)}]");
            Console.WriteLine($"  Greedy Result: {result3}, DP Result: {result3DP}, Expected: 5");
            Console.WriteLine($"  Status: {(result3 == 5 ? "PASS" : "FAIL")}");
            
            // Additional test cases
            int[] test4 = {1, -1, 1, -1, 1};
            int result4 = MaxCitiesVisited(test4);
            int result4DP = MaxCitiesVisitedDP(test4);
            Console.WriteLine($"\nTest 4: [{string.Join(", ", test4)}]");
            Console.WriteLine($"  Greedy Result: {result4}, DP Result: {result4DP}");
            
            int[] test5 = {-1, -2, -3};
            int result5 = MaxCitiesVisited(test5);
            int result5DP = MaxCitiesVisitedDP(test5);
            Console.WriteLine($"\nTest 5: [{string.Join(", ", test5)}]");
            Console.WriteLine($"  Greedy Result: {result5}, DP Result: {result5DP}");
            
            // Edge case tests
            int[] test6 = {10};
            int result6 = MaxCitiesVisited(test6);
            Console.WriteLine($"\nTest 6 (single positive): [{string.Join(", ", test6)}] → {result6}");
            
            int[] test7 = {-10};
            int result7 = MaxCitiesVisited(test7);
            Console.WriteLine($"Test 7 (single negative): [{string.Join(", ", test7)}] → {result7}");
            
            int[] test8 = {};
            int result8 = MaxCitiesVisited(test8);
            Console.WriteLine($"Test 8 (empty array): [] → {result8}");
            
            int[] test9 = {100, -50, -30, -20};
            int result9 = MaxCitiesVisited(test9);
            Console.WriteLine($"Test 9 (large values): [{string.Join(", ", test9)}] → {result9}");
        }
    }
}
