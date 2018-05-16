// naokinakagawa
// 2017/07/23
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Robot2 : MonoBehaviour
    {
        [SerializeField] public float speed = 1f;
        [HideInInspector] public Rigidbody2D rb;
        private float speedMultiplier = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            rb.angularVelocity = speed * speedMultiplier;
        }

        private void OnCollisionStay2D(Collision2D c)
        {
            for (int i = 0; i < c.contacts.Length; i++)
            {
                if (c.contacts[i].normal.x < -0.8f)
                {
                    speedMultiplier = 1f;
                    return;
                }
                if (c.contacts[i].normal.x > 0.8f)
                {
                    speedMultiplier = -1f;
                    return;
                }
            }
        }
    }
}
