using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class ScreenFade : MonoBehaviour
    {
        public static ScreenFade Instance
        {
            get
            {
                if (instance == null)
                    instance = Instantiate(Resources.Load<GameObject>("ScreenFade")).GetComponent<ScreenFade>();
                return instance;
            }
        }

        private static ScreenFade instance;

        public SpriteRenderer sprite;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void FadeIn()
        {
            sprite.color = new Color(0, 0, 0, 1);
            StartCoroutine(FadeInRoutine());
        }

        public IEnumerator FadeInRoutine()
        {
            float totalTime = 1f;
            float time = 0f;
            while (time < totalTime)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / totalTime);
                sprite.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            sprite.color = new Color(0, 0, 0, 0);
        }

        public void FadeOut()
        {
            sprite.color = new Color(0, 0, 0, 0);
            StartCoroutine(FadeOutRoutine());
        }

        public IEnumerator FadeOutRoutine()
        {
            float totalTime = 1f;
            float time = 0f;
            while (time < totalTime)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / totalTime);
                sprite.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            sprite.color = new Color(0, 0, 0, 1);
        }
    }
}