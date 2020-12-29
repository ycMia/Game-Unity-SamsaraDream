using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter_Bed_1 : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject egger;
    public float countSet = 0.5f;
    private float count;
    private int count_Egg;

    private bool viewAllowed;
    // Start is called before the first frame update
    void Start()
    {
        count_Egg = 0;
        count = countSet;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        egger.SetActive(false);
        viewAllowed = false;
    }

    public bool Interact()
    {
        if (count_Egg == 1)
        {
            egger.SetActive(true);
            egger.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if (count_Egg >= 1)
        {
            egger.SetActive(true);
            egger.GetComponent<SpriteRenderer>().sprite = sprites[(int)Random.Range(1, sprites.Length)];
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        count_Egg++;

        
        viewAllowed = true;
        return true;
    }

    // Update is called once per frame
    void Update()
    {

        if(viewAllowed)
        {
            if (count <= 0)
            {
                count = countSet;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                egger.SetActive(false);
                viewAllowed = false;
            }
            else
            {
                count -= Time.deltaTime;
            }
        }
    }
}
