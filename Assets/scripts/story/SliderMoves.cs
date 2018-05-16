// naokinakagawa
// 2017/07/5
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class SliderMoves : Story
    {
        [SerializeField] private SliderJoint2D slider;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public override IEnumerator Play(Player p)
        {
            if (p.GetComponent<SpringJoint2D>().connectedBody != rb)
            {
                isRunninng = false;
                yield break;
            }
            slider.useMotor = false;
        }
    }
}
