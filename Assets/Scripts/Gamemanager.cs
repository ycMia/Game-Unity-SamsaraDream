using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public bool g_allowUI;
    public GameObject UIContainer;

    public Buttoner[] buttoners;
    public Camera mCCamera; //MainCharacterCamera
    public GameObject mC; //MainCharacter
    private Rigidbody2D mCR2D;
    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;

    public float g_accelerationX;

    public float g_VelocityJump;
    private bool m_jumpBumper = true;

    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    public bool[] g_movementJurisdiction = new bool[4] { true, true, true, true};
    //0 left,1 right,2 up,3 down
    public bool g_grounded = false;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        //Camera
        mCCamera.transform.position = Vector3.Lerp(mCCamera.transform.position, mC.transform.position, 0.1f);
        mCCamera.transform.position = new Vector3(mCCamera.transform.position.x, mCCamera.transform.position.y, -10f);
        //Movement Control

        if(!g_allowUI)
        {
            if (Input.GetKey(KeyCode.A) && (g_grounded || g_movementJurisdiction[0]) == true)
            {
                mCR2D.velocity += new Vector2(-g_accelerationX, 0);
            }
            else if (Input.GetKey(KeyCode.D) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                mCR2D.velocity += new Vector2(g_accelerationX, 0);
            }
            if (Input.GetKeyDown(KeyCode.Space) && g_movementJurisdiction[2] == true)
            {
                if (g_jumpEnabledCount > 0)
                {
                    mCR2D.velocity = new Vector2(mCR2D.velocity.x, g_VelocityJump);
                    g_jumpEnabledCount--;
                }
            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.A)|| buttoners[0].pressed) && (g_grounded || g_movementJurisdiction[0]) == true)
            {
                mCR2D.velocity += new Vector2(-g_accelerationX, 0);
            }
            else if((Input.GetKey(KeyCode.D) || buttoners[1].pressed) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                mCR2D.velocity += new Vector2(g_accelerationX, 0 );
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

        //Final Fixing
        RestrictMaxSpeed(mCR2D, g_restrictVelocityXAbs, g_restrictVelocityYAbs);
    }
    

    void RestrictMaxSpeed(Rigidbody2D rigidbody2D,float restrictXAbs,float restrictYAbs)
    {
        if(Mathf.Abs(rigidbody2D.velocity.x) >= restrictXAbs)
        {
            if(rigidbody2D.velocity.x > 0)
                rigidbody2D.velocity = new Vector2(restrictXAbs, rigidbody2D.velocity.y);
            else
                rigidbody2D.velocity = new Vector2(-restrictXAbs, rigidbody2D.velocity.y);
        }
        if (Mathf.Abs(rigidbody2D.velocity.y) >= restrictYAbs) //attention here because of its 
        {
            if (rigidbody2D.velocity.y > 0)
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, restrictYAbs);
            else
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -restrictYAbs);
        }
    }
}
