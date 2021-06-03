using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vee.Consts
{
    /// <summary>
    /// unity 自定义的layer
    /// 这里的值应该与unity内设置的值保持一致
    /// </summary>
    public static class UnityLayer
    {
        public const int Default = 0;
        public const int UI = 5;
        public const int WorldUI = 8;
        public const int Battle = 9;
        public const int AttackableBox = 10;
        public const int AttackiveBox = 11;
        public const int UIMonster = 12;


        public const int Guide = 30; // deprecated

        public static LayerMask GetLayerMask(params int[] someLayers)
        {
            int maskInt = 0;
            foreach (var layer in someLayers)
            {
                maskInt |= 1 << layer;
            }

            return (LayerMask) maskInt;
        }
    }

    // unity 自定义的 Sorting layer
    public static class UnitySortingLayer
    {
        public const string Background = "Background";
        public const string Water = "Water";
        public const string Ground = "Ground";
        public const string StoneGround = "StoneGround";
        public const string WhiteCloudShadow = "White Cloud Shadow";
        public const string Default = "Default";
        public const string Resources = "Resources";
        public const string Factories = "Factories";
        public const string Drop = "Drop";

        public const string WhiteCloud = "White Cloud";
        public const string BlackCloudShadow = "Black Cloud Shadow";
        public const string BlackCloud = "Black Cloud";

        public const string UI = "UI";
        public const string UICharacter = "UICharacter";
        public const string UIPet = "UIPet";
        public const string UISkillEffect = "UISkillEffect";
        public const string UIForeground = "UIForeground";
        public const string UIEffect = "UIEffect";
        public const string UITop = "UITop";

        public static List<string> GetAvailables()
        {
            return new List<string>() {
                Background,
                Water,
                Ground,
                StoneGround,
                WhiteCloudShadow,
                Default,
                Resources,
                Factories,
                Drop,
                WhiteCloud,
                BlackCloudShadow,
                BlackCloud,
                UI,
                UICharacter,
                UIPet,
                UISkillEffect,
                UIForeground,
                UIEffect,
                UITop
            };
        }

        public static string EnumToString(AvailableSortingLayer enumLayer)
        {
            switch (enumLayer)
            {
                case AvailableSortingLayer.Background: return Background;
                case AvailableSortingLayer.Water: return Water;
                case AvailableSortingLayer.Ground: return Ground;
                case AvailableSortingLayer.StoneGround: return StoneGround;

                case AvailableSortingLayer.WhiteCloudShadow: return WhiteCloudShadow;
                case AvailableSortingLayer.Default: return Default;
                case AvailableSortingLayer.Resources: return Resources;
                case AvailableSortingLayer.Factories: return Factories;
                case AvailableSortingLayer.Drop: return Drop;
                case AvailableSortingLayer.WhiteCloud: return WhiteCloud;
                case AvailableSortingLayer.BlackCloudShadow: return BlackCloudShadow;
                case AvailableSortingLayer.BlackCloud: return BlackCloud;

                case AvailableSortingLayer.UI: return UI;
                case AvailableSortingLayer.UICharacter: return UICharacter;
                case AvailableSortingLayer.UIPet: return UIPet;
                case AvailableSortingLayer.UISkillEffect: return UISkillEffect;
                case AvailableSortingLayer.UIForeground: return UIForeground;
                case AvailableSortingLayer.UIEffect: return UIEffect;
                case AvailableSortingLayer.UITop: return UITop;
                default: return string.Empty;
            }
        }

        public static bool CheckLayerAvailable(string layerName)
        {
            var availables = GetAvailables();
            return availables.Contains(layerName);
        }
    }

    /// <summary>
    /// 对应Editor中的Sorting Layer 顺序
    /// 游戏内应该使用字符串来传递SortingLayer，而不要依赖此枚举的整数值
    /// </summary>
    public enum AvailableSortingLayer
    {
        Unknown = -999,

        Background = -5,
        Water = -4,
        Ground = -3,
        StoneGround = -2,

        WhiteCloudShadow = -1,
        Default = 0,
        Resources = 1,
        Factories = 2,
        Drop = 3,
        WhiteCloud = 4,
        BlackCloudShadow = 5,
        BlackCloud = 6,

        UI = 7,
        UICharacter = 8,
        UIPet = 9,
        UISkillEffect = 10,
        UIForeground = 11,
        UIEffect = 12,
        UITop = 13,
    }
}