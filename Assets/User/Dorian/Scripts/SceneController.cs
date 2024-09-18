using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    Controls _Controls;
    
    // Start is called before the first frame update
    void Awake()
    {
        _Controls = new Controls();
        _Controls.actionmap.Restart.performed += ctx => ResstartScene();
    }

    void ResstartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable(){_Controls.Enable();}
    private void OnDisable(){ _Controls.Disable(); }
}
