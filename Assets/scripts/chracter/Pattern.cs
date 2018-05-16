// naokinakagawa
// 2017/03/13
using UnityEngine;

namespace net.windblow.stickycat
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pattern : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private Texture2D texture;
        private Material _material;
        private SpriteRenderer _sprite;

        protected Material Material
        {
            get
            {
                if (_material == null)
                {
                    _material = new Material(Shader.Find("Sprites/CharacterBody"));
                    _material.hideFlags = HideFlags.HideAndDontSave;
                }
                return _material;
            }
        }

        private SpriteRenderer Sprite
        {
            get
            {
                if (_sprite == null)
                {
                    _sprite = GetComponent<SpriteRenderer>();
                }

                return _sprite;
            }
        }

        private void Awake()
        {
            Sprite.material = Material;   
            if (texture != null)
            {
                Material.SetColor("_Color", color);
                Material.SetTexture("_Decal", texture);
            }
        }

        #if UNITY_EDITOR
        private void Update()
        {
            Awake();
        }
        #endif
    }
}
