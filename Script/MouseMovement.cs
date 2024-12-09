using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500.0f;

    float xRotation = 0.0f;
    float yRotation = 0.0f;

    public float topClamp = -90.0f;
    public float bottomClamp = 55.0f;

    // Start is called before the first frame update
    void Start()
    {
        //lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;    
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotate player around x axis (up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp); // clamp rotation to prevent player from looking behind them

        // rotate player around y axis (left and right)
        yRotation += mouseX;
        
        // apply rotation to player
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);

    }
}
