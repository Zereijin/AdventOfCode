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

		/// <summary>
		/// Break the string input into a list of single-digit integers.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input converted into a list of single-digit integers.</returns>
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
			
			int sumOfNumbers = 0;
			switch( part ) {
				case 1:
					sumOfNumbers = GetSequentialMatchSum( numbers );
					break;
				case 2:
					sumOfNumbers = GetHalfAroundMatchSum( numbers );
					break;
				default:
					return String.Format( "Day 01 part {0} solver not found.", part );
			}

			return "" + sumOfNumbers;
		}

		/// <summary>
		/// Get the sum of a list of numbers, only counting numbers that are immediately followed by themselves.
		/// </summary>
		/// <param name="numbers">The list of numbers to sum.</param>
		/// <returns>The sequential match sum.</returns>
		private int GetSequentialMatchSum( List<int> numbers ) {
			int sum = 0;

			for( int i = 0; i < ( numbers.Count - 1 ); i++ ) {
				sum += GetSumContribution( numbers[ i ], numbers[ i + 1 ] );
			}

			sum += GetSumContribution( numbers[ numbers.Count - 1 ], numbers[ 0 ] );

			return sum;
		}

		/// <summary>
		/// Get the sum of a list of numbers, only counting numbers that are equal to the number on the opposite side of the wrapped array.
		/// </summary>
		/// <param name="numbers">The list of numbers to sum.</param>
		/// <returns>The half-around match sum.</returns>
		private int GetHalfAroundMatchSum( List<int> numbers ) {
			int sum = 0;
			int halfSize = numbers.Count / 2;

			// i + half + half == i, therefore we only need to check half the list, then double it.
			for( int i = 0; i < halfSize; i++ ) {
				sum += GetSumContribution( numbers[ i ], numbers[ i + halfSize ] );
			}

			return sum * 2;
		}

		/// <summary>
		/// Determines how much two numbers contribute to a sum.
		/// </summary>
		/// <remarks>
		/// If a and b are equal, then we contribute the full amount; otherwise, we contribute nothing.
		/// </remarks>
		/// <param name="a">The first number.</param>
		/// <param name="b">The second number.</param>
		/// <returns>The sum contribution that the two numbers permit</returns>
		private int GetSumContribution( int a, int b ) {
			if( a == b ) {
				return a;
			}

			return 0;
		}
	}
}
