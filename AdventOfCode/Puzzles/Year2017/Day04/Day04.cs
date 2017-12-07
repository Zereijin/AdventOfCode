using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2017.Day04 {
	class Day04 : Puzzle {
		protected override void SetupTestCases() {
			base.SetupTestCases();

			testCases.Add( new TestCase( "aa bb cc dd ee", "1", 1 ) );
			testCases.Add( new TestCase( "aa bb cc dd aa", "0", 1 ) );
			testCases.Add( new TestCase( "aa bb cc dd aaa", "1", 1 ) );
			testCases.Add( new TestCase( "abcde fghij", "1", 2 ) );
			testCases.Add( new TestCase( "abcde xyz ecdab", "0", 2 ) );
			testCases.Add( new TestCase( "a ab abc abd abf abj", "1", 2 ) );
			testCases.Add( new TestCase( "iiii oiii ooii oooi oooo", "1", 2 ) );
			testCases.Add( new TestCase( "oiii ioii iioi iiio", "0", 2 ) );
		}

		private List<List<string>> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<List<string>> passphrases = new List<List<string>>();

			foreach( string entry in inputArray ) {
				List<string> passphrase = new List<string>();

				string[] words = entry.Split( ' ' );
				foreach( string word in words ) {
					passphrase.Add( word );
				}

				passphrases.Add( passphrase );
			}

			return passphrases;
		}

		public override string Solve( string input, int part ) {
			List<List<string>> passphrases = ParseInput( input );
			
			switch( part ) {
				case 1:
					return "" + GetNonDuplicateCount( passphrases );
					break;
				case 2:
					return "" + GetNonAnagramCount( passphrases );
					break;
				default:
				return String.Format( "Day 04 part {0} solver not found.", part );
			}

		}

		private int GetNonDuplicateCount( List<List<string>> passphrases ) {
			int validPassphrases = 0;

			foreach( List<string> passphrase in passphrases ) {
				if( !HasDuplicate( passphrase ) ) {
					validPassphrases++;
				}
			}

			return validPassphrases;
		}

		private int GetNonAnagramCount( List<List<string>> passphrases ) {
			int validPassphrases = 0;

			foreach( List<string> passphrase in passphrases ) {
				if( !HasAnagram( passphrase ) ) {
					validPassphrases++;
				}
			}

			return validPassphrases;
		}

		private bool HasDuplicate( List<string> passphrase ) {
			string passString = "";
			foreach( string word in passphrase ) {
				passString += word + " ";
			}

			string pattern = @"(\b\S+\b).+(\b\1\b)";
			Regex regex = new Regex( pattern );

			return regex.IsMatch( passString );
		}

		private bool HasAnagram( List<string> passphrase ) {
			string passString = "";
			foreach( string word in passphrase ) {
				char[] charArray = word.ToCharArray();
				Array.Sort( charArray );

				foreach( char letter in charArray ) {
					passString += letter;
				}
				passString += " ";
			}

			string pattern = @"(\b\S+\b).+(\b\1\b)";
			Regex regex = new Regex( pattern );

			return regex.IsMatch( passString );
		}
	}
}
