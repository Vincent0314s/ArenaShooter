using UnityEngine;

public class PathDetection : MonoBehaviour
{
    [SerializeField]
    private Unit_Ally unit_Ally;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(ConstStringCollection.TAG_PATH_LEFT)) {
            unit_Ally.path = WalkablePath.Left;
            gameObject.SetActive(false);
        } else if (other.tag.Equals(ConstStringCollection.TAG_PATH_RIGHT)) {
            unit_Ally.path = WalkablePath.Right;
            gameObject.SetActive(false);
        }
    }
}
