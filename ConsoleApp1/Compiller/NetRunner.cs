using System;
using System.Linq;

namespace ConsoleApp1 {


    class NetRunner {
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

            if (inner) { lastInnerBlock.InnerBlock(block); } else if (lastBlock != null) { lastBlock.AddBlock(block); }

            // this is wrong; IT WORKS
            if (lastInnerBlock != null)
                lastBlock = lastInnerBlock;
            else lastBlock = block;
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
                            if (Global.kwords[lexeme.Index] == "end")
                                inner = false;
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
                            index = endOfBlock;
                            break;
                        }
                }

            }
            firstBlock?.Start();

            return true;
        }
    }
}
