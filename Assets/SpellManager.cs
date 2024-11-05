using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameManager;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;
    [SerializeField] public int attack = 20, heal = 10, speed = 5; // 4:2:1
    [SerializeField] public int spell1, spell2;
    [SerializeField] public int[] deck1, deck2;
    [SerializeField] public GameObject deck, card1, card2, card3;
    [SerializeField] TextMeshProUGUI name1, name2, name3;
    [SerializeField] public bool tokens1, tokens2;
    [SerializeField] public Sprite AttackCard, HealCard, SpeedCard;


    private void Awake()
    {
        Instance = this;
        Sprite AttackCard = Resources.Load<Sprite>("Sprites/AttackCard");
        Sprite HealCard = Resources.Load<Sprite>("Sprites/HealCard");

        tokens1 = false;
        tokens2 = false;
        deck.SetActive(false);
        deck1 = new int[3];
        deck2 = new int[3];
    }

    // Start is called before the first frame update
    void Start()
    {
        DealHands(deck1);
        DealHands(deck2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DealHands(int[] deck) // need to implement deck limits
    {
        for (int i=0; i<3; i++)
        {
            int card = Random.Range(1, 7);
            deck[i] = card;
        }
    }
    public void SetHand(int[] deck)
    {
        TextMeshProUGUI[] names = {name1, name2, name3};

        for (int i=0; i<3; i++)
        {
            if (deck[i] == 1)
            {

                if (i == 0)
                {
                    card1.GetComponent<Image>().sprite = SpeedCard;
                }
                else if (i == 1)
                {
                    card2.GetComponent<Image>().sprite = SpeedCard;
                }
                else
                {
                    card3.GetComponent<Image>().sprite = SpeedCard;
                }
            }
            else if ((deck[i] == 2) || (deck[i] == 3)){

                if (i == 0)
                {
                    card1.GetComponent<Image>().sprite = HealCard;
                }
                else if (i == 1)
                {
                    card2.GetComponent<Image>().sprite = HealCard;
                }
                else
                {
                    card3.GetComponent<Image>().sprite = HealCard;
                }
            }
            else
            {

                if (i == 0)
                {
                    card1.GetComponent<Image>().sprite = AttackCard;
                }
                else if (i == 1)
                {
                    card2.GetComponent<Image>().sprite = AttackCard;
                }
                else
                {
                    card3.GetComponent<Image>().sprite = AttackCard;
                }
            }
        }

    }


    public void NewCard(int[] deck, int pos)
    {
        int newcard = Random.Range(1, 7);
        deck[pos] = newcard;

        if (pos == 1)
        {
            if (newcard == 1) card1.GetComponent<Image>().sprite = SpeedCard;
            
            else if ((newcard == 2) || (newcard == 3)) card1.GetComponent<Image>().sprite = HealCard;

            else card1.GetComponent<Image>().sprite = AttackCard;
        }
        else if (pos == 2)
        {
            if (newcard == 1) card2.GetComponent<Image>().sprite = SpeedCard;

            else if ((newcard == 2) || (newcard == 3)) card2.GetComponent<Image>().sprite = HealCard;

            else card2.GetComponent<Image>().sprite = AttackCard;
        }
        else
        {
            if (newcard == 1) card3.GetComponent<Image>().sprite = SpeedCard;

            else if ((newcard == 2) || (newcard == 3)) card3.GetComponent<Image>().sprite = HealCard;

            else card3.GetComponent<Image>().sprite = AttackCard;
        }

    }

    public void NoSpell()
    {
        Button card1_pick = card1.GetComponent<Button>();
        card1_pick.interactable = false;

        Button card2_pick = card2.GetComponent<Button>();
        card2_pick.interactable = false;

        Button card3_pick = card3.GetComponent<Button>();
        card3_pick.interactable = false;
    }

    public void YesSpell()
    {
        Button card1_pick = card1.GetComponent<Button>();
        card1_pick.interactable = true;

        Button card2_pick = card2.GetComponent<Button>();
        card2_pick.interactable = true;

        Button card3_pick = card3.GetComponent<Button>();
        card3_pick.interactable = true;
    }

    public void ChooseSpell()
    {
        GameObject card = EventSystem.current.currentSelectedGameObject;
        //spell1 = 0;
        spell2 = 0;

        if (GameManager.Instance.State == GameState.Player1Spell)
        {
            if (card == card1)
            {
                TurnManager.Instance.spell1 = deck1[0];
                spell1 = deck1[0];
                NewCard(deck1, 0);
            }
            else if (card == card2)
            {
                TurnManager.Instance.spell1 = deck1[1];
                spell1 = deck1[1];
                NewCard(deck1, 1);
            }
            else
            {
                TurnManager.Instance.spell1 = deck1[2];
                spell1 = deck1[2];
                NewCard(deck1, 2);
            }

            GameManager.Instance.UpdateGameState(GameState.Player2Move);
        }

        if (GameManager.Instance.State == GameState.Player2Spell)
        {
            if (card == card1)
            {
                TurnManager.Instance.spell2 = deck2[0];
                spell2 = deck2[0];
                NewCard(deck2, 0);
            }
            else if (card == card2)
            {
                TurnManager.Instance.spell2 = deck2[1];
                spell2 = deck2[1];
                NewCard(deck2, 1);
            }
            else
            {
                TurnManager.Instance.spell2 = deck2[2];
                spell2 = deck2[2];
                NewCard(deck2, 2);
            }

            GameManager.Instance.UpdateGameState(GameState.ShowMoves);
        }

        TurnManager.Instance.HideDeck();

    }
    public void ShowSpell1(bool spelltoken1)
    {
        int dice = Random.Range(1, 6);
        bool inreach = PlayerMain.Instance.PlayerRange();

        if (dice == 1)
        {
            FindObjectOfType<AudioManager>().Play("Fail");
            GameManager.Instance.UpdateGameState(GameState.ShowSpell2);
        }

        if ((spell1 > 3))
        {
            //if (inreach) PlayerMain.Instance.playerTwo.health -= 1;
            //spell1 = 0;
            if (inreach && !spelltoken1) PlayerMain.Instance.playerTwo.health -= 1;
            else if (inreach && spelltoken1) PlayerMain.Instance.playerTwo.health -= 2;
            FindObjectOfType<AudioManager>().Play("Attack");

            tokens1 = false;
            GameManager.Instance.UpdateGameState(GameState.ShowSpell2);
        }
        else if ((spell1 == 2) || (spell1 == 3))
        {
            PlayerMain.Instance.playerOne.health = Mathf.Min(3, PlayerMain.Instance.playerOne.health + 2);
            FindObjectOfType<AudioManager>().Play("Heal");
            //spell1 = 0;

            tokens1 = false;
            GameManager.Instance.UpdateGameState(GameState.ShowSpell2);
        }
        else if (spell1 == 1)
        {
            GameManager.Instance.UpdateGameState(GameState.Speed1);
        }
        else if (spell1 == 0)
        {
            GameManager.Instance.UpdateGameState(GameState.ShowSpell2);
        }

        //tokens1 = false;
    }
    public void ShowSpell2(bool spelltoken2)
    {
        int dice = Random.Range(1, 6);
        bool inreach = PlayerMain.Instance.PlayerRange();

        if (dice == 1)
        {
            FindObjectOfType<AudioManager>().Play("Fail");
            GameManager.Instance.UpdateGameState(GameState.Player1Move);
        }

        if ((spell2 > 3))
        {
            if (inreach && !spelltoken2) PlayerMain.Instance.playerOne.health -= 1;
            else if (inreach && spelltoken2) PlayerMain.Instance.playerOne.health -= 2;
            FindObjectOfType<AudioManager>().Play("Attack");

            tokens2 = false;
            GameManager.Instance.UpdateGameState(GameState.Player1Move);
        }
        else if ((spell2 == 2) || (spell2 == 3))
        {
            PlayerMain.Instance.playerTwo.health = Mathf.Min(3, PlayerMain.Instance.playerTwo.health + 2);
            FindObjectOfType<AudioManager>().Play("Heal");

            tokens2 = false;
            GameManager.Instance.UpdateGameState(GameState.Player1Move);
        }
        else if (spell2 == 1)
        {
            GameManager.Instance.UpdateGameState(GameState.Speed2);
        }
        else if (spell2 == 0)
        {
            GameManager.Instance.UpdateGameState(GameState.Player1Move);
        }

        //tokens2 = false;
    }

    public void ResetSpells()
    {
        spell1 = 0;
        spell2 = 0;
    }
    public void SpellSpeed1(bool speedtoken1)
    {
        if (!speedtoken1) PlayerMain.Instance.CheckRange(PlayerMain.Instance.playerOne);
        else if (speedtoken1) PlayerMain.Instance.CheckRangeTwo(PlayerMain.Instance.playerOne);

        //PlayerMain.Instance.CheckRange(PlayerMain.Instance.playerOne);
        tokens1 = false;
    }
    public void SpellSpeed2(bool speedtoken2)
    {
        if (!speedtoken2) PlayerMain.Instance.CheckRange(PlayerMain.Instance.playerTwo);
        else if (speedtoken2) PlayerMain.Instance.CheckRangeTwo(PlayerMain.Instance.playerTwo);

        //PlayerMain.Instance.CheckRange(PlayerMain.Instance.playerTwo);
        tokens2 = false;
    }

}
