// naokinakagawa
// 2016/12/03
using UnityEngine;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickJoint : MonoBehaviour
    {
        public Rigidbody2D connectedBody = null;
        public Vector2 connectedAnchor;
        private Rigidbody2D _rigidbody = null;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (connectedBody == null)
            {
                _rigidbody.gravityScale = 1f;
                return;
            }

            _rigidbody.gravityScale = 0f;
            Vector2 target = connectedBody.position + connectedAnchor;
            #if DEBUG
            target = Vector2.Lerp(_rigidbody.position, target, Time.timeScale);
            #endif
            _rigidbody.MovePosition(Vector2.Lerp(_rigidbody.position, target, 0.4f));
        }

        #if DEBUG && LINE
        private void OnRenderObject()
        {
            if (connectedBody == null)
                return;

            Utility.Joint(Color.blue, transform.position, connectedBody.position + connectedAnchor, 0.1f);
        }
        #endif
    }
}
