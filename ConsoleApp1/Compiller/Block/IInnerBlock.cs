namespace ConsoleApp1 {
    interface IInnerBlock : IBlock{
        IBlock Inner { get; set; }
    }
}
