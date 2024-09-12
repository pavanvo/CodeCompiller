using System;
using System.Collections.Generic;

namespace ConsoleApp1 {
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
}
