using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day07 {
	class Program {
		public string name;
		public int weight;
		public string childrenString;
		public List<Program> children;
		public Program parent;
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
			testCases.Add( new TestCase( exampleInput, "60", 2 ) );
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
				// The problem was here!  Without Trim(), the last element in a child list had "\n" at the end!
				program.childrenString = match.Groups[ 4 ].Value.Trim();
				program.children = new List<Program>();
				programs.Add( program );

				if( match.Groups[ 4 ] != null && match.Groups[ 4 ].Length > 0 ) {
					string[] children = program.childrenString.Split( new string[] { ", " }, StringSplitOptions.None );

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

			switch( part ) {
				case 1:
					return FindBottomProgramName( programs );
				case 2:
					return "" + FindCorrectedWeight( programs );
			}
			
			return String.Format( "Day 07 part {0} solver not found.", part );
		}

		private string FindBottomProgramName( List<Program> programs ) {
			string currentName = programs[ 0 ].name;

			while( childParentDictionary.ContainsKey( currentName ) ) {
				currentName = childParentDictionary[ currentName ];
			}

			return currentName;
		}

		private int FindCorrectedWeight( List<Program> programs ) {
			Program bottomProgram = CreateTree( programs );

			Program faultyProgram = FindFaultyProgram( bottomProgram );
			Program faultyParent = faultyProgram.parent;
			int faultyWeight = GetSubTowerWeight( faultyProgram );

			foreach( Program child in faultyParent.children ) {
				int childWeight = GetSubTowerWeight( child );
				if( childWeight != faultyWeight ) {
					return faultyProgram.weight + childWeight - faultyWeight;
				}
			}
			
			return -1;
		}

		private Program CreateTree( List<Program> programs ) {
			string bottomProgramName = FindBottomProgramName( programs );
			
			return CreateNode( programs, bottomProgramName );
		}

		private Program CreateNode( List<Program> programs, string childName ) {
			Program currentProgram = new Program();

			foreach( Program program in programs ) {
				if( program.name == childName ) {
					currentProgram = program;
					break;
				}
			}

			if( currentProgram.childrenString != null ) {
				string[] childArray = currentProgram.childrenString.Split( new string[] { ", " }, StringSplitOptions.None );
				foreach( string subChildName in childArray ) {
					Program subChild = CreateNode( programs, subChildName );
					subChild.parent = currentProgram;
					currentProgram.children.Add( subChild );
				}
			}
			
			return currentProgram;
		}

		private Program FindFaultyProgram( Program program ) {
			if( program.children == null || program.children.Count <= 0 ) {
				return null;
			}

			Program faultyProgram;
			foreach( Program child in program.children ) {
				faultyProgram = FindFaultyProgram( child );

				if( faultyProgram != null ) {
					return faultyProgram;
				}
			}

			// BUG:  If the first instance of an imbalance belongs to one of only two children, we can't know which is wrong.
			int firstChildWeight = GetSubTowerWeight( program.children[ 0 ] );
			List<int> firstChildMismatchIndices = new List<int>();
			for( int i = 1; i < program.children.Count; i++ ) {
				if( GetSubTowerWeight( program.children[ i ] ) != firstChildWeight ) {
					firstChildMismatchIndices.Add( i );
				}
			}

			if( firstChildMismatchIndices.Count > 1 ) {
				return program.children[ 0 ];
			} else if( firstChildMismatchIndices.Count == 1 ) {
				return program.children[ firstChildMismatchIndices[ 0 ] ];
			}

			return null;
		}

		private int GetSubTowerWeight( Program program ) {
			int totalWeight = program.weight;

			if( program.children != null ) {
				foreach( Program child in program.children ) {
					totalWeight += GetSubTowerWeight( child );
				}
			}

			return totalWeight;
		}
	}
}
