using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vee.Managers;


namespace Vee.Services.Localize {
    
    /// <summary>
    /// 语言信息
    /// </summary>
    public class LangInfo {
        public string showName;
        public string i2Name;

        public LangInfo(string sName, string iName) {
            showName = sName;
            i2Name = iName;
        }
    }

    public class LocalizeService : ServiceBase {
        public const LangTag DefaultLanguage = LangTag.en;

        public static LocalizeService Instance => ServiceManager.Get<LocalizeService>();

        public bool LogLocalizeFail = false;

        [Tooltip("当前设置的语言，可以在编辑器中更改")]
        [OnValueChanged("OnSetLanguageChanged")]
        public LangTag SetLanguage = DefaultLanguage;
        
        [ShowInInspector]
        public static Dictionary<LangTag, LangInfo> languageInfos = new Dictionary<LangTag, LangInfo>() {
            {LangTag.en, new LangInfo("ENGLISH", "English")},
            {LangTag.zhHans, new LangInfo("简体中文", "Chinese")},
            {LangTag.zhHant, new LangInfo("繁體中文", "English")},
            {LangTag.ja, new LangInfo("日本語", "English")},
            {LangTag.ko, new LangInfo("한국어", "English")},
            {LangTag.pt, new LangInfo("Português", "English")},
            {LangTag.es, new LangInfo("Español", "English")},
            {LangTag.it, new LangInfo("lingua italiana", "English")},
            {LangTag.ru, new LangInfo("Русский язык", "English")},
            {LangTag.fr, new LangInfo("Français", "English")},
            {LangTag.de, new LangInfo("Deutsche Sprache", "English")},
        };


        public static LangInfo GetLangInfo(LangTag tag) {
            return languageInfos.ContainsKey(tag) ? languageInfos[tag] : languageInfos[DefaultLanguage];
        }

        // public LangTag GetUserSelectLang() {
        //     return GlobalVariables.instance.UserData.SelectLang;
        // }
        //
        // public void SetUserSelectLang(LangTag newLang) {
        //     var curLang = GetUserSelectLang();
        //     if (curLang != newLang) {
        //         GlobalVariables.instance.UserData.SelectLang = newLang;
        //     }
        // }
        
        // public string GetLanguageName(LangTag lang = LangTag.max) {
        //     if (lang == LangTag.max) {
        //         lang = GetUserSelectLang();
        //     }
        //
        //     var langInfo = GetLangInfo(lang);
        //     return langInfo.showName;
        // }

        /// <summary>S
        /// 获取本地化字符串
        /// </summary>
        /// <returns>The localized string for key.</returns>
        /// <param name="key">Key.</param>
        public string GetLocalizedString(string key)
        {
            return key;
            
            /*
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(key)) {
                Warning($"Localize string key is empty");
                return "Empty String";
            }
#endif

            string ls = LocalizationManager.GetTranslation(key);

#if UNITY_EDITOR
            if (LogLocalizeFail && string.IsNullOrEmpty(ls)) {
                Warning($"Can't Localize string : {key}");
            }
#endif

            return string.IsNullOrEmpty(ls) ? key : ls;
            */
        }


        // /// <summary>
        // /// 设置当前的语言
        // /// </summary>
        // /// <param name="lang"></param>
        // public void SetCurrentLanguage(LangTag lang) {
        //     var langInfo = GetLangInfo(lang);
        //     SetLanguage = lang;
        //     LocalizationManager.CurrentLanguage = langInfo.i2Name;
        //     Log($"Current Language Set To {langInfo.showName}");
        // }
        

        protected override void OnInitialize() {
            // SetCurrentLanguage(GetUserSelectLang());
        }
        
#if UNITY_EDITOR
        void OnSetLanguageChanged() {
            Debug.Log($"Editor change Language : {SetLanguage}");
            // SetCurrentLanguage(SetLanguage);
        }
#endif
    }
}