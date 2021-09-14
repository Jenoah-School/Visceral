using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMouse : MonoBehaviour
{
    [SerializeField] private bool unlockMouseOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (unlockMouseOnStart) MouseUnlock();
    }

    public void MouseUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
