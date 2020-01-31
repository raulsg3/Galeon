using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableIfOutOfSight : MonoBehaviour
{
    public RectTransform m_rectTransfom;
    private RectTransform topMostPos;
    private RectTransform bottomMostPos;
    private RectTransform leftMostPos;
    private RectTransform rightMostPos;

    private bool isVisible = true;
    private bool hasCheckAtLeastOnce =false;

    public Canvas canvasToHide;
    public List<RectTransform> parentsToSearch;
    public List<Image> imagesToHideShow;
    public List<TMPro.TextMeshProUGUI> textsToHideShow;
    private void Start()
    {
        isVisible = true;
    }
    #if UNITY_EDITOR
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
        #else
        [DebugButton]
        #endif
    #endif
    public void E_FillElements()
    {
       if(textsToHideShow == null) textsToHideShow = new List<TMPro.TextMeshProUGUI>();
       textsToHideShow.Clear();
       if(imagesToHideShow == null) imagesToHideShow = new List<Image>();
       imagesToHideShow.Clear();

        foreach (RectTransform RT in parentsToSearch)
        {
            TMPro.TextMeshProUGUI  [] texts = RT.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
            foreach (var item in texts)
            {

            textsToHideShow.Add(item);
            }

           Image  [] image = RT.GetComponentsInChildren<Image>(true);
           foreach (var item in image)
           {
           
           imagesToHideShow.Add(item);
           }
        }

    } 
    void OnEnable()
    {
        CheckVisibility(false);
        hasCheckAtLeastOnce=false;

    }
    void Update()
    {
        CheckVisibility();
    }

    public void CheckVisibility(bool forceChange = false)
    {
        if(DisableIfOutOfSightPositions.Instance == null) return;
        if(m_rectTransfom == null) m_rectTransfom = GetComponent<RectTransform>();
        if(leftMostPos == null) leftMostPos = DisableIfOutOfSightPositions.Instance.leftMostPos;
        if(rightMostPos == null) rightMostPos = DisableIfOutOfSightPositions.Instance.rightMostPos;
        if(topMostPos == null) topMostPos = DisableIfOutOfSightPositions.Instance.topMostPos;
        if(bottomMostPos == null) bottomMostPos = DisableIfOutOfSightPositions.Instance.bottomMostPos;

        if(IsInsideView())
        {
            OnBecomeVisible(forceChange);
        }
        else{
            OnBecomeInvisible(forceChange);
        }

    }

    public bool IsInsideView()
    {
        return m_rectTransfom.position.x > leftMostPos.position.x &&
                m_rectTransfom.position.x < rightMostPos.position.x &&
                m_rectTransfom.position.y < topMostPos.position.y &&
                m_rectTransfom.position.y > bottomMostPos.position.y;
    }

     private void OnBecomeVisible(bool forceChange = false)
    {
        if(isVisible && !forceChange && hasCheckAtLeastOnce)
        {
            return;
        }
        isVisible =true;
        hasCheckAtLeastOnce=true;
        SetImagesListState(true);
        SetCanvasState(true);
        SetTextListState(true);
    }
     private void OnBecomeInvisible(bool forceChange = false)
    {
        if(!isVisible && !forceChange && hasCheckAtLeastOnce)
        {
            return;
        }
        isVisible=false;
        hasCheckAtLeastOnce=true;
        SetImagesListState(false);
        SetCanvasState(false);
        
        SetTextListState(false);
    }
    private void SetCanvasState(bool status)
    {
        if(canvasToHide != null)
        canvasToHide.enabled = status;
    }

    private void SetImagesListState(bool status)
    {
        for (int i = 0; i < imagesToHideShow.Count; i++)
        {
            imagesToHideShow[i].enabled= status;
        }
    }
    private void SetTextListState(bool status)
    {
        for (int i = 0; i < textsToHideShow.Count; i++)
        {
            textsToHideShow[i].enabled= status;
        }
    }
}
