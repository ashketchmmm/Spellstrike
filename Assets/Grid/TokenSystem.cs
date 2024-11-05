using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSystem : MonoBehaviour
{
    [SerializeField] public Token token1, token2, token3;
    [SerializeField] public bool firsttwo = true;
    [SerializeField] public bool first, second, spawntoken3;
    public static TokenSystem Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        first = true;
        second = true;
        spawntoken3 = false;

        token1 = Instantiate(token1, new Vector3(0, 8), Quaternion.identity);
        token1.width = 0;
        token1.height = 8;

        token2 = Instantiate(token2, new Vector3(8, 0), Quaternion.identity);
        token2.width = 8;
        token2.height = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!first) && (!second)) {
            firsttwo = false;
        }

        if ((!firsttwo) && (!spawntoken3)){
            spawntoken3 = true;
            Spawn3();
        }

        //CheckTokens(); 
    }

    public void CheckTokens()
    {
        if (firsttwo) {
            if ((token1.width == PlayerMain.Instance.p1_x) && (token1.height == PlayerMain.Instance.p1_y))
            {
                PlayerMain.Instance.playerOne.tokens += 1;
                Destroy(token1.GetComponent<SpriteRenderer>());
                first = false;

            }
            else if ((token1.width == PlayerMain.Instance.p2_x) && (token1.height == PlayerMain.Instance.p2_y))
            {
                PlayerMain.Instance.playerTwo.tokens += 1;
                Destroy(token1.GetComponent<SpriteRenderer>());
                first = false;
            }

            if ((token2.width == PlayerMain.Instance.p1_x) && (token2.height == PlayerMain.Instance.p1_y))
            {
                PlayerMain.Instance.playerOne.tokens += 1;
                Destroy(token2.GetComponent<SpriteRenderer>());
                second = false;
            }
            else if ((token2.width == PlayerMain.Instance.p2_x) && (token2.height == PlayerMain.Instance.p2_y))
            {
                PlayerMain.Instance.playerTwo.tokens += 1;
                Destroy(token2.GetComponent<SpriteRenderer>());
                second = false;
            }
        }
        else
        {
            if ((token3.width == PlayerMain.Instance.p1_x) && (token3.height == PlayerMain.Instance.p1_y))
            {
                if ((PlayerMain.Instance.playerOne.tokens < 2) && (PlayerMain.Instance.playerTwo.tokens != 2))
                {
                    PlayerMain.Instance.playerOne.tokens += 1;
                    //SpawnToken();
                }

                if ((PlayerMain.Instance.playerOne.tokens < 2) && (PlayerMain.Instance.playerTwo.tokens < 2))
                {
                    SpawnToken();
                }
                else
                {
                    //Destroy(token3.GetComponent<SpriteRenderer>());
                    token3.transform.position = new Vector3(-14, 2, 0);

                }

            }
            else if ((token3.width == PlayerMain.Instance.p2_x) && (token3.height == PlayerMain.Instance.p2_y))
            {
                if ((PlayerMain.Instance.playerTwo.tokens < 2) && (PlayerMain.Instance.playerOne.tokens != 2))
                {
                    PlayerMain.Instance.playerTwo.tokens += 1;
                    //SpawnToken();
                }

                if ((PlayerMain.Instance.playerOne.tokens < 2) && (PlayerMain.Instance.playerTwo.tokens < 2))
                {
                    SpawnToken();
                }
                else
                {
                    //Destroy(token3.GetComponent<SpriteRenderer>());
                    token3.transform.position = new Vector3(-14, 2, 0);

                }
            }

            //if ((PlayerMain.Instance.playerOne.tokens < 2) || (PlayerMain.Instance.playerTwo.tokens < 2)){
            //    SpawnToken();
            //}

        }
    }

    public void Spawn3()
    {
        token3 = Instantiate(token3, new Vector3(4, 4), Quaternion.identity);
        token3.width = 4;
        token3.height = 4;
    }

    public void SpawnToken()
    {

        int new_width = Random.Range(0, 9);
        int new_height = Random.Range(0, 9);

        while (((new_width == PlayerMain.Instance.p1_x) && (new_height == PlayerMain.Instance.p1_y)) || 
              ((new_width == PlayerMain.Instance.p2_x) && (new_height == PlayerMain.Instance.p2_y)))
        {
            new_width = Random.Range(0, 9);
            new_height = Random.Range(0, 9);
        }

        //token3.width = new_width;
        //token3.height = new_height;
        token3.transform.position = new Vector3(new_width, new_height, 0);
        token3.width = new_width;
        token3.height = new_height;
        //Update3(new_width, new_height);
    }

    public void UseTokensSpend()
    {
        if (GameManager.Instance.State == GameManager.GameState.Player1Spell)
        {
            PlayerMain.Instance.playerOne.tokens = 0;
            SpellManager.Instance.tokens1 = true;
            FindObjectOfType<AudioManager>().Play("Token");

        }
        else if (GameManager.Instance.State == GameManager.GameState.Player2Spell)
        {
            PlayerMain.Instance.playerTwo.tokens = 0;
            SpellManager.Instance.tokens2 = true;
            FindObjectOfType<AudioManager>().Play("Token");

        }
    }

}
