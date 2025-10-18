using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Sit Timing")]
    [SerializeField] private float timeToSitMin = 3f;
    [SerializeField] private float timeToSitMax = 5f;

    [Header("LieDown Timing")]
    [SerializeField] private float timeToLieDownMin = 10f;
    [SerializeField] private float timeToLieDownMax = 15f;

    [SerializeField] private bool canLieDown = true;

    [Header("Referencia do Dialogue Manager")]
    [SerializeField] private InkDialogueManager dialogueManager;

    private float idleTimer = 0f;
    private float currentTimeToSit;
    private float currentTimeToLieDown;

    private int currentIdlePhase = 0;
    private bool isMoving = false;

    private PlayerMove playerMove;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

        if (animator == null)
            animator = GetComponent<Animator>();

        GenerateRandomIdleTimes();
    }

    private void Update()
    {
        Vector2 currentInput = moveAction.ReadValue<Vector2>();
        isMoving = playerMove.canMove && currentInput.magnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        bool dialogueIsActive = dialogueManager != null && dialogueManager.IsDialogueActive();

        if (dialogueIsActive)
        {
            ResetIdle();
            animator.SetBool("CanLieDown", false);
            return;
        }

        animator.SetBool("CanLieDown", canLieDown);

        if (isMoving)
        {
            ResetIdle();
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= currentTimeToLieDown && currentIdlePhase < 3 && canLieDown)
            {
                currentIdlePhase = 3;
                animator.SetInteger("IdlePhase", 3);
            }
            else if (idleTimer >= currentTimeToSit && currentIdlePhase < 1)
            {
                currentIdlePhase = 1;
                animator.SetInteger("IdlePhase", 1);
            }
        }
    }

    private void ResetIdle()
    {
        idleTimer = 0f;
        currentIdlePhase = 0;

        GenerateRandomIdleTimes();

        animator.SetInteger("IdlePhase", 0);
        animator.ResetTrigger("ResetIdle");
    }

    private void GenerateRandomIdleTimes()
    {
        currentTimeToSit = Random.Range(timeToSitMin, timeToSitMax);
        currentTimeToLieDown = Random.Range(timeToLieDownMin, timeToLieDownMax);
    }

    public void SetCanLieDown(bool value)
    {
        canLieDown = value;
        animator.SetBool("CanLieDown", value);
    }
}