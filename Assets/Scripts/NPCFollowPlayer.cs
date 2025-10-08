using UnityEngine;

public class NPCFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;  // Referência ao jogador
    [SerializeField] private float followSpeed = 3f;  // Velocidade de seguimento
    [SerializeField] private float stoppingDistance = 1f;  // Distância para parar de seguir

    private bool shouldFollow = false;  // Flag para determinar se o NPC deve seguir o jogador

    private void Update()
    {
        if (shouldFollow)
        {
            FollowPlayer();
        }
    }

    // Função para fazer o NPC seguir o jogador
    private void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            // Move o NPC em direção ao jogador
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }

    // Função para começar a seguir o jogador
    public void StartFollowing()
    {
        shouldFollow = true;
    }

    // Função para parar de seguir o jogador
    public void StopFollowing()
    {
        shouldFollow = false;
    }
}
