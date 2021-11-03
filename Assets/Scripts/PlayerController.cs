using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpForce;
    public float pushForce;
    public float gravityScale;
    public CharacterController cc;

    private Vector3 moveDir;

    public bool inCutscene;
    public Material transparent;
    public Material opaque;
    private Renderer rend;
    public Camera mainCamera;
    public Camera bullshit;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        rend = GetComponent<Renderer>();
        inCutscene = false;
        rend.material = transparent;
        bullshit.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!inCutscene) //only move if not in a cutscene
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDir.y, Input.GetAxis("Vertical") * moveSpeed);

            if (cc.isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = jumpForce;
                } else
                {
                    moveDir.y = 0f;
                }
            }

            //apply gravity
            moveDir.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            //apply mouse-based horizontal rotation
            moveDir = Quaternion.Euler(0f, this.transform.eulerAngles.y, 0f) * moveDir;
        }

        //use Time.deltaTime to make sure moveSpeed is not based on fps
        cc.Move(moveDir * Time.deltaTime);

        //if(Input.GetAxis("Horizontal") != 0)p.transform.Rotate(0f, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnTriggerEnter(Collider collider)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collider.gameObject.tag == "CutsceneTrigger")
        {
            inCutscene = true;
            rend.material = opaque;
            bullshit.enabled = true;
            mainCamera.enabled = false;
            moveDir = new Vector3(0f, 0f, 0f); //freeze player (otherwise funny floats off into the void)
        }
    }

    //unity documentation copy-paste
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // if we only push objects to the sides never up and down: 
        // hit.moveDirection.x, 0, hit.moveDirection.z
        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushForce;
    }
}
