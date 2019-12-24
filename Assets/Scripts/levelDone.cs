#region Namespaces

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#endregion // Namespaces

// ######################################################################
// levelDone class
// ######################################################################

public class levelDone : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################
	
	#region Variables

	// Canvas
	public Canvas m_Canvas;
	public GUIAnimFREE m_Title2;
	public GUIAnimFREE m_Dialog1;
    public GameObject m_TitleLevel;
    public GameObject m_TitleLevelDescr;

    #endregion // Variables

    // ########################################
    // MonoBehaviour Functions
    // http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    // ########################################

    #region MonoBehaviour

    // Awake is called when the script instance is being loaded.
    // http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    void Awake ()
	{
		if(enabled)
		{
			// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start ()
	{
        m_TitleLevel.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetString("DoneLevel");
        m_TitleLevelDescr.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetString("DoneLevelDescr");

        // MoveIn m_Title2
        StartCoroutine(MoveInTitleGameObjects());

		// Disable all scene switch buttons	
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, false);
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update ()
	{		
	}
	
	#endregion // MonoBehaviour
	
	// ########################################
	// MoveIn/MoveOut functions
	// ########################################
	
	#region MoveIn/MoveOut
	
	// MoveIn m_Title2
	IEnumerator MoveInTitleGameObjects()
	{
		yield return new WaitForSeconds(1.0f);
		m_Title2.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);		
		// MoveIn m_Dialog
		StartCoroutine(MoveInPrimaryButtons());
	}
	
	// MoveIn m_Dialog
	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(1.0f);
		// MoveIn dialogs
		m_Dialog1.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}
	
	public void HideAllGUIs()
	{
		// MoveOut dialogs
		m_Dialog1.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		// MoveOut m_Title2
		StartCoroutine(HideTitleTextMeshes());
	}
	
	// MoveOut m_Title2
	IEnumerator HideTitleTextMeshes()
	{
		yield return new WaitForSeconds(1.0f);		
		// MoveOut m_Title2
		m_Title2.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);		
	}	
	#endregion // MoveIn/MoveOut
	
	// ########################################
	// Enable/Disable button functions
	// ########################################
	
	#region Enable/Disable buttons
	
	// Enable/Disable all scene switch Coroutine	
	IEnumerator EnableAllDemoButtons()
	{
		yield return new WaitForSeconds(1.0f);

		// Enable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, true);
	}
	
	// Disable all buttons for a few seconds
	IEnumerator DisableButtonForSeconds(GameObject GO, float DisableTime)
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, false);		
		yield return new WaitForSeconds(DisableTime);		
		// Enable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, true);
	}
	
	#endregion // Enable/Disable buttons
	
	// ########################################
	// UI Responder functions
	// ########################################
	
	#region UI Responder
	
	public void OnButton_Dialog1()
	{			
		// MoveOut m_Dialog1
		m_Dialog1.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable m_Dialog1 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.5f));
		
		// MoveIn m_Dialog1
		StartCoroutine(Dialog1_MoveIn());
	}
	public void OnButton_MoveOutAllDialogs()
	{		
		// Disable m_Dialog1 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.5f));

		// MoveOut dialog
		m_Dialog1.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Move dialogs back to screen with Coroutines
		StartCoroutine(Dialog1_MoveIn());
	}
	
	#endregion // UI Responder
	
	// ########################################
	// Move dialog functions
	// ########################################
	
	#region Move Dialog
	
	// MoveIn m_Dialog1
	IEnumerator Dialog1_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog1
		m_Dialog1.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen to screen
		m_Dialog1.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	#endregion // Move Dialog

    public void GoToNextLevel()
    {
        //SceneManager.LoadScene(PlayerPrefs.GetInt("NextLevel", 0));
        SceneManager.LoadScene("SelectLevel");
    }

}
