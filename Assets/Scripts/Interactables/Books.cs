using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : Interactable
{
  private Transform player;
  private new Rigidbody rigidbody;
  public float scootForce = 1f;

  protected override void Start()
  {
    base.Start();
    player = GameObject.FindGameObjectWithTag("Player").transform;
    rigidbody = GetComponent<Rigidbody>();
  }

  protected override void PawInteraction()
  {
    base.PawInteraction();
    Vector3 direction = transform.position - player.position;
    direction.y = 0;
    // Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime);
    rigidbody.AddForce(direction * scootForce, ForceMode.Force);
  }
}
