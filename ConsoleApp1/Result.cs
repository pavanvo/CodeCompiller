using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    class Result {
        private readonly string[] kwords = { "var", "int", "boolean", "begin", "end", "for", "to", "do" };

        private readonly string[] delimiters = { ";", ",", ":", ":=", "+", "*", "/", "<", ">", "<=", ">=" };

        private readonly List<int> litearals = new List<int>(); //for now -int only

        private readonly List<string> ids = new List<string>();

        private readonly List<Lexeme> lexemes = new List<Lexeme>();

        public int isKword(string id) {
            return Array.IndexOf(kwords, id);
        }

        public int isDelimiter(string delim) {
            return Array.IndexOf(delimiters, delim);
        }

        public Lexeme[] GetLexemes() {
            return lexemes.ToArray();
        }

        public int[] GetLiterals() {
            return litearals.ToArray();
        }

        public string[] GetIDs() {
            return ids.ToArray();
        }

        public string[] GetKwords() {
            return kwords.ToArray();
        }

        public string[] GetDelimiters() {
            return delimiters.ToArray();
        }

        public void addToken(Lexeme lexeme) {
            lexemes.Add(lexeme);
        }

        // its not well optimized
        public int addID(string id) {
            ids.Add(id);
            return ids.IndexOf(id);
        }

        public int addLiteral(int liter) {
            litearals.Add(liter);
            return litearals.IndexOf(liter);
        }
    }
}
