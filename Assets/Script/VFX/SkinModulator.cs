using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RendererMode { skin, mesh } 
public class SkinModulator : MonoBehaviour
{
    [Tooltip("이 모드를 통해서 Skinned를 사용할것인지, Mesh를 사용할것인지 선택 한다.")]
    public RendererMode mode;

    [SerializeField]
    [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    [Tooltip("SkinMaterial 스킨을 입힐 대상들이다.")]
    public MeshRenderer[] meshRenderers;
    
    //인덱스는 부위각각이다. 
    [SerializeField] public List<Material> mSkinMaterials = new List<Material>();
    public Material TransMaterial;

    [ContextMenu("Awake")]
    private void Awake() {
        if(mSkinMaterials.Count == 0) throw new System.Exception("Skin리스트가 없어서 설정하고싶은 스킨이 없음");
    }

    public void ChangeSkin(Material _skin){
        mSkinMaterials[1] = _skin;
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

    public void RevertSkin() {
        mSkinMaterials[1] = TransMaterial;
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

    public int GetSkinsSize(){
        return mSkinMaterials.Count;
    }
    public Material GetSkinSets(int _index){
        return mSkinMaterials[_index];
    }

    public void AddSkinSets(Material _skin){
        mSkinMaterials.Add(_skin);
    }
    public void DelSkinSets(Material _skin){
        mSkinMaterials.Remove(_skin);
    }
}
