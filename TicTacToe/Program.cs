using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SFML_Test
{
    class Program
    {
        static Board board = new Board(new Vector2f(3, 3));
        static bool isCross = true;
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void Main()
        {
            RenderWindow window = new RenderWindow(new VideoMode(900, 900), "SFML Works!");
            window.Closed += new EventHandler(OnClose);
            window.MouseButtonPressed += OnMouseClick;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                board.DrawBoard(window);
                window.Display();
            }

        }
        static void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            Console.Clear();
            Cell nearestPoint = null;
            Vector2f posFactor = new Vector2f(board.cells[0][0].Size.X / 2, board.cells[0][0].Size.Y / 2);

            for (int i = 0; i < board.cells.Count; i++)
            {
                for (int j = 0; j < board.cells[i].Count; j++)
                {
                    Console.WriteLine(GetDistance(board.cells[i][j].Position + posFactor, new Vector2f(e.X, e.Y)));
                    if (GetDistance(board.cells[i][j].Position + posFactor, new Vector2f(e.X, e.Y)) < 4000)
                    {
                        if (nearestPoint != null)
                        {
                            if (GetDistance(board.cells[i][j].Position + posFactor, new Vector2f(e.X, e.Y)) < GetDistance(nearestPoint.Position + posFactor, new Vector2f(e.X, e.Y)))
                            {
                                nearestPoint = board.cells[i][j];
                            }
                        }
                        else nearestPoint = board.cells[i][j];
                    }
                }
            }
            if (nearestPoint != null)
            {
                nearestPoint.Activate(ref isCross);
            }
        }

        static float GetDistance(Vector2f a, Vector2f b)
        {

            return MathF.Sqrt(Math.Abs(MathF.Pow(b.X - a.X, 2) + MathF.Pow(b.Y - a.Y, 2)));
        }

    }
    class Cell : RectangleShape
    {
        private Color crossColor = Color.Red;
        private Color circleColor = Color.Green;

        public Cell(Vector2f position)
        {
            Position = position;
            FillColor = Color.White;
            Size = new Vector2f(300, 300);
        }

        public void Activate(ref bool isCross)
        {
            FillColor = isCross ? crossColor : circleColor;
            isCross = !isCross;
        }
    }

    class Board
    {
        public List<List<Cell>> cells = new List<List<Cell>>();
        public Board(Vector2f size)
        {
            int posY = 0;

            for (int i = 0; i < 3; i++)
            {
                int posX = 0;
                cells.Add(new List<Cell> ());
                for (int j = 0; j < 3; j++)
                {
                    Cell cell = new Cell(new Vector2f(posX, posY));
                    cells[i].Add(cell);
                    posX += 300;
                }
                posY += 300;
            }
        }

        public void DrawBoard(RenderWindow window)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    window.Draw(cells[i][j]);
                }
            }
        }
    }
}
