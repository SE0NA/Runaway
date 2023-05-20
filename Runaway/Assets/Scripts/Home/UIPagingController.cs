using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ScrollRect))]

    public class UIPagingController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] protected GameObject obj_contentView = null;
        //   [SerializeField] protected UIPageControl pageControl;

        [SerializeField] private float animationDuration = 0.3f;
        // For Animation Curve
        private float key1InTan = 0f;
        private float key1OutTan = 1f;
        private float key2InTan = 1f;
        private float key2OutTan = 0f;

        private bool isAnimating = false;
        private Vector2 destPos;
        private Vector2 initPos;
        private AnimationCurve animCurve;
        private int prevPageIdx = 0;
        private Rect curViewRect;

        public RectTransform CachedRectTransform
        {
            get
            {
                return GetComponent<RectTransform>();
            }
        }
        public ScrollRect CachedScrollRect
        {
            get
            {
                return GetComponent<ScrollRect>();
            }
        }

        // �巡�� ���� �� ȣ��
        public void OnBeginDrag(PointerEventData eventData)
        {
            isAnimating = false;
        }
        // �巡�� ���� �� ȣ��
        public void OnEndDrag(PointerEventData eventData)
        {
            GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
            CachedScrollRect.StopMovement();    // ��ũ�� �� ����

            float pageWidth = -(grid.cellSize.x + grid.spacing.x);

            int pageIdx = Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);
            if(pageIdx == prevPageIdx && Mathf.Abs(eventData.delta.x) >= 4)
            {
                CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
                pageIdx += (int)Mathf.Sign(-eventData.delta.x);
            }

            // ù������ ������������ ��ũ�� ����
            if (pageIdx < 0)
                pageIdx = 0;
            else if (pageIdx > grid.transform.childCount - 1)
                pageIdx = grid.transform.childCount - 1;

            prevPageIdx = pageIdx;

            // ���� ��ũ�� ���
            float destX = pageIdx * pageWidth;
            destPos = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

            // ���� �� ��ũ�� ��ġ ����
            initPos = CachedScrollRect.content.anchoredPosition;

            // �ִϸ��̼� Ŀ��
            Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTan, key1OutTan);
            Keyframe keyFrame2 = new Keyframe(Time.time +animationDuration, 1.0f, key2InTan, key2OutTan);
            animCurve = new AnimationCurve(keyFrame1, keyFrame2);

            isAnimating = true;

        }


        void LateUpdate()
        {
            if (isAnimating)
            {
                if(Time.time >= animCurve.keys[animCurve.length - 1].time)
                {
                    // �ִϸ��̼� Ŀ�� ������ Ű������ �� �ִϸ��̼� ����
                    CachedScrollRect.content.anchoredPosition = destPos;
                    isAnimating = false;
                    return;
                }
                // �ִϸ��̼� Ŀ�꿡�� ���� ��ũ�� ��ġ ��� -> ��ũ�� �� �̵�
                Vector2 newPos = initPos + (destPos - initPos) * animCurve.Evaluate(Time.time);
                CachedScrollRect.content.anchoredPosition = newPos;
            }
        }

        void Start()
        {
            UpdateView();
        }

        // Update is called once per frame
        void Update()
        {
            // ��ũ�Ѻ� ��, ���� ��ȭ �� padding ����
            if(CachedRectTransform.rect.width != curViewRect.width || CachedRectTransform.rect.height != curViewRect.height)
            {
                UpdateView();
            }
        }
        
        private void UpdateView()
        {
            curViewRect = CachedRectTransform.rect;

            // content padding ��� �� ����
            GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
            int paddingH = Mathf.RoundToInt((curViewRect.width - grid.cellSize.x) / 2.0f);
            int paddingV = Mathf.RoundToInt((curViewRect.height - grid.cellSize.y) / 2.0f);
            grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);
        }
    }
}
