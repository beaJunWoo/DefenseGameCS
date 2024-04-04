using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface nav;
    public Transform startPos;
    public Transform endPos;

    public bool CheckNavigation()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(startPos.position, endPos.position, NavMesh.AllAreas, path);

        return path.status == NavMeshPathStatus.PathComplete ? true : false;
    }
}