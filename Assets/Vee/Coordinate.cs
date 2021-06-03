using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vee {
    [Serializable]
    public class Coordinate {
        public int x;
        public int y;

        public int ToIDX(int columns) {
            return y * columns + x;
        }

        public float DistanceTo(Coordinate target) {
            return Vector2.Distance(new Vector2(x, y), new Vector2(target.x, target.y));
        }

        public Coordinate(Coordinate coo) {
            this.x = coo.x;
            this.y = coo.y;
        }

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Coordinate(int Idx, int cols, bool fromIDX) {
            this.x = Idx % cols;
            this.y = Idx / cols;
        }

        public static Coordinate operator +(Coordinate left, Coordinate right) {
            return new Coordinate(left.x + right.x, left.y + right.y);
        }

        public static Coordinate operator -(Coordinate left, Coordinate right) {
            return new Coordinate(left.x - right.x, left.y - right.y);
        }

        public static bool operator !=(Coordinate left, Coordinate right) {
            if ((left as object) != null)
                return !left.Equals(right);
            else
                return ((right as object) != null);
        }

        public static bool operator ==(Coordinate left, Coordinate right) {
            if ((left as object) != null)
                return left.Equals(right);
            else
                return ((right as object) == null);
        }

        public override bool Equals(object obj) {
            // if parameter is null return false
            if (obj == null) {
                return false;
            }

            // if parameter cannot be cast to Setting return false
            Coordinate coo = obj as Coordinate;
            if ((System.Object) coo == null) {
                return false;
            }

            // return true if the fields match
            if (x != coo.x || y != coo.y)
                return false;
            else
                return true;
        }

        public override int GetHashCode() {
            return x;
        }

        public override string ToString() {
            return "(" + x + "," + y + ")";
        }

        public static bool TryParse(string cooStr, out Coordinate coo) {
            cooStr = cooStr.Trim().TrimStart('(').TrimEnd(')');
            var nums = cooStr.Split(',');
            if (nums.Length == 2) {
                int x = 0;
                int y = 0;
                if (int.TryParse(nums[0], out x) && int.TryParse(nums[1], out y)) {
                    coo = new Coordinate(x, y);
                    return true;
                }
            }

            coo = new Coordinate(0, 0);
            return false;
        }

        public static List<Coordinate> ParseList(string configStr) {
            List<Coordinate> result = new List<Coordinate>();
            var fragments = configStr.Split(';');
            var count = fragments.Length;
            for (var i = 0; i < count; ++i) {
                string frag = fragments[i];
                Coordinate parseCoord;
                if (TryParse(frag, out parseCoord)) {
                    result.Add(parseCoord);
                }
            }

            return result;
        }

        public static Coordinate TopLeft {
            get { return new Coordinate(-1, -1); }
        }

        public static Coordinate Top {
            get { return new Coordinate(0, -1); }
        }

        public static Coordinate TopRight {
            get { return new Coordinate(1, -1); }
        }

        public static Coordinate Left {
            get { return new Coordinate(-1, 0); }
        }

        public static Coordinate Zero {
            get { return new Coordinate(0, 0); }
        }

        public static Coordinate Right {
            get { return new Coordinate(1, 0); }
        }

        public static Coordinate BottomLeft {
            get { return new Coordinate(-1, 1); }
        }

        public static Coordinate Bottom {
            get { return new Coordinate(0, 1); }
        }

        public static Coordinate BottomRight {
            get { return new Coordinate(1, 1); }
        }

        public static Coordinate[] NineDirections {
            get { return new[] {TopLeft, Top, TopRight, Left, Zero, Right, BottomLeft, Bottom, BottomRight}; }
        }

        public Vector3Int ToVector3Int() {
            return new Vector3Int(x, y, 0);
        }
    }
}