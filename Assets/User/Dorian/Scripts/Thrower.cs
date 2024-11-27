using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PhysicsGunInteractionBehavior))]
public class Thrower : MonoBehaviour
{
    private Controls _Controls;
    private PhysicsGunInteractionBehavior _PhysGun;
    private float Power;
    private GameObject _CurrentGrabedObject;

    public float MaxPower;
    public float GetPower { get => Power; }

    private void Awake()
    {
        _Controls = new Controls();
    }
    // Start is called before the first frame update
    void Start()
    {
        _PhysGun.OnObjectGrabbed.AddListener(GrabCheck);
    }

    void GrabCheck( GameObject gameObject )
    {
        if(gameObject != null){ Grabed(gameObject); }
        else { Throw(); }
    }
    void Grabed(GameObject gameObject)
    {
        Power = 0;
        _CurrentGrabedObject = gameObject;
    }

    void Throw()
    {

        _CurrentGrabedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable(){ _Controls.Enable(); }
    private void OnDisable(){ _Controls.Disable();}
}
