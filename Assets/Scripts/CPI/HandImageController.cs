using DG.Tweening;
using UnityEngine;

namespace CPI
{
    public class HandImageController : MonoBehaviour
    {
        [SerializeField] private RectTransform handImageTransform;
        [SerializeField] private int sortingLayer=10;


        private Tween _lastAnimation;
        private Vector3 _baseScale;
        
        private void Start()
        {
            _baseScale = handImageTransform.localScale;
            handImageTransform.GetComponent<Renderer>().sortingOrder=sortingLayer;

        }


        private void Update()
        {

            var mousePos = Input.mousePosition;
            handImageTransform.transform.position = mousePos;
            
            
            if (Input.GetMouseButtonDown(0))
            {
                
                OnDownAnimation();
            }


            if (Input.GetMouseButtonUp(0))
            {
                OnUpAnimation();
            }
            
            
            
            
        }


        private void OnDownAnimation()
        {

            _lastAnimation?.Kill();
            var targetScale = _baseScale * .8f;
            _lastAnimation = handImageTransform.transform.DOScale(targetScale, .25f).SetEase(Ease.Linear);

        }

        private void OnUpAnimation()
        {
            _lastAnimation?.Kill();
            _lastAnimation = handImageTransform.transform.DOScale(_baseScale, .25f).SetEase(Ease.Linear);
        }
    }
}