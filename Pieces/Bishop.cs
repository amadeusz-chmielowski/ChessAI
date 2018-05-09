using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAI.Pieces
{
    class Bishop : Piece
    {
        // members
        const int size = 8;
        const char Letter = 'B';
        const int MaxValueOfPosition = 10;
        public override int maxValueAtPosition { get { return MaxValueOfPosition; } }

        const int ValueOfPiece = 330;
        int[,] WhiteArrayPiecePosition = new int[8,8]{
            {-20,-10,-10,-10,-10,-10,-10,-20 },
            {-10,  0,  0,  0,  0,  0,  0,-10 },
            {-10,  0,  5, 10, 10,  5,  0,-10 },
            { -10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10 },
            {-10, 10, 10, 10, 10, 10, 10,-10 },
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20 },
        };
        int[,] BlackArrayPiecePosition = new int[8,8] {
            {-20,-10,-10,-10,-10,-10,-10,-20 },
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10 },
            {-10,  0, 10, 10, 10, 10,  0,-10 },
            { -10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10 },
            {-10,  0,  0,  0,  0,  0,  0,-10 },
            {-20,-10,-10,-10,-10,-10,-10,-20 },
        };

        public override int[,] whiteArrayPiecePosition { get { return WhiteArrayPiecePosition; } }

        public override int[,] blackArrayPiecePosition { get { return BlackArrayPiecePosition; } }
        public override char letter { get { return Letter; } }
        public override int valueOfPiece { get { return ValueOfPiece; } }

      


        // constructors
        public Bishop() { }
        public Bishop(Color color) : base(color) { }

        //useful in all pieces, that can move in lines -> add  abstrac class for that?
        private bool AddToList(List<Point> list, Board board, int x, int y)
        {

            //null -> add, continue
            //color=mycolor -> stop
            //color!=mycolor -> add, stop
            if (board.BoardTab[x, y] == null)
                list.Add(new Point(x, y));
            else if (board.BoardTab[x, y].color == color)
                return false;
            else
            {
                list.Add(new Point(x, y));
                return false;
            }
            return true;
        }
        public override List<Point> GetMoves(Board board, Point myPos)
        {
            List<Point> list = new List<Point>();
            if (myPos.y < 7 && myPos.x > 0) //down left
            {
                for (int i = 1; myPos.y + i <= 7 && myPos.x - i >= 0; i++)
                {
                    if (!AddToList(list, board, myPos.x - i, myPos.y + i))
                        break;
                }
            }
            if (myPos.y < 7 && myPos.x < 7)//down right
            {
                for (int i = 1; myPos.y + i <= 7 && myPos.x + i <= 7; i++)
                {
                    if (!AddToList(list, board, myPos.x + i, myPos.y + i))
                        break;
                }
            }
            if (myPos.y > 0 && myPos.x > 0)//up left
            {
                for (int i = 1; myPos.x - i >= 0 && myPos.y - i >= 0; i++)
                {
                    if (!AddToList(list, board, myPos.x - i, myPos.y - i))
                        break;
                }
            }
            if (myPos.y > 0 && myPos.x < 7)//up right
            {
                for (int i = 1; myPos.y - i >= 0 && myPos.x + i <= 7; i++)
                {
                    if (!AddToList(list, board, myPos.x + i, myPos.y - i))
                        break;
                }
            }
            return list;
        }
    }
}
