using System;
using System.Collections.Generic;
using System.Drawing;

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
			List<Point> visitedAddresses = new List<Point>();
			xPos = yPos = 0;

			foreach( Instruction instruction in instructions ) {
				Turn( instruction.direction );

				// Walk one block at a time, mapping new locations and verifying if we've been here before.
				for( int i = 0; i < instruction.distance; i++ ) {
					Point currentAddress = new Point( xPos, yPos );

					// If we're revisiting somewhere, we can stop walking; we've reached our destination.
					if( visitedAddresses.Contains( currentAddress ) ) {
						break;
					}

					// Log the current location as visited.
					visitedAddresses.Add( currentAddress );

					Walk( 1 );
				}
			}
			
			return "" + ( Math.Abs( xPos ) + Math.Abs( yPos ) );
		}

		/// <summary>
		/// Update the drone's facing after a rotation instruction.
		/// </summary>
		/// <param name="direction">The direction to turn.</param>
		private void Turn( Direction direction ) {
			int numFacings = Enum.GetNames( typeof( Facing ) ).Length;

			switch( direction ) {
				case Direction.LEFT:
					currentFacing--;

					// If we go left of NORTH (our leftmost direction), wrap around to WEST.
					while( currentFacing < Facing.NORTH ) {
						currentFacing += numFacings;
					}
					break;
				case Direction.RIGHT:
					currentFacing++;

					// If we go right of WEST (our rightmost direction), wrap around to NORTH.
					while( currentFacing > Facing.WEST ) {
						currentFacing -= numFacings;
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
