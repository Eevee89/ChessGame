namespace ChessGame
{
    internal class Rook : Piece
    {
        protected bool begin = true;

        public Rook(int[] coordinates, string color) : base(coordinates, color)
        {
            this.name = "Rook";
        }

        public bool HasMoved() { return !begin; }

        public void UpdateAllow()
            // Update the allowed moves
        {
            if (last[2] == coordinates[0] && last[3] == coordinates[1])
            { 
                begin = false;
            }
            string[] dirs = new string[4];
            dirs[0] = "U"; dirs[1] = "D"; dirs[2] = "L"; dirs[3] = "R";
            allowed = AllowedCase(dirs);
        }
    }
}