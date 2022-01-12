using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{
    [SerializeField] private Button menuSelectButton;
    [SerializeField] private Slider optionsSelectButton;
    [SerializeField] private Button aboutSelectButton;
    [SerializeField] private PlayerInput playerInput;

    public void Start()
    {

        SelectStartMenu();

    }

    //StartMenu
    public void SelectStartMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + menuSelectButton.gameObject);
            //EventSystem.current.SetSelectedGameObject(menuSelectButton.gameObject);
            menuSelectButton.Select();
            menuSelectButton.OnSelect(null);
            print("Select menu");
           
        }
    }
    //OptionsMenu
    public void SelectOptionsMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + optionsSelectButton.gameObject);
            //EventSystem.current.SetSelectedGameObject(optionsSelectButton.gameObject);

            optionsSelectButton.Select();
            optionsSelectButton.OnSelect(null);
            print("Select options");
            
        }
    }

    //AboutMenu

    public void SelectAboutMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + aboutSelectButton.gameObject);
            //EventSystem.current.SetSelectedGameObject(aboutSelectButton.gameObject);
            aboutSelectButton.Select();
            aboutSelectButton.OnSelect(null);
            print("Select about");
        
        }
    }

    public void OnNavigate()
    {
        print(EventSystem.current.currentSelectedGameObject);
    }
    public void OnControlsChanged()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            if (menuSelectButton.transform.parent.gameObject.activeSelf == true)
            {
                SelectStartMenu();
            }
            if (optionsSelectButton.transform.parent.gameObject.activeSelf == true)
            {
                SelectOptionsMenu();
            }
            if (aboutSelectButton.transform.parent.gameObject.activeSelf == true)
            {
                SelectAboutMenu();
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        

    }
}
