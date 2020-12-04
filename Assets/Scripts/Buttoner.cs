using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Buttoner : MonoBehaviour
{
    Gamemanager gm;
    public bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void My_onClick()
    {
        pressed = true;
    }

    public void My_onLeave()
    {
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
