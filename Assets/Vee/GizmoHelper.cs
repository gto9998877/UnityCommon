using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vee.Debugs.CustomGizmos;

namespace Vee {
    public static class GizmoHelper {
        public static Gizmo_Base DrawPosition(GameObject root) {
            var mono = root.GetOrAddComponent<Gizmo_DrawSphereAtPosition>();
            mono.enabled = true;

            return mono;
        }
        
        public static void DrawBounds(Bounds b) {
            var p1 = b.center + new Vector3(-b.size.x / 2, -b.size.y / 2);
            var p2 = b.center + new Vector3(-b.size.x / 2, b.size.y / 2);
            var p3 = b.center + new Vector3(b.size.x / 2, b.size.y / 2);
            var p4 = b.center + new Vector3(b.size.x / 2, -b.size.y / 2);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }

        public static void DrawRect2D(Rect RectInWorld, Color color) {
            
            var drawCorners = new Vector3[5];
            
            var bottomLeft = new Vector3(RectInWorld.position.x, RectInWorld.position.y, 0f);
            drawCorners[0] = bottomLeft;
            drawCorners[1] = bottomLeft + new Vector3(0f, RectInWorld.height, 0f);
            drawCorners[2] = bottomLeft + new Vector3(RectInWorld.width, RectInWorld.height, 0f);
            drawCorners[3] = bottomLeft + new Vector3(RectInWorld.width, 0f, 0f);
            drawCorners[4] = bottomLeft;

            GizmoHelper.DrawLines(drawCorners, color);
        }
        
        public static void DrawRect2D(RectTransform rt, Color color) 
        {
            var drawCorners = new Vector3[5];
            rt.GetWorldCorners(drawCorners);
            drawCorners[4] = drawCorners[0];
            GizmoHelper.DrawLines(drawCorners, color);
        }
        
        public static void DrawLines(Vector3[] line, Color color) {
            if (line == null || line.Length <= 0) {
                return;
            }

            Gizmos.color = color;
            for (int i = 0; i < line.Length - 1; i++) {
                Gizmos.DrawLine(line[i], line[i + 1]);
            }
        }

        public static void DrawCircleXZ(Vector3 center, float radius, Color color, float deltaTheta = 0.1f) {
            if (deltaTheta < 0.0001f) deltaTheta = 0.0001f;

            //// 设置矩阵
            //Matrix4x4 defaultMatrix = Gizmos.matrix;
            //Gizmos.matrix = m_Transform.localToWorldMatrix;

            //// 设置颜色
            //Color defaultColor = Gizmos.color;
            //Gizmos.color = m_Color;

            // 绘制圆环
            List<Vector3> pointsOnCircle = new List<Vector3>();
            Vector3 firstPoint = center;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += deltaTheta) {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                Vector3 point = new Vector3(x, 0, z) + center;
                pointsOnCircle.Add(point);
                if (theta == 0) {
                    firstPoint = point;
                }
            }

            pointsOnCircle.Add(firstPoint);
            DrawLines(pointsOnCircle.ToArray(), color);
            //// 绘制最后一条线段
            //Gizmos.DrawLine(firstPoint, beginPoint);

            //// 恢复默认颜色
            //Gizmos.color = defaultColor;

            //// 恢复默认矩阵
            //Gizmos.matrix = defaultMatrix;
        }
    }
}