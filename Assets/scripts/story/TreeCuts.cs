// naokinakagawa
// 2017/07/1
using System.Collections;
using UnityEngine;

namespace net.windblow.stickycat
{
	public class TreeCuts : Story
	{
		[SerializeField] private FixedJoint2D treeJoint;
        [SerializeField] private Rigidbody2D treeBody;
		[SerializeField] private Rigidbody2D rock;
		[SerializeField] private Robot robot;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private AudioSource treeAudio;
        [SerializeField] private Squirrel[] squirrels;

        public override IEnumerator Play(Player p)
		{
			yield return robot.Wake();
			yield return robot.Move(22f);
			yield return robot.Wake();
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < 60; i++)
			{
                robot.rb.AddForce(new Vector2(i * Random.Range(2900f, 3100f), 0f), ForceMode2D.Force);
				yield return null;
			}
            yield return new WaitForSeconds(0.1f);
            particle.Play();
            treeAudio.Play();
            treeBody.AddForce(new Vector2(treeBody.mass * -8f, 0f), ForceMode2D.Impulse);
			treeJoint.enabled = false;
			yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < squirrels.Length; i++)
            {
                squirrels[i].released = true;
                squirrels[i].Stan();
                squirrels[i].gameObject.layer = LayerMask.NameToLayer("Squirrel");

            }
            p.Release();
            yield return new WaitForSeconds(1.5f);
			p.Stan();
			p.enabled = false;
            StartCoroutine(WakeUp(p));
			yield return robot.Move(15f);
			yield return robot.Wake();
			yield return robot.Turn();
			yield return robot.Wake();
            yield return new WaitForSeconds(1f);
			treeJoint.connectedBody = robot.rb;
			treeJoint.frequency = 0.01f;
            treeBody.mass = 100f;
			treeJoint.enabled = true;
            yield return robot.Move(p.transform.position.x + 20f);
			Destroy(robot.gameObject);
			Destroy(treeBody.gameObject);
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < 60; i++)
            {
                p.wink.t = Mathf.Lerp(p.wink.t, 1f, 0.1f);
                yield return null;
            }
            p.enabled = true;
		}

        private IEnumerator WakeUp(Player p)
        {
            for (int i = 0; i < squirrels.Length; i++)
            {
                squirrels[i].wink.t = 1f;
                squirrels[i].released = false;
                squirrels[i].target = p.gameObject;
                squirrels[i].Stick(rock, squirrels[i].transform.position);
                squirrels[i].rigidbody.WakeUp();
                yield return new WaitForSeconds(Random.Range(0f, 1f));
            }
        }
	}
}
