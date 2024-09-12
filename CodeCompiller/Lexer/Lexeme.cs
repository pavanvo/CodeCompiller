namespace CodeCompiller {
    public class Lexeme {
        public Lexeme() {

        }
        public Lexeme(LexemeType type, int index) {
            Type = type;
            Index = index;
        }

        public LexemeType Type { get; set; } = 0;
        public int Index { get; set; } = -1;

        public override string ToString() {
            return $"{Type}: {Index}";
        }
    }
}
