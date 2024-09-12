namespace CodeCompiller {
    interface IInnerBlock : IBlock{
        IBlock Inner { get; set; }

        void InnerBlock(IBlock block);
    }
}
