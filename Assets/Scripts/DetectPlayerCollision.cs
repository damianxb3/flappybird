using UnityEngine;
using System.Collections;

public class DetectPlayerCollision : MonoBehaviour {

  private GameController gameController;
  
  void Start() {

    // getting GameController:
    GameObject gameControllerObject = GameObject.FindWithTag("GameController");
    if(gameControllerObject != null)
      gameController = gameControllerObject.GetComponent<GameController>();
    if(gameController == null)
      Debug.Log("Cannot find 'Game Controller' script");
  }

  void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("Player") && !other.gameObject.GetComponent<PlayerController>().GodMode) {
      gameController.OnPlayerHitBarrier();
    }
  }
}
