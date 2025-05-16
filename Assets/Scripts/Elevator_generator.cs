using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Elevator_generator : MonoBehaviour
{
    // Inspector manipulators
    [Range(24f, 200)]
    public List<float> elevatorSizes = new List<float>(){24f, 48f};

    [Tooltip("The floor of the Elevator")]
    [SerializeField] private GameObject elevatorBox;

    [Range(0,100)]
    public int ElevatorPosition = 0;

    [Tooltip("The floor of the Elevator")]
    [SerializeField] private Mesh elevatorFloorMesh;

    [Tooltip("The base of the Elevator")]
    [SerializeField] private Mesh elevatorBaseMesh;

    [Tooltip("The top of the Elevator")]
    [SerializeField] private Mesh elevatorTopMesh;

    [Tooltip("The tunnel of the Elevator")]
    [SerializeField] private Mesh elevatorTunnelMesh_01;

    [Tooltip("The tunnel of the Elevator")]
    [SerializeField] private Mesh elevatorTunnelMesh_02;

    [Tooltip("The tunnel of the Elevator")]
    [SerializeField] private Mesh elevatorTunnelMesh_03;

    [Tooltip("The material of the Elevator Floor")]
    [SerializeField] private Material mElevatorFloor;

    [Tooltip("The material of the Elevator Base")]
    [SerializeField] private Material mElevatorBase;

    [Tooltip("The material of the Elevator Top")]
    [SerializeField] private Material mElevatorTop;

    [Tooltip("The material of the Elevator Tunnel")]
    [SerializeField] private Material mElevatorTunnel_01;

    [Tooltip("The material of the Elevator Tunnel")]
    [SerializeField] private Material mElevatorTunnel_02;

    [Tooltip("The material of the Elevator Tunnel")]
    [SerializeField] private Material mElevatorTunnel_03;

    [SerializeField] private ElevatorManager manager;
    [SerializeField] private List<Transform> elevatorLocations;
    [SerializeField] private InteractionZone interactionZonePrefab;
    //[SerializeField] private Collider[] interactionColliders;
    //[SerializeField] private Transform[] loadPlayerPositions;

    // Hidden Variables from the inspector
    [HideInInspector]
    private List<float> _ElevatorSizesCheck = null;
    private Vector3 _ElevatorPosCheck = new Vector3();
    private Quaternion _ElevatorRotCheck = new Quaternion();
    private Vector3 _ElevatorScaleCheck = new Vector3();

    private float elevatorFloorSize = 1.5f;
    private float elevatorBaseSize = 22.5f;
    private float elevatorTopSize = 35f;
    private float elevatorTunnelSize = 20f;

    private int seed = 123;
    private int currentPosition;
    private bool moveable = true;
    private Vector3 elevatorVelocity = Vector3.zero;

    private List<Matrix4x4> ElevatorMatricesF;
    private List<Matrix4x4> ElevatorMatricesB;
    private List<Matrix4x4> ElevatorMatricesT;
    private List<Matrix4x4> ElevatorMatricesT1;
    private List<Matrix4x4> ElevatorMatricesT2;
    private List<Matrix4x4> ElevatorMatricesT3;

    // Start is called before the first frame update
    void Start()
    {
        createElevator();

        elevatorBox.transform.position = ElevatorMatricesF[0].GetPosition() + new Vector3(0, 1.5f * transform.localScale.y, 0);
        currentPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (positionChange()){
            var newPosition = ElevatorMatricesF[ElevatorPosition].GetPosition() + new Vector3(0, 1.5f * transform.localScale.y, 0);
            elevatorBox.transform.position = Vector3.SmoothDamp(elevatorBox.transform.position, newPosition, ref elevatorVelocity, 2f, 100f * transform.localScale.y);
            if (positionMoved()){
                Debug.Log("Done");
                currentPosition = ElevatorPosition;
            }
        }
        
        if (anyChange()){
            createElevator();
            placeLocators();
            PlaceInteractionColliders();

            elevatorBox.transform.position = ElevatorMatricesF[ElevatorPosition].GetPosition() + new Vector3(0, 1.5f * transform.localScale.y, 0);
        }
        renderElevator();
    }


    bool anyChange(){
        if (sizeChange() || transformChange()){
            return true;
        }
        return false;
    }

    bool sizeChange(){
        if (_ElevatorSizesCheck == null){
            _ElevatorSizesCheck = new List<float>(elevatorSizes);
            return true;
        }
        else if (_ElevatorSizesCheck.Count != elevatorSizes.Count){
            _ElevatorSizesCheck = new List<float>(elevatorSizes);
            return true;
        }

        for (int i = 0; i < _ElevatorSizesCheck.Count; i++) {
            if (_ElevatorSizesCheck[i] != elevatorSizes[i]){
                _ElevatorSizesCheck = new List<float>(elevatorSizes);
                return true;
            }
        }        
        return false;
    }

    bool transformChange(){
        if (_ElevatorPosCheck != transform.position ||
            _ElevatorRotCheck != transform.rotation ||
            _ElevatorScaleCheck != transform.localScale){
                _ElevatorPosCheck = transform.position;
                _ElevatorRotCheck = transform.rotation;
                _ElevatorScaleCheck = transform.localScale;
                return true;
            }
        return false;
    }

    bool positionMoved(){
        var newPosition = ElevatorMatricesF[ElevatorPosition].GetPosition() + new Vector3(0, 1.5f * transform.localScale.y, 0);

        if (Vector3.Distance(elevatorBox.transform.position, newPosition) < 0.1){
            return true;
        }
        return false;
    }

    bool positionChange(){
        if (currentPosition != ElevatorPosition && (ElevatorPosition < elevatorSizes.Count) && (ElevatorPosition >= 0)){
            return true;
        }

        return false;

    }

    void createElevator(){

        Random.InitState(seed);

        var offset = new Vector3(0, 0, 0);

        ElevatorMatricesF = new List<Matrix4x4>();
        ElevatorMatricesB = new List<Matrix4x4>();
        ElevatorMatricesT = new List<Matrix4x4>();
        ElevatorMatricesT1 = new List<Matrix4x4>();
        ElevatorMatricesT2 = new List<Matrix4x4>();
        ElevatorMatricesT3 = new List<Matrix4x4>();

        for (int i = 0; i < elevatorSizes.Count; i++){

            var tunnelCount = Mathf.Max(1, (int)(elevatorSizes[i]/elevatorTunnelSize));
            var baseScale = (elevatorSizes[i]/tunnelCount) / elevatorBaseSize;

            if (i != 0){
                offset += new Vector3(0, -elevatorSizes[i], 0);
            }

            var currentPosition = transform.position + offset;
            
            var matF = Matrix4x4.TRS(Vector3.Scale(currentPosition, new Vector3(1, transform.localScale.y, 1)), transform.rotation, transform.localScale);
            ElevatorMatricesF.Add(matF);

            var matB = Matrix4x4.TRS(Vector3.Scale(currentPosition + new Vector3(0, elevatorFloorSize, 0), new Vector3(1, transform.localScale.y, 1)), transform.rotation, Vector3.Scale(transform.localScale, new Vector3(1, baseScale, 1)));
            ElevatorMatricesB.Add(matB);

            var tunnelScale = (elevatorSizes[i]/tunnelCount) / elevatorTunnelSize;

            if (i == 0){
                var t = currentPosition + new Vector3(0, (elevatorFloorSize + (elevatorBaseSize * baseScale)), 0) + new Vector3(0,-elevatorSizes[i]/2 + elevatorTunnelSize * tunnelScale * (1 + 0.5f * (tunnelCount - 2)) + (tunnelCount - 1) * tunnelScale * elevatorTunnelSize,0);
                var matTop = Matrix4x4.TRS(Vector3.Scale(t, new Vector3(1, transform.localScale.y, 1)), transform.rotation, transform.localScale);
                ElevatorMatricesT.Add(matTop);
            }

            for (int j = 0; j < tunnelCount - 1; j++){

                var t = currentPosition + new Vector3(0, (elevatorFloorSize + (elevatorBaseSize * baseScale)), 0) + new Vector3(0, -elevatorSizes[i]/2 + elevatorTunnelSize * tunnelScale * (1 + 0.5f * (Mathf.Max(0, tunnelCount - 2))) + j * tunnelScale * elevatorTunnelSize, 0);
                var matT = Matrix4x4.TRS(Vector3.Scale(t, new Vector3(1, transform.localScale.y, 1)), transform.rotation, Vector3.Scale(transform.localScale, new Vector3(1, tunnelScale, 1)));

                var rand = Random.Range(0,3);

                if (rand < 1){
                    ElevatorMatricesT1.Add(matT);
                } else if (rand < 2){
                    ElevatorMatricesT2.Add(matT);
                } else{
                    ElevatorMatricesT3.Add(matT);
                }

            }
        }


    }

    void renderElevator(){

        if (ElevatorMatricesF != null){
            Graphics.DrawMeshInstanced(elevatorFloorMesh, 0, mElevatorFloor, ElevatorMatricesF.ToArray(), ElevatorMatricesF.Count);
        }

        if (ElevatorMatricesB != null){
            Graphics.DrawMeshInstanced(elevatorBaseMesh, 0, mElevatorBase, ElevatorMatricesB.ToArray(), ElevatorMatricesB.Count);
        }

        if (ElevatorMatricesT != null){
            Graphics.DrawMeshInstanced(elevatorTopMesh, 0, mElevatorTop, ElevatorMatricesT.ToArray(), ElevatorMatricesT.Count);
        }

        if (ElevatorMatricesT1 != null){
            Graphics.DrawMeshInstanced(elevatorTunnelMesh_01, 0, mElevatorTunnel_01, ElevatorMatricesT1.ToArray(), ElevatorMatricesT1.Count);
        }

        if (ElevatorMatricesT2 != null){
            Graphics.DrawMeshInstanced(elevatorTunnelMesh_02, 0, mElevatorTunnel_02, ElevatorMatricesT2.ToArray(), ElevatorMatricesT2.Count);
        }

        if (ElevatorMatricesT3 != null){
            Graphics.DrawMeshInstanced(elevatorTunnelMesh_03, 0, mElevatorTunnel_03, ElevatorMatricesT3.ToArray(), ElevatorMatricesT3.Count);
        }

    }

    void placeLocators(){

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Locator"){
                Destroy(child.gameObject);
            }
        }

        ////Remove old values from list
        for (int i = elevatorLocations.Count - 1; i >= 0; i--)
        {
            elevatorLocations.RemoveAt(i);
        }

        manager.ResetPositionLists();

        int numberOfLoc = elevatorSizes.Count;
        
        for (int i = 0; i < numberOfLoc; i += 1 ){
            GameObject child = new GameObject($"elevator_loc_{i}");
            child.tag = "Locator";
            child.transform.SetParent(transform);
            child.transform.position = ElevatorMatricesF[i].GetPosition();

            //Add onBoardPositions
            GameObject onBoardPosition = new GameObject($"onBoardPosition{i}");
            onBoardPosition.transform.position = child.transform.position + new Vector3(0, 1f, 0);
            manager.AddOnBoardPosition(onBoardPosition.transform);

            //Add offLoadPositions
            GameObject offLoadPosition = new GameObject($"offLoadPosition{i}");
            child.transform.position = ElevatorMatricesF[i].GetPosition() + new Vector3(0, 0, -3f);
            offLoadPosition.transform.position = child.transform.position + new Vector3(0, 1f, 0);
            manager.AddOffLoadPosition(offLoadPosition.transform);

            child.transform.position = ElevatorMatricesF[i].GetPosition() + new Vector3(0, 0, -6f); // Space them out
            elevatorLocations.Add(child.transform);
        }
    }

    private void PlaceInteractionColliders()
    {
        manager.ResetInteractionColliderList();

        foreach (Transform location in elevatorLocations)
        {
            Vector3 position = location.position;
            position.z = 0;
            position.y += 1;
            Debug.Log(position);
            InteractionZone zone = Instantiate(interactionZonePrefab, position, Quaternion.identity);
            manager.AddInteractionCollider(zone);
        }
    }

    public GameObject GetElevatorBox()
    {
        return elevatorBox;
    }

    public int GetElevatorPosition()
    {
        return currentPosition;
    }



}
