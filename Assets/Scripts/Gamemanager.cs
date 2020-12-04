using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public Camera mCCamera; //MainCharacterCamera
    public GameObject mC; //MainCharacter
    private Rigidbody2D mCR2D;
    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;


    public float g_accelerationX;
    public float g_VelocityJump;
    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    // Start is called before the first frame update
    void Start()
    {
        mCR2D = mC.GetComponent<Rigidbody2D>();
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

    public void SetCharacterSlideAtSide(Rigidbody2D rigidbody2D, int mode) 
        // 1 : notAllowRightwayMovement
        // 2 : notAllowLeftwayMovement
    {

    }


    // Update is called once per frame
    void Update()
    {
        //Camera
        mCCamera.transform.position = Vector3.Lerp(mCCamera.transform.position, mC.transform.position, 0.1f);
        mCCamera.transform.position = new Vector3(mCCamera.transform.position.x, mCCamera.transform.position.y,  - 10f);
        //Movement
        if (Input.GetKey(KeyCode.A))
        {
            mCR2D.velocity += new Vector2(-g_accelerationX, 0);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            mCR2D.velocity += new Vector2(g_accelerationX, 0 );
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(g_jumpEnabledCount > 0)
            {
                mCR2D.velocity = new Vector2(mCR2D.velocity.x , g_VelocityJump);
                g_jumpEnabledCount--;
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
