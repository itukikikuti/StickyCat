// naokinakagawa
// 2017/05/13
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Image))]
    public class Fade : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Instance = Instantiate(Resources.Load<GameObject>("Prefabs/Fade")).GetComponent<Fade>();
            DontDestroyOnLoad(Instance.gameObject);
        }

        public static Fade Instance
        {
            get;
            private set;
        }

        public float target = 0f;
        private Image image;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            image = GetComponent<Image>();
        }

        private void Update()
        {
            image.color = new Color(0f, 0f, 0f, Mathf.Lerp(image.color.a, target, 0.1f));
        }
    }
}
