// naokinakagawa
// 2017/05/26
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.windblow.stickycat
{
    public class Dead : Story
    {
        public override IEnumerator Play(Player p)
        {
            p.enabled = false;
            p.Dead();
            p.rigidbody.velocity *= 2f;
            yield return new WaitForSeconds(1f);
            Fade.Instance.target = 1f;
            yield return new WaitForSeconds(1f);

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            sceneLoad.allowSceneActivation = false;
            yield return new WaitWhile(() => sceneLoad.progress < 0.9f);

            Fade.Instance.target = 0f;
            sceneLoad.allowSceneActivation = true;
        }
    }
}
