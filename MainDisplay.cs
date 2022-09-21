using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
using System.Xml.Serialization;


public class MainDisplay : MonoBehaviour
{
    [HideInInspector]
    //�������ô��ں���
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    //���뵱ǰ�����
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();
    //��ʾ����
    const uint SWP_SHOWWINDOW = 0x0040;
    //��չ��������������X��ʼλ��
    int x = -1920;
    //��չ��������������Y��ʼλ��
    int y = 0;
    //��չ�����
    int width = 2350;
    //��չ���߶�
    int height = 1000;

    void Start()
    {

        Screen.fullScreen = false;  //���óɷ�ȫ��
        SetWindowPos(GetActiveWindow(), -1, x, y, width, height, SWP_SHOWWINDOW);







        Screen.SetResolution(2350, 850, true);
    }
    void Update()
    {
        //  ��ESC�˳�ȫ��
        if (Input.GetKey(KeyCode.Escape))
        {
            Screen.fullScreen = false; //�˳�ȫ��         
        }

        //����Ϊ��ȫ��
        if (Input.GetKey(KeyCode.F1))
        {
            Screen.SetResolution(2350, 850, false);
        }

        //����ȫ��
        if (Input.GetKey(KeyCode.F2))
        {
            Screen.SetResolution(2350, 850, true);
        }

        if (Input.GetKey(KeyCode.F3))
        {
            Resolution[] resolutions = Screen.resolutions; //��ȡ���õ�ǰ��Ļ�ֱ���
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height,
                true); //���õ�ǰ�ֱ���
            Screen.fullScreen = true; //���ó�ȫ��,
        }
    }


}
