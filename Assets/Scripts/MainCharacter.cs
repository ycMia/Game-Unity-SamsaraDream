//MainCharacter.cs
//
//
//
//
//
//
//
//
//
//
//
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public Gamemanager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.transform.gameObject.tag == "Ground" )
        {
            gm.g_grounded = true;
            if (gm.AddJump() == true)
                gm.AddJump();
            // two times if touched ground
            // and a bit fixing
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Left")
        {
            gm.g_movementJurisdiction[1] = false;// right denied
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Right")
        {
            gm.g_movementJurisdiction[0] = false;// left denied
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Interactive_Coin")
        {
            collision.gameObject.GetComponent<Interacter_Coin>().DeleteSelf();
        }

        if (collision.transform.gameObject.tag == "Interactive_Bed")
        {
            gm.nowInteracting = collision;
            gm.SwitchKeyMode(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Interactive_Bed")
        {
            gm.nowInteracting = null; //[Tip][20201225]这里逻辑绝对有问题
            gm.SwitchKeyMode(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.tag == "Ground")
        {
            gm.g_grounded = false;
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Left")
        {
            gm.g_movementJurisdiction[1] = true; //right allowed
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Right")
        {
            gm.g_movementJurisdiction[0] = true; //left allowed
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
