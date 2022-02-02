using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] screens;

    int activeScreen;

    private void Start()
    {
        activeScreen = 0;
    }

    public void nextScreen()
    {

        screens[activeScreen].SetActive(false);
        deactivateChildren(screens[activeScreen]);

        if (activeScreen == screens.Length-1)
        {
            activeScreen = 0;
        } else
        {
            activeScreen++;
        }
        screens[activeScreen].SetActive(true);
        activateChildren(screens[activeScreen]);
    }

    public void previousScreen()
    {
        
        if (activeScreen == 0)
        {
            return;
        }
        else
        {
            screens[activeScreen].SetActive(false);
            deactivateChildren(screens[activeScreen]);
            activeScreen--;
            screens[activeScreen].SetActive(true);
            activateChildren(screens[activeScreen]);
        }
        
    }

    void activateChildren(GameObject parentObject)
    {
        for (int i = 0; i < screens[activeScreen].transform.childCount; i++)
        {
            parentObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void deactivateChildren(GameObject parentObject)
    {
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            parentObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
