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
    private Spawner spawner;
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
       print(spawner);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = beginPosition;
        transform.rotation = Quaternion.Euler(beginrotation.x, beginrotation.y, beginrotation.z);
        mlAgentBody.velocity *= 0f;
        mlAgentBody.angularVelocity *= 0f;
        spawner.resetArea();
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
        sensor.AddObservation(goal.transform.position);

        sensor.AddObservation(Vector3.Distance(goal.transform.position, transform.position));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Move(actions.DiscreteActions);
        Strafe(actions.DiscreteActions);
        LookY(actions.DiscreteActions);
        LookX(actions.DiscreteActions);
        AddReward(-1f/MaxStep);
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.CompareTag("Goal")) {
            SetReward(1f);
            StartCoroutine(GoalScored(correct, 0.5f));
            EndEpisode();
        }
        if(col.gameObject.CompareTag("Block")) {
            SetReward(-0.1f);
        }
    }

    IEnumerator GoalScored(Material mat, float time) {
        backWall.GetComponent<Renderer>().material = mat;
        yield return new WaitForSeconds(time);
        backWall.GetComponent<Renderer>().material = glass;
    }

    private void LookY(ActionSegment<int> discreteActions)
    {
        var rotateDir = Vector3.zero;
        var action = discreteActions[2];

        switch (action)
        {            
            case 1:
                rotateDir = Vector3.up * 1f;
                break;
            
            case 2:
                rotateDir = Vector3.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
    }

    private void LookX(ActionSegment<int> discreteActions)
    {
        var rotateDir = Vector3.zero;
        var action = discreteActions[3];

        switch (action)
        {            
            case 1:
                rotateDir = Vector3.right * -1f;
                break;

            case 2:
                rotateDir = Vector3.right * 1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);               
    }

    private void Strafe(ActionSegment<int> discreteActions) 
    {
        Vector3 direction = Vector3.zero;
        int action = discreteActions[1];
     
        switch (action)
        {
            case 1:
                direction = transform.right * -1f;
                break;

            case 2:
                direction = transform.right * 1f;
                break;
        }
        mlAgentBody.AddForce(direction * runspeed, ForceMode.VelocityChange);
    }

    private void Move(ActionSegment<int> discreteActions)
    {
        Vector3 direction = Vector3.zero;
        int action = discreteActions[0];

        switch (action)
        {
            case 1:
                direction = transform.forward * 1f;
                break;

            case 2:
                direction = transform.forward * -1f;
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
            a[1] = 1;
        if (Input.GetKey(KeyCode.D))
            a[1] = 2;
        if (Input.GetKey(KeyCode.RightArrow))
            a[2] = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            a[2] = 2;
        if (Input.GetKey(KeyCode.UpArrow))
            a[3] = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            a[3] = 2;
    }
}
