using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGamePlayerController : BaseController
{
    [Range(5f, 15f)][SerializeField] private float moveForce = 10f;
    [Range(0f, 10f)][SerializeField] private float drag = 3f;
    [Range(5f, 15f)][SerializeField] private float speed = 8f;

    [Header("VFXs")]
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Transform particlePivot;

    private Camera cam;
    private GameManager gameManager;
    private TrailRenderer trailRenderer;
    private GameObject particle;
    private ParticleSystem implementedParticleSystem;

    /// <summary>
    /// Initialize Player Character
    /// </summary>
    /// <param name="gameManager"></param>
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        cam = Camera.main;
    }

    /// <summary>
    /// Awake is called once when scripts being loaded
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        trailRenderer = Helper.GetComponentInChildren_Helper<TrailRenderer>(gameObject, true);
    }

    /// <summary>
    /// Start is called once after scripts are loaded
    /// </summary>
    protected override void Start()
    {
        rigidBody.drag = drag;
        statHandler.Speed = speed;
        if(particlePrefab != null) 
        { 
            particle = Instantiate(particlePrefab, particlePivot); 
            implementedParticleSystem = Helper.GetComponent_Helper<ParticleSystem>(particle); 
        }
        ChangeCharacter(GlobalGameManager.Instance.GetCurrentCharacterData());
    }

    /// <summary>
    /// Update is called every frames per secs
    /// </summary>
    protected override void Update()
    {
        if (!gameManager.IsGameActive) { return; }

        base.Update();
    }

    /// <summary>
    /// FixedUpdate is called every fixed time
    /// </summary>
    protected override void FixedUpdate()
    {
        if (!gameManager.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        Movement(movementDirection);
    }
    
    /// <summary>
    /// Called when player character is dead
    /// </summary>
    public override void Die()
    {
        rigidBody.gravityScale = 1f;
        if(implementedParticleSystem != null) implementedParticleSystem.Stop();
        base.Die();
        gameManager.GameOver();
    }

    /// <summary>
    /// Calculates character movement behavior
    /// </summary>
    /// <param name="direction"></param>
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
    
    /// <summary>
    /// Changes character data(Animation, Sprite, Particle Color)
    /// </summary>
    /// <param name="characterData"></param>
    private void ChangeCharacter(CharacterData characterData)
    {
        this.characterData = characterData;

        // Change Character base sprite and trail renderer's color
        characterRenderer.sprite = characterData.characterSprite;
        trailRenderer.startColor = new Color(characterData.trailColor.r, characterData.trailColor.g, characterData.trailColor.b, 1);
        trailRenderer.endColor = new Color(1, 1, 1, 0);
        
        // Change Animation Clip of player character
        animationHandler.ChangeAnimationClips(characterData);

        // Change Particle color and play
        ChangeParticleColor(characterData);
        if(implementedParticleSystem != null) implementedParticleSystem.Play();
    }

    /// <summary>
    /// Changes particle color of character
    /// </summary>
    /// <param name="characterData"></param>
    private void ChangeParticleColor(CharacterData characterData)
    {
        var col = implementedParticleSystem.colorOverLifetime;
        col.enabled = true;

        Gradient gradient = new();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(characterData.trailColor, 0f), new GradientColorKey(Color.white, 1f) },
                         new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 0f) });
        col.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    #region Handles Player Input
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
    #endregion
}
