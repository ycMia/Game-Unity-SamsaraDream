using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Gamemanager : MonoBehaviour
{
    public Canvas g_canvas;

    public bool g_allowUI;
    public GameObject UIContainer;
    
    public Buttoner[] buttoners;
    public GameObject mC; //MainCharacter
    public Rigidbody2D mCR2D;
    public Collider2D nowGrounding;

    public float score = 0;
    public Text textline;

    public Collider2D nowInteracting;
    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;

    public float g_touchGroundsjumpForce;
    public float tGjF_init;

    public float g_accelerationX_origin;
    
    private float tGjF_count; //TouchGroundLittleJump_Count
    
    public float g_VelocityJump;
    //private bool m_jumpBumper = true;

    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    public bool[] g_movementJurisdiction = new bool[4] { true, true, true, true};
    /*0 left,1 right,2 up,3 down
     *true ==  
     */                                                                    
    public bool g_grounded = false;

    //public bool ui_keyMode = true; // true == jump , false == interact //已弃用

    

    void Start()
    {
        //g_canvas.scaleFactor = Math.Min(Screen.height / 1980f, Screen.width / 1080f);
        tGjF_count = tGjF_init;
        //mCR2D = mC.GetComponent<Rigidbody2D>();
        UIContainer.SetActive(g_allowUI);
    }
    
    //public bool SwitchKeyMode(bool tVal)
    //{
    //    ui_keyMode = tVal;
    //    return true;
    //}

    public bool AddJump()
    {
        if ( g_jumpEnabledCount < g_jumpEnabledLimit)
        {
            g_jumpEnabledCount++;
            return true;
        }
        return false;
    }

    public float AddScore(float n)
    {
        score += n;
        return score;
    }

    void Update()
    {
        tGjF_count -= Time.deltaTime;

        if (score>=8)
        {
            textline.text = "Score:" + (int)score + "You Win!!";
        }
        else
        {
            textline.text = "Score:" + (int)score;
        }


        if (!g_allowUI)
        {
            if (Input.GetKey(KeyCode.A) && (g_grounded || g_movementJurisdiction[0]) == true)
            {
                if ((mCR2D.velocity + new Vector2(-g_accelerationX_origin, 0)).x <= -g_restrictVelocityXAbs)
                    mCR2D.velocity += new Vector2(-g_restrictVelocityXAbs - mCR2D.velocity.x, 0);
                else
                    mCR2D.velocity += new Vector2(-g_accelerationX_origin, 0);
            }
            else if (Input.GetKey(KeyCode.D) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                if ((mCR2D.velocity + new Vector2(g_accelerationX_origin, 0)).x >= g_restrictVelocityXAbs)
                    mCR2D.velocity += new Vector2(g_restrictVelocityXAbs - mCR2D.velocity.x, 0);
                else
                    mCR2D.velocity += new Vector2(g_accelerationX_origin, 0);
            }
            if (Input.GetKeyDown(KeyCode.Space) && g_movementJurisdiction[2] == true)
            {
                if (g_jumpEnabledCount > 0)
                {
                    mCR2D.velocity = new Vector2(mCR2D.velocity.x, g_VelocityJump);
                    g_jumpEnabledCount--;
                }
            }
            if (Input.GetKeyDown(KeyCode.J) && nowInteracting == true )
            {
                nowInteracting.gameObject.GetComponent<Bed_1>().Interact();
            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.A)|| buttoners[0].pressed) && (g_grounded || g_movementJurisdiction[0]) == true)
            {
                if ((mCR2D.velocity + new Vector2(-g_accelerationX_origin, 0)).x <= -g_restrictVelocityXAbs)
                    mCR2D.velocity += new Vector2(-g_restrictVelocityXAbs - mCR2D.velocity.x, 0);
                else
                    mCR2D.velocity += new Vector2(-g_accelerationX_origin, 0);
            }
            else if((Input.GetKey(KeyCode.D) || buttoners[1].pressed) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                if ((mCR2D.velocity + new Vector2(g_accelerationX_origin, 0)).x >= g_restrictVelocityXAbs)
                    mCR2D.velocity += new Vector2(g_restrictVelocityXAbs - mCR2D.velocity.x, 0);
                else
                    mCR2D.velocity += new Vector2(g_accelerationX_origin, 0);
            }
            if( (Input.GetKeyDown(KeyCode.Space) || buttoners[4].pressed) &&  g_movementJurisdiction[2] == true)
            {
                //jump
                if(g_jumpEnabledCount > 0)
                {
                    mCR2D.velocity = new Vector2(mCR2D.velocity.x , g_VelocityJump);
                    g_jumpEnabledCount--;
                }
            }
            else if( (Input.GetKeyDown(KeyCode.J) || buttoners[5].pressed) && nowInteracting == true)
            {
                //interact
                nowInteracting.gameObject.GetComponent<Bed_1>().Interact(); //[Tip][20201225]因为只有Bed_1
            }
        }


        if(nowInteracting)
        {
            buttoners[5].gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttoners[5].gameObject.GetComponent<Button>().interactable = false;
        }
    }
    
}
