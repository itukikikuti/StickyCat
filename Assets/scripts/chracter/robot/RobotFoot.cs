// naokinakagawa
// 2016/04/19
using UnityEngine;

namespace net.windblow.stickycat
{
    public class RobotFoot : MonoBehaviour
    {
        [SerializeField] public SpringJoint2D joint;
        [SerializeField] private FixedJoint2D toLevel;
        [SerializeField] private Collider2D[] ignore;
        [SerializeField] private AudioSource sound;
        public bool isRelease = false;

        public bool IsJointed()
        {
            if (!toLevel.enabled)
                return false;
            if (toLevel.connectedBody == null)
                return false;

            return true;
        }

        private void Awake()
        {
            isRelease = true;
            toLevel.enabled = false;
        }

        public void Move(Vector2 position)
        {
            joint.connectedAnchor = position;
        }

        public void Add(Vector2 position)
        {
            joint.connectedAnchor += position;
        }

        public void Release()
        {
            isRelease = true;
            toLevel.enabled = false;
            toLevel.connectedBody = null;
        }

        private void OnCollisionEnter2D(Collision2D c)
        {
            if (isRelease)
                return;
            if (c.rigidbody == null)
                return;
            if (c.contacts[0].normal.y < -0.5f)
                return;
            for (int i = 0; i < ignore.Length; i++)
            {
                if (c.collider == ignore[i])
                    return;
            }

            toLevel.enabled = true;
            toLevel.connectedBody = c.rigidbody;
            sound.pitch = Random.Range(0.9f, 1.1f);
            sound.volume = c.relativeVelocity.sqrMagnitude / 20f;
            sound.Play();
        }
    }
}
