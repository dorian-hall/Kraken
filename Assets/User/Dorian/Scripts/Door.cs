using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform DoorTransfrom;
    public Vector3 Startpos;
    public Vector3 Endpos;
    public float speed;
    public bool ismoving;

    private void Start()
    {
        DoorTransfrom.position = Startpos;
    }
    IEnumerator Move(float lerp)
    {
        lerp += speed * Time.fixedDeltaTime;
        if (lerp < 1) yield return null;
        DoorTransfrom.position = Vector3.Lerp(Startpos, Endpos, lerp);
        yield return new WaitForFixedUpdate();
        StartCoroutine(Move(lerp));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ismoving) { return; }
        StartCoroutine(Move(0));
        ismoving = true;
    }

}
