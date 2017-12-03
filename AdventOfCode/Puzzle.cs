using System;
using System.Collections.Generic;

namespace AdventOfCode {
	/// <summary>
	/// A single test case for a puzzle.
	/// </summary>
	struct TestCase {

		public string input;
		public string expected;
		public int part;

		/// <summary>
		/// Creates a test case.
		/// </summary>
		/// <param name="input">The puzzle input being tested.</param>
		/// <param name="expected">The expected result of the test.</param>
		/// <param name="part">The puzzle part to test.</param>
		public TestCase( string input, string expected, int part ) {
			this.input = input;
			this.expected = expected;
			this.part = part;
		}
	}

	/// <summary>
	/// A solver for an Advent of Code daily puzzle.
	/// </summary>
	abstract class Puzzle {
		protected List<TestCase> testCases;

		/// <summary>
		/// Produce a solution for the puzzle.
		/// </summary>
		/// <param name="input">The input (in string form) for the puzzle.</param>
		/// <param name="part">The puzzle part being solved.</param>
		/// <returns></returns>
		public abstract string Solve( string input, int part );

		/// <summary>
		/// Load up the puzzle's test cases.
		/// </summary>
		/// <remarks>
		/// This function initializes testCases (a list of TestCases).  When overriding, call the base constructor, then add TestCases to testCases to have them run in RunTestCases().
		/// </remarks>
		protected virtual void SetupTestCases() {
			testCases = new List<TestCase>();
		}

		/// <summary>
		/// Run through all loaded TestCases.
		/// </summary>
		/// <returns>
		/// An empty string if all tests passed; a debug string for all failed tests otherwise.
		/// </returns>
		public virtual string RunTestCases() {
			SetupTestCases();

			string results = "";
			
			foreach( TestCase test in testCases ) {
				string output = Solve( test.input, test.part );

				if( output != test.expected ) {
					results += String.Format( "[{0}] failed in Part {1}.\nExpected: \"{2}\"\nGot:     \"{3}\"\n\n", test.input, test.part, test.expected, output );
				}
			}

			return results;
		}
	}
}
