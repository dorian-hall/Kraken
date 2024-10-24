using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControls : MonoBehaviour
{
    [SerializeField] Texture2D CursorTex;
    [SerializeField] GameObject FixedCursor;
    public static MouseControls Instance;

    Controls _Controls;
    
    private void Awake()
    {
        _Controls = new Controls();
        _Controls.actionmap.Exit.performed += ctx => UpdateCursorMode(!Cursor.visible);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        
        UpdateCursorSprite();
        UpdateCursorMode(false);
    }

    public void UpdateCursorSprite(){ Cursor.SetCursor(CursorTex, Vector2.zero, CursorMode.Auto); }

    public void UpdateCursorMode(bool isvisible)
    {
        if (isvisible) 
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
        }
        else 
        { 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true; // this does not work for some reason.
        
        }
        FixedCursor.SetActive(!isvisible); // this is used because the cursor is not visible when locked.
    }
    private void OnEnable(){_Controls.Enable();}
    private void OnDisable(){_Controls.Disable();}
}
