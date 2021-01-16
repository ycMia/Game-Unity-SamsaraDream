using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed_1 : MonoBehaviour
{
    public Sprite[] sprites;
    private float count;
    public float countSet = 0.5f;

    private bool viewAllowed;
    // Start is called before the first frame update
    void Start()
    {
        count = countSet;
        gameObject.transform.GetChild(0).gameObject.SetActive(false); //这是名为tip的gameObject
        viewAllowed = false;
    }

    public bool Interact()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        viewAllowed = true;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if(viewAllowed)
        {
            if(count<=0)
            {
                count = countSet;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                viewAllowed = false;
            }
            else
            {
                count -= Time.deltaTime;
            }
        }
    }
}
