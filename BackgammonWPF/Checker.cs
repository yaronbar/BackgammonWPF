using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BackgammonWPF
{
    public class Checker
    {
        public SolidColorBrush color { get; set; } // color of the Checker: black/white
        bool isOnBoard { get; set; }               // is the checker on board or not
        Ellipse checkerEllipse;
        //Board board;                               // each Checker holds an instance of the board
                                                   // in order to implement the move routine
        public Checker()
        {
            //board = new Board();
            
        }

        /*public SolidColorBrush getColor()
        {
            return color;
        }

        public void setColor(SolidColorBrush _color)
        {
            color = _color;
        }*/
        
    }


}
