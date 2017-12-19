using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles.Year2017 {
	class KnotHash {
		private int[] knotHash;
		private int currentIndex;
		private int skipSize;		

		public KnotHash( int size ) {
			knotHash = new int[ size ];

			for( int i = 0; i < size; i++ ) {
				knotHash[ i ] = i;
			}

			currentIndex = 0;
			skipSize = 0;
		}

		public void Initialize( List<int> lengths ) {
			lengths.Add( 17 );
			lengths.Add( 31 );
			lengths.Add( 73 );
			lengths.Add( 47 );
			lengths.Add( 23 );
			
			TieKnots( lengths );
		}

		public void TieKnots( List<int> lengths ) {
			for( int i = 0; i < 64; i++ ) {
				TieKnotRound( lengths );
			}
		}

		public void TieKnotRound( List<int> lengths ) {
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

		public int[] GetDenseHash() {
			int[] denseArray = new int[ 16 ];

			int denseIndex = 0;
			for( int i = 0; i < 16; i++ ) {
				if( denseIndex * 16 >= knotHash.Length ) break;

				int denseBit = knotHash[ denseIndex * 16 ];
				for( int j = 1; j < 16; j++ ) {
					denseBit = denseBit ^ knotHash[ denseIndex * 16 + j ];
				}

				denseArray[ denseIndex ] = denseBit;
				denseIndex++;
			}

			return denseArray;
		}

		public string ToHexString() {
			string result = "";
			int[] denseHash = GetDenseHash();

			foreach( int item in denseHash ) {
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

		public string ToBinString() {
			string result = "";
			int[] denseHash = GetDenseHash();

			foreach( int item in denseHash ) {
				int itemRemainder = item;
				string subResult = "";

				while( itemRemainder > 0 ) {
					subResult = ( itemRemainder % 2 ) + subResult;
					itemRemainder /= 2;
				}

				result += subResult;
			}

			return result;
		}
	}
}
