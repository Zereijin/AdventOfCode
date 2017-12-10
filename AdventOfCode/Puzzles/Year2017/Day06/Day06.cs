using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day06 {
	class Day06 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "0\t2\t7\t0", "5", 1 ) );
			testCases.Add( new TestCase( "0\t2\t7\t0", "4", 2 ) );
		}

		/// <summary>
		/// Break the input string into an array of memory banks.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input string converted into an array of memory banks.</returns>
		private int[] ParseInput( string input ) {
			string[] inputArray = input.Split( '\t' );
			int[] memoryBanks = new int[ inputArray.Length ];

			for( int i = 0; i < inputArray.Length; i++ ) {
				memoryBanks[ i ] = Int32.Parse( inputArray[ i ] );
			}

			return memoryBanks;
		}

		public override string Solve( string input, int part ) {
			int[] memoryBanks = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetStepsUntilLoop( memoryBanks );
				case 2:
					return "" + GetSizeOfLoop( memoryBanks );
			}

			return String.Format( "Day 06 part {0} solver not found.", part );
		}

		/// <summary>
		/// Find how many memory allocations we can perform before we reach a configuration we've seen before.
		/// </summary>
		/// <remarks>
		/// This debug function does not modify the memory banks in place.
		/// </remarks>
		/// <param name="memoryBanks">The memory bank configuration to reallocate over.</param>
		/// <returns>The number of reallocations we can make before we start looping.</returns>
		private int GetStepsUntilLoop( int[] memoryBanks ) {
			int passCount = 0;
			List<int[]> previousConfigs = new List<int[]>();
			int[] currentMemoryConfig = new int[ memoryBanks.Length ];

			Array.Copy( memoryBanks, currentMemoryConfig, memoryBanks.Length );

			while( !ListContainsArray( previousConfigs, currentMemoryConfig ) ) {
				previousConfigs.Add( currentMemoryConfig );
				currentMemoryConfig = ReallocateMemory( currentMemoryConfig );
				passCount++;
			}

			return passCount;
		}

		/// <summary>
		/// Find how large of a loop we create when allowed to reallocate infinitely.
		/// </summary>
		/// <remarks>
		/// This debug function does not modify the memory banks in place.
		/// </remarks>
		/// <param name="memoryBanks">The memory bank configuration to reallocate over.</param>
		/// <returns>The number of reallocations we can make before we start looping.</returns>
		private int GetSizeOfLoop( int[] memoryBanks ) {
			int passCount = 0;
			List<int[]> previousConfigs = new List<int[]>();
			int[] currentMemoryConfig = new int[ memoryBanks.Length ];

			Array.Copy( memoryBanks, currentMemoryConfig, memoryBanks.Length );

			while( !ListContainsArray( previousConfigs, currentMemoryConfig ) ) {
				previousConfigs.Add( currentMemoryConfig );
				currentMemoryConfig = ReallocateMemory( currentMemoryConfig );
				passCount++;
			}
			
			return passCount - GetIndexOfArrayInList( previousConfigs, currentMemoryConfig );
		}

		/// <summary>
		/// Check if a list of arrays has an exact match for a given array configuration.
		/// </summary>
		/// <remarks>
		/// Integer array matching only.
		/// </remarks>
		/// <param name="list">The list of arrays to search.</param>
		/// <param name="array">The array to match against.</param>
		/// <returns>True if the array is inside the list; false otherwise.</returns>
		private bool ListContainsArray( List<int[]> list, int[] array ) {
			return GetIndexOfArrayInList( list, array ) >= 0;
		}

		/// <summary>
		/// Find the first index of a given array configuration inside of a list of arrays.
		/// </summary>
		/// <remarks>
		/// Integer array matching only.
		/// </remarks>
		/// <param name="list">The list of arrays to search.</param>
		/// <param name="array">The array to match against.</param>
		/// <returns>The first index the given array can be found at, or -1 if the array is not in the list.</returns>
		private int GetIndexOfArrayInList( List<int[]> list, int[] array ) {
			for( int i = 0; i < list.Count; i++ ) {
				if( ArraysMatch( list[ i ], array ) ) {
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Compare two arrays to see if their contents match exactly.
		/// </summary>
		/// <param name="a">The first array to compare.</param>
		/// <param name="b">The second array to compare.</param>
		/// <returns>True if the arrays match; false otherwise.</returns>
		private bool ArraysMatch( int[] a, int[] b ) {
			for( int i = 0; i < a.Length; i++ ) {
				if( a[ i ] != b[ i ] ) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Reallocate memory blocks across an array of memory banks.
		/// </summary>
		/// <remarks>
		/// This does not modify the memory banks in place.
		/// </remarks>
		/// <param name="memoryConfig">The memory bank configuration to reallocate.</param>
		/// <returns>The new memory configuration.</returns>
		private int[] ReallocateMemory( int[] memoryConfig ) {
			int distributorBankIndex = GetMaxIndex( memoryConfig );
			int blocksToDistribute = memoryConfig[ distributorBankIndex ];
			int[] reallocatedConfig = new int[ memoryConfig.Length ];
			int nextBankIndex = distributorBankIndex;

			Array.Copy( memoryConfig, reallocatedConfig, memoryConfig.Length );

			reallocatedConfig[ distributorBankIndex ] = 0;
			while( blocksToDistribute > 0 ) {
				nextBankIndex++;
				if( nextBankIndex >= memoryConfig.Length ) {
					nextBankIndex = 0;
				}

				reallocatedConfig[ nextBankIndex ]++;
				blocksToDistribute--;
			}

			return reallocatedConfig;
		}

		/// <summary>
		/// Find the first index of the largest integer in an array.
		/// </summary>
		/// <param name="array">The array to search.</param>
		/// <returns>The first index containing the largest integer, or -1 if the array is empty.</returns>
		private int GetMaxIndex( int[] array ) {
			if( array == null || array.Length <= 0 ) {
				return -1;
			}

			int maxValue = array[ 0 ];
			int maxIndex = 0;

			for( int i = 1; i < array.Length; i++ ) {
				if( array[ i ] > maxValue ) {
					maxValue = array[ i ];
					maxIndex = i;
				}
			}

			return maxIndex;
		}
	}
}
