using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform Target;
    [Header("Position")]
    [SerializeField] bool followPosition;
    [SerializeField] Vector3 PositionOffset;
    [Header("Rotation")]
    [SerializeField] bool followRotation;
    [SerializeField] Vector3 RotationOffset;
    [Header("Scale")]
    [SerializeField] bool followScale;
    [SerializeField] Vector3 ScaleOffset;

    void Update()
    {
        RotationOffset.x += Input.GetAxis("Mouse Y") * 90 * Time.deltaTime;
        //RotationOffset.y += Input.GetAxis("Mouse X") * 90 * Time.deltaTime;
        if (followPosition) { transform.position = Target.position + PositionOffset; }
        if (followRotation) { transform.rotation = Quaternion.Euler(Target.rotation.eulerAngles + RotationOffset); }
        if (followScale) { transform.localScale = Target.localScale + ScaleOffset; }
        
    }
}
