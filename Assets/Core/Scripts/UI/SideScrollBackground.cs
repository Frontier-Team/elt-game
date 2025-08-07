using UnityEngine;

public class SideScrollBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private bool reverseDirection = false;
    [SerializeField] private GameObject prefab;
    private GameObject root;
    private float backgroundWidth;
    
    private Vector3 startPosition;

    private void Awake()
    {
        root = Instantiate(prefab, transform);
        root.name = "Root";
        SetBackgroundWidth();
        BuildBounds();
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        var newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundWidth);
        transform.position = !reverseDirection
            ? startPosition + Vector3.left * newPosition
            : startPosition + Vector3.right * newPosition;
    }
    
    private void SetBackgroundWidth()
    {
        var rootRenderer = root?.GetComponent<Renderer>();
        if (rootRenderer != null)
        {
            backgroundWidth = rootRenderer.bounds.size.x * transform.localScale.x;
        }
    }

    private void BuildBounds()
    {
        if (backgroundWidth > 0)
        {
            var leftChild = Instantiate(prefab, root.transform);
            leftChild.name = "LeftChild";
            leftChild.transform.SetParent(gameObject.transform);
            leftChild.transform.localPosition = new Vector3(-backgroundWidth, root.transform.position.y, root.transform.position.z);

            var rightChild = Instantiate(prefab, root.transform);
            rightChild.name = "RightChild";
            rightChild.transform.SetParent(gameObject.transform);
            rightChild.transform.localPosition = new Vector3(backgroundWidth, root.transform.position.y, root.transform.position.z);
        }
    }
}