// naokinakagawa
// 2016/12/30
using UnityEngine;
using System.Collections;

namespace net.windblow.stickycat
{
    public class Gecko : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2d = null;
        [SerializeField] private CircleCollider2D collder = null;
        [SerializeField] private Transform frontLeftLeg = null;
        [SerializeField] private Transform frontRightLeg = null;
        [SerializeField] private Transform backLeftLeg = null;
        [SerializeField] private Transform backRightLeg = null;
        [SerializeField] private SpriteRenderer tail = null;
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private float anim = 1f;
        [SerializeField] private Vector2 waitRange;
        private float t = 0f;

        private IEnumerator Start()
        {
            transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
            while (true)
            {
                t = 0f;
                float walk = Random.Range(waitRange.x, waitRange.y);
                while (t < walk)
                {
                    t += Time.deltaTime;
                    Walk();
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(waitRange.x, waitRange.y));
            }
        }

        private void Walk()
        {
            
            rigidbody2d.MovePosition(rigidbody2d.position + (Vector2)transform.up * speed * Time.timeScale);
            frontLeftLeg.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * anim) * 30f);
            frontRightLeg.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * anim) * 30f);
            backLeftLeg.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * anim) * -30f);
            backRightLeg.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * anim) * -30f);
            tail.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * anim) * 20f);
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.05f, transform.up, collder.radius);
            if (hit.rigidbody != null)
                transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z - 30f);
        }

        #if DEBUG && LINE
        private void OnRenderObject()
        {
            Utility.Capsule(Color.red, transform.position, transform.up, collder.radius, 0.05f);
        }
        #endif
    }
}
