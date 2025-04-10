using UnityEngine;

public class Interactable : MonoBehaviour
{

    public bool HasInteracted { get; set; }

    protected float radius = 1.5f;

    protected virtual void Awake()
    {
        HasInteracted = false;
    }

    private void OnEnable()
    {
        HasInteracted = false;
    }

    public virtual void Interact()
    {
    }

    public virtual void NonInteract()
    {
    }

    // Update is called once per frame
/*    private void Update()
    {
        if (!HasInteracted)
        {
            if (DistanceFromPlayer <= radius)
            {
                Interact();
            }
        }
        else
        {
            if (DistanceFromPlayer > radius)
            {
                NonInteract();
            }
        }
    }*/
}