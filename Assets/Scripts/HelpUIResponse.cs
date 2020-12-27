using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;

public class HelpUIResponse : MonoBehaviour
{
    //https://www.jianshu.com/p/95cb4621206e Unity高级开发-项目与屏幕适配
    //https://docs.unity3d.com/ScriptReference/Screen.SetResolution.html 官方文档
    private Resolution[] resolutions;

    private void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        //获取设置当前屏幕分辩率

        print("\r\nwidth=" + resolutions[resolutions.Length - 1].width.ToString()+
           "\r\nlength="+ resolutions[resolutions.Length - 1].height.ToString()  );
        
        
        Screen.SetResolution(resolutions[resolutions.Length - 1].width,
                            resolutions[resolutions.Length - 1].height,
                            true);//设置当前分辨率 -- 设置成全屏,  读取在 Screen.resolutions[] 中的 最后一个 resolution 的数据(也就是初始数据)

        //Screen.fullScreen = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Resolution[] resolutions = Screen.resolutions;
            // print("\r\nwidth=" + resolutions[resolutions.Length - 1].width.ToString() +
            //"\r\nlength=" + resolutions[resolutions.Length - 1].height.ToString());
            print("\r\nwidth=" + resolutions[0].width.ToString() +
           "\r\nlength=" + resolutions[0].height.ToString());
            //打印最新一个屏幕数据
        }
        //Screen.SetResolution(1920, 1080, true);

    }
}