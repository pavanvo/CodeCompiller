using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1 {
    class Iterator : IInnerBlock {
        private int Current { get; set; }
        private int To { get; set; }
        private string Name { get; set; }


        public IBlock Inner { get; set; }

        public IBlock Next { get; set; }

        public void Start() {
            Iterate();
        }

        void Assign(int value) {
            Global.variables[Name] = value;
        }


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

        public Iterator(IEnumerable<Lexeme> lexemes) {
            Name = Global.ids[lexemes.ElementAt(1).Index];
            Current = Global.literals[lexemes.ElementAt(3).Index] - 1;
            To = Global.literals[lexemes.ElementAt(5).Index];
        }

        void Iterate() {
            while (Current < To) {
                Current++;

                Assign(Current);
                Console.WriteLine($"Iterator, {Name}: {Global.variables[Name]}");
                Inner?.Start();
            }

            Next?.Start();
        }

        public static bool readSignature(IEnumerable<Lexeme> test) {
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

        public void AddBlock(IBlock block) {
            if (Next != null)
                Next.AddBlock(block);
            else Next = block;
        }

        public void InnerBlock(IBlock block) {
            if (Inner != null) {
                if (Inner is IInnerBlock) { (Inner as IInnerBlock).InnerBlock(block); } 
                else Inner.AddBlock(block);
            } else Inner = block;
        }
    }
}
