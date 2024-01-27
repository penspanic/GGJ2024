using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurCatController : MonoBehaviour
{
    [SerializeField]
    private GameObject angryFace = null;

    [SerializeField]
    private GameObject feverFace = null;

    [SerializeField]
    private GameObject normalFace = null;

    [SerializeField]
    private GameObject happyFace = null;

    [SerializeField]
    private GameObject normalBody = null;
    [SerializeField]
    private GameObject kickBody = null;

    private float angryIntervalTime = 0f;
    private float angryIntervalTimer = 0f;
    private float angryDurationTime = 0f;
    private float angryDurationTimer = 0f;
    private float feverGauge = 0f;
    private float angryGauge = 0f;
    private bool isAngry = false;
    private bool isScrubbing = false;
    private bool isFever = false;

    private void Awake() 
    {
        angryFace.SetActive(false);
        //feverFace.SetActive(false);
        normalFace.SetActive(true);
        happyFace.SetActive(false);

        angryIntervalTimer = 0f;
        angryIntervalTime = Random.Range(5f, 10f);
        angryDurationTimer = 0f;
        angryDurationTime = Random.Range(1f, 3f);
    }

    private void Update() 
    {
        if(isFever is true) 
        {
            return;
        }
        
        Debug.Log("fur cat update");
        
        if (isAngry) 
        {
            angryDurationTimer += Time.deltaTime;
            if (angryDurationTimer >= angryDurationTime) 
            {
                angryDurationTimer = 0f;
                angryDurationTime = Random.Range(0.5f, 3f);
                isAngry = false;
            }
        }
        else 
        {
            angryIntervalTimer += Time.deltaTime;
            if (angryIntervalTimer >= angryIntervalTime) 
            {
                angryIntervalTimer = 0f;
                angryIntervalTime = Random.Range(2f, 8f);
                isAngry = true;
                OnAngry();
            }

            if(isScrubbing is false)
            {
                Debug.Log("check query");
                using var query = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(CatFurStatus));
                if (query.TryGetSingleton(out CatFurStatus furStatus) is true) 
                {
                    Debug.Log(furStatus.LastTouchTime);
                    if (furStatus.LastTouchTime > Time.time - 0.5f) 
                    {
                        OnScrub();
                        return;
                    }
                } 
                else 
                {
                    Debug.Log("TryGetSingleton false");
                }

                OnNormal();
            }
        }
    }

    private void OnNormal() 
    {
        angryFace.SetActive(false);
        //feverFace.SetActive(false);
        normalFace.SetActive(true);
        happyFace.SetActive(false);
    }

    public void OnScrub() 
    {
        if(isScrubbing)
            return;

        isScrubbing = true;

        if(isAngry) {
            // add angry gauge
            angryGauge += 0.1f;

            // if angry gauge is full
            if(angryGauge >= 1f) {
                // to next game
                MainScene.Instance.LoadNextGame();
                return;
            }
        } else {
            // TODO: mouse 나 고양이에 연출?
            MainScene.Instance.Score += 1;

            // add fever gauge
            feverGauge += 0.1f;

            angryFace.SetActive(false);
            //feverFace.SetActive(false);
            normalFace.SetActive(false);
            happyFace.SetActive(true);
        }

        StartCoroutine(EndScrub());
    }

    private IEnumerator EndScrub() {
        yield return new WaitForSeconds(0.5f);
        OnScrubEnd();
    }

    public void OnScrubEnd() {
        isScrubbing = false;
    }

    private void OnAngry() {
        angryFace.SetActive(true);
        //feverFace.SetActive(false);
        normalFace.SetActive(false);
        happyFace.SetActive(false);
    }

    // called by button
    public void OnFever() {
        if(feverGauge >= 1f) {
            angryFace.SetActive(false);
            //feverFace.SetActive(true);
            normalFace.SetActive(false);
            happyFace.SetActive(false);
        }
        // 츄르 주고 angry 안되게
        StartCoroutine(FeverRoutine());
    }

    private IEnumerator FeverRoutine() {
        isFever = true;
        yield return new WaitForSeconds(5f);
        OnFeverEnd();
    }

    private void OnFeverEnd() {
        angryFace.SetActive(false);
        //feverFace.SetActive(false);
        normalFace.SetActive(true);
        happyFace.SetActive(false);

        isFever = false;
    }
}
