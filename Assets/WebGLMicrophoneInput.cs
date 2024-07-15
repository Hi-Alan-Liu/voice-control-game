using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLMicrophoneInput : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void StartMicrophone();

    [DllImport("__Internal")]
    private static extern float GetMicrophoneVolume();

    private float currentVolume;

    void Start()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        StartMicrophone();
        #endif
    }

    void Update()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        currentVolume = GetMicrophoneVolume();
        #endif
    }

    public float GetRMSVolume()
    {
        return currentVolume;
    }
}
