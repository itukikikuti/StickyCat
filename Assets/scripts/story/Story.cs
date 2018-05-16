// naokinakagawa
// 2017/04/22
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public abstract class Story : MonoBehaviour
    {
        protected bool isRunninng = false;

        private void OnCollisionEnter2D(Collision2D c)
        {
            OnTriggerEnter2D(c.collider);
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            if (isRunninng)
                return;
			Player p = c.GetComponent<Player>();
			if (p == null)
                return;
            isRunninng = true;
            StartCoroutine(Play(p));
        }

		public abstract IEnumerator Play(Player p);

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "Story.png", true);
        }
    }
}
