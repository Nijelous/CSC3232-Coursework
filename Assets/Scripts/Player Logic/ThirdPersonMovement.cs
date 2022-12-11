using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Method handling the player movement
 * controller: the character controller attached to the player
 * cam: the main camera of the scene
 * speed: how fast the player moves
 * mouseWorldPosition: the point in the world the mouse is currently pointing, calculated when mouse1 is held down
 * distToGround: half of the height of the player
 * gravity: the amount of gravity currently acting upon the player, which is removed from their height every update
 * jumpPower: how high the player can jump
 * playerVelocity: how fast the player is currently moving
 * sm: the SceneManager class in the scene
 */
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    GameObject cam;

    [SerializeField]
    float speed = 6f;

    public Vector3 mouseWorldPosition = Vector3.zero;

    private float distToGround;

    [SerializeField]
    private float gravity = -14f;

    [SerializeField]
    private float jumpPower = 2f;

    private Vector3 playerVelocity;

    private SceneHandler sm;

    private void Start()
    {
        sm = GameObject.Find("Scene Manager").GetComponent<SceneHandler>();
        if (cam == null)
        {
            cam = GameObject.Find("Main Camera");
        }
        distToGround = controller.bounds.extents.y;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks for whether the object should be moving right now depending on what is in control
        if (gameObject.layer == 7 || !sm.ShadowActive())
        {
            // Gets the directional buttons currently pressed by the user and assigns the way the player should be facing and moving accordingly
            Vector3 moveDir = Vector3.zero;
            Vector3 faceDir = Vector3.zero;
            bool moving = false;
            if (Input.GetKey(KeyCode.W))
            {
                moveDir += transform.forward;
                faceDir = cam.transform.forward;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir -= transform.forward;
                faceDir = cam.transform.forward;
                moving = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir += transform.right;
                faceDir = cam.transform.forward;
                moving = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDir -= transform.right;
                faceDir = cam.transform.forward;
                moving = true;
            }
            // If the player is aiming, sends out a raycast from the middle of the screen to hit either an enemy or default, determining where the player is looking
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
                Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Enemy")))
                {
                    mouseWorldPosition = raycastHit.point;
                }

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                // Lerp for smooth rotation of the player
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else if(moving) // If the player has moved, and isn't aiming, then uses this logic to get the direction its facing, removing the y component
            {
                faceDir.y = 0f;
                transform.forward = Vector3.Lerp(transform.forward, faceDir, Time.deltaTime * 20f);
            }
            controller.Move(speed * Time.deltaTime * moveDir);
            // Checks to see if the player can jump, and if they are jumping then adds the y velocity to the final move
            if (IsGrounded() && playerVelocity.y < 0) playerVelocity.y = 0f;
            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                playerVelocity.y += Mathf.Sqrt(jumpPower * -3.0f * gravity);
            }
        }
        // Takes into account gravity as the player is dragged back down to the terrain
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(distToGround + 0.1));
    }

    public void SetGravity(float g, float j)
    {
        gravity = g;
        jumpPower = j;
    }
}
