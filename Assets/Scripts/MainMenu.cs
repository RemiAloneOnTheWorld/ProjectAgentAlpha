using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Button startButton;

    private void Start()
    {
        SelectMenuItem();
    }

    public void OnControlsChanged()
    {
        SelectMenuItem();
    }

    public void RunGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectMenuItem()
    {
        if (_playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            startButton.OnSelect(null);
            print("Select menu");
        }
    }
}
