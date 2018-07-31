using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackgammonWPF
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        StartingWindow startwindow = new StartingWindow();
        BitmapImage logo = new BitmapImage();
        ImageBrush mainWindownImage = new ImageBrush();
        ToolTip tip = new ToolTip();
        Ellipse[,] allCheckers;
        Board board = new Board();
        
        SolidColorBrush black = new SolidColorBrush(Colors.Black);
        SolidColorBrush white = new SolidColorBrush(Colors.FloralWhite);
        SolidColorBrush gold = new SolidColorBrush(Colors.Gold);
        
        BitmapImage[] dices = new BitmapImage[6];

        int dice1;
        int dice2;
        int Player1Score;           // if each of the players gets to 15 - he wins
        int Player2Score;

        // variables for running game:
        bool timeToDice; // when true - a player can the throw the dices
        int turn = 1;                   // stands for the turn of whice player is it now:
                                    // 0 - no one, 1 - player 1, 2 - player 2
        int lastPlayerToPlay = 2;         // player 1 will start the game


        // when showing possible moves:
                                               // initiate with -1 for begining
        int lastChosenCheckerStack = -1;       // the checker's stack we chose to see it's possible moves
        int lastChosenCheckerNumber = -1;      // the checker's number we chose o see it's possible moves

        // for each throw of dices:

        bool isItPossibleToUseDice1;           // if dice1 creates any possible move for the player
        bool isItPossibleToUseDice2;           // if dice2 creates any possible move for the player

        // indicates whether possible moves for dice1 and dice2 are shown

        bool Dice1PossibleMovesShown;
        bool Dice2PossibleMovesShown;


        // indicates the following:
        
        bool AnyOptionsShown = false;          // indicates if theres a possible move drawn on board
                                               // we use it to know if we need to erase the possiblites
                                               // in case the user clicked on the same checker twice

        // each player has at most 2 moves each round
        bool isDice1AlreadyUsed = false;       // if the player already played with dice1
        bool isDice2AlreadyUsed = false;       // if the player already played with dice2

        // indicates how many white and black eaten checkers are
        int whitesEaten = 0;
        int blacksEaten = 0;

        // indicates if we had first click on a eaten checker
        bool isHandleingBlackEaten = false;
        bool isHandleingWhiteEaten = false;

        bool isDoubleMode = false;
        int doubleCount = 0;

        bool isItPossibleToUseDice1ToRevive = false;
        bool isItPossibleToUseDice2ToRevive = false;



        public MainWindow()
        {
            
            InitializeComponent();
            InitiateBoard();
            InitiateObjects();
            InitiateImages();
            HideAllObjects();
            Run();
         

        }



        public void Run()
        {
            this.Hide();
            startwindow.ShowDialog();
            this.ShowDialog();
            
        }

        public void InitiateBoard()
        {
            mainWindownImage.ImageSource = new BitmapImage(new Uri("https://cdn.pixabay.com/photo/2012/12/24/08/39/backdrop-72250_960_720.jpg"));
            this.Width += 50;
            this.Height += 20;
            this.Background = mainWindownImage;
            
            startGame.IsEnabled = true;
            turn = 0;
            Player1ScoreTextBox.Visibility = Visibility.Hidden;
            Player2ScoreTextBox.Visibility = Visibility.Hidden;
            ScoreTextBlock.Visibility = Visibility.Hidden;

            BitmapImage forfireworks = new BitmapImage();
            forfireworks.BeginInit();
            forfireworks.UriSource = new Uri("https://thumbs.dreamstime.com/b/you-win-slogan-golden-text-vivid-stars-d-render-86513922.jpg");
            forfireworks.EndInit();
            fireworks.Source = forfireworks;
            fireworks.Visibility = Visibility.Hidden;
            startBoard();
        }



        private void InitiateObjects()
        {
            int i;
            timeToDice = true;
            for(i = 0; i < 6; i++)
            {
                dices[i] = new BitmapImage();
            }


        }

        private void setDiceSource(int numberOfDice)
        {
            switch (numberOfDice)
            {
                case 0:
                    dices[0].UriSource = new Uri("http://www.clker.com/cliparts/m/v/m/J/4/V/dice-1-md.png");
                    break;
                case 1:
                    dices[1].UriSource = new Uri("http://mariafresa.net/data_gallery/dice-2-clip-art-at-clker-com-vector-clip-art-online-royalty-free-JVGYH8-clipart.png");
                    break;
                case 2:
                    dices[2].UriSource = new Uri("http://www.clker.com/cliparts/M/e/P/O/L/b/dice-3-md.png");
                    break;
                case 3:
                    dices[3].UriSource = new Uri("https://cdn.pixabay.com/photo/2014/04/03/11/56/dice-312623_1280.png");
                    break;
                case 4:
                    dices[4].UriSource = new Uri("http://clipart-library.com/images/8cxnLaAdi.png");
                    break;
                case 5:
                    dices[5].UriSource = new Uri("http://www.clker.com/cliparts/Y/g/8/e/o/9/dice-6-md.png");
                    break;
            }
        }

        private void InitiateImages()
        {
            int i;
            //Icon + board:
            logo.BeginInit();
            logo.UriSource = new Uri("https://cdn.shopify.com/s/files/1/0210/6626/products/PC162517_86aa633b-2484-45d3-8be3-b21a40c6cf05_1024x1024.jpeg?v=1389067252");
            logo.EndInit();
            this.Icon = logo;

            // set each dice with it's own image
            for (i = 0; i < 6; i++)
            {
                dices[i].BeginInit();
                setDiceSource(i);
                dices[i].EndInit();
            }

            OneDice.Source = dices[4];
            TwoDice.Source = dices[5];

        }

        private void HideAllObjects()
        {

            OneDice.Visibility = Visibility.Hidden;
            TwoDice.Visibility = Visibility.Hidden;

        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {

            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            SolidColorBrush white = new SolidColorBrush(Colors.FloralWhite);
            backgroundImage.Source = logo;

            OneDice.Visibility = Visibility.Visible;
            TwoDice.Visibility = Visibility.Visible;


            Panel.SetZIndex(backgroundImage, 1);
            Panel.SetZIndex(OneDice, 1);
            Panel.SetZIndex(TwoDice, 1);
            
            
            Panel.SetZIndex(Player1ScoreTextBox, 4);
            Panel.SetZIndex(Player2ScoreTextBox, 4);
            Panel.SetZIndex(ScoreTextBlock, 4);
            Panel.SetZIndex(fireworks, 4);

            ScoreTextBlock.Visibility = Visibility.Visible;
            Player1ScoreTextBox.Visibility = Visibility.Visible;
            Player2ScoreTextBox.Visibility = Visibility.Visible;
            startGame.Visibility = Visibility.Hidden;
            
            Player1Score = 0;
            Player2Score = 0;
            
        }


        private void startGame_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolTip tip = new ToolTip();
            startGame.ToolTip = tip;
            tip.Content = "click to start a new game";
            tip.Visibility = Visibility.Visible;
        }

        private void OneDice_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DiceClicked();
        }

        private void TwoDice_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DiceClicked();
        }

        private void DiceClicked()
        {
            if (timeToDice)
            {
                ThrowTheDice();

                if (lastPlayerToPlay == 2)
                {
                    turn = 1;
                }

                else
                {
                    turn = 2;
                }

                timeToDice = false;

                CheckAvailableMoves(turn, true, true);
                if (!isItPossibleToUseDice1 & !isItPossibleToUseDice2) // player has nothing to do
                {

                    PrepareOtherPlayerTurn();
                }

            }
        }

        private void ThrowTheDice()
        {
            int rand1, rand2;
            rand1 = IntUtil.Random(0, 6);
            rand2 = IntUtil.Random(0, 6);
            OneDice.Source = dices[rand1];
            TwoDice.Source = dices[rand2];
            dice1 = rand1 + 1;
            dice2 = rand2 + 1;
            if (dice1 == dice2)
            {
                isDoubleMode = true;
                doubleCount = 4;
            }

            else
            {
                isDoubleMode = false;
            }
        }



        public void CheckAvailableMoves(int player, bool checkDice1, bool checkDice2)
        {

            int i;
            isItPossibleToUseDice1 = false;
            isItPossibleToUseDice2 = false;

            if (checkDice1 || (isDoubleMode & (doubleCount > 0)))
            {
                if (player == 1) // BLACK
                {

                    // first check if there are any eaten - if there are must bring them back if
                    if (blacksEaten > 0)
                    {
                        if (!(board.allCheckers[(dice1-1), 0].Fill == white & board.allCheckers[dice1 - 1, 1].Fill == white))
                        {
                            isItPossibleToUseDice1 = true;
                        }
                    }

                    for (i = 0; i <= 23; i++) // there's no need to check the last stack
                    {
                        if (board.allCheckers[i, 0].Fill == black) // check the stack only if there's a black checker in it
                        {
                            if (i + dice1 < 24)
                            {
                                if (board.allCheckers[i + (dice1-1), 0].Fill == gold) // stack free
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i + (dice1-1), 0].Fill == black) // stack black
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i + (dice1-1), 1].Fill == gold) // can eat white
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                            }

                            else
                            {
                                if (FreeToClear())
                                    isItPossibleToUseDice1 = true;
                            }
                        }
                    }
                }

                else            // WHITE
                {
                    // first check if there are any eaten - if there are must bring them back if
                    if (whitesEaten > 0)
                    {
                        if (!(board.allCheckers[24 - dice1, 0].Fill == black & board.allCheckers[24 - dice1, 1].Fill == black))
                        {
                            isItPossibleToUseDice1 = true;
                        }
                    }

                    for (i = 23; i >= 0; i--)
                    {
                        if (board.allCheckers[i, 0].Fill == white) // check the stack only if there's a black checker in it
                        {
                            
                            if (i - dice1 > -1)
                            {
                                if (board.allCheckers[i - (dice1-1), 0].Fill == gold) // stack free
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i - (dice1-1), 0].Fill == white) // stack white
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i - (dice1-1), 1].Fill == gold) // can eat black
                                {
                                    isItPossibleToUseDice1 = true;
                                    //res = false;
                                }
                            }
                            else
                            {
                                if (FreeToClear())
                                    isItPossibleToUseDice1 = true;
                            }
                        }
                    }
                }
                        
            }
                    

        if (checkDice2 || (isDoubleMode & (doubleCount > 0)))
                {
                if (player == 1) // BLACK
                {
                    // first check if there are any eaten - if there are must bring them back if
                    if (blacksEaten > 0)
                    {
                        if (!(board.allCheckers[dice2-1, 0].Fill == white & board.allCheckers[dice2-1, 1].Fill == white))
                        {
                            isItPossibleToUseDice1 = true;
                        }
                    }

                    for (i = 0; i <= 23; i++) // there's no need to check the last stack
                    {
                        if (board.allCheckers[i, 0].Fill == black) // check the stack only if there's a black checker in it
                        {
                            if (i + dice2 < 24)
                            {
                                if (board.allCheckers[i + (dice2-1), 0].Fill == gold) // stack free
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i + (dice2-1), 0].Fill == black) // stack black
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i + (dice2-1), 1].Fill == gold) // can eat white
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                            }
                            else
                            {
                                if (FreeToClear())
                                    isItPossibleToUseDice2 = true;
                            }
                        }
                    }
                }

                else            // WHITE
                {
                    // first check if there are any eaten - if there are must bring them back if
                    if (whitesEaten > 0)
                    {
                        if (!(board.allCheckers[24 - dice2, 0].Fill == black & board.allCheckers[24 - dice2, 1].Fill == black))
                        {
                            isItPossibleToUseDice1 = true;
                        }
                    }

                    for (i = 23; i >= 0; i--)
                    {
                        if (board.allCheckers[i, 0].Fill == white) // check the stack only if there's a black checker in it
                        {
                            
                            if (i - dice2 > -1)
                            {
                                if (board.allCheckers[i - (dice2-1), 0].Fill == gold) // stack free
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i - (dice2-1), 0].Fill == white) // stack white
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                                else if (board.allCheckers[i - (dice2-1), 1].Fill == gold) // can eat black
                                {
                                    isItPossibleToUseDice2 = true;
                                    //res = false;
                                }
                            }
                            else
                            {
                                if (FreeToClear())
                                    isItPossibleToUseDice2 = true;
                            }
                        }
                    }
                }
            }
                
            
        }


        public void startBoard() // each checker at it's place and with it's color
        {
            SetCheckers();
            BindCheckers();
            StartEatenCheckers();
            StartBlackCheckers();
            StartWhiteCheckers();
            StartNoneCheckers();
            
        }

        public void SetCheckers()
        {
            
            int i, j;
            for (i = 0; i < 24; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    board.allCheckers[i, j] = new Ellipse();
                }
            }
        }

        public void BindCheckers() // Binding between the real checkers on the actual board
                                   // with the virtual board's checkers we created to easily execute 
                                   // the players moves
        {
            board.allCheckers[0, 0] = Checker00;
            board.allCheckers[0, 1] = Checker01;
            board.allCheckers[0, 2] = Checker02;
            board.allCheckers[0, 3] = Checker03;
            board.allCheckers[0, 4] = Checker04;
            board.allCheckers[0, 5] = Checker05;
            board.allCheckers[0, 6] = Checker06;
            board.allCheckers[0, 7] = Checker07;
            board.allCheckers[1, 0] = Checker10;
            board.allCheckers[1, 1] = Checker11;
            board.allCheckers[1, 2] = Checker12;
            board.allCheckers[1, 3] = Checker13;
            board.allCheckers[1, 4] = Checker14;
            board.allCheckers[1, 5] = Checker15;
            board.allCheckers[1, 6] = Checker16;
            board.allCheckers[1, 7] = Checker17;
            board.allCheckers[2, 0] = Checker20;
            board.allCheckers[2, 1] = Checker21;
            board.allCheckers[2, 2] = Checker22;
            board.allCheckers[2, 3] = Checker23;
            board.allCheckers[2, 4] = Checker24;
            board.allCheckers[2, 5] = Checker25;
            board.allCheckers[2, 6] = Checker26;
            board.allCheckers[2, 7] = Checker27;
            board.allCheckers[3, 0] = Checker30;
            board.allCheckers[3, 1] = Checker31;
            board.allCheckers[3, 2] = Checker32;
            board.allCheckers[3, 3] = Checker33;
            board.allCheckers[3, 4] = Checker34;
            board.allCheckers[3, 5] = Checker35;
            board.allCheckers[3, 6] = Checker36;
            board.allCheckers[3, 7] = Checker37;
            board.allCheckers[4, 0] = Checker40;
            board.allCheckers[4, 1] = Checker41;
            board.allCheckers[4, 2] = Checker42;
            board.allCheckers[4, 3] = Checker43;
            board.allCheckers[4, 4] = Checker44;
            board.allCheckers[4, 5] = Checker45;
            board.allCheckers[4, 6] = Checker46;
            board.allCheckers[4, 7] = Checker47;
            board.allCheckers[5, 0] = Checker50;
            board.allCheckers[5, 1] = Checker51;
            board.allCheckers[5, 2] = Checker52;
            board.allCheckers[5, 3] = Checker53;
            board.allCheckers[5, 4] = Checker54;
            board.allCheckers[5, 5] = Checker55;
            board.allCheckers[5, 6] = Checker56;
            board.allCheckers[5, 7] = Checker57;
            board.allCheckers[6, 0] = Checker60;
            board.allCheckers[6, 1] = Checker61;
            board.allCheckers[6, 2] = Checker62;
            board.allCheckers[6, 3] = Checker63;
            board.allCheckers[6, 4] = Checker64;
            board.allCheckers[6, 5] = Checker65;
            board.allCheckers[6, 6] = Checker66;
            board.allCheckers[6, 7] = Checker67;
            board.allCheckers[7, 0] = Checker70;
            board.allCheckers[7, 1] = Checker71;
            board.allCheckers[7, 2] = Checker72;
            board.allCheckers[7, 3] = Checker73;
            board.allCheckers[7, 4] = Checker74;
            board.allCheckers[7, 5] = Checker75;
            board.allCheckers[7, 6] = Checker76;
            board.allCheckers[7, 7] = Checker77;
            board.allCheckers[8, 0] = Checker80;
            board.allCheckers[8, 1] = Checker81;
            board.allCheckers[8, 2] = Checker82;
            board.allCheckers[8, 3] = Checker83;
            board.allCheckers[8, 4] = Checker84;
            board.allCheckers[8, 5] = Checker85;
            board.allCheckers[8, 6] = Checker86;
            board.allCheckers[8, 7] = Checker87;
            board.allCheckers[9, 0] = Checker90;
            board.allCheckers[9, 1] = Checker91;
            board.allCheckers[9, 2] = Checker92;
            board.allCheckers[9, 3] = Checker93;
            board.allCheckers[9, 4] = Checker94;
            board.allCheckers[9, 5] = Checker95;
            board.allCheckers[9, 6] = Checker96;
            board.allCheckers[9, 7] = Checker97;
            board.allCheckers[10, 0] = Checker100;
            board.allCheckers[10, 1] = Checker101;
            board.allCheckers[10, 2] = Checker102;
            board.allCheckers[10, 3] = Checker103;
            board.allCheckers[10, 4] = Checker104;
            board.allCheckers[10, 5] = Checker105;
            board.allCheckers[10, 6] = Checker106;
            board.allCheckers[10, 7] = Checker107;
            board.allCheckers[11, 0] = Checker110;
            board.allCheckers[11, 1] = Checker111;
            board.allCheckers[11, 2] = Checker112;
            board.allCheckers[11, 3] = Checker113;
            board.allCheckers[11, 4] = Checker114;
            board.allCheckers[11, 5] = Checker115;
            board.allCheckers[11, 6] = Checker116;
            board.allCheckers[11, 7] = Checker117;
            board.allCheckers[12, 0] = Checker120;
            board.allCheckers[12, 1] = Checker121;
            board.allCheckers[12, 2] = Checker122;
            board.allCheckers[12, 3] = Checker123;
            board.allCheckers[12, 4] = Checker124;
            board.allCheckers[12, 5] = Checker125;
            board.allCheckers[12, 6] = Checker126;
            board.allCheckers[12, 7] = Checker127;
            board.allCheckers[13, 0] = Checker130;
            board.allCheckers[13, 1] = Checker131;
            board.allCheckers[13, 2] = Checker132;
            board.allCheckers[13, 3] = Checker133;
            board.allCheckers[13, 4] = Checker134;
            board.allCheckers[13, 5] = Checker135;
            board.allCheckers[13, 6] = Checker136;
            board.allCheckers[13, 7] = Checker137;
            board.allCheckers[14, 0] = Checker140;
            board.allCheckers[14, 1] = Checker141;
            board.allCheckers[14, 2] = Checker142;
            board.allCheckers[14, 3] = Checker143;
            board.allCheckers[14, 4] = Checker144;
            board.allCheckers[14, 5] = Checker145;
            board.allCheckers[14, 6] = Checker146;
            board.allCheckers[14, 7] = Checker147;
            board.allCheckers[15, 0] = Checker150;
            board.allCheckers[15, 1] = Checker151;
            board.allCheckers[15, 2] = Checker152;
            board.allCheckers[15, 3] = Checker153;
            board.allCheckers[15, 4] = Checker154;
            board.allCheckers[15, 5] = Checker155;
            board.allCheckers[15, 6] = Checker156;
            board.allCheckers[15, 7] = Checker157;
            board.allCheckers[16, 0] = Checker160;
            board.allCheckers[16, 1] = Checker161;
            board.allCheckers[16, 2] = Checker162;
            board.allCheckers[16, 3] = Checker163;
            board.allCheckers[16, 4] = Checker164;
            board.allCheckers[16, 5] = Checker165;
            board.allCheckers[16, 6] = Checker166;
            board.allCheckers[16, 7] = Checker167;
            board.allCheckers[17, 0] = Checker170;
            board.allCheckers[17, 1] = Checker171;
            board.allCheckers[17, 2] = Checker172;
            board.allCheckers[17, 3] = Checker173;
            board.allCheckers[17, 4] = Checker174;
            board.allCheckers[17, 5] = Checker175;
            board.allCheckers[17, 6] = Checker176;
            board.allCheckers[17, 7] = Checker177;
            board.allCheckers[18, 0] = Checker180;
            board.allCheckers[18, 1] = Checker181;
            board.allCheckers[18, 2] = Checker182;
            board.allCheckers[18, 3] = Checker183;
            board.allCheckers[18, 4] = Checker184;
            board.allCheckers[18, 5] = Checker185;
            board.allCheckers[18, 6] = Checker186;
            board.allCheckers[18, 7] = Checker187;
            board.allCheckers[19, 0] = Checker190;
            board.allCheckers[19, 1] = Checker191;
            board.allCheckers[19, 2] = Checker192;
            board.allCheckers[19, 3] = Checker193;
            board.allCheckers[19, 4] = Checker194;
            board.allCheckers[19, 5] = Checker195;
            board.allCheckers[19, 6] = Checker196;
            board.allCheckers[19, 7] = Checker197;
            board.allCheckers[20, 0] = Checker200;
            board.allCheckers[20, 1] = Checker201;
            board.allCheckers[20, 2] = Checker202;
            board.allCheckers[20, 3] = Checker203;
            board.allCheckers[20, 4] = Checker204;
            board.allCheckers[20, 5] = Checker205;
            board.allCheckers[20, 6] = Checker206;
            board.allCheckers[20, 7] = Checker207;
            board.allCheckers[21, 0] = Checker210;
            board.allCheckers[21, 1] = Checker211;
            board.allCheckers[21, 2] = Checker212;
            board.allCheckers[21, 3] = Checker213;
            board.allCheckers[21, 4] = Checker214;
            board.allCheckers[21, 5] = Checker215;
            board.allCheckers[21, 6] = Checker216;
            board.allCheckers[21, 7] = Checker217;
            board.allCheckers[22, 0] = Checker220;
            board.allCheckers[22, 1] = Checker221;
            board.allCheckers[22, 2] = Checker222;
            board.allCheckers[22, 3] = Checker223;
            board.allCheckers[22, 4] = Checker224;
            board.allCheckers[22, 5] = Checker225;
            board.allCheckers[22, 6] = Checker226;
            board.allCheckers[22, 7] = Checker227;
            board.allCheckers[23, 0] = Checker230;
            board.allCheckers[23, 1] = Checker231;
            board.allCheckers[23, 2] = Checker232;
            board.allCheckers[23, 3] = Checker233;
            board.allCheckers[23, 4] = Checker234;
            board.allCheckers[23, 5] = Checker235;
            board.allCheckers[23, 6] = Checker236;
            board.allCheckers[23, 7] = Checker237;

            for (int i = 0; i < 24; i++)
            {
                for (int j= 0; j < 8; j++)
                {
                    Panel.SetZIndex(board.allCheckers[i, j], 3);
                }
            }
        }

        // the following 4 methods organize the board to it's first display

        public void StartEatenCheckers() 
        {
            Panel.SetZIndex(CheckerWhiteEaten1, 3);
            Panel.SetZIndex(CheckerWhiteEaten2, 3);
            Panel.SetZIndex(CheckerWhiteEaten3, 3);
            Panel.SetZIndex(CheckerBlackEaten1, 3);
            Panel.SetZIndex(CheckerBlackEaten2, 3);
            Panel.SetZIndex(CheckerBlackEaten3, 3);

            CheckerWhiteEaten1.Visibility = CheckerWhiteEaten2.Visibility = CheckerWhiteEaten3.Visibility = Visibility.Hidden;
            CheckerBlackEaten1.Visibility = CheckerBlackEaten2.Visibility = CheckerBlackEaten3.Visibility = Visibility.Hidden;

            CheckerWhiteEaten1.Fill = CheckerWhiteEaten2.Fill = CheckerWhiteEaten3.Fill = white;
            CheckerBlackEaten1.Fill = CheckerBlackEaten2.Fill = CheckerBlackEaten3.Fill = black;
        }

        public void StartBlackCheckers()
        {
            int i;
            for (i = 0; i < 2; i++)
            {
                board.allCheckers[0, i].Fill = black;
                board.allCheckers[0, i].Visibility = Visibility.Visible;
            }
            for (i = 0; i < 5; i++)
            {
                board.allCheckers[11, i].Fill = black;
                board.allCheckers[11, i].Visibility = Visibility.Visible;
                board.allCheckers[18, i].Fill = black;
                board.allCheckers[18, i].Visibility = Visibility.Visible;
            }
            for (i = 0; i < 3; i++)
            {
                board.allCheckers[16, i].Fill = black;
                board.allCheckers[16, i].Visibility = Visibility.Visible;
            }

        }

        public void StartWhiteCheckers()
        {
            int i;
            for (i = 0; i < 2; i++)
            {
                board.allCheckers[23, i].Fill = white;
                board.allCheckers[23, i].Visibility = Visibility.Visible;
            }
            for (i = 0; i < 5; i++)
            {
                board.allCheckers[5, i].Fill = white;
                board.allCheckers[5, i].Visibility = Visibility.Visible;
                board.allCheckers[12, i].Fill = white;
                board.allCheckers[12, i].Visibility = Visibility.Visible;
            }
            for (i = 0; i < 3; i++)
            {
                board.allCheckers[7, i].Fill = white;
                board.allCheckers[7, i].Visibility = Visibility.Visible;
            }

        }

        public void StartNoneCheckers()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                board.allCheckers[1, i].Fill = gold;
                board.allCheckers[1, i].Visibility = Visibility.Hidden;
                //board.allCheckers[1, i].Opacity = 0;

                board.allCheckers[2, i].Fill = gold;
                board.allCheckers[2, i].Visibility = Visibility.Hidden;
                //board.allCheckers[2, i].Opacity = 0;

                board.allCheckers[3, i].Fill = gold;
                board.allCheckers[3, i].Visibility = Visibility.Hidden;
                //board.allCheckers[3, i].Opacity = 0;

                board.allCheckers[4, i].Fill = gold;
                board.allCheckers[4, i].Visibility = Visibility.Hidden;
                //board.allCheckers[4, i].Opacity = 0;

                board.allCheckers[6, i].Fill = gold;
                board.allCheckers[6, i].Visibility = Visibility.Hidden;
                //board.allCheckers[6, i].Opacity = 0;

                board.allCheckers[8, i].Fill = gold;
                board.allCheckers[8, i].Visibility = Visibility.Hidden;
     //           board.allCheckers[8, i].Opacity = 0;

                board.allCheckers[9, i].Fill = gold;
                board.allCheckers[9, i].Visibility = Visibility.Hidden;
                //board.allCheckers[9, i].Opacity = 0;

                board.allCheckers[10, i].Fill = gold;
                board.allCheckers[10, i].Visibility = Visibility.Hidden;
                //board.allCheckers[10, i].Opacity = 0;

                board.allCheckers[13, i].Fill = gold;
                board.allCheckers[13, i].Visibility = Visibility.Hidden;
                //board.allCheckers[13, i].Opacity = 0;

                board.allCheckers[14, i].Fill = gold;
                board.allCheckers[14, i].Visibility = Visibility.Hidden;
                //board.allCheckers[14, i].Opacity = 0;

                board.allCheckers[15, i].Fill = gold;
                board.allCheckers[15, i].Visibility = Visibility.Hidden;
                //board.allCheckers[15, i].Opacity = 0;

                board.allCheckers[17, i].Fill = gold;
                board.allCheckers[17, i].Visibility = Visibility.Hidden;
                //board.allCheckers[17, i].Opacity = 0;

                board.allCheckers[19, i].Fill = gold;
                board.allCheckers[19, i].Visibility = Visibility.Hidden;
                //board.allCheckers[19, i].Opacity = 0;

                board.allCheckers[20, i].Fill = gold;
                board.allCheckers[20, i].Visibility = Visibility.Hidden;
                //board.allCheckers[20, i].Opacity = 0;

                board.allCheckers[21, i].Fill = gold;
                board.allCheckers[21, i].Visibility = Visibility.Hidden;
                //board.allCheckers[21, i].Opacity = 0;

                board.allCheckers[22, i].Fill = gold;
                board.allCheckers[22, i].Visibility = Visibility.Hidden;
                //board.allCheckers[22, i].Opacity = 0;
            }

            for (i = 5; i < 8; i++)
            {
                board.allCheckers[5, i].Fill = gold;
                board.allCheckers[5, i].Visibility = Visibility.Hidden;
                //board.allCheckers[5, i].Opacity = 0;

                board.allCheckers[11, i].Fill = gold;
                board.allCheckers[11, i].Visibility = Visibility.Hidden;
                //  board.allCheckers[11, i].Opacity = 0;

                board.allCheckers[12, i].Fill = gold;
                board.allCheckers[12, i].Visibility = Visibility.Hidden;
                //board.allCheckers[12, i].Opacity = 0;

                board.allCheckers[18, i].Fill = gold;
                board.allCheckers[18, i].Visibility = Visibility.Hidden;
                //board.allCheckers[18, i].Opacity = 0;
            }

            for (i = 3; i < 8; i++)
            {
                board.allCheckers[7, i].Fill = gold;
                board.allCheckers[7, i].Visibility = Visibility.Hidden;
                //board.allCheckers[7, i].Opacity = 0;

                board.allCheckers[16, i].Fill = gold;
                board.allCheckers[16, i].Visibility = Visibility.Hidden;
                //board.allCheckers[16, i].Opacity = 0;

            }

            for (i = 2; i < 8; i++)
            {
                board.allCheckers[0, i].Fill = gold;
                board.allCheckers[0, i].Visibility = Visibility.Hidden;
                //board.allCheckers[0, i].Opacity = 0;

                board.allCheckers[23, i].Fill = gold;
                board.allCheckers[23, i].Visibility = Visibility.Hidden;
                //board.allCheckers[23, i].Opacity = 0;
            }
        }

        public int findStackAndNumber(Ellipse checker, out int stack, out int number)
        {
            int i, j;
            for (i = 0; i < 24; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (board.allCheckers[i, j] == checker)
                    {
                        stack = i;
                        number = j;
                        return 1;
                    }
                }
            }
            stack = -1;
            number = -1;
            return 0;
        }

        public bool IsThereAnyPossibleDrawn() // return true if there's atleast 1 possible move drawn on board right now
        {
            int i, j;
            
            for (i = 0; i < 24; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (board.allCheckers[i, j].Opacity == 0.5) // possible move always drawn with opacity 0.5
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool FreeToClear()
        {
            if (turn == 1)
            {
                if (blacksEaten == 0)
                {
                    for (int i = 0; i < 18; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (board.allCheckers[i, j].Fill == black)
                                return false;
                        }
                    }
                    return true;
                }
                else
                    return false;

            }

            else
            {
                if (whitesEaten == 0)
                {
                    for (int i = 23; i > 5; i--)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (board.allCheckers[i, j].Fill == white)
                                return false;
                        }
                    }

                    return true;
                }
                else
                    return false;
            }
        }

        public void MakeAMoveAndUpdateDiceUse(int stack, int number, out int hasEaten)
        {
            hasEaten = 0; // don't want to clear
            board.allCheckers[stack, number].Opacity = 100;
            board.allCheckers[stack, number].StrokeThickness = 1;

            if (board.allCheckers[lastChosenCheckerStack, lastChosenCheckerNumber].Fill == black) // switch with black
            {
                if (board.allCheckers[stack, number].Fill == black) // meaning player1 clears the checker
                {
                    if (FreeToClear())
                    {
                        Player1Score++;
                        board.allCheckers[stack, number].Fill = gold;
                        board.allCheckers[stack, number].Visibility = Visibility.Hidden;
                        Player1ScoreTextBox.Text = "Black: " + Player1Score.ToString();
                        hasEaten = 2; // wanted to clear and clear
                    }

                    else
                    {
                        hasEaten = 1; // wanted to clear but failed
                    }
                }

                else if (board.allCheckers[stack, number].Fill == white) // meaning wants to eat
                {
                    
                    board.allCheckers[stack, number].Fill = black;
                    if (whitesEaten == 0)
                    {
                        CheckerWhiteEaten1.Visibility = Visibility.Visible;
                    }
                    else if (whitesEaten == 1)
                    {
                        CheckerWhiteEaten2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CheckerWhiteEaten3.Visibility = Visibility.Visible;
                    }
                    whitesEaten++;
                }

                else
                {
                    board.allCheckers[stack, number].Fill = black;
                }

            }
            

            else
            {
                if (board.allCheckers[stack, number].Fill == white) // meaning player1 clears the checker
                {
                    if (FreeToClear())
                    {
                        Player2Score++;
                        board.allCheckers[stack, number].Fill = gold;
                        board.allCheckers[stack, number].Visibility = Visibility.Hidden;
                        Player2ScoreTextBox.Text = "White: " + Player2Score.ToString();
                        hasEaten = 2;
                    }
                    else
                        hasEaten = 1;
                }

                else if (board.allCheckers[stack, number].Fill == black) // meaning wants to eat
                {
                    board.allCheckers[stack, number].Fill = white;
                    if (blacksEaten == 0)
                    {
                        CheckerBlackEaten1.Visibility = Visibility.Visible;
                    }
                    else if (blacksEaten == 1)
                    {
                        CheckerBlackEaten2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CheckerBlackEaten3.Visibility = Visibility.Visible;
                    }
                    blacksEaten++;
                }
                else
                {
                    board.allCheckers[stack, number].Fill = white;
                }



            }

            // DONE BY BOTH:
            if (hasEaten != 1)
            {
                if (hasEaten == 2)
                {
                    if (turn == 1) // black player
                    {
                        if ((stack + dice1 > 23) & (stack + dice2 > 23)) // both can use to clear

                        {
                            if (isItPossibleToUseDice2 & isItPossibleToUseDice1) // both hasn't used yet
                            {
                                //use the small one
                                if (dice1 < dice2)
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                }
                                else
                                {
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                            }

                            else // use the one hasn't use
                            {
                                if (isItPossibleToUseDice1)
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                }
                                else if (isItPossibleToUseDice2)
                                {
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                                else
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                            }
                        }

                        else // only one can use to clear - use it!
                        {
                            if (dice1 < dice2)
                            {
                                isDice2AlreadyUsed = true;
                                isItPossibleToUseDice2 = false;

                            }
                            else
                            {
                                isDice1AlreadyUsed = true;
                                isItPossibleToUseDice1 = false;
                            }
                        }
                    }

                    else // white player
                    {
                        if ((stack - dice1 < 0) & (stack - dice2 < 0)) // both can use to clear
                        {
                            if (isItPossibleToUseDice2 & isItPossibleToUseDice1) // both hasn't used yet
                            {
                                // take the small one
                                if (dice1 < dice2)
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                }
                                else
                                {
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                            }
                            else
                            {
                                if (isItPossibleToUseDice1)
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                }
                                else if (isItPossibleToUseDice2)
                                {
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                                else
                                {
                                    isDice1AlreadyUsed = true;
                                    isItPossibleToUseDice1 = false;
                                    isDice2AlreadyUsed = true;
                                    isItPossibleToUseDice2 = false;
                                }
                            }
                        }

                        else // use the one that can clear
                        {
                            if (dice1 < dice2)
                            {
                                isDice2AlreadyUsed = true;
                                isItPossibleToUseDice2 = false;

                            }
                            else
                            {
                                isDice1AlreadyUsed = true;
                                isItPossibleToUseDice1 = false;
                            }
                        }
                    }

                }

                else
                {
                    if (turn == 1)
                    {
                        if (stack - lastChosenCheckerStack == dice1)
                        {
                            isItPossibleToUseDice1 = false;
                            isDice1AlreadyUsed = true;
                        }
                        else
                        {
                            isItPossibleToUseDice2 = false;
                            isDice2AlreadyUsed = true;
                        }
                    }

                    else
                    {
                        if (lastChosenCheckerStack - stack == dice1)
                        {
                            isItPossibleToUseDice1 = false;
                            isDice1AlreadyUsed = true;
                        }
                        else
                        {
                            isItPossibleToUseDice2 = false;
                            isDice2AlreadyUsed = true;
                        }
                    }
                }

                if (isDoubleMode)
                {
                    doubleCount--;
                }
            }
            else
            {
                return;
            }
            // HANDLE THE HASEATEN == 1 : must not allowed to change to gold and hidden

            board.allCheckers[lastChosenCheckerStack, lastChosenCheckerNumber].Fill = gold;
            board.allCheckers[lastChosenCheckerStack, lastChosenCheckerNumber].Visibility = Visibility.Hidden;
            if (IsThereAnyPossibleDrawn())
                CleanPossibleMoves();
        }

        public void CleanPossibleMoves()
        {
            int i, j;
            for (i = 0; i < 24; i ++)
            {
                for (j = 0; j < 8; j ++)
                {
                    if (board.allCheckers[i, j].Opacity == 0.5 & board.allCheckers[i, j].Fill == gold)
                    {
                        board.allCheckers[i, j].Opacity = 100;
                        board.allCheckers[i, j].StrokeThickness = 1;
                        board.allCheckers[i, j].Visibility = Visibility.Hidden;
                    }
                    else if (board.allCheckers[i, j].Opacity == 0.5)
                    {
                        board.allCheckers[i, j].StrokeThickness = 1;
                        board.allCheckers[i, j].Opacity = 100;
                    }
                }
            }
        }

        public void WhenEatenClicked(object sender)
        {
            Ellipse _sender = sender as Ellipse;
            int i;
            int number = -1;

            if (!timeToDice) // can click on the button only when it's time to play
            {
                if (_sender.Fill == black & turn == 1 & !isHandleingBlackEaten)
                {
                    if (!isDice1AlreadyUsed)
                    {
                        if (!(board.allCheckers[dice1 - 1, 0].Fill == white & board.allCheckers[dice1 - 1, 1].Fill == white))
                        {
                            isItPossibleToUseDice1ToRevive = true;
                            isHandleingBlackEaten = true;
                            if (board.allCheckers[dice1 - 1, 0].Fill == white)
                            {
                                board.allCheckers[dice1 - 1, 0].Opacity = 0.5;
                                board.allCheckers[dice1 - 1, 0].StrokeThickness = 4;
                            }

                            else
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    if (board.allCheckers[dice1 - 1, i].Fill == gold)
                                    {
                                        number = i;
                                        break;
                                    }
                                }

                                board.allCheckers[dice1 - 1, number].Opacity = 0.5;
                                board.allCheckers[dice1 - 1, number].StrokeThickness = 4;
                                board.allCheckers[dice1 - 1, number].Visibility = Visibility.Visible;
                            }
                        }
                    }

                    else
                    {
                        isItPossibleToUseDice1ToRevive = false;
                    }

                    if (!isDice2AlreadyUsed)
                    {
                        if (!(board.allCheckers[dice2 - 1, 0].Fill == white & board.allCheckers[dice2 - 1, 1].Fill == white))
                        {
                            isItPossibleToUseDice2ToRevive = true;
                            isHandleingBlackEaten = true;
                            if (board.allCheckers[dice2 - 1, 0].Fill == white)
                            {
                                board.allCheckers[dice2 - 1, 0].Opacity = 0.5;
                                board.allCheckers[dice2 - 1, 0].StrokeThickness = 4;

                            }
                            else
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    if (board.allCheckers[dice2 - 1, i].Fill == gold)
                                    {
                                        number = i;
                                        break;
                                    }
                                }
                                board.allCheckers[dice2 - 1, number].Visibility = Visibility.Visible;
                                board.allCheckers[dice2 - 1, number].Opacity = 0.5;
                                board.allCheckers[dice2 - 1, number].StrokeThickness = 4;
                            }

                        }
                    }

                    else
                    {
                        isItPossibleToUseDice2ToRevive = false;
                    }
                }

                else if (_sender.Fill == white & turn == 2 & !isHandleingWhiteEaten)
                {
                    if (!isDice1AlreadyUsed)
                    {
                        if (!(board.allCheckers[24 - dice1, 0].Fill == black & board.allCheckers[24 - dice1, 1].Fill == black))
                        {
                            isItPossibleToUseDice1ToRevive = true;
                            isHandleingWhiteEaten = true;
                            if (board.allCheckers[24 - dice1, 0].Fill == black)
                            {
                                board.allCheckers[24 - dice1, 0].Opacity = 0.5;
                                board.allCheckers[24 - dice1, 0].StrokeThickness = 4;
                            }
                            else
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    if (board.allCheckers[24 - dice1, i].Fill == gold)
                                    {
                                        number = i;
                                        break;
                                    }
                                }
                                board.allCheckers[24 - dice1, number].Opacity = 0.5;
                                board.allCheckers[24 - dice1, number].StrokeThickness = 4;
                                board.allCheckers[24 - dice1, number].Visibility = Visibility.Visible;
                            }

                        }
                    }

                    else
                    {
                        isItPossibleToUseDice1ToRevive = false;
                    }

                    if (!isDice2AlreadyUsed)
                    {
                        if (!(board.allCheckers[24 - dice2, 0].Fill == black & board.allCheckers[24 - dice2, 1].Fill == black))
                        {
                            isItPossibleToUseDice2ToRevive = true;
                            isHandleingWhiteEaten = true;
                            if (board.allCheckers[24 - dice2, 0].Fill == black)
                            {
                                board.allCheckers[24 - dice2, 0].Opacity = 0.5;
                                board.allCheckers[24 - dice2, 0].StrokeThickness = 4;
                            }
                            else
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    if (board.allCheckers[24 - dice2, i].Fill == gold)
                                    {
                                        number = i;
                                        break;
                                    }
                                }
                                board.allCheckers[24 - dice2, number].Opacity = 0.5;
                                board.allCheckers[24 - dice2, number].StrokeThickness = 4;
                                board.allCheckers[24 - dice2, number].Visibility = Visibility.Visible;
                            }

                        }
                    }

                    else
                    {
                        isItPossibleToUseDice2ToRevive = false;
                    }


                }

                if (!isItPossibleToUseDice1ToRevive & !isItPossibleToUseDice2ToRevive)
                {
                    PrepareOtherPlayerTurn();
                }
            }
            
        }

        public void ReviveBlackByPlace(int stack, int number)
        {
            if (board.allCheckers[stack, number].Fill == white)
            {
                if (CheckerWhiteEaten1.Visibility == Visibility.Hidden)
                {
                    CheckerWhiteEaten1.Visibility = Visibility.Visible;
                }

                else if (CheckerWhiteEaten2.Visibility == Visibility.Hidden)
                {
                    CheckerWhiteEaten2.Visibility = Visibility.Visible;
                }

                else
                {
                    CheckerWhiteEaten3.Visibility = Visibility.Visible;
                }

                whitesEaten++;
            }

            blacksEaten--;
            isHandleingBlackEaten = false;
            board.allCheckers[stack, number].Fill = black;
            board.allCheckers[stack, number].Opacity = 100;
            board.allCheckers[stack, number].StrokeThickness = 1;

            if (CheckerBlackEaten3.Visibility == Visibility.Visible)
            {
                CheckerBlackEaten3.Visibility = Visibility.Hidden;
            }

            else if (CheckerBlackEaten2.Visibility == Visibility.Visible)
            {
                CheckerBlackEaten2.Visibility = Visibility.Hidden;
            }

            else
            {
                CheckerBlackEaten1.Visibility = Visibility.Hidden;
            }

            if (stack == dice1 - 1)
            {
                isItPossibleToUseDice1 = false;
                isDice1AlreadyUsed = true;
                isItPossibleToUseDice1ToRevive = false;
            }

            else
            {
                isItPossibleToUseDice2 = false;
                isDice2AlreadyUsed = true;
                isItPossibleToUseDice2ToRevive = false;
            }

            if (isDoubleMode)
            {
                doubleCount--;
            }

            if (IsThereAnyPossibleDrawn())
                CleanPossibleMoves();
           
        }

        public void ReviveWhiteByPlace(int stack, int number)
        {
            if (board.allCheckers[stack, number].Fill == black)
            {
                if (CheckerBlackEaten1.Visibility == Visibility.Hidden)
                {
                    CheckerBlackEaten1.Visibility = Visibility.Visible;
                }
                else if (CheckerBlackEaten2.Visibility == Visibility.Hidden)
                {
                    CheckerBlackEaten2.Visibility = Visibility.Visible;
                }
                else
                {
                    CheckerBlackEaten3.Visibility = Visibility.Visible;
                }
                blacksEaten++;
            }

            isHandleingWhiteEaten = false;
            whitesEaten--;
            board.allCheckers[stack, number].Fill = white;
            board.allCheckers[stack, number].Opacity = 100;
            board.allCheckers[stack, number].StrokeThickness = 1;

            if (CheckerWhiteEaten3.Visibility == Visibility.Visible)
            {
                CheckerWhiteEaten3.Visibility = Visibility.Hidden;
            }
            else if (CheckerWhiteEaten2.Visibility == Visibility.Visible)
            {
                CheckerWhiteEaten2.Visibility = Visibility.Hidden;
            }
            else
            {
                CheckerWhiteEaten1.Visibility = Visibility.Hidden;
            }

            if (24 - stack == dice1)
            {
                isItPossibleToUseDice1 = false;
                isDice1AlreadyUsed = true;
            }
            else
            {
                isItPossibleToUseDice2 = false;
                isDice2AlreadyUsed = true;
            }

            if (isDoubleMode)
            {
                doubleCount--;
            }

            if (IsThereAnyPossibleDrawn())
                CleanPossibleMoves();
        }

        public void WhenClicked(object sender)
        {
            Ellipse _sender = sender as Ellipse;
            int stack, number;
            int hasEaten = 0;

            if (!timeToDice) // the player must throw the dices in order to begin his turn
            {

                if (isHandleingBlackEaten || isHandleingWhiteEaten) // here means we chose one of the eatens
                                                                    // and now we want to check where to add it
                {
                    findStackAndNumber(_sender, out stack, out number);
                    if (turn == 1) // black player's turn
                    {
                        AnyOptionsShown = IsThereAnyPossibleDrawn();
                        if (AnyOptionsShown) // must be gold or white
                        {
                            ReviveBlackByPlace(stack, number);

                        }
                    }
                    else
                    {
                        AnyOptionsShown = IsThereAnyPossibleDrawn();
                        if (AnyOptionsShown) // must be gold or white
                        {
                            ReviveWhiteByPlace(stack, number);

                        }
                    }
                }

                else
                {
                    if (!((turn == 1 & blacksEaten > 0) || (turn == 2 & whitesEaten > 0)))
                    {
                        findStackAndNumber(_sender, out stack, out number); // stack and number indicates on whice checker we choose
                        AnyOptionsShown = IsThereAnyPossibleDrawn();
                        if (!AnyOptionsShown)
                        {
                            lastChosenCheckerStack = stack;
                            lastChosenCheckerNumber = number;
                        }


                        // Player wants to make a move
                        if (AnyOptionsShown & board.allCheckers[stack, number].Opacity == 0.5)
                        {
                            // do the swaping
                            MakeAMoveAndUpdateDiceUse(stack, number, out hasEaten);
                            if (!isDoubleMode)
                            {
                                CheckAvailableMoves(turn, !isDice1AlreadyUsed, !isDice2AlreadyUsed);
                                if (!isItPossibleToUseDice1 & !isItPossibleToUseDice2)
                                {
                                    PrepareOtherPlayerTurn();
                                }
                            }

                            else
                            {
                                if (doubleCount == 0)
                                    PrepareOtherPlayerTurn();
                                else if (Player1Score >= 15 | Player2Score >= 15)
                                    PrepareOtherPlayerTurn();
                            }
                        }

                        // Player wants to check other possibilites
                        else if (stack == lastChosenCheckerStack & AnyOptionsShown) // undo clicking to see options
                        {
                            if (IsThereAnyPossibleDrawn())
                            {
                                CleanPossibleMoves();
                            }
                            
                        }

                        else // choosing on clean board
                        {
                            CheckAvailableMoves(turn, !isDice1AlreadyUsed, !isDice2AlreadyUsed);
                            lastChosenCheckerStack = stack;
                            lastChosenCheckerNumber = number;
                            if (!isItPossibleToUseDice1 & !isItPossibleToUseDice2)
                            {
                                PrepareOtherPlayerTurn();
                            }

                            else if (_sender.Fill == black)
                            {
                                ClickOnCheckerAsBlack(sender, stack, number);
                            }

                            else if (_sender.Fill == white)
                            {
                                ClickOnCheckerAsWhite(sender, stack, number);
                            }

                            else
                            {
                                // clicked on a golden hidden checker - do nothing
                            }
                        }
                    }
                }
            }
        }

        public void PrepareOtherPlayerTurn()
        {
            int isPlayerWon = PlayerWon();
            if (isPlayerWon > 0)
            {
                fireworks.Visibility = Visibility.Visible;
                if (isPlayerWon == 1)
                {

                    // player1 won
                }
                else
                {
                    
                    // player2 won 
                }
            }

            else
            {
                timeToDice = true;
                isDice1AlreadyUsed = false;
                isDice2AlreadyUsed = false;
                isItPossibleToUseDice1 = true;
                isItPossibleToUseDice1 = true;
                Dice1PossibleMovesShown = false;
                Dice2PossibleMovesShown = false;
                isItPossibleToUseDice1ToRevive = false;
                isItPossibleToUseDice2ToRevive = false;
                isDoubleMode = false;
                doubleCount = 0;
                if(IsThereAnyPossibleDrawn())
                    CleanPossibleMoves();
                
                if (turn == 1)
                {
                    turn = 2;
                    lastPlayerToPlay = 1;
                }
                else
                {
                    turn = 1;
                    lastPlayerToPlay = 2;
                }
            }
        }

        public int PlayerWon()
        {
            if (Player1Score == 15)
                return 1;
            
            else if (Player2Score == 15)
                return 2;

            else
                return 0;
        }



        public void ClickOnCheckerAsBlack(object sender, int stack, int number)
        {
            int firstFreeDice1, firstFreeDice2;
            if (turn == 1)
            {
                if (!isDice1AlreadyUsed)
                {
                    FindFirstFreeCheckerOnStack(stack + dice1, 1, out firstFreeDice1);
                    if (firstFreeDice1 == -1) // player can get a checker out of the board
                    {
                        board.allCheckers[stack, number].StrokeThickness = 4;
                        board.allCheckers[stack, number].Opacity = 0.5;
                    }

                    else if (firstFreeDice1 == 10) // the stack belongs to the other player
                    {
                        //THE PLAYER CHOSE A CHECKER WE CAN'T MOVE WITH DICE 1
                        //return;
                    }

                    else
                    {
                        if (firstFreeDice1 == 8) // black checker can eat
                        {
                            board.allCheckers[stack + dice1, 0].StrokeThickness = 4;
                            board.allCheckers[stack + dice1, 0].Opacity = 0.5;
                            
                        }
                        else if (firstFreeDice1 == 8) // stack full, insert to 7
                        {

                        }

                        else // normal possible moves
                        {
                            board.allCheckers[stack + dice1, firstFreeDice1].Visibility = Visibility.Visible;
                            board.allCheckers[stack + dice1, firstFreeDice1].Opacity = 0.5; // half transparent
                            board.allCheckers[stack + dice1, firstFreeDice1].StrokeThickness = 4;
                        }

                        Dice1PossibleMovesShown = true;
                        //return;
                    }
                }

                if (!isDice2AlreadyUsed || (isDoubleMode & (doubleCount > 0)))
                {
                    FindFirstFreeCheckerOnStack(stack + dice2, 1, out firstFreeDice2);
                    if (firstFreeDice2 == -1) // player can get a checker out of the board
                    {
                        board.allCheckers[stack, number].StrokeThickness = 4;
                        board.allCheckers[stack, number].Opacity = 0.5;
                        
                    }
                    else if (firstFreeDice2 == 10)
                    {
                        //THE PLAYER CHOSE A CHECKER WE CAN'T MOVE WITH DICE 2
                        //return;
                    }
                    else
                    {
                        if (firstFreeDice2 == 8) // black checker can eat
                        {
                            
                           
                            board.allCheckers[stack + dice2, 0].StrokeThickness = 4;
                            board.allCheckers[stack + dice2, 0].Opacity = 0.5;
                        }
                        else if (firstFreeDice2 == 8) // stack full, insert to 7
                        {

                        }

                        else // normal possible moves
                        {
                            board.allCheckers[stack + dice2, firstFreeDice2].Visibility = Visibility.Visible;
                            board.allCheckers[stack + dice2, firstFreeDice2].Opacity = 0.5; // half transparent
                            board.allCheckers[stack + dice2, firstFreeDice2].StrokeThickness = 4;
                        }

                        Dice2PossibleMovesShown = true;
                        //return;
                    }
                }

            }
        }

        public void FindFirstFreeCheckerOnStack(int stack, int player, out int firstFree)
        // returns the first free open (or 7 if all unablable
        // if the stack belongs to the other player - return - 1;
        {
            int i;
            if (stack < 24 & stack > -1)
            {
                if ((board.allCheckers[stack, 0].Fill == white & board.allCheckers[stack, 1].Fill == white & player == 1)       // if the stack belongs to the other player
                    || (board.allCheckers[stack, 0].Fill == black & board.allCheckers[stack, 1].Fill == black & player == 2))
                {                        // the stack belongs to the other player
                    firstFree = 10; // firstFree = 10 - cant go there
                    return;
                }
                else if ((board.allCheckers[stack, 0].Fill == white & board.allCheckers[stack, 1].Fill == gold & player == 1)
                    || (board.allCheckers[stack, 0].Fill == black & board.allCheckers[stack, 1].Fill == gold & player == 2))
                {
                    firstFree = 8; // 8 means can eat
                    return;
                    // can EAT
                }

                else // either the stack is free or it's already belongs to the player
                {
                    for (i = 0; i < 8; i++)
                    {
                        if (board.allCheckers[stack, i].Fill == gold)
                        {
                            firstFree = i;
                            return;
                        }
                    }
                    firstFree = 7;
                    return;
                }
            }
            else
            {
                firstFree = -1;
            }
        }

        public void ClickOnCheckerAsWhite(object sender, int stack, int number)
        {
            int firstFreeDice1, firstFreeDice2;
            if (turn == 2)
            {
                if (!isDice1AlreadyUsed)
                {
                    FindFirstFreeCheckerOnStack(stack - dice1, 2, out firstFreeDice1);
                    if (firstFreeDice1 == -1) // player can get a checker out of the board
                    {
                        board.allCheckers[stack, number].StrokeThickness = 4;
                        board.allCheckers[stack, number].Opacity = 0.5;
                        
                    }
                    else if (firstFreeDice1 == 10)
                    {
                        //THE PLAYER CHOSE A CHECKER WE CAN'T MOVE WITH DICE 1
                        //return;
                    }
                    else
                    {
                        if (firstFreeDice1 == 8) // black checker can eat
                        {

                            board.allCheckers[stack - dice1, 0].StrokeThickness = 4;
                            board.allCheckers[stack - dice1, 0].Opacity = 0.5;
                            
                        }
                        else if (firstFreeDice1 == 7) // stack full, insert to 7
                        {

                        }

                        else // normal possible moves
                        {
                            board.allCheckers[stack - dice1, firstFreeDice1].Visibility = Visibility.Visible;
                            board.allCheckers[stack - dice1, firstFreeDice1].Opacity = 0.5; // half transparent
                            board.allCheckers[stack - dice1, firstFreeDice1].StrokeThickness = 4;
                        }
                        Dice1PossibleMovesShown = true;
                        //return;
                    }
                }


                if (!isDice2AlreadyUsed || (isDoubleMode & (doubleCount > 0)))
                {
                    FindFirstFreeCheckerOnStack(stack - dice2, 2, out firstFreeDice2);
                    if (firstFreeDice2 == -1) // player can get a checker out of the board
                    {
                        board.allCheckers[stack, number].StrokeThickness = 4;
                        board.allCheckers[stack, number].Opacity = 0.5;
                        
                    }
                    else if (firstFreeDice2 == 10)
                    {
                        //THE PLAYER CHOSE A CHECKER WE CAN'T MOVE WITH DICE 2
                        //return;
                        
                    }
                    else
                    {
                        if (firstFreeDice2 == 8) // black checker can eat
                        {
                                                        board.allCheckers[stack - dice2, 0].StrokeThickness = 4;
                            board.allCheckers[stack - dice2, 0].Opacity = 0.5;
                           
                        }
                        else if (firstFreeDice2 == 7) // stack full, insert to 7
                        {
                            
                        }

                        else // normal possible moves
                        {
                            board.allCheckers[stack - dice2, firstFreeDice2].Visibility = Visibility.Visible;
                            board.allCheckers[stack - dice2, firstFreeDice2].Opacity = 0.5; // half transparent
                            board.allCheckers[stack - dice2, firstFreeDice2].StrokeThickness = 4;
                        }
                        Dice2PossibleMovesShown = true;
                        //return;
                    }
                }
            }
        }


        //Stack 0:
        private void Checker00_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker01_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker02_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker03_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker04_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker05_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker06_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        //Stack 1:
        private void Checker10_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker11_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker12_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker13_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker14_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker15_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker16_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker20_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker21_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker22_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker23_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker24_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker25_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker26_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker30_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker31_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker32_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker33_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker34_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker35_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker36_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker40_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker41_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker42_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker43_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker44_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker45_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker46_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker50_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker51_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker52_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker53_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker54_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker55_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker56_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker60_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker61_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker62_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker63_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker64_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker65_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker66_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker70_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker71_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker72_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker73_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker74_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker75_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker76_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker80_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker81_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker82_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker83_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker84_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker85_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker86_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker90_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker91_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker92_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker93_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker94_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker95_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker96_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker100_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker101_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker102_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker103_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker104_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker105_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker106_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker110_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker111_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker112_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker113_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker114_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker115_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker116_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker120_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker121_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker122_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker123_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker124_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker125_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker126_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker130_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker131_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker132_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker133_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker134_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker135_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker136_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker140_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker141_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker142_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker143_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker144_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker145_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker146_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker150_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker151_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker152_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker153_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker154_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker155_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker156_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker160_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker161_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker162_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker163_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker164_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker165_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker166_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker170_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker171_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker172_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker173_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker174_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker175_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker176_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker180_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker181_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker182_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker183_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker184_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker185_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker186_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker190_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker191_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker192_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker193_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker194_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker195_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker196_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker200_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker201_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker202_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker203_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker204_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker205_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker206_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker210_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker211_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker212_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker213_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker214_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker215_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker216_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker220_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker221_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker222_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker223_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker224_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker225_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker226_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker230_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker231_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker232_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker233_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker234_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker235_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker236_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }





        private void CheckerBlackEaten3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void CheckerBlackEaten2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void CheckerBlackEaten1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void CheckerWhiteEaten1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void CheckerWhiteEaten2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void CheckerWhiteEaten3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenEatenClicked(sender);
        }

        private void Checker07_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker17_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker27_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker37_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker47_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker57_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker67_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker77_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker87_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker97_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker107_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker117_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker237_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker227_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker217_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker207_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker197_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker187_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker177_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker167_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker157_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker147_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker137_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }

        private void Checker127_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhenClicked(sender);
        }
    }


    // REAL RANDOM FOR THE DICES
    public static class IntUtil
    {
        private static Random random;

        private static void Init()
        {
            if (random == null) random = new Random();
        }

        public static int Random(int min, int max)
        {
            Init();
            return random.Next(min, max);
        }
    }

}


