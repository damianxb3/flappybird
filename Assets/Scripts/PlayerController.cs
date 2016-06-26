using UnityEngine;

public class PlayerController : MonoBehaviour {

  public float yMin, yMax;
  public float liftFactor;
  public bool GodMode {
    get { return godMode; }
    set { godMode = value; }
  }

  public bool ControlBlocked {
    get { return controlBlocked; }
    set { controlBlocked = value; }
  }

  private Rigidbody2D rb2d;
  private SpriteRenderer spriteRenderer;
  private AudioSource wingsSound;
  private float lift;
  private bool godMode;
  private bool controlBlocked;

  public void SetGravity(float gravityScale) {
    rb2d.velocity = Vector3.zero;
    rb2d.gravityScale = gravityScale;
    lift = gravityScale * liftFactor;
  }

  void Start () {
    rb2d = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    wingsSound = GetComponent<AudioSource>();
  }

	void Update () {
	  if(Input.touchCount > 0 && !controlBlocked) {
      if(!godMode && rb2d.gravityScale == 0)
        SetGravity(20.0f);
      rb2d.velocity = new Vector2(0.0f, lift);
      if(!wingsSound.isPlaying) {
        wingsSound.volume = Random.Range(0.8f, 1f);
        wingsSound.pitch = Random.Range(1.0f, 1.5f);
        wingsSound.Play();
      }
    }

	}

  public void PlayerFlashing() {
    if(godMode) {
      spriteRenderer.enabled = !spriteRenderer.enabled;
      Invoke("PlayerFlashing", 0.1f);
      return;
    }
    spriteRenderer.enabled = true;
  }
}
