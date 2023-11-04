namespace ChessGame
{
    internal class Queen : Piece
    {
        public Queen(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "Queen";
        }

        public void UpdateAllow()
            // Update the allowed moves
        {
            string[] dirs = new string[8];
            dirs[0] = "U"; dirs[1] = "D"; dirs[2] = "L"; dirs[3] = "R"; 
            dirs[4] = "LU"; dirs[5] = "RD"; dirs[6] = "LD"; dirs[7] = "RU";
            allowed = AllowedCase(dirs);
        }
    }
}