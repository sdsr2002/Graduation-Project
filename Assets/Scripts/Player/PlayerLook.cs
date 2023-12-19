using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera Cam;
    private float _xRotation = 0f;
    public float XSensitivity = 30f;
    public float YSensitivity = 30f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //Calculate camera roation for looking up and down
        _xRotation -= (mouseY * Time.deltaTime) * YSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        Cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * XSensitivity);
    }
}
