using System;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode {
	/// <summary>
	/// The Main class for running Advent of Code puzzles.
	/// </summary>
	class Program {
		/// <summary>
		/// The Main function.
		/// </summary>
		/// <remarks>
		/// This program expects 3 args:  The year, day, and part corresponding to a solved Advent of Code puzzle.
		/// </remarks>
		/// <param name="args">The argument list for the program.</param>
		static void Main( string[] args ) {
			// End the program with an error if args were not set up correctly.
			if( args == null || args.Length != 3 ) {
				PromptForKey( "Incorrect args: " + String.Join( " ", args) );
				return;
			}

			// Convert the args input into numbers.
			int year = Int32.Parse( args[ 0 ] );
			int day = Int32.Parse( args[ 1 ] );
			int part = Int32.Parse( args[ 2 ] );

			// Ensure that the input numbers are legal.
			// 2015 is the first year for AoC.  It runs from Dec 1 to Dec 25 each year.
			if( year < 2015 || day < 1 || day > 25 || part < 1 ) {
				PromptForKey( String.Format( "Invalid puzzle: Year {0} Day {1} Part {2}", year, day, part ) );
				return;
			}

			Solve( year, day, part );
		}

		/// <summary>
		/// Get the result of an Advent of Code puzzle.
		/// </summary>
		/// <param name="year">The year of the puzzle to solve.</param>
		/// <param name="day">The day of the puzzle to solve.</param>
		/// <param name="part">The part of the puzzle to solve.</param>
		static void Solve( int year, int day, int part ) {
			Console.Title = ( String.Format( "Advent of Code {0} - Day {1} Part {2}", year, day, part ) );

			Puzzle puzzle = GetPuzzle( year, day );

			// Verify the test cases for the puzzle pass.  If any of them failed, abort the puzzle.
			if( !Test( puzzle ) ) {
				return;
			}
			Console.WriteLine();

			// Convert the input file into a string that the puzzle can read.
			string inputFilename = String.Format( "{0}\\..\\..\\Puzzles\\Year{1}\\Day{2:00}\\input.txt", Directory.GetCurrentDirectory(), year, day );
			string input = File.ReadAllText( inputFilename );

			// Start a stopwatch-- this will time how long the puzzle solver was running for.
			Stopwatch stopwatch = Stopwatch.StartNew();

			string solution = puzzle.Solve( input, part );

			stopwatch.Stop();
			Console.WriteLine( String.Format( "Solver ran for {0:N2} seconds", stopwatch.Elapsed.TotalSeconds ) );
			Console.WriteLine();

			// Now that the puzzle is finished, divest its wisdom unto the user.
			PromptForKey( solution );
		}

		/// <summary>
		/// Factory method for pulling the puzzle solver for the given date.
		/// </summary>
		/// <param name="year">The year of the target Puzzle.</param>
		/// <param name="day">The day of the target Puzzle.</param>
		/// <returns>The Puzzle for the requested date.</returns>
		static Puzzle GetPuzzle( int year, int day ) {
			string typeName = String.Format( "AdventOfCode.Puzzles.Year{0}.Day{1:00}.Day{1:00}", year, day );
			Type type = Type.GetType( typeName );
			return Activator.CreateInstance( type ) as Puzzle;
		}

		/// <summary>
		/// Run the test cases for a Puzzle.
		/// </summary>
		/// <param name="puzzle">The Puzzle to test.</param>
		/// <returns>True if all test cases passed; false otherwise.</returns>
		static bool Test( Puzzle puzzle ) {
			string testOutput = puzzle.RunTestCases();

			if( testOutput == "" ) {
				Console.WriteLine( "Tests all passed." );
				return true;
			}

			PromptForKey( testOutput );
			return false;
		}

		/// <summary>
		/// Print a line to the console, then wait for user input before proceeding.
		/// </summary>
		/// <remarks>
		/// Without introducing this wait command, this program, when run outside of a shell, would display a puzzle solution and then immediately close the window.  We cannot read that quickly.
		/// </remarks>
		/// <param name="statement">The statement to print prior to waiting for user input.</param>
		static void PromptForKey( string statement ) {
			Console.WriteLine( statement );
			Console.ReadKey();
		}
	}
}
