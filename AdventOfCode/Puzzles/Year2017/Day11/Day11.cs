using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day11 {
	class Day11 : Puzzle {
		enum Direction {
			N,
			NE,
			SE,
			S,
			SW,
			NW
		}
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "ne,ne,ne", "3", 1 ) );
			testCases.Add( new TestCase( "ne,ne,sw,sw", "0", 1 ) );
			testCases.Add( new TestCase( "ne,ne,s,s", "2", 1 ) );
			testCases.Add( new TestCase( "se,sw,se,sw,sw", "3", 1 ) );
		}

		private List<Direction> ParseInput( string input ) {
			string[] inputArray = input.Split( ',' );
			List<Direction> directions = new List<Direction>();

			foreach( string entry in inputArray ) {
				if( entry == "n" ) {
					directions.Add( Direction.N );
				} else if( entry == "ne" ) {
					directions.Add( Direction.NE );
				} else if( entry == "se" ) {
					directions.Add( Direction.SE );
				} else if( entry == "s" ) {
					directions.Add( Direction.S );
				} else if( entry == "sw" ) {
					directions.Add( Direction.SW );
				} else if( entry == "nw" ) {
					directions.Add( Direction.NW );
				}
			}

			return directions;
		}

		public override string Solve( string input, int part ) {
			switch( part ) {
				case 1: return "" + GetDistance( ParseInput( input ) );
				case 2: return "" + GetMaximumDistance( ParseInput( input ) );
			}
			
			return String.Format( "Day 11 part {0} solver not found.", part );
		}

		private int GetDistance( List<Direction> directions ) {
			int nsCount = 0;
			int neswCount = 0;
			int nwseCount = 0;

			foreach( Direction direction in directions ) {
				switch( direction ) {
					case Direction.N:
						nsCount++;
						break;
					case Direction.NE:
						neswCount++;
						break;
					case Direction.SE:
						nwseCount--;
						break;
					case Direction.S:
						nsCount--;
						break;
					case Direction.SW:
						neswCount--;
						break;
					case Direction.NW:
						nwseCount++;
						break;
				}
			}

			if( nsCount > 0 && nwseCount < 0 ) {
				int redundancy = GetRedundancy( nsCount, nwseCount );
				nsCount -= redundancy;
				nwseCount += redundancy;
				neswCount += redundancy;
			}
			if( neswCount > 0 && nsCount < 0 ) {
				int redundancy = GetRedundancy( neswCount, nsCount );
				nsCount += redundancy;
				nwseCount -= redundancy;
				neswCount -= redundancy;
			}
			if( nwseCount < 0 && neswCount < 0 ) {
				int redundancy = GetRedundancy( nwseCount, neswCount );
				nsCount -= redundancy;
				nwseCount += redundancy;
				neswCount += redundancy;
			}
			if( nsCount < 0 && nwseCount > 0 ) {
				int redundancy = GetRedundancy( nsCount, nwseCount );
				nsCount += redundancy;
				nwseCount -= redundancy;
				neswCount -= redundancy;
			}
			if( neswCount < 0 && nsCount > 0 ) {
				int redundancy = GetRedundancy( neswCount, nsCount );
				nsCount -= redundancy;
				nwseCount += redundancy;
				neswCount += redundancy;
			}
			if( nwseCount > 0 && neswCount > 0 ) {
				int redundancy = GetRedundancy( nwseCount, neswCount );
				nsCount += redundancy;
				nwseCount -= redundancy;
				neswCount -= redundancy;
			}

			return Math.Abs( nsCount ) + Math.Abs( neswCount ) + Math.Abs( nwseCount );
		}

		private int GetRedundancy( int a, int b ) {
			int absA = Math.Abs( a );
			int absB = Math.Abs( b );

			return Math.Max( absA, absB ) - ( Math.Max( absA, absB ) - Math.Min( absA, absB ) );
		}

		private int GetMaximumDistance( List<Direction> directions ) {
			int maximumDistance = 0;

			for( int i = 1; i < directions.Count; i++ ) {
				int currentDistance = GetDistance( directions.GetRange( 0, i ) );
				if( currentDistance > maximumDistance ) {
					maximumDistance = currentDistance;
				}
			}

			return maximumDistance;
		}
	}
}