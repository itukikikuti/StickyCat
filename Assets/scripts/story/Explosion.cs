// naokinakagawa
// 2017/08/03
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Explosion : Story
    {
        [SerializeField] private Vector2 force;

        public override IEnumerator Play(Player p)
        {
            p.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            p.GetComponent<Rigidbody2D>().AddTorque(-5f);
            yield break;
        }
    }
}
