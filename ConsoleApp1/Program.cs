using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            var scaner = new Scaner();
            var result = scaner.Scan("");
            var kwords = result.GetKwords();
            var delimiters = result.GetDelimiters();
            var literals = result.GetLiterals();
            var IDs = result.GetIDs();
            var lexemes = result.GetLexemes();

            foreach (var item in lexemes) {
                var lexeme = string.Empty;
                switch (item.Type) {
                    case LexemeType.Kword:
                        lexeme = kwords[item.Index];
                        break;
                    case LexemeType.Delimiter:
                        lexeme = delimiters[item.Index];
                        break;
                    case LexemeType.Number:
                        lexeme = literals[item.Index] + "";
                        break;
                    case LexemeType.ID:
                        lexeme = IDs[item.Index];
                        break;
                }
                Console.WriteLine("{0,7} |{1, 2}", lexeme, $"{((int)item.Type) + 1}, {item.Index + 1} ");
            }
            
            Console.ReadKey();
        }
    }
}
