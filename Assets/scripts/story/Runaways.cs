// naokinakagawa
// 2017/08/03
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Runaways : Story
    {
        [SerializeField] private Robot robot;
        private IEnumerator move;

        public override IEnumerator Play(Player p)
        {
            yield return robot.Wake();
            LensFlare[] robotEyes = robot.GetComponentsInChildren<LensFlare>();
            foreach (LensFlare robotEye in robotEyes)
            {
                robotEye.enabled = true;
            }
            move = robot.Move(100f);
            yield return move;
        }

        private void OnDestroy()
        {
            StopCoroutine(move);
        }
    }
}
