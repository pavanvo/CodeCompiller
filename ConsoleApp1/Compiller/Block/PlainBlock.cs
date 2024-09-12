using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1 {
    class PlainBlock : IBlock {
        public IBlock Next { get; set; }

        public List<ICode> Code = new List<ICode>();

        string Name;
        IEnumerable<Lexeme> expr;

        public PlainBlock(IEnumerable<Lexeme> lexemes) {
            Name = Global.ids[lexemes.ElementAt(0).Index];
            expr = lexemes.Skip(2).Take(lexemes.Count() - 3);
        }

        void Build() {
            Code.Clear();
            for (int i = 0; i < expr.Count(); i++) {
                var value = new Value(expr.ElementAt(i));
                if (value.Check()) {
                    Code.Add(value);
                }

                var expression = new Expression(expr.ElementAt(i));
                if (expression.Check()) {
                    Code.Add(expression);
                }
            }
        }

        void Assign(int value) {
            Global.variables[Name] = value;
        }

        ICode Execute() {
            do {
                // use math properly
                var index = Code.FindIndex(x => x is Expression && (x as Expression).GetSymbol() == "*");
                if (index < 0) {
                    index = Code.FindIndex(x => x is Expression && (x as Expression).GetSymbol() == "/");
                    if (index < 0) 
                        index = Code.FindIndex(x => x is Expression);
                }
                var left = Code[index - 1];
                var right = Code[index + 1];
                var expr = Code[index] as Expression;

                expr.Prepare(left, right);

               var result = expr.GetResult();

                Code.RemoveRange(index - 1, 3);
                Code.Insert(index - 1, result);

            } while (Code.Count > 1);

            return Code.FirstOrDefault();
        }

        public void Start() {
            Build();
            var exec = Execute();
            Assign(exec.Get());

            Console.WriteLine($"PlainBlock, {Name}: {Global.variables[Name]}");
            Next?.Start();
        }

        public void AddBlock(IBlock block) {
            if (Next != null)
                Next.AddBlock(block);
            else Next = block;
        }

        public static bool readSignature(IEnumerable<Lexeme> test) {
            if (test.ElementAt(0).Type != LexemeType.ID) return false; //variable
            if (test.ElementAt(1).Type != LexemeType.Delimiter || test.ElementAt(1).Index != 3) return false; //Assign
            if (test.Last().Type != LexemeType.Delimiter || test.Last().Index != 0) return false; // ;          

            var expr = test.Skip(2).Take(test.Count() - 3);
            if(!new Value(expr.First()).Check()) return false; // first and last should be a value type
            if(!new Value(expr.Last()).Check()) return false;

            // check if value and expressions in correct order
            var lastvalue = false;
            for (int i = 0; i < expr.Count(); i++) {
                if (!lastvalue && new Value(expr.ElementAt(i)).Check()) { 
                    lastvalue = true;
                    continue;
                }
                if (lastvalue && new Expression(expr.ElementAt(i)).Check()) lastvalue = false;

            }

            return true;
        }
    }
}
