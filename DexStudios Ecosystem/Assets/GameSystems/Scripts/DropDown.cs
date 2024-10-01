using UnityEngine;


public class DropDown : MonoBehaviour
{
    private GameManager _gameManager;

    
    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectsWithTag("Grass").Length > 1) return;
        if (other.gameObject.CompareTag("Wheel") || other.gameObject.CompareTag("Rider"))
        {
            if(_gameManager.Player.wheel.CompareTag("Untagged") || _gameManager.Player.rider.CompareTag("Untagged"))
                return;
            
            other.gameObject.tag = "Untagged";
            _gameManager.HitPlayer("fallen", default);
        }
        else if (other.gameObject.CompareTag("AI Wheels"))
        {
            other.gameObject.tag = "Untagged";
            _gameManager.HitAIPlayer(other.transform.parent.GetComponent<AIPlayer>(), "fallen", default);
        }
    }
}
