namespace ChessGame
{
    internal class Game
    {

        public static int ComparePiece(Piece piece1, Piece piece2)
        {
            return piece1.CompareTo(piece2);
        }

        public static int[] FromTo()
            // Get the coordinates of the piece to move and of the case where to move it
            // Insure valides coordinates (between 0 and 7 included)
        {
            int[] r = new int[4];
            Console.Write("Piece to move : ");
            string? coup = Console.ReadLine();
            while (coup.Length != 2 || ((65 > coup[0] || coup[0] > 72) && (97 > coup[0] || coup[0] > 104)) || (49 > (int)coup[1] || (int)coup[1] > 56))
            {
                Console.WriteLine("Null move");
                Console.Write("Piece to move : ");
                coup = Console.ReadLine();
            }
            if (65 <= coup[0] && coup[0] <= 72) { r[0] = coup[0] - 65; }
            else { r[0] = coup[0] - 97; }
            r[1] = (int)coup[1] - 49;

            Console.Write("Where to move it : ");
            coup = Console.ReadLine();
            while (coup.Length != 2 || ((65 > coup[0] || coup[0] > 72) && (97 > coup[0] || coup[0] > 104)) || (49 > (int)coup[1] || (int)coup[1] > 56))
            {
                Console.WriteLine("Null move");
                Console.Write("Where to move it : ");
                coup = Console.ReadLine();
            }
            if (65 <= coup[0] && coup[0] <= 72) { r[2] = coup[0] - 65; }
            else { r[2] = coup[0] - 97; }
            r[3] = (int)coup[1] - 49;
            // r[0, 1] is the coordinates of the moved piece, r[2, 3] the ones of the target
            return r;
        }

        public static bool LegalDep(List<Piece> pieces, Piece jouee, int[] arr)
            // Return true if the move is legal
        {
            bool allow = jouee.Check(arr); // true if the move put in check the allied king
            if (jouee.GetName() == "King")
            {
                if (Math.Abs(arr[0] - jouee.GetPos()[0]) == 2) // Castling attempt
                {
                    if (allow)
                    {
                        Console.WriteLine("Castling not allowed : Case 2 in check");
                        return false;
                    }
                    int[] r = new int[2];
                    r[1] = arr[1];
                    if (arr[0] == 2) { r[0] = 3; }
                    else { r[0] = 5; }
                    allow = jouee.Check(r);
                    if (allow)
                    {
                        Console.WriteLine("Castling not allowed : Case 1 in check");
                        return false;
                    }
                }
                else // Regular king move
                {
                    if (allow)
                    {
                        Console.WriteLine("Illegal move : target in check");
                        return false;
                    }
                }

            }

            if (allow) // The move put the allied king in check
            {
                Console.WriteLine("Illegal move : this piece protects the king");
                return false;
            }

            return jouee.DepAllowed(arr[0], arr[1], true);
        }

        public static void AffichePlateau(List<Piece> pieces)
            // Display the gameboard
        {
            string sep = " ";
            for (int i = 0; i < 8; i++) { sep += "------ "; }
            Console.WriteLine(sep);
            string voidline = "|";
            for (int i = 0; i < 8; i++) { voidline += "      |"; }

            for (int i = 7; i > -1; i--) 
            {
                string[] strings = new string[8];
                for (int k = 0; k < 8; k++) { strings[k] = "      "; }
                foreach (Piece piece in pieces)
                {
                    int[] coo = piece.GetPos();
                    if (coo[1] == i)
                    {
                        strings[coo[0]] = piece.Short();
                    }
                }
                string line = "|";
                for (int j  = 0; j < 8; j++)
                {
                    line += strings[j] + "|";
                }
                line += " " + (i+1);
                Console.WriteLine(voidline);
                Console.WriteLine(line);
                Console.WriteLine(voidline);
                Console.WriteLine(sep);
            }
            string letters = " ";
            for (int i = 0; i < 8; i++)
            {
                char maj = (char)(i + 65);
                char min = (char)(i + 97);
                letters += "  "+maj+min+"   "; 
            }
            Console.WriteLine(letters);
            Console.WriteLine();
        }

        public static List<Piece> Copy(List<Piece> pieces)
        {
            List<Piece> copy = new List<Piece>();
            foreach (Piece piece in pieces)
            {
                copy.Add(piece.Copy());
            }
            foreach (Piece piece in copy)
            {
                piece.SetPlate(copy);
            }
            return copy;
        }

        public static void Main()
        {
            // Lines 178 to 249 : initialization of the gameboard
            List<Piece> pieces = new List<Piece>();
            List<Piece> queens = new List<Piece>();
            int[] coo = new int[2];
            for (int i = 0; i < 8; i++)
            {
                coo = new int[2];
                coo[0] = i; coo[1] = 1;
                Piece p = new Pawn(coo, "Light");
                pieces.Add(p);
                coo = new int[2];
                coo[0] = i; coo[1] = 6;
                p = new Pawn(coo, "Dark");
                pieces.Add(p);
            }

            coo = new int[2];
            coo[0] = 0; coo[1] = 0;
            pieces.Add(new Rook(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 7; coo[1] = 0;
            pieces.Add(new Rook(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 0; coo[1] = 7;
            pieces.Add(new Rook(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));
            coo = new int[2];
            coo[0] = 7; coo[1] = 7;
            pieces.Add(new Rook(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));

            coo = new int[2];
            coo[0] = 1; coo[1] = 0;
            pieces.Add(new Knight(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 6; coo[1] = 0;
            pieces.Add(new Knight(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 1; coo[1] = 7;
            pieces.Add(new Knight(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));
            coo = new int[2];
            coo[0] = 6; coo[1] = 7;
            pieces.Add(new Knight(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));

            coo = new int[2];
            coo[0] = 2; coo[1] = 0;
            pieces.Add(new Bishop(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 5; coo[1] = 0;
            pieces.Add(new Bishop(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 2; coo[1] = 7;
            pieces.Add(new Bishop(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));
            coo = new int[2];
            coo[0] = 5; coo[1] = 7;
            pieces.Add(new Bishop(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));

            coo = new int[2];
            coo[0] = 4; coo[1] = 0;
            pieces.Add(new King(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 4; coo[1] = 7;
            pieces.Add(new King(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));

            coo = new int[2];
            coo[0] = 3; coo[1] = 0;
            pieces.Add(new Queen(coo, "Light"));
            queens.Add(new Queen(coo, "Dark"));
            coo = new int[2];
            coo[0] = 3; coo[1] = 7;
            pieces.Add(new Queen(coo, "Dark"));
            queens.Add(new Queen(coo, "Light"));

            pieces.Sort();
            foreach (Piece piece in pieces)
            {
                piece.SetPlate(pieces);
            }
            foreach (Piece piece in queens) { piece.SetPlate(pieces); }

            Console.WriteLine("Do you want to play agaisnt a bot or a friend ?");
            Console.WriteLine("1. A bot / 2. A friend");
            string? mode = Console.ReadLine();
            while (mode != "1" && mode != "2")
            {
                Console.WriteLine("Invalid answer. Do you want to play agaisnt a bot or a friend ?");
                Console.WriteLine("1. A bot / 2. A friend");
                mode = Console.ReadLine();
            }
            int rep = int.Parse(mode);
            Console.WriteLine("");

            if (rep == 2)
            {
                Console.WriteLine("Player vs player mode");
                Console.WriteLine("");
                AffichePlateau(pieces);

                string tour = "Light"; // Current player color
                bool game = true;
                while (game)
                {
                    Console.WriteLine(tour+"'s turn");
                    bool allow = false;
                    int[] dep = new int[2];
                    int[] arr = new int[2];
                    Piece jouee = new Piece(dep, "Neutral");
                    jouee.SetPlate(pieces);
                    while (!allow)
                    {
                        int[] r = FromTo();
                        dep = new int[2];
                        arr = new int[2];
                        dep[0] = r[0]; dep[1] = r[1];
                        arr[0] = r[2]; arr[1] = r[3];

                        foreach (Piece p in pieces)
                        {
                            if (p.GetPos()[0] == dep[0] && p.GetPos()[1] == dep[1])
                            {
                                jouee = p;
                                break;
                            }
                        }

                        allow = jouee.GetColor() == tour;
                        if (!allow)
                        {
                            Console.WriteLine("Illegal move : Not the right color");
                        }
                        else allow = LegalDep(pieces, jouee, arr);
                    }

                    Console.WriteLine("Legal move");

                    Piece? captured = null;
                    foreach (Piece piece in pieces) { if (piece.GetPos()[0] == arr[0] && piece.GetPos()[1] == arr[1]) captured = piece; }
                    if (jouee.GetName() == "Pawn")
                    {
                        if (Math.Abs(arr[0] - dep[0]) == 1 && Math.Abs(arr[1] - dep[1]) == 1 && jouee.IsEmpty(arr[0], arr[1]))
                        {
                            captured = jouee.PieceAt(arr[0], dep[1]);
                        }
                    }
                    pieces.Remove(captured);

                    if (jouee.GetName() == "King" && Math.Abs(arr[0] - jouee.GetPos()[0]) == 2) // Castling
                    {
                        jouee.Move(arr);
                        int[] r = new int[2];
                        r[1] = jouee.GetPos()[1];
                        Rook? rook = null;
                        if (arr[0] == 2)
                        {
                            r[0] = 3;
                            foreach (Piece p in pieces)
                            {
                                if (p.GetPos()[0] == 0 && p.GetPos()[1] == jouee.GetPos()[1])
                                {
                                    rook = (Rook)p;
                                }
                            }
                        }
                        else
                        {
                            r[0] = 5;
                            foreach (Piece p in pieces)
                            {
                                if (p.GetPos()[0] == 7 && p.GetPos()[1] == jouee.GetPos()[1])
                                {
                                    rook = (Rook)p;
                                }
                            }
                        }
                        rook.Move(r);
                    }

                    else jouee.Move(arr);

                    foreach (Queen p in queens) { p.UpdateAllow(); }
                    if (jouee.GetName() == "Pawn" && (arr[1] == 0 || arr[1] == 7)) // Pawn promotion
                    {
                        pieces.Remove(jouee);
                        foreach (Queen queen in queens)
                        {
                            if (queen.GetPos()[0] == arr[0] && queen.GetPos()[1] == arr[1]) { pieces.Add(queen); }
                        }
                    }

                    int[] move = new int[4];
                    move[0] = dep[0];
                    move[1] = dep[1];
                    move[2] = arr[0];
                    move[3] = arr[1];
                    foreach (Piece p in pieces) { p.UpdateLast(move); }
                    foreach (Piece p in queens) { p.UpdateLast(move); }

                    Console.WriteLine();
                    bool ischess = jouee.PutCheck(); // true if the move put the opposing king in check
                    if (ischess)
                    {
                        bool checkmate = jouee.CheckMate(); // true if checkmate
                        if (checkmate)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Checkmate ! The " + tour + " win !");
                            game = false;
                        }
                        else Console.WriteLine("Careful : Check!"); 
                    }

                    AffichePlateau(pieces);
                    if (tour == "Light") tour = "Dark";
                    else tour = "Light";
                }
            }

            else if (rep == 1)
            {
                Console.WriteLine("Player vs bot mode");
                Console.WriteLine("");
                AffichePlateau(pieces);

                string tour = "Light";
                bool game = true;
                while (game)
                {
                    if (tour == "Light")
                    {
                        Console.WriteLine("Your turn");
                        bool allow = false;
                        int[] dep = new int[2];
                        int[] arr = new int[2];
                        Piece jouee = new Piece(dep, "Neutral");
                        jouee.SetPlate(pieces);
                        while (!allow)
                        {
                            int[] r = FromTo();
                            dep = new int[2];
                            arr = new int[2];
                            dep[0] = r[0]; dep[1] = r[1];
                            arr[0] = r[2]; arr[1] = r[3];

                            foreach (Piece p in pieces)
                            {
                                if (p.GetPos()[0] == dep[0] && p.GetPos()[1] == dep[1])
                                {
                                    jouee = p;
                                    break;
                                }
                            }

                            allow = jouee.GetColor() == tour;
                            if (!allow)
                            {
                                Console.WriteLine("Illegal move : Not the right color");
                            }
                            else allow = LegalDep(pieces, jouee, arr);
                        }

                        Console.WriteLine("Legal move");

                        Piece? captured = null;
                        foreach (Piece piece in pieces) { if (piece.GetPos()[0] == arr[0] && piece.GetPos()[1] == arr[1]) captured = piece; }
                        if (jouee.GetName() == "Pawn")
                        {
                            if (Math.Abs(arr[0] - dep[0]) == 1 && Math.Abs(arr[1] - dep[1]) == 1 && jouee.IsEmpty(arr[0], arr[1]))
                            {
                                captured = jouee.PieceAt(arr[0], dep[1]);
                            }
                        }
                        pieces.Remove(captured);

                        if (jouee.GetName() == "King" && Math.Abs(arr[0] - jouee.GetPos()[0]) == 2)
                        {
                            jouee.Move(arr);
                            int[] r = new int[2];
                            r[1] = jouee.GetPos()[1];
                            Rook? rook = null;
                            if (arr[0] == 2)
                            {
                                r[0] = 3;
                                foreach (Piece p in pieces)
                                {
                                    if (p.GetPos()[0] == 0 && p.GetPos()[1] == jouee.GetPos()[1])
                                    {
                                        rook = (Rook)p;
                                    }
                                }
                            }
                            else
                            {
                                r[0] = 5;
                                foreach (Piece p in pieces)
                                {
                                    if (p.GetPos()[0] == 7 && p.GetPos()[1] == jouee.GetPos()[1])
                                    {
                                        rook = (Rook)p;
                                    }
                                }
                            }
                            rook.Move(r);
                        }

                        else jouee.Move(arr);

                        foreach (Queen p in queens) { p.UpdateAllow(); }
                        if (jouee.GetName() == "Pawn" && (arr[1] == 0 || arr[1] == 7))
                        {
                            pieces.Remove(jouee);
                            foreach (Queen queen in queens)
                            {
                                if (queen.GetPos()[0] == arr[0] && queen.GetPos()[1] == arr[1]) { pieces.Add(queen); }
                            }
                        }

                        int[] move = new int[4];
                        move[0] = dep[0];
                        move[1] = dep[1];
                        move[2] = arr[0];
                        move[3] = arr[1];
                        foreach (Piece p in pieces) { p.UpdateLast(move); }
                        foreach (Piece p in queens) { p.UpdateLast(move); }
                        Console.WriteLine();
                        bool ischess = jouee.PutCheck();
                        if (ischess)
                        {
                            bool checkmate = jouee.CheckMate();
                            if (checkmate)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Checkmate ! You win !");
                                game = false;
                            }
                            else Console.WriteLine("The bot is in check !");
                        }
                        AffichePlateau(pieces);
                        tour = "Dark";
                        
                    }

                    else
                    {
                        Console.WriteLine("Bot's turn");
                        Minimax minimax = new Minimax("Dark", Copy(pieces), Copy(queens));
                        int[] moov = minimax.Compute();
                        
                        int[] dep = new int[2] { moov[0], moov[1] };
                        int[] arr = new int[2] { moov[2], moov[3] };
                        Piece jouee = new Piece(dep, "Neutral");
                        foreach(Piece piece in pieces) 
                        {
                            if (piece.GetPos()[0] == dep[0] && piece.GetPos()[1] == dep[1])
                            {
                                jouee = piece;
                                break;
                            }
                        }

                        string d = "" + (char)(dep[0]+65) + (dep[1]+1);
                        string a = "" + (char)(arr[0] + 65) + (arr[1] + 1);
                        Console.WriteLine("");
                        Console.WriteLine("The bot plays "+d+" to "+a);

                        Piece? captured = null;
                        foreach (Piece piece in pieces) { if (piece.GetPos()[0] == arr[0] && piece.GetPos()[1] == arr[1]) captured = piece; }
                        if (jouee.GetName() == "Pawn")
                        {
                            if (Math.Abs(arr[0] - dep[0]) == 1 && Math.Abs(arr[1] - dep[1]) == 1 && jouee.IsEmpty(arr[0], arr[1]))
                            {
                                captured = jouee.PieceAt(arr[0], dep[1]);
                            }
                        }
                        pieces.Remove(captured);

                        if (jouee.GetName() == "King" && Math.Abs(arr[0] - jouee.GetPos()[0]) == 2)
                        {
                            jouee.Move(arr);
                            int[] r = new int[2];
                            r[1] = jouee.GetPos()[1];
                            Rook? rook = null;
                            if (arr[0] == 2)
                            {
                                r[0] = 3;
                                foreach (Piece p in pieces)
                                {
                                    if (p.GetPos()[0] == 0 && p.GetPos()[1] == jouee.GetPos()[1])
                                    {
                                        rook = (Rook)p;
                                    }
                                }
                            }
                            else
                            {
                                r[0] = 5;
                                foreach (Piece p in pieces)
                                {
                                    if (p.GetPos()[0] == 7 && p.GetPos()[1] == jouee.GetPos()[1])
                                    {
                                        rook = (Rook)p;
                                    }
                                }
                            }
                            rook.Move(r);
                        }

                        else jouee.Move(arr);

                        foreach (Queen p in queens) { p.UpdateAllow(); }
                        if (jouee.GetName() == "Pawn" && (arr[1] == 0 || arr[1] == 7))
                        {
                            pieces.Remove(jouee);
                            foreach (Queen queen in queens)
                            {
                                if (queen.GetPos()[0] == arr[0] && queen.GetPos()[1] == arr[1]) { pieces.Add(queen); }
                            }
                        }
                        foreach (Piece p in pieces) { p.UpdateLast(moov); }
                        foreach (Piece p in queens) { p.UpdateLast(moov); }

                        Console.WriteLine();
                        bool ischess = jouee.PutCheck();
                        if (ischess)
                        {
                            bool checkmate = jouee.CheckMate();
                            if (checkmate)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Checkmate ! You lose");
                                game = false;
                            }
                            else Console.WriteLine("Careful ! You are in check");
                        }
                        AffichePlateau(pieces);
                        tour = "Light";
                    }
                }
            }
        }
    }
}