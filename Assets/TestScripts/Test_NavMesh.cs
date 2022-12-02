using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test_NavMesh : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public void Start()
    {
        index = 0;
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }
    int index;
    NavMeshAgent agent;
    void MoveToRandomPoint()
    {
        int nextPoint = Random.Range(0, wayPoints.Count);
        while (nextPoint == index)
            nextPoint = Random.Range(0, wayPoints.Count);

        index = nextPoint;
        var pathCheck = agent.SetDestination(wayPoints[index].position);
        if (!pathCheck)
            Debug.LogError($"SetDestination Error");
    }

    private void Update()
    {
        if (agent.remainingDistance <= 1f)
        {
            Debug.Log("MoveEnd. NextPoint");
            MoveToRandomPoint();
        }
        else
            Debug.Log($"Remaining... { agent.remainingDistance}");
    }
}
