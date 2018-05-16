// naokinakagawa
// 2017/02/18
using UnityEngine;

namespace net.windblow.stickycat
{
    [RequireComponent(typeof(Jelly))]
    public class Breath : MonoBehaviour
    {
        [SerializeField] private AnimationCurve scale = null;
        private Jelly _jelly;
        private float _random = 0f;
        private float _scale;

        private void Awake()
        {
            _jelly = GetComponent<Jelly>();
            _random = Random.Range(0f, 10f);
        }

        private void Update()
        {
            _scale = scale.Evaluate(Time.time + _random);
            //if (transform.eulerAngles.z > 90f && transform.eulerAngles.z < 270f)
                //_scale = -_scale;
            _jelly.original = new Vector3(_jelly.original.x, _scale, _jelly.original.z);
        }
    }
}
