using Microsoft.VisualBasic;

namespace ChessGame
{
    internal class Knight : Piece
    {
        public Knight(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "Knight";
            int[] ints = new int[2];
            ints[0] = coordinates[0] - 1;
            if (color == "Light")
            {
                ints[1] = coordinates[1] + 2;
                allowed.Add(ints);
                ints = new int[2];
                ints[0] = coordinates[0] + 1;
                ints[1] = coordinates[1] + 2;
                allowed.Add(ints);
            }

            else
            {
                ints[1] = coordinates[1] - 2;
                allowed.Add(ints);
                ints = new int[2];
                ints[0] = coordinates[0] + 1;
                ints[1] = coordinates[1] - 2;
                allowed.Add(ints);
            }

        }

        public void UpdateAllow()
            //Update the allowed moves
        {
            List<int[]> ints = new List<int[]>();
            int[] ints1 = new int[2];
            ints1[0] = coordinates[0] - 2;
            ints1[1] = coordinates[1] - 1;
            if (ints1[0] >= 0 && ints1[1] >= 0)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[0] = coordinates[0] - 2;
            ints1[1] = coordinates[1] + 1;
            if (ints1[0] >= 0 && ints1[1] <= 7)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[0] = coordinates[0] + 2;
            ints1[1] = coordinates[1] - 1;
            if (ints1[0] <= 7 && ints1[1] >= 0)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[0] = coordinates[0] + 2;
            ints1[1] = coordinates[1] + 1;
            if (ints1[0] <= 7 && ints1[1] <= 7)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[1] = coordinates[1] - 2;
            ints1[0] = coordinates[0] - 1;
            if (ints1[1] >= 0 && ints1[0] >= 0)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[1] = coordinates[1] - 2;
            ints1[0] = coordinates[0] + 1;
            if (ints1[1] >= 0 && ints1[0] <= 7)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[1] = coordinates[1] + 2;
            ints1[0] = coordinates[0] - 1;
            if (ints1[1] <= 7 && ints1[1] >= 0)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            ints1 = new int[2];
            ints1[1] = coordinates[1] + 2;
            ints1[0] = coordinates[0] + 1;
            if (ints1[1] <= 7 && ints1[0] <= 7)
            {
                if (IsEmpty(ints1[0], ints1[1]) || PieceAt(ints1[0], ints1[1]).GetColor() != color)
                {
                    ints.Add(ints1);
                }
            }

            allowed = ints;
        }
    }
}