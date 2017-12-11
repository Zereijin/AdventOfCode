using System;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2017.Day09 {
	class Day09 : Puzzle {
		int trashedCharacters;

		protected override void SetupTestCases() {
			base.SetupTestCases();

			//testCases.Add( new TestCase( "{}", "1", 1 ) );
			//testCases.Add( new TestCase( "{{{}}}", "6", 1 ) );
			//testCases.Add( new TestCase( "{{},{}}", "5", 1 ) );
			//testCases.Add( new TestCase( "{{{},{},{{}}}}", "16", 1 ) );
			//testCases.Add( new TestCase( "{<a>,<a>,<a>,<a>}", "1", 1 ) );
			//testCases.Add( new TestCase( "{{<ab>},{<ab>},{<ab>},{<ab>}}", "9", 1 ) );
			//testCases.Add( new TestCase( "{{<!!>},{<!!>},{<!!>},{<!!>}}", "9", 1 ) );
			//testCases.Add( new TestCase( "{{<a!>},{<a!>},{<a!>},{<ab>}}", "3", 1 ) );
			//testCases.Add( new TestCase( "<>", "0", 2 ) );
			//testCases.Add( new TestCase( "<random characters>", "17", 2 ) );
			//testCases.Add( new TestCase( "<<<<>", "3", 2 ) );
			//testCases.Add( new TestCase( "<{!>}>", "2", 2 ) );
			//testCases.Add( new TestCase( "<!!>", "0", 2 ) );
			//testCases.Add( new TestCase( "<!!!>>", "0", 2 ) );
			//testCases.Add( new TestCase( "<{o\"i!a,<{i<a>", "10", 2 ) );
			testCases.Add( new TestCase( "{{<a!>},{<a!>},{<a!>},{<ab>}}", "17", 2 ) );
		}

		private char[] ParseInput( string input ) {
			return input.ToCharArray();
		}

		public override string Solve( string input, int part ) {
			char[] stream = ParseInput( input );
			trashedCharacters = 0;

			switch( part ) {
				case 1:
					return "" + GetStreamScore( stream, 0 );
				case 2:
					return "" + GetTrashSize( stream );
			}

			return String.Format( "Day 09 part {0} solver not found.", part );
		}

		private int GetTrashSize( char[] stream ) {
			int index = 0;
			int trashSize = 0;
			bool trashMode = false;


			while( index < stream.Length ) {
				switch( stream[ index ] ) {
					case '<':
						if( !trashMode ) {
							trashMode = true;
							trashSize--;
						}
						break;
					case '!':
						if( trashMode ) {
							index += 2;
							continue;
						}
						break;
					case '>':
						trashMode = false;
						break;
					default:
						break;
				}

				
				if( trashMode ) {
					trashSize++;
				}

				index++;
			}
			Console.Write( "\n");
			return trashSize;
		}

		private int GetStreamScore( char[] stream, int value ) {
			if( stream == null || stream.Length == 0 ) {
				return value;
			}

			int index = 0;
			int score = 0;

			while( index < stream.Length ) {
				switch( stream[ index ] ) {
					case '{':
						int level = 0;
						int closeIndex = 0;

						for( int i = index; i < stream.Length; i++ ) {
							switch( stream[ i ] ) {
								case '{':
									level++;
									break;
								case '}':
									level--;
									break;
								case '<':
									char[] tSubStream = new char[ stream.Length - i - 1 ];
									Array.Copy( stream, i, tSubStream, 0, tSubStream.Length );

									i += GetSizeOfTrash( tSubStream );
									break;
							}

							if( level == 0 ) {
								closeIndex = i;
								break;
							}
						}

						char[] subStream = new char[ closeIndex - index - 1 ];
						Array.Copy( stream, index + 1, subStream, 0, subStream.Length );

						score += GetStreamScore( subStream, value + 1 );
						index = closeIndex;
						break;
					case ',':
						break;
					case '<':
						char[] trashSubStream = new char[ stream.Length - index - 1 ];
						Array.Copy( stream, index, trashSubStream, 0, trashSubStream.Length );
						
						index += GetSizeOfTrash( trashSubStream ) + 1;
						break;
				}

				index++;
			}
			
			return score + value;
		}
		
		private int GetSizeOfTrash( char[] stream ) {
			for( int i = 0; i < stream.Length; i++ ) {
				switch( stream[ i ] ) {
					case '!':
						i++;
						break;
					case '>':
						return i - 1;
				}
			}

			return stream.Length;
		}
	}
}
