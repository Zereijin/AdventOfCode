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

		/// <summary>
		/// Break input down into a list of Passphrases.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The input string converted into a list of Passphrases.</returns>
		private List<Passphrase> ParseInput( string input ) {
			string[] inputArray = input.Split( '\n' );
			List<Passphrase> passphrases = new List<Passphrase>();

			foreach( string entry in inputArray ) {
				Passphrase passphrase = new Passphrase();

				string[] words = entry.Split( ' ' );
				foreach( string word in words ) {
					passphrase.AddWord( word );
				}

				passphrases.Add( passphrase );
			}

			return passphrases;
		}

		public override string Solve( string input, int part ) {
			List<Passphrase> passphrases = ParseInput( input );
			
			switch( part ) {
				case 1:
					return "" + GetValidPassphrases( PassphraseHasNoDuplicateWords, passphrases ).Count;
				case 2:
					return "" + GetValidPassphrases( PassphraseHasNoDuplicateAnagrams, passphrases ).Count;
			}

			return String.Format( "Day 04 part {0} solver not found.", part );
		}

		/// <summary>
		/// Get a list of valid passphrases.
		/// </summary>
		/// <param name="validCondition">The function to determine passphrase validity.</param>
		/// <param name="passphrases">The list of unvalidated passphrases to search.</param>
		/// <returns></returns>
		private List<Passphrase> GetValidPassphrases( Func<Passphrase, bool> validCondition, List<Passphrase> passphrases ) {
			List<Passphrase> validPassphrases = new List<Passphrase>();

			foreach( Passphrase passphrase in passphrases ) {
				if( validCondition( passphrase ) ) {
					validPassphrases.Add( passphrase );
				}
			}

			return validPassphrases;
		}

		/// <summary>
		/// Check if a passphrase is free of duplicate words.
		/// </summary>
		/// <param name="passphrase">The passphrase to check.</param>
		/// <returns>True if the passphrase has no duplicate words; false otherwise.</returns>
		private bool PassphraseHasNoDuplicateWords( Passphrase passphrase ) {
			return !passphrase.DetectDuplicateWords();
		}
		
		/// <summary>
		/// Check if a passphrase is free of duplicate anagrams.
		/// </summary>
		/// <param name="passphrase">The passphrase to check.</param>
		/// <returns>True if the passphrase has no duplicate anagrams; false otherwise.</returns>
		private bool PassphraseHasNoDuplicateAnagrams( Passphrase passphrase ) {
			return !passphrase.DetectDuplicateAnagrams();
		}
	}
	
	class Passphrase {
		private List<string> words;
		private readonly Regex duplicateWordRegex = new Regex( @"(\b\S+\b).+(\b\1\b)" );

		public Passphrase() {
			words = new List<string>();
		}

		/// <summary>
		/// Add a word to the passphrase.
		/// </summary>
		/// <param name="word">The word to add to the passphrase.</param>
		public void AddWord( string word ) {
			words.Add( word );
		}

		/// <summary>
		/// Search the passphrase for any duplicated words.
		/// </summary>
		/// <returns>True if a duplicate word is detected; false otherwise.</returns>
		public bool DetectDuplicateWords() {
			return duplicateWordRegex.IsMatch( ToString() );
		}
		

		/// <summary>
		/// Search the passphrase for any duplicated anagrams.
		/// </summary>
		/// <returns>True if a duplicate anagram is detected; false otherwise.</returns>
		public bool DetectDuplicateAnagrams() {
			if( words.Count < 2 ) {
				return false;
			}

			string passString = "";
			for( int i = 0; i < words.Count; i++ ) {
				char[] charArray = words[ i ].ToCharArray();
				Array.Sort( charArray );

				foreach( char letter in charArray ) {
					passString += letter;
				}

				if( i < words.Count - 1 ) {
					passString += " ";
				}
			}

			return duplicateWordRegex.IsMatch( passString );
		}

		public override string ToString() {
			if( words.Count <= 0 ) {
				return "";
			}

			string passString = words[ 0 ];
			for( int i = 1; i < words.Count; i++ ) {
				passString += " " + words[ i ];
			}

			return passString;
		}
	}
}
