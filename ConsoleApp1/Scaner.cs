using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    enum State { Start, ID, Number, Delimiter, Error };
    enum LexemeType { Kword, Delimiter, Number, ID,  };

    class Lexeme {
        public Lexeme() {

        }
        public Lexeme(LexemeType type, int index) {
            Type = type;
            Index = index;
        }

        public LexemeType Type { get; set; } = 0;
        public int Index { get; set; } = 0;

        public override string ToString() {
            return $"{Type}: {Index}"; 
        }
    }

    class Result {
        private readonly string[] kwords = { "var", "int", "boolean", "begin", "end", "for", "to", "do" };

        private readonly string[] delimiters = { ";", ",", ":", ":=", "+", "*", "/", "<", ">", "<=", ">=" };

        private readonly List<int> litearals = new List<int>(); //for now -int only

        private readonly List<string> IDs = new List<string>();

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
            return IDs.ToArray();
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
            IDs.Add(id);
            return IDs.IndexOf(id);
        }

        public int addLiteral(int liter) {
            litearals.Add(liter);
            return litearals.IndexOf(liter);
        }
    }


    class Scaner {
        private string fileText = string.Empty;

        private int index { get; set; } = 0;

        private void init(string filename) {
            //fileText = File.ReadAllText(filename);
            fileText = "for i:=1 to 10 do x:=x+1; ";
        }

        private bool read() {
            return index < fileText.Length;
        }

        private char getNext() {
            return fileText[index++];
        }

       

        public Result Scan(string filename) {
            init(filename);
            char c = new char();
            State state = State.Start;

            void restart(bool hard = true) {
                if(hard) c = getNext();
                state = State.Start;
            }
            restart();

            var result = new Result();

            while (read()) {
                switch (state) {
                    case State.Start: 
                        {
                            // spaces
                            while (c == ' ' || c == '\t' || c == '\n') {
                                restart();
                            }
                            // Identificators
                            if ((c >= 'A' && c <= 'Z') ||
                                (c >= 'a' && c <= 'z') || c == '_') {
                                state = State.ID;
                            }// numbers
                            else if ((c >= '0' && c <= '9') || c == '.') {
                                state = State.Number;
                            } else if (c == ':') {
                                state = State.Delimiter;
                            } else {
                                state = State.Delimiter;
                            }
                            break;
                        }
                    case State.ID: 
                        {
                            var sb = new StringBuilder();
                            while ((c >= 'A' && c <= 'Z') || 
                                   (c >= 'a' && c <= 'z') || 
                                   (c >= '0' && c <= '9') || 
                                   c == '_') {
                                sb.Append(c);
                                c = getNext();
                            }

                            var value = sb.ToString();
                            var index = result.isKword(value);
                            var isKword = index > 0;

                            if (!isKword) index = result.addID(value);

                            var token = new Lexeme {
                                Index = index, 
                                Type = isKword ? LexemeType.Kword : LexemeType.ID
                            };

                            result.addToken(token);
                            restart(hard: false);
                            break;
                        }
                    case State.Number: 
                        {
                            var sb = new StringBuilder();
                            while ((c >= '0' && c <= '9') || c == '.') {
                                sb.Append(c);
                                c = getNext();
                            }

                            var value = sb.ToString();
                            var index = result.addLiteral(int.Parse(value));

                            var token = new Lexeme(LexemeType.Number, index);
                            result.addToken(token);
                            restart(hard: false);
                            break;
                        }
                    case State.Delimiter: 
                        {
                            var sb = new StringBuilder();
                            while (c == '(' || c == ')' || c == ';' || 
                                c == ':' || c == '=' || c == '+' || c == '-') {
                                sb.Append(c);
                                c = getNext();
                            }

                            var value = sb.ToString();
                            var index = result.isDelimiter(value);

                            if (index > 0) {
                                var token = new Lexeme(LexemeType.Delimiter, index);
                                result.addToken(token);
                                restart(hard: false);
                            } else {
                                state = State.Error;
                            }
                            break;
                        }
                    case State.Error: {
                            Console.WriteLine($"Error: Index = {index}");
                            restart();
                            break;
                        }
                };
            }
            return result;
        }
    }
}
