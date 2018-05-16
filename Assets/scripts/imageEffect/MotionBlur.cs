// naokinakagawa
// 2017/01/12
using UnityEngine;

namespace net.windblow.stickycat
{
    public class MotionBlur : ImageEffect
    {
        [Range(0.0f, 0.9f)] public float blurAmount = 0.5f;
        private RenderTexture _accumTexture;

        protected override void OnDisable()
        {
            base.OnDisable();
            DestroyImmediate(_accumTexture);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_accumTexture == null || _accumTexture.width != source.width || _accumTexture.height != source.height)
            {
                DestroyImmediate(_accumTexture);
                _accumTexture = new RenderTexture(source.width, source.height, 0);
                _accumTexture.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, _accumTexture);
            }

            Material.SetTexture("_MainTex", _accumTexture);
            Material.SetFloat("_AccumOrig", 1f - blurAmount);

            _accumTexture.MarkRestoreExpected();

            Graphics.Blit(source, _accumTexture, Material);
            Graphics.Blit(_accumTexture, destination);
        }
    }
}
