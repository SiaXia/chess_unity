using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csKnight : csChessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        // 2 moves -> 1 move
        // ForwardLeft
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);

        // ForwardRight
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);

        // BackwardLeft
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);

        // BackwardRight
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);

        // RightForward
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        // RightBackward
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        // LeftForward
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        // LeftBackward
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }
    public void KnightMove(int x,int y,ref bool[,] r)
    {
        csChessman c;
        if (x >= 0 && x < 8 && y>=0 && y < 8){
            c = csBoardManager.Instance.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else if (c.isWhite != isWhite)
                {
                    r[x, y] = true;
                    if (c.isKing)
                        csBoardManager.Instance.check = true;
                }
        }
    }
}
