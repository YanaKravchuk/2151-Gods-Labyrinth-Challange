using UnityEngine;
using UnityEngine.AI;

namespace MazeGenerator
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform _target;
        [SerializeField] private NavMeshAgent _agent;

        private void Start()
        {
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
        }

        void Update()
        {
            _agent.SetDestination(_target.position);

            if (Input.GetMouseButtonDown(1))
            {
                SetTargetPosition();
            }
            _agent.speed = speed;
        }

        private void SetTargetPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;

            transform.position = mousePosition;
        }

    }
}