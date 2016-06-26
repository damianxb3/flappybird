using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

  public Barrier barriersPrefab;
  public Diamond diamondPrefab;
  public GameObject heartPrefab;
  public GameObject heartsArea;
  public PlayerController player;
  public BackgroundMover background;
  public TextMesh playGameText;
  public TextMesh gameOverText;
  public TextMesh scoreText;
  public TextMesh highScoreText;
  public float gravity;
  public float xBarrierPos;
  public float startTime;
  public float waitTimeMin;
  public float waitTimeMax;
  public float minBarriersHoleLength;
  public float maxBarriersHoleLength;
  public float scoreTimeDelay;

  private AudioSource impactSound;
  private GameObject[] hearts;
  private bool gameStarted;
  private float randomWaitTime;
  private int playerScore;
  private int playerHighScore;
  private int playerLives;

  void Start() {
    impactSound = GetComponent<AudioSource>();
    playerHighScore = PlayerPrefs.GetInt("highscore", -10);
    ShowStartScreen();
  }

  void Update() {
    if(!gameStarted && Input.touchCount > 0)
      StartGame();
    scoreText.text = string.Format("SCORE: {0}", playerScore);
  }

  public void OnPlayerHitBarrier() {
    // hitting sound:
    impactSound.Play();
    if(--playerLives != 0) {
      Destroy(hearts[playerLives]);
      player.GodMode = true;
      player.PlayerFlashing();
      player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
      player.SetGravity(0.0f);
      Invoke("PlayerResetGodMode", 0.5f);
      return;
    }
    OnPlayerDie();
  }
  void OnPlayerDie() {
    Destroy(hearts[0]);
    //show Game Over text:
    gameOverText.gameObject.SetActive(true);
    // stop creating barriers and moving created barriers:
    StopAllCoroutines();
    foreach(GameObject barrier in GameObject.FindGameObjectsWithTag("Barrier")) {
      Barrier b = barrier.GetComponent<Barrier>();
      if(b != null)
        b.StopBarrier();
    }
    // stop scrolling:
    background.StopScrolling();
    //block control:
    player.SetGravity(0.0f);
    player.ControlBlocked = true;
    // save highscore (safe):
    SetHighscore(playerScore);
    Invoke("GameOver", 2);
  }
  void GameOver() {
    GoogleMobileAdsDemoScript.Instance.ShowInterstitial();
    // Restarting the scene:
    ShowStartScreen();
  }

  void PlayerResetGodMode() {
    player.GodMode = false;
  }

  void SetHighscore(int highscore) {
    string key = "highscore";

    if(PlayerPrefs.HasKey(key)) {
      if(PlayerPrefs.GetInt(key) <= highscore)
        PlayerPrefs.SetInt(key, highscore);
    } else
      PlayerPrefs.SetInt(key, highscore);
    PlayerPrefs.Save();
  }

  IEnumerator CreateBarriers() {
    yield return new WaitForSeconds(startTime);
    while(true) {
      // set a hole length to random:
      float holeLength = Random.Range(minBarriersHoleLength, maxBarriersHoleLength);

      Barrier topBarrier = Instantiate (
        barriersPrefab, 
        new Vector2(xBarrierPos, Random.Range(holeLength+20, 100-20)),
        barriersPrefab.transform.rotation
      ) as Barrier;
      // bottomBarrier: 
      Instantiate(
        barriersPrefab,
        new Vector2(xBarrierPos, topBarrier.transform.position.y-100-holeLength), 
        barriersPrefab.transform.rotation
      );

      // set random width of the barriers:
      //float barrierWidth = Random.Range(barrierMinWidth, barrierMaxWidth);

      // calculate time for the next barrier:
      randomWaitTime = Random.Range(waitTimeMin, waitTimeMax);

      // create a diamond/heart (or not) depending on the time for the next barrier turning up
      int random = Random.Range(1, 10);
      if(random >= 8) {
        float x = xBarrierPos + Random.Range(10, 35);
        float y = Random.Range(-35, 35);
        if(playerLives < 3 && random >= 9)
          CreateHeart(topBarrier, new Vector2(x, y));
        else
          CreateDiamond(topBarrier, new Vector2(x, y));
      }
      yield return new WaitForSeconds(randomWaitTime);
    }
  }
  IEnumerator AddScore() {
    while(true) {
      yield return new WaitForSeconds(scoreTimeDelay);
      playerScore += 10;
    }
  }
  public void AddScore(int value) {
    playerScore += value;
  }
  public void AddPlayerLive() {
    if(playerLives == 3) {
      AddScore(100);
      return;
    }
    playerLives += 1;
    hearts[playerLives-1] = Instantiate(heartPrefab);
    hearts[playerLives-1].transform.parent = heartsArea.transform;
    hearts[playerLives-1].transform.localPosition = new Vector3(4*(playerLives-1), 0, 0);
  }
  void ShowStartScreen() {
    // texts:
    gameOverText.gameObject.SetActive(false);
    scoreText.gameObject.SetActive(false);
    highScoreText.gameObject.SetActive(false);
    playGameText.gameObject.SetActive(true);
    // destroy all barriers (if any):
    GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
    foreach(GameObject barrier in barriers)
      Destroy(barrier);
    gameStarted = false;
    // block the player in the middle of the screen until the game starts:
    player.transform.position = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);
    player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    player.ControlBlocked = true;
    // scrolling background:
    background.StartScrolling();
  }
  void StartGame() {
    gameStarted = true;
    playerScore = 0;
    playerLives = 3;
    InstantiateThreeHearts();
    scoreText.gameObject.SetActive(true);
    
    // highscore:
    playerHighScore = PlayerPrefs.GetInt("highscore", 0);
    highScoreText.text = string.Format("HIGHSCORE: {0}", playerHighScore);
    highScoreText.gameObject.SetActive(true);

    playGameText.gameObject.SetActive(false);
    player.SetGravity(gravity);
    player.ControlBlocked = false;

    StartCoroutine(CreateBarriers());
    StartCoroutine(AddScore());
  }
  void InstantiateThreeHearts() {
    hearts = new GameObject[3];
    hearts[0] = Instantiate(heartPrefab);
    hearts[1] = Instantiate(heartPrefab);
    hearts[2] = Instantiate(heartPrefab);
    hearts[0].transform.parent = heartsArea.transform;
    hearts[1].transform.parent = heartsArea.transform;
    hearts[2].transform.parent = heartsArea.transform;
    hearts[0].transform.localPosition = new Vector3(0, 0, 0);
    hearts[1].transform.localPosition = new Vector3(4, 0, 0);
    hearts[2].transform.localPosition = new Vector3(8, 0, 0);
  }

  // parent - the barrier after which the diamond is created
  void CreateDiamond(Barrier parent, Vector2 pos) {
    Diamond diamond = Instantiate(diamondPrefab, pos, Quaternion.identity) as Diamond;
    diamond.transform.parent = parent.transform;
  }
  void CreateHeart(Barrier parent, Vector2 pos) {
    GameObject heart = Instantiate(heartPrefab, pos, Quaternion.identity) as GameObject;
    heart.transform.parent = parent.transform;
  }

}
