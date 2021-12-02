using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private int ready = 0;
    [SerializeField]
    private GameObject spectatePosPlayer1;
    [SerializeField]
    private GameObject spectatePosPlayer2;
    [SerializeField]
    private float duration;


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

    // Start is called before the first frame update
    void Start()
    {
        Player_Movement[] players = FindObjectsOfType<Player_Movement>();
        player1.gameObject = players[0].gameObject;
        player2.gameObject = players[1].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready == 2)
        {
            player1.gameObject.transform.position = Vector3.Lerp(player1.startPos, spectatePosPlayer1.transform.position, (Time.time - player1.startTime - Time.deltaTime) / duration);
            player2.gameObject.transform.position = Vector3.Lerp(player2.startPos, spectatePosPlayer2.transform.position, (Time.time - player2.startTime - Time.deltaTime) / duration);
        }
    }
    public void Ready()
    {
        ready++;
        if (ready == 2)
        {
            PrePhase2(player1, spectatePosPlayer1);
            PrePhase2(player2, spectatePosPlayer2);
            
        }
    }

    public void PrePhase2(Player player, GameObject spectatePos)
    {
        Vector3 posP1 = player.gameObject.transform.position;  
        player.startTime = Time.time;
        player.journeyLength = Vector3.Distance(posP1, spectatePos.transform.position);
        player.startPos = new Vector3(posP1.x, posP1.y, posP1.z);
        player.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void EndPhase2()
    {
        ready = 0;
        player1.gameObject.GetComponent<MeshRenderer>().enabled = true;
        player2.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    


}
