
using UnityEngine;
using Sirenix.OdinInspector;


namespace Vee.UnityExtend.Binding {
    public enum BindElementType {
        Unknown = 0,

        // Unity Classes
        GameObject = 1,
        Transform = 2,
        Animator = 3,
        Camera = 4,
        ParticleSystem = 5,
        Renderer = 11,
        SpriteRenderer = 12,
        MeshRenderer = 13,
        TrailRenderer = 14,
        LineRenderer = 15,
        Collider = 21,
        BoxCollider = 22,
        BoxCollider2D = 23,
        TileMap = 101,
        AudioSource = 151,


        // UGUI Classes
        RectTransform = 1001,
        Canvas = 1002,
        EventSystem = 1003,
        Graphic = 1021,
        Button = 1031,
        Image = 1041,
        RawImage = 1042,
        Text = 1051,
        Toggle = 1061,
        Slider = 1071,
        ScrollBar = 1081,
        ScrollRect = 1091,
        DropDown = 1101,
        InputField = 1111,
        TextMeshProUGUI = 1007,
        
        // Veewo Classes
        AlienButton = 2001,
        UIAnimatedButton = 2002,
        VeewoButton = 2010,
        CommonButton = 2011,
        BetterProgressBarController = 2021,
        UIColorScroller = 2031,
        SizeProgressBar = 2041,
        
        // Game Classes
        UIFortuneSpecialStoneCard = 10001,
    }

    [ExecuteAlways]
    public class BindElement : MonoBehaviour {
        [Required] 
        public string RootName = "";
        [Required] 
        public string BindName = "";
        [ValidateInput("CheckInputEleType", "Element Type is Unknown")]
        public BindElementType Type = BindElementType.GameObject;
        
//        public string EleName {
//            get {
//                if (VeeUtils.CheckStringValid(BindName)) {
//                    return BindName;
//                }
//                else {
//                    return gameObject.name;
//                }
//            }
//        }

        public System.Type EleType {
            get {
                switch (Type) {
                    // Unity Class
                    case BindElementType.GameObject:
                        return typeof(UnityEngine.GameObject);
                    case BindElementType.Transform :
                        return typeof(UnityEngine.Transform);
                    case BindElementType.Animator :
                        return typeof(UnityEngine.Animator);
                    case BindElementType.Camera:
                        return typeof(UnityEngine.Camera);
                    case BindElementType.ParticleSystem:
                        return typeof(UnityEngine.ParticleSystem);
                    case BindElementType.Renderer:
                        return typeof(UnityEngine.Renderer);
                    case BindElementType.SpriteRenderer:
                        return typeof(UnityEngine.SpriteRenderer);
                    case BindElementType.MeshRenderer:
                        return typeof(UnityEngine.MeshRenderer);
                    case BindElementType.TrailRenderer:
                        return typeof(UnityEngine.TrailRenderer);
                    case BindElementType.LineRenderer:
                        return typeof(UnityEngine.LineRenderer);
                    case BindElementType.Collider:
                        return typeof(UnityEngine.Collider);
                    case BindElementType.BoxCollider:
                        return typeof(UnityEngine.BoxCollider);
                    case BindElementType.BoxCollider2D:
                        return typeof(UnityEngine.BoxCollider2D);
                    case BindElementType.TileMap:
                        return typeof(UnityEngine.Tilemaps.Tilemap);
                    case BindElementType.AudioSource:
                        return typeof(UnityEngine.AudioSource);

                        
                    // UGUI Classes
                    case BindElementType.RectTransform:
                        return typeof(UnityEngine.RectTransform);
                    case BindElementType.Canvas:
                        return typeof(UnityEngine.Canvas);
                    case BindElementType.EventSystem:
                        return typeof(UnityEngine.EventSystems.EventSystem);
                    case BindElementType.Graphic :
                        return typeof(UnityEngine.UI.Graphic);
                    case BindElementType.Button:
                        return typeof(UnityEngine.UI.Button);
                    case BindElementType.Image:
                        return typeof(UnityEngine.UI.Image);
                    case BindElementType.RawImage:
                        return typeof(UnityEngine.UI.RawImage);
                    case BindElementType.Text:
                        return typeof(UnityEngine.UI.Text);
                    case BindElementType.Toggle:
                        return typeof(UnityEngine.UI.Toggle);
                    case BindElementType.Slider:
                        return typeof(UnityEngine.UI.Slider);
                    case BindElementType.ScrollBar:
                        return typeof(UnityEngine.UI.Scrollbar);
                    case BindElementType.ScrollRect:
                        return typeof(UnityEngine.UI.ScrollRect);
                    case BindElementType.DropDown:
                        return typeof(UnityEngine.UI.Dropdown);
                    case BindElementType.InputField:
                        return typeof(UnityEngine.UI.InputField);
                    case BindElementType.TextMeshProUGUI:
                        return typeof(TMPro.TextMeshProUGUI);
                    
                    
                    // Veewo Classes

                    
                    // Game Classes


                    default:
                        return null;
                }
            }
        }
        void Awake() {
            if (! string.IsNullOrEmpty(RootName)) return;
            
            var root = gameObject.GetComponentInParent<BindRoot>();
            if (root != null) {
                RootName = root.ScriptName;
            }
        }

        private bool CheckInputEleType(BindElementType t, ref string errorMessage, ref InfoMessageType? messageType) {
            if (t != BindElementType.Unknown) {
                return true;
            }
            else {
                errorMessage = "Element Type is Unknown";
                messageType = InfoMessageType.Warning;
                return false;
            }
        }
    }
}