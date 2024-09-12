namespace ConsoleApp1 {
    public class BurnExpression : ICode {
        public BurnExpression(int value) {
            this.value = value;
        }
        int value;
        public bool Check() {
            return true;
        }

        public int Get() {
            return value;
        }
    }
}
