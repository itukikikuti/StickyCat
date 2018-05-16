// naokinakagawa
// 2016/12/29
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Diagnostics;

namespace net.windblow.stickycat
{
    public static class Utility
    {
        public static void SetTimeScale(float ts)
        {
            Time.timeScale = ts;
            Time.fixedDeltaTime = ts / 60f;
        }

        #if DEBUG && LINE
        private static Matrix4x4 _matrix;
        private static Material _lineMaterial = null;

        [RuntimeInitializeOnLoadMethod]
        private static void InitLine()
        {
            _matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            _lineMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        public static void Line(Color color, Vector2 begin, Vector2 end)
        {
            GL.PushMatrix();
            GL.MultMatrix(_matrix);
            _lineMaterial.color = color;
            _lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(begin);
            GL.Vertex(end);
            GL.End();
            GL.PopMatrix();
        }

        public static void Line(Color color, Vector2 begin, Vector2 direction, float distance)
        {
            Line(color, begin, begin + direction * distance);
        }

        public static void Quad(Color color, Vector2 begin, Vector2 end, float radius)
        {
            Vector2 n = (new Vector2(end.y, begin.x) - new Vector2(begin.y, end.x)).normalized * radius;
            Line(color, begin + n, end + n);
            Line(color, begin - n, end - n);
            Line(color, begin + n, begin - n);
            Line(color, end + n, end - n);
        }

        public static void Quad(Color color, Vector2 begin, Vector2 direction, float distance, float radius)
        {
            Quad(color, begin, begin + direction * distance, radius);
        }

        public static void Circle(Color color, Vector2 center, float radius, int stroke = 20)
        {
            float angle = Mathf.PI * 2f / stroke;
            for (int i = 0; i < stroke; i++)
            {
                float a = angle * i;
                Vector2 direction1 = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                a = angle * (i + 1);
                Vector2 direction2 = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                Line(color, center + direction1 * radius, center + direction2 * radius);
            }
        }

        public static void Capsule(Color color, Vector2 begin, Vector2 end, float radius)
        {
            Circle(color, begin, radius);
            Circle(color, end, radius);
            Quad(color, begin, end, radius);
        }

        public static void Capsule(Color color, Vector2 begin, Vector2 direction, float distance, float radius)
        {
            Capsule(color, begin, begin + direction * distance, radius);
        }

        public static void Joint(Color color, Vector2 begin, Vector2 end, float radius = 0.1f)
        {
            Circle(color, end, radius);
            Line(color, begin, end);
        }
        #endif

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitGrid()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (EditorApplication.isPlaying)
                return;
            foreach (GameObject go in Selection.gameObjects)
            {
                if (go.layer == LayerMask.NameToLayer("UI"))
                    continue;
                go.transform.localPosition = Snap(go.transform.localPosition, go.transform.localScale.x);
                go.transform.localScale = Snap(go.transform.localScale, 1f);
            }
        }

        private static Vector3 Snap(Vector3 input, float scale)
        {
            input *= 16f / scale;
            input = new Vector3(Mathf.Floor(input.x), Mathf.Floor(input.y), Mathf.Floor(input.z));
            input /= 16f / scale;
            return input;
        }
        #endif
    }
}
