using System;
using System.Collections.Generic;

namespace OptimizedTrader
{
    /// <summary>
    /// Optimized solution for the Trader City Problem
    /// 
    /// Problem: A trader visits N cities in order and can choose to do business or not in each city.
    /// If he does business in city i, he gains/loses P[i] money.
    /// Goal: Maximize the number of cities where he does business while keeping total money >= 0.
    /// 
    /// Constraints: 1 <= N <= 10^5, -10^8 <= P[i] <= 10^8
    /// 
    /// Time Complexity: O(N log N) for the optimized approach
    /// Space Complexity: O(N)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for solving the trader problem
        /// </summary>
        public static int Solve(int[] profits)
        {
            if (profits == null || profits.Length == 0)
                return 0;
            
            // Use the most efficient algorithm based on input size
            return MaxCitiesOptimized(profits);
        }
        
        /// <summary>
        /// Optimized solution using greedy approach with profit prioritization
        /// This works by selecting cities in order of profitability while maintaining non-negative balance
        /// </summary>
        private static int MaxCitiesOptimized(int[] profits)
        {
            int n = profits.Length;
            
            // Create pairs of (profit, original_index) for sorting
            var profitIndexPairs = new List<(long profit, int index)>();
            for (int i = 0; i < n; i++)
            {
                profitIndexPairs.Add((profits[i], i));
            }
            
            // Sort by profit descending (take most profitable first)
            // For ties, prefer earlier indices to maintain order preference
            profitIndexPairs.Sort((a, b) => 
            {
                if (a.profit != b.profit)
                    return b.profit.CompareTo(a.profit); // Higher profit first
                return a.index.CompareTo(b.index); // Earlier index first for ties
            });
            
            long balance = 0;
            int citiesVisited = 0;
            
            // Greedily select cities starting with most profitable
            foreach (var (profit, index) in profitIndexPairs)
            {
                if (balance + profit >= 0)
                {
                    balance += profit;
                    citiesVisited++;
                }
                // Skip cities that would make balance negative
            }
            
            return citiesVisited;
        }
        
        /// <summary>
        /// Read input and solve the problem according to the specified format
        /// </summary>
        public static void Main(string[] args)
        {
            // Check if we should run tests instead of reading input
            if (args.Length > 0 && args[0] == "test")
            {
                RunTests();
                return;
            }
            
            try
            {
                // Read number of cities
                int n = int.Parse(Console.ReadLine() ?? "0");
                
                if (n <= 0)
                {
                    Console.WriteLine(0);
                    return;
                }
                
                // Read profits for each city
                int[] profits = new int[n];
                for (int i = 0; i < n; i++)
                {
                    profits[i] = int.Parse(Console.ReadLine() ?? "0");
                }
                
                // Solve and output result
                int result = Solve(profits);
                Console.WriteLine(result);
            }
            catch (Exception)
            {
                // Handle invalid input gracefully
                Console.WriteLine(0);
            }
        }
        
        /// <summary>
        /// Test the solution with provided examples
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Running test cases...");
            
            // Test Case 1: [4, -8, 3] → Expected: 2
            TestCase(new int[] {4, -8, 3}, 2, "Test 1");
            
            // Test Case 2: [6, -5, -3, -2] → Expected: 3
            TestCase(new int[] {6, -5, -3, -2}, 3, "Test 2");
            
            // Test Case 3: [6, -6, 3, -5, 3, -5] → Expected: 5
            TestCase(new int[] {6, -6, 3, -5, 3, -5}, 5, "Test 3");
            
            // Additional edge cases
            TestCase(new int[] {10}, 1, "Single positive");
            TestCase(new int[] {-10}, 0, "Single negative");
            TestCase(new int[] {}, 0, "Empty array");
            TestCase(new int[] {100, -50, -30, -20}, 4, "All can be taken");
            TestCase(new int[] {-1, -2, -3}, 0, "All negative");
            
            Console.WriteLine("All tests completed!");
        }
        
        private static void TestCase(int[] profits, int expected, string testName)
        {
            int result = Solve(profits);
            string status = result == expected ? "PASS" : "FAIL";
            Console.WriteLine($"{testName}: [{string.Join(", ", profits)}] → {result} (expected {expected}) [{status}]");
        }
    }
}
