// naokinakagawa
// 2016/12/19
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using SD = System.Drawing;

namespace net.windblow.stickycat
{
    public class CreateIconWindow : ScriptableWizard
    {
        private RenderTexture _androidTexture = null;
        private RenderTexture _iOSTexture = null;
        private static readonly int[] androidSizes = { 512, 192, 144, 96, 72, 48, 36 };
        private static readonly int[] iOSSizes = { 1024, 180, 167, 152, 144, 120, 114, 87, 80, 76, 72, 58, 57, 40, 29 };

        [MenuItem("Window/Create Icon")]
        public static void Open()
        {
            DisplayWizard<CreateIconWindow>("Create Icon");
        }

        protected override bool DrawWizardGUI()
        {
            _androidTexture = (RenderTexture)EditorGUILayout.ObjectField("Android", _androidTexture, typeof(RenderTexture), false);
            _iOSTexture = (RenderTexture)EditorGUILayout.ObjectField("iOS", _iOSTexture, typeof(RenderTexture), false);
            return base.DrawWizardGUI();
        }

        private void OnWizardCreate()
        {
            CreateIcon(_androidTexture, androidSizes, "Android");
            CreateIcon(_iOSTexture, iOSSizes, "iOS");
        }

        private static void CreateIcon(RenderTexture source, int[] sizes, string fileName)
        {
            if (source == null)
            {
                return;
            }

            RenderTexture.active = source;
            Texture2D texture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, false, false);
            texture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();
            RenderTexture.active = null;

            for (int i = 0; i < sizes.Length; i++)
            {
                using (SD.Bitmap icon = new SD.Bitmap(sizes[i], sizes[i]))
                using (SD.Graphics g = SD.Graphics.FromImage(icon))
                using (MemoryStream ms = new MemoryStream(bytes))
                using (SD.Bitmap image = new SD.Bitmap(ms))
                {
                    g.InterpolationMode = SD.Drawing2D.InterpolationMode.Bilinear;
                    g.DrawImage(image, 0, 0, sizes[i], sizes[i]);
                    icon.Save(string.Format("{0}/editor/icons/{1}Icon{2}.png", Application.dataPath, fileName, sizes[i]), SD.Imaging.ImageFormat.Png);
                }
            }
        }
    }
}
