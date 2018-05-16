// naokinakagawa
// 2017/01/03
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace net.windblow.stickycat
{
    public class BrandLogo : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve = null;
        [SerializeField] private AnimationCurve zoomIn = null;
        [SerializeField] private string scene;
        [SerializeField] private float wait;
        private float t = 0f;

        private IEnumerator Start()
        {
            t = 0;
            while (t < curve.keys[curve.length - 1].time + wait)
            {
                t += Time.deltaTime;
                transform.eulerAngles = new Vector3(0f, curve.Evaluate(t), 0f);
                yield return null;
            }
            t = 0;
            while (t < zoomIn.keys[zoomIn.length - 1].time + wait)
            {
                t += Time.deltaTime;
                Camera.main.fieldOfView = zoomIn.Evaluate(t);
                yield return null;
            }

            SceneManager.LoadSceneAsync(scene);
        }
    }
}
