using System.Collections;
using System.Collections.Generic;
using TMPro;

//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;
using Button = UnityEngine.UI.Button;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerMain playerManager;
    [SerializeField] private SpellManager spellManager;
    [SerializeField] public int tracker=0, spell1, spell2;
    [SerializeField] private Player playerOne, playerTwo;
    [SerializeField] public GameObject button_skipMove, button_showDeck, button_skipSpell, victoryScreen;
    [SerializeField] private bool deckShowing = false;
    [SerializeField] private TextMeshProUGUI victoryText;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        victoryScreen.SetActive(false);
        playerOne = playerManager.playerOne;
        playerTwo = playerManager.playerTwo;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.Instance.State == GameState.Player1Move) || (GameManager.Instance.State == GameState.Player2Move))
        {
            deckShowing = false;
            SetMove();
        }

        if (deckShowing)
        {
            Button skipm = button_skipMove.GetComponent<Button>();
            skipm.interactable = false;
            Button skips = button_skipSpell.GetComponent<Button>();
            skips.interactable = true;
        }
        else if (!deckShowing)
        {
            Button skipm = button_skipMove.GetComponent<Button>();
            skipm.interactable = true;
            Button skips = button_skipSpell.GetComponent<Button>();
            skips.interactable = false;
        }

    }

    public void SetMove()
    {
        if (GameManager.Instance.State == GameState.Player1Move)
        {
            playerManager.CheckRange(playerOne);
        }
        else if (GameManager.Instance.State == GameState.Player2Move)
        {
            playerManager.CheckRange(playerTwo);
        }
        else
        {
            gridManager.ClearGrid();
        }
    }

    public void SkipMove()
    {
        GridManager.Instance.ClearGrid();

        if (GameManager.Instance.State == GameState.Player1Move)
        {
            HideDeck();
            Vector2 currentpos = new Vector2(PlayerMain.Instance.p1_x, PlayerMain.Instance.p1_y);
            Tile current = GridManager.Instance.GetPosition(currentpos);
            current.occupied = true;

            //tracker += 1;
            //GameManager.Instance.UpdateGameState(GameState.Player2Move);
            GameManager.Instance.UpdateGameState(GameState.Player1Spell);
        }
        else if (GameManager.Instance.State == GameState.Player2Move)
        {
            HideDeck();
            Vector2 currentpos = new Vector2(PlayerMain.Instance.p2_x, PlayerMain.Instance.p2_y);
            Tile current = GridManager.Instance.GetPosition(currentpos);
            current.occupied = true;

            //tracker += 1;
            //GameManager.Instance.UpdateGameState(GameState.Show);
            GameManager.Instance.UpdateGameState(GameState.Player2Spell);

        }
    }
    public void ShowDeck()
    {
        if (GameManager.Instance.State == GameState.Player1Spell || GameManager.Instance.State == GameState.Player1Move)
        {
            if (GameManager.Instance.State == GameState.Player1Move)
            {
                SpellManager.Instance.NoSpell();
            }
            else
            {
                SpellManager.Instance.YesSpell();
            }

            SpellManager.Instance.SetHand(SpellManager.Instance.deck1);
            SpellManager.Instance.deck.SetActive(true);
        }
        else if (GameManager.Instance.State == GameState.Player2Spell || GameManager.Instance.State == GameState.Player2Move)
        {
            if (GameManager.Instance.State == GameState.Player2Move)
            {
                SpellManager.Instance.NoSpell();
            }
            else
            {
                SpellManager.Instance.YesSpell();
            }

            SpellManager.Instance.SetHand(SpellManager.Instance.deck2);
            SpellManager.Instance.deck.SetActive(true);
        }

        deckShowing = true;
        button_showDeck.SetActive(false);
    }

    public void HideDeck()
    {
        SpellManager.Instance.deck.SetActive(false);
        button_showDeck.SetActive(true);
    }

    public void SkipSpell()
    {
        if (GameManager.Instance.State == GameState.Player1Spell)
        {
            HideDeck();
            GameManager.Instance.UpdateGameState(GameState.Player2Move);
        }
        else if (GameManager.Instance.State == GameState.Player2Spell)
        {
            HideDeck();
            GameManager.Instance.UpdateGameState(GameState.ShowMoves);
        }

    }

    public void Victory(bool first)
    {
        victoryScreen.SetActive(true);

        if (first)
        {
            victoryText.text = "Player One wins!";
        }
        else
        {
            victoryText.text = "Player Two wins!";
        }
    }



}
