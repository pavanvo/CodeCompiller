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

            bool tryGetValue(int i, out int value) {
                value = 0;
                var lexeme = lexemes[i];

                switch (lexeme.Type) {
                    case LexemeType.Number:
                        value = literals[lexeme.Index];
                        return true;
                    case LexemeType.ID:
                        if (variables.TryGetValue(ids[lexeme.Index], out value)) {
                            return true;
                        } else return true;
                    case LexemeType.Kword:
                        return false;
                    case LexemeType.Delimiter:
                        return false;

                }

                return false;
            }


            for (int index = 0; index < lexemes.Length; index++) {
                var lexeme = lexemes[index];

                // by default we use PlainBlocks, only kwords can init InnerBlock
                var plainBlock = new PlainBlock();

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
                            index += signature.Length;
                            //iterator.Start();
                        }
                        break;
                    case LexemeType.Delimiter:
                        var delimiter = delimiters[lexeme.Index];
                        if (Expressions.TryGetValue(delimiter, out Func<int, int, int> func)) {
                            if (tryGetValue(index - 1, out int left) &&
                                tryGetValue(index + 1, out int right)) {

                                var test = func(left, right);
                            }


                        }

                        //plainBlock.Funcs.Add(Expressions[delimiter]);
                        break;
                    case LexemeType.Number:
                        break;
                    case LexemeType.ID:
                        break;
                }

            }
            blocks.FirstOrDefault()?.Start();
        }

        //IBlock GetBlock() {
        //}
    }

}
