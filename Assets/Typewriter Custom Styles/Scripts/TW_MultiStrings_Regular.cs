using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(TW_MultiStrings_Regular)), CanEditMultipleObjects]
[Serializable]
public class TW_MultiStrings_Regular_Editor : Editor
{
    private int indexOfString;
    private static string[] PointerSymbols = { "None", "<", "_", "|", ">" };
    private TW_MultiStrings_Regular TW_MS_RegularScript;

    private void Awake() {
        TW_MS_RegularScript = (TW_MultiStrings_Regular)target;
    }

    private void MakeArrayGUI(SerializedObject obj, string name)
    {
        int size = obj.FindProperty(name + ".Array.size").intValue;
        int newSize = size;
        if (newSize != size)
            obj.FindProperty(name + ".Array.size").intValue = newSize;
        int[] array_value = new int[newSize];
        for (int i = 1; i < newSize; i++)
        {
            array_value[i] = i;
        }
        string[] array_content = new string[newSize];
        for (int i = 1; i < newSize; i++)
        {
            array_content[i] = (array_value[i] + 1).ToString();
        }
        if (TW_MS_RegularScript.MultiStrings.Length == 0)
            EditorGUILayout.HelpBox("Number of Strings must be more than 0!", MessageType.Error);
        MakePopup(obj);
        EditorGUILayout.HelpBox("Chose number of string in PoPup and edit text in TextArea below", MessageType.Info, true);
        indexOfString = EditorGUILayout.IntPopup("Edit string №", indexOfString, array_content, array_value, EditorStyles.popup);
        TW_MS_RegularScript.MultiStrings[indexOfString] = EditorGUILayout.TextArea(TW_MS_RegularScript.MultiStrings[indexOfString], GUILayout.ExpandHeight(true));
    }

    private void MakePopup(SerializedObject obj)
    {
        TW_MS_RegularScript.pointer = EditorGUILayout.Popup("Pointer symbol", TW_MS_RegularScript.pointer, PointerSymbols, EditorStyles.popup);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SerializedObject SO = new SerializedObject(TW_MS_RegularScript);
        MakeArrayGUI(SO, "MultiStrings");
    }
}
#endif

public class TW_MultiStrings_Regular : MonoBehaviour {

    public bool LaunchOnStart = true;
    public int timeOut = 1;
    public string[] MultiStrings = new string[1];
    [HideInInspector]
    public int pointer=0;
    public string ORIGINAL_TEXT;
    public string ORIGINAL_TEXT_RUS;
    public string ORIGINAL_TEXT_ENG;

    private float time = 0f;
    private int сharIndex = 0;
    private int index_of_string = 0;
    private bool start;
    private List<int> n_l_list;
    private static string[] PointerSymbols = { "None", "<", "_", "|", ">" };
    private AudioSource audioSource;

    void Start ()
    {
        gameObject.GetComponent<Text>().text = (Settings_Manager.Language == SystemLanguage.Russian
                                                ? ORIGINAL_TEXT_RUS //"Отработав десять лет программистом в банке, ты научился проектировать базы данных, создавать сложные отчеты, стал настоящим виртуозом в области банковских систем. Зарплата выросла выше рынка, ты хорошо устроился в своём тёплом гнёздышке и, казалось бы, жизнь удалась... Но однажды тебе сообщают, что банк закрывается, и через 1 год тебе придется работать в новом месте. В каком именно - ты должен выбрать сам. Осмотревшись на рынке, ты понимаешь, что хочешь стать iOS разработчиком. Купив свой первый mac mini, ты приступил к изучению новых технологий. Для того, чтобы устроиться Junior разработчиком, тебе понадобится изучить всего лишь: SWIFT, MVC, MVP, MVVM, VIPER, SOLID, XML, JSON, GIT, ENUMS, UIKIT, GCD, KVC, KVO, AUTOLAYOUT, а так же иметь не менее года опыта коммерческой разработки. Зачем с такими навыками работать на дядю - остается загадкой. Возможно, всё дело в бесплатном кофе и печенюшках, от которых ты отказаться не в силах. Пройдя свой первый курс по iOS-разработке от одной известной IT-компании, ты понял, что знаний пока ещё явно не достаточно и нужно дальше осваивать новые технологии. Засучив рукава, ты смело пускаешься на встречу приключениям!" 
                                                : ORIGINAL_TEXT_ENG //"After working for ten years as a programmer in a bank, you learned how to design databases, create complex reports, and became a real virtuoso in the field of banking systems. The salary has grown above the market, you are well settled in your warm nest and, it would seem, your life has been successful ... But once you are informed that the bank is closing, and after 1 year you will have to work in a new place. Which one - you have to choose yourself. Looking around the market, you realize that you want to become an iOS developer. Having bought your first mac mini, you set about exploring new technologies. In order to get a Junior developer, you only need to learn: SWIFT, MVC, MVP, MVVM, VIPER, SOLID, XML, JSON, GIT, ENUMS, UIKIT, GCD, KVC, KVO, AUTOLAYOUT, as well as have at least years of commercial development experience. Why you should not work for yourself with such an experience remains a mystery. Perhaps the whole thing is free coffee and cookies, which you can’t refuse. Having passed your first iOS development course from one well-known IT company, you realized that knowledge is still clearly not enough and you need to further develop new technologies. Rolling up your sleeves, you boldly embark on an adventure!"
                                                );
        MultiStrings[0] = gameObject.GetComponent<Text>().text;
        ORIGINAL_TEXT = gameObject.GetComponent<Text>().text;
        gameObject.GetComponent<Text>().text = "";
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(StartSoundTypeWrite());

        if (LaunchOnStart)
        {
            StartTypewriter();
        }

    }
	
	void Update () {
        if (start == true){
            NewLineCheck(ORIGINAL_TEXT);
        }
    }

    public void StartTypewriter()
    {
        start = true;
        сharIndex = 0;
        time = 0f;
    }

    public void SkipTypewriter()
    {
        сharIndex = ORIGINAL_TEXT.Length - 1;
    }

    public void NextScreen()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void GoGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void NextString()
    {
        start = true;
        сharIndex = 0;
        time = 0f;
        if (index_of_string + 1 < MultiStrings.Length){
            index_of_string++;
        }
        else{
            index_of_string = 0;
        }
        ORIGINAL_TEXT = MultiStrings[index_of_string];
    }

    public void LastString()
    {
        start = true;
        ORIGINAL_TEXT = MultiStrings[MultiStrings.Length - 1];
        сharIndex = ORIGINAL_TEXT.Length - 1;
    }

    private void NewLineCheck(string S)
    {
        if (S.Contains("\n"))
        {
            StartCoroutine(MakeTypewriterTextWithNewLine(S, GetPointerSymbol(), MakeList(S)));
        }
        else
        {
            StartCoroutine(MakeTypewriterText(S, GetPointerSymbol()));
        }
    }

    private IEnumerator StartSoundTypeWrite()
    {
        yield return new WaitForSeconds(0.4f);
        audioSource.Play();
    }

    private IEnumerator MakeTypewriterText(string ORIGINAL, string POINTER)
    {
        start = false;
        if (сharIndex != ORIGINAL.Length + 1)
        {
            string emptyString = new string(' ', ORIGINAL.Length-POINTER.Length);
            string TEXT = ORIGINAL.Substring(0, сharIndex);
            if (сharIndex < ORIGINAL.Length) TEXT = TEXT + POINTER + emptyString.Substring(сharIndex);
            gameObject.GetComponent<Text>().text = TEXT;
            time += 1;
            yield return new WaitForSeconds(0.01f);
            CharIndexPlus();
            start = true;
        } else
            audioSource.Stop();
    }

    private IEnumerator MakeTypewriterTextWithNewLine(string ORIGINAL, string POINTER, List<int> List)
    {
        start = false;
        if (сharIndex != ORIGINAL.Length + 1)
        {
            string emptyString = new string(' ', ORIGINAL.Length - POINTER.Length);
            string TEXT = ORIGINAL.Substring(0, сharIndex);
            if (сharIndex < ORIGINAL.Length) TEXT = TEXT + POINTER + emptyString.Substring(сharIndex);
            TEXT = InsertNewLine(TEXT, List);
            gameObject.GetComponent<Text>().text = TEXT;
            time += 1f;
            yield return new WaitForSeconds(0.01f);
            CharIndexPlus();
            start = true;
        } else
            audioSource.Stop();
    }

    private List<int> MakeList(string S)
    {
        n_l_list = new List<int>();
        for (int i = 0; i < S.Length; i++)
        {
            if (S[i] == '\n')
            {
                n_l_list.Add(i);
            }
        }
        return n_l_list;
    }

    private string InsertNewLine(string _TEXT, List<int> _List)
    {
        for (int index = 0; index < _List.Count; index++)
        {
            if (сharIndex - 1 < _List[index])
            {
                _TEXT = _TEXT.Insert(_List[index], "\n");
            }
        }
        return _TEXT;
    }

    private string GetPointerSymbol()
    {
        if (pointer == 0){
            return "";
        }
        else{
            return PointerSymbols[pointer];
        }
    }

    private void CharIndexPlus()
    {
        if (time == timeOut)
        {
            time = 0f;
            сharIndex += 1;
        }
    }
}


