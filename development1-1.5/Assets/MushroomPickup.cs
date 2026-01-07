using UnityEngine;
using System.Collections;

public class MushroomPickup : MonoBehaviour
{
    public GameObject uiPrompt;
    public GameObject pickupEffect;
    public AudioClip pickupSound;
    public float fadeDuration = 1.0f;

    private bool isPlayerInRange = false;
    private bool isPickedUp = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (isPlayerInRange && !isPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickUpSequence());
        }
    }

    IEnumerator PickUpSequence()
    {
        isPickedUp = true;
        
        if (uiPrompt != null) uiPrompt.SetActive(false);

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        float elapsed = 0;
        Vector3 initialScale = transform.localScale;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / fadeDuration;

            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, percent);
            
            yield return null;
        }

        Destroy(gameObject);
        Debug.Log("蘑菇采集完成并销毁");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            isPlayerInRange = true;
            if (uiPrompt != null) uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (uiPrompt != null) uiPrompt.SetActive(false);
        }
    }
}