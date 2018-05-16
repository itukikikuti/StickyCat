// naokinakagawa
// 2017/07/2
using UnityEngine;

namespace net.windblow.stickycat
{
    public class KeyTrigger : MonoBehaviour
    {
        [SerializeField] private Gimmick[] gimmicks;
        private Joint2D j;
        private bool isPlaying = false;

        private void Awake()
        {
            j = GetComponent<Joint2D>();
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            if (c.tag != "Key")
                return;
            j.enabled = false;
            isPlaying = true;
        }

        private void Update()
        {
            if (isPlaying)
                foreach (Gimmick gimmick in gimmicks)
                    gimmick.Play();
        }
    }
}
