using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class MomentumPlatformV2 : MonoBehaviour, IMovablePlatForm, ITriggerable
{
    [SerializeField] private EnemyAIController ai;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform destination;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private SpriteRenderer render;
    private BoxCollider2D col;
    private Rigidbody2D rb;

    private BlackBoard local;
    private BlackBoard global;

    private Vector2 previousPos;
    private Vector2 currentMomentumVector2;
    private float maxMomentumMagnitude2D = 0f;


    private void Awake()
    {
        local = new BlackBoard();
        col = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        render = this.GetComponent<SpriteRenderer>();

        local.Set("InitPos", this.transform.position);
        local.Set("Destination", destination);
        previousPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        //this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
        CalculateMomentum();
    }

    private void CalculateMomentum()
    {
        Vector2 currentPosition = rb.position;
        Vector2 calculatedVelocity = (currentPosition - previousPos) / Time.fixedDeltaTime; //모멘텀 계산

        currentMomentumVector2 = calculatedVelocity * rb.mass;

        float currentMagnitude = currentMomentumVector2.magnitude;
        local.Set("CurrentMomentum", currentMomentumVector2);

        if (currentMagnitude > local.Get<Vector2>("MaxMomentum").magnitude)
        {
            maxMomentumMagnitude2D = currentMagnitude;
            local.Set("MaxMomentum", currentMomentumVector2);
        }
        previousPos = currentPosition;
    }

    private void Start()
    {
        ai.BlackBoardInit(local, GameManager.instance.globalBlackBoard);

        local.Set("MoveSpeed", moveSpeed);
        local.Set("Transform", this.transform);
        local.Set("Collider", col);
        local.Set("LayerMask", 1 << LayerMask.NameToLayer("Player"));
        local.Set("Momentum", Vector2.zero);
        local.Set("Rigid", this.GetComponent<Rigidbody2D>());
        local.Set("CanReturnMomentum", false);
        local.Set("Trigger", false);
        local.Set("PreviousTrigger", local.Get<bool>("Trigger"));
    }

    public Vector2 GetMomentum()
    {
        if (local.Get<bool>("CanReturnMomentum"))
        {
            return local.Get<Vector2>("MaxMomentum");
        }
        else
        {
            return Vector2.zero;
        }
    }

    public float GetMomentumY()
    {
        if (local.Get<bool>("CanReturnMomentum"))
        {
            return 0;
        }
        else
        {
            return moveSpeed;
        }
    }

    public BlackBoard GetBlackBoard()
    {
        return local;
    }

    public void Trigger()
    {
        render.sprite = !local.Get<bool>("Trigger") ? sprites[0] : sprites[1];
    }
}
