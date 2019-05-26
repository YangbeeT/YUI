﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class MUSEditor : ShaderGUI {

    static int rm;

    public enum BlendingMode {FADE, TRANSPARENT}

    static GUIContent MainTexLabel = new GUIContent("Main Texture", "");
    static GUIContent AlbedoLabel = new GUIContent("Albedo", "");
    static GUIContent DetailAlbedoLabel = new GUIContent("Detail Albedo", "");
    static GUIContent DetailNormalLabel = new GUIContent("Detail Normal Map", "");
    static GUIContent EmissTexLabel = new GUIContent("Emission Map", "");
    static GUIContent EmissMaskLabel = new GUIContent("Mask", "");
    static GUIContent NormalTexLabel = new GUIContent("Normal Map", "");
    static GUIContent MetallicTexLabel = new GUIContent("Metallic", "");
    static GUIContent RoughnessTexLabel = new GUIContent("Roughness", "");
    static GUIContent OcclusionMapLabel = new GUIContent("Occlusion", "");
    static GUIContent HeightTexLabel = new GUIContent("Height", "");
    static GUIContent ReflCubeLabel = new GUIContent("Reflection Cubemap", "");
    static GUIContent TranslucencyLabel = new GUIContent("Translucency Map", "");
    static GUIContent ShadowRampLabel = new GUIContent("Shadow Ramp", "");
    static GUIContent SpecularTexLabel = new GUIContent("Specular Map", "");
    static GUIContent DetailMaskLabel = new GUIContent("Detail Mask", "");
    static GUIContent LightingMaskLabel = new GUIContent("Lighting Mask", "");
    static GUIContent ShadowMaskLabel = new GUIContent("Shadow Mask", "");
    static GUIContent PrimaryMapsLabel = new GUIContent("Primary Maps (UV0)", "");
    static GUIContent DetailMapsLabel = new GUIContent("Detail Maps (UV1)", "");
    
    static bool showBase = true;
    static bool showShading = false;
    static bool showEmiss = false;
    static bool showRim = false;

    static bool showReflections = false;
    static bool showSpecular = false;
    static bool showSSS = false;
    static bool showFresnel = false;
    
    static string header = "Header_Pro";
    static string baseFoldout = "BaseFoldout_Pro";
    static string shadingFoldout = "ShadingFoldout_Pro";
    static string emissFoldout = "EmissFoldout_Pro";
    static string rimFoldout = "RimLightingFoldout_Pro";
    static string discordPic = "Discord_Pro";
    static string discordIconPic = "Discord_Icon_Pro";
    static string versionPic = "Version_Pro";
    
    MaterialProperty _RenderMode = null; 
    MaterialProperty _CullingMode = null; 
    MaterialProperty _MainTex = null; 
    MaterialProperty _Color = null; 
    MaterialProperty _SaturationRGB = null; 
    MaterialProperty _Contrast = null; 
    MaterialProperty _EmissionMap = null; 
    MaterialProperty _EmissMask = null; 
    MaterialProperty _EmissionColor = null; 
    MaterialProperty _UsingMask = null; 
    MaterialProperty _CrossMode = null; 
    MaterialProperty _XScroll = null; 
    MaterialProperty _YScroll = null; 
    MaterialProperty _PulseSpeed = null; 
    MaterialProperty _Crossfade = null; 
    MaterialProperty _ReactToggle = null; 
    MaterialProperty _ReactThresh = null;
    MaterialProperty _Outline = null; 
    MaterialProperty _OutlineThicc = null; 
    MaterialProperty _OutlineCol = null;
    MaterialProperty _RimCol = null; 
    MaterialProperty _RimStrength = null; 
    MaterialProperty _RimWidth = null; 
    MaterialProperty _RimEdge = null;
    MaterialProperty _BumpMap = null; 
    MaterialProperty _DetailNormalMap = null; 
    MaterialProperty _BumpScale = null; 
    MaterialProperty _DetailNormalMapScale = null;
    MaterialProperty _PreserveCol = null; 
    MaterialProperty _ShadowStr = null; 
    MaterialProperty _RampWidth = null;
    MaterialProperty _FakeSpec = null; 
    MaterialProperty _SpecStr = null; 
    MaterialProperty _SpecSize = null; 
    MaterialProperty _SpecCol = null; 
    MaterialProperty _SpecBias = null;
    MaterialProperty _Metallic = null; 
    MaterialProperty _MetallicGlossMap = null;
    MaterialProperty _Glossiness = null; 
    MaterialProperty _SpecGlossMap = null;
    MaterialProperty _OcclusionStrength = null; 
    MaterialProperty _OcclusionMap = null;
    MaterialProperty _Parallax = null;
    MaterialProperty _ParallaxMap = null; 
    MaterialProperty _March = null; 
    MaterialProperty _HeightContrast = null;
    MaterialProperty _UseReflCube = null; 
    MaterialProperty _ReflCube = null;
    MaterialProperty _ZWrite = null; 
    MaterialProperty _Cutoff = null; 
    MaterialProperty _ATM = null;
    MaterialProperty _Subsurface = null;
    MaterialProperty _TranslucencyMap = null; 
    MaterialProperty _SColor = null;
    MaterialProperty _SStrength = null;
    MaterialProperty _SSharp = null;
    MaterialProperty _Reflections = null;
    MaterialProperty _ReflSmooth = null;
    MaterialProperty _EnableShadowRamp = null;
    MaterialProperty _ShadowRamp = null;
    MaterialProperty _SPen = null;
    MaterialProperty _Brightness = null;
    MaterialProperty _RAmt = null;
    MaterialProperty _GAmt = null;
    MaterialProperty _BAmt = null;
    MaterialProperty _Hue = null;
    MaterialProperty _SaturationHSL = null;
    MaterialProperty _Luminance = null;
    MaterialProperty _HSLMin = null;
    MaterialProperty _HSLMax = null;
    MaterialProperty _FilterModel = null;
    MaterialProperty _AutoShift = null;
    MaterialProperty _AutoShiftSpeed = null;
    MaterialProperty _InvertRough = null;
    MaterialProperty _LightMask = null;
    MaterialProperty _PBRWorkflow = null;
    MaterialProperty _SourceAlpha = null;
    MaterialProperty _PBRSpecCol = null;
    MaterialProperty _RimMask = null;
    MaterialProperty _RimMaskStr = null;
    MaterialProperty _RimTex = null;
    MaterialProperty _GlossMapScale = null;
    MaterialProperty _DetailAlbedoMap = null;
    MaterialProperty _DetailMask = null;
    MaterialProperty _DetailMaskStr = null;
    MaterialProperty _ReflCubeFallback = null;
    MaterialProperty _HDR = null;
    MaterialProperty _StaticLightDirToggle = null;
    MaterialProperty _StaticLightDir = null;
    MaterialProperty _ShadowMask = null;
    MaterialProperty _ToonMetallic = null;
    MaterialProperty _ShadowMaskStr = null;
    MaterialProperty _LightMaskStr = null;
    MaterialProperty _UnlitRim = null;
    MaterialProperty _RimBlending = null;
    MaterialProperty _BlendMode = null;
    MaterialProperty _RimNoise = null;
    MaterialProperty _IOR = null;
    MaterialProperty _FresnelStr = null;
    MaterialProperty _FresnelFade = null;
    MaterialProperty _FresnelSmooth = null;
    MaterialProperty _FresnelMetallic = null;

    BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    private const float kMaxfp16 = 65536f; // Clamp to a value that fits into fp16.
    ColorPickerHDRConfig HDRField = new ColorPickerHDRConfig(0f, kMaxfp16, 1 / kMaxfp16, 3f);

    MaterialEditor m_MaterialEditor;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props) {

        Material material = materialEditor.target as Material;
        bool isTransparent = material.shader.name.Contains("Transparent");
        bool isCutout = material.shader.name.Contains("Cutout");
        if (!materialEditor.isVisible)
            return;
        foreach (var property in GetType().GetFields(bindingFlags)){
            if (property.FieldType == typeof(MaterialProperty)){
                property.SetValue(this, FindProperty(property.Name, props));
            }
        }

        if (!EditorGUIUtility.isProSkin){
                header = "Header";
                baseFoldout = "BaseFoldout";
                shadingFoldout = "ShadingFoldout";
                emissFoldout = "EmissFoldout";
                rimFoldout = "RimLightingFoldout";
                discordPic = "Discord";
                discordIconPic = "Discord_Icon";
                versionPic = "Version";
        }

        Texture2D headerTex = (Texture2D)Resources.Load(header, typeof(Texture2D));
        Texture2D shadingTex = (Texture2D)Resources.Load(shadingFoldout, typeof(Texture2D));
        ShaderGUIHelper.CenteredTexture(headerTex, 0, 0);
        EditorGUI.BeginChangeCheck(); {

            bool isPBR = _RenderMode.floatValue == 2;
            bool isToon = _RenderMode.floatValue == 1;
            bool isBasic = _RenderMode.floatValue == 0;
            bool usingHeight = _ParallaxMap.textureValue;
            bool usingNormal = _BumpMap.textureValue;
            bool usingDetail = _DetailNormalMap.textureValue;
            bool usingMetallic = _MetallicGlossMap.textureValue;
            bool usingRoughness = _SpecGlossMap.textureValue;
            bool usingRamp = _ShadowRamp.textureValue;
            bool usingMask = _EmissMask.textureValue;
            bool usingEmiss = _EmissionMap.textureValue;
            bool usingRefl = _ReflCube.textureValue;

            if (isToon){
                if (usingRamp) _EnableShadowRamp.floatValue = 1;
                else _EnableShadowRamp.floatValue = 0;
            }
            if (isPBR){
                if (usingHeight) material.EnableKeyword("_PARALLAXMAP");
                else material.DisableKeyword("_PARALLAXMAP");
                if (_PBRWorkflow.floatValue == 0){
                    if (usingMetallic) _Metallic.floatValue = 1;
                    if (usingRoughness) _Glossiness.floatValue = 1;
                    if (!usingRoughness) _InvertRough.floatValue = 0;
                }
                else {
                    if (usingRoughness) material.EnableKeyword("_SPECGLOSSMAP");
                    else material.DisableKeyword("_SPECGLOSSMAP");
                }
            }
            else {
                material.DisableKeyword("_SPECGLOSSMAP");
                material.DisableKeyword("_PARALLAXMAP");
            }
            if (isToon || isPBR){
                if (usingRefl) _UseReflCube.floatValue = 1;
                else _UseReflCube.floatValue = 0;
                if (usingDetail) material.EnableKeyword("_DETAIL_MULX2");
                else material.DisableKeyword("_DETAIL_MULX2");
            }
            else material.DisableKeyword("_DETAIL_MULX2");

            Texture2D baseTex = (Texture2D)Resources.Load(baseFoldout, typeof(Texture2D));
            showBase = ShaderGUIHelper.Foldout(baseTex, showBase, 22.0f);
            if (showBase){
                GUILayout.Space(4);
                materialEditor.ShaderProperty(_RenderMode, "Shading Style");
                materialEditor.ShaderProperty(_CullingMode, "Backface Culling");

                // Outline
                if (!isTransparent){
                    materialEditor.ShaderProperty(_Outline, "Outline");
                    if (_Outline.floatValue >= 1){
                        materialEditor.ShaderProperty(_OutlineCol, "Color", 1);
                        materialEditor.ShaderProperty(_OutlineThicc, "Thickness", 1);
                        if (isCutout || isTransparent)
                            GUILayout.Space(8);
                    }
                }
                // Cutout and Transparent specific properties
                if (isCutout){
                    if (!isPBR)
                        materialEditor.ShaderProperty(_ATM, "Alpha To Mask");
                    if (_ATM.floatValue != 1)
                        materialEditor.ShaderProperty(_Cutoff, "Cutout Amount");
                }
                if (isTransparent){
                    materialEditor.ShaderProperty(_ZWrite, _ZWrite.displayName);
                    EditorGUI.showMixedValue = _BlendMode.hasMixedValue;
                    var mode = (BlendingMode)_BlendMode.floatValue;
                    EditorGUI.BeginChangeCheck();
                    mode = (BlendingMode)EditorGUILayout.Popup("Blending Mode", (int)mode, Enum.GetNames(typeof(BlendingMode)));
                    if (EditorGUI.EndChangeCheck()) {
                        materialEditor.RegisterPropertyChangeUndo("Blending Mode");
                        _BlendMode.floatValue = (float)mode;
                        foreach (var obj in _BlendMode.targets){
                            SetBlendMode((Material)obj, (BlendingMode)mode);
                        }
                        EditorGUI.showMixedValue = false;
                    }
                }

                GUILayout.Space(15);
                if (isBasic || isToon){
                    materialEditor.TexturePropertySingleLine(MainTexLabel, _MainTex, _Color, _PreserveCol);
                    ShaderGUIHelper.ToggleLabel("Preserve Color", 145);
                }
                else {
                    materialEditor.TexturePropertySingleLine(AlbedoLabel, _MainTex, _Color);
                }
                materialEditor.TextureScaleOffsetProperty(_MainTex);
                GUILayout.Space(8);

                // Filters
                materialEditor.ShaderProperty(_FilterModel, "Filters");
                if (_FilterModel.floatValue == 1){
                    materialEditor.ShaderProperty(_HDR, "HDR", 1);
                    materialEditor.ShaderProperty(_Contrast, "Contrast", 1);
                    materialEditor.ShaderProperty(_SaturationRGB, "Saturation", 1);
                    materialEditor.ShaderProperty(_Brightness, "Brightness", 1);
                    materialEditor.ShaderProperty(_RAmt, "Red", 1);
                    materialEditor.ShaderProperty(_GAmt, "Green", 1);
                    materialEditor.ShaderProperty(_BAmt, "Blue", 1);
                }
                if (_FilterModel.floatValue == 2){
                    materialEditor.ShaderProperty(_AutoShift, "Auto Shift", 1);
                    if (_AutoShift.floatValue == 1)
                        materialEditor.ShaderProperty(_AutoShiftSpeed, "Speed", 1);
                    else
                        materialEditor.ShaderProperty(_Hue, "Hue", 1);
                    materialEditor.ShaderProperty(_SaturationHSL, "Saturation", 1);
                    materialEditor.ShaderProperty(_Luminance, "Luminance", 1);
                    materialEditor.ShaderProperty(_HSLMin, "Min Threshold", 1);
                    materialEditor.ShaderProperty(_HSLMax, "Max Threshold", 1);
                    materialEditor.ShaderProperty(_HDR, "HDR", 1);
                    materialEditor.ShaderProperty(_Contrast, "Contrast", 1);
                }
                GUILayout.Space(8);
            }

            // Toon Shading
            if (isToon){

                // Lighting
                showShading = ShaderGUIHelper.Foldout(shadingTex, showShading, 22.0f);
                
                if (showShading){
                    GUILayout.Label(PrimaryMapsLabel, EditorStyles.boldLabel);
                    materialEditor.TexturePropertySingleLine(NormalTexLabel, _BumpMap, usingNormal ? _BumpScale : null);
                    materialEditor.TexturePropertySingleLine(OcclusionMapLabel, _OcclusionMap, _OcclusionMap.textureValue ? _OcclusionStrength : null);
                    materialEditor.TexturePropertySingleLine(DetailMaskLabel, _DetailMask, _DetailMask.textureValue ? _DetailMaskStr : null);
                    materialEditor.TexturePropertySingleLine(LightingMaskLabel, _LightMask, _LightMask.textureValue ? _LightMaskStr : null);
                    materialEditor.TexturePropertySingleLine(ShadowMaskLabel, _ShadowMask, _ShadowMask.textureValue ? _ShadowMaskStr : null);
                    GUILayout.Label(DetailMapsLabel, EditorStyles.boldLabel);
                    materialEditor.TexturePropertySingleLine(DetailAlbedoLabel, _DetailAlbedoMap);
                    materialEditor.TexturePropertySingleLine(DetailNormalLabel, _DetailNormalMap, usingDetail ? _DetailNormalMapScale : null);
                    if (_DetailAlbedoMap.textureValue || usingDetail)
                        materialEditor.TextureScaleOffsetProperty(_DetailAlbedoMap);
                    GUILayout.Label(new GUIContent("Lighting"), EditorStyles.boldLabel);
                    ShaderGUIHelper.ToggleVector3("Static Light Direction", _StaticLightDirToggle, _StaticLightDir);
                    GUILayout.Space(8);
                    showReflections = ShaderGUIHelper.SimpleFoldout("    Reflections", showReflections);
                    materialEditor.ShaderProperty(_Reflections, " ");
                    if (showReflections){
                        GUILayout.Space(8);
                        EditorGUI.BeginDisabledGroup(_Reflections.floatValue == 0);
                        materialEditor.TexturePropertySingleLine(ReflCubeLabel, _ReflCube, _ReflCubeFallback);
                        ShaderGUIHelper.ToggleLabel("Fallback", 105);
                        materialEditor.ShaderProperty(_ReflSmooth, "Smoothness");
                        materialEditor.ShaderProperty(_ToonMetallic, "Metallic");
                        EditorGUI.EndDisabledGroup();
                        GUILayout.Space(8);
                    }
                    showSpecular = ShaderGUIHelper.SimpleFoldout("    Specular Highlights", showSpecular);
                    materialEditor.ShaderProperty(_FakeSpec, " ");
                    if (showSpecular){
                        GUILayout.Space(8);
                        EditorGUI.BeginDisabledGroup(_FakeSpec.floatValue == 0);
                        materialEditor.ShaderProperty(_SpecCol, "Color");
                        materialEditor.ShaderProperty(_SpecBias, "Bias");
                        materialEditor.ShaderProperty(_SpecStr, "Strength");
                        materialEditor.ShaderProperty(_SpecSize, "Smoothness");
                        EditorGUI.EndDisabledGroup();
                        GUILayout.Space(8);
                    }
                    showFresnel = ShaderGUIHelper.SimpleFoldout("    Fresnel", showFresnel);
                    GUILayout.Space(20);
                    bool canFresnel = _Reflections.floatValue == 0 && _FakeSpec.floatValue == 0;
                    if (showFresnel){
                        if (canFresnel){
                            GUILayout.Space(-6);
                            ShaderGUIHelper.CenteredText("REQUIRES REFLECTIONS OR SPECULAR HIGHLIGHTS", 0, 0);
                        }
                        EditorGUI.BeginDisabledGroup(canFresnel);
                        materialEditor.ShaderProperty(_FresnelStr, "Strength");
                        materialEditor.ShaderProperty(_IOR, "Radius");
                        materialEditor.ShaderProperty(_FresnelFade, "Fade");
                        materialEditor.ShaderProperty(_FresnelSmooth, "Smoothness");
                        materialEditor.ShaderProperty(_FresnelMetallic, "Metallic");
                        EditorGUI.EndDisabledGroup();
                        GUILayout.Space(8);
                    }
                    else
                        GUILayout.Space(-2);
                    showSSS = ShaderGUIHelper.SimpleFoldout("    Subsurface Scattering", showSSS);
                    materialEditor.ShaderProperty(_Subsurface, " ");
                    if (showSSS){
                        GUILayout.Space(8);
                        EditorGUI.BeginDisabledGroup(_Subsurface.floatValue == 0);
                        materialEditor.TexturePropertySingleLine(TranslucencyLabel, _TranslucencyMap);
                        materialEditor.ShaderProperty(_SColor, "Color");
                        materialEditor.ShaderProperty(_SStrength, "Strength");
                        materialEditor.ShaderProperty(_SPen, "Penetration");
                        materialEditor.ShaderProperty(_SSharp, "Sharpness");
                        EditorGUI.EndDisabledGroup();
                    }
                    GUILayout.Label(new GUIContent("Shadows"), EditorStyles.boldLabel);
                    materialEditor.TexturePropertySingleLine(ShadowRampLabel, _ShadowRamp);
                    materialEditor.ShaderProperty(_ShadowStr, "Strength");
                    EditorGUI.BeginDisabledGroup(usingRamp);
                    materialEditor.ShaderProperty(_RampWidth, "Smoothness");
                    EditorGUI.EndDisabledGroup();
                    GUILayout.Space(8);
                }
            }

            // PBR
            if (isPBR){    
                showShading = ShaderGUIHelper.Foldout(shadingTex, showShading, 22.0f);
                if (showShading){
                    GUILayout.Space(8);
                    materialEditor.ShaderProperty(_PBRWorkflow, "Workflow");
                    GUILayout.Label(PrimaryMapsLabel, EditorStyles.boldLabel);
                    if (_PBRWorkflow.floatValue == 0){
                        materialEditor.TexturePropertySingleLine(MetallicTexLabel, _MetallicGlossMap, !usingMetallic ? _Metallic : null);
                        materialEditor.TexturePropertySingleLine(RoughnessTexLabel, _SpecGlossMap, !usingRoughness ? _Glossiness : null, usingRoughness ? _InvertRough : null);
                        if (usingRoughness)
                            ShaderGUIHelper.ToggleLabel("Read as Smoothness", 180);
                    }
                    else {
                        materialEditor.TexturePropertySingleLine(SpecularTexLabel, _SpecGlossMap, !usingRoughness ? _PBRSpecCol : null);
                        if (usingRoughness)
                            materialEditor.ShaderProperty(_GlossMapScale, "Smoothness", 2);
                        else {
                            if (_SourceAlpha.floatValue == 1)
                                materialEditor.ShaderProperty(_GlossMapScale, "Smoothness", 2);
                            else
                                materialEditor.ShaderProperty(_Glossiness, "Smoothness", 2);
                        }
                        materialEditor.ShaderProperty(_SourceAlpha, "Source Alpha", 2);
                    }
                    materialEditor.TexturePropertySingleLine(NormalTexLabel, _BumpMap, usingNormal ? _BumpScale : null);
                    materialEditor.TexturePropertySingleLine(OcclusionMapLabel, _OcclusionMap, _OcclusionMap.textureValue != null ? _OcclusionStrength : null);
                    materialEditor.TexturePropertySingleLine(HeightTexLabel, _ParallaxMap, usingHeight ? _Parallax : null);
                    if (usingHeight){
                        materialEditor.ShaderProperty(_HeightContrast, "Height Contrast", 2);
                        materialEditor.ShaderProperty(_March, "High Quality", 2);
                    }
                    materialEditor.TexturePropertySingleLine(DetailMaskLabel, _DetailMask, _DetailMask.textureValue ? _DetailMaskStr : null);
                    materialEditor.TexturePropertySingleLine(ReflCubeLabel, _ReflCube, _ReflCubeFallback);
                    ShaderGUIHelper.ToggleLabel("Fallback", 105);
                    ShaderGUIHelper.ToggleVector3("Static Light Direction", _StaticLightDirToggle, _StaticLightDir);
                    GUILayout.Label(DetailMapsLabel, EditorStyles.boldLabel);
                    materialEditor.TexturePropertySingleLine(AlbedoLabel, _DetailAlbedoMap);
                    materialEditor.TexturePropertySingleLine(NormalTexLabel, _DetailNormalMap, usingDetail ? _DetailNormalMapScale : null);
                    if (_DetailAlbedoMap.textureValue || usingDetail)
                        materialEditor.TextureScaleOffsetProperty(_DetailAlbedoMap);
                    GUILayout.Space(8);
                }
            }

            // Emission
            if (usingMask)
                _UsingMask.floatValue = 1;
            Texture2D emissTex = (Texture2D)Resources.Load(emissFoldout, typeof(Texture2D));
            showEmiss = ShaderGUIHelper.Foldout(emissTex, showEmiss, 22.0f);
            if (showEmiss){
                GUILayout.Space(8);
                materialEditor.TexturePropertyWithHDRColor(EmissTexLabel, _EmissionMap, _EmissionColor, HDRField, false);
                ShaderGUIHelper.ToggleLabel("Brightness", 120);
                if (usingEmiss){
                    GUILayout.Space(2);
                    materialEditor.TextureScaleOffsetProperty(_EmissionMap);
                    GUILayout.Space(8);
                }
                materialEditor.TexturePropertySingleLine(EmissMaskLabel, _EmissMask);
                if (usingMask){
                    materialEditor.TextureScaleOffsetProperty(_EmissMask);
                }
                GUILayout.Space(8);
                materialEditor.ShaderProperty(_XScroll, "X Scrolling");
                materialEditor.ShaderProperty(_YScroll, "Y Scrolling");
                materialEditor.ShaderProperty(_PulseSpeed, "Pulse Speed");
                materialEditor.ShaderProperty(_ReactToggle, "Light Reactivity");
                EditorGUI.BeginDisabledGroup(_ReactToggle.floatValue == 0);
                materialEditor.ShaderProperty(_CrossMode, "Crossfade Mode", 2);
                EditorGUI.EndDisabledGroup();
                if (_CrossMode.floatValue == 1 && _ReactToggle.floatValue == 1){
                    materialEditor.ShaderProperty(_ReactThresh, "Threshold", 2);
                    materialEditor.ShaderProperty(_Crossfade, "Strength", 2);
                }
                GUILayout.Space(8);
            }

            // Rim Lighting
            if (!isBasic){
                Texture2D rimTex = (Texture2D)Resources.Load(rimFoldout, typeof(Texture2D));
                showRim = ShaderGUIHelper.Foldout(rimTex, showRim, 22.0f);
                if (showRim){
                    GUILayout.Space(8);
                    materialEditor.ShaderProperty(_RimBlending, "Blending");
                    GUILayout.Space(8);
                    materialEditor.TexturePropertySingleLine(new GUIContent("Color"), _RimTex, _RimCol, _UnlitRim);
                    ShaderGUIHelper.ToggleLabel("Unlit", 87);
                    if (_RimTex.textureValue != null){
                        materialEditor.TextureScaleOffsetProperty(_RimTex);
                        GUILayout.Space(10);
                    }
                    materialEditor.TexturePropertySingleLine(new GUIContent("Mask"), _RimMask, _RimMask.textureValue != null ? _RimMaskStr : null);
                    if (_RimMask.textureValue != null){
                        materialEditor.TextureScaleOffsetProperty(_RimMask);
                        GUILayout.Space(10);
                    }
                    materialEditor.ShaderProperty(_RimStrength, "Strength");
                    materialEditor.ShaderProperty(_RimWidth, "Width");
                    materialEditor.ShaderProperty(_RimEdge, "Sharpness");
                    materialEditor.ShaderProperty(_RimNoise, "Noise");
                    GUILayout.Space(15);
                }
            }
            
            Texture2D discord = (Texture2D)Resources.Load(discordPic, typeof(Texture2D));
            Texture2D discordIcon = (Texture2D)Resources.Load(discordIconPic, typeof(Texture2D));
            Texture2D version = (Texture2D)Resources.Load(versionPic, typeof(Texture2D));
            ShaderGUIHelper.CenteredTexture(discordIcon, discord, -20, 0, 0);
            ShaderGUIHelper.CenteredTexture(version, -10, 0);
        }
    }

    public static void SetBlendMode(Material material, BlendingMode mode) {
        EditorGUI.BeginChangeCheck();
        switch (mode) {
             case BlendingMode.FADE:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                break;   
            case BlendingMode.TRANSPARENT:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                break;
            default: break;
        }
    } 
}
