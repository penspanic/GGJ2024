using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : MonoBehaviour
{
    private Quaternion initialRotation;
    private Coroutine coroutine;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public SpriteRenderer spriteRenderer;

    private void OnMouseDown()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        transform.rotation = initialRotation;
        coroutine = StartCoroutine(RotateAndRestoreCoroutine());
    }

    void Start() {
        initialRotation = transform.rotation;
        spriteRenderer.sprite = sprite1;
    }

    private IEnumerator RotateAndRestoreCoroutine()
    {
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, -60f);
        float duration = 0.05f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;

            if(elapsed / duration > 0.3f)
            {
                spriteRenderer.sprite = sprite2;
            }

            if(elapsed / duration > 0.8f)
            {
                spriteRenderer.sprite = sprite3;
            }

            yield return null;
        }

        transform.rotation = targetRotation;
        yield return new WaitForSeconds(0.4f);

        transform.rotation = initialRotation;
        spriteRenderer.sprite = sprite1;
    }
}
