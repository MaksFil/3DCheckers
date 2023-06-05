using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingCameras : MonoBehaviour
{
    [SerializeField] private GameObject[] _cameras = new GameObject[2];

    [SerializeField] private int _startCameraIndex;

    private void Start() 
    {
        SetCamera(_startCameraIndex);
    }

    public void SetCamera(int index) 
    {
        for(int i = 0; i < _cameras.Length; i++) 
        {
            if(i == index) _cameras[i].SetActive(true);
            else _cameras[i].SetActive(false);
        }
    }
}
