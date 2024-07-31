using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GlobalNavMeshController : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    public void UpdateNavMesh(Vector3 position)
    {
        navMeshSurface.transform.position = position;
        navMeshSurface.BuildNavMesh();
    }
}
