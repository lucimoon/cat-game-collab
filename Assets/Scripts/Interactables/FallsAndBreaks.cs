using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallsAndBreaks : Interactable
{
  private Transform player;
  private new Rigidbody rigidbody;
  private BoxCollider boxCollider;
  private new ParticleSystem particleSystem;
  private MeshRenderer meshRenderer;
  private IEnumerator coroutine;
  public bool isDestroyed = false;
  public bool isMoving = false;
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

  void LateUpdate()
  {
    if (rigidbody != null && rigidbody.velocity.magnitude > 0.01f)
    {
      isMoving = true;
    }
    else
    {
      isMoving = false;
    }
  }

  protected override void PawInteraction()
  {
    base.PawInteraction();

    if (allowInteractions)
    {
      Debug.Log("Starting object transform, turning off interactions");
      allowInteractions = false;
      rigidbody.useGravity = true;
      rigidbody.isKinematic = false;
      boxCollider.isTrigger = false;

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

      // Remove the mesh & collider of original interactable
      Destroy(meshRenderer);
      Destroy(boxCollider);
      Destroy(rigidbody);

      isDestroyed = true;

      // Persist effect after pause
      StartCoroutine(coroutine = ParticlePlayback());
    }
    else
    {
      if (!isDestroyed && !isMoving)
      {
        StartCoroutine(coroutine = DelayInteraction());
      }
    }
  }

  private IEnumerator ParticlePlayback()
  {
    yield return new WaitForSeconds(2.0f);
    particleSystem.Pause();
  }

  private IEnumerator DelayInteraction()
  {
    yield return new WaitForSeconds(2.0f);
    if (rigidbody != null)
    {
      rigidbody.useGravity = false;
      rigidbody.isKinematic = true;
      boxCollider.isTrigger = true;
      allowInteractions = true;
    }
  }
}
