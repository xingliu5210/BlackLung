using UnityEngine;

public class Player : MonoBehaviour
{
    public ControllPlayer controllerplayer;

    void OnMouseDown()
    {
        controllerplayer.ChangePlayer(this.gameObject);
        GetComponent<PlayerMovement>().enabled = true;
    }
}
