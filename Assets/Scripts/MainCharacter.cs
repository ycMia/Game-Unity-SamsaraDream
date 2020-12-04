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
            if (gm.AddJump() == true)
            {
                gm.AddJump();
            }   
            else
            {

            }
            // two times if touched ground
            // and a bit fixing
        }

        else if(collision.transform.gameObject.tag == "Ground_Side_Left")
        {
            
        }

        else if (collision.transform.gameObject.tag == "Ground_Side_Right")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
