using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLAgent : Agent
{   
    [SerializeField]
    private Material correct;
    [SerializeField]
    private Material wrong;
    [SerializeField]
    private Material glass;
    [SerializeField]
    private Rigidbody mlAgentBody;
    [SerializeField]
    private GameObject backWall;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    public float maxSpeed = 200f;
    [SerializeField]
    private float runspeed = 1.5f;
    [SerializeField]
    private Vector3 beginPosition;
    [SerializeField]
    private Vector3 beginrotation;


    public override void Initialize()
    {
       
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = beginPosition;
        transform.rotation = Quaternion.Euler(beginrotation.x, beginrotation.y, beginrotation.z);
        mlAgentBody.velocity *= 0f;
        mlAgentBody.angularVelocity *= 0f;
    }

   void Update()
   {
        if (mlAgentBody.velocity.magnitude > maxSpeed)
        {
            mlAgentBody.velocity = mlAgentBody.velocity.normalized * maxSpeed;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(goal.transform.localPosition);

        sensor.AddObservation(Vector3.Distance(goal.transform.position, transform.position));

        sensor.AddObservation(transform.InverseTransformDirection(mlAgentBody.velocity));

        sensor.AddObservation(transform.localPosition);

        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Move(actions.DiscreteActions);
        Look(actions.DiscreteActions);
        AddReward(-1f/MaxStep);
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.CompareTag("Goal")) {
            SetReward(2f);
            StartCoroutine(GoalScored(0.5f, correct));
            EndEpisode();
        }
        if (col.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f);
            StartCoroutine(GoalScored(0.3f, wrong));
            EndEpisode();
        }
    }

    IEnumerator GoalScored(float time, Material material) {
        backWall.GetComponent<Renderer>().material = material;
        yield return new WaitForSeconds(time);
        backWall.GetComponent<Renderer>().material = glass;
    }

   
    private void Look(ActionSegment<int> discreteActions)
    {
        var rotateDir = Vector3.zero;
        var action = discreteActions[1];

        switch (action)
        {
            case 1:
                rotateDir = Vector3.right * 1f;
                break;
            
            case 2:
                rotateDir = Vector3.right * -1f;
                break;
            
            case 3:
                rotateDir = Vector3.up * 1f;
                break;
            
            case 4:
                rotateDir = Vector3.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
    }

    private void Move(ActionSegment<int> discreteActions)
    {
        Vector3 direction = Vector3.zero;

        int action = discreteActions[0];
        switch (action)
        {
            case 1:
                direction = new Vector3(0,0,1) * 1f;
                break;

            case 2:
                direction = new Vector3(0,0,1) * -1f;
                break;
            
            case 3:
                direction = new Vector3(1, 0, 0) * -1f;
                break;

            case 4:
                direction = new Vector3(1, 0, 0) * 1f;
                break;

            case 5:
                direction = new Vector3(0, 1, 0) * 1f;
                break;

            case 6:
                direction = new Vector3(0, 1, 0) * -1f;
                break;
        }
        mlAgentBody.AddForce(direction * runspeed, ForceMode.VelocityChange);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       var a = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W))
            a[0] = 1;
        if (Input.GetKey(KeyCode.S))
            a[0] = 2;
        if (Input.GetKey(KeyCode.A))
            a[0] = 3;
        if (Input.GetKey(KeyCode.D))
            a[0] = 4;
        if (Input.GetKey(KeyCode.LeftArrow))
            a[1] = 4;
        if (Input.GetKey(KeyCode.RightArrow))
            a[1] = 3;
        if (Input.GetKey(KeyCode.UpArrow))
            a[1] = 2;
        if (Input.GetKey(KeyCode.DownArrow))
            a[1] = 1;
    }
}
