using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{

    [SerializeField]
    private float fadeSpeed;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public IEnumerator fadeOut()
    {
        while (image.color.a < 0.98f){
            Color startColor = image.color;
            image.color = Color.Lerp(startColor, Color.black, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        image.color = new Color(0f, 0f, 0f, 1f);
        yield return null;
    }

    public IEnumerator fadeIn()
    {
        while (image.color.a > 0.02f)
        {
            Color startColor = image.color;
            image.color = Color.Lerp(startColor, Color.clear, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        image.color = new Color(0f, 0f, 0f, 0f);
        yield return null;
    }
}
