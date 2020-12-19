using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter_Coin : MonoBehaviour
{
    public Gamemanager gm;

    private float destroyingRemaining;
    private float originColorAlphaValue;
    public bool onDestroying = false;
    public float score;
    public float destroyRemainTime_Set; //Seconds
    public GameObject tiper;
    
    void Start()
    {
        tiper.SetActive(false); // Warn here
    }

    public void DeleteSelf()
    {
        if (onDestroying)
            return;
        gm.AddScore(score);
        destroyingRemaining = 0f;
        tiper.SetActive(true);
        onDestroying = true;
        //gameObject.SetActive(false);
        originColorAlphaValue = tiper.GetComponent<SpriteRenderer>().color.a;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0f);
    }
    
    void Update()
    {
        if(onDestroying)
        {
            destroyingRemaining += Time.deltaTime;
            float step = ( Time.deltaTime / destroyRemainTime_Set ) * originColorAlphaValue;
            if(destroyingRemaining>=destroyRemainTime_Set)
            {
                Destroy(tiper);
                Destroy(gameObject);
            }
            tiper.GetComponent<SpriteRenderer>().color = new Color(
                tiper.GetComponent<SpriteRenderer>().color.r,
                tiper.GetComponent<SpriteRenderer>().color.g,
                tiper.GetComponent<SpriteRenderer>().color.b,
                tiper.GetComponent<SpriteRenderer>().color.a - step);
            tiper.transform.position += new Vector3(0f, 0.01f, 0f);
        }
    }
}
