using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {

  public float barrierMovingSpeed;

  private Rigidbody2D rb2d;

  void Start() {
    // moving:
    rb2d = GetComponent<Rigidbody2D>();
    rb2d.velocity = new Vector2(-barrierMovingSpeed, 0.0f);
  }

  public void StopBarrier() {
    rb2d.velocity = Vector2.zero;
  }
}
