namespace ConsoleApp1 {
    interface IBlock {
        void Start();

        void AddBlock(IBlock block);
        IBlock Next { get; set; }
    }
}
