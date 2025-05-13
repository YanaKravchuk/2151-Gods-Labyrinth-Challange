using UnityEngine;

namespace MazeGenerator
{
    public class Target : MonoBehaviour
    {
        private Vector3 targetPosition;
        private bool isMoving = false;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetTargetPosition();
            }
        }

        private void SetTargetPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;

            transform.position = mousePosition;
        }
    }
}
