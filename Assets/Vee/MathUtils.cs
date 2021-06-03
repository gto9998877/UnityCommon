using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using Vee.Debugs;

namespace Vee {
    public abstract class MathUtils {
        #region Int

        /// <summary>
        /// 判断整数是否为奇数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsOdd(int number) {
            return ((number % 2) == 1);
        }

        /// <summary>
        /// 判断一个整数是不是2的次幂
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsPowerOf2(int v) {
            return ((v & (v - 1)) == 0);
        }

        #endregion // Int

        #region Float

        public const float PRECISION = 0.000001f;

        /// <summary>
        /// 判断两个浮点数是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool FloatEqual(float a, float b) {
            return FloatIsZero(a - b);
        }

        /// <summary>
        /// 判断浮点数是否为零
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool FloatIsZero(float f /*, bool accurate = false*/) {
            //          if (accurate) {
            //              unsafe
            //              {
            //                  int* ptrToInt = (int*)(void*)&f;
            //                  return ((*ptrToInt & (0x7fffffff)) == 0);
            //              }
            //          } else {
            return (Mathf.Abs(f) <= 0.00001f);
            //          }
        }

        /// <summary>
        /// 获取浮点数的符号(1为整数，-1为负数，0为0)
        /// </summary>
        /// <returns>The sign.</returns>
        /// <param name="srcValue">Source value.</param>
        public static int GetSign(float srcValue) {
            if (srcValue > 0) {
                return 1;
            }
            else if (srcValue < 0) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取数值的小数部分
        /// </summary>
        /// <returns>The fractional.</returns>
        /// <param name="number">Number.</param>
        public static float GetFractional(float number) {
            return Mathf.Clamp01(number - Mathf.Floor(number));
        }

        /// <summary>
        /// 计算浮点数的decay衰减后值
        /// </summary>
        /// <returns>The decay.</returns>
        /// <param name="srcValue">Source value.</param>
        /// <param name="decayFactor">Decay factor.</param>
        public static float CalcDecay(float srcValue, float decayFactor) {
            if (decayFactor > 0) decayFactor = -decayFactor;
            var mag = Mathf.Abs(srcValue) + decayFactor;
            if (mag < 0f) mag = 0f;
            return mag * GetSign(srcValue);
        }

        /// <summary>
        /// 两个浮点数之间按照给定的速率进行插值
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float valueTo(float current, float target, float speed) {
            var valueOffset = target - current;
            if (valueOffset > 0) {
                current += speed;
                current = current > target ? target : current;
            }
            else if (valueOffset < 0) {
                current -= speed;
                current = current < target ? target : current;
            }

            return current;
        }

        /// <summary>
        /// 计算0-1的进度值
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float CalcProgress(float current, float max) {
            return FloatIsZero(max) ? 1f : Mathf.Clamp01(current / max);
        }

        /// <summary>
        /// 从数组解析区间
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static Vector2 ParseRange(float[] arr) {
            if (arr == null || arr.Length <= 1) return default;
            var min = arr[0];
            var max = arr[1];
            if (max < min) {
                max = min;
            }
            return new Vector2(min, max);
        }
        
        #endregion // Float


        #region 空间计算

        /// <summary>
        /// 获取曼哈顿距离
        /// </summary>
        /// <returns>The distance.</returns>
        /// <param name="p">P.</param>
        public static float manhattanDistance(Vector2 p) {
            return Mathf.Abs(p.x) + Mathf.Abs(p.y);
        }

        /// <summary>
        /// 计算两个角度之间的最小夹角(两个角都是0-360)
        /// </summary>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        /// <returns></returns>
        public static float MinRotateAngleBetween(float angle1, float angle2) {
            float minAngle = Mathf.Min(angle1, angle2);
            float maxAngle = Mathf.Min(angle1, angle2);

            return Mathf.Min((maxAngle - minAngle), (minAngle + 360f - maxAngle));
        }

        /// <summary>
        /// 计算两点连线的角度(相对于up)
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public static float AngleOfLine(Vector2 lineStart, Vector2 lineEnd) {
            var dx = lineEnd.x - lineStart.x;
            var dy = lineEnd.y - lineStart.y;
            if (dx == 0) {
                return dy > 0 ? 0f : 180f;
            }

            float flat = (dx > 0 ? 1f : -1f);
            float rads = Mathf.Acos(dy / Mathf.Sqrt(dx * dx + dy * dy));
            float degree = flat * Mathf.Rad2Deg * rads;
            if (degree < 0) {
                degree += 360;
            }

            return degree;
        }

        /// <summary>
        /// 2D坐标系中，给定起点，角度，距离，计算终点
        /// </summary>
        /// <returns>The point with angle.</returns>
        /// <param name="center">Center.</param>
        /// <param name="distance">Distance.</param>
        /// <param name="angle">Angle. 角度值 非弧度</param>
        public static Vector2 GetPointWithAngle(Vector2 center, float distance, float angle) {
            // var radian = angle * Mathf.PI / 180;
            var radian = angle * Mathf.Deg2Rad;
            var deltaX = Mathf.Sin(radian) * distance;
            var deltaY = Mathf.Cos(radian) * distance;
            return new Vector2(center.x + deltaX, center.y + deltaY);
        }

        /// <summary>
        /// 2D坐标系中，给定速度和角度，计算速度向量
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 GetVelocityWithAngle(float speed, float angle) {
            var radian = angle * Mathf.Deg2Rad;
            var deltaX = Mathf.Sin(radian) * speed;
            var deltaY = Mathf.Cos(radian) * speed;
            return new Vector2(deltaX, deltaY);
        }

        /// <summary>
        /// 生成旋转向量
        /// </summary>
        /// <param name="position"></param>
        /// <param name="center"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle) {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            return resultVec3;
        }

        /// <summary>
        /// 给定起点，方向向量，距离，计算终点
        /// </summary>
        /// <param name="beginPos">起点</param>
        /// <param name="dir">方向</param>
        /// <param name="dis">距离</param>
        /// <returns></returns>
        public static Vector3 CalcDestination(Vector3 beginPos, Vector3 dir, float dis) {
            Ray ray = new Ray(beginPos, dir);
            return ray.GetPoint(dis);
        }

        /// <summary>
        /// 给定起点，方向向量，距离，计算终点(2D)
        /// </summary>
        /// <param name="beginPos">起点</param>
        /// <param name="dir">方向</param>
        /// <param name="dis">距离</param>
        /// <returns></returns>
        public static Vector2 CalcDestination2D(Vector2 beginPos, Vector2 dir, float dis) {
            Ray2D ray = new Ray2D(beginPos, dir);
            return ray.GetPoint(dis);
        }

        /// <summary>
        /// 计算直线和平面的交点
        /// </summary>
        /// <param name="point">直线上任意一点</param>
        /// <param name="direct">直线的方向向量</param>
        /// <param name="planeNormal">平面的法向量</param>
        /// <param name="planePoint">平面上任意一点</param>
        /// <returns></returns>
        public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal,
            Vector3 planePoint) {
            float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct, planeNormal);
            return d * direct.normalized + point;
        }

        /// <summary>
        /// 检测点与直线的关系
        /// </summary>
        /// <param name="lineP1">直线上点1</param>
        /// <param name="lineP2">直线上点2</param>
        /// <param name="checkP">检测点</param>
        /// <returns></returns>
        public static int CheckPointWithLineOnXZ(Vector3 lineP1, Vector3 lineP2, Vector3 checkP) {
            var s = (lineP1.x - checkP.x) * (lineP2.z - checkP.z) - (lineP1.z - checkP.z) * (lineP2.x - checkP.x);
            if (s > 0) {
                return 1; //点在线左侧
            }
            else if (s < 0) {
                return -1; //点在线右侧
            }
            else {
                return 0; //点在线上
            }
        }

        /// <summary>
        /// 返回直线上距离检测点最近的点(垂直投影点)
        /// </summary>
        /// <param name="lineP1">直线上点1</param>
        /// <param name="lineP2">直线上点2</param>
        /// <param name="checkP">检测点</param>
        /// <returns></returns>
        public static Vector3 CalcNeareatPointOnLine(Vector3 lineP1, Vector3 lineP2, Vector3 checkP) {
            Vector3 ab = lineP2 - lineP1;
            Vector3 ac = checkP - lineP1;
            float f = Vector3.Dot(ab, ac);
            float d = Vector3.Dot(ab, ab);
            f = f / d;

            return (lineP1 + f * ab);
        }

        /// <summary>
        /// 计算点到线段的最短距离
        /// </summary>
        /// <param name="lineP1">线段端点1</param>
        /// <param name="lineP2">线段端点2</param>
        /// <param name="checkP">检测点</param>
        /// <returns></returns>
        public static float CalcShortestDistanceBetweenPointAndLine(Vector3 lineP1, Vector3 lineP2, Vector3 checkP) {
            Vector3 ab = lineP2 - lineP1;
            Vector3 ac = checkP - lineP1;
            float f = Vector3.Dot(ab, ac);
            if (f < 0) return Vector3.Distance(lineP1, checkP);
            float d = Vector3.Dot(ab, ab);
            if (f > d) return Vector3.Distance(lineP1, checkP);
            f = f / d;
            Vector3 D = lineP1 + f * ab; // c在ab线段上的投影点
            return Vector3.Distance(lineP1, D);
        }

        /// <summary>
        /// 从点的列表中寻找距离目标点最近的点
        /// </summary>
        /// <param name="targetPoint"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Vector3 FindNearestPointInList(Vector3 targetPoint, List<Vector3> list) {
            if (list == null || list.Count <= 0) return targetPoint;
            Vector3 nearestPoint = list[0];
            float nearestDis = Vector3.Distance(targetPoint, nearestPoint);
            int listCnt = list.Count;
            for (var i = 1; i < listCnt; ++i) {
                var point = list[i];
                float dis = Vector3.Distance(targetPoint, point);
                if (dis < nearestDis) {
                    nearestPoint = point;
                    nearestDis = dis;
                }
            }

            return nearestPoint;
        }

        #endregion // 空间计算


        #region  Bezier

        /// <summary>
        /// 根据T值，计算二次贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t"></param>T值（0-1）
        /// <param name="p0"></param>起始点
        /// <param name="p1"></param>控制点
        /// <param name="p2"></param>目标点
        /// <returns></returns>根据T值计算出来的贝赛尔曲线点
        public static Vector3 CalcBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }

        /// <summary>
        /// 根据T值，计算三次贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Vector3 CalcBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        /// <summary>
        /// 计算任意数量控制点的贝塞尔曲线
        /// </summary>
        /// <param name="t"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Vector3 CalcBezierPoint(float t, List<Vector3> points) {
            var pointCount = points.Count;
            if (pointCount == 3) {
                return CalcBezierPoint(t, points[0], points[1], points[2]);
            }
            else if (pointCount == 4) {
                return CalcBezierPoint(t, points[0], points[1], points[2], points[3]);
            }
            else {
                VeeDebug.LogWarning("Bezier now can not over 4 points");
                return Vector3.zero;
            }
        }


        /// <summary>
        /// 获取存储贝塞尔曲线采样点
        /// </summary>
        /// <param name="startPoint"></param>起始点
        /// <param name="controlPoint"></param>控制点
        /// <param name="endPoint"></param>目标点
        /// <param name="segmentNum"></param>采样点的数量(最小2，即起点和终点)
        /// <returns></returns>存储贝塞尔曲线点的数组
        public static Vector3[] SampleBeizerPath(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint,
            int segmentNum) {
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 0; i <= segmentNum; i++) {
                float t = (float) i / (float) segmentNum;
                Vector3 point = CalcBezierPoint(t, startPoint,
                    controlPoint, endPoint);
                path[i] = point;
                // Debug.Log (path[i - 1]);
            }

            return path;
        }

        #endregion // Bezier

        #region Hash

        /// <summary>
        /// time33
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static uint HashTime33(string key) {
            var len = key.Length;
            uint hash = 0; 
            for (int i = 0; i < len; i++) { 
                hash = hash * 33 + (uint) key[i]; 
            } 
            return (hash & 0x7FFFFFFF);
        }

        #endregion // Hash
    }
}