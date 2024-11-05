using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;
using static UnityEngine.ParticleSystem;

public class PlayerMain : MonoBehaviour
{
    public static PlayerMain Instance;
    [SerializeField] public Player playerOne, playerTwo;
    [SerializeField] private GridManager gridManager;
    [SerializeField] public int p1_x, p1_y, p2_x, p2_y = 0;
    [SerializeField] public bool p1_move, p2_move = false;
    [SerializeField] public TextMeshProUGUI turnIndicator;

    void Awake()
    {
        Instance = this;

        int width = gridManager.GetWidth() - 1;
        int height = gridManager.GetHeight() - 1;

        playerOne = Instantiate(playerOne, new Vector3(0, 0), Quaternion.identity);
        playerOne.name = $"Player One";
        playerOne.first = true;
        p1_x = 0; p1_y = 0;

        playerTwo = Instantiate(playerTwo, new Vector3(width, height), Quaternion.identity);
        playerTwo.name = $"Player Two";
        playerTwo.first = false;
        p2_x = width; p2_y = height;

        playerOne.Init(true);
        playerTwo.Init(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        HeartManager.Instance.CreateHearts(playerOne);
        HeartManager.Instance.CreateHearts(playerTwo);

    }

    // Update is called once per frame
    void Update()
    {
        if ((playerOne.health == 0) || (playerOne.health < 0))
        {
            GameManager.Instance.UpdateGameState(GameState.Victory2);
        }
        else if ((playerTwo.health == 0) || (playerTwo.health < 0))
        {
            GameManager.Instance.UpdateGameState(GameState.Victory1);
        }

        if ((GameManager.Instance.State == GameState.Player1Move) || (GameManager.Instance.State == GameState.Player1Spell) || (GameManager.Instance.State == GameState.Speed1))
        {
            playerOne.active.SetActive(true);
            playerTwo.active.SetActive(false);
            turnIndicator.text = "Player 1 Turn";

        }
        else if ((GameManager.Instance.State == GameState.Player2Move) || (GameManager.Instance.State == GameState.Player2Spell) || (GameManager.Instance.State == GameState.Speed2))
        { 
            playerOne.active.SetActive(false);
            playerTwo.active.SetActive(true);
            turnIndicator.text = "Player 2 Turn";
        }
        else
        {
            playerOne.active.SetActive(false);
            playerTwo.active.SetActive(false);
            turnIndicator.text = " ";
        }

        //UseTokens(playerOne);
        //UseTokens(playerTwo);
        UseTokens();

    }
    public void ShowMoves()
    {
        playerOne.transform.position = new Vector3(p1_x, p1_y, 0);
        playerTwo.transform.position = new Vector3(p2_x, p2_y, 0);
        GameManager.Instance.UpdateGameState(GameManager.GameState.ShowSpell1);
    }
    public void CheckRange(Player player)
    {
        // Vector2 current;
        int current_x, current_y;

        if (player == playerOne)
        {
            current_x = p1_x; current_y = p1_y;
            p1_move = true;
            p2_move = false;
        }
        else
        {
            current_x = p2_x; current_y = p2_y;
            p1_move = false;
            p2_move = true;
        }

        Vector2[] reachableCoords = GetInRangeTiles(current_x, current_y);
        for (int i = 0; i < reachableCoords.Length; i++)
        {
            Tile curr_tile = gridManager.GetPosition(reachableCoords[i]);

            if (curr_tile != null)
            {
                curr_tile.InRange();
            }
        }
    }

    public void CheckRangeTwo(Player player)
    {
        int current_x, current_y;

        if (player == playerOne)
        {
            current_x = p1_x; current_y = p1_y;
            p1_move = true;
            p2_move = false;
        }
        else
        {
            current_x = p2_x; current_y = p2_y;
            p1_move = false;
            p2_move = true;
        }

        Vector2[] reachableCoords = GetInRangeTiles(current_x, current_y, true);
        for (int i = 0; i < reachableCoords.Length; i++)
        {
            Tile curr_tile = gridManager.GetPosition(reachableCoords[i]);

            if (curr_tile != null)
            {
                curr_tile.InRange();
            }
        }
    }
    public bool PlayerRange(bool token = false)
    {
        if (!token)
        {
            Vector2[] range = GetInRangeTiles(p1_x, p1_y);
            if (range.Contains(new Vector2(p2_x, p2_y)))
            {
                return true;
            }
        }
        return false;
    }

    public Vector2[] GetInRangeTiles(int x, int y, bool speed = false)
    {
        int minXRange = Math.Max(0, x-2);
        int maxXRange = Math.Min(gridManager.GetWidth(), x+2);
        int minYRange = Math.Max(0, y-2);
        int maxYRange = Math.Min(gridManager.GetHeight(), y+2);

        bool IsValidTile(int row, int col) 
        {
            if (row < minXRange || row > maxXRange || col < minYRange || col > maxYRange)
            {
                return false;
            }
            return true;
        }

        HashSet<Vector2> oneMoveReachableTiles = new HashSet<Vector2>{new Vector2(x, y)};
        HashSet<Vector2> reachableTiles = new HashSet<Vector2>{new Vector2(x, y)};
        Vector2[] allDirections = {Vector2.down, Vector2.left, Vector2.right, Vector2.up, new Vector2(1,1), new Vector2(-1,-1), new Vector2(1,-1), new Vector2(-1,1)};

        for (int i = 0; i < allDirections.Length; i++)
        {
            int x2 = x + (int)allDirections[i].x;
            int y2 = y + (int)allDirections[i].y;
            if (!IsValidTile(x2, y2))
            {
                continue;
            }

            if (gridManager.IsReachable(x, y, x2, y2))
            {
                oneMoveReachableTiles.Add(new Vector2(x2, y2));
                reachableTiles.Add(new Vector2(x2, y2));
            }
        }

        for (int i = 0; i < allDirections.Length; i++)
        {
            int newX = x + (int)allDirections[i].x;
            int newY = y + (int)allDirections[i].y;
            if (oneMoveReachableTiles.Contains(new Vector2(newX, newY)))
            {
                for (int j = 0; j < allDirections.Length; j++)
                {
                    int x2 = newX + (int)allDirections[j].x;
                    int y2 = newY + (int)allDirections[j].y;

                    if (!IsValidTile(x2, y2))
                    {
                        continue;
                    }
                    if (gridManager.IsReachable(newX, newY, x2, y2))
                    {
                        reachableTiles.Add(new Vector2(x2, y2));
                    }
                }
            }
        }

        if (speed)
        {
            reachableTiles.ExceptWith(oneMoveReachableTiles);
            HashSet<Vector2> allReachTiles = new HashSet<Vector2>();
            foreach (Vector2 coord in reachableTiles)
            {
                allReachTiles.UnionWith(new HashSet<Vector2>(GetInRangeTiles((int)coord.x, (int)coord.y)));
            }
            return allReachTiles.ToArray();
        }

        return reachableTiles.ToArray();
    }
    
    //public void UseTokens(Player player)
    //{
    //    if (player.tokens == 2)
    //    {
    //        MenuManager.Instance.OpenUseTokens(player);
    //    }
    //    //else
    //    //{
    //    //    MenuManager.Instance.CloseUseTokens(player);
    //    //}
    //}

    public void UseTokens()
    {
        MenuManager.Instance.OpenUseTokens();
        //else
        //{
        //    MenuManager.Instance.CloseUseTokens(player);
        //}
    }
}