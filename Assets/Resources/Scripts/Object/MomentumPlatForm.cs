using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

public class MomentumPlatform : MonoBehaviour, IMovablePlatForm
{
    [SerializeField] private float moveSpeed = 2f; // 이동 속도
    [SerializeField] private Transform destination; // 플랫폼의 목표

    private Vector2 initialPosition;
    private Vector2 previousPos;
    public Vector2 currentMomentumVector2;
    public float maxMomentumMagnitude2D = 0f;

    private Vector2 targetPos;
    public Vector2 momentum;
    public Vector2 finalMomentum;
    private Rigidbody2D rb;
    private Tween platformTween;

    public bool isPlayerOn;
    public bool reTurning;
    [SerializeField] private float returnMomentumTime;
    private WaitForSeconds waitForSeconds;

    void Awake() // Start보다 먼저 호출되어 초기 위치를 정확히 저장
    {
        rb = GetComponent<Rigidbody2D>();
        waitForSeconds = new WaitForSeconds(returnMomentumTime);

        initialPosition = transform.position;
        previousPos = transform.position;
    }

    void FixedUpdate()
    {
        Debug.Log(momentum);
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

        maxMomentumMagnitude2D = 0f;
        momentum = Vector2.zero;

        if (isPlayerOn && !reTurning)
        {
            targetPos = destination.position;
        }
        else if (!isPlayerOn && reTurning)
        {
            targetPos = initialPosition;
        }

        platformTween = transform.DOMove(targetPos, moveSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => { finalMomentum = momentum; StartCoroutine(ResetMomentum()); });
    }

    private void CalculateMomentum()
    {
        Vector2 currentPosition = rb.position;
        Vector2 calculatedVelocity = (currentPosition - previousPos) / Time.fixedDeltaTime; //모멘텀 계산

        currentMomentumVector2 = calculatedVelocity * rb.mass;

        float currentMagnitude = currentMomentumVector2.magnitude;

        if (currentMagnitude > maxMomentumMagnitude2D)
        {
            maxMomentumMagnitude2D = currentMagnitude;
            momentum = currentMomentumVector2;
        }

        previousPos = currentPosition;
    }

    public Vector3 GetMomentum()
    {
        return finalMomentum;
    }

    IEnumerator ResetMomentum()
    {
        yield return waitForSeconds;
        finalMomentum = Vector3.zero;
        momentum = Vector3.zero;
        maxMomentumMagnitude2D = 0;
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