// naokinakagawa
// 2017/07/23
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D c)
        {
            Player p = c.GetComponent<Player>();
            if (p == null)
                return;
            transform.parent = p.transform;
            transform.localPosition *= 0f;
        }
    }
}
