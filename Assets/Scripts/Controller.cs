using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	private bool isGrounded;
	private float radius=0.9f;
	private float force=320;
	public Transform coinsParent;
	public Transform groundPoint;
	public LayerMask ground;
	public AudioSource audioSource;
	public AudioClip jump;
	public AudioClip coin;
	public AudioClip win;
	public Animator animator;
	public int coins;
	public TextMesh score;
	private Rigidbody2D rigidbody;
	bool jumped;
	float jumpTime=0;
	float jumpDelay=0.5f;
	public Game gameComponent;//Referencia a Game

	void Start(){
		rigidbody = GetComponent<Rigidbody2D> (); 
		animator = GetComponent<Animator> ();
	}

	void Update () {


		isGrounded = Physics2D.OverlapCircle (groundPoint.position, radius, ground);

		//Debug.Log (isGrounded);

		if (isGrounded) {
			if(Input.GetKeyDown(KeyCode.Space)){
				rigidbody.AddForce(Vector2.up * force);
				audioSource.clip= jump;
				audioSource.Play();

				//Animacion Salto
				//Debug.Log("jump");
				jumpTime=jumpDelay;
				jumped=true;
				animator.SetTrigger("jumped");
			}
		}

		jumpTime -= Time.deltaTime;
		//Debug.Log (isGrounded);
		if(jumpTime<=0 && isGrounded && jumped){
			//Debug.Log("land");
			jumped=false;
			animator.SetTrigger("landed");
		}

	}

	void OnTriggerEnter2D (Collider2D collider2d){

		if (collider2d.tag == "Coin") {
			coins++;
			score.text = coins + "";
			Destroy (collider2d.gameObject);
			audioSource.clip = coin;
			audioSource.Play ();

		}else if(collider2d.tag=="ComeGotiko"){
			Debug.Log("Comido");


		}else if(collider2d.tag=="DeadZone"){

			//Abrimos la ventana de derrota
			Game.shownDialogType=Game.DialogType.LOSE;
			gameComponent.OnGameLose(Game.LoseReason.WRONG_CHOICE);

		}else if(collider2d.tag=="Finish"){
			//rigidbody.isKinematic=true;
			audioSource.clip=win;
			audioSource.Play();

			//Abrimos la ventana de Victoria
			Game.winStarsCount=3; //Ponemos 3 estrellas de manera provisional
			Game.shownDialogType=Game.DialogType.WIN;
			gameComponent.OnGameWin();
		}

	}




}
