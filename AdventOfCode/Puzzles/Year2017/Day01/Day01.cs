using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day01 {
	class Day01 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();
			
			testCases.Add( new TestCase( "1122", "3", 1 ) );
			testCases.Add( new TestCase( "1111", "4", 1 ) );
			testCases.Add( new TestCase( "1234", "0", 1 ) );
			testCases.Add( new TestCase( "91212129", "9", 1 ) );
			testCases.Add( new TestCase( "1212", "6", 2 ) );
			testCases.Add( new TestCase( "1221", "0", 2 ) );
			testCases.Add( new TestCase( "123425", "4", 2 ) );
			testCases.Add( new TestCase( "123123", "12", 2 ) );
			testCases.Add( new TestCase( "12131415", "4", 2 ) );
		}

		private List<int> ParseInput( string input ) {
			char[] inputArray = input.ToCharArray();
			List<int> numbers = new List<int>();

			foreach( char entry in inputArray ) {
				numbers.Add( Int32.Parse( "" + entry ) );
			}

			return numbers;
		}

		public override string Solve( string input, int part ) {
			List<int> numbers = ParseInput( input );
			
			int sumOfSequentialNumbers = 0;
			switch( part ) {
				case 1:
					sumOfSequentialNumbers = GetSequentialSum( numbers );
					break;
				case 2:
					sumOfSequentialNumbers = GetHalfAroundSum( numbers );
					break;
				default:
					return String.Format( "Day 01 part {0} solver not found.", part );

			}

			return "" + sumOfSequentialNumbers;

		}

		private int GetSequentialSum( List<int> numbers ) {
			int sum = 0;

			for( int i = 0; i < ( numbers.Count - 1 ); i++ ) {
				sum += GetSumContribution( numbers[ i ], numbers[ i + 1 ] );
			}

			sum += GetSumContribution( numbers[ numbers.Count - 1 ], numbers[ 0 ] );

			return sum;
		}

		private int GetHalfAroundSum( List<int> numbers ) {
			int sum = 0;
			int halfSize = numbers.Count / 2;

			for( int i = 0; i < halfSize; i++ ) {
				sum += GetSumContribution( numbers[ i ], numbers[ i + halfSize ] );
			}

			//for( int i = halfSize; i < numbers.Count; i++ ) {
			//	sum += GetSumContribution( )
			//}

			//numbers[ i ] == numbers[ i + halfway]

			return sum * 2;
		}

		private int GetSumContribution( int a, int b ) {
			if( a == b ) {
				return a;
			}

			return 0;
		}
	}
}
