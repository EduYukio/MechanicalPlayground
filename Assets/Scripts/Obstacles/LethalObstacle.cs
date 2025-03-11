using UnityEngine;

public class LethalObstacle : MonoBehaviour
{
    private string obstacleType;

    private void Start()
    {
        DetermineObstacleType();
        CheckInvulnerability();
    }

    private void OnEnable()
    {
        Mechanics.MechanicChanged += CheckInvulnerability;
    }

    private void OnDisable()
    {
        Mechanics.MechanicChanged -= CheckInvulnerability;
    }

    private void DetermineObstacleType()
    {
        if (gameObject.name.Contains("Spike")) obstacleType = "Spike";
        else if (gameObject.name.Contains("Saw")) obstacleType = "Saw";
    }

    private void CheckInvulnerability()
    {
        GameObject playerObj = GameObject.Find("PlayerFSM");
        if (PlayerIsInvulnerableToObstacle(playerObj))
        {
            gameObject.tag = "Ground";
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ProcessCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject collidedObj)
    {
        if (!collidedObj.CompareTag("Player")) return;
        if (PlayerIsInvulnerableToObstacle(collidedObj)) return;

        KillPlayer(collidedObj);
    }

    private bool PlayerIsInvulnerableToObstacle(GameObject collidedObj)
    {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        string mechanicName = obstacleType + " Invulnerability";
        if (player.mechanics.IsEnabled(mechanicName)) return true;

        return false;
    }

    private void KillPlayer(GameObject collidedObj)
    {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        player.TransitionToState(player.DyingState);
    }
}