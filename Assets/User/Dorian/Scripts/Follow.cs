using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Follow : MonoBehaviour
{
    [SerializeField] float maxRotation;
    [SerializeField] float minRotation;

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
        if(RotationOffset.x < minRotation) { RotationOffset.x = minRotation; }
        else if(RotationOffset.x > maxRotation) {  RotationOffset.x = maxRotation; }

        if (followPosition) { transform.position = Target.transform.TransformPoint(PositionOffset); }
        if (followRotation) { transform.rotation = Quaternion.Euler(Target.rotation.eulerAngles + RotationOffset); }
        if (followScale) { transform.localScale = Target.localScale + ScaleOffset; }
        
    }
}
