using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public bool g_allowUI;
    public GameObject UIContainer;

    public Buttoner[] buttoners;
    public GameObject mC; //MainCharacter
    private Rigidbody2D mCR2D;

    public float score = 0;
    public Text textline;

    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;

    public float g_accelerationX_origin;
    //private float accelerationX_adapted;

    public float g_VelocityJump;
    private bool m_jumpBumper = true;

    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    public bool[] g_movementJurisdiction = new bool[4] { true, true, true, true};
    //0 left,1 right,2 up,3 down
    public bool g_grounded = false;
    
    void Start()
    {
        mCR2D = mC.GetComponent<Rigidbody2D>();
        UIContainer.SetActive(g_allowUI);
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
            if((Input.GetKeyDown(KeyCode.Space) || buttoners[4].pressed) &&  g_movementJurisdiction[2] == true && m_jumpBumper == true)
            {
                m_jumpBumper = !m_jumpBumper;
                if(g_jumpEnabledCount > 0)
                {
                    mCR2D.velocity = new Vector2(mCR2D.velocity.x , g_VelocityJump);
                    g_jumpEnabledCount--;
                }
            }
            if(m_jumpBumper == false && buttoners[4].pressed == false)
            {
                m_jumpBumper = true;
            }
        }

    }
}
