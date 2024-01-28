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
    private Collider2D collider2D;

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
        collider2D = spriteRenderer.gameObject.GetComponent<Collider2D>();
        collider2D.enabled = false;
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
                collider2D.enabled = true;
            }

            if(elapsed / duration > 0.8f)
            {
                spriteRenderer.sprite = sprite3;
                collider2D.enabled = true;
            }

            yield return null;
        }

        transform.rotation = targetRotation;
        collider2D.enabled = false;
        yield return new WaitForSeconds(0.4f);

        transform.rotation = initialRotation;
        spriteRenderer.sprite = sprite1;
    }
}
