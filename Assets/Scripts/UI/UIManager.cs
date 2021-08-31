using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenu = null;
    //[SerializeField] private GameObject inventoryMenu = null;

    [Header("Settings")]
    [SerializeField] private bool lockMouseInGame = true;
    [SerializeField] private Selectable firstSelectionOnPause = null;

    private UIAnimations[] pausePanels = null;
    private VolumeFader volumeFader = null;
    private bool canUseMenus = true;

    private MoveInputToSelectable moveInputToSelectable = null;

    public static bool isPaused { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        pausePanels = pauseMenu.GetComponentsInChildren<UIAnimations>();
        volumeFader = FindObjectOfType<VolumeFader>();
        ResumeGame();
        //inventoryMenu.SetActive(false);
        moveInputToSelectable = GetComponent<MoveInputToSelectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canUseMenus) return;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        /*
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            SetMouseLock(!inventoryMenu.activeSelf);
            FreezeGame(inventoryMenu.activeSelf);
        }
        */
    }

    /// <summary>
    /// Sets timescale to 0 and pauses input
    /// </summary>
    /// <param name="isFrozen"></param>
    public void FreezeGame(bool isFrozen)
    {
        if (isFrozen)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    /// <summary>
    /// Sets ability to show/hide menus
    /// </summary>
    /// <param name="canInteract"></param>
    public void SetMenuInteractionState(bool canInteract)
    {
        canUseMenus = canInteract;
    }

    /// <summary>
    /// (Un)locks mouse pointer
    /// </summary>
    /// <param name="isLocked"></param>
    public void SetMouseLock(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }

    /// <summary>
    /// Shows the pausemenu and freezes game
    /// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        FreezeGame(true);
        SetMouseLock(false);

        if (volumeFader != null)
            volumeFader.FadeToVolumeLevel(-10f);

        if (moveInputToSelectable != null && firstSelectionOnPause != null)
            moveInputToSelectable.MoveToSelectable(firstSelectionOnPause);
    }

    /// <summary>
    /// Hides the pause menu and continues game
    /// </summary>
    public void ResumeGame()
    {
        if (pausePanels.Length > 0)
        {
            for (int i = 0; i < pausePanels.Length; i++)
            {
                pausePanels[i].MoveToStartPositionImmediately();
            }
        }
        pauseMenu.SetActive(false);

        FreezeGame(false);

        if (lockMouseInGame)
        {
            SetMouseLock(true);
        }

        if (volumeFader != null)
            volumeFader.FadeIn();
    }
}
