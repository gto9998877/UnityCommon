using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryMSDNExample : MonoBehaviour {

	//using System
	[Serializable]
	public class FileAssociation : Dict<string, string>{};

	public FileAssociation openWith;
	void Start () {
		if(openWith.Count == 0)
		{
			// Add some elements to the dictionary. There are no 
        	// duplicate keys, but some of the values are duplicates.
			openWith.Add("txt", "notepad.exe");
			openWith.Add("bmp", "paint.exe");
			openWith.Add("dib", "paint.exe");
			openWith.Add("rtf", "wordpad.exe");
		}

        // The Add method throws an exception if the new key is 
        // already in the dictionary.
        try
        {
            openWith.Add("txt", "winword.exe");
        }
        catch (ArgumentException)
        {
            Debug.Log("An element with Key = \"txt\" already exists.");
        }
		// The indexer can be used to change the value associated
        // with a key.
        openWith["rtf"] = "winword.exe";
        Debug.Log("For key = \"rtf\", value = "+ openWith["rtf"]);

		// If a key does not exist, setting the indexer for that key
        // adds a new key/value pair.
        openWith["doc"] = "winword.exe";

		// The indexer throws an exception if the requested key is
        // not in the dictionary.
        try
        {
            Debug.Log("For key = \"tif\", value = " + openWith["tif"]);
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Key = \"tif\" is not found.");
        }

		// When a program often has to try keys that turn out not to
        // be in the dictionary, TryGetValue can be a more efficient 
        // way to retrieve values.
        string value = "";
        if (openWith.TryGetValue("tif", out value))
        {
            Debug.Log("For key = \"tif\", value = "+ value);
        }
        else
        {
            Debug.Log("Key = \"tif\" is not found.");
        }

		// ContainsKey can be used to test keys before inserting 
        // them.
        if (!openWith.ContainsKey("ht"))
        {
            openWith.Add("ht", "hypertrm.exe");
            Debug.Log("Value added for key = \"ht\": " + openWith["ht"]);
        }

		// When you use foreach to enumerate dictionary elements,
        // the elements are retrieved as KeyValuePair objects.
        Console.WriteLine();
        foreach( KeyValuePair<string, string> kvp in openWith )
        {
            Debug.Log("Key = " + kvp.Key+" , Value = "+ kvp.Value);
        }

		// To get the values alone, use the Values property.
        Dictionary<string, string>.ValueCollection valueColl = openWith.Values;

		// The elements of the ValueCollection are strongly typed
        // with the type that was specified for dictionary values.
        foreach( string s in valueColl )
        {
            Debug.Log("Value = "+ s);
        }

		// To get the keys alone, use the Keys property.
        Dictionary<string, string>.KeyCollection keyColl = openWith.Keys;

        // The elements of the KeyCollection are strongly typed
        // with the type that was specified for dictionary keys.
        foreach( string s in keyColl )
        {
            Debug.Log("Key = "+ s);
        }

        // Use the Remove method to remove a key/value pair.
        Debug.Log("\nRemove(\"doc\")");
        openWith.Remove("doc");

        if (!openWith.ContainsKey("doc"))
        {
            Debug.Log("Key \"doc\" is not found.");
        }
	}
}
