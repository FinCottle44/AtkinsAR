using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class buttoncontroller : MonoBehaviour {

    public RectTransform btnCone, btnRing, btnBarrier;

    public Vector3 conePos, ringPos, barrierPos;

    private Vector3 HiddenPos;

	// Use this for initialization
	void Start () {

        conePos = btnCone.anchoredPosition;
        ringPos = btnRing.anchoredPosition;
        barrierPos = btnBarrier.anchoredPosition;
        HiddenPos = new Vector3(100f, 100f, 0f);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Button("Show")]
    public void Show()
    {
        btnCone.gameObject.SetActive(true);
        btnRing.gameObject.SetActive(true);
        btnBarrier.gameObject.SetActive(true);
        btnCone.DOAnchorPos(conePos, 1f);
        btnRing.DOAnchorPos(ringPos, 1f);
        btnBarrier.DOAnchorPos(barrierPos, 1f);
    }

    [Button("Hide")]
    public void Hide()
    {
        btnCone.DOAnchorPos(HiddenPos, 1f).OnComplete(() =>
        {
            btnCone.gameObject.SetActive(false);
        });
        btnRing.DOAnchorPos(HiddenPos, 1f).OnComplete(() =>
        {
            btnRing.gameObject.SetActive(false);
        });
        btnBarrier.DOAnchorPos(HiddenPos, 1f).OnComplete(() =>
        {
            btnBarrier.gameObject.SetActive(false);
        });
    }
}
