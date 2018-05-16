// naokinakagawa
// 2017/07/24
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Jail : Gimmick
    {
        private HingeJoint2D hj;

        private void Awake()
        {
            hj = GetComponent<HingeJoint2D>();
        }

        public override void Play()
        {
            JointMotor2D jm = hj.motor;
            jm.motorSpeed = 100f;
            hj.motor = jm;
        }
    }
}
