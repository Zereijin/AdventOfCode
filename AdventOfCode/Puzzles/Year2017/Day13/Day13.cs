using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day13 {
	class Scanner {
		private int position;
		private int direction;
		private int size;

		public Scanner( int size ) {
			Reset();
			this.size = size;
		}

		public void Step() {
			if( position == 0 || position == size - 1 ) {
				direction *= -1;
			}

			position += direction;
		}

		public int GetSize() {
			return size;
		}

		public int GetPosition() {
			return position;
		}

		public void Reset() {
			position = 0;
			direction = -1;
		}
	}

	class Day13 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( @"0: 3
1: 2
4: 4
6: 4", "24", 1 ) );
			testCases.Add( new TestCase( @"0: 3
1: 2
4: 4
6: 4", "10", 2 ) );
		}

		private Dictionary<int, Scanner> ParseInput( string input ) {
			Dictionary<int, Scanner> scanners = new Dictionary<int, Scanner>();
			Regex scannerRegex = new Regex( @"(\d+):\s*(\d+)\n?" );
			string[] inputArray = input.Split( '\n' );

			foreach( string entry in inputArray ) {
				Match match = scannerRegex.Match( entry );

				Scanner scanner = new Scanner( Int32.Parse( match.Groups[ 2 ].Value ) );
				scanners.Add( Int32.Parse( match.Groups[ 1 ].Value ), scanner );
			}

			return scanners;
		}

		public override string Solve( string input, int part ) {
			Dictionary<int, Scanner> scanners = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetSeverityAfterWait( 0, scanners );
				case 2:
					return "" + GetWaitUntilSafePassage( scanners );
			}

			return String.Format( "Day 13 part {0} solver not found.", part );
		}

		private int GetSeverityAfterWait( int wait, Dictionary<int, Scanner> scanners ) {
			List<int> scannerKeys = new List<int>( scanners.Keys );
			int maxKey = -1;

			foreach( int scannerKey in scannerKeys ) {
				if( scannerKey > maxKey ) {
					maxKey = scannerKey;
				}

				Scanner scanner = scanners[ scannerKey ];
				for( int i = 0; i < wait; i++ ) {
					scanner.Step();
				}
			}
			
			int severity = 0;
			for( int i = 0; i < maxKey + 1; i++ ) {
				if( scannerKeys.Contains( i ) && scanners[ i ].GetPosition() == 0 ) {
					severity += i * scanners[ i ].GetSize();
				}

				foreach( int scannerKey in scannerKeys ) {
					scanners[ scannerKey ].Step();
				}
			}

			return severity;
		}

		private int GetWaitUntilSafePassage( Dictionary<int, Scanner> scanners ) {
			List<int> scannerKeys = new List<int>( scanners.Keys );
			int wait = -1;

			// 0 > 1 2 3 5 6 7 9 10		0
			// 1 > 0 2 4 6 8 10			1
			// 4 > 0 1 3 4 5 6 7 9 10	2
			// 6 > 1 2 3 4 5 7 8 9 10	0

			// wait % ( size + 1 - pos ) - pos
			// ( wait + pos ) % ( size + ( size - 2 ) )
			
			bool waitFound = false;
			while( !waitFound ) {
				waitFound = true;
				wait++;

				foreach( int scannerKey in scannerKeys ) {
					int scannerSize = scanners[ scannerKey ].GetSize();
					int mod = scannerSize * 2 - 2;

					if( ( wait + scannerKey ) % mod == 0 ) {
						waitFound = false;
						break;
					}
				}
			}

			return wait;

			//while( true ) {
			//	if( wait % ( scanners[ 0 ].GetSize() + 1 ) != 0 ) {
			//		foreach( int scannerKey in scannerKeys ) {
			//			scanners[ scannerKey ].Reset();
			//		}

			//		if( GetSeverityAfterWait( wait, scanners ) == 0 ) {
			//			break;
			//		}
			//	}

			//	wait++;
			//}

			return wait;
		}
	}
}
