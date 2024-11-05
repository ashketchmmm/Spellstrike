using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Player : MonoBehaviour
{
    [SerializeField] private Color oneColor, twoColor;
    [SerializeField] public GameObject active, blue, orange;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] public int health = 3;
    [SerializeField] public int tokens = 0;
    [SerializeField] public Heart heart1, heart2, heart3;
    [SerializeField] public bool first;
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
        blue.SetActive(false); 
        orange.SetActive(false); 
        active.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(bool isFirst)
    {
        first = isFirst;

        //_renderer.color = isFirst ? oneColor : twoColor;
        if (isFirst)
        {
            blue.SetActive(true);
        }
        else
        {
            orange.SetActive(true);
        }
    }

    //public void BlueMove()
    //{
    //    bluerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 255);
    //    orangerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 60);
    //}
    //public void OrangeMove()
    //{
    //    orangerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 255);
    //    bluerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 60);
    //}

    //public void SpellMove()
    //{
    //    orangerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 255);
    //    bluerend.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 255);
    //}

}
