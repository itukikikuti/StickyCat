// naokinakagawa
// 2017/04/22
using UnityEngine;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class CollisionSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D c)
        {
            audioSource.volume = c.relativeVelocity.magnitude * c.rigidbody.mass / 500f;
            Debug.Log(c.gameObject.name + ", " + audioSource.volume.ToString());
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
    }
}
