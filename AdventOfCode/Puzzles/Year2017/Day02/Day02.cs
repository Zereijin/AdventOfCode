using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day02 {
	class Day02 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "5 1 9 5\n7 5 3\n 2 4 6 8", "18", 1 ) );
			testCases.Add( new TestCase( "5 9 2 8\n9 4 7 3\n 3 8 6 5", "9", 2 ) );
		}

		/// <summary>
		/// Break the input string into a 2-dimensional list of numbers.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input string converted to a 2-dimensional list of numbers.</returns>
		private List<List<int>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<List<int>> numberLists = new List<List<int>>();
			
			// This pattern finds each integer separated by any number of non-digit characters in the string.
			string pattern = @"\d+";
			Regex regex = new Regex( pattern );

			foreach( string entry in inputArray ) {
				MatchCollection matches = regex.Matches( entry );
				List<int> numberList = new List<int>();

				foreach( Match match in matches ) {
					numberList.Add( Int32.Parse( match.ToString() ) );
				}

				numberLists.Add( numberList );
			}

			return numberLists;
		}

		public override string Solve( string input, int part ) {
			List<List<int>> numberLists = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetChecksum( numberLists );
				case 2:
					return "" + GetQuotientSum( numberLists );
			}

			return String.Format( "Day 02 part {0} solver not found.", part );
		}

		/// <summary>
		/// Find the checksum of a 2-dimensional list of integers.
		/// </summary>
		/// <remarks>
		/// The checksum is made by finding the difference between the max and min values of each sublist, and then adding
		/// those differences together.
		/// </remarks>
		/// <param name="numberLists">The 2-dimensional list to get the checksum for.</param>
		/// <returns>The checksum of the 2-dimensional list.</returns>
		private int GetChecksum( List<List<int>> numberLists ) {
			int checksum = 0;

			foreach( List<int> numberList in numberLists ) {
				checksum += GetDifferenceOfMaxAndMin( numberList );
			}

			return checksum;
		}

		/// <summary>
		/// Find the sum of the integer quotients in a 2-dimensional list of integers.
		/// </summary>
		/// <remarks>
		/// Each sublist is expected to have exactly one number that evenly divides into one other number.
		/// With the quotient of these numbers, we find the sum of each list.
		/// </remarks>
		/// <param name="numberLists">The 2-dimensional list to get the quotient sum for.</param>
		/// <returns></returns>
		private int GetQuotientSum( List<List<int>> numberLists ) {
			int sum = 0;

			foreach( List<int> numberList in numberLists ) {
				sum += GetQuotientOfDivisibles( numberList );
			}

			return sum;
		}

		/// <summary>
		/// Find the quotient of the first two numbers that divide evenly into each other in a list.
		/// </summary>
		/// <param name="numberList">The list to find the quotient for.</param>
		/// <returns>The quotient of the even divisors, or 0 if there are none.</returns>
		private int GetQuotientOfDivisibles( List<int> numberList ) {
			for( int i = 0; i < numberList.Count; i++ ) {
				for( int j = 0; j < numberList.Count; j++ ) {
					// We don't want a false positive when matching a number against itself.
					if( i == j ) continue;

					// If the i-th number is evenly divisible by the j-th number, return their quotient.
					if( numberList[ i ] % numberList[ j ] == 0 ) {
						return numberList[ i ] / numberList[ j ];
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Find the difference between the maximum and minimum values in a list.
		/// </summary>
		/// <param name="numberList">The list to get the difference for.</param>
		/// <returns>The difference between the maximum and minimum values.</returns>
		private int GetDifferenceOfMaxAndMin( List<int> numberList ) {
			return Max( numberList ) - Min( numberList );
		}

		/// <summary>
		/// Find the maximum number in a list.
		/// </summary>
		/// <remarks>
		/// For additional efficiency, we could search for Max and Min within the same function.
		/// However, finding them separately doesn't add much inefficiency (both are O(N)), and makes them more
		/// useful in the general case.
		/// </remarks>
		/// <param name="numberList">The list to analyze.</param>
		/// <returns>The maximum number in the list.</returns>
		private int Max( List<int> numberList ) {
			if( numberList.Count <= 0 ) {
				return 0;
			}

			int max = numberList[ 0 ];

			foreach( int number in numberList ) {
				if( number > max ) {
					max = number;
				}
			}

			return max;
		}

		/// <summary>
		/// Find the minimum number in a list.
		/// </summary>
		/// <remarks>
		/// See Max().
		/// </remarks>
		/// <param name="numberList">The list to analyze.</param>
		/// <returns>The minimum number in the list.</returns>
		private int Min( List<int> numberList ) {
			if( numberList.Count <= 0 ) {
				return 0;
			}

			int min = numberList[ 0 ];

			foreach( int number in numberList ) {
				if( number < min ) {
					min = number;
				}
			}

			return min;
		}
	}
}
