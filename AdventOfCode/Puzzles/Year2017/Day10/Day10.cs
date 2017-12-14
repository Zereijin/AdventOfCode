using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day10 {
	class Day10 : Puzzle {
		private int[] knotHash;
		private int currentIndex;
		private int skipSize;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			//testCases.Add( new TestCase( "3,4,1,5", "12", 1 ) );
		}

		private List<int> ParseInput( string input ) {
			string[] inputArray = input.Split( ',' );
			List<int> lengths = new List<int>();

			foreach( string entry in inputArray ) {
				lengths.Add( Int32.Parse( entry ) );
			}

			return lengths;
		}

		public override string Solve( string input, int part ) {
			knotHash = CreateHash( 256 );

			TieKnots( ParseInput( input ) );
			return "" + knotHash[ 0 ] * knotHash[ 1 ];

			return String.Format( "Day 10 part {0} solver not found.", part );
		}

		private int[] CreateHash( int size ) {
			int[] hash = new int[ size ];

			for( int i = 0; i < size; i++ ) {
				hash[ i ] = i;
			}

			return hash;
		}

		private void TieKnots( List<int> lengths ) {
			foreach( int length in lengths ) {
				Reverse( length );

				currentIndex = WrapIndexToKnotHash( currentIndex + length + skipSize );

				skipSize++;
			}
		}

		private void Reverse( int length ) {
			int[] subArray = new int[ length ];
			int nextIndex = 0;

			// Fill subArray
			for( int i = 0; i + currentIndex < currentIndex + length; i++ ) {
				nextIndex = WrapIndexToKnotHash( i + currentIndex );
				subArray[ i ] = knotHash[ nextIndex ];
			}

			// Reverse subArray
			int[] reversedSubArray = new int[ subArray.Length ];
			for( int i = 0; i < subArray.Length; i++ ) {
				reversedSubArray[ i ] = subArray[ subArray.Length - 1 - i ];
			}

			// Replace knotHash with reversed subArray
			for( int i = 0; i + currentIndex < currentIndex + length; i++ ) {
				nextIndex = WrapIndexToKnotHash( i + currentIndex );
				knotHash[ nextIndex ] = reversedSubArray[ i ];
			}
		}

		private int WrapIndexToKnotHash( int index ) {
			while( index >= knotHash.Length ) {
				index -= knotHash.Length;
			}

			return index;
		}
	}
}
