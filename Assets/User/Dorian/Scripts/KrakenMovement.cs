using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KrakenMovement : MonoBehaviour
{
    
    [SerializeField] Check GroundCheck;
    [SerializeField] Check WallCheck;
    [SerializeField] Check CliffCheck;
    [SerializeField] LayerMask RaycastLayerMask;

    [SerializeField] float Gravity;
    [SerializeField] AnimationCurve LerpCurve;

    Controls _Controls;

    Vector2 MovementOffset;

    Rigidbody _Rigidbody;
    private void Awake()
    {
        _Controls = new Controls();
    }

    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        GroundCheck.Raylayermask = RaycastLayerMask;
        WallCheck.Raylayermask = RaycastLayerMask;
        CliffCheck.Raylayermask = RaycastLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        MovementOffset = _Controls.actionmap.Movemnet.ReadValue<Vector2>();
        Debug.Log("raw"+MovementOffset);
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (GroundCheck.Cast(out hit))
        {
            _Rigidbody.velocity = Vector3.zero;
            transform.up = hit.normal;
            Debug.Log("offset" + (Vector3)MovementOffset * Time.fixedDeltaTime);
            transform.position += transform.InverseTransformPoint( transform.localPosition+new Vector3( MovementOffset.x,0, MovementOffset.y) * Time.fixedDeltaTime);

        }
        else
        {
            _Rigidbody.velocity += Vector3.up * Gravity * Time.fixedDeltaTime;
            transform.up = Vector3.up;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck.DebugColor;
        Gizmos.DrawSphere(GroundCheck.Transform.position + GroundCheck.direction.normalized * GroundCheck.Raylength, 0.1f);
        Gizmos.DrawRay(GroundCheck.Transform.position, GroundCheck.direction.normalized * GroundCheck.Raylength);

        Gizmos.color = WallCheck.DebugColor;
        Gizmos.DrawSphere(WallCheck.Transform.position + WallCheck.direction.normalized * WallCheck.Raylength, 0.1f);
        Gizmos.DrawRay(WallCheck.Transform.position, WallCheck.direction.normalized * WallCheck.Raylength);

        Gizmos.color = CliffCheck.DebugColor;
        Gizmos.DrawSphere(CliffCheck.Transform.position + CliffCheck.direction.normalized * CliffCheck.Raylength, 0.1f);
        Gizmos.DrawRay(CliffCheck.Transform.position, CliffCheck.direction.normalized * CliffCheck.Raylength);
    }
    [Serializable]
    struct Check
    {
        public Transform Transform;
        public float Raylength;
        public bool invert;
        public Vector3 direction;
        public Color DebugColor;
        [NonSerialized] public LayerMask Raylayermask;
        public bool Cast(out RaycastHit hit)
        {
            bool hashit = Physics.Raycast(new Ray(Transform.position, direction), out hit, Raylength,Raylayermask);
            if (invert) { hashit = !hashit; }
            return hashit;
        }

    }

    private void OnEnable(){ _Controls.Enable(); }
    private void OnDisable(){ _Controls.Disable();}
}
