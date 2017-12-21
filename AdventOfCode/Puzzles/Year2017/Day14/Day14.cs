using System;
using System.Collections.Generic;
using AdventOfCode.Puzzles.Year2017;

namespace AdventOfCode.Puzzles.Year2017.Day14 {
	class Day14 : Puzzle {
		private List<int>[] visitedSquares;
		private KnotHash[] knotHashArray;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "flqrgnkx", "8108", 1 ) );
			testCases.Add( new TestCase( "flqrgnkx", "1242", 2 ) );
		}

		public override string Solve( string input, int part ) {
			List<KnotHash> knotHashes = new List<KnotHash>();

			for( int i = 0; i < 128; i++ ) {
				KnotHash knotHash = new KnotHash( 256 );
				knotHash.InitializeStrong( input + "-" + i );
				knotHashes.Add( knotHash );
			}

			switch( part ) {
				case 1: return "" + GetUsedSquares( knotHashes );
				case 2: return "" + GetRegionCount( knotHashes );
			}
			
			return String.Format( "Day 14 part {0} solver not found.", part );
		}

		private int GetUsedSquares( List<KnotHash> knotHashes ) {
			int usedCount = 0;

			foreach( KnotHash hash in knotHashes ) {
				char[] binString = hash.ToBinString().ToCharArray();

				foreach( char c in binString ) {
					if( c == '1' ) {
						usedCount++;
					}
				}
			}

			return usedCount;
		}

		private int GetRegionCount( List<KnotHash> knotHashes ) {
			knotHashArray = knotHashes.ToArray();
			int groupCount = 0;

			visitedSquares = new List<int>[ knotHashes.Count ];
			for( int i = 0; i < visitedSquares.Length; i++ ) {
				visitedSquares[ i ] = new List<int>();
			}

			for( int i = 0; i < knotHashArray.Length; i++ ) {
				string binString = knotHashArray[ i ].ToBinString();

				for( int j = 0; j < binString.Length; j++ ) {
					if( visitedSquares[ i ].Contains( j ) ) {
						continue;
					}

					if( binString[ j ] == '1' ) {
						Probe( j, i );
						groupCount++;
					}
				}
			}

			return groupCount;
		}

		private void Probe( int x, int y ) {
			// Don't check above or below array boundaries
			if( y < 0 || y >= knotHashArray.Length ) return;
			
			string currentRowBinString = knotHashArray[ y ].ToBinString();
			// Don't check left or right of array boundaries
			if( x < 0 || x >= currentRowBinString.Length  ) return;

			// Don't check visited squares
			if( visitedSquares[ y ].Contains( x ) ) return;

			visitedSquares[ y ].Add( x );

			// Don't check empty squares
			if( currentRowBinString[ x ] != '1' ) return;

			Probe( x - 1, y );
			Probe( x + 1, y );
			Probe( x, y - 1 );
			Probe( x, y + 1 );
		}
	}
}
