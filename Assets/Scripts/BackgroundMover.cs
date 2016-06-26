using UnityEngine;
using System.Collections;

public class BackgroundMover : MonoBehaviour {

  public float scrollingSpeed;
  public float tileSizeX;

  private Vector3 startPosition;
  private bool scrolling;

	void Start () {
    startPosition = transform.position;
    StartScrolling();
	}
	
	// Update is called once per frame
	void Update () {
    if(scrolling) {
      float newPosition = Mathf.Repeat(Time.time * scrollingSpeed, tileSizeX);
      transform.position = startPosition - Vector3.right * newPosition;
    }
  }

  public void StartScrolling() {
    scrolling = true;
  }

  public void StopScrolling() {
    scrolling = false;
  }

}
