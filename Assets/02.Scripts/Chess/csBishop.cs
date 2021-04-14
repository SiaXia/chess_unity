using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBishop : csChessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        csChessman c;
        int i, j;

        // Forward Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;

            c = csBoardManager.Instance.Chessmans[i, j];

            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i,j] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }
                break;
            }
        }

        // Forward Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >=8 || j >= 8)
                break;

            c = csBoardManager.Instance.Chessmans[i, j];

            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }
                break;
            }
        }

        // Backward Left
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j <0)
                break;

            c = csBoardManager.Instance.Chessmans[i, j];

            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }
                break;
            }
        }

        // Backward Right
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >=8 || j <0)
                break;

            c = csBoardManager.Instance.Chessmans[i, j];

            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, j] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }
                break;
            }
        }

        return r;
    }
}
