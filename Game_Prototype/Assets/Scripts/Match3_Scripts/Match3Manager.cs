using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class Match3Manager : MonoBehaviour
{
    //A cellákban helyezkednek el a Piece elemek
    GridCell[,] gameGrid;

    int width = 5;
    int height = 8;

    //NxN méretű képek esetén
    public static int imageSize = 125;

    [Header("UI Elements")]
    public RectTransform gameBoard;

	[Header("Prefabs")]
    public GameObject piecePrefab;

    Data_Controller dataController;
    BattleManager battleManager;

    //Listák a különböző elemek nyilvántartására
    List<Piece> updatePieceList;
    List<FlippedPieces> flippedPieceList;
    List<Piece> usedPieceList;

    int[] fills;


    //*******Előkészületi fázis*******

    //Match3 Manager belépési pontja
    void Start()
    {
        PrepareGame();
    }

    //A játék előkészítése
    void PrepareGame()
    {
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
        battleManager = GameObject.Find("Battle_Controller(Clone)").GetComponent<BattleManager>();
    	//Listák és változók inicializálása
        fills = new int[width];
        updatePieceList = new List<Piece>();
        flippedPieceList = new List<FlippedPieces>();
        usedPieceList = new List<Piece>();
        //Prefab méretének beállítása
        piecePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(imageSize,imageSize);

        //A kezdeti tábla létrehozása és megjelenítése
        InitializeBoard();
        VerifyBoard();
        CreateBoard();
	}

	//Feltölti random értékekkel ellátott cellákkal a táblát.
    void InitializeBoard()
    {
        gameGrid = new GridCell[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gameGrid[x, y] = new GridCell(GiveRandomPieceValue(), new Point(x, y));
            }
        }
    }

    //Visszaad egy random cellaértéket
    int GiveRandomPieceValue()
    {
    	/*
        int val = UnityEngine.Random.Range(0,100);
        val = val % typeSprites.Length;
        return val+1;
        */
        if(dataController.elements.Count > 0)
        	return (UnityEngine.Random.Range(1,500) % dataController.elements.Count)+1;
    	else
    		return 0;
    }

    //Ellenőrzi és javítja a kezdeti táblát
    void VerifyBoard()
    {
        List<int> valuesToRemove = new List<int>();

        //Végigmegy minden ponton és megnézi, hogy egy match része-e
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point pointToCheck = new Point(x, y);
                int valueOfPoint = GetValueofPoint(pointToCheck);
                if (valueOfPoint <= 0) continue; //Ha üres vagy lyuk, akkor szkipp

                valuesToRemove.Clear();
                //Ha az elemünk és szomszédos elemek kapcsolatban vannak
                while (IsConnected(pointToCheck, true).Count > 0)
                {
                    valueOfPoint = GetValueofPoint(pointToCheck);
                    //És még nincs az eltávolítandó értékek listáján
                    if (!valuesToRemove.Contains(valueOfPoint))
                    {
                        valuesToRemove.Add(valueOfPoint);
                    }
                    SetValueAtPoint(pointToCheck, GiveBetterValue(ref valuesToRemove));
                }
            }
        }
    }

    //Visszaadja a tábla adott pontján található értéket
    public int GetValueofPoint(Point point)
    {
        if(point.x < 0 || point.x >= width || point.y < 0 ||point.y >= height) return -1;
        return gameGrid[point.x,point.y].valueOfCell;
    }

    //Visszaadja a megadott ponthoz tartozo Node-ot a gameBoardbol
    GridCell GetCellAtPoint(Point point)
    {
        return gameGrid[point.x, point.y];
    }

    //Hozzáadja a sublist elemeit a fő listához, ha még nem szerepelnek benne
    void IncludePoints(ref List<Point> mainList, List<Point> subList)
    {
        foreach (Point checkedPoint in subList)
        {
            bool doAdd = true;
            for(int i = 0; i < mainList.Count; i++)
            {
                if(mainList[i].EqualPoints(checkedPoint))
                {
                    doAdd = false;
                    break;
                }
            }

            if(doAdd)
            {
                mainList.Add(checkedPoint);
            }
        }
    }

    //A már rendezett grid értékek szerint létrehozza a Piece elemeket és a cellákhoz adja őket
    void CreateBoard()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GridCell currentCell = GetCellAtPoint(new Point(x,y));
                if(currentCell.valueOfCell <= 0) continue;

                GameObject point = Instantiate(piecePrefab, gameBoard);
                Piece piece = point.GetComponent<Piece>();
                RectTransform basePosition = point.GetComponent<RectTransform>();
                basePosition.anchoredPosition = new Vector2(imageSize/2 + (imageSize*x), -imageSize/2 - (imageSize*y));
                piece.Initialize(currentCell.valueOfCell, new Point(x,y), dataController.elements[currentCell.valueOfCell - 1].returnSprite());
                currentCell.AddPieceToCell(piece);

            }
        }
    }

    //Egy jó elemértéket ad vissza, amivel nem lesz match
    int GiveBetterValue(ref List<int> valuesToRemove)
    {
        List<int> availableValues = new List<int>();
        for(int i = 0; i < dataController.elements.Count; i++)
        {
            availableValues.Add(i+1);
        }
        foreach (int i in valuesToRemove)
        {
            availableValues.Remove(i);
        }
        //Ez csak hogy mentsük magunkat
        if(availableValues.Count <= 0) return 0;
        //A lehetseges elem típusok közül egy randomot ad vissza
        return availableValues[UnityEngine.Random.Range(0,availableValues.Count)];
    }

    //Adott pozícióban lévő pont értéket megváltoztatja a kapott értékre
    void SetValueAtPoint(Point point, int value)
    {
        gameGrid[point.x, point.y].valueOfCell = value;
    }

    //*******Ellenőrzési fázis*******

    //Adott pontot és szomszédjait "rekurzivan" vizsgálja, hogy kapcsolatban vannak-e
    List<Point> IsConnected(Point checkedPoint, bool isItMainCall)
    {
        List<Point> listOfConnectedPoints = new List<Point>();
        int valueOfOurPoint = GetValueofPoint(checkedPoint);
        List<Point> pointsInLine = new List<Point>();
        int sameType = 0;
        //Irányok
        Point[] directions =
        { 
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };

        //A pontot vizsgálva végigmegy az irányokon és vonalat keres (+ alakban ellenőriz)
        /*              [X]
         * XX[X]O vagy   X O
         *               X
         */
        foreach (Point dir in directions)
        {
            pointsInLine.Clear();
            sameType = 0;
            //Minden irányban 2 távolságig elmegy
            for(int i = 1; i < 3; i++)
            {
                Point nextPoint = Point.AddPoints(checkedPoint, Point.MultiplyPoint(dir, i));
                //Ha igen akkor szamontartjuk es noveljuk a countert
                if(GetValueofPoint(nextPoint) == valueOfOurPoint)
                {
                    pointsInLine.Add(nextPoint);
                    sameType++;
                }
            }
            //Ha adott irányban több azonos típus van
            if(sameType>1)
            {
                //A fő connection listához adjuk a pontjainkat
                IncludePoints(ref listOfConnectedPoints, pointsInLine); 
            }
        }

        //Check hogy egy match közepe vagyunk-e
        //Pl: X[X]X
        for(int i = 0; i < 2; i++) 
        {
            pointsInLine.Clear();
            sameType = 0;
            Point[] checkedPoints = {Point.AddPoints(checkedPoint, directions[i]), Point.AddPoints(checkedPoint, directions[i+2])};
            foreach (Point nextPoint in checkedPoints)
            {
                if(GetValueofPoint(nextPoint) == valueOfOurPoint)
                {
                    pointsInLine.Add(nextPoint);
                    sameType++;
                }
            }
            if(sameType>1)
            {
                IncludePoints(ref listOfConnectedPoints, pointsInLine); //A fő connection listához adjuk a pontjainkat
            }
        }
        

        //Ellenőrzi hogy van-e 2x2, azaz square match
        //Pl: X X 
        //   [X]X
        for(int i = 0; i < 4; i++) 
        {
            pointsInLine.Clear();
            sameType = 0;
            int nextDirection = i + 1;
            if(nextDirection >= 4) 
                nextDirection -= 4;
            //Hozzaadja a horizontal, vertical es diagonal szomszedokat
            Point[] nextPoints = { Point.AddPoints(checkedPoint, directions[i]), Point.AddPoints(checkedPoint, directions[nextDirection]), Point.AddPoints(checkedPoint,Point.AddPoints(directions[i],directions[nextDirection]))};
            foreach (Point nextPoint in nextPoints)
            {
                if(GetValueofPoint(nextPoint) == valueOfOurPoint)
                {
                    pointsInLine.Add(nextPoint);
                    sameType++;
                }
            }

            if(sameType>2)
            {
                IncludePoints(ref listOfConnectedPoints, pointsInLine); //A fo connected listahoz adja ezeket a pontokat
            }
        }
        //Rekurziós cucc
        if(isItMainCall)
        {
            for(int i = 0; i < listOfConnectedPoints.Count; i++)
            {
                IncludePoints(ref listOfConnectedPoints, IsConnected(listOfConnectedPoints[i],false));
            }
        }

        return listOfConnectedPoints;
    }

    //*******Aktív fázis*******

    //Változások kezelése
    void Update()
    {
        if(battleManager.currentBattleState == BattleState.PlayerTurn)
        {
            //Mozgások szerint a pozíciók frissítése
            List<Piece> finishedUpdatingList = new List<Piece>();
            for (int i = 0; i < updatePieceList.Count; i++)
            {
                Piece piece = updatePieceList[i];
                if (!piece.UpdatePiece()) finishedUpdatingList.Add(piece);
            }
            for (int i = 0; i < finishedUpdatingList.Count; i++)
            {
                Piece piece = finishedUpdatingList[i];
                FlippedPieces flip = GetFlipped(piece);
                Piece flippedPiece = null;

                //Simább piece esés
                int x = (int)piece.positionInGrid.x;
                fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

                List<Point> listOfConnectedPoints = IsConnected(piece.positionInGrid, true);
                bool wasFlipped = (flip != null);

                //Ha volt flip az update előtt megnézzük, hogy lett-e match
                if (wasFlipped)
                {
                    flippedPiece = flip.GetOtherPiece(piece);
                    IncludePoints(ref listOfConnectedPoints, IsConnected(flippedPiece.positionInGrid, true));
                }
                //Ha nem lett match
                if (listOfConnectedPoints.Count == 0)
                {
                    if (wasFlipped) //Visszacsinaljuk a flippet
                    {
                        FlipPieces(piece.positionInGrid, flippedPiece.positionInGrid, false);
                    }
                }
                //Ha match lett
                else
                {   
                    //Eltávolítja a listában szereplő pontokat
                    foreach (Point pointInConnection in listOfConnectedPoints)
                    {
                        GridCell cell = GetCellAtPoint(pointInConnection);
                        Piece tempPiece = cell.GetPieceFromCell();

                        if (tempPiece != null)
                        {
                            tempPiece.gameObject.SetActive(false);
                            usedPieceList.Add(tempPiece);

                            //Növeljük az adott típus counterét
                            ScoreController.IncreasePoints(tempPiece.typeValue-1);
                            battleManager.Attack(tempPiece.typeValue-1);
                        }

                        
                        //Kinullázzuk az adott cellához tartozó piece-t
                        cell.AddPieceToCell(null);
                        //Lyuk keletkezett, szóval hívjuk a gravitációt
                        ApplyGravityToColumn(cell.positionOfCell.x);
                    }
                    battleManager.ChangeTurn();

                }
                //Eltakarítjuk a pontokat mikor már végeztünk velük
                flippedPieceList.Remove(flip);
                updatePieceList.Remove(piece);
            } 
        }

        else if(battleManager.currentBattleState == BattleState.EnemyTurn)
        {
            int random = UnityEngine.Random.Range(1,3);
            for(int i = 0; i < random; i++)
            {
                battleManager.Attack();
            }
            battleManager.ChangeTurn();
        }

    }

    //Visszaadja a hekyzetét adott pontnak.
    public Vector2 GetPositionFromPoint(Point point)
    {
        return new Vector2(imageSize/2 + (imageSize*point.x), -imageSize/2 - (imageSize*point.y));
    }

    //Ha a megadott piece flipelve lett nem null értékkel, visszaadjuk a párját
    FlippedPieces GetFlipped(Piece p)
    {
        FlippedPieces flip = null;
        for(int i =0; i<flippedPieceList.Count; i++)
        {
            if(flippedPieceList[i].GetOtherPiece(p) != null) 
             {
                flip = flippedPieceList[i];
                break;
             }
        }
        return flip;
    }

    //Adott elem poziciojat reseteli
    public void ResetPiece(Piece pieceToReset)
    {
        pieceToReset.ResetPosition();
        updatePieceList.Add(pieceToReset);
    }

    //Megcserél két piece-t
    public void FlipPieces(Point firstPoint, Point secondPoint, bool isItMain)
    {
        if(GetValueofPoint(firstPoint) < 0) return;

        GridCell firstCell = GetCellAtPoint(firstPoint);
        Piece firstPiece = firstCell.GetPieceFromCell();

        //Ellenőrzi a másik elemet is
        if(GetValueofPoint(secondPoint) > 0)
        {
            GridCell secondCell = GetCellAtPoint(secondPoint);
            Piece secondPiece = secondCell.GetPieceFromCell();

            firstCell.AddPieceToCell(secondPiece);
            secondCell.AddPieceToCell(firstPiece);

            if(isItMain)
            {
                flippedPieceList.Add(new FlippedPieces(firstPiece, secondPiece));
            }

            //Hozzáadjuk őket, mert kell az update
            updatePieceList.Add(firstPiece);
            updatePieceList.Add(secondPiece);
        }
        //Ha a második piece nem érvényes (pl hole, blank stb) akkor nincs csere
        else
        {
            ResetPiece(firstPiece);
        }
    }

    void ApplyGravity()
    {
        for(int x = 0; x < width; x++)
        {
            //Balról jobbra és lentről felfele vizsgáljuk a gridet.
            for(int y = (height - 1); y >= 0; y--)
            {
                Point checkedPoint = new Point(x,y);
                GridCell cell = GetCellAtPoint(checkedPoint);
                int val = GetValueofPoint(checkedPoint);
                if (val != 0) continue;

                //A következő elemeket vizsgáljuk
                for(int my = (y-1); my >= -1; my--)
                {
                    Point nextPoint = new Point(x,my);
                    int nextval = GetValueofPoint(nextPoint);
                    if(nextval == 0) continue;

                    //Ha a következő pont nem egy lyuk
                    if(nextval != -1)
                    {
                        //Akkor a következő elemet magunkra húzzuk és lyukat hagyunk helyette
                        GridCell nextCell = GetCellAtPoint(nextPoint);
                        Piece nextPiece = nextCell.GetPieceFromCell();

                        cell.AddPieceToCell(nextPiece);
                        updatePieceList.Add(nextPiece);

                        nextCell.AddPieceToCell(null);
                    }

                    //Ha a következő elem egy lyuk
                    else
                    {
                        //Kérünk egy random értéket
                        int newPieceValue = GiveRandomPieceValue();
                        Piece newPiece;

                        //Beállítjuk az eséspontot
                        Point fallPoint = new Point(x, (-1 - fills[x]));

                        //Ha van már használt piece akkor újrahasznosítjuk
                        if(usedPieceList.Count > 0)
                        {
                            Piece reusedPiece = usedPieceList[0];
                            reusedPiece.gameObject.SetActive(true);
                            newPiece = reusedPiece;

                            //Eltávolítjuk az újrahasznált piece-t a listáról
                            usedPieceList.RemoveAt(0);
                        }
                        //Ha nincs akkor meg csinálunk újat
                        else
                        {
                            GameObject obj = Instantiate(piecePrefab,gameBoard);
                            Piece createdPiece = obj.GetComponent<Piece>();
                            newPiece = createdPiece;
                        }
                        //Létrehozzuk a zuhanási pontra az új elemünket
                        newPiece.Initialize(newPieceValue, checkedPoint, dataController.elements[newPieceValue-1].returnSprite());
                        newPiece.rect.anchoredPosition = GetPositionFromPoint(fallPoint);
                        
                        //Betömjük a lyukat
                        GridCell hole = GetCellAtPoint(checkedPoint);
                        hole.AddPieceToCell(newPiece);
                        ResetPiece(newPiece);
                        fills[x]++;
                    }
                    break;
                }
            }
        }
    }

    void ApplyGravityToColumn(int x)
    {
        //Lentről felfele vizsgáljuk a gridet.
        for(int y = (height - 1); y >= 0; y--)
        {
            Point checkedPoint = new Point(x,y);
            GridCell cell = GetCellAtPoint(checkedPoint);
            int val = GetValueofPoint(checkedPoint);
            if (val != 0) continue;

            //A következő elemeket vizsgáljuk
            for(int my = (y-1); my >= -1; my--)
            {
                Point nextPoint = new Point(x,my);
                int nextval = GetValueofPoint(nextPoint);
                if(nextval == 0) continue;

                //Ha a következő pont nem egy lyuk
                if(nextval != -1)
                {
                    //Akkor a következő elemet magunkra húzzuk és lyukat hagyunk helyette
                    GridCell nextCell = GetCellAtPoint(nextPoint);
                    Piece nextPiece = nextCell.GetPieceFromCell();

                    cell.AddPieceToCell(nextPiece);
                    updatePieceList.Add(nextPiece);

                    nextCell.AddPieceToCell(null);
                }

                //Ha a következő elem egy lyuk
                else
                {
                    //Kérünk egy random értéket
                    int newPieceValue = GiveRandomPieceValue();
                    Piece newPiece;

                    //Beállítjuk az eséspontot
                    Point fallPoint = new Point(x, (-1 - fills[x]));

                    //Ha van már használt piece akkor újrahasznosítjuk
                    if(usedPieceList.Count > 0)
                    {
                        Piece reusedPiece = usedPieceList[0];
                        reusedPiece.gameObject.SetActive(true);
                        newPiece = reusedPiece;

                        //Eltávolítjuk az újrahasznált piece-t a listáról
                        usedPieceList.RemoveAt(0);
                    }
                    //Ha nincs akkor meg csinálunk újat
                    else
                    {
                        GameObject obj = Instantiate(piecePrefab,gameBoard);
                        Piece createdPiece = obj.GetComponent<Piece>();
                        newPiece = createdPiece;
                    }
                    //Létrehozzuk a zuhanási pontra az új elemünket
                    newPiece.Initialize(newPieceValue, checkedPoint, dataController.elements[newPieceValue-1].returnSprite());
                    newPiece.rect.anchoredPosition = GetPositionFromPoint(fallPoint);
                    
                    //Betömjük a lyukat
                    GridCell hole = GetCellAtPoint(checkedPoint);
                    hole.AddPieceToCell(newPiece);
                    ResetPiece(newPiece);
                    fills[x]++;
                }
                break;
            }
        }
    }





}
