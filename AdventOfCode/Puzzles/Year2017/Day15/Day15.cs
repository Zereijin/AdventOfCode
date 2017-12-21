using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day15 {
	class Generator {
		private long prevValue;
		private int factor;
		private const int divisor = 2147483647;

		public Generator( int factor, long seed ) {
			this.prevValue = seed;
			this.factor = factor;
		}

		public long GetNextNumber() {
			prevValue = ( prevValue * factor ) % divisor;
			return prevValue;
		}
	}

	class Day15 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( @"Generator A starts with 65
Generator B starts with 8921", "588", 1 ) );
		}

		private int[] ParseInput( string input ) {
			Regex generatoSeedRegex = new Regex( @"Generator \w starts with (\d+)\n?");
			MatchCollection matches = generatoSeedRegex.Matches( input );

			int[] generatorSeeds = new int[ matches.Count ];

			for( int i = 0; i < generatorSeeds.Length; i++ ) {
				Match match = matches[ i ];
				generatorSeeds[ i ] = Int32.Parse( match.Groups[ 1 ].Value );
			}

			return generatorSeeds;
		}

		public override string Solve( string input, int part ) {
			int[] seeds = ParseInput( input );
			int[] factors = new int[] { 16807, 48271 };
			Generator[] generators = new Generator[ seeds.Length ];

			for( int i = 0; i < seeds.Length; i++ ) {
				generators[ i ] = new Generator( factors[ i ], seeds[ i ] );
			}

			switch( part ) {
				case 1: return "" + FindMatchesIn40MRounds( generators );
				case 2: return "" + FindPickyMatchesIn5MRounds( generators );
			}

			return String.Format( "Day 15 part {0} solver not found.", part );
		}

		private int FindMatchesIn40MRounds( Generator[] generators ) {
			int count = 0;

			for( int i = 0; i < 40000000; i++ ) {
				long a = generators[ 0 ].GetNextNumber();
				long b = generators[ 1 ].GetNextNumber();

				if( MatchLast16Bits( a, b ) ) {
					count++;
				}
			}

			return count;
		}

		private int FindPickyMatchesIn5MRounds( Generator[] generators ) {
			int count = 0;

			for( int i = 0; i < 5000000; i++ ) {
				long a = -1;
				while( a % 4 != 0 ) {
					a = generators[ 0 ].GetNextNumber();
				}
				
				long b = -1;
				while( b % 8 != 0 ) {
					b = generators[ 1 ].GetNextNumber();
				}

				if( MatchLast16Bits( a, b ) ) {
					count++;
				}
			}

			return count;
		}

		private bool MatchLast16Bits( long a, long b ) {
			for( int i = 0; i < 16; i++ ) {
				if( ( a & 1 ) != ( b & 1 ) ) {
					return false;
				}

				a = a >> 1;
				b = b >> 1;
			}

			return true;
		}
	}
}
