using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {

    public class Value : ICode {
        public Value(Lexeme lexeme) {
            this.lexeme = lexeme;
        }
        Lexeme lexeme;

        public bool Check() {
            switch (lexeme.Type) {
                case LexemeType.Number:
                    return true;
                case LexemeType.ID:
                    return true;
                case LexemeType.Kword:
                    return false;
                case LexemeType.Delimiter:
                    return false;
            }

            return false;
        }

        public int Get() {
            var value = 0;
            switch (lexeme.Type) {
                case LexemeType.Number:
                    value = Global.literals[lexeme.Index];
                    break;
                case LexemeType.ID:
                    Global.variables.TryGetValue(Global.ids[lexeme.Index], out value);
                    break;
            }

            return value;
        }
    }

    public class BurnExpression : ICode {
        public BurnExpression(int value) {
            this.value = value;
        }
        int value;
        public bool Check() {
           return true;
        }

        public int Get() {
            return value;
        }
    }

    public class Expression : ICode {

        public Expression(Lexeme lexeme) {
            this.lexeme = lexeme;
        }

        Lexeme lexeme;
        ICode left;
        ICode right;

        static readonly Dictionary<string, Func<int, int, int>> expressions = new Dictionary<string, Func<int, int, int>> {
                { "+", (left, right) => left + right },
                { "-", (left, right) => left - right },
                { "*", (left, right) => left * right },
                { "/", (left, right) => left / right }
            };
        public bool Check() {
            if (lexeme.Type == LexemeType.Delimiter) {
                var delimiter = GetSymbol();
                if (expressions.ContainsKey(delimiter))
                    return true;
            }
            return false;
        }

        public string GetSymbol() {
            return Global.delimiters[lexeme.Index];
        }

        public void Prepare(ICode left, ICode right) {
            this.left = left;
            this.right = right;
        }

        public int Get() {
            var func = expressions[GetSymbol()];
            return func(left.Get(), right.Get());
        }

        public BurnExpression GetResult() {
            return new BurnExpression(Get());
        }
    }

    public static class Global {

        public static string[] kwords { get; private set; } = { "var", "int", "boolean", "begin", "end", "for", "to", "do" };

        public static string[] delimiters { get; private set; } = { ";", ",", ":", ":=", "+", "*", "/", "<", ">", "<=", ">=" };

        public static int[] literals { get; private set; }  //for now -int only

        public static string[] ids { get; private set; }

        public static Lexeme[] lexemes { get; private set; }

        public static Dictionary<string, int> variables { get; private set; } // for now - int only

        public static void Init(Result result) {
            literals = result.GetLiterals();
            ids = result.GetIDs();
            lexemes = result.GetLexemes();
            variables = new Dictionary<string, int>();
        }
    }
    class NetRunner {

        Dictionary<string, int> variables = new Dictionary<string, int>(); // for now - int only

        public NetRunner(Result result) {
            Global.Init(result);
        }

        IBlock firstBlock; // only to start
        IBlock lastBlock;
        IInnerBlock lastInnerBlock;
        bool inner = false;
        // No IDea how-to properly handle this hierarhy
        void AppendBlock(IBlock block) {
            if (firstBlock == null) firstBlock = block;
            
            if (block is IInnerBlock) {
                lastInnerBlock = block as IInnerBlock;
            }

            if (inner) { lastInnerBlock.Inner = block; }


             lastBlock = block;
        }

        public bool Run() {
            for (int index = 0; index < Global.lexemes.Length; index++) {
                var lexeme = Global.lexemes[index];

                switch (lexeme.Type) {
                    case LexemeType.Kword: {
                            if (Global.kwords[lexeme.Index] == "for") {
                                //verivy loop signature
                                var signature = Iterator.Signature();
                                var part = Global.lexemes.Skip(index).Take(signature.Length);
                                // exit if wrong
                                if (!Iterator.readSignature(part)) return false;
                                // setup loop
                                var iterator = new Iterator(part);
                                AppendBlock(iterator); inner = true;
                                index += signature.Length - 1;
                            }
                            break;
                        }
                    case LexemeType.Delimiter:
                        break;
                    case LexemeType.Number:
                        break;
                    case LexemeType.ID: {
                            var endOfBlock = Array.FindIndex(Global.lexemes, index,
                                x => x.Type == LexemeType.Delimiter && x.Index == 0); // ;
                            var part = Global.lexemes.Skip(index).Take(endOfBlock - index + 1);

                            if (!PlainBlock.readSignature(part)) return false;
                            var plainBlock = new PlainBlock(part);
                            AppendBlock(plainBlock);
                            index += endOfBlock;
                            break;
                        }
                }

            }
            firstBlock?.Start();

            return true;
        }
    }
}
