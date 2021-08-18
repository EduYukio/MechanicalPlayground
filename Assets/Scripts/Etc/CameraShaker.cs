using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
    public IEnumerator ShakeCoroutine(GameObject cameraObj, float duration, float magnitude) {
        Vector3 originalPos = cameraObj.transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraObj.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraObj.transform.localPosition = originalPos;
    }

    public void Shake(GameObject cameraObj, float duration, float magnitude) {
        StartCoroutine(ShakeCoroutine(cameraObj, duration, magnitude));
    }
}
