using System;
using System.Collections.Generic;
using AdventOfCode.Puzzles.Year2017;

namespace AdventOfCode.Puzzles.Year2017.Day10 {
	class Day10 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			//testCases.Add( new TestCase( "3,4,1,5", "12", 1 ) );
		}

		public override string Solve( string input, int part ) {
			KnotHash knotHash = new KnotHash( 256 );

			switch( part ) {
				case 1:
					knotHash.InitializeWeak( input );
					return "" + knotHash.hash[ 0 ] * knotHash.hash[ 1 ];
				case 2:
					knotHash.InitializeStrong( input );
					return knotHash.ToHexString();
			}

			return String.Format( "Day 10 part {0} solver not found.", part );
		}
	}
}
