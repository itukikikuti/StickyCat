// naokinakagawa
// 2017/01/12
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Shadow : ImageEffect
    {
        [SerializeField] private Shader blurShader;
        [SerializeField, Range(0, 10)] private int iterate = 3;
        [SerializeField, Range(0f, 1f)] private float spred = 0.5f;
        [SerializeField] private float offset;
        [SerializeField] private Color color;
        [SerializeField] private Color backgroundColor;

        private Material _blurMaterial;

        protected Material BlurMaterial
        {
            get
            {
                if (_blurMaterial == null)
                {
                    _blurMaterial = new Material(blurShader);
                    _blurMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return _blurMaterial;
            }
        }

        protected override void OnDisable()
        {
            if (_blurMaterial)
            {
                DestroyImmediate(_blurMaterial);
            }
        }

        private void DownSample4x(RenderTexture source, RenderTexture dest, float offset, float scale)
        {
            BlurMaterial.SetFloat("_Offset", offset);
            Graphics.BlitMultiTap(source, dest, BlurMaterial,
                new Vector2(-scale, -scale),
                new Vector2(-scale, scale),
                new Vector2(scale, scale),
                new Vector2(scale, -scale)
            );
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            int w = source.width / 4;
            int h = source.height / 4;
            RenderTexture buffer = RenderTexture.GetTemporary(w, h, 0);
            DownSample4x(source, buffer, offset, 1f);

            for (int i = 0; i < iterate; i++)
            {
                RenderTexture buffer2 = RenderTexture.GetTemporary(w, h, 0);
                DownSample4x(buffer, buffer2, offset, 0.5f + i * spred);
                RenderTexture.ReleaseTemporary(buffer);
                buffer = buffer2;
            }
            Material.SetTexture("_ShadowTex", buffer);
            Material.SetColor("_Color", color);
            Material.SetColor("_BackgroundColor", backgroundColor);
            Graphics.Blit(source, destination, Material);
            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}
