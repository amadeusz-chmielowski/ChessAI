
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAI
{
	class Human : Player
	{
		public Human(Color color) : base(color) { }
		private Point GetPiece(string message, bool canDraw)
		{
			string piece;
			while (true)
			{
				Console.WriteLine(message);
				piece = Console.ReadLine();

                if (piece.Equals("-q"))
                    throw new QuitException();
                else if (piece.Equals("-r"))
                    throw new ReverseException();
                else if (piece.Equals("-d"))
                {
                    if (canDraw)
                        throw new DeclareDrawException();
                    else
                    {
                        Console.WriteLine("Nie możesz wymagać remisu");
                        continue;
                    }
                }
                else if (piece.Length < 2 || piece[0] < 'a' || piece[0] > 'h' || piece[1] < '0' || piece[1] > '9')
                {
                    Console.WriteLine("Złe polecenie");
                    continue;
                }
				break;
			}
			return new Point(piece[0] - 'a', 8 - (piece[1] - '0'));

		}
		public override Point[] Decide(Board board)
		{
			board.Print(null);
			Console.WriteLine("Tura nr" + board.Turns + " " + (color == Color.White ? "Białych" : "Czarnych"));
			Point[] playerChoice = new Point[2];
			Piece onBoard;
			Point[] availableMoves;

			while (true)
			{
				playerChoice[0] = GetPiece("Wybierz bierkę", board.CanDraw);

				onBoard = board.BoardTab[playerChoice[0].x, playerChoice[0].y];
				if (onBoard == null || onBoard.color != color)
				{
					//not your piece
					Console.WriteLine("To nie jest twoja bierka");
				}
				else
				{
					availableMoves = onBoard.GetValidMoves(board, playerChoice[0]).ToArray();
					if (availableMoves.Length == 0)
					{
						Console.WriteLine("Ta bierka nie ma dostępnych ruchów, wybierz inną");
					}
					else break;
				}
			}
			board.Print(availableMoves);
			while (true)
			{
				Console.WriteLine("Wybrano bierkę: " + (char)('a' + playerChoice[0].x) + (8 - playerChoice[0].y).ToString());
				Console.WriteLine("Dostępne ruchy");
				foreach (Point move in availableMoves)
				{
					Console.Write((char)('a' + move.x) + (8 - move.y).ToString() + " ");
				}
				Console.WriteLine();
				playerChoice[1] = GetPiece("Ustaw wybraną bierkę", board.CanDraw);
				if (!Array.Exists(availableMoves, x => x.x == playerChoice[1].x && x.y == playerChoice[1].y))
				{
					Console.WriteLine("Nie możesz wykonać takiego ruchu");
				}
				else break;
			}
			return playerChoice;
		}
	}
}
