// naokinakagawa
// 2017/07/16
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Squirrel : ExpandCharacter
    {
        [SerializeField] public GameObject target;
        public bool released = false;

        protected virtual float GetSpeed()
        {
            return 2f;
        }

        protected virtual float GetJumpPower()
        {
            return Random.Range(3f, 8f);
        }

        private void Update()
        {
            if (target != null)
            {
                Release();
                rigidbody.AddForce(new Vector2((target.transform.position - transform.position).normalized.x * GetSpeed() * 0.1f, 0f), ForceMode2D.Force);
            }
        }

        public void OnCollisionEnter2D(Collision2D c)
        {
            if (released)
                return;
            if (c.rigidbody == null)
                return;
            if (target != null)
                return;
            
            Stick(c.rigidbody, transform.position);
        }

        private void OnCollisionStay2D(Collision2D c)
        {
            if (target == null)
                return;
            for (int i = 0; i < c.contacts.Length; i++)
            {
                if (c.contacts[i].normal.y < 0.1f)
                {
                    return;
                }
            }
            
            PlaySound();
            Release();
            Vector2 force = new Vector2((target.transform.position - transform.position).normalized.x * GetSpeed(), GetJumpPower());
            rigidbody.velocity = force;
        }
    }
}
