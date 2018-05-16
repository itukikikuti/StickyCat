// naokinakagawa
// 2017/05/13
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.windblow.stickycat
{
	public class Goal : Story
    {
        [SerializeField] private AudioClip sound;

        public override IEnumerator Play(Player p)
        {
            GetComponent<AudioSource>().PlayOneShot(sound);
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            main.startSizeMultiplier = 2f;
            main.startSpeedMultiplier = 5f;

			if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                yield return ChangeScene(SceneManager.GetActiveScene().buildIndex + 1, p);
			else
                yield return ChangeScene(0, p);
        }

        public static IEnumerator ChangeScene(int sceneIndex, Player p)
        {
            p.enabled = false;
            yield return new WaitForSeconds(1f);
            Fade.Instance.target = 1f;
            yield return new WaitForSeconds(1f);

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneIndex);
            sceneLoad.allowSceneActivation = false;
            yield return new WaitWhile(() => sceneLoad.progress < 0.9f);

            Fade.Instance.target = 0f;
            sceneLoad.allowSceneActivation = true;
        }
    }
}
