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

    class Result {
        public readonly string[] kwords = { "var", "int", "boolean", "begin", "end", "for", "to", "do" };

        public readonly string[] delimiters = { ";", ",", ":", ":=", "+", "*", "/", "<", ">", "<=", ">=" };

        public readonly List<int> litearals = new List<int>(); //for now -int only

        public readonly List<string> IDs = new List<string>();

        public readonly List<Lexeme> lexemes = new List<Lexeme>();

        public bool isKword(string id) {
            return kwords.Contains(id);
        }

        public bool isDelimiter(string delim) {
            return delimiters.Contains(delim);
        }

        public void addToken(Lexeme lexeme) {
            lexemes.Add(lexeme);
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
                            var token = new Lexeme {
                                Value = value, 
                                Type = result.isKword(value) ? LexemeType.Kword: LexemeType.ID 
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
                            var token = new Lexeme(LexemeType.Number, value);
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
                            if (result.isDelimiter(value)) {
                                var token = new Lexeme(LexemeType.Delimiter, value);
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
