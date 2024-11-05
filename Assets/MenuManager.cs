using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using static GameManager;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject Menu, ActionMenu, Credits, TokenButton, Rules, RulesOpen;
    public bool menuopen = true;

    // Start is called before the first frame update
    void Start()
    {
        Rules.SetActive(false);
        TokenButton.SetActive(false);
        Credits.SetActive(false);
        ActionMenu.SetActive(false);
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if ((GameManager.Instance.State != GameManager.GameState.Player1Spell) || (GameManager.Instance.State != GameManager.GameState.Player2Spell))
        //{
        //    TokenButton.SetActive(false);
        //    Button tokenuse = TokenButton.GetComponent<Button>();
        //    tokenuse.interactable = false;
        //}
    }

    public void CloseMenu()
    {
        Menu.SetActive(false);
        ActionMenu.SetActive(true); 
        menuopen = false;
        GameManager.Instance.UpdateGameState(GameState.Player1Move);
    }

    public void OpenCredits()
    {
        Menu.SetActive(false);
        Credits.SetActive(true);
    }
    public void CloseCredits()
    {
        Menu.SetActive(true);
        Credits.SetActive(false);
    }

    public void OpenRules()
    {
        Menu.SetActive(false);
        Rules.SetActive(true);
        RulesOpen.SetActive(false);
        ActionMenu.SetActive(false);
    }
    public void CloseRules()
    {
        if (menuopen) Menu.SetActive(true);
        else Menu.SetActive(false);
        RulesOpen.SetActive(true);
        Rules.SetActive(false);
        ActionMenu.SetActive(true);

    }


    //public void OpenUseTokens(Player player)
    //{
    //    //Button tokenuse = TokenButton.GetComponent<Button>();
    //    //tokenuse.interactable = true;

    //    if (player == PlayerMain.Instance.playerOne)
    //    {
    //        if (GameManager.Instance.State == GameManager.GameState.Player1Spell)
    //        {
    //            TokenButton.SetActive(true);
    //            Button tokenuse = TokenButton.GetComponent<Button>();
    //            tokenuse.interactable = true;
    //        }
    //        else
    //        {
    //            TokenButton.SetActive(false);
    //        }
    //    }
    //    else if (player == PlayerMain.Instance.playerTwo)
    //    {
    //        //if (GameManager.Instance.State == GameManager.GameState.Player2Spell) TokenButton.SetActive(true);
    //        if (GameManager.Instance.State == GameManager.GameState.Player2Spell)
    //        {
    //            TokenButton.SetActive(true);
    //            Button tokenuse = TokenButton.GetComponent<Button>();
    //            tokenuse.interactable = true;
    //        }
    //        else
    //        {
    //            TokenButton.SetActive(false);
    //        }
    //    }

    //}

    public void OpenUseTokens()
    {

        if ((PlayerMain.Instance.playerOne.tokens == 2) && (GameManager.Instance.State == GameManager.GameState.Player1Spell))
        {
            TokenButton.SetActive(true);
            Button tokenuse = TokenButton.GetComponent<Button>();
            tokenuse.interactable = true;

        }
        else if ((PlayerMain.Instance.playerTwo.tokens == 2) && (GameManager.Instance.State == GameManager.GameState.Player2Spell))
        {
            TokenButton.SetActive(true);
            Button tokenuse = TokenButton.GetComponent<Button>();
            tokenuse.interactable = true;

        }
        else
        {
            TokenButton.SetActive(false);

        }
    }

    public void CloseUseTokens(Player player)
    {
        if (player == PlayerMain.Instance.playerOne)
        {
            if (GameManager.Instance.State == GameManager.GameState.Player1Spell)
            {
                TokenButton.SetActive(false);
                Button tokenuse = TokenButton.GetComponent<Button>();
                tokenuse.interactable = false;
            }

        }
        else if (player == PlayerMain.Instance.playerTwo)
        {
            if (GameManager.Instance.State == GameManager.GameState.Player2Spell){
                TokenButton.SetActive(false);
                Button tokenuse = TokenButton.GetComponent<Button>();
                tokenuse.interactable = false;
            }
        }

        //Button tokenuse = TokenButton.GetComponent<Button>();
        //tokenuse.interactable = false;
    }

}
