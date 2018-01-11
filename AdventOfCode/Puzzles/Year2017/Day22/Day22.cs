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

	enum NodeState {
		CLEAN,
		WEAKENED,
		INFECTED,
		FLAGGED
	}

	class Day22 : Puzzle {
		private Dictionary<int, Dictionary<int, NodeState>> infectionGrid;
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

		private Dictionary<int, Dictionary<int, NodeState>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );

			int gridHeight = inputArray.Length;
			int gridWidth = inputArray[ 0 ].TrimEnd().Length;
			Point midPoint = new Point( ( gridWidth - 1 ) / 2, ( gridHeight - 1 ) / 2 );

			Dictionary<int, Dictionary<int, NodeState>> infectionGrid = new Dictionary<int, Dictionary<int, NodeState>>();

			for( int i = 0; i < inputArray.Length; i++ ) {
				int yPos = i - midPoint.Y;
				infectionGrid.Add( yPos, new Dictionary<int, NodeState>() );

				for( int j = 0; j < inputArray[ i ].TrimEnd().Length; j++ ) {
					int xPos = j - midPoint.X;
					NodeState state = inputArray[ i ][ j ] == '#' ? NodeState.INFECTED : NodeState.CLEAN;
					infectionGrid[ yPos ].Add( xPos, state );
				}
			}

			return infectionGrid;
		}

		public override string Solve( string input, int part ) {
			infectionGrid = ParseInput( input );
			infectionBurstCount = 0;
			carrierPosition = new Point( 0, 0 );
			carrierFacing = Facing.UP;

			switch( part ) {
				case 1:
					for( int i = 0; i < 10000; i++ ) {
						StepSporificaVirus();
					}
					break;
				case 2:
					//for( int i = 0; i < 10000000; i++ ) {
					//	StepEvolvedSporificaVirus();
					//}
					break;
				default:
					return String.Format( "Day 22 part {0} solver not found.", part );
			}

			return "" + infectionBurstCount;
		}

		private void StepSporificaVirus() {
			// If infected, turn right.  Else, turn left.
			if( !infectionGrid.ContainsKey( carrierPosition.Y ) ) {
				infectionGrid.Add( carrierPosition.Y, new Dictionary<int, NodeState>() );
			}

			if( !infectionGrid[ carrierPosition.Y ].ContainsKey( carrierPosition.X ) ) {
				infectionGrid[ carrierPosition.Y ].Add( carrierPosition.X, NodeState.CLEAN );
			}

			NodeState currentNodeState = infectionGrid[ carrierPosition.Y ][ carrierPosition.X ];
			if( currentNodeState == NodeState.INFECTED ) {
				TurnCarrierRight();
			} else {
				TurnCarrierLeft();
				infectionBurstCount++;
			}

			// Flip current node state.
			if( currentNodeState == NodeState.INFECTED ) {
				infectionGrid[ carrierPosition.Y ][ carrierPosition.X ] = NodeState.CLEAN;
			} else {
				infectionGrid[ carrierPosition.Y ][ carrierPosition.X ] = NodeState.INFECTED;
			}

			// Move carrier forward.
			MoveCarrierForward();
		}

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
