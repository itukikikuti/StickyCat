// naokinakagawa
// 2017/05/21
using UnityEngine;

namespace net.windblow.stickycat
{
    [CreateAssetMenu()]
    public class Initialize : ScriptableObject
    {
		public float timeScale = 1f;

        void OnEnable()
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                return;
            #endif
            #if UNITY_ANDROID
            Screen.fullScreen = false;
            #endif
            #if UNITY_STANDALONE
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            #endif
            Utility.SetTimeScale(timeScale);
        }
    }
}
