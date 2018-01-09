using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day20 {
	class Day20 : Puzzle {
		class Vector3 {
			public int x;
			public int y;
			public int z;

			public Vector3( int x, int y, int z ) {
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		class Particle {
			public Vector3 p;
			public Vector3 v;
			public Vector3 a;

			public Particle( Vector3 p, Vector3 v, Vector3 a ) {
				this.p = p;
				this.v = v;
				this.a = a;
			}
		}

		protected override void SetupTestCases() {
			base.SetupTestCases();

			string testParticles = @"p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>
p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>";
			string testRemoveParticles= @"p=<-6,0,0>, v=<3,0,0>, a=<0,0,0>
p=<-4,0,0>, v=<2,0,0>, a=<0,0,0>
p=<-2,0,0>, v=<1,0,0>, a=<0,0,0>
p=<3,0,0>, v=<-1,0,0>, a=<0,0,0>";

			testCases.Add( new TestCase( testParticles, "0", 1 ) );
			testCases.Add( new TestCase( testRemoveParticles, "1", 2 ) );
		}

		private List<Particle> ParseInput( string input ) {
			List<Particle> particles = new List<Particle>();
			Regex particleRegex = new Regex( @"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>" );

			string[] inputArray = input.Split( '\n' );
			foreach( string entry in inputArray ) {
				Match match = particleRegex.Match( entry );

				Vector3 p = new Vector3( Int32.Parse( match.Groups[ 1 ].Value ), Int32.Parse( match.Groups[ 2 ].Value ), Int32.Parse( match.Groups[ 3 ].Value ) );
				Vector3 v = new Vector3( Int32.Parse( match.Groups[ 4 ].Value ), Int32.Parse( match.Groups[ 5 ].Value ), Int32.Parse( match.Groups[ 6 ].Value ) );
				Vector3 a = new Vector3( Int32.Parse( match.Groups[ 7 ].Value ), Int32.Parse( match.Groups[ 8 ].Value ), Int32.Parse( match.Groups[ 9 ].Value ) );

				particles.Add( new Particle( p, v, a ) );
			}

			return particles;
		}

		public override string Solve( string input, int part ) {
			List<Particle> particles = ParseInput( input );

			switch( part ) {
				case 1:
					return "" + GetIndexOfClosestToOriginOverInfinity( particles );
				case 2:
					return "" + GetParticleCountAfterCollisions( particles );
			}
			

			return String.Format( "Day 20 part {0} solver not found.", part );
		}

		private int GetIndexOfClosestToOriginOverInfinity( List<Particle> particles ) {
			int lastWinnerIndex = -1;
			int consecutiveWins = 0;
			int targetConsecutiveWins = 1000;

			while( consecutiveWins < targetConsecutiveWins ) {
				for( int i = 0; i < particles.Count; i++ ) {
					particles[ i ] = Update( particles[ i ] );
				}

				int newWinnerIndex = GetIndexOfClosest( particles );
				if( lastWinnerIndex == newWinnerIndex ) {
					consecutiveWins++;
				} else {
					lastWinnerIndex = newWinnerIndex;
					consecutiveWins = 0;
				}
			}

			return lastWinnerIndex;
		}

		private int GetParticleCountAfterCollisions( List<Particle> particles ) {
			int lastWinnerIndex = -1;
			int consecutiveWins = 0;
			int targetConsecutiveWins = 1000;
			Dictionary<string, List<int>> positions;

			while( consecutiveWins < targetConsecutiveWins ) {
				positions = new Dictionary<string, List<int>>();

				for( int i = 0; i < particles.Count; i++ ) {
					if( particles[ i ] == null ) continue;

					Particle updatedParticle = particles[ i ] = Update( particles[ i ] );

					string posKey = updatedParticle.p.x + "," + updatedParticle.p.y + "," + updatedParticle.p.z;
					if( !positions.ContainsKey( posKey ) ) {
						positions[ posKey ] = new List<int>();
					}

					positions[ posKey ].Add( i );
				}

				List<string> keys = new List<string>( positions.Keys );
				foreach( string key in keys ) {
					List<int> particleIndices = positions[ key ];
					if( particleIndices.Count > 1 ) {
						foreach( int index in particleIndices ) {
							// We need to null instead of remove; if we remove, our lastWinnerIndex becomes invalid!
							particles[ index ] = null;
						}
					}
				}

				int newWinnerIndex = GetIndexOfClosest( particles );
				if( lastWinnerIndex == newWinnerIndex ) {
					consecutiveWins++;
				} else {
					lastWinnerIndex = newWinnerIndex;
					consecutiveWins = 0;
				}
			}

			int remainingParticles = 0;
			foreach( Particle particle in particles ) {
				if( particle != null )
					remainingParticles++;
			}

			return remainingParticles;
		}

		private Particle Update( Particle particle ) {
			particle.v.x += particle.a.x;
			particle.v.y += particle.a.y;
			particle.v.z += particle.a.z;
			particle.p.x += particle.v.x;
			particle.p.y += particle.v.y;
			particle.p.z += particle.v.z;

			return particle;
		}

		private int GetIndexOfClosest( List<Particle> particles ) {
			int closestIndex = -1;
			int distance = -1;

			for( int i = 0; i < particles.Count; i++ ) {
				Particle particle = particles[ i ];
				if( particle == null ) continue;

				int particleDistance = Math.Abs( particle.p.x ) + Math.Abs( particle.p.y ) + Math.Abs( particle.p.z );

				if( distance == -1 || particleDistance < distance ) {
					distance = particleDistance;
					closestIndex = i;
				}
			}

			return closestIndex;
		}
	}
}
