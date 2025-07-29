using UnityEngine;
using DG.Tweening; // DoTween 네임스페이스 추가

public class MomentumPlatform : MonoBehaviour, IMovablePlatForm
{
    [SerializeField] private float moveSpeed = 2f; // 이동 속도
    [SerializeField] private Transform destination; // 플랫폼의 목표
    private Vector2 initialPosition; 
    
    private Vector2 previousPos;
    public Vector2 currentMomentumVector2;
    public Vector2 deltaPos { get; set; }
    public float maxMomentumMagnitude2D = 0f; 
    
    private Vector2 targetPos;
    public Vector2 momentum { get; set; }
    private Rigidbody2D rb;
    private Tween platformTween;
    public bool isPlayerOn { get; private set; }
    public bool reTurning;

    void Awake() // Start보다 먼저 호출되어 초기 위치를 정확히 저장
    {
        rb = GetComponent<Rigidbody2D>();

        initialPosition = transform.position;
        previousPos = transform.position;
    }

    void FixedUpdate()
    {
        if ((Vector2)transform.position == initialPosition)
        {
            reTurning = false;
        }

        CalculateMomentum();
    }

    private void SetPlayerOnState(bool newState)
    {
        if (isPlayerOn == newState) return; // 상태가 실제로 변경될 때만 진행

        isPlayerOn = newState;
        
        StartPlatformMovement(); 
    }
    
    private void StartPlatformMovement()
    {
        if (platformTween != null && platformTween.IsActive())
        {
            platformTween.Kill();
        }

        if (isPlayerOn && !reTurning)
        {
            targetPos = destination.position;
        }
        else if(!isPlayerOn && reTurning)
        {
            targetPos = initialPosition;
        }
        platformTween = rb.DOMove(targetPos, moveSpeed).SetSpeedBased().Play(); // SetEase 추가
    }

    private void CalculateMomentum()
    {
        Vector2 currentPosition = rb.position; 
        Vector2 calculatedVelocity = (currentPosition - previousPos) / Time.fixedDeltaTime; //모멘텀 계산

        deltaPos = currentPosition - previousPos;

        currentMomentumVector2 = calculatedVelocity * rb.mass; 
        
        float currentMagnitude = currentMomentumVector2.magnitude;

        if (currentMagnitude > maxMomentumMagnitude2D)
        {
            maxMomentumMagnitude2D = currentMagnitude;
            momentum = currentMomentumVector2;
        }

        previousPos = currentPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetPlayerOnState(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetPlayerOnState(false);
            reTurning = true;
        }
    }

    void OnDisable()
    {
        if (platformTween != null && platformTween.IsActive())
        {
            platformTween.Kill();
        }
    }
}