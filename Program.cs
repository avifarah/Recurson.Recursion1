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
            *		n (n - 1) . . 2 1 1 2 . . (n - 1) n
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
                Console.WriteLine($"Error while processing Pascal's row 2.  Internal message: {ex.Message}");
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

                // We will write the check of Math.Pow(baseValue, n) only if there is a sufficient difference between
                // the C# Math.Pow library and our recursive Power result
                Console.WriteLine($"PowerOf.Power({baseValue}, {n,2}): {p}{(check > 1e-15 ? $"  Expected: {trust}  Actual: {p}" : string.Empty)}");
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
            *
            *  Assignment:
            *       Write a recursive program that return the n'th row of Pascal's triangle
            */
            Console.WriteLine();
            Console.WriteLine($"Pascal's triangle.");
            var row = 0;
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
            // Sanity check that need not be done at the beginning of every recursive call
            if (n < 1)
                throw new ArgumentException($"intput given {n} is not allowed.  It is expected to be positive", nameof(n));

    		//return PalindromeHelper(n).ToList();

            // In case we do not have access to IEnumerable's "yield return" construct.
            // See explanation for "yield return" in the PalindromeHelper(..) summary.
            //return PalindromeHelper2(n);

            // Optimization over PalindromeHelper3
            var pal = new List<int>();
            PalindromeHelper3(n, pal);
            return pal;

            // Another option is:
            // return PalindromeHelper3(n, new List<int>());
        }
	
        /// <summary>
        /// Assumption input to PalindromeHelper cannot be smaller than 1
        ///
        /// Recursive definition:
        ///     List { n } + Palindrome(n - 1) + List { n }
        ///
        /// In order to understand this routine we need to understand the C# yield keyword:
        /// When the method return IEnumerable<type T>
        ///         private static IEnumerable<int> PalindromeHelper(int n)
        ///                        ^^^^^^^^^^^^^^^^
        /// a yield return tells the compiler to return the current value but return to that very spot
        /// for the nect method call.
        /// Example:
        ///     Enumerable.Range(1, 5) will return an IEnumerable<int> { 1, 2, 3, 4, 5 }
        ///     The difference between IEnumerable<int> and List<int> is that:
        ///         * a list is an ordered collection of integers, allowing random access of the elements via an index, removal etc
        ///         * an IEnumerable<int> allows only access to current element and move next.
        /// Continue with example:
        ///             IEnumerable<int> fun() {
        ///                 foreach (var i in Enumerable.Range(1, 5))
        ///                     yield return i;
        ///             }
        ///             IEnumerable<int> sq() {
        ///                 foreach (var n in fun())
        ///                     Console.WriteLine(n * n);
        ///             }
        ///
        /// Since not every language has an equivalent of the "yield return" construct, I provided a second Helper:
        ///     PalindromeHelper2
        /// </summary>
        private static IEnumerable<int> PalindromeHelper(int n)
        {
            // n == 0 is the terminating condition
            if (n > 0)
            {
                yield return n;
			
                // Recursive definition: PalindromeHelper(n) = n PalindromeHelper(n - 1) n
                var innerPalindrome = PalindromeHelper(n - 1);
                foreach (var m in innerPalindrome)
                    yield return m;

                yield return n;
            }
        }

        /// <summary>
        /// Assumption input to PalindromeHelper2 cannot be smaller than 1
        ///
        /// Syntax:
        ///     The question mark (?) suffix after List<int>? in the method declaration:
        ///         private static List<int> PalindromeHelper2(int n, List<int>? palindrome = null)
        ///                                                           ^^^^^^^^^^
        ///     Indicates that the varable palindrome may be null.  Without the ? suffix if
        ///     the complier's static time analysis would flag palindrome as a WARNING if the
        ///     compiler "thinks" that palindrome may get a null value
        /// </summary>
        private static List<int> PalindromeHelper2(int n, List<int>? palindrome = null)
        {
            if (n == 0)
                return new List<int> {};

            // Recursive definition
            var list = new List<int> { n };
            list.AddRange(PalindromeHelper2(n - 1, palindrome));
            list.Add(n);

            return list;
        }

        /// <summary>
        /// An optimization over PalindromeHellper3 is having to initialize a single palindrome list
        /// in this routine.
        ///
        /// This PalindromeHelper3(..) algorithm works because we start with palindrom = empty list
        ///
        /// When n <= 1 the calling routine eliminates this option with an exception
        /// when n = 1 then we add 1 to an empty list then call PalindromeHelper3(0, emptylist) then add 1.  Works as advertised.
        /// when n = 2 then we add 2 to an empty list, then call PalindromeHelper3(1, List { 2 }) then add 2.  Works as advertised.
        /// when n = 3 then we add 3 to an empty list, then call PalindromeHelper3(1, List { 3 }) then add 3.  Works as advertised.
        /// etc...
        ///
        /// The return palindrom is superfluous because the calling routine can access the parameter palindrome!
        /// </summary>
        private static List<int> PalindromeHelper3(int n, List<int> palindrome)
        {
            if (n == 0)
                return palindrome;

            palindrome.Add(n);
            _ = PalindromeHelper3(n - 1, palindrome);
            palindrome.Add(n);

            // This return is superfluous
            return palindrome;
        }
    }

    /// <summary>
    /// Terminating condition expValue == 0:	baseValue^0 = 1
    ///
    /// Recursive definition:
    /// if expValue > 0
    /// 		Recursive definition:	baseValue^n = baseValue * baseValue^(expValue - 1)
    /// if expValue < 0
    /// 		Recursive definition:	baseValue^n = (1 / baseValue) * baseValue^(expValue + 1)
    ///
    /// Note, we make 2 checks at the beginning of each recursive call. One check can be eliminated
    /// If expValue > 0 then it is always > 0 until terminating condition where expValue == 0
    /// and visa versa if expValue < 0 then it is always < 0 until the terminating condition.
    ///
    /// Therefore, we can make an optimization and make the positive / negative check once
    /// </summary.
    class PowerOf
    {
        public static double Power(int baseValue, int expValue)
        {
            if (expValue == 0)
                return 1.0;

            if (expValue > 0)
                return PostiveExpPow(baseValue, expValue);

            return NegativeExpPow(baseValue, expValue);
        }

        private static double PostiveExpPow(int baseValue, int expValue)
        {
            if (expValue == 0)
                return 1.0;

            // Tail recursion
            return baseValue * PostiveExpPow(baseValue, expValue - 1);
        }

        private static double NegativeExpPow(int baseValue, int expValue)
        {
            if (expValue == 0)
                return 1.0;

            // Tail recursion
            return (1.0 / baseValue) * NegativeExpPow(baseValue, expValue + 1);

        }
    }

    class PascalsTriangle
    {
        public static IEnumerable<int> PascalsRow(int n)
        {
            // A one time check that need not be repeated before every recursive call
            if (n < 0)
                throw new ArgumentException($"input value of {n} is illegal.  Expected value of 0 or more", nameof(n));

            // The recursive call starts with row[0] that has a value of list<int> { 1 }
            var row0 = new List<int> { 1 };
            var res = PascalRowHelper(row0, n);
            return res;
        }

        /// <summary>
        ///Given row[0]
        ///     row[1] = Next(row[0])
        ///     row[2] = Next(Next[0]))
        ///     etc...
        ///
        /// So recursively Call Next(..) with a count n
        /// When count reaches 0 then we are done!
        /// </summary>
        private static IEnumerable<int> PascalRowHelper(IEnumerable<int> row, int n)
        {
            // Terminating condition: if current count reached the limit that we are looking for then we are done
            if (n == 0)
                return row;

            // Next row: Process the current row to reach next row
            var next = Next(row);

            // Generate the next row recursively
            // This is tail recursion equivalent to an iteration
            // ..   second parameter, n, decreases to 0
            return PascalRowHelper(next, n - 1);
        }

        /// <summary>
        /// Given row[n]:
        ///     row[n+1] = list { 1 }
        ///         + (n - 1 item list)(row[n] (k in (0 .. n-1) | row[n][k] + row[n][k+1]))
        ///         + list { 1 }
        ///     row[n+1] = Next(row[n])
        /// </summary>
        private static IEnumerable<int> Next(IEnumerable<int> row)
        {
            // return new List<int> { 1 }
            //     .Concat(Next1Helper(row))
            //     .Concat(new List<int> { 1 });
            return new List<int> { 1 }
                .Concat(Next2Helper(row))
                .Concat(new List<int> { 1 });
        }

        /// <summary>
        /// For a "yield return" discussion see summary for method: PalindromeHelper(..)
        /// list.Skip(n) generates a new IEnumerable collection starting with the n'th
        /// indexed element as opposed to the 0'th element.
        ///
        /// For discussion of IEnumerable see: the summary of PalindromeHelper(..)
        /// </summary>
        private static IEnumerable<int> Next1Helper(IEnumerable<int> list)
        {
            //
            // list.Count() will run through the entire collection therefore the check for
            //         if (list.Count() > 1)        -- O(n)
            // can be optimized to:
            //         if (list.Skip(1).Any())      -- O(2)
            if (list.Skip(1).Any())
            {
                yield return list.First() + list.Skip(1).First();
                var res = Next1Helper(list.Skip(1));
                foreach (var elem in res)
                    yield return elem;
            }
        }

        /// <summary>
        /// Similar to Next1(..) except that this routine does not use the "yield return" construct
        /// Returns the n - 1 items (list[k] + list[k+1]) items where k runs from 0 .. n - 1
        /// This row is missing the leading and trailing 1's
        /// </summary>
        private static List<int> Next2Helper(IEnumerable<int> list)
        {
            // Terminating condition is list contains 1 element only
            var res = new List<int>();

            // Syntax:
            //      ! prefix indicates NOT logical operator
            //      list.Skip(1) will return the original list skipping the 0'th element
            //      .Any() will return true if the list contains at least 1 element
            if (!list.Skip(1).Any()) return res;

            // Process the first 2 items by adding them then add them to the res list
            res.Add(list.First() + list.Skip(1).First());

            // Recursive definition: Add the rest of the list
            res.AddRange(Next2Helper(list.Skip(1)));
            return res;
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

            // Recursive definition
            // Note the double recursive all -- making it an expensive call
            return FibHelper(n - 1) + FibHelper(n - 2);
        }

        /// <summary>
        /// This is a more efficient algorithm for the Fibonacci Helper
        /// </summary>
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
            if (diskCount <= 0)
                throw new ArgumentException($"diskCount value ({diskCount}) is invalid.  It must be a positive number", nameof(diskCount));

            MoveTowerHelper(diskCount, p1, p2, p3);
        }
	
        /// <summary>
        /// Recursive definition:
        ///         We need to Move n - 1 disks from A (p1) to B (p2) using p3
        ///         Move a single disk from A (p1) to C (p3)
        ///         Move the tower from B (p2) to C (p3) using p1
        ///
        /// In this way no larger disk will lay on a smaller disk
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
