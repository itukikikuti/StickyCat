// naokinakagawa
// 2017/07/31
using UnityEditor;
using UnityEngine;
using System;
using System.IO;

namespace net.windblow.stickycat
{
    public class CreateScreenShotWindow : ScriptableWizard
    {
        private int index = 0;
        private RenderTexture iPhone = null;
        private RenderTexture iPad = null;

        [MenuItem("Window/Create ScreenShot")]
        public static void Open()
        {
            DisplayWizard<CreateScreenShotWindow>("Create ScreenShot");
        }

        protected override bool DrawWizardGUI()
        {
            index = EditorGUILayout.IntField("index", index);
            iPhone = (RenderTexture)EditorGUILayout.ObjectField("iPhone", iPhone, typeof(RenderTexture), false);
            iPad = (RenderTexture)EditorGUILayout.ObjectField("iPad", iPad, typeof(RenderTexture), false);
            return base.DrawWizardGUI();
        }

        private void OnWizardCreate()
        {
            CreateIcon(iPhone, "iPhone", index);
            CreateIcon(iPad, "iPad", index);
        }

        private static void CreateIcon(RenderTexture source, string fileName, int index)
        {
            if (source == null)
            {
                return;
            }

            RenderTexture.active = source;
            Texture2D texture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, false, false);
            texture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            texture.Apply();
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color c = texture.GetPixel(x, y);
                    c.a = 1f;
                    texture.SetPixel(x, y, c);
                }
            }
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();
            RenderTexture.active = null;

            using (FileStream fs = new FileStream(string.Format("{0}/editor/screenshot/{1}ScreenShot{2}.png", Application.dataPath, fileName, index), FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
