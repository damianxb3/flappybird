using UnityEngine;
using System.Collections;

public class HoldPlayerInTheScreen : MonoBehaviour {

  void OnTriggerStay2D(Collider2D other) {
    if(other.CompareTag("Player")) {
      other.transform.position = new Vector3(other.transform.position.x, 36.75f, other.transform.position.z);
    }
  }
  
}
