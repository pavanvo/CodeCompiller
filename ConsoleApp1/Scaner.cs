using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1 {
    enum State { Start, ID, Number, Delimiter, Error };
    enum LexemeType { Kword, Delimiter, Number, ID, };

    class Scaner {

        private bool end = false;

        private string fileText = string.Empty;

        private int CurrentIndex { get; set; } = -1;

        private void init(string filename) {
            //fileText = File.ReadAllText(filename);
            fileText = "for i:=1 to 10 do x:=x+1;";
        }

        private bool read() {
            return CurrentIndex < fileText.Length && !end;
        }

        private char getNext() {
            if (CurrentIndex == fileText.Length - 1) 
                { end = true; return new char(); }
            return fileText[++CurrentIndex];
        }

        private char getBack() {
            return fileText[--CurrentIndex];
        }



        public Result Scan(string filename) {
            init(filename);
            char c = new char();
            State state = State.Start;

            var result = new Result();

            while (read()) {
                switch (state) {
                    case State.Start: {
                            c = getNext();
                            // spaces
                            while (c == ' ' || c == '\t' || c == '\n') {
                                c = getNext();
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
                    case State.ID: {
                            var sb = new StringBuilder();
                            while ((c >= 'A' && c <= 'Z') ||
                                   (c >= 'a' && c <= 'z') ||
                                   (c >= '0' && c <= '9') ||
                                   c == '_') {
                                sb.Append(c);
                                c = getNext();
                            }

                            c = getBack();

                            var value = sb.ToString();
                            var index = result.isKword(value);
                            var isKword = index >= 0;

                            if (!isKword) index = result.addID(value);

                            var token = new Lexeme {
                                Index = index,
                                Type = isKword ? LexemeType.Kword : LexemeType.ID
                            };

                            result.addToken(token);
                            state = State.Start;
                            break;
                        }
                    case State.Number: {
                            var sb = new StringBuilder();
                            while ((c >= '0' && c <= '9') || c == '.') {
                                sb.Append(c);
                                c = getNext();
                            }

                            c = getBack();

                            var value = sb.ToString();
                            var index = result.addLiteral(int.Parse(value));

                            var token = new Lexeme(LexemeType.Number, index);
                            result.addToken(token);
                            state = State.Start;
                            break;
                        }
                    case State.Delimiter: {
                            var sb = new StringBuilder();
                            while (c == '(' || c == ')' || c == ';' ||
                                c == ':' || c == '=' || c == '+' || c == '-') {
                                sb.Append(c);
                                c = getNext();
                            }

                            c = getBack();

                            var value = sb.ToString();
                            var index = result.isDelimiter(value);

                            if (index >= 0) {
                                var token = new Lexeme(LexemeType.Delimiter, index);
                                result.addToken(token);
                                state = State.Start;
                            } else {
                                state = State.Error;
                            }
                            break;
                        }
                    case State.Error: {
                            Console.WriteLine($"Error: Index = {CurrentIndex}");
                            c = getNext();
                            state = State.Start;
                            break;
                        }
                };
            }
            return result;
        }
    }
}
