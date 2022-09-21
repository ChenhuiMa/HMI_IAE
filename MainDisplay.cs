using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
using System.Xml.Serialization;


public class MainDisplay : MonoBehaviour
{
    [HideInInspector]
    //导入设置窗口函数
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    //导入当前活动窗口
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();
    //显示窗口
    const uint SWP_SHOWWINDOW = 0x0040;
    //扩展屏在整个大屏的X起始位置
    int x = -1920;
    //扩展屏在整个大屏的Y起始位置
    int y = 0;
    //扩展屏宽度
    int width = 2350;
    //扩展屏高度
    int height = 1000;

    void Start()
    {

        Screen.fullScreen = false;  //设置成非全屏
        SetWindowPos(GetActiveWindow(), -1, x, y, width, height, SWP_SHOWWINDOW);







        Screen.SetResolution(2350, 850, true);
    }
    void Update()
    {
        //  按ESC退出全屏
        if (Input.GetKey(KeyCode.Escape))
        {
            Screen.fullScreen = false; //退出全屏         
        }

        //设置为不全屏
        if (Input.GetKey(KeyCode.F1))
        {
            Screen.SetResolution(2350, 850, false);
        }

        //设置全屏
        if (Input.GetKey(KeyCode.F2))
        {
            Screen.SetResolution(2350, 850, true);
        }

        if (Input.GetKey(KeyCode.F3))
        {
            Resolution[] resolutions = Screen.resolutions; //获取设置当前屏幕分辩率
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height,
                true); //设置当前分辨率
            Screen.fullScreen = true; //设置成全屏,
        }
    }


}
