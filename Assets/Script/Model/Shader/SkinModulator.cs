using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinModulator : MonoBehaviour
{
    [SerializeField]
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public MeshRenderer[] meshRenderers;
    
    [SerializeField]//인덱스는 부위각각이다. 
    public List<Material> mSkinMaterials = new List<Material>();
    public Material TransMaterial;

    public enum RendererMode { skin, mesh } 
    public RendererMode mode;

    [ContextMenu("Awake")]
    private void Awake() {
        if(mode == RendererMode.skin) {
            if(mSkinMaterials.Count == 0) mSkinMaterials = skinnedMeshRenderers[0].materials.ToList();
            for(int j = 0; j < skinnedMeshRenderers.Length; j++){
                skinnedMeshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }
        }
        if(mode == RendererMode.mesh) {
            if(mSkinMaterials.Count == 0) mSkinMaterials = meshRenderers[0].materials.ToList();
            for(int j = 0; j < meshRenderers.Length; j++){
                meshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }     
        }
    }
    public int GetSkinsSize(){
        return mSkinMaterials.Count;
    }
    public Material GetSkinSets(int _index){
        return mSkinMaterials[_index];
    }

    public void SetSkinSets(int _index, Material _skin){
        mSkinMaterials[_index] = _skin;
        if(mode == RendererMode.skin){
            for(int j = 0; j < skinnedMeshRenderers.Length; j++){
                skinnedMeshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }
        }
        if(mode == RendererMode.mesh) {
            for(int j = 0; j < meshRenderers.Length; j++){
                meshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }
        }
    }

    public void SetSkinSets(int _index) {
        mSkinMaterials[_index] = TransMaterial;
        if(mode == RendererMode.skin){
            for(int j = 0; j < skinnedMeshRenderers.Length; j++){
                skinnedMeshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }
        }
        if(mode == RendererMode.mesh) {
            for(int j = 0; j < meshRenderers.Length; j++){
                meshRenderers[j].sharedMaterials = mSkinMaterials.ToArray();
            }
        }
    }

    public void AddSkinSets(Material _skin){
        mSkinMaterials.Add(_skin);
    }
    public void DelSkinSets(Material _skin){
        mSkinMaterials.Remove(_skin);
    }
}
