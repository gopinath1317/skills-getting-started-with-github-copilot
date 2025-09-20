# C# Trader Solution

This directory contains the C# solution for the trader city visiting optimization problem.

## Problem Statement
A trader wants to travel to N cities and maximize the number of cities where they can do business while maintaining a non-negative profit balance.

## Solution Approach
The solution uses an optimized greedy algorithm with profit prioritization:
1. Sort cities by profit/loss in descending order (most profitable first)
2. Greedily select cities that keep the balance non-negative
3. Time Complexity: O(N log N), Space Complexity: O(N)

## Test Cases
- Case 1: [4, -8, 3] → Expected: 2 ✅
- Case 2: [6, -5, -3, -2] → Expected: 3 ✅
- Case 3: [6, -6, 3, -5, 3, -5] → Expected: 5 ✅

## Files
- `OptimizedTrader/` - Main optimized solution with input/output handling
- `TraderProblem/` - Development version with multiple algorithms for comparison

## Usage
```bash
cd OptimizedTrader
dotnet run test          # Run test cases
echo -e "3\n4\n-8\n3" | dotnet run  # Run with input
```