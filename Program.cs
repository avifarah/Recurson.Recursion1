#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Recursion1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            *  Palindrome n:
            *		n = 1:	Palindrome(n) = 1 1
            *		n = 2:	Palindrome(n) = 2 1 1 2
            *		n = 3:	Palindrome(n) = 3 2 1 1 2 3
            *		etc...
            *  Write a recursive program that will take n as input and produce the list:
            *		n (n - 1) (n - 2) . . 2 1 1 2 . . (n - 2) (n - 1) n
            */
            try
            {
                int test = 5;
                var pal = Palindrome.GetPalindrome(test);
                Console.WriteLine($"Palindrome.GetPalindrome({test})");
                Console.WriteLine(string.Join(" ", pal));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing Archimedis' row 2.  Internal message: {ex.Message}");
            }

            /*
            *  Power:
            *  x ^ y find result in a recursive manner
            */
            Console.WriteLine();
            Console.WriteLine("Power:");
            var baseValue = 5;
            for (var n = -5; n <= 5; ++n)
            {
                var p = PowerOf.Power(baseValue, n);
                var trust = Math.Pow(baseValue, n);
                var check = (trust - p) / trust;
                Console.WriteLine($"PowerOf.Power({baseValue}, {n,2}): {p}{(check > 1e-15 ? $"  Check detected an error.  {trust} vs {p}" : string.Empty)}");
            }

            /*
            *  Pascal's triangle (https://en.wikipedia.org/wiki/Pascal%27s_triangle):
            *  n = 0               1
            *  n = 1             1   1
            *  n = 2           1   2   1
            *  n = 3         1   3   3   1
            *  n = 4       1   4   6   4   1
            *  n = 5      1  5  10  10   5   1
            *
            *  Note that:
            *  	(a + b)^0 = 1
            *		(a + b)^1 = 1*a + 1*b
            *		(a + b)^2 = 1*a^2 + 2*a*b + 1*b^2
            *		(a + b)^3 = 1*a^3 + 3*a^2*b + 3*a*b^2 + 1*b^3
            *  Write a recursive program that return the n'th row of Archimedis triangle
            */
            var row = 0;
            Console.WriteLine();
            Console.WriteLine($"Pascal's triangle.");
            try
            {
                for ( ; row < 6; ++row)
                {
                    var rowN = PascalsTriangle.PascalsRow(row);
                    Console.WriteLine($"Pascal's triangle row: {row}:  {string.Join(" ", rowN)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing Pascal's row {row}.  Internal message: {ex.Message}");
            }

            /*
            *	Fibonacci:
            *		Fibonacci(0) = 0
            *		Fibonacci(1) = 1
            *		Fibonacci(n) = Fibonacci(n - 1) + Fibonacci(n - 2)
            */
            var fibValue = 40;
            try
            {
                Console.WriteLine();
                Console.WriteLine("Fibonacci number value");
                var sw = Stopwatch.StartNew();

                var fib = Fibonacci.Fib(fibValue);

                sw.Stop();
                Console.WriteLine($"Fibonacci({fibValue}):  {fib:#,##0}.  (Calculated in {sw.ElapsedMilliseconds:#,##0} milliseconds)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing Fibonacci({fibValue}).  Internal message: {ex.Message}");
            }

            // If diskCount == 1
            //		MoveDisk:	A -> C
            // if diskCount == 2
            //		MoveDisk:	A -> B
            //		MoveDisk:	A -> C
            //		MoveDisk:	B -> c
            var diskCount = 3;
            try
            {
                Console.WriteLine();
                Console.WriteLine($"Tower of Hanoi with {diskCount} disks:");
                TowerOfHanoi.Peg fromPeg = TowerOfHanoi.Peg.A;
                TowerOfHanoi.Peg toPeg = TowerOfHanoi.Peg.C;
                TowerOfHanoi.Peg usingPeg = TowerOfHanoi.Peg.B;
                TowerOfHanoi.MoveTower(diskCount, fromPeg, usingPeg, toPeg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing TowerOfHanoi({diskCount}).  Internal message: {ex.Message}");
            }
        }
    }

    class Palindrome
    {
        public static List<int> GetPalindrome(int n)
        {
            if (n < 1) throw new ArgumentException($"intput given {n} is not allowed.  It is expected to be positive", nameof(n));

    		//return PalindromeHelper(n).ToList();

            // In case we do not have access to IEnumerable's yield return construct:
            var list = new List<int>();
            PalindromeHelper2(n, list);
            return list;
        }
	
        /// <summary>
        /// Assumption input to PalindromeHelper cannot be smaller than 1
        /// </summary>
        private static IEnumerable<int> PalindromeHelper(int n)
        {
            // n == 0 is the terminating condition
            if (n > 0)
            {
                yield return n;
			
                // Recursive definition: PlindromeHelper(n) = n PalindromeHelper(n - 1) n
                var innerPalindrome = PalindromeHelper(n - 1);
                foreach (var m in innerPalindrome)
                    yield return m;

                yield return n;
            }
        }

        /// <summary>
        /// Assumption input to PalindromeHelper2 cannot be smaller than 1
        /// </summary>
        private static void PalindromeHelper2(int n, List<int> palindrome)
        {
            if (n == 0) return;

    		palindrome.Add(n);

            // Recursive definition
            PalindromeHelper2(n - 1, palindrome);

            palindrome.Add(n);
        }
    }

    class PowerOf
    {
        // Terminating condition expValue == 0:	baseValue^0 = 1

        // if expValue > 0
        // 		Recursive definition:	baseValue^n = baseValue * baseValue^(expValue - 1)
        // if expValue < 0
        // 		Recursive definition:	baseValue^n = (1 / baseValue) * baseValue^(expValue + 1)
        public static double Power(int baseValue, int expValue)
        {
            if (expValue == 0)
                return 1.0;
            if (expValue > 0)
                return baseValue * Power(baseValue, expValue - 1);
            else
                return (1.0 / baseValue) * Power(baseValue, expValue + 1);
        }
    }

    class PascalsTriangle
    {
        public static IEnumerable<int> PascalsRow(int n)
        {
            if (n < 0) throw new ArgumentException($"input value of {n} is illegal.  Expected value of 0 or more", nameof(n));

            var row0 = new List<int> { 1 };
            var res = PascalRowHelper(row0, 0, n);
            return res;
        }

        private static IEnumerable<int> PascalRowHelper(IEnumerable<int> list, int current, int n)
        {
            // Terminating condition: if current count reached the limit that we are looking for then we are done
            if (n - current == 0)
                return list;

            // Process the current row
            var next = new List<int> { 1 }
                .Concat(Next2(list))
                .Concat(new List<int> { 1 });

            // Generate the next row recursively.
            return PascalRowHelper(next, current + 1, n);
        }

        /// <summary>
        /// Returns the n - 1 (list[i] + list[i+1]) items
        /// This row is missing the leading and trailing 1's
        /// </summary>
        private static List<int> Next2(IEnumerable<int> list)
        {
            // Terminating condition is list contains 1 element only
            var res = new List<int>();
            if (!list.Skip(1).Any()) return res;

            // Process the first 2 items by adding them then add them to the res list
            res.Add(list.First() + list.Skip(1).First());

            // Recursive definition: Add the rest of the list
            res.AddRange(Next2(list.Skip(1)));
            return res;
        }

        private static IEnumerable<int> Next(IEnumerable<int> list)
        {
            if (list.Count() > 1)
            {
                yield return list.First() + list.Skip(1).First();
                var res = Next(list.Skip(1));
                foreach (var elem in res)
                    yield return elem;
            }
        }
    }

    class Fibonacci
    {
        public static BigInteger Fib(int n)
        {
            if (n < 0) throw new ArgumentException($"intput ({n}) may not be negative", nameof(n));
            //return FibHelper(n);
            return FibHelper2(0, 1, n);
        }

        /// <summary>
        /// Execution order is O(n^2)
        /// </summary>
        private static BigInteger FibHelper(int n)
        {
            // terminating condition
            if (n == 0) return 0;
            if (n == 1) return 1;

            // recursive definition
            return FibHelper(n - 1) + FibHelper(n - 2);
        }

        private static BigInteger FibHelper2(int current, int next, int n)
        {
            if (n == 0) return current;
            return FibHelper2(next, current + next, n - 1);
        }
    }

    class TowerOfHanoi
    {
        public enum Peg { A, B, C }

        /*
        *	Tower of Hanoi.
        *	We have 3 pegs, A, B, C
        *	We also have N disks on peg A stacked in order largest at the bottom smallest at the top.
        *  No 2 disks are of the same size.
        *  We need to move the disks from peg A to peg C (using B) such that we:
        *		A.	move 1 disk at a time and 
        *		B.	No bigger disk sits on top of a smaller disk.
        *
        * See: https://www.mathsisfun.com/games/towerofhanoi.html
        */
        /// <summary>
        /// Originally we need to move the Tower from A (p1) to C (p3)
        /// </summary>
        public static void MoveTower(int diskCount, Peg p1, Peg p2, Peg p3)
        {
            if (diskCount <= 0) throw new ArgumentException($"diskCount value ({diskCount}) is invalid.  It must be a positive number", nameof(diskCount));
            MoveTowerHelper(diskCount, p1, p2, p3);
        }
	
        /// <summary>
        /// We need to Move n - 1 disks from A (p1) to B (p2) using p3
        /// Then move a single disk from A (p1) to C (p3)
        /// Then move the tower from B (p2) to C (p3) using p1
        /// In this way no smaller disk will lay on a bigger disk
        /// </summary>
        private static void MoveTowerHelper(int n, Peg p1, Peg p2, Peg p3)
        {
            // Terminating condition
            if (n == 1)
            {
                MoveDisk(p1, p3);
                return;
            }

            // Recursive definition
            MoveTowerHelper(n - 1, p1, p3, p2);
            MoveDisk(p1, p3);
            MoveTowerHelper(n - 1, p2, p1, p3);
        }
	
        public static void MoveDisk(Peg fr, Peg to)
        {
            Console.WriteLine($"{fr} -> {to}");
        }
    }
}
