using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    private Mesh _mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private int frameCount;
    private Vector3 previousTipPosition;
    private Vector3 previousBasePosition;
    
    void Start()
    {
        //Init mesh and triangles
        _meshParent.transform.position = Vector3.zero;
        _mesh = new Mesh();
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;

        Material trailMaterial = Instantiate(_meshParent.GetComponent<MeshRenderer>().sharedMaterial);
        trailMaterial.SetColor("Color_8F0C0815", _colour);
        _meshParent.GetComponent<MeshRenderer>().sharedMaterial = trailMaterial;

        Material bladeMaterial = Instantiate(blade.GetComponent<MeshRenderer>().sharedMaterial);
        bladeMaterial.SetColor("Color_AF2E1BB", _colour);
        blade.GetComponent<MeshRenderer>().sharedMaterial = bladeMaterial;

        vertices = new Vector3[_trailFrameLength * NUMvertices];
        triangles = new int[vertices.Length];

        //Set starting position for tip and base
        previousTipPosition = tipTransform.position;
        previousBasePosition = baseTransform.position;
    }
    
    void LateUpdate()
    {
        //Reset the frame count one we reach the frame length
        if(frameCount == (_trailFrameLength * NUMvertices))
        {
            frameCount = 0;
        }

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
        _meshParent.transform.position = transform.position / 7;
    }
}
