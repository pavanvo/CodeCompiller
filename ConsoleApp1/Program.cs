using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            var scaner = new Scaner();
            var test = scaner.Scan("");
            foreach (var item in test) {
                Console.WriteLine($"{item.Value} {item.Type}");
            }
            
            Console.ReadKey();
        }
    }
}
