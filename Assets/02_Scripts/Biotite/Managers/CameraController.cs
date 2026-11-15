using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [Header("Parrying Shake Settings")]
    [SerializeField] private float parryingTimeScale = 0.2f;
    [SerializeField] private float parryingSlowDuration = 0.3f;

    private Vector3 originalPosition;
    private CancellationTokenSource shakeCts;
    private CancellationTokenSource timeScaleCts;

    public void Init()
    {
        originalPosition = transform.localPosition;
        Managers.Event.AddEvent<OnParryingSucceed>(OnParryingSucceed);
        Managers.Event.AddEvent<OnPlayerHit>(OnPlayerHit);
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent<OnParryingSucceed>(OnParryingSucceed);
        Managers.Event.RemoveEvent<OnPlayerHit>(OnPlayerHit);

        shakeCts?.Cancel();
        shakeCts?.Dispose();

        timeScaleCts?.Cancel();
        timeScaleCts?.Dispose();
        Time.timeScale = 1f;
    }

    private void OnParryingSucceed(OnParryingSucceed e)
    {
        ResetTimeScaleTask();
        timeScaleCts = new CancellationTokenSource();
        ApplyParryingSlowMotionAsync(timeScaleCts.Token).Forget();

        ResetShakeTask();
        shakeCts = new CancellationTokenSource();
        ShakeCameraAsync(shakeCts.Token, useUnscaledTime: true).Forget();
    }

    private void OnPlayerHit(OnPlayerHit e)
    {
        ResetShakeTask();
        shakeCts = new CancellationTokenSource();
        ShakeCameraAsync(shakeCts.Token, useUnscaledTime: false).Forget();
    }

    private async UniTaskVoid ShakeCameraAsync(CancellationToken cancellationToken, bool useUnscaledTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            if (cancellationToken.IsCancellationRequested) return;

            float currentDecay = 1f - (elapsedTime / shakeDuration);

            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude * currentDecay;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude * currentDecay;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsedTime += deltaTime;

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }

        transform.localPosition = originalPosition;
    }

    private async UniTaskVoid ApplyParryingSlowMotionAsync(CancellationToken cancellationToken)
    {
        Time.timeScale = parryingTimeScale;

        bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(parryingSlowDuration), DelayType.Realtime, cancellationToken: cancellationToken).SuppressCancellationThrow();

        if (!isCanceled)
        {
            Time.timeScale = 1f;
        }
    }

    private void ResetShakeTask()
    {
        shakeCts?.Cancel();
        shakeCts?.Dispose();
    }

    private void ResetTimeScaleTask()
    {
        timeScaleCts?.Cancel();
        timeScaleCts?.Dispose();
    }
}