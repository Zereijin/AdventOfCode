using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2016.Day02 {
	/// <summary>
	/// Directional instructions for our finger.
	/// </summary>
	struct Instruction {
		public string directions;

		/// <summary>
		/// Create an instruction.
		/// </summary>
		/// <param name="directions">A string with directional input.</param>
		/// <remarks>
		/// Input is read as:
		/// U = Up
		/// D = Down
		/// L = Left
		/// R = Right
		/// 
		/// From our current keyPad position, we will move one button in each direction requested, ignoring moves that would take us
		/// off the keyPad.
		/// </remarks>
		public Instruction( string directions ) {
			this.directions = directions;
		}
	}

	class Day02 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "ULL\nRRDDD\nLURDL\nUUUUD", "1985", 1 ) );			
			testCases.Add( new TestCase( "ULL\nRRDDD\nLURDL\nUUUUD", "5DB3", 2 ) );
		}

		/// <summary>
		/// Break the string input down into finger instructions.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input converted into a list of instructions for our finger to follow.</returns>
		private List<Instruction> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<Instruction> instructions = new List<Instruction>();

			foreach( string entry in inputArray ) {
				instructions.Add( new Instruction( entry ) );
			}

			return instructions;
		}

		public override string Solve( string input, int part ) {
			List<Instruction> instructions = ParseInput( input );

			switch( part ) {
				case 1:
					return GetBathroomCode( instructions );
				case 2:
					return GetRealBathroomCode( instructions );
			}

			return "";
		}

		/// <summary>
		/// Deduce the bathroom code, assuming a standard 9-button numPad.
		/// </summary>
		/// <param name="instructions">A list of finger instructions.</param>
		/// <returns>The bathroom code.</returns>
		private string GetBathroomCode( List<Instruction> instructions ) {
			char[,] numPad = {
				{ '1', '2', '3' },
				{ '4', '5', '6' },
				{ '7', '8', '9' }
			};
			
			return GetCodeForKeypad( numPad, 1, 1, instructions );
		}

		/// <summary>
		/// Deduce the bathroom code, using the actual keyPad.
		/// </summary>
		/// <param name="instructions"></param>
		/// <returns></returns>
		private string GetRealBathroomCode( List<Instruction> instructions ) {
			char[,] keyPad = {
				{ '#', '#', '1', '#', '#' },
				{ '#', '2', '3', '4', '#' },
				{ '5', '6', '7', '8', '9' },
				{ '#', 'A', 'B', 'C', '#' },
				{ '#', '#', 'D', '#', '#' },
			};
			
			return GetCodeForKeypad( keyPad, 0, 2, instructions );
		}

		/// <summary>
		/// Deduce a code for a given keyPad
		/// </summary>
		/// <param name="keyPad">An array representation of the keyPad to use.</param>
		/// <param name="startX">The starting x-coordinate of the user's finger.</param>
		/// <param name="startY">The starting y-coordinate of the user's finger.</param>
		/// <param name="instructions">A list of finger instructions for determining the code.</param>
		/// <returns></returns>
		private string GetCodeForKeypad( char[,] keyPad, int startX, int startY, List<Instruction> instructions ) {
			int xPos = startX;
			int yPos = startY;
			string code = "";

			foreach( Instruction instruction in instructions ) {
				char[] steps = instruction.directions.ToCharArray();

				foreach( char step in steps ) {
					int xTarget = xPos;
					int yTarget = yPos;

					switch( step ) {
						case 'U':
							yTarget--;
							break;
						case 'D':
							yTarget++;
							break;
						case 'L':
							xTarget--;
							break;
						case 'R':
							xTarget++;
							break;
					}

					// Only update our current position if the last move was valid.
					if( yTarget >= 0 && yTarget < keyPad.GetLength( 0 ) ) {
						if( xTarget >= 0 && xTarget < keyPad.GetLength( 1 ) ) {
							if( keyPad[ yTarget, xTarget ] != '#' ) {
								xPos = xTarget;
								yPos = yTarget;
							}
						}
					}
				}

				code += keyPad[ yPos, xPos ];
			}
			//test
			return code;
		}
	}
}
