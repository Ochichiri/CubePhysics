using UnityEngine;

[RequireComponent(typeof(PlayerInteraction))]
public class PlayerInput : MonoBehaviour
{
    private PlayerInteraction _playerInteraction;

    private void Awake()
    {
        _playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _playerInteraction.Interact();
        }
    }
}