using UnityEngine;

public class ControllPlayer : MonoBehaviour
{
    public GameObject[] Players;
    [SerializeField]
    GameObject CurrentPlayer;

    // Use this for initialization
    void Start () {
        for (int i = 1; i < Players.Length; i++)
        {
            Players[i].GetComponent<PlayerMovement>().enabled = false;
        }

        CurrentPlayer = Players[0];
    }

    public void ChangePlayer(GameObject Player)
    {
        CurrentPlayer.GetComponent<PlayerMovement>().enabled = false;
        CurrentPlayer = Player;
    }
}
