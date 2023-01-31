using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    [SerializeField] private PostprocessMaterials _postprocessMaterials;
    [SerializeField] private Material _blurMaterial;
    [SerializeField] private float _changeSpeed = 1.0f;
    [SerializeField] private float _defauitBlur = 0.0f, _aimBlur = 15.0f, _addShiftBlur = 5.0f;
    [SerializeField] private float _defauitCircle = 500.0f, _aimCircle = 400.0f, _addShiftCircle = -100.0f;

    private float blurTarget;
    private float addBlurShift;

    private float circleTarget;
    private float addCircleShift;

    private float blurAmount, circleValue;
    private int blurId, circleRadiusId;

    private void Awake()
    {
        blurId = Shader.PropertyToID("_BlurSize");
        circleRadiusId = Shader.PropertyToID("_CenterSize");

        blurTarget = _defauitBlur;
        circleTarget = _defauitCircle;
        addBlurShift = 0;
        addCircleShift = 0;

        InputHandler.RightMouseDowned += () => { blurTarget = _aimBlur; circleTarget = _aimCircle; };
        InputHandler.RightMouseUped += () => { blurTarget = _defauitBlur; circleTarget = _defauitCircle; };
        InputHandler.ShiftPressed += () => 
        { 
            addBlurShift = HeroState.IsIdleAim ? _addShiftBlur : 0; 
            addCircleShift = HeroState.IsIdleAim ? _addShiftCircle : 0;
        };
        InputHandler.ShiftUped += () => { addBlurShift = 0; addCircleShift = 0; };
    }

    private void FixedUpdate()
    {
        blurAmount = Mathf.Lerp(blurAmount, HeroState.IsWeaponOnHand ?
                                            blurTarget + addBlurShift : _defauitBlur,
                                            Time.fixedDeltaTime * _changeSpeed);
        circleValue = Mathf.Lerp(circleValue, HeroState.IsWeaponOnHand ?
                                              circleTarget + addCircleShift :_defauitCircle,
                                              Time.fixedDeltaTime * _changeSpeed);
        _blurMaterial.SetFloat(blurId, blurAmount);
        _blurMaterial.SetFloat(circleRadiusId, circleValue);
    }
}
