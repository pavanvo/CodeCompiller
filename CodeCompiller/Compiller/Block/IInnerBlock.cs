namespace ConsoleApp1 {
    interface IInnerBlock : IBlock{
        IBlock Inner { get; set; }

        void InnerBlock(IBlock block);
    }
}
