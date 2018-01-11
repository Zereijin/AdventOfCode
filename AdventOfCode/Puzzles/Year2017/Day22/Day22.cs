using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode.Puzzles.Year2017.Day22 {
	enum Facing {
		UP,
		RIGHT,
		DOWN,
		LEFT
	}

	class Day22 : Puzzle {
		private Dictionary<int, Dictionary<int, bool>> infectionGrid;
		private Facing carrierFacing;
		private Point carrierPosition;
		private int infectionBurstCount;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testMap = @"..#
#..
...";

			testCases.Add( new TestCase( testMap, "5587", 1 ) );
		}

		private Dictionary<int, Dictionary<int, bool>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );

			int gridHeight = inputArray.Length;
			int gridWidth = inputArray[ 0 ].TrimEnd().Length;
			Point midPoint = new Point( ( gridWidth - 1 ) / 2, ( gridHeight - 1 ) / 2 );

			Dictionary<int, Dictionary<int, bool>> infectionGrid = new Dictionary<int, Dictionary<int, bool>>();

			for( int i = 0; i < inputArray.Length; i++ ) {
				int yPos = i - midPoint.Y;
				infectionGrid.Add( yPos, new Dictionary<int, bool>() );

				for( int j = 0; j < inputArray[ i ].TrimEnd().Length; j++ ) {
					int xPos = j - midPoint.X;
					infectionGrid[ yPos ].Add( xPos, inputArray[ i ][ j ] == '#' );
				}
			}

			return infectionGrid;
		}

		public override string Solve( string input, int part ) {
			infectionGrid = ParseInput( input );
			infectionBurstCount = 0;
			carrierPosition = new Point( 0, 0 );
			carrierFacing = Facing.UP;

			for( int i = 0; i < 10000; i++ ) {
				StepVirus();
			}

			return "" + infectionBurstCount;

			return String.Format( "Day 22 part {0} solver not found.", part );
		}

		private void StepVirus() {
			// If infected, turn right.  Else, turn left.
			if( !infectionGrid.ContainsKey( carrierPosition.Y ) ) {
				infectionGrid.Add( carrierPosition.Y, new Dictionary<int, bool>() );
			}

			if( !infectionGrid[ carrierPosition.Y ].ContainsKey( carrierPosition.X ) ) {
				infectionGrid[ carrierPosition.Y ].Add( carrierPosition.X, false );
			}

			bool isCurrentNodeInfected = infectionGrid[ carrierPosition.Y ][ carrierPosition.X ];
			if( isCurrentNodeInfected ) {
				TurnCarrierRight();
			} else {
				TurnCarrierLeft();
				infectionBurstCount++;
			}

			// Flip current node state.
			infectionGrid[ carrierPosition.Y ][ carrierPosition.X ] = !isCurrentNodeInfected;

			// Move carrier forward.
			MoveCarrierForward();
		}

		//private int GetInfectedNodeCount() {
		//	int infectionCount = 0;

		//	List<int> yKeys = new List<int>( infectionGrid.Keys );
		//	foreach( int y in yKeys ) {
		//		List<int> xKeys = new List<int>( infectionGrid[ y ].Keys );
		//		foreach( int x in xKeys ) {
		//			if( infectionGrid[ y ][ x ] ) {
		//				infectionCount++;
		//			}					
		//		}
		//	}

		//	return infectionCount;
		//}

		private void TurnCarrierRight() {
			switch( carrierFacing ) {
				case Facing.UP:
					carrierFacing = Facing.RIGHT;
					break;
				case Facing.RIGHT:
					carrierFacing = Facing.DOWN;
					break;
				case Facing.DOWN:
					carrierFacing = Facing.LEFT;
					break;
				case Facing.LEFT:
					carrierFacing = Facing.UP;
					break;
			}
		}

		private void TurnCarrierLeft() {
			for( int i = 0; i < 3; i++ ) {
				TurnCarrierRight();
			}
		}

		private void MoveCarrierForward() {		
			switch( carrierFacing ) {
				case Facing.UP:
					carrierPosition.Y--;
					break;
				case Facing.RIGHT:
					carrierPosition.X++;
					break;
				case Facing.DOWN:
					carrierPosition.Y++;
					break;
				case Facing.LEFT:
					carrierPosition.X--;
					break;
			}
		}
	}
}
