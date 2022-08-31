using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathDisplayer : MonoBehaviour {
    [SerializeField] private Color color = Color.white;
    private NavMeshAgent agent;
    public void Start() {
        agent = gameObject.GetComponent<NavMeshAgent> ();
    }

    public void Update() {
        StartCoroutine(DrawPath(agent.path));
    }

    IEnumerator DrawPath(NavMeshPath path) {
        yield return new WaitForEndOfFrame();
        path = agent.path;
        if (path.corners.Length < 1)
            yield return null;
        
        switch (path.status) {
            case NavMeshPathStatus.PathComplete:
                color = Color.white;
                break;
            case NavMeshPathStatus.PathInvalid:
                color = Color.red;
                break;
            case NavMeshPathStatus.PathPartial:
                color = Color.yellow;
                break;
        }
        
        Vector3 previousCorner = path.corners[0];
    
        int i = 1;
        while (i < path.corners.Length) {
            Vector3 currentCorner = path.corners[i];
            previousCorner = currentCorner;
            i++;
        }
    
    }
}