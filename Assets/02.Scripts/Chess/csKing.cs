using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csKing : csChessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        bool[,] r2 = new bool[8, 8]; // opponent PossibleMove

        csChessman c, c2, c3, rook;
        int i, j;
        
        // Forward Side
        i = CurrentX - 1;
        j = CurrentY + 1;
 
        if (CurrentY != 7)
        {
            for(int k = 0; k < 3; k++)
            {
                if(i>=0||i<8)
                {
                    c = csBoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Backward Side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 || i < 8)
                {
                    c = csBoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }

                i++;
            }
        }

        // Left
        if (CurrentX != 0)
        {
            c = csBoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null)
            {
                r[CurrentX - 1, CurrentY] = true;

                if (CurrentX == 4|| CurrentX == 3 && CurrentY == 0)
                {
                    // Castling
                    c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY];
                    if (c.isWhite)
                        rook = csBoardManager.Instance.Chessmans[0, 0]; // rook
                    else
                        rook = csBoardManager.Instance.Chessmans[0, 7]; // rook

                    if (!c.isMoved && !rook.isMoved)
                    {
                        c2 = csBoardManager.Instance.Chessmans[CurrentX - 2, CurrentY];
                        c3 = csBoardManager.Instance.Chessmans[CurrentX - 3, CurrentY];
                        if (c2 == null && c3 == null)
                            r[CurrentX - 2, CurrentY] = true;
                    }
                }
            }
            else if (isWhite != c.isWhite)
                r[CurrentX - 1, CurrentY] = true;
        }

        // Right
        if (CurrentX !=7)
        {
            c = csBoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
            {
                r[CurrentX + 1, CurrentY] = true;

                if (CurrentX == 4 || CurrentX == 3 && CurrentY == 0)
                {
                    // Castling
                    c = csBoardManager.Instance.Chessmans[CurrentX, CurrentY];
                    if (c.isWhite)
                        rook = csBoardManager.Instance.Chessmans[7, 0]; // rook
                    else
                        rook = csBoardManager.Instance.Chessmans[7, 7]; // rook


                    c2 = csBoardManager.Instance.Chessmans[CurrentX + 2, CurrentY];
                    if (!c.isMoved && !rook.isMoved && c2 == null)
                        r[CurrentX + 2, CurrentY] = true;
                }
            }
            else if (isWhite != c.isWhite)
                r[CurrentX + 1, CurrentY] = true;
        }

        // Checked
        // King cannot move the way could be checked
        /*
        if (true)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    c = csBoardManager.Instance.Chessmans[i, j];
                    if (c != null&&c.isWhite!= csBoardManager.Instance.isWhiteTurn)
                    {
                        r2 = c.PossibleMove();
                        //for (int x = 0; x < 8; x++)
                        //{
                        //  for (int y = 0; y < 8; y++)
                        // {
                        //     if (r2[x, y] && r[x, y])
                        //        r[x, y] = false;
                        //  }
                        // }
                        r2 = new bool[8, 8];
                    }
                }
            }
        }
        */
        return r;
    }
}
