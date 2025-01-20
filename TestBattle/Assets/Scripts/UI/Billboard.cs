using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    void LateUpdate()
    {
        // ���������� ������ � �������� �������� �������� �� ������
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
