using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//A mozgatás kezelése
public class MoveManager : MonoBehaviour
{
    public static MoveManager instance;
    Match3Manager game;
    
    Piece movingPiece;
    Point newIndexPoint;
    Vector2 mouseStart;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        game = GetComponent<Match3Manager>();
    }

    //Ha van elmentett elem (tehat lenyomva tartunk valamit) akkor azt mozgatja
    void Update()
    {
        if(movingPiece != null)
        {
            //Eger pozicio valtozasanak reszletei
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(System.Math.Abs(dir.x),System.Math.Abs(dir.y));

            newIndexPoint = Point.ClonePoints(movingPiece.positionInGrid);
            Point difference = Point.Zero;
            if(dir.magnitude > 32)
            {
                if(aDir.x > aDir.y)
                {
                    difference = (new Point ( (nDir.x > 0) ? 1 : -1, 0 ));
                }
                else if(aDir.x < aDir.y)
                {
                    difference = (new Point (0, (nDir.y > 0) ? -1 : 1));
                }
            }

            newIndexPoint.Add(difference);
            Vector2 pos = game.GetPositionFromPoint(movingPiece.positionInGrid);
            if(!newIndexPoint.EqualPoints(movingPiece.positionInGrid))
            {
                pos += Point.MultiplyPoint(new Point(difference.x, -difference.y),16).ToVector();
            }
            movingPiece.MovePositionTo(pos);
        }

    }

    //Akkor hivodik meg, mikor lenyomas tortent egy node-on. 
    //Elmenti az elemet es a mouse positiont az updatehez
    public void MovePiece(Piece piece)
    {
        if(movingPiece != null) return;
        movingPiece = piece;
        mouseStart = Input.mousePosition;
    }
    //Felengedeskor hivodik meg
    //Reseteli az elemet
    public void DropPiece()
    {
        if(movingPiece == null) return;

        if (!newIndexPoint.EqualPoints(movingPiece.positionInGrid))
        {
            game.FlipPieces(movingPiece.positionInGrid, newIndexPoint, true);
        }

        else
        {
            game.ResetPiece(movingPiece);
        }
        
        movingPiece = null;

    }
}