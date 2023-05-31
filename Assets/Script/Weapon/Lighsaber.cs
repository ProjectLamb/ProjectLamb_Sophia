using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lighsaber : MonoBehaviour
{
    //https://github.com/Tvtig/UnityLightsaber
    //The number of vertices to create per frame
    private const int NUMvertices = 12;

    [SerializeField]
    [Tooltip("무기 모델")]
    private GameObject blade = null;
     
    [SerializeField]
    [Tooltip("트레일 생길 꼭데기")]
    private Transform tipTransform = null;

    [SerializeField]
    [Tooltip("트레일 생길 아래")]
    private Transform baseTransform = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject   meshParent = null;
    private MeshFilter   meshFilter = null;
    private MeshRenderer meshRenderer = null;

    [SerializeField]
<<<<<<< HEAD
    [Tooltip("얼마나 긴 프레임동안 유지될것인가?")]
    private int trailFrameLength = 3;
=======
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;
>>>>>>> TA_Escatrgot_AffectorManager

    private Mesh _mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 previousTipPosition;
    private Vector3 previousBasePosition;
    public bool IsActivate = false;

    public UnityAction drawOn;
    public UnityAction drawOff;
    

    void Awake()
    {
        _mesh = new Mesh();
        meshParent.TryGetComponent<MeshFilter>(out meshFilter);
        meshParent.TryGetComponent<MeshRenderer>(out meshRenderer);
        meshFilter.mesh = _mesh;

<<<<<<< HEAD
        vertices = new Vector3[trailFrameLength * NUMvertices];
=======
        vertices = new Vector3[_trailFrameLength * NUMvertices];
>>>>>>> TA_Escatrgot_AffectorManager
        triangles = new int[vertices.Length];
    }
    private void Start() {
        //Set starting position for tip and base
        previousTipPosition = tipTransform.position;
        previousBasePosition = baseTransform.position;
        drawOn = () => {
            IsActivate = true;
            meshFilter.mesh = _mesh;
        };
        drawOff = () => {
            IsActivate = false;
            meshParent = null;
            _mesh = new Mesh();
        };
    }
    
    void LateUpdate()
    {
        if(!IsActivate) return;
        //Reset the frame count one we reach the frame length
        if(frameCount == (trailFrameLength * NUMvertices)) { frameCount = 0; }

        //Draw first triangle vertices for back and front
        vertices[frameCount] = baseTransform.position;
        vertices[frameCount + 1] = tipTransform.position;
        vertices[frameCount + 2] = previousTipPosition;
        vertices[frameCount + 3] = baseTransform.position;
        vertices[frameCount + 4] = previousTipPosition;
        vertices[frameCount + 5] = tipTransform.position;

        //Draw fill in triangle vertices
        vertices[frameCount + 6] = previousTipPosition;
        vertices[frameCount + 7] = baseTransform.position;
        vertices[frameCount + 8] = previousBasePosition;
        vertices[frameCount + 9] = previousTipPosition;
        vertices[frameCount + 10] = previousBasePosition;
        vertices[frameCount + 11] = baseTransform.position;

        //Set triangles
        triangles[frameCount] = frameCount;
        triangles[frameCount + 1] = frameCount + 1;
        triangles[frameCount + 2] = frameCount + 2;
        triangles[frameCount + 3] = frameCount + 3;
        triangles[frameCount + 4] = frameCount + 4;
        triangles[frameCount + 5] = frameCount + 5;
        triangles[frameCount + 6] = frameCount + 6;
        triangles[frameCount + 7] = frameCount + 7;
        triangles[frameCount + 8] = frameCount + 8;
        triangles[frameCount + 9] = frameCount + 9;
        triangles[frameCount + 10] = frameCount + 10;
        triangles[frameCount + 11] = frameCount + 11;

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;

        //Track the previous base and tip positions for the next frame
        previousTipPosition = tipTransform.position;
        previousBasePosition = baseTransform.position;
        frameCount += NUMvertices;
    }
}
