using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPawn : csChessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        csChessman c, c2;
        int[] e = csBoardManager.Instance.EnPassantMove;

        // White team move
        if (isWhite)
        {
            // Diagonal Left
            if (CurrentX != 0 && CurrentY != 7)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }

                // En Passant
                if (e[0] == CurrentX - 1 && e[1] == CurrentY + 1)
                    r[CurrentX - 1 , CurrentY + 1] = true;
            }

            // Diagonal Right
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }

                // En Passant
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    r[CurrentX + 1 , CurrentY + 1] = true;
            }

            // Forward
            if (CurrentY != 7)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c == null)
                    r[CurrentX, CurrentY + 1] = true;
            }

            // Forward on first move
            if (CurrentY == 1)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                c2 = csBoardManager.Instance.Chessmans[CurrentX, CurrentY + 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY + 2] = true;
            }
        }

        // Black team move
        else
        {
            // Diagonal Left
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }

                // En Passant
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                    r[CurrentX - 1 , CurrentY - 1] = true;
            }

            // Diagonal Right
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }

                // En Passant
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    r[CurrentX + 1 , CurrentY - 1] = true;
            }

            // Forward
            if (CurrentY != 0)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c == null)
                    r[CurrentX, CurrentY - 1] = true;
            }

            // Forward on first move
            if (CurrentY == 6)
            {
                c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                c2 = csBoardManager.Instance.Chessmans[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                    r[CurrentX, CurrentY - 2] = true;
            }
        }
        return r;
    }
}
