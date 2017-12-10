using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day08 {
	enum Operation {
		INC,
		DEC
	}

	struct Instruction {
		public string register;
		public Operation op;
		public int opAmount;
		public string condition;
	}

	class Day08 : Puzzle {
		private Dictionary<string, int> registers;
		private int maxEverRegisterValue;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string exampleInput = @"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10";

			testCases.Add( new TestCase( exampleInput, "1", 1 ) );
			testCases.Add( new TestCase( exampleInput, "10", 2 ) );
		}

		private List<Instruction> ParseInput( string input ) {
			List<Instruction> instructions = new List<Instruction>();
			string[] inputArray = input.Split( '\n' );
			Regex parseRegex = new Regex( @"(\w+) (\w+) (-?\d+) if (.*)" );
			
			foreach( string entry in inputArray ) {
				Match match = parseRegex.Match( entry );
				
				Instruction instruction = new Instruction();
				instruction.register = match.Groups[ 1 ].Value;
				instruction.op = ( match.Groups[ 2 ].Value == "inc" ? Operation.INC : Operation.DEC );
				instruction.opAmount = Int32.Parse( match.Groups[ 3 ].Value );
				instruction.condition = match.Groups[ 4 ].Value;

				instructions.Add( instruction );
			}
			
			return instructions;
		}

		public override string Solve( string input, int part ) {
			registers = new Dictionary<string, int>();
			List<Instruction> instructions = ParseInput( input );
			
			ExecuteInstructions( instructions );
			
			switch( part ) {
				case 1:
					return "" + GetMaxRegisterValue();
				case 2:
					return "" + maxEverRegisterValue;
			}
			
			return String.Format( "Day 08 part {0} solver not found.", part );
		}

		private void ExecuteInstructions( List<Instruction> instructions ) {
			maxEverRegisterValue = 0;

			foreach( Instruction i in instructions ) {
				Execute( i );

				int currentMax = GetMaxRegisterValue();
				if( currentMax > maxEverRegisterValue ) {
					maxEverRegisterValue = currentMax;
				}
			}
		}

		private void Execute( Instruction i ) {
			Regex conditionRegex = new Regex( @"(\w+) (.*) (-?\d+)" );
			Match match = conditionRegex.Match( i.condition );

			string conditionRegisterName = match.Groups[ 1 ].Value;
			string comparison = match.Groups[ 2 ].Value;
			int comparee = Int32.Parse( match.Groups[ 3 ].Value );

			switch( comparison ) {
				case ">":
					if( !( GetRegisterValue( conditionRegisterName ) > comparee ) ) {
						return;
					}
					break;
				case "<":
					if( !( GetRegisterValue( conditionRegisterName ) < comparee ) ) {
						return;
					}
					break;
				case ">=":
					if( !( GetRegisterValue( conditionRegisterName ) >= comparee ) ) {
						return;
					}
					break;
				case "<=":
					if( !( GetRegisterValue( conditionRegisterName ) <= comparee ) ) {
						return;
					}
					break;
				case "==":
					if( !( GetRegisterValue( conditionRegisterName ) == comparee ) ) {
						return;
					}
					break;
				case "!=":
					if( !( GetRegisterValue( conditionRegisterName ) != comparee ) ) {
						return;
					}
					break;
			}

			// Conditional passed
			GetRegisterValue( i.register );
			switch( i.op ) {
				case Operation.INC:
					registers[ i.register ] += i.opAmount;
					break;
				case Operation.DEC:
					registers[ i.register ] -= i.opAmount;
					break;
			}
		}

		private int GetRegisterValue( string name ) {
			if( !registers.ContainsKey( name ) ) {
				registers.Add( name, 0 );
			}

			return registers[ name ];
		}

		private int GetMaxRegisterValue() {
			int maxValue = 0;

			foreach( KeyValuePair<string, int> entry in registers ) {
				if( entry.Value > maxValue ) {
					maxValue = entry.Value;
				}
			}

			return maxValue;
		}
	}
}
