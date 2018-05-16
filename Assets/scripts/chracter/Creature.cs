// naokinakagawa
// 2017/07/24
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Creature : Squirrel
    {
        protected override float GetSpeed()
        {
            return 5f;
        }

        protected override float GetJumpPower()
        {
            return Random.Range(7f, 10f);
        }
    }
}
