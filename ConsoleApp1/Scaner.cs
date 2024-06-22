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

        private readonly string[] kwords = { "for", "to", "do" };

        private readonly List<Lexeme> lexemes = new List<Lexeme>();

        private int index { get; set; } = 0;

        private void init(string filename) {
            //fileText = File.ReadAllText(filename);
            fileText = "for i:=1 to 10 do x:=x+1;";
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
            if (last != null) {
                if (last.Type == lexeme.Type) {
                    last.Value = new StringBuilder(last.Value)
                        .Append(lexeme.Value).ToString();
                }
            } else lexemes.Add(lexeme);
        }

        public List<Lexeme> Scan(string filename) {
            init(filename);

            State state = State.Start;
            while (read()) {

                var c = getNext();

                switch (state) {
                    case State.Start: 
                        {
                            // spaces
                            while ((c == ' ') || (c == '\t') || (c == '\n')) {
                                c = getNext();
                            }
                            // Identificators
                            if (((c >= 'A') && (c <= 'Z')) ||
                                ((c >= 'a') && (c <= 'z')) || (c == '_')) {
                                state = State.ID;
                            }// numbers
                            else if (((c >= '0') && (c <= '9')) || (c == '.') ||
                                         (c == '+') || (c == '-')) {
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
                            while (((c >= 'A') && (c <= 'Z')) || ((c >= 'a') &&
                                 (c <= 'z')) || ((c >= '0') && (c <= '9')) ||
                                 (c == '_')) {
                                sb.Append(c);

                            }

                            var token = new Lexeme();
                            if (isKword(sb.ToString()))
                                token.Type = LexemeType.Kword;
                            else
                                token.Type = LexemeType.ID;

                            addToken(token);
                            state = State.Start;
                            break;
                        }
                    case State.Number:

                        break;
                    case State.Delimiter: 
                        {
                            if ((c == '(') || (c == ')') || (c == ';')) {
                                var token = new Lexeme(LexemeType.Delimiter, c + "");
                                addToken(token);
                                state = State.Start;
                                //
                            } else if ((c == '<') || (c == '>')/* || (c == '=')*/) {
                                var token = new Lexeme(LexemeType.Operator, c + "");
                                addToken(token);
                                state = State.Start;
                            } else {
                                state = State.Error;
                            }// if((c == '(') || (c == ')') || (c == ';'))
                            break;
                        }
                    case State.Error: {
                            Console.WriteLine($"Error: Index = {index}");
                            break;
                        }
                };
            }
            return lexemes;
        }
    }
}
