using HMI.Vehicles.Behaviours;
using HMI.Vehicles.Behaviours.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using ImmerUDP;


namespace ReadySignal
{
    public class ReadySignal : MonoBehaviour
    {
        public GameObject ready;
        public GameObject AD;
        public GameObject ACC;
        public GameObject LCC;
        public GameObject SetSpeed;
        public GameObject SetSpeed1;
        public GameObject target;
        public string _s = "0";
        public string AD_Flag;
        public string ACC_Flag;
        public string LCC_Flag;



        void Awake()
        {
            Hide(AD);
            Hide(ACC);
            Hide(LCC);
            Hide(ready);
            Hide(SetSpeed);
            Hide(SetSpeed1);
            target = GameObject.Find("UdpServer");
        }

        // Update is called once per frame
        void Update()
        {
            _s = target.gameObject.GetComponent<UdpServer>().EngineReady;
            AD_Flag = target.gameObject.GetComponent<UdpServer>().AD;
            ACC_Flag = target.gameObject.GetComponent<UdpServer>().ACC;
            LCC_Flag = target.gameObject.GetComponent<UdpServer>().LCC;
            if (_s == "1") show(ready);
            else Hide(ready);
            if (AD_Flag == "1") show(AD);
            else Hide(AD);
            if (ACC_Flag == "1") show(ACC);
            else Hide(ACC);
            if (LCC_Flag == "1") show(LCC);
            else Hide(LCC);
            if (ACC_Flag == "1") show(SetSpeed);
            else Hide(SetSpeed);
            if (LCC_Flag == "1") show(SetSpeed1);
            else Hide(SetSpeed1);
        }

        void show(GameObject ready)
        {
            ready.GetComponent<Renderer>().enabled = true;
        }
        void Hide(GameObject ready)
        {
            ready.GetComponent<Renderer>().enabled = false;
        }




    }
}
