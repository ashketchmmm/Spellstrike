using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public static Heart Instance;
    [SerializeField] public GameObject blue, orange, dead;
    [SerializeField] public SpriteRenderer heartrend;

    // Start is called before the first frame update

    void Awake()
    {
        blue.SetActive(false);
        orange.SetActive(false);
        dead.SetActive(false);
        Instance = this;
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Alive(bool first)
    {
        if (first)
        {
            blue.SetActive(true);
            dead.SetActive(false);
        }
        else
        {
            orange.SetActive(true);
            dead.SetActive(false);
        }
    }

    public void Dead()
    {
        blue.SetActive(false);
        orange.SetActive(false);
        dead.SetActive(true);
        //heartrend.color = Color.clear;
    }
}
