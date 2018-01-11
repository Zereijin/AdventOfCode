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

			//testCases.Add( new TestCase( testRules, "12", 1 ) );
		}

		private List<Rule> ParseInput( string input ) {
			Regex rulesRegex = new Regex( @"(.+) => (.+)" );
			rules = new List<Rule>();

			string[] inputArray = input.Split( '\n' );
			foreach( string entry in inputArray ) {
				Match match = rulesRegex.Match( entry );
				rules.Add( new Rule( match.Groups[ 1 ].Value, match.Groups[ 2 ].Value.TrimEnd() ) );
			}

			return rules;
		}

		public override string Solve( string input, int part ) {
			List<Rule> rules = ParseInput( input );
			string currentPattern = ".#./..#/###";
			int iterations = 0;

			switch( part ) {
				case 1:
					iterations = 5;
					break;
				case 2:
					iterations = 18;
					break;
			}

			for( int i = 0; i < iterations; i++ ) {
				currentPattern = Iterate( currentPattern );
			}

			////DEBUG
			//Console.WriteLine();
			//Console.WriteLine( "FINAL STATE " );
			//Console.WriteLine( currentPattern );

			return "" + GetOnPixelCount( currentPattern );

			return String.Format( "Day 21 part {0} solver not found.", part );
		}

		private string Iterate( string pattern ) {
			string[] grid = StringToGrid( pattern );

			if( grid.Length % 2 == 0 ) {
				return GridToString( IterateAxA( grid, 2 ) );
			}

			return GridToString( IterateAxA( grid, 3 ) );
		}

		private string[] IterateAxA( string[] grid, int size ) {
			int charsPerSubGrid = size * size;
			int subGridCountSqrt = grid.Length / size;

			string[,][] subGrids = BreakIntoSubGrids( grid, subGridCountSqrt, size );
			string[,][] replacedSubGrids = Fractalize( subGrids, subGridCountSqrt );
			string[] replacedGrid = MergeSubGrids( replacedSubGrids, subGridCountSqrt );
			
			return replacedGrid;
		}

		private string[,][] BreakIntoSubGrids( string[] grid, int subGridCountSqrt, int size ) {
			string[,][] subGrids = new string[ subGridCountSqrt, subGridCountSqrt ][];
			int subGridCount = subGridCountSqrt * subGridCountSqrt;

			// Break grid into sub grids
			for( int i = 0; i < subGridCount; i++ ) {
				int subGridX = i % subGridCountSqrt;
				int subGridY = i / subGridCountSqrt;

				string[] subGrid = new string[ size ];
				for( int j = 0; j < size; j++ ) {
					subGrid[ j ] = grid[ j + subGridY * size ].Substring( subGridX * size, size );
				}

				subGrids[ subGridX, subGridY ] = subGrid;
			}

			return subGrids;
		}

		private string[,][] Fractalize( string[,][] subGrids, int subGridCountSqrt ) {
			// Translate sub grids into their replacements
			string[,][] replacedSubGrids = new string[ subGridCountSqrt, subGridCountSqrt ][];
			for( int i = 0; i < subGridCountSqrt; i++ ) {
				for( int j = 0; j < subGridCountSqrt; j++ ) {
					string subGridString = GridToString( subGrids[ i, j ] );

					foreach( Rule rule in rules ) {
						string newSubGridString = rule.CheckMatch( subGridString );
						if( newSubGridString != null ) {
							replacedSubGrids[ i, j ] = StringToGrid( newSubGridString );
							break;
						}
					}
				}
			}

			return replacedSubGrids;
		}

		private string[] MergeSubGrids( string[,][] subGrids, int subGridCountSqrt ) {
			// Construct new grid
			int replacedSubGridSize = subGrids[ 0, 0 ].Length;
			string[] replacedGrid = new string[ subGridCountSqrt * replacedSubGridSize ];

			for( int i = 0; i < replacedGrid.Length; i++ ) {
				replacedGrid[ i ] = "";

				for( int j = 0; j < subGridCountSqrt; j++ ) {
					replacedGrid[ i ] += subGrids[ j, i / replacedSubGridSize ][ i % replacedSubGridSize ];
				}
			}

			return replacedGrid;
		}

		private string GridToString( string[] grid ) {
			return String.Join( "/", grid );
		}

		private string[] StringToGrid( string s ) {
			return s.Split( '/' );
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
