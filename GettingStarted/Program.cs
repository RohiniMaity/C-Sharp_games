// See https://aka.ms/new-console-template for more information
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Snake
{
    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    internal class Coord
    {
        private int x;
        private int y;

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
                return false;

            Coord other = (Coord)obj;
            return x == other.x && y == other.y;
        }

        public void ApplyMovementDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    x--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Up:
                    y--;
                    break;
                case Direction.Down:
                    y++;
                    break;
            }
        }
    }
}

namespace Giraffe
{
    class Program
    {
        static string[] grid = new string[9] {"1", "2", "3", "4", "5", "6", "7", "8", "9"};
        static bool isPlayer1 = true;
        static int numTurns = 0;
        static void getCalculator()
        {
            int num1, num2, ans;
            Console.WriteLine("Enter 2 number");
            num1 = Convert.ToInt32(Console.ReadLine());
            num2 = Convert.ToInt32(Console.ReadLine());

            string op;

            Console.WriteLine("Enter operation");
            op = Console.ReadLine();

            if( op == "+")
            {
                ans = num1+num2;
            }
            else if(op == "-")
            {
                ans = num1-num2;
            }
            else if(op =="*")
            {
                ans = num1*num2;
            }
            else
            {
                ans = num1/num2;
            }
            Console.WriteLine("{0}", ans);
        }

        static void getDiceRoll()
        {
            int player1, player2;
            Random ran = new Random();
            for (int i=0; i<10; i++)
            {
                Console.WriteLine("Press key to roll dice");
                Console.ReadKey();
                player1 = ran.Next(1,7);
                player2 = ran.Next(1,7);
                Console.WriteLine("Player 1 has rolled: {0}\nPlayer 2 has rolled: {1}", player1, player2);
            }
        }

        static void getTicTacToe()
        {
            while(!CheckVictory() && numTurns != 9)
            {
                PrintGrid();

                if(isPlayer1)
                    Console.WriteLine("Player 1 turn");
                else
                    Console.WriteLine("Player 2 turn");
                
                string choice = Console.ReadLine();

                if(grid.Contains(choice) && choice != "X" && choice != "O")
                {
                    int gridIndex = Convert.ToInt32(choice) - 1;
                    if(isPlayer1)
                        grid[gridIndex] = "X";
                    else
                        grid[gridIndex] = "O";
                    numTurns++;
                    Console.WriteLine(numTurns);
                }
                isPlayer1 = !isPlayer1;
            }
            if (CheckVictory())
                Console.WriteLine("You win!");
            else
                Console.WriteLine("Tie!");
        }

        static void PrintGrid()
        {
            for (int i=0;i<3;i++)
            {
                for (int j=0;j<3;j++)
                {
                    Console.Write(grid[i*3+j] + "|");
                }
                Console.WriteLine();
                Console.WriteLine("--------");
            }
        }

        static bool CheckVictory()
        {
            bool row1 = grid[0] == grid[1] && grid[1] == grid[2];
            bool row2 = grid[3] == grid[4] && grid[4] == grid[5];
            bool row3 = grid[6] == grid[7] && grid[7] == grid[8];
            bool col1 = grid[0] == grid[3] && grid[3] == grid[6];
            bool col2 = grid[1] == grid[4] && grid[4] == grid[7];
            bool col3 = grid[2] == grid[5] && grid[5] == grid[8];
            bool diagDown = grid[0] == grid[4] && grid[4] == grid[8];
            bool diagUp = grid[6] == grid[4] && grid[4] == grid[2];

            return row1 || row2 || row3 || col1 || col2 || col3 || diagDown || diagUp;
        }

        static void getGuessGame()
        {
            string input, expected;
            expected = "giraffe";
            while(true)
            {
                Console.WriteLine("What is your guess?");
                input = Console.ReadLine();

                if( input == expected)
                {
                    Console.WriteLine("You win");
                    return;
                }
                else
                    Console.WriteLine("Try again");
            }
        }
        static void getSnakeGame()
        {
            Random random = new Random();
            Snake.Coord gridDimensions = new Snake.Coord(50, 20);

            Snake.Direction movementDirection = Snake.Direction.Down;
            Snake.Coord snakePos = new Snake.Coord(10, 0);
            Snake.Coord applePos = new Snake.Coord(random.Next(1, gridDimensions.X-1), random.Next(1, gridDimensions.Y-1));
            List<Snake.Coord> snakePosHistory = new List<Snake.Coord>();
            int frameDelayMilli = 10;
            int score = 0;
            int tailLength = 1;

            while(true)
            {
                Console.Clear();
                Console.WriteLine("Score : " + score);
                snakePos.ApplyMovementDirection(movementDirection);
                for(int y=0; y<gridDimensions.Y; y++)
                {
                    for(int x=0; x<gridDimensions.X; x++)
                    {
                        Snake.Coord currentCord = new Snake.Coord(x,y);
                        if(snakePos.Equals(currentCord) || snakePosHistory.Contains(currentCord))
                            Console.Write("■");
                        else if(applePos.Equals(currentCord))
                            Console.Write("a");
                        else if(x==0 || y ==0 || x==gridDimensions.X-1 || y==gridDimensions.Y-1)
                            Console.Write("#");
                        else
                            Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                if(snakePos.Equals(applePos))
                {
                    tailLength++;
                    score++;
                    applePos = new Snake.Coord(random.Next(1, gridDimensions.X-1), random.Next(1, gridDimensions.Y-1));
                }
                else if( snakePos.X == 0 || snakePos.Y == 0 || snakePos.X == gridDimensions.X-1 || snakePos.Y == gridDimensions.Y-1 || snakePosHistory.Contains(snakePos))
                {
                    score = 0;
                    tailLength = 1;
                    snakePos = new Snake.Coord(10,1);
                    snakePosHistory.Clear();
                    movementDirection = Snake.Direction.Down;
                    continue;
                }

                snakePosHistory.Add(new Snake.Coord(snakePos.X, snakePos.Y));
                if(snakePosHistory.Count > tailLength)
                    snakePosHistory.RemoveAt(0);

                DateTime dateTime = DateTime.Now;
                while((DateTime.Now - dateTime).Milliseconds < frameDelayMilli)
                {
                    if(Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey().Key;
                        switch(key)
                        {
                            case ConsoleKey.LeftArrow:
                                movementDirection = Snake.Direction.Left;
                                break;
                            case ConsoleKey.RightArrow:
                                movementDirection = Snake.Direction.Right;
                                break;
                            case ConsoleKey.UpArrow:
                                movementDirection = Snake.Direction.Up;
                                break;
                            case ConsoleKey.DownArrow:
                                movementDirection = Snake.Direction.Down;
                                break;
                        }
                    }
                }
            }
        }

        static void getHangman()
        {
            int maxLives = 7;
            int currLives = maxLives;
            string secret_word = "happy";
            int len = secret_word.Length;
            bool win = false;
            List<char> letters = new List<char>();

            while(currLives>0 && !win)
            {
                foreach(char c in secret_word)
                {
                    if (letters.Contains(c))
                        Console.Write(c);
                    else
                        Console.Write("_");
                }
                Console.WriteLine("\nPlease guess letter!");
                Console.WriteLine(currLives + "/" + maxLives + "Lives Remaining");

                char guess = Convert.ToChar(Console.ReadLine());
                if (secret_word.Contains(guess) && !letters.Contains(guess))
                    Console.WriteLine("Correct");
                else
                {
                    Console.WriteLine("Incorrect");
                    currLives--;
                }
                letters.Add(guess);

                bool wordComplete = true;
                foreach(char c in secret_word)
                    if(!letters.Contains(c))
                        wordComplete = false;
                win = wordComplete;
            }

            if(win)
                Console.WriteLine("Congratulations");
            else
                Console.WriteLine("You loose");

        }

        static void Main(string[] args)
        {
            int c;
            Console.WriteLine("Choose what you want \n 1. Calculator \n 2. Dice Roll \n 3. TicTacToe \n 4. Guessing Game \n 5. Snake Game \n 6. Hangman ");
            c = Convert.ToInt32(Console.ReadLine());
            if(c==1)
                getCalculator();
            else if(c==2)
                getDiceRoll();
            else if(c==3)
                getTicTacToe();
            else if(c==4)
                getGuessGame();
            else if(c==5)
                getSnakeGame();
            else if(c==6)
                getHangman();
        }
        
    }
}
