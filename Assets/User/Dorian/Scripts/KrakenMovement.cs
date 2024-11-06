using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class KrakenMovement : MonoBehaviour
{
   
    Controls _Controls;
    [SerializeField] float GravitySpeed;
    [SerializeField] bool UseGravity;
    [SerializeField] float MoveSpeed = 1;
    
    [SerializeField] RayPoint GroundCheck;
    [SerializeField] RayPoint WallCheck;
    [SerializeField] RayPoint CliffCheck;

    [SerializeField] LayerMask layerMask;
    [SerializeField] Hit hits;
    private void Awake()
    {
        _Controls = new Controls();
    }

    private void FixedUpdate()
    {
        Vector2 MoveInput = _Controls.actionmap.Movemnet.ReadValue<Vector2>();
        RaycastHit GroundHit;
        if (Physics.Raycast(GroundCheck.transform.position, GroundCheck.Raydirection, out GroundHit, GroundCheck.RayLenght, layerMask)) hits.Ground = true;
        else hits.Ground = false;

        RaycastHit WallHit;
        if (Physics.Raycast(WallCheck.transform.position, WallCheck.Raydirection, out WallHit, WallCheck.RayLenght, layerMask)) hits.Wall = true;
        else hits.Wall = false;
        
        RaycastHit CliffHit;
        if (Physics.Raycast(CliffCheck.transform.position, CliffCheck.Raydirection, out CliffHit, CliffCheck.RayLenght, layerMask)) hits.Cliff = true;
        else hits.Cliff = false;

        
        switch (hits)
        {
            case {Ground: true }:
                float NolDist = Mathf.Lerp(0, GroundCheck.RayLenght, GroundHit.distance);
                transform.up = Vector3.Lerp(transform.up,GroundHit.normal,1-NolDist);
                break;
            
            case { Wall: true }:
                Debug.Log("Walled");
                break;
            
            case { Cliff: true }:
                Debug.Log("Cliffed");
                break;
            default:
                Gravity();
                break;
        }

        Look(MoveInput);
        Move(MoveInput);
    }

    void Look(Vector2 input)
    {
        
    }

    void Move(Vector2 input)
    {
        if (input.x == 0 && input.y == 0) { return; }
        transform.forward = transform.InverseTransformDirection(new Vector3(input.x, 0, input.y));
        transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(GroundCheck.transform.position,GroundCheck.transform.forward * GroundCheck.RayLenght);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(WallCheck.transform.position, WallCheck.transform.forward * WallCheck.RayLenght);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(CliffCheck.transform.position, CliffCheck.transform.forward * CliffCheck.RayLenght);
    }
    void Gravity()
    {
        if (!UseGravity) { return; }
       transform.position += (transform.up * GravitySpeed) * Time.fixedDeltaTime;
    }
    [Serializable]
    struct RayPoint
    {
       [SerializeField] public Transform transform;
       [SerializeField] public Vector3 Raydirection;
       [SerializeField] public float RayLenght;
    }
    [Serializable]
    struct Hit
    {
        public bool Ground;
        public bool Wall;
        public bool Cliff;
    }

    private void OnEnable(){_Controls.actionmap.Enable();}
    private void OnDisable(){_Controls.actionmap.Disable();}
}
