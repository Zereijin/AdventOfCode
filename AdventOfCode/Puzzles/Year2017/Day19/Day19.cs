using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day19 {
	enum Facing {
		RIGHT,
		UP,
		LEFT,
		DOWN
	}

	class Day19 : Puzzle {
		private char[,] map;
		private Point dronePosition;
		private Facing droneFacing;
		private Regex whitespaceRegex = new Regex( @"\s" );
		private int stepsTaken;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testMaze = @"     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ ";

			testCases.Add( new TestCase( testMaze, "ABCDEF", 1 ) );
			testCases.Add( new TestCase( testMaze, "38", 2 ) );
		}

		private void ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );

			map = new char[ inputArray.Length, inputArray[ 0 ].Length ];

			for( int i = 0; i < inputArray.Length; i++ ) {
				string entry = inputArray[ i ];

				for( int j = 0; j < entry.Length; j++ ) {
					map[ i, j ] = entry[ j ];
				}
			}
		}

		public override string Solve( string input, int part ) {
			ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetLettersOnPath();
				case 2:
					return "" + GetPathLength();
			}

			return String.Format( "Day 19 part {0} solver not found.", part );
		}

		private string GetLettersOnPath() {
			dronePosition = new Point( 0, 0 );
			droneFacing = Facing.DOWN;
			stepsTaken = 0;

			int mapWidth = map.GetLength( 1 );
			for( int i = 0; i < mapWidth; i++ ) {
				if( map[ 0, i ] != ' ' ) {
					dronePosition.X = i;
					break;
				}
			}

			return WalkPath();
		}

		
		private int GetPathLength() {
			int oldStepsTaken = stepsTaken;
			Point oldDronePosition = dronePosition;
			Facing oldDroneFacing = droneFacing;

			GetLettersOnPath();

			return stepsTaken;
		}

		private string WalkPath() {
			string pathValue = "";
			bool deadEndFound = false;

			while( !deadEndFound ) {
				stepsTaken++;

				char droneChar = map[ dronePosition.Y, dronePosition.X ];
				if( !( droneChar == '|' || droneChar == '-' || droneChar == '+' ) ) {
					pathValue += droneChar;
				}

				if( CanWalk( dronePosition, droneFacing ) ) {
					dronePosition = GetPositionAfterWalkingDirection( dronePosition, droneFacing );
				} else {
					switch( droneFacing ) {
						case Facing.DOWN:
							if( CanWalk( dronePosition, Facing.LEFT ) ) {
								droneFacing = Facing.LEFT;								
							} else if( CanWalk( dronePosition, Facing.RIGHT ) ) {
								droneFacing = Facing.RIGHT;
							} else {
								deadEndFound = true;
							}
							break;
						case Facing.LEFT:
							if( CanWalk( dronePosition, Facing.UP ) ) {
								droneFacing = Facing.UP;								
							} else if( CanWalk( dronePosition, Facing.DOWN ) ) {
								droneFacing = Facing.DOWN;
							} else {
								deadEndFound = true;
							}
							break;
						case Facing.RIGHT:
							if( CanWalk( dronePosition, Facing.UP ) ) {
								droneFacing = Facing.UP;								
							} else if( CanWalk( dronePosition, Facing.DOWN ) ) {
								droneFacing = Facing.DOWN;
							} else {
								deadEndFound = true;
							}
							break;
						case Facing.UP:
							if( CanWalk( dronePosition, Facing.LEFT ) ) {
								droneFacing = Facing.LEFT;								
							} else if( CanWalk( dronePosition, Facing.RIGHT ) ) {
								droneFacing = Facing.RIGHT;
							} else {
								deadEndFound = true;
							}
							break;
					}

					if( !deadEndFound ) {
						dronePosition = GetPositionAfterWalkingDirection( dronePosition, droneFacing );
					}
				}
			}

			return pathValue;
		}

		private bool CanWalk( Point startPosition, Facing direction ) {
			Point targetPos = GetPositionAfterWalkingDirection( startPosition, direction );

			if( targetPos.X < 0 || targetPos.X >= map.GetLength( 1 ) ) return false;
			if( targetPos.Y < 0 || targetPos.Y >= map.GetLength( 0 ) ) return false;
			if( whitespaceRegex.IsMatch( "" + map[ targetPos.Y, targetPos.X ] ) ) return false;
			return true;
		}

		private Point GetPositionAfterWalkingDirection( Point startPosition, Facing direction ) {
			Point targetPos = new Point( startPosition.X, startPosition.Y );

			switch( direction ) {
				case Facing.DOWN:
					targetPos.Y++;
					break;
				case Facing.LEFT:
					targetPos.X--;
					break;
				case Facing.RIGHT:
					targetPos.X++;
					break;
				case Facing.UP:
					targetPos.Y--;
					break;
			}

			return targetPos;
		}
	}
}
