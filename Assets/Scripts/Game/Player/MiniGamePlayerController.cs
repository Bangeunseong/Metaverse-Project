using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGamePlayerController : BaseController
{
    [Range(5f, 15f)][SerializeField] private float moveForce = 10f;
    [Range(0f, 10f)][SerializeField] private float drag = 3f;
    [Range(5f, 15f)][SerializeField] private float speed = 8f;

    private Camera cam;
    private GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        cam = Camera.main;
    }

    protected override void Start()
    {
        rigidBody.drag = drag;
        statHandler.Speed = speed;
        
        ChangeCharacter(GlobalGameManager.Instance.GetCurrentCharacterData());
    }

    public void ChangeCharacter(CharacterData characterData)
    {
        this.characterData = characterData;
        characterRenderer.sprite = characterData.characterSprite;
        animationHandler.ChangeAnimationClips(characterData);
    }

    protected override void Update()
    {
        if (!gameManager.IsGameActive) { return; }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (!gameManager.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        Movement(movementDirection);
    }

    private void Movement(Vector2 direction)
    {
        if(rigidBody.velocity.magnitude > statHandler.Speed)
        {
            rigidBody.velocity *= (statHandler.Speed / rigidBody.velocity.magnitude);    
        }

        if (direction.magnitude > 0)
        {
            rigidBody.AddForce(direction.normalized * moveForce, ForceMode2D.Force);
            float dot = Vector3.Dot(Vector3.right, direction);

            rigidBody.drag = dot >= 0 ? drag : 0;
        }
        else rigidBody.drag = drag;
        
    }

    public override void Die()
    {
        base.Die();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>().normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);

        lookAtDirection = (worldPos - (Vector2)transform.position);

        if (lookAtDirection.magnitude < .9f) { lookAtDirection = Vector2.zero; }
        else { lookAtDirection = lookAtDirection.normalized; }
    }

    void OnFire(InputValue inputValue)
    {
        isAttacking = inputValue.Get<float>() > 0;
    }
}
