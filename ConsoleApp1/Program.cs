using System;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            var scaner = new Scaner();
            var result = scaner.Scan("program.txt");

            var netRunner = new NetRunner(result);
            netRunner.Run();

            Console.ReadKey();
        }
    }
}
