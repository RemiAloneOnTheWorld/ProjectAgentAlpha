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
    public GameObject[] goals;
    [SerializeField]
    private Spawner[] spawners = null;
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
    private bool training;
    private GameObject goal;

    public override void Initialize()
    {
        beginPosition = transform.localPosition;
    }

    private void Start() {
        if(!training)
            goal = findClosestGoal();
    }

    public GameObject findClosestGoal() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Goal");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public override void OnEpisodeBegin()
    {
        if (training) {
            transform.localPosition = beginPosition;

            foreach (GameObject goal in goals) {
                goal.SetActive(false);
            }

            int randomGoal = Random.Range(0,2);
            goals[randomGoal].SetActive(true);
            goal = goals[randomGoal];
        }

        transform.rotation = Quaternion.Euler(beginrotation.x, beginrotation.y, beginrotation.z);
        mlAgentBody.velocity *= 0f;
        mlAgentBody.angularVelocity *= 0f;
        if(spawners != null)
            foreach (Spawner spawner in spawners) {
                spawner.resetArea();
            }
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
        sensor.AddObservation(Vector3.Distance(goal.transform.position, transform.position));

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
        if(!training)
            this.enabled = false;
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
}
