using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO
 * Jumping in Global Gravity 
 * Rotataion Should not be effect by Surface normals 
 * able to walk to all surface regarless of angle 
*/


[RequireComponent(typeof(Rigidbody))] 
public class Movement : MonoBehaviour
{


    [SerializeField] float moveSpeed = 6; // move speed
    [SerializeField] float turnSpeed = 90; // turning speed (degrees/second)
    [SerializeField] float lerpSpeed = 10; // smoothing speed
    [SerializeField] float gravity = 10; // gravity acceleration
    [SerializeField] bool isGrounded;
    [SerializeField] float deltaGround = 0.2f; // character is grounded up to this distance
    [SerializeField] float jumpSpeed = 10; // vertical jump initial speed
    [SerializeField] float jumpRange = 10; // range to detect target wall
    [SerializeField] Vector3 surfaceNormal; // current surface normal
    [SerializeField] Vector3 myNormal; // character normal
    [SerializeField] float distGround; // distance from character position to ground
    [SerializeField] bool jumping = false; // flag "I'm jumping to wall"
    [SerializeField] float vertSpeed = 0; // vertical jump current speed
    private Rigidbody rigidbody;
    private Transform myTransform;
    public BoxCollider boxCollider; // drag BoxCollider ref in editor

    private void Start()
    {
        myNormal = transform.up; // normal starts as character up direction
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true; // disable physics rotation
                                         // distance from transform.position to ground
        distGround = boxCollider.size.y - boxCollider.center.y;

    }

    private void FixedUpdate()
    {
        // apply constant weight force according to character normal:
        rigidbody.AddForce(-gravity * rigidbody.mass * myNormal);
    }

    private void Update()
    {
        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall

        Ray ray;
        RaycastHit hit;

        if (Input.GetButtonDown("Jump"))
        { // jump pressed:
            ray = new Ray(myTransform.position, myTransform.forward);
            if (Physics.Raycast(ray, out hit, jumpRange))
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
            }
            else if (isGrounded)
            { // no: if grounded, jump up
                rigidbody.velocity += jumpSpeed * myNormal;
            }
        }

        // movement code - turn left/right with Horizontal axis:
        myTransform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        // update surface normal and isGrounded:
        ray = new Ray(myTransform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character forth/back with Vertical axis:
        myTransform.Translate(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
    }

    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumping = true; // signal it's jumping to wall
        
        //rigidbody.isKinematic = true; // disable physics while jumping
        rigidbody.useGravity = true;
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        Vector3 dstPos = point+ transform.forward * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
            
        }
        myNormal = normal; // update myNormal
        rigidbody.isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished

    }
}
