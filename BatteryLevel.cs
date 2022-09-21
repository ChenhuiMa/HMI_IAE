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

public class BatteryLevel : MonoBehaviour
{
    public GameObject target;
    private SpriteRenderer SpriteRenderer;
    public float BatteryStateOfCharge;

    void Awake()
    {
        target = GameObject.Find("UdpServer");
        SpriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        BatteryStateOfCharge = target.gameObject.GetComponent<UdpServer>().BatteryStateOfCharge;
        SpriteRenderer.material.SetFloat("Fill", BatteryStateOfCharge);

    }
}
