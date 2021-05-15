using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    [SerializeField] GameObject m_FeedbackEffect;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SetScore();
        }
    }

    void SetScore()
    {
        ScoreManager.GetCherry();
        Instantiate(m_FeedbackEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
