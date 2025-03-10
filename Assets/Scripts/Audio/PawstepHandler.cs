using UnityEngine;
using AK.Wwise;

public class PawstepHandler : MonoBehaviour
{
    private enum CURRENT_TERRAIN { DIRT, METAL, STONE, WOOD };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField] private AK.Wwise.Event walkPawstepsEvent;
    [SerializeField] private AK.Wwise.Event runPawstepsEvent;
    [SerializeField] private AK.Wwise.Event landPawstepsEvent;


    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    private void Update()
    {
        CheckTerrain();
    }

    private void CheckTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                currentTerrain = CURRENT_TERRAIN.DIRT;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Metal"))
            {
                currentTerrain = CURRENT_TERRAIN.METAL;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Stone"))
            {
                currentTerrain = CURRENT_TERRAIN.STONE;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;
            }
        }
    }

    private void PlayPawstepWalk(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(walkPawstepsEvent.Id, this.gameObject);
    }

    private void PlayPawstepRun(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(runPawstepsEvent.Id, this.gameObject);
    }

    private void PlayPawstepLand(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(landPawstepsEvent.Id, this.gameObject);
    }

    public void SelectAndPlayPawtstepWalk()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayPawstepWalk(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayPawstepWalk(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayPawstepWalk(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayPawstepWalk(3);
                break;

            default:
                PlayPawstepWalk(0);
                break;
        }
    }

    public void SelectAndPlayPawstepRun()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayPawstepRun(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayPawstepRun(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayPawstepRun(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayPawstepRun(3);
                break;

            default:
                PlayPawstepRun(0);
                break;
        }
    }

    public void SelectAndPlayPawstepLand()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayPawstepLand(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayPawstepLand(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayPawstepLand(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayPawstepLand(3);
                break;

            default:
                PlayPawstepLand(0);
                break;
        }
    }

}
