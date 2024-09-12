namespace CodeCompiller {
    interface IBlock {
        void Start();

        void AddBlock(IBlock block);
        IBlock Next { get; set; }
    }
}
