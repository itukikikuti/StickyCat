// naokinakagawa
// 2017/05/17
using UnityEngine;

namespace net.windblow.stickycat
{
    public class PlateTrigger : MonoBehaviour
    {
        [SerializeField] private Gimmick[] gimmicks;
        private SpriteRenderer sprite;
        private bool isPlaying = false;

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.size = new Vector2(sprite.size.x, 1f);
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            Player p = c.GetComponent<Player>();
            if (p == null)
                return;
            sprite.size = new Vector2(sprite.size.x, 0.5f);
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
