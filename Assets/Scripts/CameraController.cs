using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private int ready = 0;

    public class Player
    {
        public GameObject gameObject;
        public float startTime;
        public float journeyLength;
        public float distCovered;
        public float fractionOfJourney;
        public Vector3 startPos;
    }

    private Player player1 = new Player();
    private Player player2 = new Player();
    private Animator animationStates;
    private ScreenFader fader;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        Player_Movement[] players = FindObjectsOfType<Player_Movement>();
        fader = FindObjectOfType<ScreenFader>();
        animationStates = GetComponent<Animator>();
        player1.gameObject = players[0].gameObject;
        player2.gameObject = players[1].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(ready == 1)
        {
            //these should be called by a game controller instead, (thats why they are public)
            Ready();
        }
        if(ready == 3)
        {
            //these should be called by a game controller instead, (thats why they are public)
            UnReady();
        }
        //only for testing it is being called here.
    }
    public void Ready()
    {
        ready++;
        if (ready == 2)
        {
            StartCoroutine(startPhase());
         
        }
    }

    public void UnReady()
    {
        ready = 0;
        EndPhase2(player1, 0.5f);
        EndPhase2(player2, 0);
        animationStates.SetFloat("Ready", ready);
    }

    IEnumerator startPhase()
    {
        StartCoroutine(fader.fadeOut());
        yield return new WaitForSeconds(4);
        PrePhase2(player1, 0);
        PrePhase2(player2, 0.5f);
        animationStates.SetFloat("Ready", ready);
        StartCoroutine(fader.fadeIn());
    }


    private void PrePhase2(Player player, float yCam)
    {
        
        //player.gameObject.GetComponent<MeshRenderer>().enabled = false;
        player.gameObject.GetComponentInChildren<Camera>().rect = new Rect(0,yCam,1,0.5f);
        
    }

    private void EndPhase2(Player player,float xCam)
    {
        //player.gameObject.GetComponent<MeshRenderer>().enabled = true;
        player.gameObject.GetComponentInChildren<Camera>().rect = new Rect(xCam, 0, 0.5f, 1);
    }

    


}
