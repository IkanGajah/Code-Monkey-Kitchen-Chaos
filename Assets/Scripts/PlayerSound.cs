using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = .1f;
    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f) {
            footstepTimer = footstepTimerMax;
            if(player.IsWalking() || player.IsRunning()) { 
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, 1f);
            }
        }
    }
}
