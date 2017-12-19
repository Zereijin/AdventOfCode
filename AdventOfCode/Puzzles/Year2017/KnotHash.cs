using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles.Year2017 {
	class KnotHash {
		private int[] hash;
		private int currentIndex;
		private int skipSize;

		public KnotHash( string key, int size ) {
			hash = new int[ size ];
			currentIndex = 0;
			skipSize = 0;

			for( int i = 0; i < size; i++ ) {
				hash[ i ] = i;
			}

			List<int> lengths = ParseKey( key );
			for( int i = 0; i < 64; i++ ) {
				TieKnots( lengths );
			}
		}

		private List<int> ParseKey( string key ) {
			char[] keyChars = key.ToCharArray();
			List<int> asciiList = new List<int>();

			foreach( char c in keyChars ) {
				asciiList.Add( (int)c );
			}
			
			asciiList.Add( 17 );
			asciiList.Add( 31 );
			asciiList.Add( 73 );
			asciiList.Add( 47 );
			asciiList.Add( 23 );

			return asciiList;
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
				subArray[ i ] = hash[ nextIndex ];
			}

			// Reverse subArray
			int[] reversedSubArray = new int[ subArray.Length ];
			for( int i = 0; i < subArray.Length; i++ ) {
				reversedSubArray[ i ] = subArray[ subArray.Length - 1 - i ];
			}

			// Replace knotHash with reversed subArray
			for( int i = 0; i + currentIndex < currentIndex + length; i++ ) {
				nextIndex = WrapIndexToKnotHash( i + currentIndex );
				hash[ nextIndex ] = reversedSubArray[ i ];
			}
		}

		private int WrapIndexToKnotHash( int index ) {
			while( index >= hash.Length ) {
				index -= hash.Length;
			}

			return index;
		}

		private int[] GetDenseHash() {
			int[] denseHash = new int[ 16 ];
			int denseSize = hash.Length / denseHash.Length;

			for( int i = 0; i < denseHash.Length; i++ ) {
				int denseBit = hash[ i * denseSize ];
				for( int j = 1; j < denseSize; j++ ) {
					denseBit = denseBit ^ hash[ i * denseSize + j ];
				}

				denseHash[ i ] = denseBit;
			}

			return denseHash;
		}

		public string ToHexString() {
			int[] denseHash = GetDenseHash();
			string result = "";

			foreach( int num in denseHash ) {
				int tens = num / 16;
				int ones = num % 16;

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
			char[] hexChars = ToHexString().ToCharArray();
			string result = "";

			foreach( char digit in hexChars ) {
				result += GetBin( digit );
			}

			return result;
		}

		private string GetBin( char hexDigit ) {
			switch( hexDigit ) {
				case '1': return "0001";
				case '2': return "0010";
				case '3': return "0011";
				case '4': return "0100";
				case '5': return "0101";
				case '6': return "0110";
				case '7': return "0111";
				case '8': return "1000";
				case '9': return "1001";
				case 'a': return "1010";
				case 'b': return "1011";
				case 'c': return "1100";
				case 'd': return "1101";
				case 'e': return "1110";
				case 'f': return "1111";
			}

			return "0000";
		}
	}
}
