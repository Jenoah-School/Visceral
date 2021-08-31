using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveInputToSelectable : MonoBehaviour
{
    [SerializeField] private UIAnimations deselectedPanel = null;
    public void MoveToSelectable(Selectable selectable)
    {
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }

    public void HidePanelOnDeselectHorizontal()
    {
        deselectedPanel.MoveToStartPosition();
    }
}
