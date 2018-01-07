using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day21 {
	class Rule {
		private string pattern;
		private string matchOutput;
		private int size;
		private HashSet<string> permutations;

		public Rule( string pattern, string matchOutput ) {
			this.pattern = pattern;
			this.matchOutput = matchOutput;
			size = ( pattern.Length == 5 ? 2 : 3 );
			CreatePermutations();
		}

		private void CreatePermutations() {
			permutations = new HashSet<string>();
			string currentPattern = pattern;

			for( int i = 0; i < 4; i++ ) {
				permutations.Add( currentPattern );
				permutations.Add( FlipX( currentPattern ) );
				permutations.Add( FlipY( currentPattern ) );
				permutations.Add( FlipX( FlipY( currentPattern ) ) );

				currentPattern = RotateClockwise( currentPattern );
			}
		}

		public string CheckMatch( string query ) {
			foreach( string permutation in permutations ) {
				if( query == permutation ) {
					return matchOutput;
				}
			}

			return null;
		}

		private string FlipX( string pattern ) {
			string[] grid = pattern.Split( '/' );

			for( int i = 0; i < grid.Length; i++ ) {
				string original = grid[ i ];
				grid[ i ] = "";

				foreach( char c in original.ToCharArray() ) {
					grid[ i ] = c + grid[ i ];
				}
			}

			return String.Join( "/", grid );
		}

		private string FlipY( string pattern ) {
			string[] grid = pattern.Split( '/' );
			string temp = grid[ 0 ];
			grid[ 0 ] = grid[ grid.Length - 1 ];
			grid[ grid.Length - 1 ] = temp;

			return String.Join( "/", grid );
		}

		private string RotateClockwise( string pattern ) {
			string[] grid = pattern.Split( '/' );
			char[,] rotatedGrid = new char[ size, size ];

			for( int i = 0; i < grid.Length; i++ ) {
				for( int j = 0; j < grid[ i ].Length; j++ ) {
					rotatedGrid[ i, j ] = grid[ size - j - 1 ][ i ];
				}
			}

			string[] rotatedStringGrid = new string[ size ];
			for( int i = 0; i < size; i++ ) {
				rotatedStringGrid[ i ] = "";
				for( int j = 0; j < size; j++ ) {
					rotatedStringGrid[ i ] += rotatedGrid[ i, j ];
				}
			}

			return String.Join( "/", rotatedStringGrid );
		}
	}

	class Day21 : Puzzle {
		private List<Rule> rules;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testRules = @"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#";

			testCases.Add( new TestCase( testRules, "12", 1 ) );
		}

		private List<Rule> ParseInput( string input ) {
			Regex rulesRegex = new Regex( @"(.+) => (.+)" );
			rules = new List<Rule>();

			string[] inputArray = input.Split( '\n' );
			foreach( string entry in inputArray ) {
				Match match = rulesRegex.Match( entry );
				rules.Add( new Rule( match.Groups[ 1 ].Value, match.Groups[ 2 ].Value ) );
			}

			return rules;
		}

		public override string Solve( string input, int part ) {
			List<Rule> rules = ParseInput( input );
			string currentPattern = ".#./..#/###";
			
			for( int i = 0; i < 1; i++ ) {
				currentPattern = Iterate( currentPattern );
			}

			//DEBUG
			Console.WriteLine();
			Console.WriteLine( "FINAL STATE ");
			Console.WriteLine( currentPattern );

			return "" + GetOnPixelCount( currentPattern );

			return String.Format( "Day 21 part {0} solver not found.", part );
		}

		private string Iterate( string pattern ) {
			string[] grid = pattern.Split( '/' );

			if( grid.Length % 2 == 0 ) {
				return String.Join( "/", IterateAxA( grid, 2 ) );
			}

			return String.Join( "/", IterateAxA( grid, 3 ) );
		}

		private string[] IterateAxA( string[] grid, int size ) {
			int subGridSize = grid.Length / size;
			int subGridCount = subGridSize * subGridSize;
			List<char[,]> subGrids = new List<char[,]>();

			for( int i = 0; i < subGridCount; i++ ) {
				char[,] subGrid = new char[ size, size ];

				for( int j = 0; j < size; j++ ) {
					for( int k = 0; k < size; k++ ) {
						int subGridX = i / size;
						int subGridY = i % size;

						subGrid[ j, k ] = grid[ subGridX * size + j ][ subGridY * size + k ];
					}
				}

				subGrids.Add( subGrid );
			}

			List<string> subGridReplacements = new List<string>();
			foreach( char[,] subGrid in subGrids ) {
				string subGridString = "";
				for( int i = 0; i < size; i++ ) {
					for( int j = 0; j < size; j++ ) {
						subGridString += subGrid[ i, j ];
						if( j == size - 1 && i != size - 1 ) {
							subGridString += "/";
						}
					}
				}

				string newSubGridString = null;
				foreach( Rule rule in rules ) {
					newSubGridString = rule.CheckMatch( subGridString );

					if( newSubGridString != null ) {
						subGridReplacements.Add( newSubGridString );
						break;
					}
				}
			}
			
			//Assemble subgrids into final grid
			string[] result = new string[ subGridSize * subGridReplacements[ 0 ].Length ];
			
			for( int i = 0; i < result.Length; i++ ) {
				result[ i ] = "";

				for( int j = 0; j < subGridSize; j++ ) {
					result[ i ] += subGridReplacements[ i * subGridSize + j ];
				}
			}
			/*
			 * .#.
			 * ..#
			 * ###
			 * 
			 * ...
			 * ...
			 * ...
			 * 
			 * ...
			 * ...
			 * ...
			 * 
			 * etc
			 */

			//string[] result = new string[ subGridSize * size ];
			//for( int i = 0; i < subGridSize * size; i++ ) {
			//	result[ i ] = "";

			//	for( int j = 0; j < subGridSize; j++ ) {
			//		result[ i ] += subGridReplacements[ ( i / size ) * size + j / size ][ j % size ];
			//	}
			//}
			
			//DEBUG
			Console.WriteLine( result[ 0 ] );

			return result;
		}

		private int GetOnPixelCount( string pattern ) {
			int pixelCount = 0;
			char[] charArray = pattern.ToCharArray();

			foreach( char c in charArray ) {
				if( c == '#' ) {
					pixelCount++;
				}
			}

			return pixelCount;
		}
	}
}
