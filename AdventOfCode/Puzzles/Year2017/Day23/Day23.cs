using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day23 {
	abstract class Command {
		private Regex numberRegex = new Regex( @"(-?\d+)" );

		public abstract Int64 Execute( Dictionary<char, Int64> registry );

		protected Int64 GetValue( string input, Dictionary<char, Int64> registry ) {
			Match match = numberRegex.Match( input );
			if( match.Success ) {
				return Int64.Parse( match.Groups[ 1 ].Value );
			}

			return GetFromRegistry( registry, input[ 0 ] );
		}

		protected Int64 GetFromRegistry( Dictionary<char, Int64> registry, char register ) {
			if( !registry.ContainsKey( register ) ) {
				registry.Add( register, 0 );
			}

			return registry[ register ];
		}
	}

	class Set : Command {
		private char register;
		private string value;

		public Set( char register, string value ) {
			this.register = register;
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			GetFromRegistry( registry, register );
			registry[ register ] = GetValue( value, registry );

			return registry[ register ];
		}
	}

	class Subtract : Command {
		private char register;
		private string value;

		public Subtract( char register, string value ) {
			this.register = register;
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			GetFromRegistry( registry, register );
			registry[ register ] -= GetValue( value, registry );

			return registry[ register ];
		}
	}

	class Multiply : Command {
		private char register;
		private string value;

		public Multiply( char register, string value ) {
			this.register = register;
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			GetFromRegistry( registry, register );
			registry[ register ] *= GetValue( value, registry );

			return registry[ register ];
		}
	}

	class JumpIfNot0 : Command {
		private string value;
		private string jumpDistance;

		public JumpIfNot0( string value, string jumpDistance ) {
			this.value = value;
			this.jumpDistance = jumpDistance;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			if( GetValue( value, registry ) != 0 ) {
				return GetValue( jumpDistance, registry );
			}

			return 0;
		}
	}

	class Day23 : Puzzle {
		Dictionary<char, Int64> registry;
		int mulCounter;

		protected override void SetupTestCases() {
			base.SetupTestCases();
		}

		private List<Command> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			Regex setRegex = new Regex( @"set (\w) (\S+)" );
			Regex subtractRegex = new Regex( @"sub (\w) (\S+)" );
			Regex multiplyRegex = new Regex( @"mul (\w) (\S+)" );
			Regex jumpRegex = new Regex( @"jnz (\S+) (\S+)" );
			List<Command> commands = new List<Command>();

			foreach( string entry in inputArray ) {
				Match match = setRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Set( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = subtractRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Subtract( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = multiplyRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Multiply( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = jumpRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new JumpIfNot0( match.Groups[ 1 ].Value, match.Groups[ 2 ].Value ) );
					continue;
				}
			}

			return commands;
		}

		public override string Solve( string input, int part ) {
			registry = new Dictionary<char, long>();
			mulCounter = 0;

			List<Command> commands = ParseInput( input );

			switch( part ) {
				case 1:
					Run( commands );
					return "" + mulCounter;
				//case 2:
				//	return "" + GetProgram1SendCountFromThreadedDuet( input );
			}

			return String.Format( "Day 23 part {0} solver not found.", part );
		}

		private void Run( List<Command> commands ) {
			int index = 0;
			while( index >= 0 && index < commands.Count ) {
				Command command = commands[ index ];

				if( command.GetType() == typeof( Multiply ) ) {
					command.Execute( registry );
					mulCounter++;
				} else if( command.GetType() == typeof( JumpIfNot0 ) ) {
					Int64 jumpOffset = command.Execute( registry );

					if( jumpOffset != 0 ) {
						index += (int)command.Execute( registry );
						continue;
					}
				} else {
					command.Execute( registry );
				}

				index++;
			}
		}
	}
}