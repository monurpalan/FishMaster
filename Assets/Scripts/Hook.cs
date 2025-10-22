using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    [SerializeField] public Transform hookTransform;
    private Camera mainCamera;
    private Collider2D coll;
    private int length;
    private int strength;
    private int fishCount;
    private bool canMove;
    private List<Fish> hookedFishes;
    private Tweener cameraTween;

    void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
    }

    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = mouseWorldPos.x;
            transform.position = position;
        }
    }

    public void StartFishing()
    {
        length = IdleManager.instance.length;
        strength = IdleManager.instance.strength;
        fishCount = 0;
        float time = (-length) * 0.1f;

        // Kamera aşağı iniş animasyonu
        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false)
            .OnUpdate(() =>
            {
                if (mainCamera.transform.position.y <= -11)
                    transform.SetParent(mainCamera.transform);
            })
            .OnComplete(() =>
            {
                coll.enabled = true;
                // Kamera yukarı çıkış animasyonu
                cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false)
                    .OnUpdate(() =>
                    {
                        if (mainCamera.transform.position.y >= -25f)
                            StopFishing();
                    });
            });

        ScreensManager.instance.ChangeScreen(Screens.GAME);
        coll.enabled = false;
        canMove = true;
        hookedFishes.Clear();
    }

    public void StopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);

        // Kamera sıfırlama animasyonu
        cameraTween = mainCamera.transform.DOMoveY(0, 2f, false)
            .OnUpdate(() =>
            {
                if (mainCamera.transform.position.y >= -11)
                {
                    transform.SetParent(null);
                    transform.position = new Vector2(transform.position.x, -6);
                }
            })
            .OnComplete(() =>
            {
                transform.position = Vector2.down * 6;
                coll.enabled = true;
                int totalGain = 0;
                foreach (var fish in hookedFishes)
                {
                    fish.transform.SetParent(null, true);
                    fish.transform.localScale = Vector3.one;
                    fish.transform.rotation = Quaternion.identity;
                    fish.ResetFish();
                    totalGain += fish.Type.price;
                }
                IdleManager.instance.totalGain = totalGain;
                ScreensManager.instance.ChangeScreen(Screens.END);
            });
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Fish") && fishCount < strength)
        {
            fishCount++;
            Fish fish = target.GetComponent<Fish>();
            fish.Hooked();
            hookedFishes.Add(fish);
            target.transform.SetParent(hookTransform);
            target.transform.position = hookTransform.position;
            target.transform.rotation = hookTransform.rotation;
            target.transform.localScale = Vector3.one;

            // Balık sallanma animasyonu
            target.transform.DOShakePosition(5, Vector3.forward * 45, 10, 90, false)
                .SetLoops(1, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    target.transform.rotation = Quaternion.identity;
                });

            if (fishCount == strength)
            {
                StopFishing();
            }
        }
    }
}