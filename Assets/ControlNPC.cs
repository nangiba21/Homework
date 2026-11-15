using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPC : MonoBehaviour
{
    bool isWandering = true;
    bool isFollowingWayPoints = false;
    float timer;
    GameObject wanderingTarget;
    public GameObject target;
    public GameObject WP1, WP2, WP3, WP4;
    int WPCount;
    GameObject[] WayPoints;

    void Start()
    {
        WP1 = GameObject.Find("wp1");
        WP2 = GameObject.Find("wp1 (1)");
        WP3 = GameObject.Find("wp1 (2)");
        WP4 = GameObject.Find("wp1 (3)");

        WayPoints = new GameObject[] { WP1, WP2, WP3, WP4 };
        WPCount = 0;

        for (int i = 0; i < WayPoints.Length; i++)
        {
            if (WayPoints[i] != null)
            {
                WayPoints[i].GetComponent<Renderer>().enabled = false;
            }
        }

        if (isWandering)
        {
            wanderingTarget = new GameObject("WanderingTarget");
            wanderingTarget.transform.position = new Vector3(20, 0, 20);
            target = wanderingTarget;
            GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        }
    }

    void Update()
    {
        if (target != null && GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        }

        if (isWandering)
        {
            timer += Time.deltaTime;
            if (timer > 4)
            {
                timer = 0;
                wander();
            }
        }

        if (isFollowingWayPoints && WayPoints.Length > 0 && WayPoints[WPCount] != null)
        {
            target = WayPoints[WPCount];
            if (Vector3.Distance(transform.position, target.transform.position) < 1.0f)
            {
                moveToNextNPC();
            }
        }

        checkAhead();
    }

    void wander()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = transform.position + Vector3.up * 0.7f;
        float distanceToObstacle = 0;
        float castingDistance = 20;
        int safetyCounter = 0; 

        do
        {
            float randomDirectionX = Random.Range(-1.0f, 1.0f);
            float randomDirectionZ = Random.Range(-1.0f, 1.0f);
            ray.direction = transform.forward * randomDirectionZ + transform.right * randomDirectionX;

            safetyCounter++;
            if (safetyCounter > 100) 
            {
                break;
            }
        }
        while (distanceToObstacle < 1.0f && safetyCounter <= 100);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
        {
            distanceToObstacle = hit.distance;
        }
        else
        {
            distanceToObstacle = castingDistance;
        }

        if (wanderingTarget != null)
        {
            wanderingTarget.transform.position = ray.origin + ray.direction * Mathf.Max(distanceToObstacle, 2.0f);
            target = wanderingTarget;
        }
    }

    void moveToNextNPC()
    {
        WPCount++;
        if (WPCount > WayPoints.Length - 1) WPCount = 0;
    }

    void moveToRandomWP()
    {
        int previous = WPCount;
        int random = 0;
        do
        {
            random = Random.Range(0, WayPoints.Length);
        }
        while (random == previous);
        WPCount = random;
    }

    void checkAhead()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        float castingDistance = 2;
        ray.origin = transform.position + Vector3.up * 0.7f;
        ray.direction = transform.forward * castingDistance;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, castingDistance))
        {
            if (isWandering)
            {
                wander();
            }
        }
    }
}