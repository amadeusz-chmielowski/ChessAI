using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAI.Pieces
{
    class Knight : Piece
    {
        const char Letter = 'N';

        const int ValueOfPiece = 320;
        const int MaxValueOfPosition = 20;
        public override int maxValueAtPosition { get { return MaxValueOfPosition; } }
        int[,] WhiteArrayPiecePosition = new int[8, 8]{
           { -50,-40,-30,-30,-30,-30,-40,-50 },
           { -40,-20,  0,  0,  0,  0,-20,-40 },
           { -30,  0, 10, 15, 15, 10,  0,-30 },
           { -30,  5, 15, 20, 20, 15,  5,-30 },
           { -30,  0, 15, 20, 20, 15,  0,-30 },
           { -30,  5, 10, 15, 15, 10,  5,-30 },
           { -40,-20,  0,  5,  5,  0,-20,-40 },
           { -50,-40,-30,-30,-30,-30,-40,-50 },
        };
        int[,] BlackArrayPiecePosition = new int[8, 8] {
           { -50,-40,-30,-30,-30,-30,-40,-50 },
           { -40,-20,  0,  0,  0,  0,-20,-40 },
           { -30,  0, 10, 15, 15, 10,  0,-30 },
           { -30,  5, 15, 20, 20, 15,  5,-30 },
           { -30,  0, 15, 20, 20, 15,  0,-30 },
           { -30,  5, 10, 15, 15, 10,  5,-30 },
           { -40,-20,  0,  5,  5,  0,-20,-40 },
           { -50,-40,-30,-30,-30,-30,-40,-50 },
        };

        public override int[,] whiteArrayPiecePosition { get { return WhiteArrayPiecePosition; } }

        public override int[,] blackArrayPiecePosition { get { return BlackArrayPiecePosition; } }
        public Knight() { }

        public Knight(Color color) : base(color) { }

        public override char letter { get { return Letter; } }
        public override int valueOfPiece { get { return ValueOfPiece; } }
        public override List<Point> GetMoves(Board board, Point myPos)
        {
            List<Point> list = new List<Point>();
            int x = 2, y = 1;
            for (int i = 0; i < 8; i++)
            {
                //start magic
                x ^= y;
                y ^= x;
                x ^= y;
                y *= i != 4 ? -1 : 1;
                //end magic
                if (myPos.x + x < 8 && myPos.x + x >= 0
                        && myPos.y + y < 8 && myPos.y + y >= 0
                        && (board.BoardTab[myPos.x + x, myPos.y + y] == null || board.BoardTab[myPos.x + x, myPos.y + y].color != color))
                    list.Add(new Point(myPos.x + x, myPos.y + y));
            }
            return list;
        }
    }
}
