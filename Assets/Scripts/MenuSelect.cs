using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{   [SerializeField] private PlayerInput playerInput;
    
    [Header("SelectButtons")]
    [SerializeField] private Button menuSelectButton;
    [SerializeField] private Slider optionsSelectButton;
    [SerializeField] private Button aboutSelectButton;
    [Header("Menus")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject aboutMenu;



    public void Start()
    {

        StartCoroutine(SelectStartMenu());

    }

    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        aboutMenu.SetActive(false);
        StartCoroutine(SelectStartMenu());
    }

    public void ShowOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        aboutMenu.SetActive(false);
        StartCoroutine(SelectOptionsMenu());
    }

    public void ShowAboutMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        aboutMenu.SetActive(true);
        StartCoroutine(SelectAboutMenu());
    }

    //StartMenu

    IEnumerator SelectStartMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + menuSelectButton.gameObject);
            yield return new WaitForEndOfFrame();
            //EventSystem.current.SetSelectedGameObject(menuSelectButton.gameObject);
            menuSelectButton.Select();
            menuSelectButton.OnSelect(null);
            print("Select menu");
            
           
        }
    }
    //OptionsMenu
    IEnumerator SelectOptionsMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + optionsSelectButton.gameObject);
            yield return new WaitForEndOfFrame();
            //EventSystem.current.SetSelectedGameObject(optionsSelectButton.gameObject);

            optionsSelectButton.Select();
            optionsSelectButton.OnSelect(null);
            print("Select options");
            
        }
    }

    //AboutMenu

    IEnumerator SelectAboutMenu()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            print("In method " + aboutSelectButton.gameObject);
            yield return new WaitForEndOfFrame();
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
                StartCoroutine(SelectStartMenu());
            }
            if (optionsSelectButton.transform.parent.gameObject.activeSelf == true)
            {
                StartCoroutine(SelectOptionsMenu());
            }
            if (aboutSelectButton.transform.parent.gameObject.activeSelf == true)
            {
                StartCoroutine(SelectAboutMenu());
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        

    }
}
