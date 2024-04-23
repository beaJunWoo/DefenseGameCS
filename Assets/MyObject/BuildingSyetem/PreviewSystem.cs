using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;
    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePeravie(previewObject);
        perpareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void perpareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePeravie(GameObject previewObject)
    {
        Renderer[] renderes =previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderes) 
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }
    public void StopShowingPerview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }
    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewOffset, position.z );
    }
}
