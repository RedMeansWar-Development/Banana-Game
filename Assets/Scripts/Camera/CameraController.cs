using UnityEngine;

namespace BananaGame.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Settings")]
        public float smoothSpeed = 8f;
        public Vector3 offset = new Vector3(0f, 0f, -10f);

        private void LateUpdate()
        {
            if (target is null) return;

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        }
    }
}