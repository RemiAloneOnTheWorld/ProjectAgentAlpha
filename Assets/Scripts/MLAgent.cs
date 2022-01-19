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
    private Material standard;
    [SerializeField]
    private Rigidbody mlAgentBody;
    public GameObject goal;
    [SerializeField]
    private Spawner spawner = null;
    [SerializeField]
    public float maxSpeed = 200f;
    [SerializeField]
    private float runspeed = 1.5f;
    [SerializeField]
    private Vector3 beginPosition;
    [SerializeField]
    private Vector3 beginrotation;
    [SerializeField]
    private bool useVectorObs;
    [SerializeField]
    private bool useBeginPosition;

    private SpaceshipManager _spaceshipManager;
    private bool _addArrivedCount = true;


    public override void Initialize()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        if(goal != null)
            return;
        if(Vector3.Distance(goals[0].transform.position, transform.position) > Vector3.Distance(goals[1].transform.position, transform.position)) {
            goal = goals[1];
        } else {
            goal = goals[0];
        }
        beginPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        if (useBeginPosition)
            transform.localPosition = beginPosition;

        transform.rotation = Quaternion.Euler(beginrotation.x, beginrotation.y, beginrotation.z);
        mlAgentBody.velocity *= 0f;
        mlAgentBody.angularVelocity *= 0f;
        if(spawner != null)
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

        sensor.AddObservation(transform.position);

        if(useVectorObs)
            sensor.AddObservation(StepCount / (float)MaxStep);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Move(actions.DiscreteActions);
        LookY(actions.DiscreteActions);
        LookX(actions.DiscreteActions);
        AddReward(-1f/MaxStep);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Goal")) {
            SetReward(1f);
            StartCoroutine(GoalScored(correct, 0.5f));
            EndEpisode();

            //Add to arrived spaceships.
            if (_addArrivedCount)
            {
                _spaceshipManager.ArrivedSpaceships++;
                _addArrivedCount = false;
            }
        }
        if(col.gameObject.CompareTag("Block")) {
            AddReward(-0.5f);
            StartCoroutine(GoalScored(wrong, 0.5f));
        }
        if(col.gameObject.CompareTag("Wall")) {
            AddReward(-0.1f);
        }
    }

    IEnumerator GoalScored(Material mat, float time)
    {
        goal.GetComponent<Renderer>().material = mat;
        yield return new WaitForSeconds(time);
        goal.GetComponent<Renderer>().material = standard;
    }

    private void LookY(ActionSegment<int> discreteActions)
    {
        var rotateDir = Vector3.zero;
        var action = discreteActions[1];

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
        var action = discreteActions[2];

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
        mlAgentBody.AddForce(direction * Time.deltaTime * runspeed, ForceMode.VelocityChange);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       var a = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W))
            a[0] = 1;
        if (Input.GetKey(KeyCode.S))
            a[0] = 2;
        if (Input.GetKey(KeyCode.RightArrow))
            a[1] = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            a[1] = 2;
        if (Input.GetKey(KeyCode.UpArrow))
            a[2] = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            a[2] = 2;
    }

    public void SetSpaceshipManager(SpaceshipManager manager)
    {
        _spaceshipManager = manager;
    }
}
