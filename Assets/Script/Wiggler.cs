using UnityEngine;
using System.Collections;

namespace Fie.Utility
{
    /// <summary>
    /// GameObjectを揺らすための汎用ユーティリティクラス
    /// </summary>
    public class Wiggler
    {
        public const string ANCHOR_OBJECT_SUFFIX = "Anchor";

        /// <summary>
        /// 揺れ段階の更新時に変更する揺れ角度の最少値
        /// </summary>
        const float MIN_RANDOM_SHAKE_ANGLE_RANGE = 120f;
        /// <summary>
        /// 揺れ段階の更新時に変更する揺れ角度の最大値
        /// </summary>
        const float MAX_RANDOM_SHAKE_ANGLE_RANGE = 240f;

        #region ---- variables -----
        private float nowAngle = 0f;
        private float nowTime = 0f;
        private float wiggleSegmentTime = 0f;

        private int totalWiggleCount = 1;
        private int nowWiggleCount = 0;

        private GameObject anchorObject = null;
        private Transform parentTransform = null;

        private Vector3 wiggleRange = Vector3.zero;
        private Vector3 nowWigglePoint = Vector3.zero;
        private Vector3 nextWigglePoint = Vector3.zero;
        private Vector3 nextPosition = Vector3.zero;
        #endregion

        /// <summary>
        /// 揺れは終了しているか？
        /// </summary>
        public bool isEnd
        {
            get
            {
                return (nowWiggleCount >= totalWiggleCount);
            }
        }

        #region ---- methods ----

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parentTransform">揺らす対象とするGameObjectのTransform</param>
        public Wiggler(Transform parentTransform)
        {
            this.parentTransform = parentTransform;

            // 親Transformの子としてアンカーオブジェクトを追加する
            anchorObject = new GameObject(this.parentTransform.gameObject.name + " " + ANCHOR_OBJECT_SUFFIX);
            anchorObject.transform.position = this.parentTransform.position;
            anchorObject.transform.parent = this.parentTransform;
            Initialize(0, 1, Vector3.zero);
        }

        /// <summary>
        /// 揺れの初期化
        /// </summary>
        /// <param name="totalTime">総振動時間(sec)</param>
        /// <param name="totalWiggleCount">総振動回数</param>
        /// <param name="wiggleRange">振動幅(m)</param>
        public void Initialize(float totalTime, int totalWiggleCount, Vector3 wiggleRange)
        {
            this.totalWiggleCount = Mathf.Max(totalWiggleCount, 1);
            this.wiggleRange = wiggleRange;

            nowTime = 0f;
            wiggleSegmentTime = Mathf.Max(totalTime, 0f) / (float)totalWiggleCount;

            if(wiggleSegmentTime <= 0f)
            {
                nowWiggleCount = totalWiggleCount;
            }
            else
            {
                nowWiggleCount = 0;
            }

            SetNextWigglePoint(nowWiggleCount);
        }

        /// <summary>
        /// ウィグラーを更新する。
        /// </summary>
        /// <returns>振動幅</returns>
        /// <param name="updateTime">更新する時間(sec)</param>
        public void UpdateWiggler(float updateTime)
        {
            if (wiggleSegmentTime <= 0) return;
            if (nowWiggleCount >= totalWiggleCount) return;

            // タイマーの更新
            nowTime += updateTime;

            // 線形補間で揺れポイント間の座標を求める
            Vector3 wigglePoint = (wiggleSegmentTime > 0f) ? Vector3.Slerp(nowWigglePoint, nextWigglePoint, Mathf.Min(nowTime / wiggleSegmentTime, 1f)) : Vector3.zero;

            // カウンタが一定値を超えたら揺れポイントの再設定
            if (nowTime > wiggleSegmentTime)
            {
                nowWiggleCount++;
                SetNextWigglePoint(nowWiggleCount);
                nowTime = 0f;
            }

            nextPosition = anchorObject.transform.position;
            anchorObject.transform.position = this.parentTransform.position - wigglePoint;
            this.parentTransform.position = nextPosition + wigglePoint;
        }

        /// <summary>
        /// 次の揺れポイントを設定する。
        /// </summary>
        /// <param name="count">Count.</param>
        private void SetNextWigglePoint(int count)
        {
            if (count < 0 || count > totalWiggleCount) return;
            if (parentTransform == null) return;

            // 現在の揺れポイントを保存
            nowWigglePoint = nextWigglePoint;

            // 揺れ幅割合を算出
            // 残り揺れ回数に反比例して揺れ幅割合は小さくなる
            float wigglePower = Mathf.Clamp((float)((float)totalWiggleCount - (float)count) / Mathf.Max((float)totalWiggleCount, 1f), 0f, 1f);

            // 揺れ幅の向きを決めるデグリー角を求める
            float nextAngle = nowAngle + Random.Range(MIN_RANDOM_SHAKE_ANGLE_RANGE, MAX_RANDOM_SHAKE_ANGLE_RANGE);

            // 親Transformのforwardベクトルを軸にして回転させた揺れ幅に揺れ幅割合を乗算する
            Quaternion normalQuatanion = Quaternion.AngleAxis(nextAngle, parentTransform.forward);
            Vector3 rotatedWiggleScale = normalQuatanion * (parentTransform.rotation * wiggleRange);
            nextWigglePoint = rotatedWiggleScale * wigglePower;

            nowAngle = nextAngle;
        }
        #endregion
    }
}
