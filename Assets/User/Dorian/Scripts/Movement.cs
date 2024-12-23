using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))] 
public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6; // move speed
    [SerializeField] float lerpSpeed = 10; // smoothing speed
    [SerializeField] float RotationSpeed = 10; 
    [SerializeField] float gravity = 10; // gravity acceleration
    [SerializeField] bool isGrounded;
    [SerializeField] float deltaGround = 0.2f; // character is grounded up to this distance
    [SerializeField] float jumpSpeed = 10; // vertical jump initial speed
    [SerializeField] float jumpRange = 10; // range to detect target wall
    [SerializeField] Vector3 surfaceNormal; // current surface normal
    [SerializeField] Vector3 myNormal; // character normal
    [SerializeField] float distGround; // distance from character position to ground
    [SerializeField] bool Lerping = false; // flag "I'm jumping to wall"
    [SerializeField] float vertSpeed = 0; // vertical jump current speed
    [SerializeField] LayerMask layerMask;
    private Rigidbody rigidbody;
    private Transform myTransform;
    public BoxCollider boxCollider; // drag BoxCollider ref in editor
    private Controls _controls;

    private Ray WallRay;
    private Ray GroundRay;
    private Ray CliffRay;

    private void Awake()
    {
        _controls = new Controls();
    }
    
    private void Start()
    {
        myNormal = transform.up; // normal starts as character up direction
        myTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true; // disable physics rotation
        distGround = boxCollider.size.y - boxCollider.center.y;

        _controls.actionmap.Jump.performed += ctx => Jump();
    }

    private void FixedUpdate()
    {
        Vector3 Direction;
        if (isGrounded) { Direction = myNormal; }
        else { Direction = Vector3.up; }

        // apply constant weight force according to character normal:
        rigidbody.AddForce(-gravity * rigidbody.mass * Direction);
    }
    void Jump()
    {
        if (isGrounded)
        { // no: if grounded, jump up
            rigidbody.velocity += jumpSpeed * transform.up;
        }
    }
    private void Update()
    {
        // jump code - jump to wall or simple jump
        if (Lerping) return; // abort Update while jumping to a wall
        RaycastHit hit;


        // update surface normal and isGrounded:

        GroundRay = new Ray(myTransform.position, -myNormal); // cast ray downwards
        CliffRay = new Ray(myTransform.TransformPoint(new Vector3(0, -1, 0)), -myTransform.forward);
        if (Physics.Raycast(GroundRay, out hit,1,layerMask))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
           
            if (Physics.Raycast(CliffRay, out hit, 1, layerMask))
            {
                surfaceNormal = hit.normal;
            }
            else
            {
                isGrounded = false;
                // assume usual ground normal to avoid "falling forever"
                surfaceNormal = Vector3.up;
            }
        }
        WallRay = new Ray(myTransform.position, myTransform.forward);
        if (Physics.Raycast(WallRay, out hit, 1, layerMask))
        { // wall ahead?
            surfaceNormal = hit.normal;
        }


        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        // Modify the update block where rotation is set
        Vector2 moveinput = _controls.actionmap.Movemnet.ReadValue<Vector2>().normalized;
        
        Vector3 loockdirection;
        if (moveinput != Vector2.zero)
        {
            loockdirection = Camera.main.transform.forward * moveinput.y;
            loockdirection += Camera.main.transform.right * moveinput.x;
            loockdirection = loockdirection.normalized;
        }
        else
        {
            loockdirection = Camera.main.transform.forward;
        }


        Vector3 cameraForwardProjected = Vector3.ProjectOnPlane(loockdirection, myNormal).normalized;
        Quaternion targetRot = Quaternion.LookRotation(cameraForwardProjected, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, RotationSpeed * Time.deltaTime);

        // move the character forth/back with Vertical axis:


        
        myTransform.Translate(0, 0, moveinput.magnitude * moveSpeed * Time.deltaTime);
        
       
    }

    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        Lerping = true; // signal it's jumping to wall
        
        rigidbody.isKinematic = true; // disable physics while jumping
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        Vector3 dstPos = point;
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(Vector3.up, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 0.5f;)
        {
            t += Time.deltaTime;
            
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
            
        }
        myNormal = normal; // update myNormal
        rigidbody.isKinematic = false; // enable physics
        Lerping = false; // jumping to wall finished

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawRay(WallRay);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(GroundRay);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(CliffRay);
    }

    private void OnEnable(){ _controls.Enable();}
    private void OnDisable(){ _controls.Disable();}
}
