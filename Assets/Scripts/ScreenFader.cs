using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{

    [SerializeField]
    private float fadeSpeed;
    private Image image;
    public bool IsFading { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public IEnumerator fadeOut(EventType eventType) {
        IsFading = true;
        while (image.color.a < 0.98f)
        {
            Color startColor = image.color;
            image.color = Color.Lerp(startColor, Color.black, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        image.color = new Color(0f, 0f, 0f, 1f);

        if (eventType == EventType.PreparationPhaseOver) {
            EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToAttack, "Attack Phase"));   
        }
        else if(eventType == EventType.AttackPhaseOver){
            EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToAttack, "Preparation Phase"));   
        }
        yield return fadeIn();
    }

    public IEnumerator fadeIn()
    {
        yield return new WaitForSeconds(1);
        while (image.color.a > 0.02f)
        {
            Color startColor = image.color;
            image.color = Color.Lerp(startColor, Color.clear, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        image.color = new Color(0f, 0f, 0f, 0f);
        IsFading = false;
        yield return null;
    }
}
