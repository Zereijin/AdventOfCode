using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day17 {
	class Day17 : Puzzle {
		List<int> spinlock;
		int currentPos;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "3", "638", 1 ) );
		}

		public override string Solve( string input, int part ) {
			switch( part ) {
				case 1:
					InitializeSpinlock( Int32.Parse( input ) );
					return "" + spinlock[ currentPos + 1 ];
				case 2:
					return "" + GetFromIndexForSize(Int32.Parse( input ), 1, 50000000 );
			}

			return String.Format( "Day 17 part {0} solver not found.", part );
		}

		private void InitializeSpinlock( int stepSize ) {
			spinlock = new List<int>();
			spinlock.Add( 0 );
			currentPos = 0;

			for( int i = 1; i <= 2017; i++ ) {
				currentPos = WrapTo( currentPos + stepSize, spinlock.Count ) + 1;
				spinlock.Insert( currentPos, i );
			}
		}

		private int GetFromIndexForSize( int stepSize, int index, Int32 size ) {
			int pos = 0;
			Int32 mockSize = 1;

			while( true ) {
				int stepsUntilWrap = ( mockSize - pos ) / ( stepSize - 1 );
				if( mockSize + stepsUntilWrap > size ) {
					break;
				}

				mockSize += stepsUntilWrap;
				pos = WrapTo( pos, mockSize ) + 1;
			}

			// Pos is at the lowest index after the last wrap happened in 50M

			while( pos != 1 ) {
				mockSize--;
				pos = WrapTo( pos - stepSize, mockSize ) - 1;
			}

			return mockSize;
		}

		private int WrapTo( Int32 position, int size ) {
			if( position < 0 ) return position + size;

			return position % size;
		}
	}
}
