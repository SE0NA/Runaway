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

        // 드래그 시작 시 호출
        public void OnBeginDrag(PointerEventData eventData)
        {
            isAnimating = false;
        }
        // 드래그 종료 시 호출
        public void OnEndDrag(PointerEventData eventData)
        {
            GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
            CachedScrollRect.StopMovement();    // 스크롤 뷰 정지

            float pageWidth = -(grid.cellSize.x + grid.spacing.x);

            int pageIdx = Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);
            if(pageIdx == prevPageIdx && Mathf.Abs(eventData.delta.x) >= 4)
            {
                CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
                pageIdx += (int)Mathf.Sign(-eventData.delta.x);
            }

            // 첫페이지 마지막페이지 스크롤 제한
            if (pageIdx < 0)
                pageIdx = 0;
            else if (pageIdx > grid.transform.childCount - 1)
                pageIdx = grid.transform.childCount - 1;

            prevPageIdx = pageIdx;

            // 최종 스크롤 계산
            float destX = pageIdx * pageWidth;
            destPos = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

            // 시작 시 스크롤 위치 저장
            initPos = CachedScrollRect.content.anchoredPosition;

            // 애니메이션 커브
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
                    // 애니메이션 커브 마지막 키프레임 후 애니메이션 종료
                    CachedScrollRect.content.anchoredPosition = destPos;
                    isAnimating = false;
                    return;
                }
                // 애니메이션 커브에서 현재 스크롤 위치 계산 -> 스크롤 뷰 이동
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
            // 스크롤뷰 폭, 높이 변화 시 padding 갱신
            if(CachedRectTransform.rect.width != curViewRect.width || CachedRectTransform.rect.height != curViewRect.height)
            {
                UpdateView();
            }
        }
        
        private void UpdateView()
        {
            curViewRect = CachedRectTransform.rect;

            // content padding 계산 및 설정
            GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
            int paddingH = Mathf.RoundToInt((curViewRect.width - grid.cellSize.x) / 2.0f);
            int paddingV = Mathf.RoundToInt((curViewRect.height - grid.cellSize.y) / 2.0f);
            grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);
        }
    }
}
