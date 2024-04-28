using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _distance;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Interact()
    {
        Vector3 mousePosition = Input.mousePosition; 
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, _distance, _layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.transform.TryGetComponent(out Cube cube))
            {
                cube.TryToSplit();
            }
        }
    }
}