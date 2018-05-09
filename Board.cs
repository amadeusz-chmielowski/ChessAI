using ChessAI.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessAI
{
	class Board
	{
		struct Move
		{
			public Point from;
			public Point to;
			public bool hasTaken;
			public bool hasPromoted;
			public bool hasEnPassanted;
		};

		// members
		public Piece[,] BoardTab { get; private set; }
		private Player whitePlayer, blackPlayer;
        private double fiftyMoveRule;
        private bool threeholdRepetition;
        public bool CanDraw { get { return fiftyMoveRule >= 50 || threeholdRepetition; } } 
		public int Turns { get; private set; }
		public Point LatestMoved { get { return history.Count != 0 ? history.Peek().to : null; } }
		public Win GameState { get; private set; }
		private Stack<Move> history;
		private Stack<Piece> deletedPieces;
        private Queue<double> whiteEvals;
        private Queue<double> blackEvals;
        // constructors
        public Board(Player a, Player b)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Turns = 0;
            fiftyMoveRule = 0;
            threeholdRepetition = false;
			history = new Stack<Move>();
			deletedPieces = new Stack<Piece>();
			whitePlayer = a.color == Color.White ? a : b;
			blackPlayer = a.color != Color.White ? a : b;
			BoardTab = new Piece[8, 8];

			BoardTab[0, 0] = new Rook(Color.White); BoardTab[7, 0] = new Rook(Color.White);
			BoardTab[0, 7] = new Rook(Color.Black); BoardTab[7, 7] = new Rook(Color.Black);

			BoardTab[2, 0] = new Bishop(Color.White); BoardTab[5, 0] = new Bishop(Color.White);
			BoardTab[2, 7] = new Bishop(Color.Black); BoardTab[5, 7] = new Bishop(Color.Black);

			BoardTab[1, 0] = new Knight(Color.White); BoardTab[6, 0] = new Knight(Color.White);
			BoardTab[1, 7] = new Knight(Color.Black); BoardTab[6, 7] = new Knight(Color.Black);

			BoardTab[3, 0] = new Queen(Color.White); BoardTab[3, 7] = new Queen(Color.Black);

			BoardTab[4, 0] = new King(Color.White); BoardTab[4, 7] = new King(Color.Black);

			for (int i = 0; i < 8; i++)
			{
				BoardTab[i, 1] = new Pawn(Color.White);
				BoardTab[i, 6] = new Pawn(Color.Black);
			}
		}

		// methods
		public void Print(Point[] markers)
		{
			Console.Clear();
			for (int height = 0; height < 9; height++)
			{
				//display vertical line over the squares
				for (int width = 0; width < 19; width++)
					Console.Write((width % 2 == 0) ? "+" : "-");
				Console.WriteLine();
				for (int width = -1; width < 8; width++)
				{
					//put horizontal line before square
					Console.Write("|");
					if (markers != null)
					{
						foreach (Point marker in markers)
						{
							if (marker.x == width && marker.y == height)
								Console.BackgroundColor = ConsoleColor.DarkRed;
						}
					}
					if (width == -1)
					{
						//number the rows
						if (height < 8)
							Console.Write(8 - height);
						else
							Console.Write(" ");
					}
					//number the columns
					else if (height == 8)
						Console.Write((char)('a' + width));
					else if (BoardTab[width, height] != null)
					{
						if (BoardTab[width, height].color == Color.Black)
							Console.ForegroundColor = ConsoleColor.DarkGray;
						//display piece
						Console.Write(BoardTab[width, height].letter);

					}
					else Console.Write(' ');
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.White;
					//additional verical line on the very right
					if (width == 7)
						Console.Write("|");
				}
				Console.WriteLine();
				if (height == 8)
				{
					//additional horizontal line on the very bottom
					for (int w = 0; w < 19; w++)
						Console.Write((w % 2 == 0) ? "+" : "-");
					Console.WriteLine();
				}
			}
		}
		public List<Point> PiecesCheckingKing(Color color, bool breakOnFound = false)
		{
			//possible optimalization:
			//1
			//start searching from top for white player and from bottom player
			//stop searching when found nr of pieces they have (maybe keep nr of them somewhere)
			//2
			//save result
			//next time start searching from pieces that where checking
			List<Point> checingPieces = new List<Point>();
            foreach(Point piecePos in GetAllPiecesPositions(color==Color.Black ? Color.White : Color.Black))
            {
                Piece piece = BoardTab[piecePos.x, piecePos.y];
                bool isChecking = piece.GetMoves(this, piecePos).Where(
                            x => { Piece dest = BoardTab[x.x, x.y]; return dest == null ? false : (dest.color == color && dest.letter == 'K'); }
                            ).Count() != 0;
                if (isChecking)
                {
                    checingPieces.Add(piecePos);
                    if (breakOnFound)
                        return checingPieces;
                }
            }
			return checingPieces;
		}
		public void UndoMove(int nrOfMoves)
		{
			if (nrOfMoves <= 0 || history.Count == 0) return;
			Move move = history.Pop();
			//reverse piece position
			Piece reversed = BoardTab[move.from.x, move.from.y] = BoardTab[move.to.x, move.to.y];
			BoardTab[move.to.x, move.to.y] = null;
			Color enemyColor = reversed.color == Color.Black ? Color.White : Color.Black;
			if (move.hasTaken)
			{
				if (move.hasEnPassanted)
				{
					//put pawn back in place
					BoardTab[move.to.x, move.from.y] = new Pawn(enemyColor)
					{
						moves = 1
					};
				}
				else
				{
					//put piece back
					BoardTab[move.to.x, move.to.y] = deletedPieces.Pop();
				}
			}
			else if (BoardTab[move.from.x, move.from.y].letter == 'K')
			{
				//check for castling
				if (move.to.x - move.from.x == 2)
				{
					//castled right
					BoardTab[7, move.to.y] = BoardTab[move.to.x - 1, move.to.y];
					BoardTab[move.to.x - 1, move.to.y] = null;
				}
				else if (move.to.x - move.from.x == -2)
				{
					//castled left
					BoardTab[0, move.to.y] = BoardTab[move.to.x + 1, move.to.y];
					BoardTab[move.to.x + 1, move.to.y] = null;
				}
			}
			if (move.hasPromoted)
			{
				//change back to pawn
				BoardTab[move.from.x, move.from.y] = new Pawn(reversed.color);
			}
			reversed.moves--;
			UndoMove(nrOfMoves - 1);
		}
		public Win UpdateGameState(Color playerColor)
		{
			//check for checkmate/stalemate and other draw options

			bool isEnemyChecked = PiecesCheckingKing(playerColor == Color.Black ? Color.White : Color.Black, true).Count != 0;
            Color opponentColor = playerColor == Color.Black ? Color.White : Color.Black;

            if (!isEnemyChecked)
			{
				GameState = Win.None;
			}
			else
			{
                if (hasAnyValidMoves(opponentColor))
					GameState = playerColor == Color.Black ? Win.BlackCheck : Win.WhiteCheck;
				else GameState = playerColor == Color.Black ? Win.Black : Win.White;
			}
			//stalemate
			if (GameState == Win.None && !hasAnyValidMoves(opponentColor))
			{
				GameState = Win.Stalemate;
			}
			return GameState;
		}
		public void Execute(Point[] move)
		{
			//save move and update board
			Move moveToSave = new Move();
			if (BoardTab[move[0].x, move[0].y].letter == 'P' && BoardTab[move[1].x, move[1].y] == null && move[0].x != move[1].x)
			{
				moveToSave.hasEnPassanted = true;
				moveToSave.hasTaken = true;
				BoardTab[move[1].x, move[0].y] = null; //en passant
			}
			else moveToSave.hasTaken = moveToSave.hasEnPassanted = false;

			BoardTab[move[0].x, move[0].y].moves++;

			if (BoardTab[move[1].x, move[1].y] != null)
			{
				moveToSave.hasTaken = true;
                whiteEvals.Clear();
                blackEvals.Clear();
                deletedPieces.Push(BoardTab[move[1].x, move[1].y]);
			}
			else moveToSave.hasTaken = false;

            if (BoardTab[move[0].x, move[0].y].letter == 'P' || moveToSave.hasTaken)
                fiftyMoveRule = 0;
            else fiftyMoveRule += 0.5; //whole move is when both players moved
            BoardTab[move[1].x, move[1].y] = BoardTab[move[0].x, move[0].y];
			BoardTab[move[0].x, move[0].y] = null;
			Piece movedPiece = BoardTab[move[1].x, move[1].y];
			Color playerColor = movedPiece.color;
			//castling
			if (movedPiece.letter == 'K' && Math.Abs(move[0].x - move[1].x) == 2)
			{
				Point[] newMove = new Point[2] {
					new Point((move[1].x - move[0].x) > 0 ? 7 : 0, move[1].y),
					new Point((move[1].x + move[0].x) / 2, move[1].y)
				};
				Execute(newMove); //to rethink (when checking for win is done)
			}
            //pawn promotion
            if (((move[1].y == 7 && playerColor == Color.White) || (move[1].y == 0 && playerColor == Color.Black))
                && movedPiece.letter == 'P')
            {
                moveToSave.hasPromoted = true;
                BoardTab[move[1].x, move[1].y] = ((Pawn)BoardTab[move[1].x, move[1].y]).Promote();
            }
            else moveToSave.hasPromoted = false;
			moveToSave.from = move[0];
			moveToSave.to = move[1];
			history.Push(moveToSave);
		}
		public double EvaluatePlayerPosition(Color playerColor)
		{
            double currentEval = 0;
            double maxvalue = 0;
            double result = 0;
            List<Point> listOfPieces = GetAllPiecesPositions(playerColor);
            foreach (var piece in listOfPieces)
            {
                currentEval += BoardTab[piece.x, piece.y].whiteArrayPiecePosition[piece.x,piece.y];
                maxvalue += BoardTab[piece.x, piece.y].maxValueAtPosition;
            }
            result = currentEval / maxvalue;
            if (result < 0)
            {
                return (double)0;
            }
            else
            {
                return result;
            }
            
        }

        public double EvaluatePlayerPieces(Color playerColor)
        {
            double currentEval = 0;
            List<Point> listOfPieces = GetAllPiecesPositions(playerColor);
            foreach (var piece in listOfPieces)
            {
                currentEval += BoardTab[piece.x, piece.y].valueOfPiece;
            }
            return currentEval/(double)24000;
        }
		public Win ExecuteTurn()
		{
			Turns++;
            double currEval;
            int reps;
            try
            {
                currEval = EvaluatePlayerPosition(whitePlayer.color);
                reps = 0;
                foreach (double eval in whiteEvals)
                {
                    if (eval == currEval)
                        reps++;
                }
                whiteEvals.Enqueue(currEval);
                if (reps == 2)
                {
                    //check back history if these are actually exact same positions
                    //if so - to speed up we might just want to ignore it
                    threeholdRepetition = true;
                }
                else if (reps == 4)
                {
                    //... as above
                    GameState = Win.Stalemate;
                    return Win.Stalemate;
                }
            }
            catch(NotImplementedException)
            { }
            try
			{
				Execute(whitePlayer.Decide(this));
				UpdateGameState(Color.White);
				if (GameState == Win.White || GameState == Win.Stalemate)
					return GameState;
            }
            catch (QuitException)
            {
                GameState = Win.Black;
                return GameState;
            }
            catch (DeclareDrawException)
            {
                GameState = Win.Stalemate;
                return GameState;
            }
            catch (ReverseException)
			{
				Turns--;
				UndoMove(1);
			}

            try
            {
                currEval = EvaluatePlayerPosition(blackPlayer.color);
                reps = 0;
                foreach (double eval in blackEvals)
                {
                    if (eval == currEval)
                        reps++;
                }
                blackEvals.Enqueue(currEval);
                if (reps == 2)
                {
                    //check back history if these are actually exact same positions
                    //if so - to speed up we might just want to ignore it
                    threeholdRepetition = true;
                }
                else if (reps == 4)
                {
                    //... as above
                    GameState = Win.Stalemate;
                    return Win.Stalemate;
                }
            }
            catch(NotImplementedException)
            { }
            try
			{
				Execute(blackPlayer.Decide(this));
				UpdateGameState(Color.Black);
			}
			catch (QuitException)
			{
				GameState = Win.White;
				return GameState;
            }
            catch (DeclareDrawException)
            {
                GameState = Win.Stalemate;
                return GameState;
            }
            catch (ReverseException)
			{
				Turns--;
				UndoMove(1);

			}
			return GameState;
		}
		public bool MatchEnded()
		{
			switch (GameState)
			{
				case Win.Black:
				case Win.White:
				case Win.Stalemate:
					return true;
				default:
					return false;
			}
		}
		public void PrintMatchResult()
		{
			Console.Write("Game Result: ");
			switch (GameState)
			{
				case Win.Black:
					Console.WriteLine("Black player has won");
					break;
				case Win.White:
					Console.WriteLine("White player has won");
					break;
				case Win.Stalemate:
					Console.WriteLine("Stalemate");
					break;
			}
			Console.Read();
		}
		public List<Point> GetAllPiecesPositions(Color playerColor)
		{
			var list = new List<Point>();
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (BoardTab[i, j] == null)
						continue;
					if (BoardTab[i, j].color == playerColor)
					{
						list.Add(new Point(i, j));
					}
				}
			}
			return list;
		}
		public bool hasAnyValidMoves(Color playerColor)
		{
			List<Point> listOfPieces = GetAllPiecesPositions(playerColor);
			List<Point> validMoves;
			foreach (var piece in listOfPieces)
			{
				validMoves = BoardTab[piece.x, piece.y].GetValidMoves(this, piece);
				if (validMoves.Count != 0)
					return true;
			}
			return false;
		}
	}
}
