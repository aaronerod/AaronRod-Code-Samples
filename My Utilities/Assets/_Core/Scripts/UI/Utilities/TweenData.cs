using UnityEngine;
using DG.Tweening;

namespace Rod.Utilities.Tweens
{
    /// <summary>
    /// Información de los tweens
    /// </summary>
   [CreateAssetMenu(fileName ="Tween Data", menuName = "Rod/Tweens/Tween Data")]
    public class TweenData : ScriptableObject
    {
        /// <summary>
        /// Tween en progreso
        /// </summary>
        Tween tween;
        /// <summary>
        /// Tipo de tween
        /// </summary>
        public TweenType tweenType;
        /// <summary>
        /// User propiedades de rect transform
        /// </summary>
        public bool useRectTransform;
        public bool useTransformValuesAsInitial;
        public bool useTransformValuesAsFinal;
        /// <summary>
        /// Valor inicial
        /// </summary>
        public Vector3 initialValue;
        /// <summary>
        /// Valor final
        /// </summary>
        public Vector3 endValue;
        /// <summary>
        /// Duración de tween normal
        /// </summary>
        public float tweenDuration = .1f;
        /// <summary>
        /// Duración de tween (random)
        /// </summary>
        public float tweenDuration2 = 0;
        /// <summary>
        /// Duración de tween en reversa
        /// </summary>
        public float tweenDurationReverse = .1f;
        /// <summary>
        /// Loops que correra este tween
        /// </summary>
        public int loops;
        /// <summary>
        /// Delay de tween normal
        /// </summary>
        public float delay;
        /// <summary>
        /// Delay de tween (random)
        /// </summary>
        public float delay2;
        /// <summary>
        /// Delay de tween en reversa
        /// </summary>
        public float delayReverse;
        /// <summary>
        /// Tipo de loop
        /// </summary>
        public LoopType loopType;
        /// <summary>
        /// Tipo de curva
        /// </summary>
        public Ease ease;
        /// <summary>
        /// Tipo de curva en reversa
        /// </summary>
        public Ease easeReverse;
        /// <summary>
        /// Amplitud de curva (No aplica en todos)
        /// </summary>
        public float easeAmplitude = -1;
        /// <summary>
        /// Periodo de curva (No aplica en todos)
        /// </summary>
        public float easePeriod = -1;
        /// <summary>
        /// Rect transform usado si aplica
        /// </summary>
        RectTransform rectTransform;

        /// <summary>
        /// Realizar tween normal
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Tween PerformTween(Transform target)
        {
            float tweenDuration = this.tweenDuration;
            float delay = this.delay;
            if (tweenDuration2 != 0)
                tweenDuration = Random.Range(this.tweenDuration, tweenDuration2);
            if (useRectTransform)
            {
                rectTransform = target.GetComponent<RectTransform>();
            }
            if (tween != null)
                tween.Kill();
            switch (tweenType)
            {
                case TweenType.Scale:
                    if (useRectTransform)
                    {

                        if (useTransformValuesAsFinal)
                            endValue = rectTransform.localScale;
                        if (!useTransformValuesAsInitial)
                            rectTransform.localScale = initialValue;
                        if (Application.isPlaying)
                            tween = rectTransform.DOScale(endValue, tweenDuration);
                        else
                            rectTransform.localScale = endValue;
                    }
                    else
                    {
                        if (useTransformValuesAsFinal)
                            endValue = target.localScale;
                        if (!useTransformValuesAsInitial)
                            target.localScale = initialValue;

                        if (Application.isPlaying)
                            tween = target.DOScale(endValue, tweenDuration);
                        else
                            target.localScale = endValue;
                    }
                    break;
                case TweenType.Rotation:
                    if (useRectTransform)
                    {
                        if (useTransformValuesAsFinal)
                            endValue = rectTransform.localEulerAngles;
                        if (!useTransformValuesAsInitial)
                            rectTransform.localRotation = Quaternion.Euler(initialValue);
                        if (Application.isPlaying)
                            tween = rectTransform.DOLocalRotate(endValue, tweenDuration);
                        else
                            rectTransform.localEulerAngles = endValue;
                    }
                    else
                    {
                        if (useTransformValuesAsFinal)
                            endValue = target.localScale;
                        if (!useTransformValuesAsInitial)
                            target.localRotation = Quaternion.Euler(initialValue);
                        if (Application.isPlaying)
                            tween = target.DOLocalRotate(endValue, tweenDuration);
                        else
                            target.localEulerAngles = endValue;
                    }
                    break;
                case TweenType.Position:
                    if (useRectTransform)
                    {
                        if (useTransformValuesAsFinal)
                            endValue = rectTransform.anchoredPosition;
                        if (!useTransformValuesAsInitial)
                            rectTransform.anchoredPosition = initialValue;
                        if (Application.isPlaying)
                            tween = rectTransform.DOAnchorPos(endValue, tweenDuration);
                        else
                            rectTransform.anchoredPosition = endValue;
                    }
                    else
                    {
                        if (useTransformValuesAsFinal)
                            endValue = target.localPosition;
                        if (!useTransformValuesAsInitial)
                            target.localPosition = initialValue;
                        if (Application.isPlaying)
                            tween = target.DOLocalMove(endValue, tweenDuration);
                        else
                            target.localPosition = endValue;
                    }
                    break;
                case TweenType.Alpha:
                    break;
            }
            if (delay2 > 0)
                delay = Random.Range(this.delay, delay2);
            SetTweenValues(tween, ease, delay);
            return tween;
        }

        /// <summary>
        /// Realizar tween en reversa
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Tween PerformReversedTween(Transform target)
        {
            Vector3 initialValue = this.endValue;
            Vector3 endValue = this.initialValue;
            float tweenDuration = tweenDurationReverse;
            if (easeReverse == Ease.Unset)
                easeReverse = ease;
            if (useRectTransform)
            {
                rectTransform = target.GetComponent<RectTransform>();
            }
            if (tween != null)
                tween.Kill();
            switch (tweenType)
            {
                case TweenType.Scale:
                    if (useRectTransform)
                    {
                        if (!useTransformValuesAsInitial)
                            rectTransform.localScale = initialValue;
                        if (Application.isPlaying)
                            tween = rectTransform.DOScale(endValue, tweenDuration);
                        else
                            rectTransform.localScale = endValue;
                    }
                    else
                    {
                        if (!useTransformValuesAsInitial)
                            target.localScale = initialValue;
                        if (Application.isPlaying)
                            tween = target.DOScale(endValue, tweenDuration);
                        else
                            target.localScale = endValue;
                    }
                    break;
                case TweenType.Rotation:
                    if (useRectTransform)
                    {
                        if (!useTransformValuesAsInitial)
                            rectTransform.localRotation = Quaternion.Euler(initialValue);
                        if (Application.isPlaying)
                            tween = rectTransform.DOLocalRotate(endValue, tweenDuration);
                        else
                            rectTransform.localEulerAngles = endValue;
                    }
                    else
                    {
                        if (!useTransformValuesAsInitial)
                            target.localRotation = Quaternion.Euler(initialValue);
                        if (Application.isPlaying)
                            tween = target.DOLocalRotate(endValue, tweenDuration);
                        else
                            target.localEulerAngles = endValue;
                    }
                    break;
                case TweenType.Position:
                    if (useRectTransform)
                    {
                        if (!useTransformValuesAsInitial)
                            rectTransform.anchoredPosition = initialValue;
                        if (Application.isPlaying)
                            tween = rectTransform.DOAnchorPos(endValue, tweenDuration);
                        else
                            rectTransform.anchoredPosition = endValue;
                    }
                    else
                    {
                        if (!useTransformValuesAsInitial)
                            target.localPosition = initialValue;
                        if (Application.isPlaying)
                            tween = target.DOLocalMove(endValue, tweenDuration);
                        else
                            target.localPosition = endValue;
                    }
                    break;
                case TweenType.Alpha:
                    break;
            }
            SetTweenValues(tween, easeReverse, delayReverse);
            return tween;
        }

        /// <summary>
        /// Realizar tween normal de manera inmediata
        /// </summary>
        /// <param name="target"></param>
        public void PerformImmediateTween(Transform target)
        {
            if (useRectTransform)
            {
                rectTransform = target.GetComponent<RectTransform>();
            }
            if (tween != null)
                tween.Kill();
            switch (tweenType)
            {
                case TweenType.Scale:
                    if (useRectTransform)
                    {
                        rectTransform.localScale = endValue;
                    }
                    else
                    {
                        target.localScale = endValue;
                    }
                    break;
                case TweenType.Rotation:
                    if (useRectTransform)
                    {
                        rectTransform.localEulerAngles = endValue;
                    }
                    else
                    {
                        target.localEulerAngles = endValue;
                    }
                    break;
                case TweenType.Position:
                    if (useRectTransform)
                    {
                        rectTransform.anchoredPosition = endValue;
                    }
                    else
                    {
                        target.localPosition = endValue;
                    }
                    break;
                case TweenType.Alpha:
                    break;
            }
        }

        /// <summary>
        /// Realizar tween en reversa de manera inmediata
        /// </summary>
        /// <param name="target"></param>
        public void PerformImmediateTweenReversed(Transform target)
        {
            Vector3 endValue = this.initialValue;
            if (useRectTransform)
            {
                rectTransform = target.GetComponent<RectTransform>();
            }
            if (tween != null)
                tween.Kill();
            switch (tweenType)
            {
                case TweenType.Scale:
                    if (useRectTransform)
                    {
                        rectTransform.localScale = endValue;
                    }
                    else
                    {
                        target.localScale = endValue;
                    }
                    break;
                case TweenType.Rotation:
                    if (useRectTransform)
                    {
                        rectTransform.localEulerAngles = endValue;
                    }
                    else
                    {
                        target.localEulerAngles = endValue;
                    }
                    break;
                case TweenType.Position:
                    if (useRectTransform)
                    {
                        rectTransform.anchoredPosition = endValue;
                    }
                    else
                    {
                        target.localPosition = endValue;
                    }
                    break;
                case TweenType.Alpha:
                    break;
            }
        }

        /// <summary>
        /// Detener tween
        /// </summary>
        public void StopTween()
        {
            if (tween != null)
                tween.Kill();
        }

        /// <summary>
        /// Configurar los valores del tween
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="ease"></param>
        /// <param name="delay"></param>
        void SetTweenValues(Tween tween, Ease ease, float delay)
        {
            if (loops != 0)
                tween.SetLoops(loops, loopType);
            if (easeAmplitude > 0 && easePeriod > 0)
                tween.SetEase(ease, easeAmplitude, easePeriod);
            else
                tween.SetEase(ease);
            if (this.delay > 0)
            {
                tween.SetDelay(delay);
            }
        }
    }

    public enum TweenType
    {
        Scale,
        Rotation,
        Position,
        Alpha
    }
}