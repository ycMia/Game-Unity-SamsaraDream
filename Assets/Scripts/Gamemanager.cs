using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Gamemanager : MonoBehaviour
{
    public bool g_allowUI;
    public GameObject UIContainer;

    public Buttoner[] buttoners;
    public GameObject mC; //MainCharacter
    private Rigidbody2D mCR2D;

    public float score = 0;
    public Text textline;

    public Collider2D nowInteracting;
    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;

    public float g_touchGroundsjumpForce;
    public float tGjF_init;

    public float g_accelerationX_origin;
    
    private float tGjF_count;
    public void TouchGroundLittleJump()//落地卡顿修正
    {
        if(tGjF_count <= 0)//防止小跳
        {
            mCR2D.AddForce(g_touchGroundsjumpForce * Vector2.up,ForceMode2D.Impulse);
            tGjF_count = tGjF_init;
        }
        //throw new NotImplementedException();
    }

    //private float accelerationX_adapted;

    public float g_VelocityJump;
    private bool m_jumpBumper = true;

    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    public bool[] g_movementJurisdiction = new bool[4] { true, true, true, true};
    //0 left,1 right,2 up,3 down
    public bool g_grounded = false;

    public bool ui_keyMode = true; // true == jump , false == interact

    void Start()
    {
        tGjF_count = tGjF_init;
        mCR2D = mC.GetComponent<Rigidbody2D>();
        UIContainer.SetActive(g_allowUI);
    }
    
    public bool SwitchKeyMode(bool tVal)
    {
        ui_keyMode = tVal;
        return true;
    }

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
            if (m_jumpBumper == false && buttoners[4].pressed == false)
            {
                m_jumpBumper = true;
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
            if(ui_keyMode == true &&(Input.GetKeyDown(KeyCode.Space) || buttoners[4].pressed) &&  g_movementJurisdiction[2] == true && m_jumpBumper == true)
            {
                //jump

                m_jumpBumper = !m_jumpBumper;
                if(g_jumpEnabledCount > 0)
                {
                    mCR2D.velocity = new Vector2(mCR2D.velocity.x , g_VelocityJump);
                    g_jumpEnabledCount--;
                }
            }
            else if(ui_keyMode == false &&(Input.GetKeyDown(KeyCode.Space) || buttoners[4].pressed) && g_movementJurisdiction[2] == true && m_jumpBumper == true)
            {
                //interact

                nowInteracting.gameObject.GetComponent<Interacter_Bed_1>().Interact(); //[Tip][20201225]因为只有Interacter_Bed_1
            }
            if(m_jumpBumper == false && buttoners[4].pressed == false)
            {
                m_jumpBumper = true;
            }
        }
        
        tGjF_count -= Time.deltaTime;
        if(nowInteracting)
        {
            buttoners[4].gameObject.transform.GetChild(0).GetComponent<Text>().text = "Interact";
            buttoners[4].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        }
        else
        {
            buttoners[4].gameObject.transform.GetChild(0).GetComponent<Text>().text = "Jump";
            buttoners[4].gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        }
    }
}
