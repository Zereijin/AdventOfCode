using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day05 {
	class Day05 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "0\n3\n0\n1\n-3", "5", 1 ) );
			testCases.Add( new TestCase( "0\n3\n0\n1\n-3", "10", 2 ) );
		}
		
		/// <summary>
		/// Break the input string down into an array of jump instructions.
		/// </summary>
		/// <param name="input">The input string</param>
		/// <returns>The input string converted to an array of jump instructions.</returns>
		private int[] ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			int[] instructions = new int[ inputArray.Length ];

			for( int i = 0; i < inputArray.Length; i++ ) {
				instructions[ i ] = Int32.Parse( inputArray[ i ] );
			}

			return instructions;
		}

		public override string Solve( string input, int part ) {
			int[] instructions = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetStepsUntilExitWhileIncrementing( instructions );
				case 2:
					return "" + GetStepsUntilExitWhileDancing( instructions );
			}
			
			return String.Format( "Day 05 part {0} solver not found.", part );
		}

		/// <summary>
		/// Using each value in an array as a jump instruction and starting at index 0, find how many instructions it takes to go out of bounds.
		/// After following each instruction, increment that instruction for future visits.
		/// </summary>
		/// <param name="instructions">The array of jump instructions.</param>
		/// <returns>The number of jumps it takes to get out of bounds.</returns>
		private int GetStepsUntilExitWhileIncrementing( int[] instructions ) {
			int stepsTaken = 0;
			int currentIndex = 0;
			int[] mutatedInstructions = new int[ instructions.Length ];

			Array.Copy( instructions, mutatedInstructions, instructions.Length );

			while( currentIndex >= 0 && currentIndex < instructions.Length ) {
				int instruction = mutatedInstructions[ currentIndex ];
				mutatedInstructions[ currentIndex ]++;
				currentIndex += instruction;
				stepsTaken++;
			}

			return stepsTaken;
		}
		
		/// <summary>
		/// Using each value in an array as a jump instruction and starting at index 0, find how many instructions it takes to go out of bounds.
		/// After following each instruction, increment that instruction for future visits if it's less than 3, or else decrement it.
		/// </summary>
		/// <remarks>
		/// "Dancing" is not a very intuitive name for this function-- but I don't know what _would_ be.
		/// </remarks>
		/// <param name="instructions">The array of jump instructions.</param>
		/// <returns>The number of jumps it takes to get out of bounds.</returns>
		private int GetStepsUntilExitWhileDancing( int[] instructions ) {
			int stepsTaken = 0;
			int currentIndex = 0;
			int[] mutatedInstructions = new int[ instructions.Length ];

			Array.Copy( instructions, mutatedInstructions, instructions.Length );

			while( currentIndex >= 0 && currentIndex < instructions.Length ) {
				int instruction = mutatedInstructions[ currentIndex ];

				if( instruction >= 3 ) {
					mutatedInstructions[ currentIndex ]--;
				} else {
					mutatedInstructions[ currentIndex ]++;
				}
				currentIndex += instruction;
				stepsTaken++;
			}

			return stepsTaken;
		}
	}
}
