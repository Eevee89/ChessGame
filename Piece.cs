using System.Text;

namespace ChessGame
{
    internal class Piece : IComparable<Piece>
    {
        protected int[] coordinates = {0, 0};
        protected string color = string.Empty;
        protected List<Piece> plate = new List<Piece>();
        protected string name = string.Empty;
        protected List<int[]> allowed = new List<int[]>();
        protected int[] last = new int[4];

        public Piece(int[] coordinates, string color)
        {
            this.coordinates = coordinates;
            this.color = color;
        }

        public void SetPlate(List<Piece> plate) { this.plate = plate; }

        public List<Piece> GetPlate() { return this.plate; }

        public int[] GetPos() { return this.coordinates; }

        public string GetColor() { return this.color; }

        public string GetName() { return this.name; }

        public void UpdateLast(int[] moov) { this.last = moov; }

        public List<int[]> GetAllowed() { return allowed; }

        public Piece Copy()
        {
            Piece piece = new Piece(coordinates, color);
            if (this.name == "Pawn")
            {
                piece = new Pawn(coordinates, color);
            }

            else if (this.name == "Rook")
            {
                piece = new Rook(coordinates, color);
            }

            else if (this.name == "Knight")
            {
                piece = new Knight(coordinates, color);
            }

            else if (this.name == "Bishop")
            {
                piece = new Bishop(coordinates, color);
            }

            else if (this.name == "Queen")
            {
                piece = new Queen(coordinates, color);
            }

            else
            {
                piece = new King(coordinates, color);
            }
            piece.allowed = this.allowed;
            piece.last = this.last;
            return piece;
        }

        public int GetValue()
        {
            if (name == "Rook")
            {
                return 5;
            }
            else if (name == "Knight")
            {
                return 3;
            }
            else if (name == "Bishop")
            {
                return 3;
            }
            else if (name == "Pawn")
            {
                return 1;
            }
            else if (name == "Queen")
            {
                return 9;
            }
            else
            {
                return 4;
            }
        }

        public string StrAllowed()
        {
            string res = "";
            foreach (int[] ints in allowed)
            {
                res += ints[0] + ";" + ints[1];
                res += " | ";
            }
            return res;
        }

        public void Move(int[] arr)
            // Move the piece to the specified coordinates
            // Update the allowed moves of others pieces
        {
            this.coordinates = arr;
            foreach(Piece p in plate)
            {
                if (p.GetName() == "Pawn")
                {
                    Pawn piece = (Pawn)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Rook")
                {
                    Rook piece = (Rook)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Knight")
                {
                    Knight piece = (Knight)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Bishop")
                {
                    Bishop piece = (Bishop)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Queen")
                {
                    Queen piece = (Queen)p;
                    piece.UpdateAllow();
                }

                else
                {
                    King piece = (King)p;
                    piece.UpdateAllow();
                }
            }
        }

        public string Short()
            // Display a piece with its color and its name initials
            // (used for the board display)
        {
            if (name == "Knight")
            {
                return "  " + color[0] + "N  ";
            }
            return "  " + color[0] + name[0] + "  ";
        }

        public int Number() { return 8 * coordinates[1] + coordinates[0]; }

        public int CompareTo(Piece other)
        {
            if (this == other) { return 0; }
            else if (other == null) { return 64; }
            else
            {
                return this.Number() - other.Number();
            }
        }

        protected int[] Vsn(string dir, int x, int y)
            // Return the neighbor of a case toward the specified direction
        {
            int[] ints = new int[2];
            ints[0] = x; ints[1] = y;
            if (dir == "R") { ints[0] += 1; }
            else if (dir == "L") { ints[0] -= 1; }
            else if (dir == "U") { ints[1] += 1; }
            else if (dir == "D") { ints[1] -= 1; }
            else if (dir == "RU") { ints[0] += 1; ints[1] += 1; }
            else if (dir == "LU") { ints[0] -= 1; ints[1] += 1; }
            else if (dir == "RD") { ints[0] += 1; ints[1] -= 1; }
            else { ints[0] -= 1; ints[1] -= 1; }
            return ints;
        }

        public bool IsEmpty(int x, int y) 
            // Return true if the case is empty
        {
            foreach (Piece piece in plate) 
            {
                if (piece.GetPos()[0] == x && piece.GetPos()[1] == y)
                {
                    return false;
                }
            }
            return true;
        }

        public Piece PieceAt(int x, int y)
            // Return the piece at the specified position, a fake piece else
        {
            Piece piece1 = new Piece(new int[2] {-1, -1}, color);
            foreach (Piece piece in plate)
            {
                if (piece.GetPos()[0] == x && piece.GetPos()[1] == y) { return piece; }
            }
            return piece1;
        }


        private bool IsLimite(string dir, int x, int y)
            // Return true if the case is the last possible toward the direction
        {
            if (dir == "R" && x >= 7) { return true; }
            else if (dir == "L" && x <= 0) { return true; }
            else if (dir == "U" && y >= 7) { return true; }
            else if (dir == "D" && y <= 0) { return true; }
            else if (dir == "RU" && (x >= 7 || y >= 7)) { return true; }
            else if (dir == "RD" && (x >= 7 || y <= 0)) { return true; }
            else if (dir == "LU" && (x <= 0 | y >= 7)) { return true; }
            else if (dir == "LD" && (x <= 0 || y <= 0)) { return true; }
            else { return false; }
        }

        protected List<int[]> AllowedCase(string[] dirs)
            // Return all possibles moves toward all specified directions
        {
            List<int[]> allowed2 = new List<int[]>();
            int[] ints = new int[2];
            foreach (string i in dirs)
            {
                if (!IsLimite(i, coordinates[0], coordinates[1]))
                {
                    ints = Vsn(i, coordinates[0], coordinates[1]);
                    while (true)
                    {
                        if (IsEmpty(ints[0], ints[1]))
                        {
                            allowed2.Add(ints);
                            if (IsLimite(i, ints[0], ints[1])) { break; }
                            else ints = Vsn(i, ints[0], ints[1]);
                        }
                        else
                        {
                            Piece piece = PieceAt(ints[0], ints[1]);
                            if (piece.GetColor() == color) { break; }
                            else
                            {
                                allowed2.Add(ints);
                                break;
                            }
                        }
                    }
                }
            }

            return allowed2;
        }

        public bool DepAllowed(int x, int y, bool display)
            // Return ture is the move is allowed
        {
            foreach (int[] ints in allowed)
            {
                if (ints[0] == x && ints[1] == y) { return true; }
            }
            if (display) Console.WriteLine("Illegal move");
            return false;
        }

        public bool IsProtected()
            // Return true if the piece is protected
        {
            if (color == "Light") { color = "Dark"; }
            else { color = "Light"; }

            foreach (Piece p in plate)
            {
                if (p.GetName() == "Pawn" && p.GetColor() != color)
                {
                    Pawn piece = (Pawn)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Rook" && p.GetColor() != color)
                {
                    Rook piece = (Rook)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Knight" && p.GetColor() != color)
                {
                    Knight piece = (Knight)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Bishop" && p.GetColor() != color)
                {
                    Bishop piece = (Bishop)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Queen" && p.GetColor() != color)
                {
                    Queen piece = (Queen)p;
                    piece.UpdateAllow();
                }

                /*else if (p.GetName() == "Roi" && p.GetColor() != color)
                {
                    King piece = (King)p;
                    piece.UpdateAllow();
                }*/
            }

            bool isprotected = false;
            foreach (Piece piece in plate)
            {
                if (piece.GetColor() != color && piece.DepAllowed(coordinates[0], coordinates[1], false))
                {
                    isprotected = true;
                }
                if(piece.GetColor() != color && piece.GetName()=="King")
                {
                    if (piece.GetPos()[0] == coordinates[0])
                    {
                        if (Math.Abs(piece.GetPos()[1] - coordinates[1]) == 1)
                        {
                            isprotected = true;
                        }
                    }

                    else if (Math.Abs(piece.GetPos()[0] - coordinates[0]) == 1)
                    {
                        if (Math.Abs(piece.GetPos()[1] - coordinates[1]) <= 1)
                        {
                            isprotected = true;
                        }
                    }
                }
            }

            if (color == "Light") { color = "Dark"; }
            else { color = "Light"; }
            foreach (Piece p in plate)
            {
                if (p.GetName() == "Pawn" && p.GetColor() == color)
                {
                    Pawn piece = (Pawn)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Rook" && p.GetColor() == color)
                {
                    Rook piece = (Rook)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Knight" && p.GetColor() == color)
                {
                    Knight piece = (Knight)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Bishop" && p.GetColor() == color)
                {
                    Bishop piece = (Bishop)p;
                    piece.UpdateAllow();
                }

                else if (p.GetName() == "Queen" && p.GetColor() == color)
                {
                    Queen piece = (Queen)p;
                    piece.UpdateAllow();
                }

                /*else if (p.GetName() == "Roi" && p.GetColor() == color)
                {
                    King piece = (King)p;
                    piece.UpdateAllow();
                }*/
            }

            return isprotected;
        }

        public bool Check(int[] arr) 
            // Return true if the move put the allied king in check
        {
            int[] dep = coordinates;
            Piece piece1 = PieceAt(arr[0], arr[1]);
            Move(arr);
            int[] king = new int[2];
            foreach(Piece piece in plate) 
            {
                if(piece.GetName() == "King" && piece.GetColor() == color) 
                {
                    king = piece.GetPos();
                    break;
                }
            }

            bool chess = false;
            foreach(Piece piece in plate)
            {
                if (piece.GetColor() != color && piece != piece1)
                {
                    chess = piece.DepAllowed(king[0], king[1], false);
                    if (chess) { break; }
                }
            }
            Move(dep);
            return chess;
        }

        public bool PutCheck()
            // Return true if the move put the oppsing king in check
        {
            int[] king = new int[2];
            foreach (Piece piece in plate)
            {
                if (piece.GetName() == "King" && piece.GetColor() != color)
                {
                    king = piece.GetPos();
                    break;
                }
            }

            foreach(Piece piece in plate)
            {
                if (piece.GetColor() == color && piece.DepAllowed(king[0], king[1], false))
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckMate() 
            // Return true if the opponant is checkmate
        {
            foreach(Piece piece in plate) 
            {
                if (piece.GetColor() != color)
                {
                    for(int i=0; i<8; i++)
                    {
                        for (int k=0; k<8; k++)
                        {
                            int[] r = new int[2];
                            r[0] = i; r[1] = k;

                            if (piece.DepAllowed(r[0], r[1], false))
                            {
                                if (!piece.Check(r))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            // Sinon, il a y échecs et mat
            return true;
        }
    }
}