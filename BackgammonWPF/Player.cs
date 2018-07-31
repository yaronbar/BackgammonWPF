using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BackgammonWPF
{
    public class Player
    {
        int playerNumber;
        Checker[] checkers;

        public Player(int number)
        {
            playerNumber = number;
            checkers = new Checker[15];
            InitiateCheckers();
        }

        public void InitiateCheckers()
        {
            SolidColorBrush _color = new SolidColorBrush();
            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            SolidColorBrush white = new SolidColorBrush(Colors.FloralWhite);
            if (playerNumber == 1)
                _color = black;
            else
                _color = white;

            for (int i = 0; i < 15; i++)
            {
                checkers[i].color = _color;
            }
            
        }
    }
}
