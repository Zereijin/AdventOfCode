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

		private List<int> ParseAsciiInput( string input ) {
			char[] inputArray = input.ToCharArray();
			List<int> lengths = new List<int>();

			foreach( char entry in inputArray ) {
				lengths.Add( (int)entry );
			}

			lengths.Add( 17 );
			lengths.Add( 31 );
			lengths.Add( 73 );
			lengths.Add( 47 );
			lengths.Add( 23 );

			return lengths;
		}

		public override string Solve( string input, int part ) {
			knotHash = CreateHash( 256 );
			currentIndex = 0;

			switch( part ) {
				case 1:
					TieKnots( ParseInput( input ) );
					return "" + knotHash[ 0 ] * knotHash[ 1 ];
				case 2:
					List<int> lengths = ParseAsciiInput( input );
					
					for( int i = 0; i < 64; i++ ) {
						TieKnots( lengths );
					}

					return GetDenseHash();
			}

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

		private string GetDenseHash() {
			int[] denseArray = new int[ 16 ];

			int denseIndex = 0;
			for( int i = 0; i < 16; i++ ) {
				int denseBit = knotHash[ denseIndex * 16 ];
				for( int j = 1; j < 16; j++ ) {
					denseBit = denseBit ^ knotHash[ denseIndex * 16 + j ];
				}

				denseArray[ denseIndex ] = denseBit;
				denseIndex++;
			}

			//DEBUG
			for( int j = 0; j < 16; j++ ) {
			Console.WriteLine( denseArray[ j ] == (knotHash[ j * 16 ] ^ knotHash[ j*16+1 ] ^ knotHash[ j*16+2 ] ^ knotHash[ j*16+3 ] ^ knotHash[ j*16+4 ] ^ knotHash[ j*16+5 ] ^ knotHash[j*16+ 6 ] ^ knotHash[j*16+ 7 ] ^ knotHash[j*16+ 8 ] ^ knotHash[j*16+ 9 ] ^ knotHash[j*16+ 10 ] ^ knotHash[ j*16+11 ] ^ knotHash[ j*16+12 ] ^ knotHash[ j*16+13 ] ^ knotHash[ j*16+14 ] ^ knotHash[ j*16+15 ]));
			}

			return ToHexString( denseArray );
		}

		private string ToHexString( int[] array ) {
			string result = "";

			foreach( int item in array ) {
				int tens = item / 16;
				int ones = item % 16;

				result += "" + GetHex( tens ) + "" + GetHex( ones );
			}

			return result;
		}

		private string GetHex( int number ) {
			switch( number ) {
				case 15: return "f";
				case 14: return "e";
				case 13: return "d";
				case 12: return "c";
				case 11: return "b";
				case 10: return "a";
				default: return "" + number;
			}
		}
	}
}
