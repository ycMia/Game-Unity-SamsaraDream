using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mCCamera; //Main Camera
    public GameObject mC; //Main Character
    public float ControlParameter_Speed;

    public bool locked = false;

    private Vector3 targetPosition; //adapted Position ( restricted to [z == -10f] )
    private float distance;
    
    void Start()
    {
        mCCamera = gameObject.GetComponent<Camera>();
        
    }
    
    void Update()
    {
        targetPosition = mC.transform.position + new Vector3(0f, 0f, -10f);
        distance = Vector3.Distance(targetPosition, mCCamera.gameObject.transform.position);

        if (!locked)
        {
            float t_Speed = ControlParameter_Speed * distance;
            mCCamera.transform.position += Time.deltaTime * t_Speed * ((targetPosition - mCCamera.transform.position) / distance);
        }
    }
}
