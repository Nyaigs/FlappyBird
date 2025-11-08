using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // animate bird wings every 0.10s
        InvokeRepeating(nameof(AnimateSprite), 0.10f, 0.10f);
    }

    private void OnEnable()
    {
        // reset player position when re-enabled
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        // flap (jump up)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        // apply gravity + move
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // tilt the bird based on movement
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger Game Over when hitting an obstacle
        if (other.gameObject.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>().GameOver();
        }
        // Trigger score increase when passing scoring zone
        else if (other.gameObject.CompareTag("Scoring"))
        {
            FindFirstObjectByType<GameManager>().IncreaseScore();
        }
    }
}
