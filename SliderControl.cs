using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SpeedObject;
    public TMP_Text DigitalSpeed;
    public Slider slider;
    private float value;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int CurrentSetSpeed0 = SpeedObject.gameObject.GetComponent<DataCollector>().ADASSpeed;
        float CurrentSetSpeed = (float)(CurrentSetSpeed0);
        DigitalSpeed.text = CurrentSetSpeed.ToString("F0");
        value = (CurrentSetSpeed - 30) / 90;
        slider.value = value;

    }
}
