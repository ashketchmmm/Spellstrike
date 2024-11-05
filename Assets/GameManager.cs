using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Start:
                break;
            case GameState.Player1Move:
                SpellManager.Instance.ResetSpells();
                //SpellManager.Instance.spell2 = 0;
                break;
            case GameState.Player1Spell:
                TurnManager.Instance.ShowDeck();
                break;
            case GameState.Player2Move:
                break;
            case GameState.Player2Spell:
                TurnManager.Instance.ShowDeck();
                break;
            case GameState.ShowMoves:
                PlayerMain.Instance.ShowMoves();
                TokenSystem.Instance.CheckTokens();
                break;
            case GameState.ShowSpell1:
                SpellManager.Instance.ShowSpell1(SpellManager.Instance.tokens1);
                break;
            case GameState.ShowSpell2:
                //SpellManager.Instance.spell1 = 0;
                SpellManager.Instance.ShowSpell2(SpellManager.Instance.tokens2);
                break;
            case GameState.Speed1:
                SpellManager.Instance.SpellSpeed1(SpellManager.Instance.tokens1);
                TokenSystem.Instance.CheckTokens();

                //SpellManager.Instance.spell1 = 0;
                break;
            case GameState.Speed2:
                //GridManager.Instance.ClearGrid();
                SpellManager.Instance.SpellSpeed2(SpellManager.Instance.tokens2);
                TokenSystem.Instance.CheckTokens();

                SpellManager.Instance.spell2 = 0;
                break;
            case GameState.Victory1:
                TurnManager.Instance.Victory(true);
                break;
            case GameState.Victory2:
                TurnManager.Instance.Victory(false);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);

    }

    public enum GameState
    {
        Start,
        Player1Move,
        Player1Spell,
        Player2Move,
        Player2Spell,
        ShowMoves,
        ShowSpell1,
        ShowSpell2,
        Speed1,
        Speed2,
        Victory1,
        Victory2
    }
}
