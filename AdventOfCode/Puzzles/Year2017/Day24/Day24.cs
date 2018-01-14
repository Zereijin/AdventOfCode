using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day24 {
	class Component {
		private int[] ports;
		private bool[] portAvailabilities;

		public Component( int[] ports ) {
			this.ports = ports;

			portAvailabilities = new bool[ ports.Length ];
			ResetAvailabilities();
		}

		private void ResetAvailabilities() {
			for( int i = 0; i < portAvailabilities.Length; i++ ) {
				portAvailabilities[ i ] = true;
			}
		}

		/***
		 * Returns the index of the first connectable port, or -1 if no connection is possible
		 */
		public int FindConnection( int requiredPins ) {
			for( int i = 0; i < ports.Length; i++ ) {
				if( portAvailabilities[ i ] ) {
					if( ports[ i ] == requiredPins ) {
						return i;
					}
				}
			}

			return -1;
		}

		public bool Connect( int requiredPins ) {
			int port = FindConnection( requiredPins );

			if( port >= 0 ) {
				portAvailabilities[ port ] = false;
			}

			return ( port >= 0 );
		}

		public void Disconnect( int requiredPins ) {
			for( int i = ports.Length - 1; i >= 0; i-- ) {
				if( !portAvailabilities[ i ] ) {
					if( ports[ i ] == requiredPins ) {
						portAvailabilities[ i ] = true;
						break;
					}
				}
			}
		}

		public int GetStrength() {
			int totalStrength = 0;

			foreach( int port in ports ) {
				totalStrength += port;
			}

			return totalStrength;
		}

		public List<int> GetUnconnectedPorts() {			
			List<int> unconnectedPorts = new List<int>();

			for( int i = 0; i < ports.Length; i++ ) {
				if( portAvailabilities[ i ] ) {
					unconnectedPorts.Add( ports[ i ] );
				}
			}

			return unconnectedPorts;
		}

		public string ToString() {
			return "" + ports[ 0 ] + "/" + ports[ 1 ];
		}
	}

	class Bridge {
		private List<Component> components;
		private List<int> connectedPorts;
		private int looseEndPins;
		private int strength;

		public Bridge() {
			components = new List<Component>();
			connectedPorts = new List<int>();
			strength = 0;
			UpdateLooseEnd();
		}

		public bool Connect( Component component ) {
			if( components.Contains( component ) ) return false;
			
			if( !component.Connect( looseEndPins ) ) return false;
			
			if( components.Count > 0 ) {
				GetLastComponent().Connect( looseEndPins );
			}
			components.Add( component );

			connectedPorts.Add( looseEndPins );

			strength += component.GetStrength();
			UpdateLooseEnd();

			return true;
		}

		public void DisconnectLast() {
			int removalIndex = components.Count - 1;
			strength -= components[ removalIndex ].GetStrength();
			components.RemoveAt( removalIndex );

			if( components.Count <= 0 ) {
				connectedPorts = new List<int>();
			} else {
				GetLastComponent().Disconnect( connectedPorts[ connectedPorts.Count - 1 ] );
				connectedPorts.RemoveAt( connectedPorts.Count - 1 );
			}

			UpdateLooseEnd();
		}

		private void UpdateLooseEnd() {
			if( components.Count <= 0 ) {
				looseEndPins = 0;
				return;
			}

			List<int> loosePorts = components[ components.Count - 1 ].GetUnconnectedPorts();

			if( loosePorts.Count == 1 ) {
				looseEndPins = loosePorts[ 0 ];
			} else {
				Console.WriteLine( "Connected to " + components[ components.Count - 1 ].ToString() + " with " + loosePorts.Count + " unconnected ports!" );
			}
		}

		public int GetStrength() {
			return strength;
		}

		public int GetLooseEndPins() {
			return looseEndPins;
		}

		public Component GetLastComponent() {
			if( components.Count == 0 ) {
				return null;
			}

			return components[ components.Count - 1 ];
		}

		public void Print() {
			string s = "";
			foreach( Component c in components ) {
				s += c.ToString() + "--";
			}
			Console.WriteLine( s );
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
		}

		private void ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			Regex componentRegex = new Regex( @"(\d+)/(\d+)" );

			components = new List<Component>();

			foreach( string entry in inputArray ) {
				Match match = componentRegex.Match( entry );

				int[] ports = { Int32.Parse( match.Groups[ 1 ].Value ), Int32.Parse( match.Groups[ 2 ].Value ) };
				Component component = new Component( ports );
				components.Add( component );
			}
		}

		public override string Solve( string input, int part ) {
			ParseInput( input );

			return "" + GetStrengthOfStrongestBridge( new Bridge() );

			return String.Format( "Day 24 part {0} solver not found.", part );
		}

		private int GetStrengthOfStrongestBridge( Bridge bridge ) {
			int bestStrength = bridge.GetStrength();
			int looseEnd = bridge.GetLooseEndPins();

			foreach( Component component in components ) {
				if( component != bridge.GetLastComponent() && bridge.Connect( component ) ) {

					//DEBUG
					bridge.Print();

					int subBridgeStrength = GetStrengthOfStrongestBridge( bridge );
					if( subBridgeStrength > bestStrength ) {
						bestStrength = subBridgeStrength;
					}

					bridge.DisconnectLast();
				}
			}
			
			return bestStrength;
		}
	}
}
