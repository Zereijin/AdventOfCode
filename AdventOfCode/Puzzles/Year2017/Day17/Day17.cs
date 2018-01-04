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
					return "" + GetIndex1ForSpinLock( Int32.Parse( input ), 50000000 );
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

		private int GetIndex1ForSpinLock( int stepSize, Int32 size ) {
			int nextIndex = 0;
			int index1Value = 0;

			for( int currentSize = 1; currentSize <= size; currentSize++ ) {
				if( nextIndex == 1 ) {
					index1Value = currentSize - 1;
				}

				nextIndex = WrapTo( nextIndex + stepSize, currentSize ) + 1;
			}

			return index1Value;
		}

		private int WrapTo( Int32 position, Int32 size ) {
			if( position < 0 ) return position + size;

			return position % size;
		}
	}
}
