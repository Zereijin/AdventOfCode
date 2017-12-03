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

		private List<List<int>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<List<int>> numberLists = new List<List<int>>();
			
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
			List<List<int>> listOfNumberLists = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetChecksum( listOfNumberLists );
					break;
				case 2:
					return "" + GetSumOfDivisibles( listOfNumberLists );
					break;
				default:
					return String.Format( "Day 02 part {0} solver not found.", part );
			}
		}

		private int GetChecksum( List<List<int>> listOfNumberLists ) {
			int checksum = 0;

			foreach( List<int> numberList in listOfNumberLists ) {
				checksum += GetDifferenceOfMaxAndMin( numberList );
			}

			return checksum;
		}

		private int GetSumOfDivisibles( List<List<int>> listOfNumberLists ) {
			int sum = 0;

			foreach( List<int> numberList in listOfNumberLists ) {
				sum += GetDivisionOfDivisibles( numberList );
			}

			return sum;
		}

		private int GetDivisionOfDivisibles( List<int> numberList ) {
			for( int i = 0; i < numberList.Count; i++ ) {
				for( int j = 0; j < numberList.Count; j++ ) {
					if( i == j ) continue;

					if( numberList[ i ] % numberList[ j ] == 0 ) {
						return numberList[ i ] / numberList[ j ];
					}
				}
			}

			return 0;
		}

		private int GetDifferenceOfMaxAndMin( List<int> numberList ) {
			return Max( numberList ) - Min( numberList );
		}

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
