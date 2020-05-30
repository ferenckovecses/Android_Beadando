using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Travel_Controller : MonoBehaviour
{
    //Lista a travel pontok kapcsolataival
    public List<GateConnection> gateConnections;
    public Animator transitionAnim;

    Data_Controller dataController;
    GameState_Controller gameState;

    void Start()
    {
        //Vezérlők referálása
        gameState = GameObject.Find("GameState").GetComponent<GameState_Controller>();
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
    }

    public void TravelThrough(Gates startPoint)
    {
        //Megváltoztajtuk a játék státuszát
        gameState.ChangeGameState(GameState.Travelling);

        //Lekérjük a másik oldalt
        Gates destination = GetOther(startPoint);

        //Ha van eredmény cselekszünk
        if(destination != null)
        {
            //Megváltoztatja az aktív stage ID-t a destinationére
            dataController.ChangeLevelId(destination.stageID);
            //Elindít egy coroutine-t az animációval és a pályaválasztással
            StartCoroutine(ChangeLevel(destination.gateName)); 
        }

        //Folytatjuk a játékot
        gameState.ChangeGameState(GameState.Outworld);

    }

    IEnumerator ChangeLevel(string destination)
    {
        //Elsötétülés
        transitionAnim.Play("Fade");
        yield return new WaitForSeconds(transitionAnim.GetCurrentAnimatorStateInfo(0).length/1.5f);

        //Sötéten tartás
        transitionAnim.Play("Black");
        yield return new WaitForSeconds(transitionAnim.GetCurrentAnimatorStateInfo(0).length/1.5f);

        //Pálya csere
        GameObject.Find("Game").GetComponent<Game_Controller>().ChangeLevel(destination);

        //Sötétség feloldása
        transitionAnim.Play("Reveal");
        yield return new WaitForSeconds(transitionAnim.GetCurrentAnimatorStateInfo(0).length/1.5f);

    }

    bool AnimatorIsPlaying()
    {
     return transitionAnim.GetCurrentAnimatorStateInfo(0).length >
            transitionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
  }

    
    public Gates GetOther(Gates gate)
    {
        for(var i = 0; i < gateConnections.Count; i++)
        {
            if(gateConnections[i].goesFrom == gate)
            {
                return gateConnections[i].goesTo;
            }
            else if(gateConnections[i].goesTo == gate)
            {
                return gateConnections[i].goesFrom;
            }
        }

        return null;
    }

}

[System.Serializable]
public class GateConnection
{
    public Gates goesFrom;
    public Gates goesTo;

    public GateConnection(Gates goesFrom, Gates goesTo)
    {
        this.goesFrom = goesFrom;
        this.goesTo = goesTo;
    }

}