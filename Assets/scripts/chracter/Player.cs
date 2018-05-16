// naokinakagawa
// 2017/02/18
using UnityEngine;
using UnityEngine.EventSystems;

namespace net.windblow.stickycat
{
    public class Player : ExpandCharacter
    {
        [SerializeField] private AnimationCurve expand;
        [SerializeField] private AudioLowPassFilter audioLowPassFilter;
        [SerializeField] private ParticleSystem blood;
        private int touchFrame = 0;

        private void Update()
        {
            if (joint.connectedBody == null)
                Release();

            if (Input.GetMouseButtonDown(0))
            {
                _isStick = false;
                _axis = Input.mousePosition;
                _distance = 0f;
                wink.t = 1f;
                touchFrame = 0;
            }
            if (Input.GetMouseButton(0))
            {
                if (!_isStick)
                {
                    touchFrame++;
                    _target = Input.mousePosition;
                    _distance = Mathf.Lerp(_distance, expand.Evaluate(Vector3.Distance(_target, _axis) / Screen.dpi) * transform.localScale.x, 0.2f);
                    if (_distance > 0f)
                    {
                        _direction = (_target - _axis).normalized;
                        Extend();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!_isStick && _distance < 0.1f && touchFrame <= 10)
                {
                    Release();
                }
                Shrink();
            }
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                audioLowPassFilter.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                audioLowPassFilter.enabled = false;
            }
        }

        public void Dead()
        {
            Stan();
            blood.Play();
        }
    }
}
