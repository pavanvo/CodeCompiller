using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    class NetRunner {

        static readonly Dictionary<string, Func<int, int, int>> Expressions = new Dictionary<string, Func<int, int, int>> {
                { "+", (left, right) => left + right },
                { "-", (left, right) => left - right },
                { "*", (left, right) => left * right },
                { "/", (left, right) => left / right }
            };

        List<IBlock> blocks = new List<IBlock>();

        public void Run(Result result) {
            var kwords = result.GetKwords();
            var delimiters = result.GetDelimiters();
            var literals = result.GetLiterals();
            var ids = result.GetIDs();
            var lexemes = result.GetLexemes();

            var variables = new Dictionary<string, int>(); // for now - int only

            for (int index = 0; index < lexemes.Length; index++) {
                var lexeme = lexemes[index];

                switch (lexeme.Type) {
                    case LexemeType.Kword:
                        if (kwords[lexeme.Index] == "for") {
                            //verivy loop signature
                            var signature = Iterator.Signature();
                            var part = lexemes.Skip(index).Take(signature.Length);

                            // setup loop
                            var iterator = // bad decision
                                new Iterator(part, literals, ids);
                            // nice
                            iterator.Assign += ((iteration, name) => variables[name] = iteration);
                            blocks.Add(iterator);
                            iterator.Start();
                        }
                        break;
                    case LexemeType.Delimiter:
                        var delimiter = delimiters[lexeme.Index];
                        break;
                    case LexemeType.Number:

                        break;
                    case LexemeType.ID:
                        //variables[ids[lexeme.Index]] = 0;
                        break;
                }

            }

        }



        //IBlock GetBlock() {
        //}
    }
}
