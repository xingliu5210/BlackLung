using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder_generator : MonoBehaviour
{
    // Ladder Size that will be adjusted
    [Range(2, 100)]
    public int ladderSize = 2;

    // Ladder Mesh references
    [Tooltip("The lowest part of the ladder")]
    [SerializeField] private Mesh ladderBottomMesh;

    [Tooltip("The mid part of the ladder")]
    [SerializeField] private Mesh ladderMidFirstMesh;

    [Tooltip("The mid part of the ladder")]
    [SerializeField] private Mesh ladderMidSecondMesh;

    [Tooltip("The top part of the ladder")]
    [SerializeField] private Mesh ladderTopMesh;

    [Tooltip("The material of the ladder")]
    [SerializeField] private Material mLadder;


    [HideInInspector]
     private int _ladderSizeCheck = -1;
     private Vector3 _LadderPosCheck = new Vector3();
     private Quaternion _LadderRotCheck = new Quaternion();
     private Vector3 _LadderScaCheck = new Vector3();


     private float ladderMeshSize = 1.5f;

     private int seed = 42;

     private List<Matrix4x4> ladderMatricesB;
     private List<Matrix4x4> ladderMatricesM1;
     private List<Matrix4x4> ladderMatricesM2;
     private List<Matrix4x4> ladderMatricesT;

     private BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anyChange()){
            createLadder();
            drawBoxCollider();
        }
        renderLadder();
    }
    
    bool anyChange(){
        if (sizeChange() || transformChange()){
            return true;
        }
        return false;
    }

    bool sizeChange(){
        if (_ladderSizeCheck != ladderSize){
            _ladderSizeCheck = ladderSize;
            return true;
        }
        return false;
    }

    bool transformChange(){
        if (_LadderPosCheck != transform.position ||
            _LadderRotCheck != transform.rotation ||
            _LadderScaCheck != transform.localScale){
                _LadderPosCheck = transform.position;
                _LadderRotCheck = transform.rotation;
                _LadderScaCheck = transform.localScale;
                return true;
            }
        return false;
    }

    void createLadder()
    {
        Random.InitState(seed);

        ladderMatricesB = new List<Matrix4x4>();
        ladderMatricesM1 = new List<Matrix4x4>();
        ladderMatricesM2 = new List<Matrix4x4>();
        ladderMatricesT = new List<Matrix4x4>();

        int ladderCount = Mathf.Max(2, ladderSize);
        
        var matB = Matrix4x4.TRS(applyRotation(transform.position), transform.rotation, transform.localScale);
        ladderMatricesB.Add(matB);

        var matT = Matrix4x4.TRS(applyRotation(transform.position + new Vector3(0, ladderMeshSize * ladderCount * transform.localScale.y, 0)), transform.rotation, transform.localScale);
        ladderMatricesT.Add(matT);

        for (int i = 1; i < ladderCount; i++){

            var t = applyRotation(transform.position + new Vector3(0, ladderMeshSize * i * transform.localScale.y, 0));
            var r = transform.rotation;
            var s = transform.localScale;

            var mat = Matrix4x4.TRS(t, r, s);

            var rand = Random.Range(0,2);

            if (rand < 1){
                ladderMatricesM1.Add(mat);
            }
            else{
                ladderMatricesM2.Add(mat);
            }
        }
    }

    void renderLadder(){

        if (ladderMatricesB != null){
            Graphics.DrawMeshInstanced(ladderBottomMesh, 0, mLadder, ladderMatricesB.ToArray(), ladderMatricesB.Count);
        }
        if (ladderMatricesM1 != null){
            Graphics.DrawMeshInstanced(ladderMidFirstMesh, 0, mLadder, ladderMatricesM1.ToArray(), ladderMatricesM1.Count);
        }
        if (ladderMatricesM2 != null){
            Graphics.DrawMeshInstanced(ladderMidSecondMesh, 0, mLadder, ladderMatricesM2.ToArray(), ladderMatricesM2.Count);
        }
        if (ladderMatricesT != null){
            Graphics.DrawMeshInstanced(ladderTopMesh, 0, mLadder, ladderMatricesT.ToArray(), ladderMatricesT.Count);
        }
    }

    void drawBoxCollider(){
        int ladderCount = Mathf.Max(2, ladderSize);
        var bottom = applyRotation(transform.position);
        var top = applyRotation(transform.position + new Vector3(0, ladderMeshSize * (ladderCount + 1)  * transform.localScale.y, 0));

        Vector3 center = (top + bottom) / 2f ;
        
        Vector3 size = new Vector3(4f, Vector3.Distance(top, bottom), 1f);

        size = new Vector3(
            size.x,
            size.y/transform.localScale.y,
            size.z
        );

        boxCollider.center = transform.InverseTransformPoint(center);
        boxCollider.size = size;
    }

    private Vector3 applyRotation(Vector3 t){

        return transform.localRotation * t;

    }
}
