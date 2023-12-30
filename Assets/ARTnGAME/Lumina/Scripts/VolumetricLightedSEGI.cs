using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using Unity.Mathematics;

namespace Artngame.LUMINA
{
    public class VolumetricLightedSEGI : ScriptableRendererFeature
    {
        [System.Serializable]
        public class VolumetricLightScatteringSettings
        {
            [Header("Volumetric Properties")]
            [Range(0.1f, 1f)]
            public float resolutionScale = 0.5f;
            [Range(0.0f, 1.0f)]
            public float intensity = 1.0f;
            [Range(0.0f, 1.0f)]
            public float blurWidth = 0.85f;
            [Range(0.0f, 0.2f)]
            public float fadeRange = 0.85f;
            [Range(50, 200)]
            public uint numSamples = 100;

            [Header("Noise Properties")]
            //public float2 noiseSpeed = 0.5f;
            public Vector2 noiseSpeed = 0.5f * Vector2.one;
            public float noiseScale = 1.0f;
            [Range(0.0f, 1.0f)]
            public float noiseStrength = 0.6f;

            //v0.1
            public RenderPassEvent eventA = RenderPassEvent.AfterRenderingSkybox;
        }

        class LightScatteringPass : ScriptableRenderPass
        {
        
            //v0.1
            //private readonly RenderTargetHandle _occluders = RenderTargetHandle.CameraTarget;
            //private readonly RenderTargetHandle _occluders = RenderTargetHandle.CameraTarget;
            //if (destination == renderingData.cameraData.renderer.cameraColorTargetHandle)//  UnityEngine.Rendering.Universal.RenderTargetHandle.CameraTarget) //v0.1
            
            
            private readonly VolumetricLightScatteringSettings _settings;
            private readonly List<ShaderTagId> _shaderTagIdList = new List<ShaderTagId>();
            //private Material _occludersMaterial;
            //private Material _radialBlurMaterial;
            private FilteringSettings _filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            private RenderTargetIdentifier _cameraColorTargetIdent;

            private RenderTargetIdentifier source;

            public LightScatteringPass(VolumetricLightScatteringSettings settings)
            {
                ///_occluders.Init("_OccludersMap");//v0.1
                _settings = settings;

                _shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
                _shaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
                _shaderTagIdList.Add(new ShaderTagId("LightweightForward"));
                _shaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
            }

            public void SetCameraColorTarget(RenderTargetIdentifier _cameraColorTargetIdent)
              => this._cameraColorTargetIdent = _cameraColorTargetIdent;

            // This method is called before executing the render pass.
            // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
            // When empty this render pass will render to the active camera render target.
            // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
            // The render pipeline will ensure target setup and clearing happens in a performant manner.
#if UNITY_2020_2_OR_NEWER
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                // get a copy of the current camera’s RenderTextureDescriptor
                // this descriptor contains all the information you need to create a new texture
                RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;

                // disable the depth buffer because we are not going to use it
                cameraTextureDescriptor.depthBufferBits = 0;

                // scale the texture dimensions
                cameraTextureDescriptor.width = Mathf.RoundToInt(cameraTextureDescriptor.width * _settings.resolutionScale);
                cameraTextureDescriptor.height = Mathf.RoundToInt(cameraTextureDescriptor.height * _settings.resolutionScale);

                // create temporary render texture
           //     cmd.GetTemporaryRT(_occluders.id, cameraTextureDescriptor, FilterMode.Bilinear);//v0.1

                // finish configuration
           //     ConfigureTarget(_occluders.Identifier());//v0.1


                //v0.1
                var renderer = renderingData.cameraData.renderer;
                //v0.1
                //source = renderer.cameraColorTarget;
#if UNITY_2022_1_OR_NEWER
                source = renderer.cameraColorTargetHandle;
#else
                source = renderer.cameraColorTarget;
#endif

            }
#else
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                // get a copy of the current camera’s RenderTextureDescriptor
                // this descriptor contains all the information you need to create a new texture
                //RenderTextureDescriptor cameraTextureDescriptor = cameraTextureDescriptor;// renderingData.cameraData.cameraTargetDescriptor;

                // disable the depth buffer because we are not going to use it
                cameraTextureDescriptor.depthBufferBits = 0;

                // scale the texture dimensions
                cameraTextureDescriptor.width = Mathf.RoundToInt(cameraTextureDescriptor.width * _settings.resolutionScale);
                cameraTextureDescriptor.height = Mathf.RoundToInt(cameraTextureDescriptor.height * _settings.resolutionScale);

                // create temporary render texture
           //     cmd.GetTemporaryRT(_occluders.id, cameraTextureDescriptor, FilterMode.Bilinear);//v0.1

                // finish configuration
           //     ConfigureTarget(_occluders.Identifier());//v0.1

                //v0.1
                //var renderer = renderingData.cameraData.renderer;
                source = _cameraColorTargetIdent; //source = renderer.cameraColorTarget;
            }
#endif

            // Here you can implement the rendering logic.
            // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
            // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {



                //v0.1
                bool enableGI = false;
                if (Camera.main != null)
                {
                    LUMINA segi = Camera.main.GetComponent<LUMINA>();
                    if (segi != null)
                    {
                        enableGI = !segi.disableGI;
                    }
                }

                //if (!_occludersMaterial || !_radialBlurMaterial) InitializeMaterials();
                if (RenderSettings.sun == null || !RenderSettings.sun.enabled || !enableGI
                    || (Camera.main != null && Camera.current != null && Camera.current != Camera.main) || Camera.main == null
                    ) { return; }

                // get command buffer pool
                CommandBuffer cmd = CommandBufferPool.Get();

                using (new ProfilingScope(cmd, new ProfilingSampler("VolumetricLightScattering")))
                {
                    //if (1 == 0)
                    //{
                    //    // prepares command buffer
                    //    context.ExecuteCommandBuffer(cmd);
                    //    cmd.Clear();

                    //    Camera camera = renderingData.cameraData.camera;
                    //    context.DrawSkybox(camera);

                    //    DrawingSettings drawSettings = CreateDrawingSettings(
                    //      _shaderTagIdList, ref renderingData, SortingCriteria.CommonOpaque
                    //    );
                    //    drawSettings.overrideMaterial = _occludersMaterial;
                    //    context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref _filteringSettings);

                    //    // schedule it for execution and release it after the execution
                    //    context.ExecuteCommandBuffer(cmd);
                    //    CommandBufferPool.Release(cmd);

                    //    //float3 sunDirectionWorldSpace = RenderSettings.sun.transform.forward;
                    //    //float3 cameraDirectionWorldSpace = camera.transform.forward;
                    //    //float3 cameraPositionWorldSpace = camera.transform.position;
                    //    //float3 sunPositionWorldSpace = cameraPositionWorldSpace + sunDirectionWorldSpace;
                    //    //float3 sunPositionViewportSpace = camera.WorldToViewportPoint(sunPositionWorldSpace);
                    //    Vector3 sunDirectionWorldSpace = RenderSettings.sun.transform.forward;
                    //    Vector3 cameraDirectionWorldSpace = camera.transform.forward;
                    //    Vector3 cameraPositionWorldSpace = camera.transform.position;
                    //    Vector3 sunPositionWorldSpace = cameraPositionWorldSpace + sunDirectionWorldSpace;
                    //    Vector3 sunPositionViewportSpace = camera.WorldToViewportPoint(sunPositionWorldSpace);

                    //    //float dotProd = math.dot(-cameraDirectionWorldSpace, sunDirectionWorldSpace);
                    //    //dotProd -= math.dot(cameraDirectionWorldSpace, Vector3.down);

                    //    float dotProd = Vector3.Dot(-cameraDirectionWorldSpace, sunDirectionWorldSpace);
                    //    dotProd -= Vector3.Dot(cameraDirectionWorldSpace, Vector3.down);

                    //    float intensityFader = dotProd / _settings.fadeRange;
                    //    intensityFader = Mathf.Clamp01(intensityFader); //intensityFader = math.saturate(intensityFader);

                    //    _radialBlurMaterial.SetColor("_Color", RenderSettings.sun.color);
                    //    _radialBlurMaterial.SetVector("_Center", new Vector4(
                    //      sunPositionViewportSpace.x, sunPositionViewportSpace.y, 0.0f, 0.0f
                    //    ));
                    //    _radialBlurMaterial.SetFloat("_BlurWidth", _settings.blurWidth);
                    //    _radialBlurMaterial.SetFloat("_NumSamples", _settings.numSamples);
                    //    _radialBlurMaterial.SetFloat("_Intensity", _settings.intensity * intensityFader);

                    //    //_radialBlurMaterial.SetVector("_NoiseSpeed", new float4(_settings.noiseSpeed, 0.0f, 0.0f));
                    //    _radialBlurMaterial.SetVector("_NoiseSpeed", new Vector4(_settings.noiseSpeed.x, _settings.noiseSpeed.y, 0.0f, 0.0f));

                    //    _radialBlurMaterial.SetFloat("_NoiseScale", _settings.noiseScale);
                    //    _radialBlurMaterial.SetFloat("_NoiseStrength", _settings.noiseStrength);

                    //    Blit(cmd, _occluders.Identifier(), _cameraColorTargetIdent, _radialBlurMaterial);
                    //}


                    //v0.1
                    //context.ExecuteCommandBuffer(cmd);
                    //cmd.Clear();
                    Camera cameraA = renderingData.cameraData.camera;
                    RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;

                    RenderTexture skyA = RenderTexture.GetTemporary(cameraTextureDescriptor.width, cameraTextureDescriptor.height, 0, RenderTextureFormat.ARGBHalf);

		     //v0.1
		     cmd.Blit(source, skyA);
                    //context.DrawSkybox(cameraA);
                    //https://www.febucci.com/2022/05/custom-post-processing-in-urp/
                    //var renderer = renderingData.cameraData.renderer;
                    //source = renderer.cameraColorTarget;
                    //context.sky
                    //Blit(cmd, renderingData.cameraData.renderer.cameraColorTarget, skyA);// Blit(cmd, _occluders.Identifier(), skyA);

                    //DEBUG 
                    // Blit(cmd, skyA, _cameraColorTargetIdent);
                    // context.ExecuteCommandBuffer(cmd);
                    //CommandBufferPool.Release(cmd);
                    //RenderTexture.ReleaseTemporary(skyA);                   
                    //return;


                    RenderTexture gi1 = RenderTexture.GetTemporary(cameraTextureDescriptor.width, cameraTextureDescriptor.height, 0, RenderTextureFormat.ARGBHalf);

                    //renderingData.cameraData.camera = Camera.main;

                    OnRenderImage(cameraA, cmd, skyA, gi1);

                    //v0.1
		     cmd.Blit(gi1, source);// _cameraColorTargetIdent); //1.0g

                    context.ExecuteCommandBuffer(cmd);
                    CommandBufferPool.Release(cmd);

                    RenderTexture.ReleaseTemporary(skyA);
                    RenderTexture.ReleaseTemporary(gi1);
                }
            }



            void OnRenderImage(Camera camera, CommandBuffer cmd, RenderTexture source, RenderTexture destination)
            {



                // Graphics.Blit(source, destination);
                //Blit(cmd, source, destination);
                //return;

                if (Camera.main != null && camera != Camera.main)
                {
                    //Debug.Log("No cam0");
                    return;
                }

                //Debug.Log(Camera.main.orthographic);
                Camera.main.depthTextureMode = DepthTextureMode.Depth;

                //v0.1
                if (Camera.main != null)
                {
                    LUMINA segi = Camera.main.GetComponent<LUMINA>();
                    if (segi != null && segi.enabled)
                    {
                        if (segi.notReadyToRender || Camera.main == null)
                        {
                            //Blit(cmd, source, source);
                            //Graphics.Blit(source, destination);
                            //v0.1
		     		cmd.Blit( source, destination);
                            return;
                        }

                        //Set parameters
                        Shader.SetGlobalFloat("SEGIVoxelScaleFactor", segi.voxelScaleFactor);

                        if (!segi.material)//v0.2a
                        {
                            segi.material = new Material(Shader.Find("Hidden/SEGI"));
                            //material.hideFlags = HideFlags.HideAndDontSave;//v0.2
                        }

                        segi.material.SetMatrix("CameraToWorld", segi.attachedCamera.cameraToWorldMatrix);
                        segi.material.SetMatrix("WorldToCamera", segi.attachedCamera.worldToCameraMatrix);
                        segi.material.SetMatrix("ProjectionMatrixInverse", segi.attachedCamera.projectionMatrix.inverse);
                        segi.material.SetMatrix("ProjectionMatrix", segi.attachedCamera.projectionMatrix);
                        segi.material.SetInt("FrameSwitch", segi.frameCounter);
                        Shader.SetGlobalInt("SEGIFrameSwitch", segi.frameCounter);
                        segi.material.SetVector("CameraPosition", segi.transform.position);
                        segi.material.SetFloat("DeltaTime", Time.deltaTime);

                        segi.material.SetInt("StochasticSampling", segi.stochasticSampling ? 1 : 0);
                        segi.material.SetInt("TraceDirections", segi.cones);
                        segi.material.SetInt("TraceSteps", segi.coneTraceSteps);
                        segi.material.SetFloat("TraceLength", segi.coneLength);
                        segi.material.SetFloat("ConeSize", segi.coneWidth);
                        segi.material.SetFloat("OcclusionStrength", segi.occlusionStrength);
                        segi.material.SetFloat("OcclusionPower", segi.occlusionPower);
                        segi.material.SetFloat("ConeTraceBias", segi.coneTraceBias);
                        segi.material.SetFloat("GIGain", segi.giGain);
                        segi.material.SetFloat("NearLightGain", segi.nearLightGain);
                        segi.material.SetFloat("NearOcclusionStrength", segi.nearOcclusionStrength);
                        segi.material.SetInt("DoReflections", segi.doReflections ? 1 : 0);
                        segi.material.SetInt("HalfResolution", segi.halfResolution ? 1 : 0);
                        segi.material.SetInt("ReflectionSteps", segi.reflectionSteps);
                        segi.material.SetFloat("ReflectionOcclusionPower", segi.reflectionOcclusionPower);
                        segi.material.SetFloat("SkyReflectionIntensity", segi.skyReflectionIntensity);
                        segi.material.SetFloat("FarOcclusionStrength", segi.farOcclusionStrength);
                        segi.material.SetFloat("FarthestOcclusionStrength", segi.farthestOcclusionStrength);
                        segi.material.SetTexture("NoiseTexture", segi.blueNoise[segi.frameCounter % 64]);
                        segi.material.SetFloat("BlendWeight", segi.temporalBlendWeight);

                        //v0.4
                        segi.material.SetFloat("contrastA", segi.contrastA);
                        segi.material.SetVector("ReflectControl", segi.ReflectControl);

                        //v0.7
                        segi.material.SetVector("ditherControl",
                        new Vector4(segi.DitherControl.x,
                        Mathf.Clamp(segi.DitherControl.y, 0.1f, 10),
                        Mathf.Clamp(segi.DitherControl.z, 0.1f, 10),
                        Mathf.Clamp(segi.DitherControl.w, 0.1f, 10))); //v1.2b

                        //v1.2
                        segi.material.SetFloat("smoothNormals", segi.smoothNormals);

                        //If Visualize Voxels is enabled, just render the voxel visualization shader pass and return
                        if (segi.visualizeVoxels)
                        {
                            //Blit(cmd, segi.blueNoise[segi.frameCounter % 64], destination);
                            //v0.1
		     cmd.Blit( source, destination, segi.material, LUMINA.Pass.VisualizeVoxels);
                            return;
                        }

                        //Setup temporary textures
                        RenderTexture gi1 = RenderTexture.GetTemporary(source.width / segi.giRenderRes, source.height / segi.giRenderRes, 0, RenderTextureFormat.ARGBHalf);
                        RenderTexture gi2 = RenderTexture.GetTemporary(source.width / segi.giRenderRes, source.height / segi.giRenderRes, 0, RenderTextureFormat.ARGBHalf);
                        RenderTexture reflections = null;

                        //If reflections are enabled, create a temporary render buffer to hold them
                        if (segi.doReflections)
                        {
                            reflections = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
                        }

                        //Setup textures to hold the current camera depth and normal
                        RenderTexture currentDepth = RenderTexture.GetTemporary(source.width / segi.giRenderRes, source.height / segi.giRenderRes, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
                        currentDepth.filterMode = FilterMode.Point;

                        RenderTexture currentNormal = RenderTexture.GetTemporary(source.width / segi.giRenderRes, source.height / segi.giRenderRes, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
                        currentNormal.filterMode = FilterMode.Point;

                        //Get the camera depth and normals
                        //v0.1
		     cmd.Blit( source, currentDepth, segi.material, LUMINA.Pass.GetCameraDepthTexture);//v0.1
                                                                                                          //Blit(cmd, source, currentDepth, segi.material, VolumeLitSEGI.Pass.GetCameraDepthTexture);


                        segi.material.SetTexture("CurrentDepth", currentDepth);
                        //v0.1
		     cmd.Blit( source, currentNormal, segi.material, LUMINA.Pass.GetWorldNormals);
                        segi.material.SetTexture("CurrentNormal", currentNormal);


                        //v0.1 - check depths
                        if (segi.visualizeNORMALS)
                        {
                            //v0.1
		     cmd.Blit( currentNormal, destination);
                            return;
                        }
                        //if (segi.visualizeDEPTH)
                        //{
                        //    Blit(cmd, currentDepth, destination);
                        //    return;
                        //}



                        //Set the previous GI result and camera depth textures to access them in the shader
                        segi.material.SetTexture("PreviousGITexture", segi.previousGIResult);
                        Shader.SetGlobalTexture("PreviousGITexture", segi.previousGIResult);
                        segi.material.SetTexture("PreviousDepth", segi.previousCameraDepth);

                        //Render diffuse GI tracing result
                        //v0.1
		     cmd.Blit( source, gi2, segi.material, LUMINA.Pass.DiffuseTrace);

                        if (segi.visualizeDEPTH)
                        {
                            //v0.1
		     cmd.Blit( gi2, destination);
                            return;
                        }

                        if (segi.doReflections)
                        {
                            //Render GI reflections result
                            //v0.1
		     cmd.Blit( source, reflections, segi.material, LUMINA.Pass.SpecularTrace);
                            segi.material.SetTexture("Reflections", reflections);
                        }


                        //Perform bilateral filtering
                        if (segi.useBilateralFiltering)
                        {
                            segi.material.SetVector("Kernel", new Vector2(0.0f, 1.0f));
                            //v0.1
		     cmd.Blit( gi2, gi1, segi.material, LUMINA.Pass.BilateralBlur);

                            segi.material.SetVector("Kernel", new Vector2(1.0f, 0.0f));
                            //v0.1
		     cmd.Blit( gi1, gi2, segi.material, LUMINA.Pass.BilateralBlur);

                            segi.material.SetVector("Kernel", new Vector2(0.0f, 1.0f));
                            //v0.1
		     cmd.Blit( gi2, gi1, segi.material, LUMINA.Pass.BilateralBlur);

                            segi.material.SetVector("Kernel", new Vector2(1.0f, 0.0f));
                            //v0.1
		     cmd.Blit( gi1, gi2, segi.material, LUMINA.Pass.BilateralBlur);
                        }

                        //If Half Resolution tracing is enabled
                        if (segi.giRenderRes == 2)
                        {
                            RenderTexture.ReleaseTemporary(gi1);

                            //Setup temporary textures
                            RenderTexture gi3 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
                            RenderTexture gi4 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);


                            //Prepare the half-resolution diffuse GI result to be bilaterally upsampled
                            gi2.filterMode = FilterMode.Point;
                            //v0.1
		     cmd.Blit( gi2, gi4);

                            RenderTexture.ReleaseTemporary(gi2);

                            gi4.filterMode = FilterMode.Point;
                            gi3.filterMode = FilterMode.Point;


                            //Perform bilateral upsampling on half-resolution diffuse GI result
                            segi.material.SetVector("Kernel", new Vector2(1.0f, 0.0f));
                            //v0.1
		     cmd.Blit( gi4, gi3, segi.material, LUMINA.Pass.BilateralUpsample);
                            segi.material.SetVector("Kernel", new Vector2(0.0f, 1.0f));

                            //Perform temporal reprojection and blending
                            if (segi.temporalBlendWeight < 1.0f)
                            {
                                //v0.1
		     cmd.Blit( gi3, gi4);
                                //v0.1
		     cmd.Blit( gi4, gi3, segi.material, LUMINA.Pass.TemporalBlend);
                                //v0.1
		     cmd.Blit( gi3, segi.previousGIResult);
                                //v0.1
		     cmd.Blit( source, segi.previousCameraDepth, segi.material, LUMINA.Pass.GetCameraDepthTexture);
                            }

                            //Set the result to be accessed in the shader
                            segi.material.SetTexture("GITexture", gi3);

                            //Actually apply the GI to the scene using gbuffer data
                            //v0.1
		     cmd.Blit( source, destination, segi.material, segi.visualizeGI ? LUMINA.Pass.VisualizeGI : LUMINA.Pass.BlendWithScene);

                            //Release temporary textures
                            RenderTexture.ReleaseTemporary(gi3);
                            RenderTexture.ReleaseTemporary(gi4);
                        }
                        else    //If Half Resolution tracing is disabled
                        {
                            //Perform temporal reprojection and blending
                            if (segi.temporalBlendWeight < 1.0f)
                            {
                                //v0.1
		     cmd.Blit( gi2, gi1, segi.material, LUMINA.Pass.TemporalBlend);
                                //v0.1
		     cmd.Blit( gi1, segi.previousGIResult);
                                //v0.1
		     cmd.Blit( source, segi.previousCameraDepth, segi.material, LUMINA.Pass.GetCameraDepthTexture);
                            }

                            //Actually apply the GI to the scene using gbuffer data
                            segi.material.SetTexture("GITexture", segi.temporalBlendWeight < 1.0f ? gi1 : gi2);
                            //v0.1
		     cmd.Blit( source, destination, segi.material, segi.visualizeGI ? LUMINA.Pass.VisualizeGI : LUMINA.Pass.BlendWithScene);

                            //Release temporary textures
                            RenderTexture.ReleaseTemporary(gi1);
                            RenderTexture.ReleaseTemporary(gi2);
                        }

                        //Release temporary textures
                        RenderTexture.ReleaseTemporary(currentDepth);
                        RenderTexture.ReleaseTemporary(currentNormal);

                        //Visualize the sun depth texture
                        if (segi.visualizeSunDepthTexture){
                            //v0.1
		     cmd.Blit( segi.sunDepthTexture, destination);
		     }


                        //Release the temporary reflections result texture
                        if (segi.doReflections)
                        {
                            RenderTexture.ReleaseTemporary(reflections);
                        }

                        //Set matrices/vectors for use during temporal reprojection
                        segi.material.SetMatrix("ProjectionPrev", segi.attachedCamera.projectionMatrix);
                        segi.material.SetMatrix("ProjectionPrevInverse", segi.attachedCamera.projectionMatrix.inverse);
                        segi.material.SetMatrix("WorldToCameraPrev", segi.attachedCamera.worldToCameraMatrix);
                        segi.material.SetMatrix("CameraToWorldPrev", segi.attachedCamera.cameraToWorldMatrix);
                        segi.material.SetVector("CameraPositionPrev", segi.transform.position);

                        //Advance the frame counter
                        segi.frameCounter = (segi.frameCounter + 1) % (64);
                    }
                }
            }

            // Cleanup any allocated resources that were created during the execution of this render pass.
#if UNITY_2020_2_OR_NEWER
            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                //cmd.ReleaseTemporaryRT(_occluders.id); //v0.1
            }
#else
            /// Cleanup any allocated resources that were created during the execution of this render pass.
            private RenderTargetHandle destination { get; set; }
            public override void FrameCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(_occluders.id);
                if (destination != RenderTargetHandle.CameraTarget)
                {
                    cmd.ReleaseTemporaryRT(destination.id);
                    destination = RenderTargetHandle.CameraTarget;
                }                
            }
#endif

            //private void InitializeMaterials()
            //{
            //    _occludersMaterial = new Material(Shader.Find("Hidden/UnlitColor"));
            //    _radialBlurMaterial = new Material(Shader.Find("Hidden/RadialBlur"));
            //}
        }

        private LightScatteringPass _scriptablePass;
        public VolumetricLightScatteringSettings _settings = new VolumetricLightScatteringSettings();

        /// <inheritdoc/>
        public override void Create()
        {
            _scriptablePass = new LightScatteringPass(_settings);

            // Configures where the render pass should be injected.
            _scriptablePass.renderPassEvent = _settings.eventA;// RenderPassEvent.BeforeRenderingPostProcessing;
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_scriptablePass);
            //_scriptablePass.SetCameraColorTarget(renderer.cameraColorTarget);//1.0g
        }

    }
}

