using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class KrakenMovement : MonoBehaviour
{
    


    Controls _Controls;

    private void Awake()
    {
        _Controls = new Controls();
    }

    private void FixedUpdate()
    {
   
        
    }

    void UseGravity()
    {
       // transform.position += (transform.up * Gravity) * Time.fixedDeltaTime;
    }


    



}
