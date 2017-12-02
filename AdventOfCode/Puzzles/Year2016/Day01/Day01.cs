using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2016.Day01 {
	/// <summary>
	/// A rotation instruction.
	/// </summary>
	enum Direction {
		LEFT,
		RIGHT
	}

	/// <summary>
	/// A compass direction that the drone can face.
	/// </summary>
	enum Facing {
		NORTH,
		EAST,
		SOUTH,
		WEST
	}

	/// <summary>
	/// An instruction for the drone to follow.
	/// </summary>
	struct Instruction {
		public Direction direction;
		public int distance;

		/// <summary>
		/// Creates a drone instruction.
		/// </summary>
		/// <param name="direction">The direction to rotate.</param>
		/// <param name="distance">The number of blocks to walk after rotating.</param>
		public Instruction( Direction direction, int distance ) {
			this.direction = direction;
			this.distance = distance;
		}
	}

	class Day01 : Puzzle {
		private Facing currentFacing;
		private int xPos; // Current x-position in the city.
		private int yPos; // Current y-position in the city.

		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "R2, L3", "5", 1 ) );
			testCases.Add( new TestCase( "R2, R2, R2", "2", 1 ) );
			testCases.Add( new TestCase( "R5, L5, R5, R3", "12", 1 ) );
			testCases.Add( new TestCase( "R8, R4, R4, R8", "4", 2 ) );
		}

		/// <summary>
		/// Break the string input down into drone instructions.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input converted into a list of instructions that the drone can understand.</returns>
		private List<Instruction> ParseInput( string input ) {
			string[] inputArray = input.Split( new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries );
			List<Instruction> instructions = new List<Instruction>();

			foreach( string entry in inputArray ) {
				Direction direction;
				int distance = 0;

				string directionChar = entry.Substring( 0, 1 );
				switch( directionChar ) {
					case "L":
						direction = Direction.LEFT;
						break;
					case "R":
						direction = Direction.RIGHT;
						break;
					default:
						// We cannot rotate DENNIS.
						Console.WriteLine( String.Format( "ERROR:  Couldn't decode instruction \"{0}\"", entry ) );
						continue;
				}

				distance = Int32.Parse( entry.Substring( 1 ) );

				instructions.Add( new Instruction( direction, distance ) );
			}

			return instructions;
		}

		public override string Solve( string input, int part ) {
			currentFacing = Facing.NORTH;

			List<Instruction> instructions = ParseInput( input );

			switch( part ) {
				case 1:
					return FindShortestPathDistance( instructions );
				case 2:
					return FindFirstDuplicateDistance( instructions );
			}

			return "";
		}

		/// <summary>
		/// Finds the distance (in blocks) between the starting location, and the final destination after all instructions have been followed.
		/// </summary>
		/// <param name="instructions">A list of drone instructions.</param>
		/// <returns>The total distance between the start and end locations.</returns>
		private string FindShortestPathDistance( List<Instruction> instructions ) {
			xPos = yPos = 0;

			foreach( Instruction instruction in instructions ) {
				Turn( instruction.direction );
				Walk( instruction.distance );
			}

			// Disregard any negativity our position might have, man.
			return "" + ( Math.Abs( xPos ) + Math.Abs( yPos ) );
		}

		/// <summary>
		/// Traverses a set of instructions until we arrive at a location we've already visited.
		/// </summary>
		/// <param name="instructions">A list of drone instructions.</param>
		/// <returns>The total distance between the start location and first revisit location.</returns>
		private string FindFirstDuplicateDistance( List<Instruction> instructions ) {
			// Dealing with a possibly-infinite city is tricky.  So let's cheat, and make it large, but finite!
			int memorySize = 1000;
			bool[][] memoryMap= new bool[ memorySize ][];
			for( int i = 0; i < memorySize; i++ ) {
				bool[] memoryRow = new bool[ memorySize ];

				for( int j = 0; j < memorySize; j++ ) {
					memoryRow[ j ] = false;
				}

				memoryMap[ i ] = memoryRow;
			}

			// 0,0 is not a valid starting point this time, since we're mapping with an array, and negative array indices have unintended behaviours.
			int initialPos = memorySize / 2;
			xPos = yPos = initialPos;

			foreach( Instruction instruction in instructions ) {
				Turn( instruction.direction );

				// Walk one block at a time, mapping new locations and verifying if we've been here before.
				for( int i = 0; i < instruction.distance; i++ ) {
					// If we're revisiting somewhere, we can stop walking; we've reached our destination.
					if( memoryMap[ xPos ][ yPos ] ) {
						break;
					}

					// Mark our location as visited.
					memoryMap[ xPos ][ yPos ] = true;

					Walk( 1 );
				}
			}

			// Remove the initial offset while calculating the final distance.
			return "" + ( Math.Abs( xPos - initialPos ) + Math.Abs( yPos - initialPos ) );
		}

		/// <summary>
		/// Update the drone's facing after a rotation instruction.
		/// </summary>
		/// <param name="direction">The direction to turn.</param>
		private void Turn( Direction direction ) {
			int numDirections = Enum.GetNames( typeof( Direction ) ).Length;

			switch( direction ) {
				case Direction.LEFT:
					currentFacing--;

					// If we go left of NORTH (our leftmost direction), wrap around to WEST.
					while( currentFacing < Facing.NORTH ) {
						currentFacing += numDirections;
					}
					break;
				case Direction.RIGHT:
					currentFacing++;

					// If we go right of WEST (our rightmost direction), wrap around to NORTH.
					while( currentFacing > Facing.WEST ) {
						currentFacing -= numDirections;
					}
					break;
			}
		}

		/// <summary>
		/// Move the drone forward.
		/// </summary>
		/// <param name="distance">The number of blocks to walk.</param>
		private void Walk( int distance ) {
			switch( currentFacing ) {
				case Facing.NORTH:
					yPos += distance;
					break;
				case Facing.SOUTH:
					yPos -= distance;
					break;
				case Facing.EAST:
					xPos += distance;
					break;
				case Facing.WEST:
					xPos -= distance;
					break;
			}
		}
	}
}
