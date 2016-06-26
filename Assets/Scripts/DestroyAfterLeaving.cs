using UnityEngine;
using System.Collections;

public class DestroyAfterLeaving : MonoBehaviour {

  void OnTriggerExit2D(Collider2D other) {
    // note: player falling animation, which works just for little time
    if(!other.CompareTag("Player"))
      Destroy(other.gameObject);
  }
}