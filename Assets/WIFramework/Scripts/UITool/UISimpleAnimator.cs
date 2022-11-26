using System.Collections;
using UnityEngine;
using static WIFramework.Util.EasingFunction;

namespace WIFramework.UI
{
    public class UISimpleAnimator : MonoBehaviour
    {
        public float speed;
        public EaseType normalEasingType;
        public EaseType rewindEasingType;

        [Header("Position")]
        [SerializeField]Vector3 originPosition;
        [SerializeField]Vector3 targetPosition;
        [SerializeField] float posTargetProcess;
        [Header("Rotation")]
        [SerializeField]Quaternion originRotation;
        [SerializeField]Quaternion targetRotation;
        [SerializeField] float rotTargetProcess;
        [Header("Size")]
        [SerializeField]Vector2 originSizeDelta;
        [SerializeField]Vector2 targetSizeDelta;
        [SerializeField] float sizeTargetProcess;
        [Header("Option")]
        public bool AutoSpeed;
        RectTransform rect;

        Coroutine anim;
        Vector3 startPos;
        Vector2 startSize;
        Quaternion startRot;
        public delegate void UIAnimationEndEvent();
        public UIAnimationEndEvent endEvent;

        float posProcess;
        float rotProcess;
        float sizeProcess;
        void CalculateProcess()
        {
            rect = GetComponent<RectTransform>();
            startPos = rect.localPosition;
            startRot = rect.localRotation;
            posTargetProcess = Vector3.Distance(originPosition, targetPosition);
            sizeTargetProcess = Vector2.Distance(originSizeDelta, targetSizeDelta);
            rotTargetProcess = Vector3.Distance(originRotation.eulerAngles, targetRotation.eulerAngles);
        }

        #region EditorAction
        public void SaveOrigin()
        {
            rect = GetComponent<RectTransform>();
            originPosition = rect.localPosition;
            originRotation = rect.localRotation;
            originSizeDelta = rect.sizeDelta;
            CalculateProcess();
        }
        public void SaveTarget()
        {
            rect = GetComponent<RectTransform>();
            targetPosition = rect.localPosition;
            targetRotation = rect.localRotation;
            targetSizeDelta = rect.sizeDelta;
            CalculateProcess();
        }
        public void SetOrigin()
        {
            rect = GetComponent<RectTransform>();
            rect.localPosition = originPosition;
            rect.localRotation = originRotation;
            rect.sizeDelta = originSizeDelta;
        }
        public void SetTarget()
        {
            rect = GetComponent<RectTransform>();
            rect.localPosition = targetPosition;
            rect.localRotation = targetRotation;
            rect.sizeDelta = targetSizeDelta;
        }
        #endregion
        void Ready()
        {
            if (anim != null)
                StopCoroutine(anim);

            ResetProcess();
        }
        public void Rewind()
        {
            Ready();
            var ef = GetEasingFunction(rewindEasingType);
            anim = AutoSpeed ? StartCoroutine(RewindAnimation_AutoSpeed(ef)) : StartCoroutine(RewindAnimation(ef));
        }
        public void Play()
        {
            Ready();
            var ef = GetEasingFunction(normalEasingType);
            anim = AutoSpeed ? StartCoroutine(Animation_AutoSpeed(ef)): StartCoroutine(Animation(ef));
        }
        void ResetProcess()
        {
            posProcess = 0f;
            rotProcess = 0f;
            sizeProcess = 0f;
            CalculateProcess();
        }

        public void OnComplete(UIAnimationEndEvent endEvent)
        {
            this.endEvent += endEvent;
        }

        #region Animation Coroutine
        IEnumerator Animation(Function ef)
        {
            while (!processCheck(posProcess, rotProcess, sizeProcess))
            {
                if (posTargetProcess > 0f)
                    PositionUpdate(ef);
                else
                    posProcess = 1f;

                if (rotTargetProcess > 0f)
                    RotationUpdate(ef);
                else
                    rotProcess = 1f;

                if (sizeTargetProcess > 0f)
                    SizeUpdate(ef);
                else
                    sizeProcess = 1f;

                yield return null;
            }
            //Debug.Log($"{posProcess}:{rotProcess}:{sizeProcess}");
            endEvent?.Invoke();
        }
        IEnumerator RewindAnimation(Function ef)
        {
            while (!processCheck(posProcess, rotProcess, sizeProcess))
            {
                if (posTargetProcess > 0f)
                    RewindPositionUpdate(ef);
                else
                    posProcess = 1f;

                if (rotTargetProcess > 0f)
                    RewindRotationUpdate(ef);
                else
                    rotProcess = 1f;

                if (sizeTargetProcess > 0f)
                    RewindSizeUpdate(ef);
                else
                    sizeProcess = 1f;

                yield return null;
            }
            //Debug.Log($"{posProcess}:{rotProcess}:{sizeProcess}");
            endEvent?.Invoke();
        }
        IEnumerator RewindAnimation_AutoSpeed(Function ef)
        {
            while (!processCheck(posProcess, rotProcess, sizeProcess))
            {
                if (posTargetProcess > 0f)
                    RewindPositionUpdate_AutoSpeed(ef);
                else
                    posProcess = 1f;
                if (rotTargetProcess > 0f)
                    RewindRotationUpdate_AutoSpeed(ef);
                else
                    rotProcess = 1f;
                if (sizeTargetProcess > 0f)
                    RewindSizeUpdate_AutoSpeed(ef);
                else
                    sizeProcess = 1f;
                yield return null;
            }
            //Debug.Log($"{posProcess}:{rotProcess}:{sizeProcess}");
            endEvent?.Invoke();
        }
        IEnumerator Animation_AutoSpeed(Function ef)
        {
            while (!processCheck(posProcess, rotProcess, sizeProcess))
            {
                if (posTargetProcess > 0f)
                    PositionUpdate_AutoSpeed(ef);
                else
                    posProcess = 1f;
                if (rotTargetProcess > 0f)
                    RotationUpdate_AutoSpeed(ef);
                else
                    rotProcess = 1f;
                if (sizeTargetProcess > 0f)
                    SizeUpdate_AutoSpeed(ef);
                else
                    sizeProcess = 1f;
                yield return null;
            }
            //Debug.Log($"{posProcess}:{rotProcess}:{sizeProcess}");
            endEvent?.Invoke();
        }
        #endregion
        #region Size
        void RewindSizeUpdate(Function ef)
        {
            sizeProcess += speed * Time.deltaTime / sizeTargetProcess;
            var p = ef(0f, sizeTargetProcess, sizeProcess);
            rect.sizeDelta = Vector2.Lerp(startSize, originSizeDelta, p);
        }
        void RewindSizeUpdate_AutoSpeed(Function ef)
        {
            sizeProcess += Time.deltaTime;
            var p = ef(0f, 1f, sizeProcess);
            rect.sizeDelta = Vector2.Lerp(startSize, originSizeDelta, p);
        }

        void SizeUpdate_AutoSpeed(Function ef)
        {
            sizeProcess += Time.deltaTime;
            var p = ef(0f, 1f, sizeProcess);
            rect.sizeDelta = Vector2.Lerp(startSize, targetSizeDelta, p);
        }

        void SizeUpdate(Function ef)
        {
            sizeProcess += speed * Time.deltaTime / sizeTargetProcess;
            var p = ef(0f, sizeTargetProcess, sizeProcess );
            rect.sizeDelta = Vector2.Lerp(startSize, targetSizeDelta, p);
        }
#endregion
        #region Rotation
        void RotationUpdate(Function ef)
        {
            rotProcess += speed * Time.deltaTime / rotTargetProcess;
            var p = ef(0f, rotTargetProcess, rotProcess);
            var l = Vector3.Lerp(startRot.eulerAngles, targetRotation.eulerAngles, p);
            rect.localRotation= Quaternion.Euler(l);
        }
        void RotationUpdate_AutoSpeed(Function ef)
        {
            rotProcess += Time.deltaTime;
            var p = ef(0f, 1f, rotProcess);
            var l = Vector3.Lerp(startRot.eulerAngles, targetRotation.eulerAngles, p);
            rect.localRotation = Quaternion.Euler(l);
        }

        void RewindRotationUpdate(Function ef)
        {
            rotProcess += speed * Time.deltaTime / rotTargetProcess;
            var p = ef(0f, rotTargetProcess, rotProcess);
            var l = Vector3.Lerp(startRot.eulerAngles, originRotation.eulerAngles, p);
            rect.localRotation = Quaternion.Euler(l);
        }
        void RewindRotationUpdate_AutoSpeed(Function ef)
        {
            rotProcess += Time.deltaTime ;
            var p = ef(0f, 1f, rotProcess);
            var l = Vector3.Lerp(startRot.eulerAngles, originRotation.eulerAngles, p);
            rect.localRotation = Quaternion.Euler(l);
        }

#endregion
        #region Position
        void PositionUpdate(Function ef)
        {
            posProcess += speed * Time.deltaTime / posTargetProcess;
            var p1 = ef(0f, posTargetProcess, posProcess);
            var l1 = Vector3.Lerp(startPos, targetPosition, p1);
            rect.localPosition = l1;
        }
        void PositionUpdate_AutoSpeed(Function ef)
        {
            posProcess += Time.deltaTime;
            var p1 = ef(0f, 1f, posProcess);
            var l1 = Vector3.Lerp(startPos, targetPosition, p1);
            rect.localPosition = l1;
        }
        void RewindPositionUpdate(Function ef)
        {
            posProcess += Time.deltaTime;
            var p1 = ef(0f, posTargetProcess, posProcess);
            var l1 = Vector3.Lerp(startPos, originPosition, p1);
            rect.localPosition = l1;
        }
        void RewindPositionUpdate_AutoSpeed(Function ef)
        {
            posProcess += Time.deltaTime;
            var p1 = ef(0f, 1f, posProcess);
            var l1 = Vector3.Lerp(startPos, originPosition, p1);
            rect.localPosition = l1;
        }
#endregion
        bool processCheck(float p1, float p2, float p3)
        {
            if (p1 < 1f)
                return false;
            if (p2 < 1f)
                return false;
            if (p3 < 1f)
                return false;
            return true;
        }
    }
}