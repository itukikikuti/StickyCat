// naokinakagawa
// 2017/04/19
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Robot : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private RobotFoot leftFoot;
        [SerializeField] private RobotFoot rightFoot;
        [SerializeField] public float speed = 1f;
        [SerializeField] private Vector2 offset = new Vector2(0f, 0f);
        [SerializeField] private Vector2 radius = new Vector2(1f, 0.5f);
        [SerializeField] private LineRenderer leg;
        [SerializeField] private ParticleSystem steam;
        [HideInInspector] public Rigidbody2D rb;
        private float t = Mathf.PI;

        private void Awake()
        {
            leg.material.mainTexture = sprite.texture;
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (leftFoot.joint != null)
                leg.SetPosition(0, leftFoot.transform.localPosition);
            else
                leg.SetPosition(0, Vector3.Lerp(leg.GetPosition(0), leg.GetPosition(1), 0.5f));

            if (rightFoot.joint != null)
                leg.SetPosition(2, rightFoot.transform.localPosition);
            else
                leg.SetPosition(2, Vector3.Lerp(leg.GetPosition(2), leg.GetPosition(1), 0.5f));
        }

        public IEnumerator Wake()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            leftFoot.isRelease = false;
            rightFoot.isRelease = false;
            steam.Play();
            for (int i = 0; i < 60; i++)
            {
                if (leftFoot.IsJointed() && rightFoot.IsJointed())
                    break;

                if (leftFoot.IsJointed())
                {
                    rightFoot.Move(Vector2.Lerp(rightFoot.joint.connectedAnchor, offset + new Vector2(radius.x, 0f), 0.05f));
                    leftFoot.Move(Vector2.Lerp(leftFoot.joint.connectedAnchor, offset - new Vector2(radius.x, 0f) + new Vector2(0f, radius.y), 0.05f));
                }
                if (rightFoot.IsJointed())
                {
                    leftFoot.Move(Vector2.Lerp(leftFoot.joint.connectedAnchor, offset - new Vector2(radius.x, 0f), 0.05f));
                    rightFoot.Move(Vector2.Lerp(rightFoot.joint.connectedAnchor, offset + new Vector2(radius.x, 0f) + new Vector2(0f, radius.y), 0.05f));
                }
                
                yield return null;
            }
            steam.Stop();
            rb.velocity = Vector2.zero;
        }

        public IEnumerator Turn()
        {
            leftFoot.Release();
            rightFoot.isRelease = false;
            for (int i = 0; i < 60; i++)
            {
                leftFoot.Move(Vector2.Lerp(leftFoot.joint.connectedAnchor, offset - new Vector2(radius.x, 0f) + new Vector2(0f, radius.y), 0.05f));
                rightFoot.Move(Vector2.Lerp(rightFoot.joint.connectedAnchor, new Vector2(0f, offset.y - radius.y), 0.05f));
                yield return null;
            }
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            yield break;
        }

        public IEnumerator Move(float x)
        {
            while (true)
            {
                if (0f < transform.localScale.x && x > transform.position.x)
                    break;
                if (0f > transform.localScale.x && x < transform.position.x)
                    break;
                Move();
                yield return null;
            }
            steam.Stop();
        }

        private void Move()
        {
            t += Time.deltaTime * speed;
            t = t % (Mathf.PI * 2);
            float u = t + Mathf.PI;
            leftFoot.Move(offset + new Vector2(Mathf.Cos(t) * radius.x, Mathf.Sin(t) * radius.y));
            rightFoot.Move(offset + new Vector2(Mathf.Cos(u) * radius.x, Mathf.Sin(u) * radius.y));
            steam.Stop();
            float halfPI = Mathf.PI / 2f;
            if (0f < t && t < halfPI)
            {
                leftFoot.Release();
                steam.Play();
            }
            else
            {
                leftFoot.isRelease = false;
            }

            if (0f + Mathf.PI < t && t < halfPI + Mathf.PI)
            {
                rightFoot.Release();
                steam.Play();
            }
            else
            {
                rightFoot.isRelease = false;
            }
        }
    }
}
