using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
//using Sirenix.OdinInspector;

public class buttoncontroller : MonoBehaviour
{

    public RectTransform btnCone, btnRing, btnBarrier, Button, btnPreset;

    public Vector3 conePos, ringPos, barrierPos, presetPos;

    private Vector3 HiddenPos;

    // Use this for initialization
    void Start()
    {
        btnCone.gameObject.SetActive(false);
        btnRing.gameObject.SetActive(false);
        btnBarrier.gameObject.SetActive(false);
        btnPreset.gameObject.SetActive(false);
        conePos = btnCone.anchoredPosition;
        ringPos = btnRing.anchoredPosition;
        barrierPos = btnBarrier.anchoredPosition;
        presetPos = btnPreset.anchoredPosition;
        HiddenPos = new Vector3(0f, 0f, 0f);
        Hide();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //[Button("Show")]
    public void Show()
    {
        btnCone.gameObject.SetActive(true);
        btnRing.gameObject.SetActive(true);
        btnBarrier.gameObject.SetActive(true);
        btnPreset.gameObject.SetActive(true);
        btnCone.DOAnchorPos(conePos, 0.5f);
        btnRing.DOAnchorPos(ringPos, 0.5f);
        btnBarrier.DOAnchorPos(barrierPos, 0.5f);
        btnPreset.DOAnchorPos(presetPos, 0.5f);

        Button.gameObject.SetActive(true);
    }

    //[Button("Hide")]
    public void Hide()
    {
        btnCone.DOAnchorPos(HiddenPos, 0.3f).OnComplete(() =>
        {
            btnCone.gameObject.SetActive(false);
        });
        btnRing.DOAnchorPos(HiddenPos, 0.3f).OnComplete(() =>
        {
            btnRing.gameObject.SetActive(false);
        });
        btnBarrier.DOAnchorPos(HiddenPos, 0.3f).OnComplete(() =>
        {
            btnBarrier.gameObject.SetActive(false);
        });
        btnPreset.DOAnchorPos(HiddenPos, 0.3f).OnComplete(() =>
        {
            btnPreset.gameObject.SetActive(false);
        });
        Button.gameObject.SetActive(false);
    }
}