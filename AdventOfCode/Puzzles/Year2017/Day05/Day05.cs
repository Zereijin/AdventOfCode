using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day05 {
	class Day05 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "0\n3\n0\n1\n-3", "5", 1 ) );
			testCases.Add( new TestCase( "0\n3\n0\n1\n-3", "10", 2 ) );
		}

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
					break;
				case 2:
					return "" + GetStepsUntilExitWhileDancing( instructions );
					break;
				default:
					return String.Format( "Day 05 part {0} solver not found.", part );
			}
		}

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
