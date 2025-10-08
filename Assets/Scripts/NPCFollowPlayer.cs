using UnityEngine;

public class NPCFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;  // Refer�ncia ao jogador
    [SerializeField] private float followSpeed = 3f;  // Velocidade de seguimento
    [SerializeField] private float stoppingDistance = 1f;  // Dist�ncia para parar de seguir

    private bool shouldFollow = false;  // Flag para determinar se o NPC deve seguir o jogador

    private void Update()
    {
        if (shouldFollow)
        {
            FollowPlayer();
        }
    }

    // Fun��o para fazer o NPC seguir o jogador
    private void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            // Move o NPC em dire��o ao jogador
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }

    // Fun��o para come�ar a seguir o jogador
    public void StartFollowing()
    {
        shouldFollow = true;
    }

    // Fun��o para parar de seguir o jogador
    public void StopFollowing()
    {
        shouldFollow = false;
    }
}
