using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    public GameObject weapon;
    private float xRotation = 0f;
    public float xRecoil = 0f;

    public float xSens = 30f;
    public float ySens = 30f;
    // Start is called before the first frame update
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        weapon.transform.localRotation = Quaternion.Euler(xRotation - xRecoil, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSens);
    }
}
