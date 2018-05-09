using System;

namespace ChessAI
{
	class Program
	{
		static void Main(string[] args)
		{
			Player A = new Human(Color.White);
			Player B = new Human(Color.Black);
			Board board = new Board(A, B);

			do
			{
				board.ExecuteTurn();
			} while (!board.MatchEnded());

			Console.Clear();
			board.PrintMatchResult();
		}
	}
}
