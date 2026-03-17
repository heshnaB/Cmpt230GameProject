using UnityEngine;

public class CatInteractionController : MonoBehaviour
{
    public CatGameManager gameManager;
    public Camera interactionCamera;
    public LayerMask interactionLayers = ~0;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<CatGameManager>();
        }

        if (interactionCamera == null)
        {
            interactionCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryCenterInteract();
        }

        if (Cursor.visible && Input.GetMouseButtonDown(0))
        {
            TryClickInteract();
        }
    }

    void TryCenterInteract()
    {
        if (interactionCamera == null)
        {
            return;
        }

        Ray ray = new Ray(interactionCamera.transform.position, interactionCamera.transform.forward);
        TryRaycastInteract(ray);
    }

    void TryClickInteract()
    {
        if (interactionCamera == null)
        {
            return;
        }

        Ray ray = interactionCamera.ScreenPointToRay(Input.mousePosition);
        TryRaycastInteract(ray);
    }

    void TryRaycastInteract(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactionLayers))
        {
            CatInteractable interactable = hit.collider.GetComponentInParent<CatInteractable>();
            if (interactable != null)
            {
                interactable.Interact(gameManager);
            }
        }
    }
}