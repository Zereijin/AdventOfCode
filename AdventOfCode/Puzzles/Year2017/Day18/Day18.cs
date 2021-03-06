﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day18 {
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

	class Play : Command  {
		private string frequency;

		public Play( string frequency ) {
			this.frequency = frequency;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			return GetValue( frequency, registry );
		}

		public string GetFrequencyString() {
			return frequency;
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

	class Add : Command {
		private char register;
		private string value;

		public Add( char register, string value ) {
			this.register = register;
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			GetFromRegistry( registry, register );
			registry[ register ] += GetValue( value, registry );

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

	class Modulo : Command {
		private char register;
		private string value;

		public Modulo( char register, string value ) {
			this.register = register;
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			GetFromRegistry( registry, register );
			registry[ register ] %= GetValue( value, registry );

			return registry[ register ];
		}
	}

	class Recover : Command {
		private string value;

		public Recover( string value ) {
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			return GetValue( value, registry );
		}

		public string GetValueString() {
			return value;
		}
	}

	class JumpIfGreaterThan0 : Command {
		private string value;
		private string jumpDistance;

		public JumpIfGreaterThan0( string value, string jumpDistance ) {
			this.value = value;
			this.jumpDistance = jumpDistance;
		}

		public override Int64 Execute( Dictionary<char, Int64> registry ) {
			if( GetValue( value, registry ) > 0 ) {
				return GetValue( jumpDistance, registry );
			}

			return 0;
		}
	}

	class Send : Command {
		private string value;

		public Send( string value ) {
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, long> registry ) {
			return GetValue( value, registry );
		}
	}

	class Receive : Command {
		private string value;

		public Receive( string value ) {
			this.value = value;
		}

		public override Int64 Execute( Dictionary<char, long> registry ) {
			return GetValue( value, registry );
		}

		public string GetValueString() {
			return value;
		}
	}

	class Day18 : Puzzle {
		Dictionary<char, Int64> registry;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testDuet = @"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2";
			string testThreaded = @"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d";

			testCases.Add( new TestCase( @"snd 0
rcv 0
snd 1
set a 0
rcv a
snd 2
add a -1
rcv a
snd 3
rcv b", "2", 1 ) );
			testCases.Add( new TestCase( testDuet, "4", 1 ) );
			testCases.Add( new TestCase( testThreaded, "3", 2 ) );
		}

		private List<Command> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			Regex soundRegex = new Regex( @"snd (\S+)" );
			Regex setRegex = new Regex( @"set (\w) (\S+)" );
			Regex addRegex = new Regex( @"add (\w) (\S+)" );
			Regex multiplyRegex = new Regex( @"mul (\w) (\S+)" );
			Regex moduloRegex = new Regex( @"mod (\w) (\S+)" );
			Regex recoverRegex = new Regex( @"rcv (\S+)" );
			Regex jumpRegex = new Regex( @"jgz (\S+) (\S+)" );
			List<Command> commands = new List<Command>();

			foreach( string entry in inputArray ) {
				Match match = soundRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Play( match.Groups[ 1 ].Value ) );
					continue;
				}
				
				match = setRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Set( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = addRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Add( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = multiplyRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Multiply( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = moduloRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Modulo( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value ) );
					continue;
				}
				
				match = recoverRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new Recover( match.Groups[ 1 ].Value ) );
					continue;
				}
				
				match = jumpRegex.Match( entry );
				if( match.Value != "" ) {
					commands.Add( new JumpIfGreaterThan0( match.Groups[ 1 ].Value, match.Groups[ 2 ].Value ) );
					continue;
				}
			}

			return commands;
		}

		private List<Command> ParseInputForThreadedDuet( string input ) {
			List<Command> commands = ParseInput( input );

			for( int i = 0; i < commands.Count; i++ ) {
				Command command = commands[ i ];

				if( command.GetType() == typeof( Play ) ) {
					commands[ i ] = new Send( ( (Play)command ).GetFrequencyString() );
				} else if( command.GetType() == typeof( Recover ) ) {
					commands[ i ] = new Receive( ( (Recover)command ).GetValueString() );
				}
			}

			return commands;
		}

		public override string Solve( string input, int part ) {
			switch( part ) {
				case 1:
					return PlayDuet( input );
				case 2:
					return "" + GetProgram1SendCountFromThreadedDuet( input );
			}

			return String.Format( "Day 18 part {0} solver not found.", part );
		}

		public string PlayDuet( string input ) {			
			registry = new Dictionary<char, Int64>();
			List<Command> commands = ParseInput( input );
			Int64 lastSound = -1;

			int index = 0;
			while( index < commands.Count ) {
				Command command = commands[ index ];

				if( command.GetType() == typeof( Play ) ) {
					lastSound = command.Execute( registry );
				} else if( command.GetType() == typeof( Recover ) ) {
					if( command.Execute( registry ) != 0 ) {
						return "" + lastSound;
					}
				} else if( command.GetType() == typeof( JumpIfGreaterThan0 ) ) {
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

			return "failure";
		}

		public Int64 GetProgram1SendCountFromThreadedDuet( string input ) {
			List<Command> commands = ParseInputForThreadedDuet( input );
			List<Program> programs = new List<Program>();

			for( int i = 0; i < 2; i++ ) {
				programs.Add( new Program( i, commands ) );
			}

			programs[ 0 ].partner = programs[ 1 ];
			programs[ 1 ].partner = programs[ 0 ];

			List<bool> programsActive = new List<bool>();
			foreach( Program program in programs ) {
				programsActive.Add( true );
			}

			bool isAProgramStillRunning = true;
			while( isAProgramStillRunning ) {
				isAProgramStillRunning = false;

				for( int i = 0; i < 2; i++ ) {
					if( programsActive[ i ] ) {
						programsActive[ i ] = programs[ i ].AttemptNextCommand();
					}

					isAProgramStillRunning = isAProgramStillRunning || programsActive[ i ];
				}
			}

			return programs[ 1 ].sendCount;
		}
	}

	class Program {
		private Dictionary<char, Int64> registry;
		private List<Command> commandList;

		public Int64 sendCount;
		public Program partner;

		private Queue<Int64> receiveQueue;
		private bool isSleeping;
		private int commandIndex;

		public Program( int id, List<Command> commands ) {
			registry = new Dictionary<char, Int64>();
			registry[ 'p' ] = id;
			commandList = commands;

			sendCount = 0;

			isSleeping = false;
			receiveQueue = new Queue<Int64>();
			commandIndex = 0;
		}

		public void ExecuteReceive( Receive command ) {
			registry[ command.GetValueString()[ 0 ] ] = receiveQueue.Dequeue();
		}

		/// <summary>
		/// Try the program's next command in the queue.
		/// </summary>
		/// <returns>False if the program terminated; true otherwise.</returns>
		public bool AttemptNextCommand() {
			Command command = commandList[ commandIndex ];

			if( isSleeping ) {
				if( receiveQueue.Count > 0 ) {
					isSleeping = false;
					ExecuteReceive( (Receive)command );
					commandIndex++;
				} else {
					// Check for Deadlock
					if( partner.IsSleeping() ) {
						return false;
					}
				}

				return true;
			}
			
			if( command.GetType() == typeof( Send ) ) {
				sendCount++;
				partner.Receive( ( (Send)command ).Execute( registry ) );
			} else if( command.GetType() == typeof( Receive ) ) {
				if( receiveQueue.Count <= 0 ) {
					isSleeping = true;
					return true;
				} else {
					ExecuteReceive( (Receive)command );
				}
			} else if( command.GetType() == typeof( JumpIfGreaterThan0 ) ) {
				Int64 jumpOffset = command.Execute( registry );

				if( jumpOffset != 0 ) {
					commandIndex += (int)command.Execute( registry );
					return !IsCommandIndexOutOfBounds();
				}
			} else {
				command.Execute( registry );
			}

			commandIndex++;
			return !IsCommandIndexOutOfBounds();
		}

		private bool IsCommandIndexOutOfBounds() {
			if( commandIndex < 0 || commandIndex >= commandList.Count ) {
				return true;
			}

			return false;
		}

		public bool IsSleeping() {
			return isSleeping;
		}

		public void Receive( Int64 value ) {
			receiveQueue.Enqueue( value );
		}
	}
}
