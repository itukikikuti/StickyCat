// naokinakagawa
// 2016/12/18
using UnityEngine;
using System.Collections;

namespace net.windblow.stickycat
{
    public class Jelly : MonoBehaviour
    {
        [SerializeField] public Vector3 original;
        [SerializeField] public float spring;
        [SerializeField] public float repulsion;
        [SerializeField] public float limit;
        private Vector3 _force;
        [HideInInspector]public bool realTime = false;

        private void FixedUpdate()
        {
            _force += (original - transform.localScale) * spring * Time.timeScale;
            _force *= Mathf.Lerp(1f, repulsion, Time.timeScale);
            transform.localScale += _force;
            transform.localScale = Vector3.Min(transform.localScale, original + new Vector3(limit, limit, limit));
            transform.localScale = Vector3.Max(transform.localScale, original - new Vector3(limit, limit, limit));
        }

        public void AddForce(Vector3 force)
        {
            _force += force;
        }
    }
}
