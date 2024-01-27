using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurCatController : MonoBehaviour
{
    [SerializeField]
    private Slider feverSlider = null;
    [SerializeField]
    private Slider angrySlider = null;

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

    private Animator animator = null;

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
        kickBody.SetActive(false);
        normalBody.SetActive(true);

        angryIntervalTimer = 0f;
        angryIntervalTime = Random.Range(5f, 10f);
        angryDurationTimer = 0f;
        angryDurationTime = Random.Range(1f, 3f);

        feverSlider.value = 0f;
        angrySlider.value = 0f;

        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        if(isFever is true) 
        {
            return;
        }

        if (isAngry) 
        {
            angryDurationTimer += Time.deltaTime;
            if (angryDurationTimer >= angryDurationTime) 
            {
                angryDurationTimer = 0f;
                angryDurationTime = Random.Range(2f, 4f);
                isAngry = false;
                animator.SetInteger("State", 0);
            }
        }
        else 
        {
            angryIntervalTimer += Time.deltaTime;
            if (angryIntervalTimer >= angryIntervalTime) 
            {
                angryIntervalTimer = 0f;
                angryIntervalTime = Random.Range(2f, 8f);
                OnAngry();
            }
        }

        if(isScrubbing is false)
        {
            using var query = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(CatFurStatus));
            if (query.TryGetSingleton(out CatFurStatus furStatus) is true) 
            {
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

    private void OnNormal() 
    {
        if(isAngry || isFever || isScrubbing)
            return;

        // angryFace.SetActive(false);
        // //feverFace.SetActive(false);
        // normalFace.SetActive(true);
        // happyFace.SetActive(false);
    }

    public void OnScrub() 
    {
        if(isScrubbing)
            return;

        isScrubbing = true;

        if(isAngry) {
            // add angry gauge
            angryGauge += 0.1f;
            angrySlider.value = angryGauge;

            // if angry gauge is full
            if(angryGauge >= 1f) {
                // to next game
                MainScene.Instance.LoadNextGame();
                return;
            }

            animator.SetInteger("State", 2);
        } else {
            // TODO: mouse 나 고양이에 연출?
            MainScene.Instance.Score += 1;

            // add fever gauge
            feverGauge += 0.02f;
            feverSlider.value = feverGauge;

            // angryFace.SetActive(false);
            // //feverFace.SetActive(false);
            // normalFace.SetActive(false);
            // happyFace.SetActive(true);

            animator.SetInteger("State", 1);
            AudioManager.Instance?.PlaySound("meow1");
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
        isAngry = true;

        animator.SetInteger("State", 2);
        // angryFace.SetActive(true);
        // //feverFace.SetActive(false);
        // normalFace.SetActive(false);
        // happyFace.SetActive(false);

        //StartCoroutine(AngryRoutine());
    }

    private IEnumerator AngryRoutine() {
        yield return new WaitForSeconds(0.1f);
        normalBody.SetActive(false);
        kickBody.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        normalBody.SetActive(true);
        kickBody.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        normalBody.SetActive(false);
        kickBody.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        normalBody.SetActive(true);
        kickBody.SetActive(false);
    }

    // called by button
    public void OnFever() {
        if(feverGauge < 1f) {
            return;
        }

        angryFace.SetActive(false);
        //feverFace.SetActive(true);
        normalFace.SetActive(false);
        happyFace.SetActive(false);
        
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
