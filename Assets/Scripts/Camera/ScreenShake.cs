using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Camera mainCam;
    public float strengthModifier;
    public float shakeDuration;
    public AnimationCurve animCurve;
    public bool start = false;
    private Vector3 startPos;

    public void StartShake(float strengthModifier, float shakeDuration) {
        this.strengthModifier = strengthModifier;
        this.shakeDuration = shakeDuration;
        start = true;
    }

    void Update() {
        if (start) {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking() {
        Vector3 startPos = mainCam.transform.position;
        float elapseTime = 0f;

        while (elapseTime < shakeDuration) {
            elapseTime += Time.deltaTime;
            float strength = animCurve.Evaluate(elapseTime / shakeDuration);
            mainCam.transform.position = startPos + (Random.insideUnitSphere * strength * strengthModifier);
            yield return null;
        }
        mainCam.transform.position = startPos;
    }
}
