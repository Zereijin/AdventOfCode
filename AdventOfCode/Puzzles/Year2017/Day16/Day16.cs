using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day16 {
	interface IMove {
		string ExecuteOn( string programs );
	}

	class Spin : IMove  {
		private int distance;

		public Spin( int distance ) {
			this.distance = distance;
		}

		public string ExecuteOn( string programs ) {
			int splitIndex = programs.Length - ( distance % programs.Length );

			string head = programs.Substring( splitIndex );
			string tail = programs.Substring( 0, splitIndex );

			return head + tail;
		}
	}

	class Exchange : IMove {
		private int aPos;
		private int bPos;

		public Exchange( int aPos, int bPos ) {
			this.aPos = aPos;
			this.bPos = bPos;
		}

		public string ExecuteOn( string programs ) {
			char[] programArray = programs.ToCharArray();
			char temp = programArray[ aPos ];
			programArray[ aPos ] = programArray[ bPos ];
			programArray[ bPos ] = temp;

			string result = "";
			foreach( char c in programArray ) {
				result += c;
			}

			return result;
		}
	}

	class Partner : IMove {
		private char a;
		private char b;

		public Partner( char a, char b ) {
			this.a = a;
			this.b = b;
		}

		public string ExecuteOn( string programs ) {
			int aPos = programs.IndexOf( a );
			int bPos = programs.IndexOf( b );

			char[] programArray = programs.ToCharArray();
			char temp = programArray[ aPos ];
			programArray[ aPos ] = programArray[ bPos ];
			programArray[ bPos ] = temp;

			string result = "";
			foreach( char c in programArray ) {
				result += c;
			}

			return result;
		}
	}

	class Day16 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();
			
			testCases.Add( new TestCase( "s3", "nopabcdefghijklm", 1 ) );
			testCases.Add( new TestCase( "x2/15", "abpdefghijklmnoc", 1 ) );
			testCases.Add( new TestCase( "pf/c", "abfdecghijklmnop", 1 ) );
			testCases.Add( new TestCase( "s1,x3/4,pe/b", "paedcbfghijklmno", 1 ) );
		}

		private List<IMove> ParseInput( string input ) {
			string[] inputArray = input.Split( ',' );
			Regex spinRegex = new Regex( @"s(\d+)" );
			Regex exchangeRegex = new Regex( @"x(\d+)/(\d+)" );
			Regex partnerRegex = new Regex( @"p(\w)/(\w)" );
			List<IMove> moves = new List<IMove>();

			foreach( string entry in inputArray ) {
				Match match = spinRegex.Match( entry );
				if( match.Value != "" ) {
					moves.Add( new Spin( Int32.Parse( match.Groups[ 1 ].Value ) ) );
					continue;
				}
				
				match = exchangeRegex.Match( entry );
				if( match.Value != "" ) {
					moves.Add( new Exchange( Int32.Parse( match.Groups[ 1 ].Value ), Int32.Parse( match.Groups[ 2 ].Value ) ) );
					continue;
				}
				
				match = partnerRegex.Match( entry );
				if( match.Value != "" ) {
					moves.Add( new Partner( match.Groups[ 1 ].Value[ 0 ], match.Groups[ 2 ].Value[ 0 ] ) );
					continue;
				}
			}

			return moves;
		}

		public override string Solve( string input, int part ) {
			string dancers = "abcdefghijklmnop";
			List<IMove> moves = ParseInput( input );

			switch( part ) {
				case 1: return Dance( dancers, moves );
				case 2: return Marathon( dancers, moves );
			}

			return String.Format( "Day 16 part {0} solver not found.", part );
		}

		private string Dance( string dancers, List<IMove> moves ) {
			foreach( IMove move in moves ) {
				dancers = move.ExecuteOn( dancers );
			}

			return dancers;
		}

		private string Marathon( string dancers, List<IMove> moves ) {
			char[] startPositions = dancers.ToCharArray();
			int[] currentPositions = new int[ dancers.Length ];

			int roundsUntilReset = 0;
			bool resetAchieved = false;
			while( !resetAchieved ) {
				roundsUntilReset++;
				dancers = Dance( dancers, moves );

				if( dancers == "abcdefghijklmnop" ) {
					resetAchieved = true;
				}
			}

			for( int i = 0; i < 1000000000 % roundsUntilReset; i++ ) {
				dancers = Dance( dancers, moves );
			}

			return dancers;
		}
	}
}
