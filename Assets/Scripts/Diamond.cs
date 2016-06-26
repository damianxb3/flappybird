using UnityEngine;
using System.Collections;

public class Diamond : MonoBehaviour {

  public AudioClip pickupSound;

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
    if(other.CompareTag("Player")) {
      gameController.AddScore(50);
      AudioSource.PlayClipAtPoint(pickupSound, new Vector3(0, 0, -50), 0.8f);
      Destroy(this.gameObject);
    }
  }

}
