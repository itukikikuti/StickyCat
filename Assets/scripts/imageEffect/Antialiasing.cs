using System;
using UnityEngine;

namespace net.windblow.stickycat
{
    public class Antialiasing : ImageEffect
    {
        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, Material);
        }
    }
}
