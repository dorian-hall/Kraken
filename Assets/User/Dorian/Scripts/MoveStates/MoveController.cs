using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
/*
blöcke bleiben stecken ven sie los gelasen werden  
movement overhoul 
blcok boost bug wenn staning on block 
die kamera solte die beide 
*/
public class MoveController : MonoBehaviour
{
    [SerializeField] Check GroundCheck;
    [SerializeField] Check WallCheck;
    [SerializeField] Check CliffCheck;
    [SerializeField] LayerMask RaycastLayerMask;

    [SerializeField] float speed;
    [SerializeField] float Gravity;
    [SerializeField] AnimationCurve LerpCurve;
    Controls _Controls;

    enum MoveStateType {Idle,Move,Tranition,Jumping,Falling,}

    private void Awake()
    {
        _Controls = new Controls();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RaycastHit Groundhit;
        Ray GroundRay = new Ray(transform.TransformPoint(GroundCheck.Transform.localPosition), transform.TransformDirection(GroundCheck.direction.normalized));
        if (Physics.Raycast(GroundRay, out Groundhit, GroundCheck.Raylength, RaycastLayerMask))
        {

        }
        else {}
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck.DebugColor;
        Gizmos.DrawRay(transform.TransformPoint(GroundCheck.Transform.localPosition), transform.TransformDirection(GroundCheck.direction.normalized * GroundCheck.Raylength));

        Gizmos.color = WallCheck.DebugColor;
        Gizmos.DrawRay(transform.TransformPoint(WallCheck.Transform.localPosition), transform.TransformDirection(WallCheck.direction.normalized * WallCheck.Raylength));

        Gizmos.color = CliffCheck.DebugColor;
        Gizmos.DrawRay(transform.TransformPoint(CliffCheck.Transform.localPosition), transform.TransformDirection(CliffCheck.direction.normalized * CliffCheck.Raylength));
    }
    private void OnEnable() { _Controls.Enable(); }
    private void OnDisable() { _Controls.Disable(); }
    
    [Serializable]
    struct Check
    {
        public Transform Transform;
        public float Raylength;
        public bool invert;
        public Vector3 direction;
        public Color DebugColor;
        [NonSerialized] public LayerMask Raylayermask;
        [NonSerialized] public Ray lastray;
    }
}
