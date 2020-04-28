using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[HideInInspector]
	public int typeValue;
    public Point positionInGrid;

    Image spriteImage;

    [HideInInspector]
    public Vector2 position;
    [HideInInspector]
    public RectTransform rect;

    int imageSize = Match3Manager.imageSize;

    bool updating;

    public void Initialize(int typeValue, Point point, Sprite icon)
    {
        spriteImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        this.typeValue = typeValue;
        SetIndex(point);
        spriteImage.sprite = icon;
    }

    //A kapott pontra allitja az elem indexet
    public void SetIndex(Point point)
    {
        this.positionInGrid = point;
        ResetPosition();
        UpdateName();
    }

    public Point GetPosition()
    {
        return this.positionInGrid;
    }

    //Visszaallitja az indexhez tartozo alap poziciot.
    public void ResetPosition()
    {
       position = new Vector2(imageSize/2 + (imageSize*this.positionInGrid.x), -imageSize/2 - (imageSize*this.positionInGrid.y));
    }

    void UpdateName()
    {
        transform.name = "Piece[" + this.positionInGrid.x + "," + this.positionInGrid.y + "]";
    }

    public bool UpdatePiece()
    {
        //Ha az elemet tartva elmozgattuk az egeret akkor mozogjon oda
        if(Vector3.Distance(rect.anchoredPosition,position) > 1)
        {
            MovePositionTo(position);
            updating = true;
            return true;
        }

        //Ha nem, akkor ne csinaljon semmit.
        else
        {
            rect.anchoredPosition = position;
            updating = false;
            return false;
        }
    }

    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    //Lenyomaskor ez hivodik meg
    public void OnPointerDown(PointerEventData eventData)
    {
        if(updating) return;
        MoveManager.instance.MovePiece(this);
    }

    //Felengedeskor ez hivodik meg
    public void OnPointerUp(PointerEventData eventData)
    {
        MoveManager.instance.DropPiece();
    }

}


//A megcserelt parok kezelesere
[System.Serializable]
public class FlippedPieces
{
    public Piece one;
    public Piece two;

    public FlippedPieces(Piece o, Piece t)
    {
        one = o;
        two = t;
    }

    public Piece GetOtherPiece(Piece p)
    {
        if(p == one) return two;
        else if(p == two) return one;
        else return null;
    }
}
