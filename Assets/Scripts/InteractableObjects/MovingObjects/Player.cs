﻿using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	public AudioClip moveSound1, moveSound2, dieSound, attackEnemySound, attackWallSound, attackRubbleSound1, attackRubbleSound2;

	private int maxHealth = 200;
	private Animator animator;
	private Vector2 touchOrigin = -Vector2.one;
	private bool isMoving;

	protected override void Start () {
		animator = GetComponent<Animator> ();
		base.Start ();
    }	

	protected override void MoveOrInteract(int xDir, int yDir){
		RaycastHit2D hit = CheckForCollision (xDir, yDir);
		UpdateSpriteDirection (xDir);

		if (hit.transform == null) {
			Move (xDir, yDir);
			return;
		}

		InteractableObject hitObject = hit.transform.GetComponent (typeof(InteractableObject)) as InteractableObject;

		if (hitObject != null )
			Interact(hitObject);
	}
		
	private void OnTriggerEnter2D (Collider2D other){
		InteractableObject steppedObject = other.transform.GetComponent (typeof(InteractableObject)) as InteractableObject;
		steppedObject.SteppedOn();
	}

	protected override void Interact(InteractableObject hitObject){
		animator.SetTrigger ("playerAttack");
		base.Interact (hitObject);
	}

	public override void Hit(int damage){
		animator.SetTrigger ("playerHit");
		base.Hit(damage);
	}

	public override void ChangeHealth (int change){		
		if (health <= 0)
			return;
		health += change;
		if (health > maxHealth)
			health = maxHealth;
	}

	protected override void Move(int xDir, int yDir){
		Vector3 end = transform.position + new Vector3(xDir, yDir, 0);
		StartCoroutine (SmoothMovement (end));
	}

	protected IEnumerator SmoothMovement(Vector3 end){
		if (isMoving)
			yield break;
		isMoving = true;
		if (health > 100)
			ChangeHealth (-2);
		SoundManager.instance.PlaySoundEffectWithRandomPitch(efxSource, moveSound1, moveSound2);
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > 0) {
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, 1f / moveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
		isMoving = false;
	}
}
