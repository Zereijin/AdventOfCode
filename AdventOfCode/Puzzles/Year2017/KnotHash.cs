using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017 {
	class KnotHash {
		public int[] hash;
		private int currentIndex;
		private int skipSize;

		/// <summary>
		/// Create a new knot hash of the given size.
		/// </summary>
		/// <remarks>
		/// The new knot hash will be a sorted array of incrementing integers.  Run InitializeWeak() or InitializeStrong()
		/// in order to experience the hash magic.
		/// </remarks>
		/// <param name="size">The size of the hash.</param>
		public KnotHash( int size ) {
			hash = new int[ size ];
			currentIndex = 0;
			skipSize = 0;

			for( int i = 0; i < size; i++ ) {
				hash[ i ] = i;
			}
		}

		/// <summary>
		/// Initialize the hash with a list of integers using a single round of knot-tying.
		/// </summary>
		/// <param name="key">A CSV list of integers.</param>
		public void InitializeWeak( string key ) {
			string[] keyValues = key.Split( ',' );
			List<int> lengths = new List<int>();

			foreach( string value in keyValues ) {
				lengths.Add( Int32.Parse( value ) );
			}

			TieKnots( lengths );
		}

		/// <summary>
		/// Initialize the hash with byte string using encryption and 64 rounds of knot-tying.
		/// </summary>
		/// <remarks>
		/// "Encryption" here being the addition of the bytes 17, 31, 73, 47, and 23 to the end.
		/// </remarks>
		/// <param name="key">A byte string.</param>
		public void InitializeStrong( string key ) {
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
			
			for( int i = 0; i < 64; i++ ) {
				TieKnots( asciiList );
			}
		}

		/// <summary>
		/// Perform a round of knot-tying.
		/// </summary>
		/// <param name="lengths">A list of lengths for the knot-tying algorithm to process.</param>
		private void TieKnots( List<int> lengths ) {
			foreach( int length in lengths ) {
				Reverse( length );

				currentIndex = WrapIndexToKnotHash( currentIndex + length + skipSize );

				skipSize++;
			}
		}

		/// <summary>
		/// Helper function to TieKnots().  Performs a reversal within the hash array.
		/// </summary>
		/// <param name="length">The number of consecutive positions to reverse, starting at the currentIndex.</param>
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

		/// <summary>
		/// Get the true (circular) array index of an index that may exceed the length of the hash.
		/// </summary>
		/// <param name="index">The target index before wrapping.</param>
		/// <returns>The true index after wrapping.</returns>
		private int WrapIndexToKnotHash( int index ) {
			while( index >= hash.Length ) {
				index -= hash.Length;
			}

			return index;
		}

		/// <summary>
		/// Produce a dense hash of the knot hash.
		/// </summary>
		/// <returns>The dense hash.</returns>
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

		/// <summary>
		/// Get a hexadecimal string representation of the knot hash.
		/// </summary>
		/// <returns>The knot hash as a hexadeximal string.</returns>
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
		
		/// <summary>
		/// Get a binary string representation of the knot hash.
		/// </summary>
		/// <returns>The knot hash as a binary string.</returns>
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
