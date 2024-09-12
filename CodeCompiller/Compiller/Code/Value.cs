namespace CodeCompiller {
    public class Value : ICode {
        public Value(Lexeme lexeme) {
            this.lexeme = lexeme;
        }
        Lexeme lexeme;

        public bool Check() {
            switch (lexeme.Type) {
                case LexemeType.Number:
                    return true;
                case LexemeType.ID:
                    return true;
                case LexemeType.Kword:
                    return false;
                case LexemeType.Delimiter:
                    return false;
            }

            return false;
        }

        public int Get() {
            var value = 0;
            switch (lexeme.Type) {
                case LexemeType.Number:
                    value = Global.literals[lexeme.Index];
                    break;
                case LexemeType.ID:
                    Global.variables.TryGetValue(Global.ids[lexeme.Index], out value);
                    break;
            }

            return value;
        }
    }
}
