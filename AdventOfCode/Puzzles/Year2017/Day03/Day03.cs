using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode.Puzzles.Year2017.Day03 {
	enum Facing {
		RIGHT,
		UP,
		LEFT,
		DOWN
	}

	class Day03 : Puzzle {
		protected Point dronePosition;
		protected Facing droneFacing = Facing.RIGHT;
		protected int[,] map;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "1", "0", 1 ) );
			testCases.Add( new TestCase( "12", "3", 1 ) );
			testCases.Add( new TestCase( "23", "2", 1 ) );
			testCases.Add( new TestCase( "1024", "31", 1 ) );
			testCases.Add( new TestCase( "747", "806", 2 ) );
		}

		private int ParseInput( string input ) {
			return Int32.Parse( input );
		}
		
		public override string Solve( string input, int part ) {
			int targetNumber = ParseInput( input );

			int mapSize = GetNeededSizeOfMap( targetNumber );
			int midpoint = mapSize / 2;

			
			map = new int[ mapSize, mapSize ];
			for( int i = 0; i < mapSize; i++ ) {
				for( int j = 0; j < mapSize; j++ ) {
					map[ i, j ] = 0;
				}
			}

			switch( part ) {
				case 1:
					CreateSpiral( mapSize, midpoint );
					break;
				case 2:
					return "" + CreateSumSpiralUntil( mapSize, midpoint, targetNumber );
					break;
				default:
					return String.Format( "Day 03 part {0} solver not found.", part );
			}

			Point numberIndex = GetTargetIndex( map, targetNumber );

			return "" + ( Math.Abs( numberIndex.X - midpoint ) + Math.Abs( numberIndex.Y - midpoint ) );
		}

		private int[,] CreateSpiral( int mapSize, int midpoint ) {
			RunSpiralDrone( mapSize, midpoint );
			
			return map;
		}

		private int CreateSumSpiralUntil( int mapSize, int midpoint, int targetNumber ) {
			return RunSumSpiralDroneUntil( mapSize, midpoint, targetNumber );
		}

		private void RunSpiralDrone( int mapSize, int midpoint) {
			int nextNumber = 1;
			dronePosition = new Point( midpoint, midpoint );

			while( Step( mapSize, nextNumber ) ) {
				nextNumber++;
			}
		}

		private int RunSumSpiralDroneUntil( int mapSize, int midpoint, int targetNumber ) {
			int nextNumber = 1;
			dronePosition = new Point( midpoint, midpoint );

			while( nextNumber <= targetNumber ) {
				Step( mapSize, nextNumber );
				nextNumber = GetSurroundingSum( dronePosition.X, dronePosition.Y, mapSize );
			}

			return nextNumber;
		}

		private int GetSurroundingSum( int x, int y, int mapSize ) {
			int surroundingSum = 0;

			for( int i = -1; i <= 1; i++ ) {
				for( int j = -1; j <= 1; j++ ) {
					Point droneProbe = new Point( dronePosition.X + i, dronePosition.Y + j );

					if( droneProbe.X >= 0 && droneProbe.X < mapSize && droneProbe.Y >= 0 && droneProbe.Y < mapSize && !( i == 0 && j == 0 ) ) {
						surroundingSum += map[ droneProbe.X, droneProbe.Y ];
					}
				}
			}

			return surroundingSum;
		}


		private bool Step( int mapSize, int nextNumber ) {
			map[ dronePosition.X, dronePosition.Y ] = nextNumber;

			switch( droneFacing ) {
				// Move forward 1 step in the direction we're facing.
				case Facing.RIGHT:
					dronePosition.X++;
					break;
				case Facing.UP:
					dronePosition.Y++;
					break;
				case Facing.LEFT:
					dronePosition.X--;
					break;
				case Facing.DOWN:
					dronePosition.Y--;
					break;
			}

			if( dronePosition.X < 0 || dronePosition.X >= mapSize || dronePosition.Y < 0 || dronePosition.Y >= mapSize ) {
				return false;
			}

			CheckForTurn();

			return true;
		}

		private void CheckForTurn() {
			switch( droneFacing ) {
				case Facing.RIGHT:
					if( map[ dronePosition.X, dronePosition.Y + 1 ] == 0 ) {
						droneFacing = Facing.UP;
					}
					break;
				case Facing.UP:
					if( map[ dronePosition.X - 1, dronePosition.Y ] == 0 ) {
						droneFacing = Facing.LEFT;
					}
					break;
				case Facing.LEFT:
					if( map[ dronePosition.X, dronePosition.Y - 1 ] == 0 ) {
						droneFacing = Facing.DOWN;
					}
					break;
				case Facing.DOWN:
					if( map[ dronePosition.X + 1, dronePosition.Y ] == 0 ) {
						droneFacing = Facing.RIGHT;
					}
					break;
			}
		}

		private int GetNeededSizeOfMap( int largestNumber ) {
			for( int i = 1; true; i += 2 ) {
				if( i * i >= largestNumber ) {
					return i;
				}
			}
		}

		private Point GetTargetIndex( int[,] map, int targetNumber ) {
			int maxRowIndex = map.GetLength( 0 ) - 1;
			int maxColIndex = map.GetLength( 1 ) - 1;

			for( int i = 0; i <= maxRowIndex; i++ ) {
				for( int j = 0 ; j <= maxColIndex; j++ ) {
					if( i == 0 || i == maxRowIndex || j == 0 || j == maxColIndex ) {
						if( map[ i, j ] == targetNumber ) {
							return new Point( i, j );
						}
					}
				}
			}

			return new Point();
		}
	}
}
