using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private GameObject pausePanel;
    
    public static bool IsPaused { get; private set; }

    private void Start() => SetPause(false);
    private void OnEnable() => inputAction.action.performed += InputPerformed;
    private void OnDisable() => inputAction.action.performed -= InputPerformed;
    private void InputPerformed(InputAction.CallbackContext _) => SetPause(!IsPaused);

    public void ButtonPause() => SetPause(true);
    public void ButtonUnPause() => SetPause(false);
    
    private void SetPause(bool value)
    {
        IsPaused = value;
        
        Time.timeScale = IsPaused ? 0f : 1f;
        
        Cursor.visible = IsPaused;
        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        
        pausePanel?.SetActive(IsPaused);
    }
}