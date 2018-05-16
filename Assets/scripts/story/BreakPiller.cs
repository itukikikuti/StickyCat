// naokinakagawa
// 2017/08/05
using UnityEngine;
using System.Collections;

namespace net.windblow.stickycat
{
    public class BreakPiller : Story
    {
        [SerializeField] private Joint2D[] pillerJoint;
        [SerializeField] private Robot robot;
        [SerializeField] private GameObject runaways;
        [SerializeField] private GameObject signal;
        [SerializeField] private AudioSource audio;

        public override IEnumerator Play(Player p)
        {
            Destroy(runaways);
            audio.Play();
            Destroy(signal);
            foreach (Joint2D j in pillerJoint)
            {
                j.breakTorque = 1f;
                yield return null;
            }
            yield return robot.Move(100f);
            yield return robot.Wake();
            yield return new WaitForSeconds(1f);
            LensFlare[] robotEyes = robot.GetComponentsInChildren<LensFlare>();
            foreach (LensFlare robotEye in robotEyes)
            {
                robotEye.enabled = false;
            }
            yield return new WaitForSeconds(1f);
            yield return Goal.ChangeScene(1, p);
        }
    }
}
