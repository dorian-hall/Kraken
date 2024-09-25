using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class KrakenMovement : MonoBehaviour
{
    
    [SerializeField] Check GroundCheck;
    [SerializeField] Check WallCheck;
    [SerializeField] Check CliffCheck;
    [SerializeField] LayerMask RaycastLayerMask;

    [SerializeField] float speed;
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
    }
    private void FixedUpdate()
    {
        RaycastHit Groundhit;
        if (GroundCheck.Cast(out Groundhit))
        {
            _Rigidbody.velocity = Vector3.zero;
            transform.up = Groundhit.normal;
        }
        else
        {
            _Rigidbody.velocity += Vector3.up * Gravity * Time.fixedDeltaTime;
            transform.up = Vector3.up;
        }
        RaycastHit Wallhit;
        if (WallCheck.Cast(out Wallhit))
        {
            Debug.Log("wallhit");
            _Rigidbody.velocity = Vector3.zero;
            transform.up = Wallhit.normal;
        }
        transform.GetChild(0).transform.forward = MovementOffset.normalized/2;
        transform.position += transform.right * MovementOffset.normalized.x * speed * Time.fixedDeltaTime;
        transform.position += transform.forward * MovementOffset.normalized.y * speed * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck.DebugColor;
        Gizmos.DrawRay(GroundCheck.Transform.position, GroundCheck.direction.normalized * GroundCheck.Raylength);

        Gizmos.color = WallCheck.DebugColor;
        Gizmos.DrawRay(WallCheck.lastray);

        Gizmos.color = CliffCheck.DebugColor;
        Gizmos.DrawRay(CliffCheck.Transform.position, CliffCheck.direction.normalized * CliffCheck.Raylength);
    }
    [Serializable]
    struct Check
    {
        public Transform Transform;
        public float Raylength;
        public bool invert;
        public bool local;
        public Vector3 direction;
        public Color DebugColor;
        [NonSerialized] public LayerMask Raylayermask;
        [NonSerialized] public Ray lastray ;
        public bool Cast(out RaycastHit hit)
        {
            
            if (local) { lastray = new Ray(Transform.position, Transform.parent.TransformDirection(direction)); }
            else { lastray =  new Ray(Transform.position, direction); }
            bool hashit = Physics.Raycast(lastray, out hit, Raylength, Raylayermask);
            if (invert) { hashit = !hashit; }
            return hashit;
        }

    }

    private void OnEnable(){ _Controls.Enable(); }
    private void OnDisable(){ _Controls.Disable();}
}
