using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAI
{
	class AlphaBeta : Player
	{
		public AlphaBeta(Color color) : base(color)
		{
		}

		public override Point[] Decide(Board board)
		{
			throw new NotImplementedException();
		}
	}
}
