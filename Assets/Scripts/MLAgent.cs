using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLAgent : Agent
{
    private Rigidbody mlAgentBody;
    [SerializeField]
    public float maxSpeed = 200f;
    [SerializeField]
    private float runspeed = 1.5f;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private Vector3 beginPosition;
    [SerializeField]
    private Vector3 beginrotation;

    public override void Initialize()
    {
        mlAgentBody = FindObjectOfType<Rigidbody>();
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
         if(mlAgentBody.velocity.magnitude > maxSpeed)
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
        MoveForward(actions.DiscreteActions);
        Strafe(actions.DiscreteActions);
        LookY(actions.DiscreteActions);
        LookX(actions.DiscreteActions);
        Jump(actions.DiscreteActions);
        AddReward(-1f / MaxStep == 0 ? 5000 : MaxStep);
    }

    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.CompareTag("Goal")) {
            SetReward(1f);
            print("hit the goal");
            EndEpisode();
        }
        if(col.gameObject.CompareTag("Wall")) {
            AddReward(-0.1f);
        }
    }

    private void Jump(ActionSegment<int> discreteActions)
    {
        int action = discreteActions[4];
        if (action == 1)
        {

        }
    }

    private void LookX(ActionSegment<int> discreteActions)
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

    private void LookY(ActionSegment<int> discreteActions)
    {
        var rotateDir = Vector3.zero;
        var action = discreteActions[3];

        switch (action)
        {
            case 1:
                rotateDir = Vector3.right * 1f;
                break;
            case 2:
                rotateDir = Vector3.right * -1f;
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
                direction = transform.right * 1f;

                break;
            case 2:
                direction = transform.right * -1f;
                break;

        }
        mlAgentBody.AddForce(direction * runspeed, ForceMode.VelocityChange);
    }

    private void MoveForward(ActionSegment<int> discreteActions)
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
            a[1] = 2;
        if (Input.GetKey(KeyCode.D))
            a[1] = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            a[2] = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            a[2] = 2;
        if (Input.GetKey(KeyCode.UpArrow))
            a[3] = 2;
        if (Input.GetKey(KeyCode.DownArrow))
            a[3] = 1;
    }
}
