namespace ConsoleApp1 {
    interface IBlock {
        void Start();

        IBlock Next { get; set; }
    }
}
