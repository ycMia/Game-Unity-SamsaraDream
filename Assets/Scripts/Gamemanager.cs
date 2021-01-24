using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Gamemanager : MonoBehaviour
{
    public Bed_1 bed_1_Manager;

    private Vector3 respawnPosition;
    public Vector3 RespawnPosition { set => respawnPosition = value; }

    public Coin coinManager;
    public Canvas g_canvas;
    public CameraManager CameraManager;

    public bool g_allowUI;
    public GameObject UIContainer;
    
    public Buttoner[] buttoners;
    public GameObject mainCharacter; //MainCharacter
    public Rigidbody2D mainCharacter_Rigidbody2D;
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

    private bool priv_jumpLocker = false;
    //lock == true ; unlock == false

    void Start()
    {
        coinManager.gameObject.SetActive(true);
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

    public void CharacterDie()
    {
        //CameraManager.locked = true;
        //print("gameOver \\(0ω0)/");
        //mainCharacter.SetActive(false);
        //coinManager.Reset();
        //bed_1_Manager.Reset();
        mainCharacter_Rigidbody2D.velocity = new Vector2(0f, 0f);
        mainCharacter.transform.position = respawnPosition;
    }

    public void GameStart()
    {
        print("gameStart _(:3 )∠_");
        CameraManager.locked = false;
        mainCharacter.gameObject.transform.position = new Vector2(0, 0);
        score = 0f;
        g_jumpEnabledCount = g_jumpEnabledLimit;
        mainCharacter.SetActive(true);
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
        if(buttoners[4].pressed == false)
        {
            priv_jumpLocker = false;
        }

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
                if ((mainCharacter_Rigidbody2D.velocity + new Vector2(-g_accelerationX_origin, 0)).x <= -g_restrictVelocityXAbs)
                    mainCharacter_Rigidbody2D.velocity += new Vector2(-g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
                else
                    mainCharacter_Rigidbody2D.velocity += new Vector2(-g_accelerationX_origin, 0);
            }
            else if (Input.GetKey(KeyCode.D) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                if ((mainCharacter_Rigidbody2D.velocity + new Vector2(g_accelerationX_origin, 0)).x >= g_restrictVelocityXAbs)
                    mainCharacter_Rigidbody2D.velocity += new Vector2(g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
                else
                    mainCharacter_Rigidbody2D.velocity += new Vector2(g_accelerationX_origin, 0);
            }
            if (Input.GetKeyDown(KeyCode.Space) && g_movementJurisdiction[2] == true)
            {
                if (g_jumpEnabledCount > 0)
                {
                    mainCharacter_Rigidbody2D.velocity = new Vector2(mainCharacter_Rigidbody2D.velocity.x, g_VelocityJump);
                    g_jumpEnabledCount--;
                }
                //因为按键的GetKeyDown不会瞬发, 所以没有lock 的必要
                
            }
            if (Input.GetKeyDown(KeyCode.J) && nowInteracting == true )
            {
                nowInteracting.transform.parent.gameObject.GetComponent<Bed_1>().Interact(int.Parse(nowInteracting.gameObject.name));
            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.A)|| buttoners[0].pressed) && (g_grounded || g_movementJurisdiction[0]) == true)
            {
                if ((mainCharacter_Rigidbody2D.velocity + new Vector2(-g_accelerationX_origin, 0)).x <= -g_restrictVelocityXAbs)
                    mainCharacter_Rigidbody2D.velocity += new Vector2(-g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
                else
                    mainCharacter_Rigidbody2D.velocity += new Vector2(-g_accelerationX_origin, 0);
            }
            else if((Input.GetKey(KeyCode.D) || buttoners[1].pressed) && (g_grounded || g_movementJurisdiction[1]) == true)
            {
                if ((mainCharacter_Rigidbody2D.velocity + new Vector2(g_accelerationX_origin, 0)).x >= g_restrictVelocityXAbs)
                    mainCharacter_Rigidbody2D.velocity += new Vector2(g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
                else
                    mainCharacter_Rigidbody2D.velocity += new Vector2(g_accelerationX_origin, 0);
            }
            if( (Input.GetKeyDown(KeyCode.Space) || (buttoners[4].pressed && priv_jumpLocker == false) ) &&  g_movementJurisdiction[2] == true)
            {
                //jump
                if(g_jumpEnabledCount > 0)
                {
                    mainCharacter_Rigidbody2D.velocity = new Vector2(mainCharacter_Rigidbody2D.velocity.x , g_VelocityJump);
                    g_jumpEnabledCount--;
                    priv_jumpLocker = true; // lock me
                }
            }
            else if( (Input.GetKeyDown(KeyCode.J) || buttoners[5].pressed) && nowInteracting == true)
            {
                //interact
                nowInteracting.transform.parent.gameObject.GetComponent<Bed_1>().Interact(int.Parse(nowInteracting.gameObject.name));
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
