using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day06 {
	class Day06 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "0\t2\t7\t0", "5", 1 ) );
			testCases.Add( new TestCase( "0\t2\t7\t0", "4", 2 ) );
		}

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
				default:
					return String.Format( "Day 06 part {0} solver not found.", part );
			}			
		}

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

		private bool ListContainsArray( List<int[]> list, int[] array ) {
			foreach( int[] item in list ) {
				if( ArraysMatch( item, array ) ) {
					return true;
				}
			}

			return false;
		}

		private int GetIndexOfArrayInList( List<int[]> list, int[] array ) {
			for( int i = 0; i < list.Count; i++ ) {
				if( ArraysMatch( list[ i ], array ) ) {
					return i;
				}
			}

			return -1;
		}

		private bool ArraysMatch( int[] a, int[] b ) {
			for( int i = 0; i < a.Length; i++ ) {
				if( a[ i ] != b[ i ] ) {
					return false;
				}
			}

			return true;
		}

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

		private int GetMaxIndex( int[] array ) {
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
