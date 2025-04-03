using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder_generator : MonoBehaviour
{
    // Ladder Size that will be adjusted
    [Range(0, 100)]
    public int ladderSize = 0;

    // Ladder Mesh references
    [Tooltip("The lowest part of the ladder")]
    [SerializeField] private Mesh ladderBottomMesh;

    [Tooltip("The mid part of the ladder")]
    [SerializeField] private Mesh ladderMidFirstMesh;

    [Tooltip("The mid part of the ladder")]
    [SerializeField] private Mesh ladderMidSecondMesh;

    [Tooltip("The top part of the ladder")]
    [SerializeField] private Mesh ladderTopMesh;

    [Tooltip("The top part of the ladder")]
    [SerializeField] private Material mLadder;


    [HideInInspector]
     private int _ladderSizeCheck;
     private float ladderMeshSize = 1.5f;

     private int seed = 42;

     private List<Matrix4x4> ladderMatricesB;
     private List<Matrix4x4> ladderMatricesM1;
     private List<Matrix4x4> ladderMatricesM2;
     private List<Matrix4x4> ladderMatricesT;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (sizeChange()){
            createLadder();
        }
        renderLadder();
    }

    bool sizeChange(){
        if (_ladderSizeCheck != ladderSize){
            _ladderSizeCheck = ladderSize;
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

        int ladderCount = Mathf.Max(1, (int)(ladderSize / ladderMeshSize));

        var matB = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        ladderMatricesB.Add(matB);

        var matT = Matrix4x4.TRS(transform.position + new Vector3(0, ladderMeshSize * ladderCount, 0), transform.rotation, new Vector3(1, 1, 1));
        ladderMatricesT.Add(matT);


        for (int i = 0; i < ladderCount; i++){

            var t = transform.position + new Vector3(0, ladderMeshSize * i, 0);
            var r = transform.rotation;
            var s = new Vector3(1, 1, 1);

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
}
