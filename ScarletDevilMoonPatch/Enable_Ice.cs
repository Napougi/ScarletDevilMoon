using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Ice : MonoBehaviour
{
    // HashSet to track objects that are currently growing
    private HashSet<GameObject> growingObjects = new HashSet<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        // Find all colliders in the vicinity
        var colliders = Physics.OverlapSphere(transform.position, 10f);

        foreach (var collider in colliders)
        {
            // Check if the collider is attached to the parent object
            if (collider.gameObject.name == "Ice trigger")
            {
                // Turn on the first and second child objects and set Frozen flag to true
                Transform firstChildTransform = collider.transform.GetChild(0); // First child
                Transform secondChildTransform = collider.transform.GetChild(1); // Second child

                AudioSource audioSource = collider.gameObject.GetComponent<AudioSource>();

                if (!firstChildTransform.gameObject.activeSelf)
                {
                    firstChildTransform.gameObject.SetActive(true);
                    Debug.Log($"{firstChildTransform.gameObject.name} has been enabled");

                    secondChildTransform.gameObject.SetActive(true);
                    Debug.Log($"{secondChildTransform.gameObject.name} has been enabled");

                    if (audioSource != null)
                    {
                        audioSource.Play();
                        Debug.Log("Playing Ice frozen audio");
                    }

                    // Start the coroutine for this specific collider if not already growing
                    if (!growingObjects.Contains(collider.gameObject))
                    {
                        growingObjects.Add(collider.gameObject); // Add to growing set
                        StartCoroutine(GrowObject(collider.gameObject));
                    }
                }
            }
        }
    }

    // Coroutine to grow the object over time
    private IEnumerator GrowObject(GameObject obj)
    {
        // Get the current scale of the object
        Vector3 targetScale = obj.transform.localScale; // Use current scale as the target
        Vector3 initialScale = targetScale * 0.33f; // Set initial scale to be smaller (33% of current scale)

        obj.transform.localScale = initialScale; // Set the object's scale to the initial scale

        float duration = 2f; // Duration in seconds for the growth
        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration); // Smoothly transition the scale
            elapsed += Time.deltaTime; // Increase elapsed time
            yield return null; // Wait until the next frame
        }

        obj.transform.localScale = targetScale; // Ensure it reaches the target scale

        // Remove from growing set after finishing
        growingObjects.Remove(obj);
    }
}
