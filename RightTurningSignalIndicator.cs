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


namespace RightTurningSignalIndicator
{
    public class RightTurningSignalIndicator : MonoBehaviour
    {
        public GameObject obj;
        public GameObject target;
        public string _s = "0";

        void Awake()
        {
            Hide();
            target = GameObject.Find("UdpServer");
        }

        // Update is called once per frame
        void Update()
        {
            _s = target.gameObject.GetComponent<UdpServer>().right_indicator;
            if (_s == "1")
            {
                show();
            }
            else
            {
                Hide();
            }
        }

        void show()
        {
            obj.GetComponent<Renderer>().enabled = true;
        }
        void Hide()
        {
            obj.GetComponent<Renderer>().enabled = false;
        }




    }
}
