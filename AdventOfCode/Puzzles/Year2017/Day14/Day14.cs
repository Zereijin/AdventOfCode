using System;
using System.Collections.Generic;
using AdventOfCode.Puzzles.Year2017;

namespace AdventOfCode.Puzzles.Year2017.Day14 {
	class Day14 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "flqrgnkx", "8108", 1 ) );
		}

		private List<int> ParseInput( string input ) {
			char[] inputArray = input.ToCharArray();
			List<int> lengths = new List<int>();

			foreach( char entry in inputArray ) {
				lengths.Add( (int)entry );
			}

			return lengths;
		}

		public override string Solve( string input, int part ) {
			List<KnotHash> knotHashes = new List<KnotHash>();

			for( int i = 0; i < 128; i++ ) {
				List<int> init = ParseInput( input + "-" + i );
				KnotHash knotHash = new KnotHash( 128 );
				knotHash.Initialize( init );
				knotHashes.Add( knotHash );
			}

			return "" + GetUsedSquares( knotHashes );

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
	}
}
