using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day07 {
	struct Program {
		public string name;
		public int weight;
	}

	class Day07 : Puzzle {
		private Dictionary<string, string> childParentDictionary;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string exampleInput = @"pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)";
			testCases.Add( new TestCase( exampleInput, "tknk", 1 ) );
		}

		private List<Program> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<Program> programs = new List<Program>();
			Regex parsingRegex = new Regex( @"(\w+) \((\d+)\)( -> (.*))?" );

			foreach( string entry in inputArray ) {
				Match match = parsingRegex.Match( entry );

				Program program = new Program();
				program.name = match.Groups[ 1 ].Value;
				program.weight = Int32.Parse( match.Groups[ 2 ].Value );
				programs.Add( program );

				if( match.Groups[ 4 ] != null && match.Groups[ 4 ].Length > 0 ) {
					// The problem was here!  Without Trim(), the last element in a child list had "\n" at the end!
					string[] children = match.Groups[ 4 ].Value.Trim().Split( new string[] { ", " }, StringSplitOptions.None );

					foreach( string child in children ) {
						childParentDictionary.Add( child, program.name );
					}
				}
			}

			return programs;
		}

		public override string Solve( string input, int part ) {
			childParentDictionary = new Dictionary<string, string>();
			List<Program> programs = ParseInput( input );
			return FindGreatestParentName( programs );
			
			return String.Format( "Day 07 part {0} solver not found.", part );
		}

		private string FindGreatestParentName( List<Program> programs ) {
			string currentName = programs[ 0 ].name;

			while( childParentDictionary.ContainsKey( currentName ) ) {
				currentName = childParentDictionary[ currentName ];
			}

			return currentName;
		}
	}
}
