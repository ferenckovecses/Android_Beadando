using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    public int valueOfCell; //0 az empty, -1 pedig a lyuk
    public Point positionOfCell;
    Piece piece;

    public GridCell(int mvalue, Point p)
    {
        this.valueOfCell = mvalue;
        this.positionOfCell = p;
    }

    public void AddPieceToCell(Piece p)
    {
        piece = p;
        valueOfCell = (piece == null) ? 0 : piece.typeValue;
        if(piece == null) return;
        piece.SetIndex(positionOfCell);
    }

    public Piece GetPieceFromCell()
    {
        return piece;
    }
}