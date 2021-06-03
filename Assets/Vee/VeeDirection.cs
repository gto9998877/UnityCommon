using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vee
{
    public class Direction2D
    {
        public const int Origin = 0;
        public const int TopLeft = 1;
        public const int Top = 2;
        public const int TopRight = 3;
        public const int Right = 4;
        public const int BottomRight = 5;
        public const int Bottom = 6;
        public const int BottomLeft = 7;
        public const int Left = 8;

        public static bool IsNextToDirection(int dir, int dir2)
        {
            var temp = Mathf.Abs(dir - dir2);
            return (dir == 0 || dir2 == 0) || (temp == 1 || temp == 7);
        }

        public static bool IsTop(int dir)
        {
            return IsNextToDirection(dir, Top);
        }

        public static bool IsRight(int dir)
        {
            return IsNextToDirection(dir, Right);
        }

        public static bool IsLeft(int dir)
        {
            return IsNextToDirection(dir, Left);
        }

        public static bool IsBottom(int dir)
        {
            return IsNextToDirection(dir, Bottom);
        }

        public static Coordinate Direction2PointInTiled(int dir)
        {
            switch (dir)
            {
                case TopLeft:
                    return new Coordinate(-1, -1);
                case Top:
                    return new Coordinate(0, -1);
                case TopRight:
                    return new Coordinate(1, -1);
                case Right:
                    return new Coordinate(1, 0);
                case BottomRight:
                    return new Coordinate(1, 1);
                case Bottom:
                    return new Coordinate(0, 1);
                case BottomLeft:
                    return new Coordinate(-1, 1);
                case Left:
                    return new Coordinate(-1, 0);
                default:
                    return new Coordinate(0, 0);
            }
        }
        public static Coordinate Direction2PointInUnity(int dir)
        {
            switch (dir)
            {
                case TopLeft:
                    return new Coordinate(-1, 1);
                case Top:
                    return new Coordinate(0, 1);
                case TopRight:
                    return new Coordinate(1, 1);
                case Right:
                    return new Coordinate(1, 0);
                case BottomRight:
                    return new Coordinate(1, -1);
                case Bottom:
                    return new Coordinate(0, -1);
                case BottomLeft:
                    return new Coordinate(-1, -1);
                case Left:
                    return new Coordinate(-1, 0);
                default:
                    return new Coordinate(0, 0);
            }
        }

        public static int PointToDirection(Vector2 point)
        {
            var coo = point.ToCoordinate();
            switch (coo.y)
            {
                case -1:
                    {
                        switch (coo.x)
                        {
                            case -1:
                                return TopLeft;
                            case 0:
                                return Top;
                            case 1:
                                return TopRight;
                        }
                    }
                    break;
                case 0:
                    {
                        switch (coo.x)
                        {
                            case -1:
                                return Left;
                            case 0:
                                return Origin;
                            case 1:
                                return Right;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (coo.x)
                        {
                            case -1:
                                return BottomLeft;
                            case 0:
                                return Bottom;
                            case 1:
                                return BottomRight;
                        }
                    }
                    break;
            }

            return Origin;
        }

        public static string Direction2String(int dir)
        {
            switch (dir)
            {
                case TopLeft:
                    return "TopLeft";
                case Top:
                    return "Top";
                case TopRight:
                    return "TopRight";
                case Right:
                    return "Right";
                case BottomRight:
                    return "BottomRight";
                case Bottom:
                    return "Bottom";
                case BottomLeft:
                    return "BottomLeft";
                case Left:
                    return "Left";
                default:
                    return "Origin";
            }
        }

        public static int String2Direction(string name)
        {
            switch (name)
            {
                case "TopLeft":
                    return TopLeft;
                case "Top":
                    return Top;
                case "TopRight":
                    return TopRight;
                case "Right":
                    return Right;
                case "BottomRight":
                    return BottomRight;
                case "Bottom":
                    return Bottom;
                case "BottomLeft":
                    return BottomLeft;
                case "Left":
                    return Left;
                default:
                    return Origin;
            }
        }


        /// <summary>
        /// Gets the direction by angle.
        /// </summary>
        /// <returns>The direction by angle.</returns>
        /// <param name="angle">Angle.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetDirectionByAngle(float angle, bool only4Direction)
        {
            int dir = (int)((Mathf.Floor((Mathf.Floor(angle + (only4Direction ? 45f : 22.5f)) % 360f) / (only4Direction ? 90f : 45f)) + 1f) * (only4Direction ? 2f : 1f));
            return (only4Direction ? dir : GetDirectionClockwise(dir));
        }

        /// <summary>
        /// Gets the direction clockwise.
        /// </summary>
        /// <returns>The direction clockwise.</returns>
        /// <param name="dir">Dir.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetDirectionClockwise(int dir, bool only4Direction = false)
        {
            dir += 1 + (only4Direction ? 1 : 0);
            if (dir > Left) dir = (only4Direction ? Top : TopLeft);
            return dir;
        }

        /// <summary>
        /// Gets the direction counter clockwise.
        /// </summary>
        /// <returns>The direction counter clockwise.</returns>
        /// <param name="dir">Dir.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetDirectionCounterClockwise(int dir, bool only4Direction)
        {
            dir -= 1 + (only4Direction ? 1 : 0);
            if (dir < (only4Direction ? Top : TopLeft)) dir = Left;
            return dir;
        }

        /// <summary>
        /// Gets the next direction.
        /// </summary>
        /// <returns>The next direction.</returns>
        /// <param name="dir">Dir.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetNextDirection(int dir, bool only4Direction)
        {
            return GetDirectionClockwise(dir, only4Direction);
        }


        /// <summary>
        /// Gets the last direction.
        /// </summary>
        /// <returns>The last direction.</returns>
        /// <param name="dir">Dir.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetLastDirection(int dir, bool only4Direction)
        {
            return GetDirectionCounterClockwise(dir, only4Direction);
        }


        /// <summary>
        /// Gets the random direction.
        /// </summary>
        /// <returns>The random direction.</returns>
        /// <param name="escapeOrigin">If set to <c>true</c> escape origin.</param>
        /// <param name="only4Direction">If set to <c>true</c> only4 direction.</param>
        public static int GetRandomDirection(bool escapeOrigin, bool only4Direction)
        {
            float max = (only4Direction ? Left / 2.0f : Left / 1.0f) + (escapeOrigin ? 0f : 1f);
            float ret = Mathf.Floor(Random.value * 1000f) % max + (escapeOrigin ? 1f : 0f);
            return only4Direction ? (int)ret * 2 : (int)ret;
        }

        public static int Revert(int dir)
        {
            // rotate 180° to get a revert direction
            dir = GetDirectionClockwise(dir);
            dir = GetDirectionClockwise(dir);
            dir = GetDirectionClockwise(dir);
            dir = GetDirectionClockwise(dir);
            return dir;
        }

        public static bool IsHorizontal(int dir)
        {
            return (dir == Left || dir == Right);
        }

        public static bool IsVertical(int dir)
        {
            return (dir == Top || dir == Bottom);
        }
    }

}