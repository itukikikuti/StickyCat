// naokinakagawa
// 2017/03/14
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Button))]
    public class TitleLogo : MonoBehaviour
    {
        [SerializeField] private Jelly[] jellys;
        [SerializeField] private Player player;
        [SerializeField] private GameObject canvas;
        private Button button;

        private void Awake()
        {
            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
            player.enabled = false;
            for (int i = 0; i < jellys.Length; i++)
            {
                jellys[i].transform.localScale = new Vector3(15f, 15f, 15f);
                jellys[i].original = new Vector3(1f, 1f, 1f);
                jellys[i].spring = 0.3f;
                jellys[i].repulsion = 0.5f;
                jellys[i].limit = 100f;
            }
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => jellys[0].transform.localScale.y < 2f);
            for (int i = 0; i < jellys.Length; i++)
            {
                jellys[i].spring = 0.9f;
                jellys[i].repulsion = 0.75f;
                jellys[i].limit = 0.8f;
            }
        }

        private void Update()
        {
            Vector2 cameraPosition = Vector2.Lerp(Camera.main.transform.position, player.transform.position, 0.1f);
            Camera.main.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, Camera.main.transform.position.z);
        }

        private void OnClick()
        {
            for (int i = 0; i < jellys.Length; i++)
            {
                jellys[i].original = new Vector3(2f, 0f, 1f);
                jellys[i].spring = 0.3f;
                jellys[i].repulsion = 0.5f;
            }
            player.enabled = true;
            Destroy(canvas, 0.5f);
        }
    }
}
