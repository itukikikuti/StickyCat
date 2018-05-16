// naokinakagawa
// 2017/05/14
using UnityEngine;

namespace net.windblow.stickycat
{
    public class JointLine : MonoBehaviour
    {
        [SerializeField] public Sprite sprite;
        private AnchoredJoint2D joint;
        private LineRenderer line;

        private void Start()
        {
            joint = GetComponent<AnchoredJoint2D>();
            line = GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.material.mainTexture = sprite.texture;
            line.sortingLayerName = "JointLine";
            line.SetPosition(0, joint.anchor);
        }

        private void Update()
        {
            if (joint == null)
                line.SetPosition(1, Vector3.Lerp(line.GetPosition(0), line.GetPosition(1), 0.5f));
            else
                line.SetPosition(1, transform.InverseTransformPoint(joint.connectedBody.transform.TransformPoint(joint.connectedAnchor)));
        }
    }
}
