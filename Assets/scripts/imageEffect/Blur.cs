// naokinakagawa
// 2017/01/12
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Blur : ImageEffect
    {
        [Range(0, 10)] public int iterate = 3;
        [Range(0f, 1f)] public float spred = 0.5f;

        private void DownSample4x(RenderTexture source, RenderTexture dest, float off)
        {
            Graphics.BlitMultiTap(source, dest, Material,
                new Vector2(-off, -off),
                new Vector2(-off, off),
                new Vector2(off, off),
                new Vector2(off, -off)
            );
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            int w = source.width / 4;
            int h = source.height / 4;
            RenderTexture buffer = RenderTexture.GetTemporary(w, h, 0);
            Graphics.Blit(source, buffer);

            for (int i = 0; i < iterate; i++)
            {
                RenderTexture buffer2 = RenderTexture.GetTemporary(w, h, 0);
                DownSample4x(buffer, buffer2, 0.5f + i * spred);
                RenderTexture.ReleaseTemporary(buffer);
                buffer = buffer2;
            }
            Graphics.Blit(buffer, destination);

            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}
