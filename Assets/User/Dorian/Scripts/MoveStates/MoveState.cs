using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MoveState
{
    public abstract void OnEnter(MoveController moveController);


    public abstract void OnUpdate(MoveController moveController);


    public abstract void OnExit(MoveController moveController);

}
