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
            int n = 5;
            try
            {
                var pal = Palindrome.GetPalindrome(n);
                Console.WriteLine($"Palindrome.GetPalindrome({n})");
                Console.WriteLine(string.Join(" ", pal));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing Palindrome({n}).  Internal message: {ex.Message}");
            }

            /*
            *  Power:
            *  x ^ y find result in a recursive manner
            */
            Console.WriteLine();
            Console.WriteLine("Power:");
            var baseValue = 5;
            for (var exp = -5; exp <= 5; ++exp)
            {
                var p = PowerOf.Power(baseValue, exp);
                var trust = Math.Pow(baseValue, exp);
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

            /*
            * We have n disks stacked in size order where the smallest is at the top.
            * We have 3 pegs A, B and C
            * The disks are stacked on Peg A
            *
            * Our task is to move the disks from Peg A to Peg C by using Peg B, such that:
            * 		1	We move one disk at a time
            *		2	No larger disk is ever placed on top of a smaller disk
            *
            * If diskCount == 1
            *		MoveDisk:	A -> C		(no sweat)
            * if diskCount == 2
            *		MoveDisk:	A -> B		(Peg B is empty and is the place holder peg.  The disk moved is the smaller disk)
            *		MoveDisk:	A -> C		(Move the larger disk to the destination Peg, C)
            *		MoveDisk:	B -> C		(Move the smaller disk from B on top of the larger disk on C)
            * etc...
            */
            var diskCount = 3;
            try
            {
                Console.WriteLine();
                Console.WriteLine($"Tower of Hanoi with {diskCount} disks:");
                var fromPeg = TowerOfHanoi.Peg.A;
                var toPeg = TowerOfHanoi.Peg.C;
                var usingPeg = TowerOfHanoi.Peg.B;
                TowerOfHanoi.MoveTower(diskCount, fromPeg, usingPeg, toPeg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing TowerOfHanoi({diskCount}).  Internal message: {ex.Message}");
            }

            /*
             * A bonus question:
             *
             * *Question *: 
             * 		Given a board of characters and a target string, 
             *      determine if a given word exists in the board or not
             *      using each letter only once.
             *
             * char[][] board, String word
             *
             * *Input:*
             * 		board =
             * 			[['A', 'B', 'C', 'E'],
             *  		 ['C', 'E', 'S', 'D'],
             *  		 ['B', 'C', 'C', 'F']]
             *
             * Examples:
             *      word: ABCED → true
             * word: ABCES → False
             */
            Console.WriteLine();
            Console.WriteLine("Find word in letter board");
       		var word = "BCESC";
            var find = new FindWordInBoard(word);
            var success = find.Find();
            Console.WriteLine($"{word,10}: Found:  Expected:  True.  Actual: {success,5}");

            word = "BCC";
            find = new FindWordInBoard(word);
            success = find.Find();
            Console.WriteLine($"{word,10}: Found:  Expected:  True.  Actual: {success,5}");

            word = "BCCC";
            find = new FindWordInBoard(word);
            success = find.Find();
            Console.WriteLine($"{word,10}: Found:  Expected: False.  Actual: {success,5}");

            word = "BCECCS";
            find = new FindWordInBoard(word);
            success = find.Find();
            Console.WriteLine($"{word,10}: Found:  Expected:  True.  Actual: {success,5}");

            word = "BCECCD";
            find = new FindWordInBoard(word);
            success = find.Find();
            Console.WriteLine($"{word,10}: Found:  Expected: False.  Actual: {success,5}");
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

            // return new List<int> { 1 }
            //     .Concat(Next2Helper(row))
            //     .Concat(new List<int> { 1 });
            
            var res = new List<int>();
            Next2OptHelper(row, res);
            return new List<int> { 1 }
                .Concat(res)
                .Concat(new List<int> { 1 });
        }

        /// <summary>
        /// For a "yield return" discussion see summary for method: PalindromeHelper(..)
        /// list.Skip(n) generates a new IEnumerable collection starting with the n'th
        /// indexed element as opposed to the 0'th element.
        ///
        /// For discussion of IEnumerable see: the summary of PalindromeHelper(..)
        /// </summary>
        private static IEnumerable<int> Next1Helper(IEnumerable<int> rowRemaining)
        {
            //
            // list.Count() will run through the entire collection therefore the check for
            //         if (list.Count() > 1)        -- O(n)
            // can be optimized to:
            //         if (list.Skip(1).Any())      -- O(2)
            //
            // Terminating condition:
            if (rowRemaining.Skip(1).Any())
            {
                yield return rowRemaining.First() + rowRemaining.Skip(1).First();
                var res = Next1Helper(rowRemaining.Skip(1));

                // This is an artifact of the "yield return" construct.  We cannot
                // directly return res 
                //          var res = Next1Helper(list.Skip(1));
                //              ^^^
                // despite the fact that res is of type IEnumerable<int>, since res
                // references the IEnumerable of the called helper method (1 down the 
                // chain of recursion)
                //
                // In order to return res, we need to "yield return" element by element.
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
            var res = new List<int>();

            // Terminating condition is list contains 1 element only
            // Syntax:
            //      ! prefix indicates NOT logical operator
            //      list.Skip(1) will return the original list skipping the 0'th element
            //      .Any() will return true if the list contains at least 1 element
            if (! list.Skip(1).Any()) return res;

            // Process the first 2 items by adding them then add them to the res list
            res.Add(list.First() + list.Skip(1).First());

            // Recursive definition: Add the rest of the list
            res.AddRange(Next2Helper(list.Skip(1)));
            return res;
        }

        /// <summary>
        /// An optimization can be done to Next2Helper(..).  Instead of creating the res
        /// list on every call we can pass an empty list and populate it in the recursive
        /// call.
        /// </summary>
        private static List<int> Next2OptHelper(IEnumerable<int> rowRemaining, List<int> res)
        {
            // Terminating condition is list contains 1 element only
            // Syntax:
            //      ! prefix indicates NOT logical operator
            //      list.Skip(1) will return the original list skipping the 0'th element
            //      .Any() will return true if the list contains at least 1 element
            if (! rowRemaining.Skip(1).Any()) return res;

            // Process the first 2 items by adding them then add them to the res list
            res.Add(rowRemaining.First() + rowRemaining.Skip(1).First());

            // Recursive definition: Add the rest of the list
            res.AddRange(Next2Helper(rowRemaining.Skip(1)));
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
class FindWordInBoard
{
	private const int cols = 4;
	private int rows = 3;
	private string LookingFor;

	private static char[][] _board = new char[][] {
			new char[] { 'A', 'B', 'C', 'E' },
			new char[] { 'C', 'E', 'S', 'D' },
			new char[] { 'B', 'C', 'C', 'F' }
		};

	public FindWordInBoard(string word)
	{
		LookingFor = word;
	}

	public bool Find()
	{
		// For each first letter match see if we have a match of the whole word
		// If we do then we are done
		for (var i = 0; i < rows; ++i)
		{
			for (var j = 0; j < cols; ++j)
			{
				if (_board[i][j] == LookingFor[0])
				{
					// First letter match is of index 0
					var b = BoardClone(_board);
					b[i][j] = ' ';

					// Match the rest of the word starting with index == 1
					var rc = Match((i, j), 1, b);
					if (rc) return true;
				}
			}
		}

		// We found no match
		return false;
	}

	/// <summary>
	/// Recursively look for a match
    ///
    /// This is an interesting problem as the solution finds the maximum letters match it can trace
    /// thereafter it will return True for succeeded or False, it did not find all the letters in the word.
    /// If the answer is True--word was matched in its entirety then we are done.
    /// If the answer is False, then we go back to the next NextStep() option and see if we matched the word completely.
	/// </summary>
	/// <param name="pos">(i, j) position of letter in LookingFor[inx - 1]</param>
	/// <param name="inx">index such that the letter: LookingFor[inx] is checked for a match</param>
	/// <param name="board">board to look through</param>
	/// <returns>LookingFor word found or not</returns>
	private bool Match((int i, int j) pos, int inx, char[][] board)
	{
        // Terminating condition
        // We matched the entire word.  Return true and we are done!
		if (inx == LookingFor.Length)
			return true;

        // For our recursive definition (this is the intersting part):
        // For each NextStep(..) we recursively look for the rest of the word.
        // So if we come back with a false meaning the path we took up to now did not
        // match the entire word, then we simply attept the next option of NextStep(..)
        // for the letter in the index: inx.
		foreach ((int i, int j) np in NextStep((pos.i, pos.j)))
		{
			if (board[np.i][np.j] == LookingFor[inx])
			{
				var b = BoardClone(board);
				b[np.i][np.j] = ' ';

                // Recursive call.  Matches the rest of the word.
				var rc = Match(np, inx + 1, b);
				if (rc) return true;
			}
		}

		return false;
	}

#if false
	private IEnumerable<(int, int)> NextStep((int i, int j) position)
	{
		if (position.i - 1 >= 0)   yield return (position.i - 1, position.j);
		if (position.i + 1 < rows) yield return (position.i + 1, position.j);
		if (position.j - 1 >= 0)   yield return (position.i,     position.j - 1);
		if (position.j + 1 < cols) yield return (position.i,     position.j + 1);
	}
#endif

    /// <summary>
    /// Without using yield return...
    /// </summary
	private IEnumerable<(int, int)> NextStep((int i, int j) position)
	{
        var nextPositions = new List<(int, int)>();
		if (position.i - 1 >= 0)   nextPositions.Add((position.i - 1, position.j));
		if (position.i + 1 < rows) nextPositions.Add((position.i + 1, position.j));
		if (position.j - 1 >= 0)   nextPositions.Add((position.i,     position.j - 1));
		if (position.j + 1 < cols) nextPositions.Add((position.i,     position.j + 1));
        return nextPositions;
	}
	
	private char[][] BoardClone(char[][] board)
	{
		var b = new char[rows][];
		for (var r = 0; r < rows; ++r)
		{
			b[r] = new char[cols];
			for (var c = 0; c < cols; ++c)
				b[r][c] = board[r][c];
		}

		return b;
	}
}