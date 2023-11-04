namespace ChessGame
{
    internal class Pawn : Piece  
    {
        protected bool begin = true;

        public Pawn(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "Pawn";
            int[] ints = new int[2];
            ints[0] = coordinates[0];
            if (color == "Light")
            {
                ints[1] = 2;
                allowed.Add(ints);
                ints = new int[2];
                ints[0] = coordinates[0];
                ints[1] = 3;
                allowed.Add(ints);
            }
            else
            {
                ints[1] = 6;
                allowed.Add(ints);
                ints = new int[2];
                ints[0] = coordinates[0];
                ints[1] = 5;
                allowed.Add(ints);
            }
        }

        public void UpdateAllow()
            // Update the allowed moves
        {
            if (last[2] == coordinates[0] && last[3] == coordinates[1]) {  begin = false; }
            List<int[]> ints = new List<int[]>();
            int[] ints1 = new int[2];
            int x = coordinates[0]; int y = coordinates[1];
            if (color == "Light")
            {
                ints1[1] = y + 1;
                ints1[0] = x - 1;
                
                if (ints1[0] >= 0 && ints1[1] <= 7) 
                {
                    if (!IsEmpty(ints1[0], ints1[1]) && PieceAt(ints1[0], ints1[1]).GetColor() != color) 
                    { 
                        ints.Add(ints1);
                    }
                    if (IsEmpty(ints1[0], ints1[1]) && ints1[1] == 5)
                    {
                        if (last[0] == ints1[0] && last[1] == 6 && last[2] == ints1[0] && last[3] == 4)
                        {
                            ints.Add(ints1);
                        }
                    }
                }

                ints1 = new int[2];
                ints1[1] = y + 1;
                ints1[0] = x + 1;
                if (ints1[0] <= 7 && ints1[1] <= 7)
                {
                    if (!IsEmpty(ints1[0], ints1[1]) && PieceAt(ints1[0], ints1[1]).GetColor() != color)
                    {
                        ints.Add(ints1);
                    }
                    if (IsEmpty(ints1[0], ints1[1]) && ints1[1] == 5)
                    {
                        if (last[0] == ints1[0] && last[1] == 6 && last[2] == ints1[0] && last[3] == 4)
                        {
                            ints.Add(ints1);
                        }
                    }
                }

                ints1 = new int[2];
                ints1[1] = y + 1;
                ints1[0] = x;
                if (ints1[1] <= 7)
                {
                    if (IsEmpty(ints1[0], ints1[1]))
                    {
                        ints.Add(ints1);
                        if (begin)
                        {
                            ints1 = new int[2];
                            ints1[1] = y + 2;
                            ints1[0] = x;
                            if (ints1[1] <= 7)
                            {
                                if (IsEmpty(ints1[0], ints1[1]))
                                {
                                    ints.Add(ints1);
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                ints1[1] = y - 1;
                ints1[0] = x - 1;
                if (ints1[0] >= 0 && ints1[1] >= 0)
                {
                    if (!IsEmpty(ints1[0], ints1[1]) && PieceAt(ints1[0], ints1[1]).GetColor() != color)
                    {
                        ints.Add(ints1);
                    }
                    if (IsEmpty(ints1[0], ints1[1]) && ints1[1] == 2)
                    {
                        if (last[0] == ints1[0] && last[1] == 1 && last[2] == ints1[0] && last[3] == 3)
                        {
                            ints.Add(ints1);
                        }
                    }
                }

                ints1 = new int[2];
                ints1[1] = y - 1;
                ints1[0] = x + 1;
                if (ints1[0] <= 7 && ints1[1] >= 0)
                {
                    if (!IsEmpty(ints1[0], ints1[1]) && PieceAt(ints1[0], ints1[1]).GetColor() != color)
                    {
                        ints.Add(ints1);
                    }
                    if (IsEmpty(ints1[0], ints1[1]) && ints1[1] == 2)
                    {
                        if (last[0] == ints1[0] && last[1] == 1 && last[2] == ints1[0] && last[3] == 3)
                        {
                            ints.Add(ints1);
                        }
                    }
                }

                ints1 = new int[2];
                ints1[1] = y - 1;
                ints1[0] = x;
                if (ints1[1] >= 0)
                {
                    if (IsEmpty(ints1[0], ints1[1]))
                    {
                        ints.Add(ints1);
                        if (begin)
                        {
                            ints1 = new int[2];
                            ints1[1] = y - 2;
                            ints1[0] = x;
                            if (ints1[1] >= 0)
                            {
                                if (IsEmpty(ints1[0], ints1[1]))
                                {
                                    ints.Add(ints1);
                                }
                            }
                        }
                    }
                }
            }

            allowed = ints;
        }
    }
}