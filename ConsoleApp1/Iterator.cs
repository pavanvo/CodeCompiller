using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
	class Iterator : IBlock {
		private int Current { get; set; }
		private int To { get; set; }
		private string Name { get; set; }


		public Action Inner = () => { Console.WriteLine("Test inner"); };

		public Action Next { get; set; } = () => { };

		public void Start() {
			Iterate();
		}

		public Action<int, string> Assign = (newValue, name) => { };


		public static Lexeme[] Signature() {
			return new Lexeme[] { 
				new Lexeme(LexemeType.Kword, 5), //0
				new Lexeme(LexemeType.ID, 0), //1
				new Lexeme(LexemeType.Delimiter, 3), //2
				new Lexeme(LexemeType.Number, 0), //3
				new Lexeme(LexemeType.Kword, 6), //4
				new Lexeme(LexemeType.Number, 1), //5
				new Lexeme(LexemeType.Kword, 7), //6
			};
		}

		public Iterator(IEnumerable<Lexeme> lexemes, int[] literals, string[] ids) {
			if (readSignature(lexemes)) {
				Name = ids[lexemes.ElementAt(1).Index];
				Current = literals[lexemes.ElementAt(3).Index] -1;
				To = literals[lexemes.ElementAt(5).Index];
			}
		}




		void Iterate() {
			if (Current < To) {
				Current++;

				Assign(Current, Name);
				Inner();
			}

			Next();
		}

		bool readSignature(IEnumerable<Lexeme> test) {
			var checks = new List<bool>();
			var sign = Signature();

			for (int index = 0; index < sign.Length; index++) {
				var lexeme = sign[index];

				switch (lexeme.Type) {
					case LexemeType.Kword:
						if (test.ElementAt(index).Type == LexemeType.Kword) {
							checks.Add(true);
							if (test.ElementAt(index).Index == lexeme.Index) {
								checks.Add(true);
							} else checks.Add(false);
						} else checks.Add(false);
						break;
					case LexemeType.Delimiter:
						if (test.ElementAt(index).Type == LexemeType.Delimiter) {
							if (test.ElementAt(index).Index == lexeme.Index) {
								checks.Add(true);
							} else checks.Add(false);
							checks.Add(true);
						} else checks.Add(false);
						break;
					case LexemeType.Number:
						if (test.ElementAt(index).Type == LexemeType.Number) {
							checks.Add(true);
						} else checks.Add(false);
						break;
					case LexemeType.ID:
						if (test.ElementAt(index).Type == LexemeType.ID) {
							checks.Add(true);
						} else checks.Add(false);
						break;
				}
			}

			return checks.TrueForAll((x) => x);
		}
	}
}
