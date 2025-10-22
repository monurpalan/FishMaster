using System;
using DG.Tweening;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Serializable]
    public class FishType
    {
        public int price;
        public float fishCount;
        public float minLength;
        public float maxLength;
        public float colliderRadius;
        public Sprite sprite;
    }

    private FishType type;
    private CircleCollider2D coll;
    private SpriteRenderer rend;
    private float screenLeft;
    private Tweener tweener;

    public FishType Type
    {
        get => type;
        set
        {
            type = value;
            if (coll == null) coll = GetComponentInChildren<CircleCollider2D>();
            if (rend == null) rend = GetComponentInChildren<SpriteRenderer>();
            if (coll != null) coll.radius = type.colliderRadius;
            if (rend != null) rend.sprite = type.sprite;
        }
    }

    void Awake()
    {
        coll = GetComponentInChildren<CircleCollider2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    public void ResetFish()
    {
        // Tüm hareketleri ve animasyonları durdur
        if (tweener != null && tweener.IsActive())
            tweener.Kill(true);

        DOTween.Kill(transform);

        // Balığı başa al
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

        float yPos = UnityEngine.Random.Range(type.minLength, type.maxLength);
        coll.enabled = true;
        transform.position = new Vector3(screenLeft, yPos, 0f);

        float yRandom = UnityEngine.Random.Range(yPos - 1f, yPos + 1f);
        Vector2 target = new Vector2(-transform.position.x, yRandom);

        float moveDuration = 3f;
        float delay = UnityEngine.Random.Range(0, 2 * moveDuration);

        tweener = transform.DOMove(target, moveDuration, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetDelay(delay)
            .OnStepComplete(() =>
            {
                // Yön değiştir
                Vector3 localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            });
    }

    public void Hooked()
    {
        coll.enabled = false;
        if (tweener != null) tweener.Kill(false);
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }
}