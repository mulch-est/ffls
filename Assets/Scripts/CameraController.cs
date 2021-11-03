using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotateSpeed;
    private Vector3 currentRotation;
    //private float minRotation = -45f;
    //private float maxRotation = 45f;

    // Start is called before the first frame update
    void Start()
    {
        /* Info:
         * The cursor state can be changed by the operating system or Unity. 
         * You should therefore check the state of the cursor for example when the application regains focus or the state of a game changes to reveal a UI. 
         * In the Editor the cursor is automatically reset when escape is pressed, or on switching applications. 
         * In the Standalone Player you have full control over the mouse cursor, but switching applications still resets the cursor.
        */

        //keep mouse cursor at screen center
        //! only works after player clicks on the program to focus it
        Cursor.lockState = CursorLockMode.Locked;
        //making cursor invisible is not necessary for locked mode
        //Cursor.visible = false;
        transform.position = target.position - offset;
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = target.transform.localRotation.eulerAngles;

        //get the x position of the mouse and rotate the target
        currentRotation.y += Input.GetAxis("Mouse X") * rotateSpeed;

        //same as above, but clamp rotation so you can't look too far up / down
        currentRotation.x -= Input.GetAxis("Mouse Y") * rotateSpeed;

        //[30, 0]U[360,330]
        if(currentRotation.x < 180)
        {
            currentRotation.x = Mathf.Clamp(currentRotation.x, -1, 30);
        }
        else
        {
            currentRotation.x = Mathf.Clamp(currentRotation.x, 330, 360);
        }
        

        target.transform.localRotation = Quaternion.Euler (currentRotation);

        //target.rotation = currentRotation;

        //move the camera based on the rotation of the target and offset
        transform.rotation = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0f);
        
        
        //moves the camera to the target
        transform.position = target.position - offset;

    }
}
