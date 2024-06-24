using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    class PlainBlock : IBlock {
        public Action Next { get ; set; }

        public List<Func<int, int, int>> Funcs = new List<Func<int, int, int>>();

        public List<Tuple<int, int>> Staff = new List<Tuple<int, int>>();

        public void Start() {
            //Funcs[0](,);
        }

        
    }
}
