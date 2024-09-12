using System.Collections.Generic;

namespace CodeCompiller {
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
}
