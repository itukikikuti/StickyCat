// naokinakagawa
// 2017/05/17
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Door : Gimmick
    {
        [SerializeField] private Vector3 force;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public override void Play()
        {
            rb.velocity = force;
        }
    }
}
