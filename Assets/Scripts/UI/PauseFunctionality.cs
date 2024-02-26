using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseFunctionality : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SwitchActionMap(string mapName)
    {
        // find player and set action map
        FindFirstObjectByType<PlayerInput>()?.SwitchCurrentActionMap(mapName);
    }
}