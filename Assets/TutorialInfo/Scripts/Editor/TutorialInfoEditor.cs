using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TutorialInfo))]
public class TutorialInfoEditor : Editor 
{
	void OnEnable()
	{
		if (PlayerPrefs.HasKey(TutorialInfo.ShowAtStartPrefsKey))
		{
			((TutorialInfo)target).showAtStart = PlayerPrefs.GetInt(TutorialInfo.ShowAtStartPrefsKey) == 1;
		}
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck ();

		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			PlayerPrefs.SetInt(TutorialInfo.ShowAtStartPrefsKey, ((TutorialInfo)target).showAtStart ? 1 : 0);
		}
	}
}
