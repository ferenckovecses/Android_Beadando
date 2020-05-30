using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {World_Creation, Outworld, Travelling, Dialogue,
 IngameMenu, NPCbattle, RandomEncounter, BattleSetup, Battle, Win, Lose};

public class GameState_Controller : MonoBehaviour
{
    public GameState currentState;

    void Start()
    {
        //Játék indulásakor a világ létrehozásával kezdünk
        currentState = GameState.World_Creation;
    }

    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
    }

    public GameState GetGameState()
    {
        return this.currentState;
    }


}
