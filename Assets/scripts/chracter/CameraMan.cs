// naokinakagawa
// 2017/05/13
using System.Collections.Generic;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class CameraMan : MonoBehaviour
    {
        [SerializeField] private List<Transform> target;
        [SerializeField] private Vector3 offset;

        private Vector3 GetTargetPosition()
        {
            Vector3 targetPosition = Vector3.zero;
            for (int i = 0; i < target.Count; i++)
            {
                targetPosition += target[i].position;
            }
            return targetPosition / target.Count;
        }

        public void AddTarget(Transform t)
        {
            target.Add(t);
        }

        public void RemoveTarget(Transform t)
        {
            target.Remove(t);
        }

        private void Awake()
        {
            transform.position = GetTargetPosition() + offset;
        }
	
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, GetTargetPosition() + offset, 0.1f);
        }
    }
}
