using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestingExample : MonoBehaviour {
	public enum Languages{
		en,
		cn,
		ru,
		ja
	}
	[System.Serializable]
	public class LanguageMap : Dict<Languages, string>{}

	[System.Serializable]
	public class Localization : Dict<string, LanguageMap>{};//nesting languagemap inside this dictionary

	public Localization UITexts;
	void Start () {
		
	}
	
}
