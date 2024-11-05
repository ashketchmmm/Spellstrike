using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{

    //[SerializeField] public Heart heart1, heart2, heart3;
    //[SerializeField] public Heart heart4, heart5, heart6;

    [SerializeField] public int health;

    public static HeartManager Instance;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth(PlayerMain.Instance.playerOne);
        UpdateHealth(PlayerMain.Instance.playerTwo);
    }
    public void CreateHearts(Player player)
    {

        if (player.first)
        {
            player.heart1 = Instantiate(player.heart1, new Vector3(-4.3f, 8.3f), Quaternion.identity);
            player.heart2 = Instantiate(player.heart2, new Vector3(-3.3f, 8.3f), Quaternion.identity);
            player.heart3 = Instantiate(player.heart3, new Vector3(-2.3f, 8.3f), Quaternion.identity);

        }
        else
        {
            player.heart1 = Instantiate(player.heart1, new Vector3(-4.3f, 7.3f), Quaternion.identity);
            player.heart2 = Instantiate(player.heart2, new Vector3(-3.3f, 7.3f), Quaternion.identity);
            player.heart3 = Instantiate(player.heart3, new Vector3(-2.3f, 7.3f), Quaternion.identity);
        }

        player.heart1.Alive(player.first);
        player.heart2.Alive(player.first);
        player.heart3.Alive(player.first);

    }

    public void UpdateHealth(Player player)
    {
        //bool first = false;

        //if (player == PlayerMain.Instance.playerOne)
        //{
        //    first = true;
        //}
        bool first = player.first;

        if (player.health == 3)
        {
            player.heart1.Alive(first);
            player.heart2.Alive(first);
            player.heart3.Alive(first);
        }
        else if (player.health == 2)
        {
            player.heart1.Dead();
            player.heart2.Alive(first);
            player.heart3.Alive(first);
        }
        else if (player.health == 1)
        {
            player.heart1.Dead();
            player.heart2.Dead();
            player.heart3.Alive(first);
        }
        else
        {
            player.heart1.Dead();
            player.heart2.Dead();
            player.heart3.Dead();
        }
    }
}
