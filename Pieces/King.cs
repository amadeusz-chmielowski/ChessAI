using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAI.Pieces
{
	class King : Piece
	{
		const char Letter = 'K';
        const int ValueOfPiece = 20000;
        const int MaxValueOfPosition = 30;
        public override int maxValueAtPosition { get { return MaxValueOfPosition; } }
        int[,] WhiteArrayPiecePosition = new int[8, 8]{
            {-30,-40,-40,-50,-50,-40,-40,-30 },
            {-30,-40,-40,-50,-50,-40,-40,-30 },
            {-30,-40,-40,-50,-50,-40,-40,-30 },
            { -30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20 },
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {20, 30, 10,  0,  0, 10, 30, 20},
        };
        int[,] BlackArrayPiecePosition = new int[8, 8] {
            {20, 30, 10,  0,  0, 10, 30, 20},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {-20,-30,-30,-40,-40,-30,-30,-20 },
            { -30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30 },
            {-30,-40,-40,-50,-50,-40,-40,-30 },
            {-30,-40,-40,-50,-50,-40,-40,-30 },
        };

        public override int[,] whiteArrayPiecePosition { get { return WhiteArrayPiecePosition; } }

        public override int[,] blackArrayPiecePosition { get { return BlackArrayPiecePosition; } }
        public King(Color color) : base(color) { }
		public King() : base() { }
		public override char letter { get { return Letter; } }
        public override int valueOfPiece { get { return ValueOfPiece; } }

        public override List<Point> GetMoves(Board board, Point myPos)
		{
			List<Point> list = new List<Point>();
			int xMin = myPos.x > 1 ? myPos.x - 1 : myPos.x;
			int xMax = myPos.x < 7 ? myPos.x + 1 : myPos.x;
			int yMin = myPos.y > 1 ? myPos.y - 1 : myPos.y;
			int yMax = myPos.y < 7 ? myPos.y + 1 : myPos.y;
			for (int x = xMin; x <= xMax; x++)
			{
				for (int y = yMin; y <= yMax; y++)
				{
					if (board.BoardTab[x, y] == null)
					{
						list.Add(new Point(x, y));
					}
				}
			}
			if (moves == 0) //king hasn't moved
			{
				if (board.BoardTab[0, myPos.y] != null && board.BoardTab[0, myPos.y].moves == 0) //there is rook on the left, that hasn't moved
				{
					bool isEmpty = true;
					for (int i = 1; i < myPos.x; i++) //all spots on the way are empty
					{
						if (board.BoardTab[i, myPos.y] != null)
						{
							isEmpty = false;
							break;
						}
					}
					if (isEmpty)
						list.Add(new Point(myPos.x - 2, myPos.y));
				}
				if (board.BoardTab[7, myPos.y] != null && board.BoardTab[7, myPos.y].moves == 0)
				{
					bool isEmpty = true;
					for (int i = myPos.x + 1; i < 7; i++) //all spots on the way are empty
					{
						if (board.BoardTab[i, myPos.y] != null)
						{
							isEmpty = false;
							break;
						}
					}
					if (isEmpty)
						list.Add(new Point(myPos.x + 2, myPos.y));
				}
			}
			return list;
		}
	}
}
