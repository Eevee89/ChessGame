namespace ChessGame
{
    internal class Bishop : Piece
    {
        public Bishop(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "Bishop";
        }

        public void UpdateAllow()
            // Update the allowed moves
        {
            string[] dirs = new string[4];
            dirs[0] = "LU"; dirs[1] = "RD"; dirs[2] = "LD"; dirs[3] = "RU";
            allowed = AllowedCase(dirs);
        }
    }
}