using System;

namespace CodeCompiller {
    class Program {
        static void Main(string[] args) {
            var scaner = new Scaner();
            var result = scaner.Scan("program.txt");

            var netRunner = new NetRunner(result);
            if(!netRunner.Run())
                Console.WriteLine("Compilation failed");

            Console.ReadKey();
        }
    }
}
