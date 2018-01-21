using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day25 {
	class Day25 : Puzzle {
		static private Dictionary<char, State> states;
		static private int cursorPosition;
		static private Dictionary<int, bool> tape;
		static private char state;
		private int stepsUntilChecksum;

		class State {
			Action[] actions;

			public State( Action act0, Action act1 ) {
				actions = new Action[ 2 ];
				actions[ 0 ] = act0;
				actions[ 1 ] = act1;
			}
		
			public void Run( bool currentValue ) {
				if( currentValue ) {
					actions[ 1 ].Run();
				} else {
					actions[ 0 ].Run();
				}
			}
		}

		class Action {
			private bool valueToWrite;
			private int cursorDirection;
			private char nextState;

			public Action( bool value, int move, char next ) {
				valueToWrite = value;
				cursorDirection = move;
				nextState = next;
			}

			public void Run() {
				tape[ cursorPosition ] = valueToWrite;
				cursorPosition += cursorDirection;
				state = nextState;
			}
		}
	
		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testTuring = @"Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.";

			testCases.Add( new TestCase( testTuring, "3", 1 ) );
		}

		private void ParseInput( string input ) {
			states = new Dictionary<char, State>();
			cursorPosition = 0;
			tape = new Dictionary<int, bool>();

			Regex initRegex = new Regex( @"Begin in state (\w)\.
Perform a diagnostic checksum after (\d+) steps\." );
			Regex stateRegex = new Regex( @"In state (\w):
\s*If the current value is 0:
\s*- Write the value (\d)\.
\s*- Move one slot to the (\w+)\.
\s*- Continue with state (\w)\.
\s*If the current value is 1:
\s*- Write the value (\d)\.
\s*- Move one slot to the (\w+)\.
\s*- Continue with state (\w)\." );

			Match initMatch = initRegex.Match( input );
			MatchCollection stateMatches = stateRegex.Matches( input );
			
			state = initMatch.Groups[ 1 ].Value[ 0 ];
			stepsUntilChecksum = Int32.Parse( initMatch.Groups[ 2 ].Value );

			foreach( Match stateMatch in stateMatches ) {
				Action act0 = new Action( stateMatch.Groups[ 2 ].Value == "1", stateMatch.Groups[ 3 ].Value == "right" ? 1 : -1, stateMatch.Groups[ 4 ].Value[ 0 ] );
				Action act1 = new Action( stateMatch.Groups[ 5 ].Value == "1", stateMatch.Groups[ 6 ].Value == "right" ? 1 : -1, stateMatch.Groups[ 7 ].Value[ 0 ] );
				states[ stateMatch.Groups[ 1 ].Value[ 0 ] ] = new State( act0, act1 );
			}
		}

		public override string Solve( string input, int part ) {
			ParseInput( input );

			for( int i = 0; i < stepsUntilChecksum; i++ ) {
				if( !tape.ContainsKey( cursorPosition ) ) {
					tape[ cursorPosition ] = false;
				}

				states[ state ].Run( tape[ cursorPosition ] );
			}

			int countOf1s = 0;
			foreach( KeyValuePair<int, bool> item in tape ) {
				if( item.Value ) {
					countOf1s++;
				}
			}
			
			return "" + countOf1s;

			return String.Format( "Day 25 part {0} solver not found.", part );
		}
	}
}
