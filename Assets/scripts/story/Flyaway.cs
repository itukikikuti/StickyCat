// naokinakagawa
// 2017/07/24
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Flyaway : Story
    {
        [SerializeField] private GameObject glass;
        [SerializeField] private ParticleSystem glassEffect;
        [SerializeField] private AudioSource glassAudio;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public override IEnumerator Play(Player p)
        {
            rb.AddForce(new Vector2(-10f, 0f), ForceMode2D.Impulse);
            yield return null;
            p.rigidbody.velocity = new Vector2(30f, 3f);
            p.transform.position = new Vector3(p.transform.position.x, transform.position.y);
            p.Release();
            yield return new WaitForSeconds(0.1f);
            Destroy(glass);
            glassEffect.Play();
            glassAudio.Play();
        }
    }
}
