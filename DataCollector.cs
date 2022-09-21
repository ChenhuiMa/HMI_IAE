using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class DataCollector : MonoBehaviour
{
    //获取对象
    public Toggle dippedLight;
    public Toggle full_lights;
    public Toggle fog_lights;
    public Toggle rear_fog_lights;
    public Toggle left_indicator;
    public Toggle right_indicator;
    public Toggle Auto;

    public Toggle FrontWiper0;
    public Toggle Frontwiper1;
    public Toggle Frontwiper2;
    public Toggle Frontwiper3;
    public Toggle Frontwiper4;

    public Toggle Rearwiper0;
    public Toggle Rearwiper1;
    public Toggle Rearwiper2;
    public Toggle Rearwiper3;
    public Toggle Rearwiper4;

    public Toggle ADAS;
    public Toggle ACC;
    public Toggle LCC;
    public Button LCCSpeedUp;
    public Button LCCSpeedDown;
    public int ADASSpeed;
    public Toggle LCCDistance1;
    public Toggle LCCDistance2;
    public Toggle LCCDistance3;

    public string SendData = "0000000220000002";
    // Use this for initialization
    void Start()
    {
        //Lights_bits:0-6
        dippedLight.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(0,isOn); });
        full_lights.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(1, isOn); });
        fog_lights.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(2, isOn); });
        rear_fog_lights.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(3, isOn); });
        left_indicator.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(4, isOn); });
        right_indicator.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(5, isOn); });
        Auto.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(6, isOn); });
        //wiper_bit:7-8
        //1：Intermittent
        //2：Off
        //3：Low
        //4：High
        //5：Auto
        FrontWiper0.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(7,isOn,"1"); });
        Frontwiper1.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(7,isOn,"2"); });
        Frontwiper2.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(7,isOn,"3"); });
        Frontwiper3.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(7,isOn,"4"); });
        Frontwiper4.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(7,isOn,"5"); });
        Rearwiper0.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(8,isOn,"1"); });
        Rearwiper1.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(8,isOn,"2"); });
        Rearwiper2.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(8,isOn,"3"); });
        Rearwiper3.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(8,isOn,"4"); });
        Rearwiper4.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(8,isOn,"5"); });

        //ADAS开关bit:9-11
        ADAS.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(9, isOn); });
        ACC.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(10, isOn); });
        LCC.onValueChanged.AddListener((bool isOn) => { OnToggleClick0(11, isOn); });
        //ADAS设置车速
        ADASSpeed = 30;
        LCCSpeedUp.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (ADASSpeed <= 110) ADASSpeed += 10;
        });
        LCCSpeedDown.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (ADASSpeed >= 40) ADASSpeed -= 10;
        });
        //ADAS设置距离
        //bit:15
        //0:5m
        //1:10m
        //2:15m
        LCCDistance1.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(15, isOn, "0"); });
        LCCDistance2.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(15, isOn, "1"); });
        LCCDistance3.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(15, isOn, "2"); });




    }

    private void OnToggleClick0(int bit,bool isOn)
    {
        //TODO
        if (isOn)
        {
            SendData = SendData.Remove(bit, 1);
            SendData = SendData.Insert(bit, "1");
        }
        else
        {
            SendData = SendData.Remove(bit, 1);
            SendData = SendData.Insert(bit, "0");
        }
    }
    private void OnToggleClick1(int bit,bool isOn,string value)
    {
        //TODO
        if (isOn)
        {
            SendData = SendData.Remove(bit, 1);
            SendData = SendData.Insert(bit, value);
        }
        //else
        //{
        //    SendData = SendData.Remove(bit, 1);
        //    SendData = SendData.Insert(bit, "0");
        //}
    }
    private void OnToggleClick2()
    { 
           
    }

    // Update is called once per frame
    void Update()
    {
        string SetSpeedTemp;
        if (ADASSpeed < 100) SetSpeedTemp = "0" + ADASSpeed.ToString("F0");
        else SetSpeedTemp = ADASSpeed.ToString("F0");
        SendData = SendData.Remove(12, 3);
        SendData = SendData.Insert(12, SetSpeedTemp);
    }
}
