using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;

public class ActivaVR : MonoBehaviour {
    public GameObject btn, joystick,mirar;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;
    public GameObject canvas;
    public SmoothMouseLook sml;

    private bool vrEnabled = false;

    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = vrEnabled;
        joystick.SetActive(false);
        mirar.SetActive(false);
        if (vrEnabled)
        {
            btn.GetComponent<Image>().color = activeColor;
           
        }
        else
        {
            btn.GetComponent<Image>().color = inactiveColor;
            
        }
    }

    public void DesactivaVR()
    {
        vrEnabled = !vrEnabled;

        if (vrEnabled)
        {
            btn.GetComponent<Image>().color = activeColor;
            joystick.SetActive(false);
            mirar.SetActive(false);
            sml.enabled = false;
        }
        else
        {
            btn.GetComponent<Image>().color = inactiveColor;
            joystick.SetActive(true);
            mirar.SetActive(true);
            sml.enabled = true;
        }

        UnityEngine.XR.XRSettings.enabled = vrEnabled;

    }
    
    

}
