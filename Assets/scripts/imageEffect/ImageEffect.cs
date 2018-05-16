// naokinakagawa
// 2017/01/12
using UnityEngine;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class ImageEffect : MonoBehaviour
    {
        [SerializeField] private Shader shader;
        private Material _material;

        protected Material Material
        {
            get
            {
                if (_material == null)
                {
                    _material = new Material(shader);
                    _material.hideFlags = HideFlags.HideAndDontSave;
                }
                return _material;
            }
        }

        protected virtual void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                return;
            }

            if (!shader || !shader.isSupported)
                enabled = false;
        }

        protected virtual void OnDisable()
        {
            if (_material)
            {
                DestroyImmediate(_material);
            }
        }
    }
}
