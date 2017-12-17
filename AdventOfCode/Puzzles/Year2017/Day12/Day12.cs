using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day12 {
	class Connection {
		public int rootProgramId;
		public List<int> connectedProgramIds;

		public Connection( int id ) {
			rootProgramId = id;
			connectedProgramIds = new List<int>();
		}
	}

	class Day12 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( @"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5", "6", 1 ) );
			testCases.Add( new TestCase( @"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5", "2", 2 ) );
		}

		private Dictionary<int, List<int>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			Dictionary<int, List<int>> connections = new Dictionary<int, List<int>>();
			Regex connectionRegex = new Regex( @"(\d+) <-> (.*)\n?" );

			foreach( string entry in inputArray ) {
				Match match = connectionRegex.Match( entry );
				Connection connection = new Connection( Int32.Parse( match.Groups[ 1 ].Value ) );

				string[] connectionIds = match.Groups[ 2 ].Value.Split( new string[] { ", " }, StringSplitOptions.None );
				foreach( string connectionId in connectionIds ) {
					connection.connectedProgramIds.Add( Int32.Parse( connectionId ) );
				}

				connections.Add( connection.rootProgramId, connection.connectedProgramIds );
			}

			return connections;
		}

		public override string Solve( string input, int part ) {
			Dictionary<int, List<int>> connections = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetProgramsConnectedTo( 0, connections ).Count;
				case 2:
					return "" + GetGroupCount( connections );
			}
			
			return String.Format( "Day 12 part {0} solver not found.", part );
		}

		private List<int> GetProgramsConnectedTo( int id, Dictionary<int, List<int>> connections ) {
			return GetUniqueProgramsConnectedTo( id, connections, new List<int>() );

		}

		private List<int> GetUniqueProgramsConnectedTo( int id, Dictionary<int, List<int>> connections, List<int> existingMatches ) {
			int matchesSize = existingMatches.Count;
			existingMatches = AddIfUnique( id, existingMatches );
			if( matchesSize >= existingMatches.Count ) {
				return existingMatches;
			}

			List<int> currentProgramConnectionIds = connections[ id ];

			foreach( int connection in currentProgramConnectionIds ) {
				existingMatches = GetUniqueProgramsConnectedTo( connection, connections, existingMatches );
			}

			return existingMatches;
		}

		private int GetGroupCount( Dictionary<int, List<int>> connections ) {
			List<int> existingMatches = new List<int>();
			int groupCount = 0;

			for( int i = 0; i < connections.Count; i++ ) {
				if( existingMatches.Contains( i ) ) {
					continue;
				}

				existingMatches.AddRange( GetProgramsConnectedTo( i, connections ) );
				groupCount++;
			}

			return groupCount;
		}

		private List<int> AddIfUnique( int id, List<int> ids ) {
			if( !ids.Contains( id ) ) {
				ids.Add( id );
			}

			return ids;
		}
	}
}
