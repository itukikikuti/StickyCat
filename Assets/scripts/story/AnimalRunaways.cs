// naokinakagawa
// 2017/07/5
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class AnimalRunaways : Story
    {
        [SerializeField] private Squirrel[] animals;
        [SerializeField] private GameObject target;

        public override IEnumerator Play(Player p)
        {
            for (int i = 0; i < animals.Length; i++)
            {
                animals[i].target = target;
            }
            yield break;
        }
    }
}
