namespace ChessGame
{
    internal class Minimax
    {
        private string color = string.Empty;
        private List<Piece> pieces; 
        private List<Piece> queens;
        private const int MAX_DEPTH = 3;
        private List<int[]> moovs = new List<int[]>();

        public Minimax(string color, List<Piece> pieces, List<Piece> queens)
        {
            this.color = color;
            this.pieces = pieces;
            this.queens = queens;
        }

        private bool LegalDep(List<Piece> pieces, Piece jouee, int[] arr)
        {
            bool allow = jouee.Check(arr);
            if (jouee.GetName() == "King")
            {
                if (Math.Abs(arr[0] - jouee.GetPos()[0]) == 2)
                {
                    if (allow)
                    {
                        return false;
                    }
                    int[] r = new int[2];
                    r[1] = arr[1];
                    if (arr[0] == 2) { r[0] = 3; }
                    else { r[0] = 5; }
                    allow = jouee.Check(r);
                    if (allow)
                    {
                        return false;
                    }
                }
                else
                {
                    if (allow)
                    {
                        return false;
                    }
                }

            }

            if (allow)
            {
                return false;
            }

            return jouee.DepAllowed(arr[0], arr[1], false);
        }


        private List<Piece>? ComputeChild(List<Piece> current, List<Piece> queens, Piece piece, int[] arr, int depth)
            // Return the game state after the specified move (null if illegal move)
        {
            foreach (Piece p in current)
            {
                p.SetPlate(current);
            }

            foreach (Piece p in queens)
            {
                p.SetPlate(current);
            }

            if (LegalDep(current, piece, arr))
            {
                int[] dep = piece.GetPos();
                Piece? piece1 = null;
                foreach (Piece p in current) { if (p.GetPos()[0] == arr[0] && p.GetPos()[1] == arr[1]) piece1 = p; }
                if (piece.GetName() == "Pawn")
                {
                    if (Math.Abs(arr[0] - dep[0]) == 1 && Math.Abs(arr[1] - dep[1]) == 1 && piece.IsEmpty(arr[0], arr[1]))
                    {
                        piece1 = piece.PieceAt(arr[0], dep[1]);
                    }
                }
                current.Remove(piece1);
                if (piece.GetName() == "King" && Math.Abs(arr[0] - piece.GetPos()[0]) == 2)
                {
                    piece.Move(arr);
                    int[] r = new int[2];
                    r[1] = piece.GetPos()[1];
                    Rook? rook = null;
                    if (arr[0] == 2)
                    {
                        r[0] = 3;
                        foreach (Piece p in current)
                        {
                            if (p.GetPos()[0] == 0 && p.GetPos()[1] == piece.GetPos()[1])
                            {
                                rook = (Rook)p;
                            }
                        }
                    }
                    else
                    {
                        r[0] = 5;
                        foreach (Piece p in current)
                        {
                            if (p.GetPos()[0] == 7 && p.GetPos()[1] == piece.GetPos()[1])
                            {
                                rook = (Rook)p;
                            }
                        }
                    }
                    rook.Move(r);
                }
                else piece.Move(arr);
                foreach (Queen p in queens) { p.UpdateAllow(); }
                if (piece.GetName() == "Pawn" && (arr[1] == 0 || arr[1] == 7))
                {
                    current.Remove(piece);
                    foreach (Queen queen in queens)
                    {
                        if (queen.GetPos()[0] == arr[0] && queen.GetPos()[1] == arr[1]) { current.Add(queen); }
                    }
                }
                int[] moov = new int[4];
                moov[0] = dep[0];
                moov[1] = dep[1];
                moov[2] = arr[0];
                moov[3] = arr[1];
                foreach (Piece p in current) { p.UpdateLast(moov); }
                foreach (Piece p in queens) { p.UpdateLast(moov); }

                if (depth == 0)
                {
                    moovs.Add(moov);
                }
                return current;
            }
            return null;
        }

        private List<Piece> Copy(List<Piece> pieces)
        {
            List<Piece> copy = new List<Piece>();
            foreach (Piece piece in pieces) 
            {
                copy.Add(piece);
            }
            return copy;
        }

        private List<int[]> Randomized(List<int[]> allowed) 
        {
            var rnd = new Random();
            var randomized = allowed.OrderBy(item => rnd.Next());
            return randomized.ToList();
        }

        private List<Piece> Randomized(List<Piece> pieces)
        {
            var rnd = new Random();
            var randomized = pieces.OrderBy(item => rnd.Next());
            return randomized.ToList();
        }

        private List<List<Piece>> NextState(List<Piece> pieceList, int depth)
            // Return the list of possible state from the state pieceList
        {
            List<List<Piece>> Childs = new List<List<Piece>>();
            pieceList = Randomized(pieceList);
            foreach(Piece piece in pieceList) 
            {
                if (depth%2 == 0) // An even depth means it's a bot move
                {
                    if (piece.GetColor() == color)
                    {
                        foreach (int[] arr in Randomized(piece.GetAllowed()))
                        {
                            // Copy the lists to avoid non-wanted modifications 
                            List<Piece> current = ComputeChild(Copy(pieceList), Copy(queens), piece, arr, depth);
                            if (current != null)
                            {
                                Childs.Add(current);
                            }
                        }
                    }
                }

                else
                {
                    if (piece.GetColor() != color)
                    {
                        foreach (int[] arr in Randomized(piece.GetAllowed()))
                        {
                            List<Piece> current = ComputeChild(Copy(pieceList), Copy(queens), piece, arr, depth);
                            if (current != null)
                            {
                                Childs.Add(current);
                            }
                        }
                    }
                }
            }
            return Childs;
        }

        private int[] ValueMax(List<Piece> state, int alpha, int beta, int depth)
            // Return the state with the best evaluation
        { 
            int v = int.MinValue;
            int[] res = new int[2] { 0, v };
            List<List<Piece>> childs = NextState(Copy(state), depth);
            foreach (List<Piece> child in childs)
            {
                if (depth >= MAX_DEPTH)
                {
                    int vm = Math.Max(v, Compare(state, child));
                    if (v >= beta)
                    {
                        res[0] = childs.IndexOf(child);
                        res[1] = v;
                        return res;
                    }
                    if (vm >= v)
                    {
                        v = vm;
                        if (v >= beta)
                        {
                            res[0] = childs.IndexOf(child);
                            res[1] = v;
                            return res;
                        }
                    }
                }
                else
                {
                    int[] vm = ValueMin(child, alpha, beta, depth + 1);
                    v = Math.Max(v, vm[1]);
                    if (v >= beta)
                    {
                        res[0] = childs.IndexOf(child);
                        res[1] = v;
                        return res;
                    }
                    alpha = Math.Max(alpha, v);
                }
            }
            res[1] = childs.Count == 0 ? Evaluation(state) : v;
            return res;
        }

        private int[] ValueMin(List<Piece> state, int alpha, int beta, int depth)
        // Return the state with the worst evaluation
        {
            int v = int.MaxValue;
            int[] res = new int[2] { 0, v };
            List<List<Piece>> childs = NextState(Copy(state), depth);
            foreach (List<Piece> child in childs)
            {
                if (depth >= MAX_DEPTH)
                {
                    int vm = Math.Min(v, Compare(state, child));
                    if (v <= alpha)
                    {
                        res[0] = childs.IndexOf(child);
                        res[1] = v;
                        return res;
                    }
                    if (vm <= v)
                    {
                        v = vm;
                        if (v <= alpha)
                        {
                            res[0] = childs.IndexOf(child);
                            res[1] = v;
                            return res;
                        }
                    }
                }
                else
                {
                    int[] vm = ValueMax(child, alpha, beta, depth + 1);
                    v = Math.Min(v, vm[1]);
                    if (depth >= MAX_DEPTH || v <= alpha)
                    {
                        res[0] = childs.IndexOf(child);
                        res[1] = v;
                        return res;
                    }
                    beta = Math.Min(beta, v);
                }
            }
            res[1] = childs.Count == 0 ? Evaluation(state) : v;
            return res;
        }

        private int Evaluation(List<Piece> pieceList)
            // Evaluate a state
        {
            int res = 0;
            foreach(Piece piece in pieceList)
            {
                if (piece.GetColor() == color)
                {
                    res += piece.GetValue();
                }
                else
                {
                    res -= piece.GetValue();
                }
            }
            return res;
        }

        private int Compare(List<Piece> state, List<Piece> child)
            // Compare a state with one of its children
        {
            int eval_state = Evaluation(state);
            int eval_child = Evaluation(child);
            return eval_child - (eval_state - eval_child);
        }

        public int[] Compute()
            // Compute the minimax algorithm with alpha-beta pruning and maximal depth MAX_DEPTH
        {
            int[] res = ValueMax(pieces, int.MinValue, int.MaxValue, 0);
            return moovs.ElementAt(res[0]);
        }
    }
}
