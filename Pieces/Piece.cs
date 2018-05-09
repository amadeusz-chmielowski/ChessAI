using System.Collections.Generic;

namespace ChessAI
{
	abstract class Piece
	{
		public int moves { get; set; }
		public Color color { get; set; }

        public abstract int valueOfPiece { get;}
        public abstract int[,] whiteArrayPiecePosition { get; }
        public abstract int[,] blackArrayPiecePosition { get; }
        public abstract int maxValueAtPosition { get; }

        public abstract char letter { get; }

		public Piece(Color color)
		{
			moves = 0;
			this.color = color;
		}
		public Piece()
		{
			moves = 0;
		}

		public List<Point> GetValidMoves(Board board, Point myPos)
		{
			List<Point> possibleMoves = GetMoves(board, myPos);
			//additional validation for all pieces
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				//would this move cause my king to be checked
				Point[] mov = { myPos, possibleMoves[i] };
				board.Execute(mov); //but don't check for check
				bool check = board.PiecesCheckingKing(color, true).Count != 0;
				board.UndoMove(1);
				if (check)
				{
					possibleMoves.Remove(possibleMoves[i]);
					i--;
				}
			}
			return possibleMoves;
		}
		public abstract List<Point> GetMoves(Board board, Point myPos);

	}
}
