using UnityEngine;
using System;
using System.Reflection;

public class InteractNPC : MonoBehaviour
{
    // Flag que indica se o jogador está dentro do trigger
    private bool playerInRange = false;

    // O nome do botão de interação (pode ser alterado nas configurações de Input do Unity)
    public string interactButton = "Interact"; // Por padrão, mapeado como "E" ou o botão configurado

    // Referência ao script que contém o método a ser executado
    public MonoBehaviour targetScript; // Script onde a função está localizada

    // Nome do método a ser chamado
    public string methodName = "TriggerAction"; // Método a ser chamado no script especificado

    // Função chamada quando o jogador entra na área de colisão
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // O jogador entrou na área de interação
            ShowInteractionPrompt(true); // Opcional: exibe o prompt de interação
        }
    }

    // Função chamada quando o jogador sai da área de colisão
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // O jogador saiu da área de interação
            ShowInteractionPrompt(false); // Opcional: oculta o prompt de interação
        }
    }

    // Função que verifica o pressionamento do botão de interação
    void Update()
    {
        // Se o jogador está dentro da área e apertou o botão de interação
        if (playerInRange && Input.GetButtonDown(interactButton))
        {
            TriggerAction();
        }
    }

    // Função que chama o método de outro script, usando Reflection
    private void TriggerAction()
    {
        if (targetScript != null && !string.IsNullOrEmpty(methodName))
        {
            // Obtém o tipo do script onde o método está localizado
            Type scriptType = targetScript.GetType();

            // Obtém o método especificado pelo nome
            MethodInfo method = scriptType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                // Chama o método encontrado (sem parâmetros, mas pode ser modificado para suportar parâmetros)
                method.Invoke(targetScript, null);
                Debug.Log($"Método {methodName} executado no script {targetScript.GetType().Name}.");
            }
            else
            {
                Debug.LogWarning($"Método {methodName} não encontrado no script {targetScript.GetType().Name}.");
            }
        }
        else
        {
            Debug.LogWarning("Nenhum script ou método foi atribuído corretamente.");
        }
    }

    // Função opcional para mostrar ou ocultar o prompt de interação
    private void ShowInteractionPrompt(bool show)
    {
        // Aqui você pode ativar/desativar o prompt de interação na UI ou no mundo.
        // Exemplo: Exibir um ícone de "pressione E para interagir" ou algo semelhante.
        // Isso pode ser feito com uma UI de texto, ícone, etc.
        // Exemplo (não implementado no código):
        // interactionPrompt.SetActive(show);
    }
}
