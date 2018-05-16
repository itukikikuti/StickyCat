// naokinakagawa
// 2016/12/18
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Fog : ImageEffect
    {
        [SerializeField] private float scale = 1.5f;
        [SerializeField] private float offset = 0.2f;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Material.SetColor("_Color", Camera.current.backgroundColor);
            Material.SetFloat("_Scale", scale);
            Material.SetFloat("_Offset", offset);
            Graphics.Blit(source, destination, Material);
        }
    }
}
