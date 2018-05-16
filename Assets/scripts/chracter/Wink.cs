// naokinakagawa
// 2016/12/22
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Wink : MonoBehaviour
    {
        [SerializeField] private AnimationCurve scale = null;
        [SerializeField] private Transform[] eyes = null;
        private float _random = 0f;
        [HideInInspector] public float t = 1f;

        private void Awake()
        {
            _random = Random.Range(0f, 10f);
        }

        private void Update()
        {
            float s = scale.Evaluate(Time.time + _random) * t;
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].localScale = new Vector3(eyes[i].localScale.x, s, eyes[i].localScale.z);
            }
        }
    }
}
