// naokinakagawa
// 2017/08/05
using System.Collections;
using UnityEngine;
using TMPro;

namespace net.windblow.stickycat
{
    public class Ending : Story
    {
        [SerializeField] private TextMeshProUGUI text;

        public override IEnumerator Play(Player p)
        {
            Color c = text.color;
            c.a = 0f;
            text.color = c;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 60; i++)
            {
                c.a = Mathf.Lerp(c.a, 1f, 0.1f);
                text.color = c;
                yield return null;
            }
            yield return new WaitForSeconds(3f);
            yield return Goal.ChangeScene(0, p);
        }
    }
}
