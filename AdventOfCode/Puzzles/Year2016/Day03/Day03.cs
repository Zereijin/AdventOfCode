using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2016.Day03 {
	/// <summary>
	/// A triangle made of sides a, b, and c.
	/// </summary>
	struct Triangle {
		public int a;
		public int b;
		public int c;
		
		/// <summary>
		/// Create a triangle.
		/// </summary>
		/// <param name="a">Length of the 1st side.</param>
		/// <param name="b">Length of the 2nd side.</param>
		/// <param name="c">Length of the 3rd side.</param>
		public Triangle( int a, int b, int c ) {
			this.a = a;
			this.b = b;
			this.c = c;
		}
	}

	class Day03 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "  5   10   25", "0", 1 ) );
			testCases.Add( new TestCase( "101 301 501\n102 302 502\n103 303 503\n201 401 601\n202 402 602\n203 403 603", "6", 2 ) );
		}

		/// <summary>
		/// Break the string input down into triangles.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input converted into a list of triangles.</returns>
		private List<Triangle> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<Triangle> triangleCandidates = new List<Triangle>();

			string pattern = @"\d+";
			Regex regex = new Regex( pattern );

			foreach( string entry in inputArray ) {
				MatchCollection matches = regex.Matches( entry );
				triangleCandidates.Add(
					new Triangle( Int32.Parse( matches[ 0 ].ToString() ), Int32.Parse( matches[ 1 ].ToString() ), Int32.Parse( matches[ 2 ].ToString() ) ) );
			}
			
			return triangleCandidates;
		}

		/// <summary>
		/// Break the string input down into triangles, with the understanding that input is arranged vertically.
		/// </summary>
		/// <remarks>
		/// Prepare for failure if the input length isn't divisible by 3!
		/// </remarks>
		/// <param name="input">The input string.</param>
		/// <returns>The input converted into a list of triangles.</returns>
		private List<Triangle> ParseInputVertically( string input ) {
			List<Triangle> triangleCandidates = ParseInput( input );
			List<Triangle> trueCandidates = new List<Triangle>();

			for( int i = 0; i < triangleCandidates.Count; i += 3 ) {
				// Rotate triangle input.
				trueCandidates.Add( new Triangle( triangleCandidates[ i ].a, triangleCandidates[ i + 1 ].a, triangleCandidates[ i + 2 ].a ) );
				trueCandidates.Add( new Triangle( triangleCandidates[ i ].b, triangleCandidates[ i + 1 ].b, triangleCandidates[ i + 2 ].b ) );
				trueCandidates.Add( new Triangle( triangleCandidates[ i ].c, triangleCandidates[ i + 1 ].c, triangleCandidates[ i + 2 ].c ) );
			}

			return trueCandidates;
		}

		public override string Solve( string input, int part ) {
			List<Triangle> triangleCandidates = ParseInput( input );
			switch( part ) {
				case 1:
					triangleCandidates = ParseInput( input );
					break;
				case 2:
					triangleCandidates = ParseInputVertically( input );
					break;
				default:
					return "";
			}

			return "" + RemoveInvalidTriangles( triangleCandidates ).Count;
		}

		/// <summary>
		/// Analyze a triangle list for valid triangles.
		/// </summary>
		/// <param name="triangles">The list of triangles to analyze.</param>
		/// <returns>A list of all valid triangles found in the input.</returns>
		private List<Triangle> RemoveInvalidTriangles( List<Triangle> triangles ) {
			List<Triangle> validTriangles = new List<Triangle>();

			foreach( Triangle triangle in triangles ) {
				if( IsValidTriangle( triangle ) ) {
					validTriangles.Add( triangle );
				}
			}
			return validTriangles;
		}

		/// <summary>
		/// Check if a triangle is valid.
		/// </summary>
		/// <param name="triangle">The triangle to check.</param>
		/// <returns>True if the triangle is a legal triangle.  False otherwise.</returns>
		private bool IsValidTriangle( Triangle triangle ) {
			if( triangle.a + triangle.b > triangle.c ) {
				if( triangle.a + triangle.c > triangle.b ) {
					if( triangle.b + triangle.c > triangle.a ) {
						return true;
					}
				}
			}

			return false;
		}
	}
}
