using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day24 {
	struct BridgeProperty {
		public int length;
		public int strength;

		public BridgeProperty( int l, int s ) {
			length = l;
			strength = s;
		}
	}

	class Component {
		private int[] ports = new int[ 2 ];

		public Component( int portA, int portB ) {
			ports[ 0 ] = portA;
			ports[ 1 ] = portB;
		}

		public bool HasPort( int port ) {
			foreach( int p in ports ) {
				if( p == port ) {
					return true;
				}
			}

			return false;
		}

		public int GetOppositePort( int port ) {
			if( ports[ 0 ] == port ) {
				return ports[ 1 ];
			}

			return ports[ 0 ];
		}

		public int GetStrength() {
			int strength = 0;

			foreach( int port in ports ) {
				strength += port;
			}

			return strength;
		}

		public override string ToString() {
			return "" + ports[ 0 ] + "/" + ports[ 1 ];
		}
	}

	class Bridge {
		private const int basePort = 0;

		private List<Component> components;
		private List<int> connectors;
		private int strength;
		private int openPort;

		public Bridge() {
			components = new List<Component>();
			connectors = new List<int>();
			strength = basePort;
			UpdateOpenPort();
		}

		private void UpdateOpenPort() {
			if( components.Count <= 0 ) {
				// There are no components, so use the bridge's initial value;
				openPort = basePort;
				return;
			}

			int lastComponentIndex = components.Count - 1;
			openPort = components[ lastComponentIndex ].GetOppositePort( connectors[ lastComponentIndex ] );
		}

		public bool Connect( Component component ) {
			if( !component.HasPort( openPort ) ) return false;

			if( components.Contains( component ) ) return false;

			PerformConnect( component );
			return true;
		}

		private void PerformConnect( Component component ) {
			components.Add( component );
			connectors.Add( openPort );
			strength += component.GetStrength();
			UpdateOpenPort();
		}

		public void DisconnectLast() {
			int lastComponentIndex = components.Count - 1;
			strength -= components[ lastComponentIndex ].GetStrength();
			components.RemoveAt( lastComponentIndex );
			connectors.RemoveAt( lastComponentIndex );
			UpdateOpenPort();
		}

		public int GetStrength() {
			return strength;
		}

		public int GetLength() {
			return components.Count;
		}

		public override string ToString() {
			string s = "";

			foreach( Component c in components ) {
				s += c + "--";
			}

			return s;
		}
	}

	class Day24 : Puzzle {
		List<Component> components;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testSet1 = @"0/3
3/7
7/4";
			string testSet2 = @"0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10";

			testCases.Add( new TestCase( testSet1, "24", 1 ) );
			testCases.Add( new TestCase( testSet2, "31", 1 ) );
			testCases.Add( new TestCase( testSet2, "19", 2 ) );
		}

		private void ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			Regex componentRegex = new Regex( @"(\d+)/(\d+)" );

			components = new List<Component>();

			foreach( string entry in inputArray ) {
				Match match = componentRegex.Match( entry );
				
				Component component = new Component( Int32.Parse( match.Groups[ 1 ].Value ), Int32.Parse( match.Groups[ 2 ].Value ) );
				components.Add( component );
			}
		}

		public override string Solve( string input, int part ) {
			ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetStrengthOfStrongestBridge( new Bridge() );
				case 2:
					return "" + GetStrengthOfStrongestLongestBridge( new Bridge() );

			}

			return String.Format( "Day 24 part {0} solver not found.", part );
		}

		private int GetStrengthOfStrongestBridge( Bridge bridge ) {
			int bestStrength = bridge.GetStrength();

			foreach( Component component in components ) {
				if( bridge.Connect( component ) ) {
					//DEBUG
					//Console.WriteLine( bridge );

					int subBridgeStrength = GetStrengthOfStrongestBridge( bridge );
					if( subBridgeStrength > bestStrength ) {
						bestStrength = subBridgeStrength;
					}

					bridge.DisconnectLast();
				}
			}

			return bestStrength;
		}

		private int GetStrengthOfStrongestLongestBridge( Bridge bridge ) {
			BridgeProperty longestBridgeProperties = GetLongestBridges( bridge );

			return longestBridgeProperties.strength;
		}

		private BridgeProperty GetLongestBridges( Bridge bridge ) {
			//List<Bridge> subBridges = new List<Bridge>();
			//bool wasConnectionMade = false;

			//foreach( Component component in components ) {
			//	if( bridge.Connect( component ) ) {
			//		wasConnectionMade = true;

			//		subBridges.Add( bridge );

			//		bridge.DisconnectLast();
			//	}
			//}

			//if( wasConnectionMade ) {
			//	return subBridges;
			//}

			//List<Bridge> bridges = new List<Bridge>();
			//bridges.Add( bridge );
			//return bridges;

			BridgeProperty bestBridgeProperties = new BridgeProperty( bridge.GetLength(), bridge.GetStrength() );

			foreach( Component component in components ) {
				if( bridge.Connect( component ) ) {
					BridgeProperty newBridgeProperties = GetLongestBridges( bridge );

					if( newBridgeProperties.length > bestBridgeProperties.length
						|| ( newBridgeProperties.length == bestBridgeProperties.length && newBridgeProperties.strength > bestBridgeProperties.strength ) ) {

						bestBridgeProperties = newBridgeProperties;
					}

					bridge.DisconnectLast();
				}
			}

			return bestBridgeProperties;
		}
	}
}
