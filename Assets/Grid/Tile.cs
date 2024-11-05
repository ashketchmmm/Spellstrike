using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor, _original;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight, _inRange;
    [SerializeField] public bool rangeCheck, occupied = false;
    [SerializeField] public int width, height;
    [SerializeField] public Player playerOne, playerTwo;

    private void Start()
    {
        playerOne = PlayerMain.Instance.playerOne;
        playerTwo = PlayerMain.Instance.playerTwo;
    }

    private void Update()
    {
        //Player playerOne = PlayerMain.Instance.playerOne;
        //Player playerTwo = PlayerMain.Instance.playerTwo;

        //if (GameManager.Instance.State == GameManager.GameState.Show)
        //{
        //    playerOne.transform.position = new Vector3(PlayerMain.Instance.p1_x, PlayerMain.Instance.p1_y, 0);
        //    playerTwo.transform.position = new Vector3(PlayerMain.Instance.p2_x, PlayerMain.Instance.p2_y, 0);
        //    GameManager.Instance.UpdateGameState(GameManager.GameState.Player1Move);
        //}
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _original = isOffset ? _offsetColor : _baseColor;
    }

    //public void ShowMoves()
    //{
    //    playerOne.transform.position = new Vector3(PlayerMain.Instance.p1_x, PlayerMain.Instance.p1_y, 0);
    //    playerTwo.transform.position = new Vector3(PlayerMain.Instance.p2_x, PlayerMain.Instance.p2_y, 0);
    //    GameManager.Instance.UpdateGameState(GameManager.GameState.Player1Move);
    //}

    private void OnMouseDown()
    {
        //Player playerOne = PlayerMain.Instance.playerOne;
        //Player playerTwo = PlayerMain.Instance.playerTwo;

        if ((rangeCheck == true) && (occupied != true) && 
            ((GameManager.Instance.State == GameManager.GameState.Player1Move) || (GameManager.Instance.State == GameManager.GameState.Speed1)))
        {
            PlayerMain.Instance.p1_x = width;
            PlayerMain.Instance.p1_y = height;

            GridManager.Instance.ClearGrid();
            occupied = true;

            //TokenSystem.Instance.CheckTokens();

            FindObjectOfType<AudioManager>().Play("Click");

            if (GameManager.Instance.State == GameManager.GameState.Player1Move) GameManager.Instance.UpdateGameState(GameManager.GameState.Player1Spell);
            
            else if (GameManager.Instance.State == GameManager.GameState.Speed1)
            {
                playerOne.transform.position = new Vector3(PlayerMain.Instance.p1_x, PlayerMain.Instance.p1_y, 0);
                FindObjectOfType<AudioManager>().Play("Speed");

                TokenSystem.Instance.CheckTokens();
                GameManager.Instance.UpdateGameState(GameManager.GameState.ShowSpell2);
            }
            
        }

        else if ((rangeCheck == true) && 
            ((GameManager.Instance.State == GameManager.GameState.Player2Move) || (GameManager.Instance.State == GameManager.GameState.Speed2)))
        {

            if (occupied != true)
            {
                PlayerMain.Instance.p2_x = width;
                PlayerMain.Instance.p2_y = height;

                //TokenSystem.Instance.CheckTokens();
            }
            else
            {
                int gridwidth = GridManager.Instance.GetWidth();
                int gridheight = GridManager.Instance.GetHeight();

                if ((0 < width) && (width < gridwidth))
                {
                    int test = 0;
                    while (test == 0) test = Random.Range(0, 2)*2-1;
                    PlayerMain.Instance.p2_x = width + test;
                }
                else if (width == 0)
                {
                    PlayerMain.Instance.p2_x = width + 1;
                }
                else
                {
                    PlayerMain.Instance.p2_x = width - 1;
                }

                if (height == 0)
                {
                    PlayerMain.Instance.p2_y = height + 1;
                }
                else if (height == gridheight - 1)
                {
                    PlayerMain.Instance.p2_y = height - 1;
                }
                else
                {
                    PlayerMain.Instance.p2_y = height;
                }
            }

            GridManager.Instance.ClearGrid();
            occupied = true;

            FindObjectOfType<AudioManager>().Play("Click");

            if (GameManager.Instance.State == GameManager.GameState.Player2Move) GameManager.Instance.UpdateGameState(GameManager.GameState.Player2Spell);
            else if (GameManager.Instance.State == GameManager.GameState.Speed2)
            {
                playerTwo.transform.position = new Vector3(PlayerMain.Instance.p2_x, PlayerMain.Instance.p2_y, 0);
                FindObjectOfType<AudioManager>().Play("Speed");

                TokenSystem.Instance.CheckTokens();
                GameManager.Instance.UpdateGameState(GameManager.GameState.Player1Move);
            }
        }

    }
    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    public void OutOfRange()
    {
        rangeCheck = false;
        occupied = false;
        _inRange.SetActive(false);
    }
    public void InRange()
    {
        rangeCheck = true;
        _inRange.SetActive(true);
    }
}
