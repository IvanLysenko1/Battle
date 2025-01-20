using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    void LateUpdate()
    {
        // Заставляем объект с полоской здоровья смотреть на камеру
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
