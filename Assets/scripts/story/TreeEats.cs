// naokinakagawa
// 2017/07/15
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace net.windblow.stickycat
{
    public class TreeEats : Story
    {
        [SerializeField] private Robot robot;
        [SerializeField] private ParticleSystem eatParticle;
        [SerializeField] private AudioSource eatAudio;
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private ParticleSystem signal;
        [SerializeField] private Squirrel[] squirrels;
        [SerializeField] private Material material;

        public override IEnumerator Play(Player p)
        {
            yield return robot.Wake();
            yield return robot.Turn();
            yield return robot.Wake();
            IEnumerator chewing = Chewing();
            StartCoroutine(chewing);
            yield return new WaitUntil(() => robot.transform.position.x - 3f < p.transform.position.x);
            StopCoroutine(chewing);
            yield return new WaitForSeconds(1f);
            yield return robot.Move(36f);
            signal.Play();
            yield return robot.Move(40f);
            LensFlare[] robotEyes = robot.GetComponentsInChildren<LensFlare>();
            foreach (LensFlare robotEye in robotEyes)
            {
                robotEye.enabled = true;
            }
            yield return robot.Turn();
            yield return robot.Wake();
            for (int i = 0; i < squirrels.Length; i++)
            {
                eatAudio.pitch = Random.Range(0.9f, 1.1f);
                eatAudio.PlayOneShot(shootSound);
                CreateJoint(squirrels[i].GetComponent<Rigidbody2D>());
                yield return new WaitForSeconds(0.3f);
            }
            yield return robot.Turn();
            robot.speed = 10f;
            yield return robot.Move(p.transform.position.x + 30f);
            Destroy(robot.gameObject);
            for (int i = 0; i < squirrels.Length; i++)
            {
                Destroy(squirrels[i].gameObject);
            }
        }

        private void CreateJoint(Rigidbody2D squirrel)
        {
            GameObject go = new GameObject("Joint");
            go.transform.SetParent(robot.transform);
            go.transform.localPosition = new Vector3(0f, -1f, 0f);
            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            SpringJoint2D joint = go.AddComponent<SpringJoint2D>();
            joint.autoConfigureDistance = false;
            joint.distance = 3f;
            joint.frequency = 0.5f;
            joint.connectedBody = squirrel;
            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.widthMultiplier = 0.1f;
            lr.startColor = lr.endColor = new Color(0.4f, 0.125f, 0f);
            lr.sharedMaterial = material;
            JointLine jl = go.AddComponent<JointLine>();
            jl.sprite = Resources.Load<Sprite>("textures/character/PixelCircle");
        }

        private IEnumerator Chewing()
        {
            Transform mouth = robot.transform.GetChild(0);
            Vector3 position = mouth.localPosition;
            eatAudio.transform.position = mouth.position;
            int soundCount = 1;
            float t = 0f;
            while (true)
            {
                t += Time.deltaTime * 10f;
                mouth.localPosition = new Vector3(Mathf.Cos(t) * 0.1f, Mathf.Sin(t) * 0.1f, 0f) + position;
                if (t / (Mathf.PI * 2f) > soundCount)
                {
                    eatAudio.pitch = Random.Range(0.9f, 1.1f);
                    eatAudio.PlayOneShot(eatAudio.clip);
                    eatParticle.Emit(10);
                    soundCount++;
                }
                yield return null;
            }
        }
    }
}
