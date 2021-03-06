using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DialogueEditor;

public class Frog : MonoBehaviour
{
    NPC_Controller m_NPC_Controller;
    Animator m_Animator;
    SpriteRenderer m_SpriteRenderer;

    public UnityEvent CrockEvent;

    [SerializeField]
    Color m_color;

    private void Awake()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        m_NPC_Controller = GetComponent<NPC_Controller>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.material.color = m_color;

        StartCoroutine(Roaming());
    }

    public float waitInterval = 1f;

    IEnumerator Roaming()
    {
        while (true)
        {
            int nextMove = Random.Range(0, 3) - 1;
            bool jump = (nextMove != 0) ? true : false;
            if(jump)
                m_NPC_Controller.Movement(nextMove, jump);
            else
            {
                int nextCrock = Random.Range(0, 3);
                if (nextCrock > 1) CrockEvent.Invoke();
            } 
            yield return new WaitForSeconds(waitInterval);
        }
    }

    public void Jump()
    {
        m_Animator.SetTrigger("jump");
    }

    public void Crock()
    {
        m_Animator.SetTrigger("crock");
    }

    public void SetColor(Color color)
    {
        m_SpriteRenderer.material.color = color;
    }

    public void Flip(bool flip)
    {
        m_SpriteRenderer.flipX = flip;
    }

}
