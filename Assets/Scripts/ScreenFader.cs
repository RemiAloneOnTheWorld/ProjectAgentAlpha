using System.Collections;
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
            EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToAttack, "AI Attack Phase"));
            EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, ""));
        }
        else if(eventType == EventType.AttackPhaseOver){
            EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToDestruction, "Destruction Phase"));
            EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, 
                "Done attacking? Press 'R' or 'North Button'"));
        }
        else {
            EventQueue.GetEventQueue().AddEvent(new MessageEventData(EventType.InitPreparationPhase, 
                "Initiate attack? Press 'R' or 'North Button'"));
            EventQueue.GetEventQueue().AddEvent(new PhaseUIEventData(EventType.InFadeToPreparation, "Preparation Phase"));
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
