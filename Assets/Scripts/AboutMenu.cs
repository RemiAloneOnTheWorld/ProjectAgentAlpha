using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AboutMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Button backButton;

    public void SelectMenuItem()
    {
        if (_playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(backButton.gameObject);
            backButton.OnSelect(null);
        }
    }
}
