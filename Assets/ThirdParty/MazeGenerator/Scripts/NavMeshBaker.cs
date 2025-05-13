using NavMeshPlus.Components;
using UnityEngine;

namespace MazeGenerator
{
    public class NavMeshBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BakeNavMesh();
            }
        }

        void BakeNavMesh()
        {
            if (_navMeshSurface != null)
            {
                Debug.Log("Baking NavMesh...");
                _navMeshSurface.BuildNavMesh();
            }
            else
            {
                Debug.LogError("NavMeshSurface component not found!");
            }
        }
    }
}
