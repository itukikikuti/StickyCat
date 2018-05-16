// naokinakagawa
// 2016/10/18
using UnityEngine;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpringJoint2D))]
    [RequireComponent(typeof(AudioSource))]
    public class ExpandCharacter : MonoBehaviour
    {
        protected Transform body;
        public new Rigidbody2D rigidbody{ get; protected set; }
        public new CircleCollider2D collider{ get; protected set; }
        public SpringJoint2D joint{ get; protected set; }
        public Jelly jelly{ get; protected set; }
        public Wink wink{ get; protected set; }
        protected AudioSource _audio = null;
        protected Vector3 _axis;
        protected Vector3 _target;
        protected bool _isStick = false;
        protected Vector3 _direction = Vector3.right;
        protected float _distance = 0f;
        [SerializeField] private LayerMask layerMask;

        protected virtual void Awake()
        {
            body = transform.GetChild(0);
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();
            joint = GetComponent<SpringJoint2D>();
            jelly = body.GetComponent<Jelly>();
            wink = body.GetComponent<Wink>();
            _audio = GetComponent<AudioSource>();
        }

        protected void Extend()
        {
            float scale = transform.localScale.x;

            transform.localScale = new Vector3(1f, _direction.x <= 0f ? -1f : 1f, 1f);
            body.position = transform.position + (_direction * scale * _distance) / 2f;
            body.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
            jelly.original = new Vector3(_distance + 1f, Mathf.Lerp(1f, 0.5f, _distance / 4f), 1f);

            RaycastHit2D hit = Physics2D.CircleCast(transform.position, collider.radius * 0.9f, _direction, scale * _distance, layerMask.value);
            if (hit.transform != null)
            {
                if (hit.rigidbody != null)
                {
                    Stick(hit.rigidbody, hit.centroid);
                }
                else
                {
                    Shrink();
                }
            }
        }

        public void Stan()
        {
            wink.t = 0.1f;
            Release();
            Shrink();
        }

        public void Release()
        {
            joint.enabled = false;
			rigidbody.angularDrag = 0.05f;
        }

        protected void Shrink()
        {
            _isStick = true;
            _distance = 0f;
            body.localPosition = Vector3.zero;
			jelly.AddForce(new Vector3(-(body.localScale.x - 1f), body.localScale.x - 1f) * 0.5f);
            jelly.original = Vector3.one;
            body.localScale = Vector3.one;
        }

        protected void PlaySound()
        {
            _audio.pitch = Random.Range(0.9f, 1.1f);
            _audio.PlayOneShot(_audio.clip);
        }

        public void Stick(Rigidbody2D connectedBody, Vector3 hit)
        {
            PlaySound();
            joint.enabled = true;
            rigidbody.velocity = Vector2.zero;
            Vector3 position = transform.position;
            transform.position = hit;
            joint.autoConfigureConnectedAnchor = true;
            joint.connectedBody = connectedBody;
            joint.autoConfigureConnectedAnchor = false;
            joint.frequency = 50f / Time.timeScale;
            transform.position = position;
			rigidbody.angularDrag = 10f;
            Shrink();
        }

        #if DEBUG && LINE
        private void OnRenderObject()
        {
            float scale = transform.localScale.x;
            Utility.Capsule(Color.red, transform.position, _direction, scale * _distance, collider.radius * 0.9f);
        }
        #endif
    }
}
