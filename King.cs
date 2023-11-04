namespace ChessGame
{
    internal class King : Piece
    {
        protected bool begin = true;

        public King(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "King";
        }

        public void UpdateAllow()
            // Update the allowed moves
        {
            if (last[2] == coordinates[0] && last[3] == coordinates[1]) { begin = false; }
            List<int[]> ints = new List<int[]>();
            int x = coordinates[0]; int y = coordinates[1];
            for(int i=-1; i<2; i++) 
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i==0 && j==0) && 0 <= (x + i) && (x + i) <= 7 && 0 <= (y + j) && (y + j) <= 7)
                    {
                        if (IsEmpty(x + i, y + j) || (PieceAt(x + i, y + j).GetColor() != color && !PieceAt(x + i, y + j).IsProtected()))
                        {
                            int[] ints1 = new int[2];
                            ints1[0] = x+i; ints1[1] = y+j;
                            ints.Add(ints1);
                        }
                    }
                }
            }
            if (begin) // Castling cases
            {
                if (PieceAt(0, coordinates[1]).GetName() == "Rook")
                {
                    Rook tour = (Rook)PieceAt(0, coordinates[1]);
                    if (!tour.HasMoved() && IsEmpty(1, coordinates[1]) && IsEmpty(2, coordinates[1]) && IsEmpty(3, coordinates[1]))
                    {
                        int[] ints1 = new int[2];
                        ints1[0] = 2; ints1[1] = coordinates[1];
                        ints.Add(ints1);
                    }
                }

                if (PieceAt(7, coordinates[1]).GetName() == "Rook")
                {
                    Rook tour = (Rook)PieceAt(7, coordinates[1]);
                    if (!tour.HasMoved() && IsEmpty(5, coordinates[1]) && IsEmpty(6, coordinates[1]))
                    {
                        int[] ints1 = new int[2];
                        ints1[0] = 6; ints1[1] = coordinates[1];
                        ints.Add(ints1);
                    }
                }
            }
            allowed = ints;
        }
    }
}