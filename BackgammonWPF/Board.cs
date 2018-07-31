using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace BackgammonWPF
{
    class Board
    {
        public Ellipse[,] allCheckers;

        public Board()
        {
            allCheckers = new Ellipse[24, 8];
        }
    }
}
