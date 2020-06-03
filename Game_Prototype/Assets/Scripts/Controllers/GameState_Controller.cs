using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, CharacterCreation, LoadGame, NewGame, Outworld, Travelling, Dialogue,
 IngameMenu, NPCbattle, RandomEncounter, BattleSetup, Battle, Win, Lose};

public class GameState_Controller : MonoBehaviour
{
    public GameState currentState;

    void Start()
    {
        //Játék indulásakor a világ létrehozásával kezdünk
        currentState = GameState.MainMenu;

        DontDestroyOnLoad (transform.gameObject);
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
