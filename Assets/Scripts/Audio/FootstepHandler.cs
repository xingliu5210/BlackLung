using UnityEngine;
using AK.Wwise;

public class FootstepHandler : MonoBehaviour
{
    private enum CURRENT_TERRAIN { DIRT, METAL, STONE, WOOD };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField] private AK.Wwise.Event walkFootstepsEvent;
    [SerializeField] private AK.Wwise.Event runFootstepsEvent;
    [SerializeField] private AK.Wwise.Event landFootstepsEvent;
    [SerializeField] private AK.Wwise.Event climbFootstepsEvent;
    [SerializeField] private AK.Wwise.Event walkClothsEvent;
    [SerializeField] private AK.Wwise.Event runClothsEvent;
    [SerializeField] private AK.Wwise.Event landClothsEvent;

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

    private void PlayFootstepWalk(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(walkClothsEvent.Id, this.gameObject);
        AkSoundEngine.PostEvent(walkFootstepsEvent.Id, this.gameObject);
    }

    private void PlayFootstepRun(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(runClothsEvent.Id, this.gameObject);
        AkSoundEngine.PostEvent(runFootstepsEvent.Id, this.gameObject);
    }

    private void PlayFootstepClimb(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(climbFootstepsEvent.Id, this.gameObject);
    }

    private void PlayFootstepLand(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(landClothsEvent.Id, this.gameObject);
        AkSoundEngine.PostEvent(landFootstepsEvent.Id, this.gameObject);
    }

    public void SelectAndPlayFootstepWalk()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayFootstepWalk(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayFootstepWalk(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayFootstepWalk(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayFootstepWalk(3);
                break;

            default:
                PlayFootstepWalk(0);
                break;
        }
    }

    public void SelectAndPlayFootstepRun()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayFootstepRun(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayFootstepRun(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayFootstepRun(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayFootstepRun(3);
                break;

            default:
                PlayFootstepRun(0);
                break;
        }
    }

    public void SelectAndPlayFootstepLand()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayFootstepLand(0);
                break;

            case CURRENT_TERRAIN.METAL:
                PlayFootstepLand(1);
                break;

            case CURRENT_TERRAIN.STONE:
                PlayFootstepLand(2);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayFootstepLand(3);
                break;

            default:
                PlayFootstepLand(0);
                break;
        }
    }

}
