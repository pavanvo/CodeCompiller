using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    enum State { Start, ID, Number, Delimiter, Error };
    enum LexemeType { Kword, Delimiter, Operator, ID, Number, };

    class Lexeme {
        public Lexeme() {

        }
        public Lexeme(LexemeType type, string value) {
            Type = type;
            Value = value;
        }

        public LexemeType Type { get; set; }
        public string Value { get; set; }
    }


    class Scaner {
        private string fileText = string.Empty;

        private readonly string[] kwords = { "var", "int", "boolean", "begin", "end", "for", "to", "do" };

        private readonly string[] delimiters = { ";", ",", ":", ":=", "+", "*", "/", "<", ">", "<=", ">=" };

        private readonly List<int> litearals = new List<int>(); //for now -int only

        private readonly List<string> IDs = new List<string>();

        private readonly List<Lexeme> lexemes = new List<Lexeme>();

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

        private bool isKword(string id) {
            return kwords.Contains(id);
        }

        private void addToken(Lexeme lexeme) {
            var last = lexemes.LastOrDefault();
            if (last != null && last.Type == lexeme.Type) {
                last.Value = new StringBuilder(last.Value)
                        .Append(lexeme.Value).ToString();

            } else lexemes.Add(lexeme);
        }

        public List<Lexeme> Scan(string filename) {
            init(filename);
            char c = new char();
            State state = State.Start;

            void restart(bool hard = true) {
                if(hard) c = getNext();
                state = State.Start;
            }
            restart();

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
                            var token = new Lexeme {
                                Value = value, 
                                Type = isKword(value) ? LexemeType.Kword: LexemeType.ID 
                            };

                            addToken(token);
                            restart(hard: false);
                            break;
                        }
                    case State.Number: 
                        {
                            var token = new Lexeme(LexemeType.Number, c + "");
                            addToken(token);
                            restart();
                            break;
                        }
                    case State.Delimiter: 
                        {
                            if (c == '(' || c == ')' || c == ';' || 
                                c == ':' || c == '=' || c == '+' || c == '-') {
                                var token = new Lexeme(LexemeType.Delimiter, c + "");
                                addToken(token);
                                restart();
                                //
                            } else if (c == '<' || c == '>'/* || (c == '=')*/) {
                                var token = new Lexeme(LexemeType.Operator, c + "");
                                addToken(token);
                                restart();
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
            return lexemes;
        }
    }
}
