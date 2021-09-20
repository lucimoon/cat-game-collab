using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FallsAndBreaks : Interactable
{
  private Transform player;
  private new Rigidbody rigidbody;
  private BoxCollider boxCollider;
  private new ParticleSystem particleSystem;
  private MeshRenderer meshRenderer;
  private IEnumerator coroutine;
  public float nudgeForce = 400f;

  protected override void Start()
  {
    base.Start();
    player = GameObject.FindGameObjectWithTag("Player").transform;
    rigidbody = GetComponent<Rigidbody>();
    boxCollider = GetComponent<BoxCollider>();
    particleSystem = GetComponent<ParticleSystem>();
    meshRenderer = GetComponent<MeshRenderer>();
  }

  // These three might be useful in other scripts for other interactables
  void FixedUpdate()
  {
    isObjectMoving();
  }

  private void isObjectMoving()
  {
    // Use IsSleeping() to test if the object is moving
    if (rigidbody != null && rigidbody.IsSleeping())
    {
      // If we run out of interactions, make the object essentially static
      if (interactionCount == maxInteractionCount)
      {
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        boxCollider.isTrigger = false;
        rigidbody.gameObject.isStatic = true;
      }
      else
      {
        // If the object is finished moving and we have more interactions left, allow more interactions
        handleInteractablity(true);
      }
    }
  }

  private void handleInteractablity(bool enable)
  {
    allowInteractions = enable && interactionCount < maxInteractionCount; // This should definitely be accessible to interaction tip so we can hide it

    // Toggle these settings to handle the physical presence and interactivity of the object
    rigidbody.useGravity = !allowInteractions;
    rigidbody.isKinematic = allowInteractions;
    boxCollider.isTrigger = allowInteractions;
  }

  protected override void PawInteraction()
  {
    base.PawInteraction();

    if (allowInteractions)
    {
      interactionCount += 1;

      // Turn off interactivity, and let the object get nudged
      handleInteractablity(false);

      Vector3 direction = transform.position - player.position;
      direction.y = 0;

      rigidbody.AddForce(direction * nudgeForce, ForceMode.Force);
    }
  }

  void OnCollisionEnter(Collision other)
  {
    GameObject interactedObject = other.gameObject;
    if (interactedObject.CompareTag("Floor"))
    {
      // Start particle effect
      particleSystem.Play();

      // Disable the mesh & collider of original interactable
      boxCollider.enabled = false;
      meshRenderer.enabled = false;
      rigidbody.gameObject.isStatic = true;


      // Persist effect after pause
      StartCoroutine(coroutine = ParticlePlayback());
    }
  }

  private IEnumerator ParticlePlayback()
  {
    yield return new WaitForSeconds(2.0f);
    particleSystem.Pause();
  }
}
