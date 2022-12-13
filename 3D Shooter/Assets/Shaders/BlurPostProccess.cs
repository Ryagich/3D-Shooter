using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[Serializable, VolumeComponentMenuForRenderPipeline("CustomEffectComponent",
    typeof(UniversalRenderPipeline))]
public class BlurPostProccess : VolumeComponent, IPostProcessComponent
{
    public NoInterpFloatParameter Strength = new NoInterpFloatParameter(0);

    public bool IsActive() => Strength.value > 0;

    public bool IsTileCompatible() => false;
}
