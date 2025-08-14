using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes the player's position to match the target GameObject's position
    /// </summary>
    /// <param name="targetObject">The GameObject whose position the player should move to</param>
    public void ChangePlayerPositionTo(GameObject targetObject)
    {
        if (targetObject != null)
        {
            // Set this GameObject's position to the target GameObject's position
            transform.position = targetObject.transform.position;
        }
        else
        {
            Debug.LogWarning("Target GameObject is null! Cannot change player position.");
        }
    }
}
