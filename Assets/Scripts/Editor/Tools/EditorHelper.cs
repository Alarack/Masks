#if UNITY_EDITOR

using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// Adds a bunch of commonly used and helpful features when dealing with Unity's editor/inspector stuff.
/// </summary>
public static class EditorHelper {
    // TODO: SPLIT INTO SUBCLASSES
    // this has grown into quite a monstrocity

    private static GUIStyle _helpLabel;
    private static GUIStyle _errorLabel;
    private static GUIStyle _okayLabel;
    private static GUIStyle _warningLabel;
    private static GUIStyle _noteLabel;
    private static GUIStyle _wrappedTextField;
    private static GUIStyle _highlightedRow;
    private static GUIStyle _nonhighlightedRow;
    private static GUIStyle _noPaddingLabel;
    private static GUIStyle _tintableLabel;
    private static GUIStyle _tintableWordWrappedLabel;
    private static GUIStyle _tintableMiniLabel;
    private static GUIStyle _tintableWordWrappedMiniLabel;

    // TODO: Methods for HelpLabel() and such

    public static GUIStyle HelpLabel {
        get {
            if (_helpLabel == null) {
                _helpLabel = new GUIStyle(EditorStyles.wordWrappedMiniLabel);
                _helpLabel.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.1f, 0.7f, 0.1f) : new Color(0.0f, 0.5f, 0.0f);
                _helpLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _helpLabel;
        }
    }

    public static GUIStyle OkayLabel {
        get {
            if (_okayLabel == null) {
                _okayLabel = new GUIStyle(EditorStyles.wordWrappedLabel);
                _okayLabel.normal.textColor = new Color(0.1f, 0.9f, 0.1f);
                _okayLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _okayLabel;
        }
    }

    public static GUIStyle ErrorLabel {
        get {
            if (_errorLabel == null) {
                _errorLabel = new GUIStyle(EditorStyles.wordWrappedLabel);
                _errorLabel.normal.textColor = new Color(0.9f, 0.1f, 0.1f);
                _errorLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _errorLabel;
        }
    }

    public static GUIStyle WarningLabel {
        get {
            if (_warningLabel == null) {
                _warningLabel = new GUIStyle(EditorStyles.wordWrappedLabel);
                _warningLabel.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.9f, 0.5f, 0.1f) : new Color(0.6f, 0.2f, 0f);
                _warningLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _warningLabel;
        }
    }

    public static GUIStyle NoteLabel {
        get {
            if (_noteLabel == null) {
                _noteLabel = new GUIStyle(EditorStyles.wordWrappedMiniLabel);
                _noteLabel.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
                _noteLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _noteLabel;
        }
    }

    public static GUIStyle WrappedTextField {
        get {
            if (_wrappedTextField == null) {
                _wrappedTextField = new GUIStyle(EditorStyles.textField);
                _wrappedTextField.wordWrap = true;
            }
            return _wrappedTextField;
        }
    }

    public static GUIStyle HighlightedRow {
        get {
            if (_highlightedRow == null) {
                _highlightedRow = new GUIStyle();
                _highlightedRow.normal.background = Make1x1Texure(EditorGUIUtility.isProSkin ? new Color(1.0f, 1.0f, 1.0f, 0.1f) : new Color(1.0f, 1.0f, 1.0f, 0.4f));
            }
            return _highlightedRow;
        }
    }

    public static GUIStyle NonhighlightedRow {
        get {
            if (_nonhighlightedRow == null) {
                _nonhighlightedRow = new GUIStyle();
            }
            return _nonhighlightedRow;
        }
    }

    public static GUIStyle NoPaddingLabel {
        get {
            if (_noPaddingLabel == null) {
                _noPaddingLabel = new GUIStyle(GUI.skin.label);
                _noPaddingLabel.padding = new RectOffset(0, 0, 0, 0);
            }
            return _noPaddingLabel;
        }
    }

    public static GUIStyle TintableLabel {
        get {
            if (!EditorGUIUtility.isProSkin) {
                if (_tintableLabel == null) {
                    _tintableLabel = EditorStyles.whiteLabel;
                    _tintableLabel.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                }
                return _tintableLabel;
            }
            else {
                _tintableWordWrappedLabel = EditorStyles.whiteLabel;
                _tintableWordWrappedLabel.wordWrap = true;
                return _tintableWordWrappedLabel;
            }
        }
    }

    public static GUIStyle TintableWordWrappedLabel {
        get {
            if (!EditorGUIUtility.isProSkin) {
                if (_tintableWordWrappedLabel == null) {
                    _tintableWordWrappedLabel = EditorStyles.whiteLabel;
                    _tintableWordWrappedLabel.wordWrap = true;
                    _tintableWordWrappedLabel.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                }
                return _tintableWordWrappedLabel;
            }
            else {
                _tintableWordWrappedLabel = EditorStyles.whiteLabel;
                _tintableWordWrappedLabel.wordWrap = true;
                return _tintableWordWrappedLabel;
            }
        }
    }

    public static GUIStyle TintableMiniLabel {
        get {
            if (!EditorGUIUtility.isProSkin) {
                if (_tintableMiniLabel == null) {
                    _tintableMiniLabel = EditorStyles.miniLabel;
                    _tintableMiniLabel.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                }
                return _tintableMiniLabel;
            }
            else {
                return EditorStyles.miniLabel;
            }
        }
    }

    public static GUIStyle TintableWordWrappedMiniLabel {
        get {
            if (!EditorGUIUtility.isProSkin) {
                if (_tintableWordWrappedMiniLabel == null) {
                    _tintableWordWrappedMiniLabel = EditorStyles.miniLabel;
                    _tintableWordWrappedMiniLabel.wordWrap = true;
                    _tintableWordWrappedMiniLabel.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                }
                return _tintableWordWrappedMiniLabel;
            }
            else {
                _tintableWordWrappedMiniLabel = EditorStyles.miniLabel;
                _tintableWordWrappedMiniLabel.wordWrap = true;
                return _tintableWordWrappedMiniLabel;
            }
        }
    }

    private static Texture2D _addTexture;
    private static Texture2D _removeTexture;
    private static Texture2D _deleteTexture;
    private static Texture2D _dropdownTexture;
    private static Texture2D _eyeTexture;
    private static Texture2D _moveUpTexture;
    private static Texture2D _moveDownTexture;
    private static Texture2D _moveLeftTexture;
    private static Texture2D _moveRightTexture;
    private static Texture2D _helpPromptTexture;
    private static Texture2D _helpPromptActiveTexture;
    private static Texture2D _dialTexture;
    private static Texture2D _iconOkayTexture;
    private static Texture2D _iconWarningTexture;
    private static Texture2D _iconErrorTexture;
    private static Texture2D _iconDisabledTexture;
    private static Texture2D _iconConfigIssueTexture;

    private static GUIStyle _iconButtonStyle;
    private static GUIStyle _iconStyle;

    private static Texture2D AddTexture { get { return _addTexture ?? (_addTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "addButtonIcon" : "addButtonIconLight")); } }
    private static Texture2D RemoveTexture { get { return _removeTexture ?? (_removeTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "removeButtonIcon" : "removeButtonIconLight")); } }
    private static Texture2D DeleteTexture { get { return _deleteTexture ?? (_deleteTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "deleteButtonIcon" : "deleteButtonIconLight")); } }
    private static Texture2D DropdownTexture { get { return _dropdownTexture ?? (_dropdownTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "lookupButtonIcon" : "lookupButtonIconLight")); } }
    private static Texture2D EyeTexture { get { return _eyeTexture ?? (_eyeTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "eyeButtonIcon" : "eyeButtonIconLight")); } }
    private static Texture2D MoveUpTexture { get { return _moveUpTexture ?? (_moveUpTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "moveUpButtonIcon" : "moveUpButtonIconLight")); } }
    private static Texture2D MoveDownTexture { get { return _moveDownTexture ?? (_moveDownTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "moveDownButtonIcon" : "moveDownButtonIconLight")); } }
    private static Texture2D MoveLeftTexture { get { return _moveLeftTexture ?? (_moveLeftTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "moveLeftButtonIcon" : "moveLeftButtonIconLight")); } }
    private static Texture2D MoveRightTexture { get { return _moveRightTexture ?? (_moveRightTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "moveRightButtonIcon" : "moveRightButtonIconLight")); } }
    private static Texture2D HelpPromptTexture { get { return _helpPromptTexture ?? (_helpPromptTexture = Resources.Load<Texture2D>("helpPromptIcon")); } }
    private static Texture2D HelpPromptTextureActive { get { return _helpPromptActiveTexture ?? (_helpPromptActiveTexture = Resources.Load<Texture2D>("helpPromptActiveIcon")); } }
    private static Texture2D DialTexture { get { return _dialTexture ?? (_dialTexture = Resources.Load<Texture2D>("dialIcon")); } }

    private static Texture2D IconOkayTexture { get { return _iconOkayTexture ?? (_iconOkayTexture = Resources.Load<Texture2D>("okayIcon")); } }
    private static Texture2D IconWarningTexture { get { return _iconWarningTexture ?? (_iconWarningTexture = Resources.Load<Texture2D>("warningIcon")); } }
    private static Texture2D IconErrorTexture { get { return _iconErrorTexture ?? (_iconErrorTexture = Resources.Load<Texture2D>("errorIcon")); } }
    private static Texture2D IconDisabledTexture { get { return _iconDisabledTexture ?? (_iconDisabledTexture = Resources.Load<Texture2D>("disabledIcon")); } }
    private static Texture2D IconConfigIssueTexture { get { return _iconConfigIssueTexture ?? (_iconConfigIssueTexture = Resources.Load<Texture2D>("configurationProblemIcon")); } }

    public static GUIStyle IconButtonStyle { get { return _iconButtonStyle ?? (_iconButtonStyle = new GUIStyle(GUI.skin.button) { padding = new RectOffset(0, 0, 0, 0), margin = new RectOffset(2, 2, 1, 1) }); } }
    private static GUIStyle IconStyle { get { return _iconStyle ?? (_iconStyle = new GUIStyle(GUI.skin.label) { padding = new RectOffset(0, 0, 0, 0) }); } }


    public static class Colors {
        public static Color Red { get { return EditorGUIUtility.isProSkin ? new Color(1f, 0.4f, 0.4f, 1f) : new Color(0.5f, 0f, 0f, 1f); } }
        public static Color Green { get { return EditorGUIUtility.isProSkin ? new Color(0.5f, 1f, 0.5f, 1f) : new Color(0f, 0.6f, 0f, 1f); } }
        public static Color Blue { get { return EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 1f, 1f) : new Color(0f, 0f, 0.5f, 1f); } }
        public static Color Cyan { get { return EditorGUIUtility.isProSkin ? new Color(0.2f, 1f, 1f, 1f) : new Color(0f, 0.5f, 0.5f, 1f); } }
        public static Color Purple { get { return EditorGUIUtility.isProSkin ? new Color(0.9f, 0.3f, 1f, 1f) : new Color(0.5f, 0f, 0.5f, 1f); } }
        public static Color Orange { get { return EditorGUIUtility.isProSkin ? new Color(1f, 0.7f, 0.3f, 1f) : new Color(0.7f, 0.3f, 0f, 1f); } }
    }


    /// <summary> Shows a small toolbar-like button with a "+" symbol </summary>
    public static bool AddButton(string tooltip = "Add new entry", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(AddTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    /// <summary> Shows a small toolbar-like button with a "-" symbol </summary>
    public static bool RemoveButton(string tooltip = "Remove entry", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(RemoveTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    /// <summary> Shows a small toolbar-like button with an "x" symbol </summary>
    public static bool DeleteButton(string tooltip = "Delete?", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(DeleteTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    /// <summary> Shows a small toolbar-like button with an eye symbol </summary>
    public static bool EyeButton(string tooltip = "Toggle visivility", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(EyeTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    /// <summary> Shows a small toolbar-like button colored red with an "x" symbol </summary>
    public static bool DeleteButtonConfirm(string tooltip = "DELETE?", int width = 20, int height = 18) {
        GUI.backgroundColor = Color.red;
        bool b = GUILayout.Button(new GUIContent(DeleteTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
        GUI.backgroundColor = Color.white;
        return b;
    }

    private static object _lastDeleteObject;

    public static bool DeleteButtonDouble(object deleteObject) {
        if (_lastDeleteObject == deleteObject) {
            if (DeleteButtonConfirm()) {
                _lastDeleteObject = null;
                return true;
            }
        }
        else {
            if (DeleteButton()) {
                _lastDeleteObject = deleteObject;
            }
        }
        return false;
    }

    public static bool MoveUpButton(string tooltip = "Move up", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(MoveUpTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    public static bool MoveDownButton(string tooltip = "Move down", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(MoveDownTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    public static bool MoveLeftButton(string tooltip = "Move left", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(MoveLeftTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    public static bool MoveRightButton(string tooltip = "Move right", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(MoveRightTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    public static bool ColoredButton(string label, Color color, int width = 20, int height = 18) {
        bool value = false;
        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        value = GUILayout.Button(label, GUILayout.Width(width), GUILayout.Height(height));

        GUI.backgroundColor = oldColor;

        return value;
    }
    public static bool ColoredButton(string label, Color color) {
        bool value = false;
        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        value = GUILayout.Button(label);

        GUI.backgroundColor = oldColor;

        return value;
    }

    private static object _lastRemoveList;
    private static int _lastRemoveIndex;

    private static bool DrawListControls<T>(
        List<T> list, int index, bool add, T defaultValue, bool useGivenDefaultValue, bool remove, bool move, bool hideMove, int width = 20, int height = 18,
        Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null, object referenceObject = null) {
        if (add) {
            if (AddButton("Add new entry", width, height)) {
                if (addCallback != null)
                    addCallback(list, index + 1);
                else
                    list.Insert(index + 1, MakeDefaultValue(defaultValue, useGivenDefaultValue));

                if (_lastRemoveList == (referenceObject ?? list)) {
                    _lastRemoveList = null;
                    _lastRemoveIndex = -1;
                }

                GUI.changed = true; // we have "changed" a "field" with this action, so mark GUI as dirty
            }
        }

        if (remove) {
            if (_lastRemoveList == (referenceObject ?? list) && _lastRemoveIndex == index) {
                if (DeleteButtonConfirm("DELETE?", width, height)) {
                    if (removeCallback != null)
                        removeCallback(list, index);
                    else
                        list.RemoveAt(index);
                    _lastRemoveList = null;
                    _lastRemoveIndex = -1;

                    GUI.changed = true; // we have "changed" a "field" with this action, so mark GUI as dirty

                    return true;
                }
            }
            else {
                if (DeleteButton("Delete?", width, height)) {
                    _lastRemoveList = (referenceObject ?? list);
                    _lastRemoveIndex = index;

                    GUI.changed = true; // we have "changed" a "field" with this action, so mark GUI as dirty

                    return true;
                }
            }
        }

        if (move || !hideMove) {
            if (!move || index == 0) GUI.enabled = false;
            if (MoveUpButton("Move up", width, height)) {
                T temp = list[index];
                list[index] = list[index - 1];
                list[index - 1] = temp;

                if (_lastRemoveList == (referenceObject ?? list)) {
                    _lastRemoveList = null;
                    _lastRemoveIndex = -1;
                }

                GUI.changed = true; // we have "changed" a "field" with this action, so mark GUI as dirty

                return true;
            }
            GUI.enabled = true;

            if (!move || index == list.Count - 1) GUI.enabled = false;
            if (MoveDownButton("Move down", width, height)) {
                T temp = list[index];
                list[index] = list[index + 1];
                list[index + 1] = temp;

                if (_lastRemoveList == (referenceObject ?? list)) {
                    _lastRemoveList = null;
                    _lastRemoveIndex = -1;
                }

                GUI.changed = true; // we have "changed" a "field" with this action, so mark GUI as dirty

                return true;
            }
            GUI.enabled = true;
        }

        return false;
    }

    public static bool DrawListMoveUpDownControls<T>(List<T> list, T item) {
        int index = list.IndexOf(item);

        if (index == 0) GUI.enabled = false;
        if (MoveUpButton()) {
            T temp = list[index];
            list[index] = list[index - 1];
            list[index - 1] = temp;
            return true;
        }
        GUI.enabled = true;

        if (index == list.Count - 1) GUI.enabled = false;
        if (MoveDownButton()) {
            T temp = list[index];
            list[index] = list[index + 1];
            list[index + 1] = temp;
            return true;
        }
        GUI.enabled = true;

        return false;
    }

    /// <summary> Makes a default value for a type -- default for value types and new instances for classes (unless it's Unity stuff, in which case null) </summary>
    private static T MakeDefaultValue<T>(T defaultValue, bool givenDefaultValue) {
        if (givenDefaultValue)
            return defaultValue;

        if (!typeof(T).IsClass)
            return default(T);

        if (typeof(T).IsSubclassOf(typeof(Object))) // Anything Unity should be made by Unity or referenced, so -> null
            return default(T);

        return Activator.CreateInstance<T>();
    }

    public static bool DropdownButton(string tooltip = "Choose from database", int width = 20, int height = 18) {
        return GUILayout.Button(new GUIContent(DropdownTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height));
    }

    public static void HelpPromptCheck(ref bool check) {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent(check ? HelpPromptTextureActive : HelpPromptTexture), NoPaddingLabel, GUILayout.Width(20), GUILayout.Height(20)))
            check = !check;
        GUI.color = check ? Color.green : Color.cyan;
        GUILayout.Label(new GUIContent("Help?", "Check me to see inline information comments!"), GUILayout.ExpandWidth(false));
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
    }

    /// <summary> A GUI helper function that draws EditorGUILayout.TextField, but colors it red if left empty </summary>
    public static string NonEmptyTextField(string label, string value) {
        if (String.IsNullOrEmpty(value)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EditorGUILayout.TextField(label, value);
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EditorGUILayout.TextField(label, value);
        }
    }


    #region Object Fields

    /// <summary> Slight shorthand function for drawing Object Fields. </summary>
    public static T ObjectField<T>(string label, T value, bool allowSceneObjects, params GUILayoutOption[] guiOptions) where T : Object {
        return EditorGUILayout.ObjectField(label, value, typeof(T), allowSceneObjects, guiOptions) as T;
    }

    /// <summary> Slight shorthand function for drawing Object Fields. </summary>
    public static T ObjectField<T>(T value, bool allowSceneObjects, params GUILayoutOption[] guiOptions) where T : Object {
        return EditorGUILayout.ObjectField(value, typeof(T), allowSceneObjects, guiOptions) as T;
    }

    /// <summary> Slight shorthand function for drawing Object Fields. </summary>
    public static T ObjectField<T>(string label, T value, params GUILayoutOption[] guiOptions) where T : Object {
        return EditorGUILayout.ObjectField(label, value, typeof(T), false, guiOptions) as T;
    }

    /// <summary> Slight shorthand function for drawing Object Fields. </summary>
    public static T ObjectField<T>(T value, params GUILayoutOption[] guiOptions) where T : Object {
        return EditorGUILayout.ObjectField(value, typeof(T), false, guiOptions) as T;
    }

    /// <summary> A GUI helper function that draws EditorGUILayout.ObjectField, but colors it red if left empty </summary>
    public static T NonNullObjectField<T>(T value, bool allowSceneObjects) where T : Object {
        if (value == null) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EditorGUILayout.ObjectField(value, typeof(T), allowSceneObjects) as T;
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EditorGUILayout.ObjectField(value, typeof(T), allowSceneObjects) as T;
        }
    }

    /// <summary> A GUI helper function that draws EditorGUILayout.ObjectField, but colors it red if left empty </summary>
    public static T NonNullObjectField<T>(string label, T value, bool allowSceneObjects) where T : Object {
        if (value == null) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EditorGUILayout.ObjectField(label, value, typeof(T), allowSceneObjects) as T;
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EditorGUILayout.ObjectField(label, value, typeof(T), allowSceneObjects) as T;
        }
    }

    #endregion


    #region Float Fields

    public static float FloatField(float value, params GUILayoutOption[] options) { return FloatField(null, value, null, Restriction.None, 0f, options); }

    public static float FloatField(float value, Restriction restriction, params GUILayoutOption[] options) { return FloatField(null, value, null, restriction, 0f, options); }

    public static float FloatField(float value, Restriction restriction, float restrictionValue, params GUILayoutOption[] options) { return FloatField(null, value, null, restriction, restrictionValue, options); }

    public static float FloatField(string label, float value, params GUILayoutOption[] options) { return FloatField(label, value, null, Restriction.None, 0f, options); }

    public static float FloatField(string label, float value, Restriction restriction, params GUILayoutOption[] options) { return FloatField(label, value, null, restriction, 0f, options); }

    public static float FloatField(string label, float value, Restriction restriction, float restrictionValue, params GUILayoutOption[] options) { return FloatField(label, value, null, restriction, restrictionValue, options); }

    public static float FloatField(float value, string suffix, params GUILayoutOption[] options) { return FloatField(null, value, suffix, Restriction.None, 0f, options); }

    public static float FloatField(float value, string suffix, Restriction restriction, params GUILayoutOption[] options) { return FloatField(null, value, suffix, restriction, 0f, options); }

    public static float FloatField(float value, string suffix, Restriction restriction, float restrictionValue, params GUILayoutOption[] options) { return FloatField(null, value, suffix, restriction, restrictionValue, options); }

    public static float FloatField(string label, float value, string suffix, Restriction restriction, params GUILayoutOption[] options) { return FloatField(label, value, suffix, restriction, 0f, options); }

    public static float FloatField(string label, float value, string suffix, Restriction restriction, float restrictionValue, params GUILayoutOption[] options) {
        switch (restriction) {
            case Restriction.None: return RestrictedFloatField(label, value, suffix, v => true, options);
            case Restriction.NonZero: return RestrictedFloatField(label, value, suffix, v => v != 0f, options);
            case Restriction.Positive: return RestrictedFloatField(label, value, suffix, v => v > 0f, options);
            case Restriction.Negative: return RestrictedFloatField(label, value, suffix, v => v < 0f, options);
            case Restriction.NonPositive: return RestrictedFloatField(label, value, suffix, v => v <= 0f, options);
            case Restriction.NonNegative: return RestrictedFloatField(label, value, suffix, v => v >= 0f, options);
            case Restriction.GreaterThan: return RestrictedFloatField(label, value, suffix, v => v > restrictionValue, options);
            case Restriction.LessThan: return RestrictedFloatField(label, value, suffix, v => v < restrictionValue, options);
            case Restriction.GreaterThanOrEqual: return RestrictedFloatField(label, value, suffix, v => v >= restrictionValue, options);
            case Restriction.LessThanOrEqual: return RestrictedFloatField(label, value, suffix, v => v <= restrictionValue, options);
            case Restriction.NotValue: return RestrictedFloatField(label, value, suffix, v => v != restrictionValue, options);
            default:
                throw new ArgumentOutOfRangeException("restriction", restriction, null);
        }
    }


    /// <summary> Draws a float field, but colors it red if it fails to match the specified validity check </summary>
    private static float RestrictedFloatField(string label, float value, string suffix, Func<float, bool> valueCheck, params GUILayoutOption[] options) {
        if (suffix != null)
            EditorGUILayout.BeginHorizontal();

        if (!valueCheck(value)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            if (label == null)
                value = EditorGUILayout.FloatField(value, options);
            else
                value = EditorGUILayout.FloatField(label, value, options);
            GUI.backgroundColor = Color.white;
        }
        else {
            if (label == null)
                value = EditorGUILayout.FloatField(value, options);
            else
                value = EditorGUILayout.FloatField(label, value, options);
        }

        if (suffix != null)
            GUILayout.Label(suffix, GUILayout.ExpandWidth(false));

        if (suffix != null)
            EditorGUILayout.EndHorizontal();

        return value;
    }

    #endregion


    #region Int Fields

    public static int IntField(int value, params GUILayoutOption[] options) { return IntField(null, value, null, Restriction.None, 0, options); }

    public static int IntField(int value, Restriction restriction, params GUILayoutOption[] options) { return IntField(null, value, null, restriction, 0, options); }

    public static int IntField(int value, Restriction restriction, int restrictionValue, params GUILayoutOption[] options) { return IntField(null, value, null, restriction, restrictionValue, options); }

    public static int IntField(string label, int value, params GUILayoutOption[] options) { return IntField(label, value, null, Restriction.None, 0, options); }

    public static int IntField(string label, int value, Restriction restriction, params GUILayoutOption[] options) { return IntField(label, value, null, restriction, 0, options); }

    public static int IntField(string label, int value, Restriction restriction, int restrictionValue, params GUILayoutOption[] options) { return IntField(label, value, null, restriction, restrictionValue, options); }

    public static int IntField(int value, string suffix, params GUILayoutOption[] options) { return IntField(null, value, suffix, Restriction.None, 0, options); }

    public static int IntField(int value, string suffix, Restriction restriction, params GUILayoutOption[] options) { return IntField(null, value, suffix, restriction, 0, options); }

    public static int IntField(int value, string suffix, Restriction restriction, int restrictionValue, params GUILayoutOption[] options) { return IntField(null, value, suffix, restriction, restrictionValue, options); }

    public static int IntField(string label, int value, string suffix, Restriction restriction, params GUILayoutOption[] options) { return IntField(label, value, suffix, restriction, 0, options); }

    public static int IntField(string label, int value, string suffix, Restriction restriction, int restrictionValue, params GUILayoutOption[] options) {
        switch (restriction) {
            case Restriction.None: return RestrictedIntField(label, value, suffix, null, options);
            case Restriction.NonZero: return RestrictedIntField(label, value, suffix, v => v != 0, options);
            case Restriction.Positive: return RestrictedIntField(label, value, suffix, v => v > 0, options);
            case Restriction.Negative: return RestrictedIntField(label, value, suffix, v => v < 0, options);
            case Restriction.NonPositive: return RestrictedIntField(label, value, suffix, v => v <= 0, options);
            case Restriction.NonNegative: return RestrictedIntField(label, value, suffix, v => v >= 0, options);
            case Restriction.GreaterThan: return RestrictedIntField(label, value, suffix, v => v > restrictionValue, options);
            case Restriction.LessThan: return RestrictedIntField(label, value, suffix, v => v < restrictionValue, options);
            case Restriction.GreaterThanOrEqual: return RestrictedIntField(label, value, suffix, v => v >= restrictionValue, options);
            case Restriction.LessThanOrEqual: return RestrictedIntField(label, value, suffix, v => v <= restrictionValue, options);
            case Restriction.NotValue: return RestrictedIntField(label, value, suffix, v => v != restrictionValue, options);
            default:
                throw new ArgumentOutOfRangeException("restriction", restriction, null);
        }
    }


    /// <summary> Draws a int field, but colors it red if it fails to match the specified validity check </summary>
    private static int RestrictedIntField(string label, int value, string suffix, Func<int, bool> valueCheck, params GUILayoutOption[] options) {
        if (suffix != null)
            EditorGUILayout.BeginHorizontal();

        if (valueCheck != null && !valueCheck(value)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            if (label == null)
                value = EditorGUILayout.IntField(value, options);
            else
                value = EditorGUILayout.IntField(label, value, options);
            GUI.backgroundColor = Color.white;
        }
        else {
            if (label == null)
                value = EditorGUILayout.IntField(value, options);
            else
                value = EditorGUILayout.IntField(label, value, options);
        }

        if (suffix != null)
            GUILayout.Label(suffix, GUILayout.ExpandWidth(false));

        if (suffix != null)
            EditorGUILayout.EndHorizontal();

        return value;
    }

    #endregion


    public static float SliderFloatField(string label, float value, float minValue, float maxValue, params GUILayoutOption[] options) {
        if (!string.IsNullOrEmpty(label))
            EditorGUILayout.PrefixLabel(label);

        value = GUILayout.HorizontalSlider(value, minValue, maxValue, options);

        value = Mathf.Clamp(EditorGUILayout.FloatField(value, GUILayout.Width(60)), minValue, maxValue);

        return value;
    }


    public static int SliderIntField(string label, int value, int minValue, int maxValue, params GUILayoutOption[] options) {
        if (!string.IsNullOrEmpty(label))
            EditorGUILayout.PrefixLabel(label);

        value = (int)GUILayout.HorizontalSlider(value, minValue, maxValue, options);

        value = Mathf.Clamp(EditorGUILayout.IntField(value, GUILayout.Width(60)), minValue, maxValue);

        return value;
    }

    #region Enum
    /// <summary> A GUI helper function that draws EditorGUILayout.EnumField, but colors it red if left at the default value </summary>
    public static T NonDefaultEnumField<T>(string label, T value, T defaultValue) where T : struct {
        if (Equals(value, defaultValue)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EnumPopup(label, value);
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EnumPopup(label, value);
        }
    }

    /// <summary> A GUI helper function that draws EditorGUILayout.EnumField, but colors it red if left at the default value </summary>
    public static T NonDefaultEnumField<T>(T value, T defaultValue) where T : struct {
        if (Equals(value, defaultValue)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EnumPopup(value);
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EnumPopup(value);
        }
    }

    /// <summary> A GUI helper function that draws EditorGUILayout.EnumField, but colors it red if left at one of the two default values </summary>
    public static T NonDefaultEnumField<T>(string label, T value, T defaultValue1, T defaultValue2) where T : struct {
        if (Equals(value, defaultValue1) || Equals(value, defaultValue2)) {
            GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);
            value = EnumPopup(label, value);
            GUI.backgroundColor = Color.white;
            return value;
        }
        else {
            return EnumPopup(label, value);
        }
    }

    /// <summary> An editor GUI helper to draw a well formatted list of enums </summary>
    public static List<T> PrettyEnumList<T>(string label, List<T> values, T defaultValue, string emptyLabel = "-none specified-", bool emptyListOkay = true) where T : struct {
        if (values == null)
            values = new List<T>();

        if (values.Count == 0) {
            EditorGUILayout.BeginHorizontal();
            if (!emptyListOkay) GUI.color = Color.red;
            EditorGUILayout.LabelField(label, emptyLabel);
            GUI.color = Color.white;
            if (AddButton())
                values.Add(defaultValue);
            EditorGUILayout.EndHorizontal();
        }
        else {
            for (int i = 0; i < values.Count; i++) {
                EditorGUILayout.BeginHorizontal();

                bool duplicate = false;
                for (int k = 0; k < values.Count; k++) {
                    if (i != k && values[i].Equals(values[k])) {
                        duplicate = true;
                        break;
                    }
                }

                if (duplicate) GUI.backgroundColor = new Color(1f, .6f, .6f);
                if (values[i].Equals(defaultValue)) GUI.backgroundColor = new Color(1f, .8f, .8f);

                values[i] = EnumPopup(i == 0 ? label : " ", values[i]);

                if (duplicate || values[i].Equals(defaultValue)) GUI.backgroundColor = Color.white;

                DrawListControls(values, i, true, defaultValue, true, true, true, false);

                EditorGUILayout.EndHorizontal();
            }
        }

        return values;
    }

    public static Enum EnumPopup(Type enumType, params GUILayoutOption[] options) {
        return EnumPopup(null, enumType, options);
    }

    public static Enum EnumPopup(string label, Type enumType, params GUILayoutOption[] options) {
        EnumRecord enumRecord = GetEnumRecord(enumType);

        int index;

        if (label != null)
            index = EditorGUILayout.Popup(label, 0, enumRecord.titles, options);
        else
            index = EditorGUILayout.Popup(0, enumRecord.titles, options);

        if (index < 0 || index >= enumRecord.names.Length)
            return (Enum)Enum.Parse(enumType, enumRecord.names[0]);
        else
            return (Enum)Enum.Parse(enumType, enumRecord.names[index]);
    }

    public static T EnumPopup<T>(T value, params GUILayoutOption[] options) where T : struct {
        return EnumPopup(null, value, false, null, options);
    }

    public static T EnumPopup<T>(string label, T value, params GUILayoutOption[] options) where T : struct {
        return EnumPopup(label, value, false, null, options);
    }

    public static T EnumPopup<T>(T value, List<T> badValues, params GUILayoutOption[] options) where T : struct {
        return EnumPopup(null, value, false, badValues, options);
    }

    public static T EnumPopup<T>(string label, T value, T badValue, params GUILayoutOption[] options) where T : struct {
        return EnumPopup(label, value, new List<T>()
        {
            badValue
        }, options);
    }

    public static T EnumPopup<T>(string label, T value, List<T> badValues, params GUILayoutOption[] options) where T : struct {
        return EnumPopup(label, value, false, badValues, options);
    }

    public static T EnumPopup<T>(string label, T value, bool drawValues, List<T> badValues = null, params GUILayoutOption[] options) where T : struct {
        Type enumType = typeof(T);

        EnumRecord enumGroupRecord = GetEnumRecord(enumType);

        string selectedName = Enum.GetName(enumType, value);
        int selectedIndex = Array.FindIndex(enumGroupRecord.names, n => n == selectedName);

        bool bad = false;
        if (badValues != null)
            if (selectedIndex >= 0 && selectedIndex < enumGroupRecord.names.Length)
                if (badValues.Contains((T)Enum.Parse(enumType, enumGroupRecord.names[selectedIndex])))
                    bad = true;

        if (bad) GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);

        if (label != null)
            selectedIndex = EditorGUILayout.Popup(label, selectedIndex, drawValues ? enumGroupRecord.titlesWithValue : enumGroupRecord.titles, options);
        else
            selectedIndex = EditorGUILayout.Popup(selectedIndex, drawValues ? enumGroupRecord.titlesWithValue : enumGroupRecord.titles, options);

        if (bad) GUI.backgroundColor = Color.white;

        if (selectedIndex < 0 || selectedIndex >= enumGroupRecord.names.Length)
            return (T)Enum.Parse(enumType, enumGroupRecord.names[0]);
        else
            return (T)Enum.Parse(enumType, enumGroupRecord.names[selectedIndex]);
    }

    public static int EnumPopup(string label, Type enumType, int value, bool drawValues, List<int> badValues, params GUILayoutOption[] options) {
        EnumRecord enumGroupRecord = GetEnumRecord(enumType);

        string selectedName = Enum.GetName(enumType, value);
        int selectedIndex = Array.FindIndex(enumGroupRecord.names, n => n == selectedName);

        bool bad = false;
        if (badValues != null)
            if (selectedIndex >= 0 && selectedIndex < enumGroupRecord.names.Length)
                if (badValues.Contains((int)Enum.Parse(enumType, enumGroupRecord.names[selectedIndex])))
                    bad = true;

        if (bad) GUI.backgroundColor = new Color(1.0f, 0.6f, 0.6f);

        if (label != null)
            selectedIndex = EditorGUILayout.Popup(label, selectedIndex, drawValues ? enumGroupRecord.titlesWithValue : enumGroupRecord.titles, options);
        else
            selectedIndex = EditorGUILayout.Popup(selectedIndex, drawValues ? enumGroupRecord.titlesWithValue : enumGroupRecord.titles, options);

        if (bad) GUI.backgroundColor = Color.white;

        if (selectedIndex < 0 || selectedIndex >= enumGroupRecord.names.Length)
            return (int)Enum.Parse(enumType, enumGroupRecord.names[0]);
        else
            return (int)Enum.Parse(enumType, enumGroupRecord.names[selectedIndex]);
    }

    private static EnumRecord GetEnumRecord(Type type) {
        if (_enumRecords.ContainsKey(type))
            return _enumRecords[type];

        _enumRecords.Add(type, new EnumRecord(type));

        return _enumRecords[type];
    }

    private static Dictionary<Type, EnumRecord> _enumRecords = new Dictionary<Type, EnumRecord>();

    private class EnumRecord {
        public readonly string[] names;
        public readonly string[] titles;
        public readonly string[] titlesWithValue;

        public EnumRecord(Type type) {
            names = Enum.GetNames(type);
            titles = names.Select(n => GetGroupPrefix(type, n) + PrettifyTitle(n) + GetObsoleteSuffix(type, n)).ToArray();
            titlesWithValue = names.Select(n => GetGroupPrefix(type, n) + PrettifyTitle(n) + " (" + (int)Enum.Parse(type, n) + ")" + GetObsoleteSuffix(type, n)).ToArray();
        }

        private string GetGroupPrefix(Type type, string value) {
            EnumGroup attribute = EnumGroup.GetAttribute<EnumGroup>(type, value);
            return attribute == null ? null : attribute.group + "/";
        }

        private string GetObsoleteSuffix(Type type, string value) {
            ObsoleteAttribute attribute = EnumGroup.GetAttribute<ObsoleteAttribute>(type, value);
            return attribute == null ? null : " [DEPRECATED]";
        }
    }

   
    #endregion

    public static string PrettifyTitle(string title, string prefixToRemove = null, string trimUntil = null, string suffixToRemove = null) {
        if (trimUntil != null) {
            int index = title.IndexOf(trimUntil, StringComparison.Ordinal);
            if (index != -1)
                title = title.Substring(index);
        }

        if (prefixToRemove != null && title.StartsWith(prefixToRemove))
            title = title.Substring(prefixToRemove.Length);

        if (suffixToRemove != null && title.EndsWith(suffixToRemove))
            title = title.Substring(0, title.Length - suffixToRemove.Length);

        title = title.Replace("_", " "); // AI_Cloud -> AI Cloud
        title = Regex.Replace(title, @"([a-z])([A-Z])", @"$1 $2"); // LootDistributor -> Loot Manager
        title = Regex.Replace(title, @"([A-Z])([A-Z][a-z])", @"$1 $2"); // AICloud -> AI Cloud
        title = Regex.Replace(title, @"([a-z])([0-9])", @"$1 $2"); // Hello2 -> Hello 2

        return title;
    }

    #region DrawList
    /// <summary>
    /// Draw an editable single-lined list of items with add/remove/rearrange controls.
    /// </summary>
    public static List<T> DrawList<T>(string listTitle, List<T> list, bool add, T defaultValue, bool remove, Func<List<T>, int, T> drawCallback) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (listTitle != null)
            EditorGUILayout.LabelField(listTitle, EditorStyles.boldLabel);

        if (list == null) // this will happen when new object doesn't create a list and expects the inspector to create it
            list = new List<T>();

        if (list.Count > 0) {
            for (int i = 0; i < list.Count; i++) {
                EditorGUILayout.BeginHorizontal();

                list[i] = drawCallback(list, i);

                if (DrawListControls(list, i, add, defaultValue, true, remove, true, false))
                    break;

                EditorGUILayout.EndHorizontal();
            }
        }
        else {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Empty");

            if (AddButton())
                list.Insert(0, defaultValue);

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        return list;
    }

    public static List<T> DrawList<T>(string listTitle, string entryLabel, List<T> list, bool add, T defaultValue, bool remove, Func<List<T>, int, string, T> drawCallback)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (listTitle != null)
            EditorGUILayout.LabelField(listTitle, EditorStyles.boldLabel);

        if (list == null) // this will happen when new object doesn't create a list and expects the inspector to create it
            list = new List<T>();

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                list[i] = drawCallback(list, i, entryLabel);

                if (DrawListControls(list, i, add, defaultValue, true, remove, true, false))
                    break;

                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Empty");

            if (AddButton())
                list.Insert(0, defaultValue);

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        return list;
    }

    public static List<T> DrawListInline<T>(List<T> list, ListControls controls, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(null, list, controls, default(T), drawCallback);
    }

    public static List<T> DrawListInline<T>(List<T> list, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(null, list, ListControls.All, default(T), drawCallback);
    }

    public static List<T> DrawListInline<T>(List<T> list, T defaultValue, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(null, list, ListControls.All, defaultValue, drawCallback);
    }

    public static List<T> DrawListInline<T>(string listTitle, List<T> list, ListControls controls, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(listTitle, list, controls, default(T), drawCallback);
    }

    public static List<T> DrawListInline<T>(string listTitle, List<T> list, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(listTitle, list, ListControls.All, default(T), drawCallback);
    }

    public static List<T> DrawListInline<T>(string listTitle, List<T> list, T defaultValue, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(listTitle, list, ListControls.All, defaultValue, drawCallback);
    }

    public static List<T> DrawListInline<T>(List<T> list, ListControls controls, T defaultValue, Func<List<T>, int, T> drawCallback) {
        return DrawListInline(null, list, controls, defaultValue, drawCallback);
    }

    /// <summary>
    /// Draw an editable single-lined non-boxed inlined list of items with add/remove/rearrange controls.
    /// </summary>
    public static List<T> DrawListInline<T>(string listTitle, List<T> list, ListControls controls, T defaultValue, Func<List<T>, int, T> drawCallback) {
        if (list == null) // this will happen when new object doesn't create a list and expects the inspector to show/create it
            list = new List<T>();

        if (list.Count > 0) {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < list.Count; i++) {
                EditorGUILayout.BeginHorizontal();

                if (listTitle != null)
                    EditorGUILayout.PrefixLabel(i == 0 ? listTitle : i == 0 ? "Entries" : " ", EditorStyles.label);

                list[i] = drawCallback(list, i);

                if (DrawListControls(
                    list, i,
                    (controls & ListControls.Add) == ListControls.Add, defaultValue, true,
                    (controls & ListControls.Remove) == ListControls.Remove,
                    (controls & ListControls.Move) == ListControls.Move,
                    false,
                    16, 14
                ))
                    break;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
        else {
            EditorGUILayout.BeginHorizontal();

            if (listTitle != null)
                EditorGUILayout.LabelField(listTitle, "Empty");
            else
                GUILayout.Label("Empty");

            DrawListControls(
                list, -1,
                (controls & ListControls.Add) == ListControls.Add, defaultValue,
                false,
                false,
                false, true,
                16, 14
            );

            EditorGUILayout.EndHorizontal();
        }

        return list;
    }

    [Flags]
    public enum ListControls {
        None = 0,
        Add = 1,
        Remove = 2,
        Move = 4,
        AddRemove = Add | Remove,
        RemoveMove = Remove | Move,
        All = Add | Remove | Move,
        // Don't think "add but not remove" or "remove but not add" makes much sense
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, string itemCaption,
        ListControls allowedListControls, T defaultValue,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            delegate (T item, int index) { return itemCaption + " #" + index; },
            allowedListControls,
            defaultValue,
            true,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, string itemCaption,
        ListControls allowedListControls,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            delegate (T item, int index) { return itemCaption + " #" + index; },
            allowedListControls,
            default(T),
            false,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, string itemCaption,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            delegate (T item, int index) { return itemCaption + " #" + index; },
            ListControls.All,
            default(T),
            false,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(List<T> list, string itemCaption,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            null,
            list,
            delegate (T item, int index) { return itemCaption + " #" + index; },
            ListControls.All,
            default(T),
            false,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, Func<T, int, string> itemCaptionCallback,
        ListControls allowedListControls, T defaultValue,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            itemCaptionCallback,
            allowedListControls,
            defaultValue,
            true,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, Func<T, int, string> itemCaptionCallback,
        ListControls allowedListControls,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            itemCaptionCallback,
            allowedListControls,
            default(T),
            false,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    public static List<T> DrawExtendedList<T>(string listTitle, List<T> list, Func<T, int, string> itemCaptionCallback,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        return DrawExtendedList(
            listTitle,
            list,
            itemCaptionCallback,
            ListControls.All,
            default(T),
            false,
            drawCallback, addCallback, removeCallback,
            referenceObject);
    }

    /// <summary>
    /// Draw an editable multi-lined list of items with add/remove/rearrange controls.
    /// </summary>
    private static List<T> DrawExtendedList<T>(string listTitle, List<T> list, Func<T, int, string> itemCaptionCallback,
        ListControls allowedListControls, T defaultValue, bool useGivenDefaultValue,
        Func<T, T> drawCallback, Action<List<T>, int> addCallback = null, Action<List<T>, int> removeCallback = null,
        object referenceObject = null) {
        if (listTitle != null)
            EditorGUILayout.LabelField(listTitle, EditorStyles.boldLabel);

        if (list == null) // this will happen when new object doesn't create a list and expects the inspector to create it
            list = new List<T>();

        if (list.Count > 0) {
            for (int i = 0; i < list.Count; i++) {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.BeginHorizontal();

                //EditorGUILayout.LabelField(itemCaption + " #" + (i + 1));
                EditorGUILayout.LabelField(itemCaptionCallback(list[i], i));

                if (DrawListControls(
                    list, i,
                    (allowedListControls & ListControls.Add) == ListControls.Add, defaultValue, useGivenDefaultValue,
                    (allowedListControls & ListControls.Remove) == ListControls.Remove,
                    (allowedListControls & ListControls.Move) == ListControls.Move, false,
                    20, 18, addCallback, removeCallback, referenceObject))
                    break;

                EditorGUILayout.EndHorizontal();

                list[i] = drawCallback(list[i]);

                EditorGUILayout.EndVertical();
            }
        }
        else {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            EditorGUILayout.LabelField("None");

            DrawListControls(list, -1,
                (allowedListControls & ListControls.Add) == ListControls.Add, defaultValue, useGivenDefaultValue,
                false,
                false, true,
                20, 18, addCallback, null, referenceObject);

            EditorGUILayout.EndHorizontal();
        }
        return list;
    }
    #endregion

    /// <summary> Returns whether the given child has the given object as its parent somewhere along the ancestry </summary>
    public static bool HasParent(Transform child, Transform parent) {
        if (child == parent)
            return true; // well, yes, I suppose if we are our own parent, then technically our parent is in our hierarchy

        if (child.transform.parent == null)
            return false;

        if (child.transform.parent == parent)
            return true;

        return HasParent(child.transform.parent, parent);
    }

    /// <summary> Warns the user (via help box) that in case the object (of the given script) is not based on a prefab. </summary>
    public static void WarnAboutPrefab(MonoBehaviour script) {
        // During run-time, none of the objects have a prefab connection, this is purely an editor thing
        if (Application.isPlaying)
            return;

        if (PrefabUtility.GetPrefabInstanceHandle(script.gameObject) == null) {
            EditorGUILayout.HelpBox("We do not appear to be based upon a prefab. " + "Updating this object will require manual adjustements for any future changes.", MessageType.Warning);
        }
    }

    public static bool IsPrefab(Object target) {
        return PrefabUtility.GetPrefabInstanceHandle(target) != null;
    }

    /// <summary>
    /// Get the current scene name either from the one loaded in editor (design-time) or the currently active one (run-time)
    /// </summary>
    public static string CurrentSceneName() {
        if (Application.isPlaying) {
            return SceneManager.GetActiveScene().name;
        }
        else {
            string currentScene = EditorSceneManager.GetActiveScene().name;
            if (!string.IsNullOrEmpty(currentScene)) {
                currentScene = currentScene.Substring(currentScene.LastIndexOf('/') + 1);
                currentScene = currentScene.Substring(0, currentScene.LastIndexOf('.'));
                return currentScene;
            }
            else {
                return "";
            }
        }
    }


    /// <summary>
    /// Returns true if any Attribute of specified type
    /// is found on the given enum. Uses reflection.
    /// </summary>
    public static bool EnumValueHasAttribute<EnumType, AttributeType>(EnumType value) where EnumType : struct where AttributeType : Attribute {
        //	Get the member info for the buff's type
        System.Reflection.MemberInfo[] memInfo = typeof(EnumType).GetMember(value.ToString());
        //	Get any ObsoleteAttributes on that member (should really only be the one)
        object[] attributes = memInfo[0].GetCustomAttributes(typeof(AttributeType), false);

        //	Check if we found any ObsoleteAttribute
        return attributes.Length > 0;
    }

    /// <summary>
    /// Returns true if any ObsoleteAttribute is
    /// found on the given enum. Uses reflection.
    /// </summary>
    public static bool EnumValueIsObsolete<T>(T value) where T : struct {
        return EnumValueHasAttribute<T, ObsoleteAttribute>(value);
    }

    /// <summary>
    /// If the given enum value has a EnumTooltip, show it as a help message, otherwise show default text (if any)
    /// </summary>
    public static void ShowEnumTooltip<T>(T value, string defaultText = null, bool indent = false, bool help = false) where T : struct {
        ShowEnumTooltip(value, new T(), defaultText, indent, help);
    }

    /// <summary>
    /// If the given enum value has a EnumTooltip, show it as a help message, otherwise show default text (if any)
    /// </summary>
    public static void ShowEnumTooltip<T>(T value, T ignoreOnValue, string defaultText = null, bool indent = false, bool help = false) where T : struct {
        EnumTooltip enumTooltip = EnumTooltip.GetAttribute<EnumTooltip, T>(value, ignoreOnValue);

        if (enumTooltip != null) {
            if (!String.IsNullOrEmpty(enumTooltip.text)) {
                if (indent)
                    EditorGUILayout.LabelField(" ", enumTooltip.text, help ? HelpLabel : EditorStyles.wordWrappedLabel);
                else
                    EditorGUILayout.LabelField(enumTooltip.text, help ? HelpLabel : EditorStyles.wordWrappedLabel);
                return;
            }
        }

        if (!String.IsNullOrEmpty(defaultText)) {
            if (indent)
                EditorGUILayout.LabelField(" ", defaultText, help ? HelpLabel : EditorStyles.wordWrappedLabel);
            else
                EditorGUILayout.LabelField(defaultText, help ? HelpLabel : EditorStyles.wordWrappedLabel);
        }
    }


    /// <summary>
    /// Draw a EditorGUILayout.FloatField with an extra suffix label after a field (for example, "sec")
    /// </summary>
    public static float FloatFieldClamped(string label, float value, float minValue = 0f, float maxValue = 1f) {
        return Mathf.Max(minValue, Mathf.Min(maxValue, EditorGUILayout.FloatField(label, value)));
    }

    /// <summary>
    /// Draw a EditorGUILayout.FloatField with an extra suffix label after a field (for example, "sec")
    /// </summary>
    public static float FloatFieldSuffixed(string label, float value, string suffix) {
        EditorGUILayout.BeginHorizontal();
        float newValue = EditorGUILayout.FloatField(label, value);
        GUILayout.Label(suffix, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();
        return newValue;
    }

    /// <summary>
    /// Draw a EditorGUILayout.IntField with an extra suffix label after a field (for example, "sec")
    /// </summary>
    public static int IntFieldSuffixed(string label, int value, string suffix) {
        EditorGUILayout.BeginHorizontal();
        int newValue = EditorGUILayout.IntField(label, value);
        GUILayout.Label(suffix, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();
        return newValue;
    }

    /// <summary>
    /// Draw a float field that represents a period value and another float field for the value expressed as frequency.
    /// In other words, something like "0.33 sec cooldown - 3 per second" for designer convenience.
    /// </summary>
    public static float TimingFloatField(string label, float value, string suffix) {
        EditorGUILayout.BeginHorizontal();
        value = Mathf.Max(0f, EditorGUILayout.FloatField(label, value));

        GUILayout.Label(" (", GUILayout.ExpandWidth(false));

        if (value == 0f) {
            GUILayout.Label(new GUIContent("INF", "Not really infinite, becuase it is limited by logic frequency, but mathematically so"), GUILayout.Width(60));
        }
        else {
            value = Mathf.Max(0f, EditorGUILayout.FloatField(1f / value, GUILayout.Width(60)));
            if (value != 0f)
                value = 1f / value;
        }

        GUILayout.Label(suffix + ")", GUILayout.ExpandWidth(false));

        EditorGUILayout.EndHorizontal();

        return value;
    }

    /// <summary>
    /// Draw a float field that represents a percentage.
    /// This will show a friendly percent-based value (e.g. 75%) rather than fraction-based (i.e. 0.75)
    /// </summary>
    public static float PercentFloatField(string label, float value, string suffix = "%", params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();

        if (label != null)
            value = EditorGUILayout.FloatField(label, value * 100f, options) / 100f;
        else
            value = EditorGUILayout.FloatField(value * 100f, options) / 100f;

        GUILayout.Label(suffix, GUILayout.ExpandWidth(false));

        EditorGUILayout.EndHorizontal();

        return value;
    }

    /// <summary>
    /// Draw a float field that represents a time in seconds.
    /// This shows both the field for the raw/total number and individual fields for min/sec.
    /// </summary>
    public static float TimeFloatField(string label, float value, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();

        if (label != null)
            value = EditorGUILayout.FloatField(label, value, options);
        else
            value = EditorGUILayout.FloatField(value, options);

        int min = Mathf.FloorToInt(value / 60f);
        float sec = value % 60f;

        GUILayout.Label("sec (", GUILayout.ExpandWidth(false));

        min = EditorGUILayout.IntField(min, GUILayout.Width(40));

        GUILayout.Label("min ", GUILayout.ExpandWidth(false));

        sec = EditorGUILayout.FloatField(sec, GUILayout.Width(40));

        GUILayout.Label("sec)", GUILayout.ExpandWidth(false));

        EditorGUILayout.EndHorizontal();

        return min * 60f + sec;
    }

    /// <summary>
    /// 
    /// </summary>
    public static float FloatFieldWithInverse(string label, float value, string suffix1, string suffix2, params GUILayoutOption[] options) {
        EditorGUILayout.BeginHorizontal();

        if (label != null)
            value = EditorGUILayout.FloatField(label, value, options);
        else
            value = EditorGUILayout.FloatField(value, options);

        GUILayout.Label(suffix1, GUILayout.ExpandWidth(false));

        if (value != 0) {
            float newValue = EditorGUILayout.FloatField(1f / value, options);
            if (newValue != 0)
                value = 1f / newValue;

            GUILayout.Label(suffix2, GUILayout.ExpandWidth(false));
        }
        else {
            GUILayout.Label("NaN " + suffix2, GUILayout.ExpandWidth(false));
        }

        EditorGUILayout.EndHorizontal();

        return value;
    }

    public static string SecondsToTimeString(float time) {
        return Mathf.FloorToInt(time / 60f) + ":" + PadNumber(time % 60f);
    }

    private static string PadNumber(float value, string padding = "0") {
        return value < 10 ? padding + value.ToString("F2") : value.ToString("F2");
    }

    public static void MinMaxSlider(string minMaxLevel, ref int valueFrom, ref int valueTo, int minValue, int maxValue) {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent(minMaxLevel));

        float floatMinLevel = valueFrom;
        float floatMaxLevel = valueTo;
        EditorGUILayout.MinMaxSlider(ref floatMinLevel, ref floatMaxLevel, minValue, maxValue);
        valueFrom = Mathf.FloorToInt(valueFrom);
        valueTo = Mathf.CeilToInt(valueTo);

        valueFrom = Mathf.Max(minValue, Mathf.Min(valueTo, EditorGUILayout.IntField(valueFrom, GUILayout.Width(30))));
        valueTo = Mathf.Max(valueFrom, Mathf.Min(maxValue, EditorGUILayout.IntField(valueTo, GUILayout.Width(30))));

        string label;
        if (valueFrom == minValue && valueTo == maxValue)
            label = "All";
        else if (valueFrom == valueTo)
            label = "= " + valueFrom;
        else if (valueFrom == minValue)
            label = "<= " + valueTo;
        else if (valueTo == maxValue)
            label = ">= " + valueFrom;
        else
            label = valueFrom + " - " + valueTo;
        GUILayout.Label(label, GUILayout.Width(50));

        EditorGUILayout.EndHorizontal();
    }


    /// <summary> Chaange the layer or the given object and all its children to the new layer </summary>
    public static void ChangeLayer(GameObject targetObject, LayerMask newLayer) {
        targetObject.layer = newLayer;
        foreach (Transform child in targetObject.transform)
            ChangeLayer(child.gameObject, newLayer);
    }

    public static GameObject AddNewChild(GameObject parent, string newName = "Child") {

        PrefabAssetType type = PrefabUtility.GetPrefabAssetType(parent);

        //if (PrefabUtility.GetPrefabType(parent) == PrefabType.Prefab) {
        //    return AddNewChildToPrefab(parent, newName);
        //}
        if (type == PrefabAssetType.Regular)
            return AddNewChildToPrefab(parent, newName);
        else
        {
            GameObject newGo = new GameObject(newName);
            newGo.transform.parent = parent.transform;
            return newGo;
        }
    }

    /// <summary> Adds a new empty child to a prefab (can't do this directly to prefabs/assets, so this goes through the rigmaroll) </summary>
    public static GameObject AddNewChildToPrefab(GameObject parent, string newName = "Child") {
        if (Application.isPlaying)
            return null; // Umm, no

        // Find the root of prefab ("parent"), i.e. the actual prefab
        GameObject prefabRoot = parent.transform.root.gameObject;

        // Make a scene copy with prefab link
        GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);

        // Find the instanced parent version of the newly created prefab instance that links to the given prefab parent object
        // In other words, if we are making a child of a child of a prefab, we need to know exactly where to place the new child

        GameObject instancedParent = null;

        if (PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance) as GameObject == parent)
            instancedParent = prefabInstance;

        if (instancedParent == null)
            foreach (Transform transform in prefabInstance.transform)
                if (PrefabUtility.GetCorrespondingObjectFromSource(transform.gameObject) as GameObject == parent)
                    instancedParent = transform.gameObject;

        if (instancedParent == null)
            return null;

        // Make the new GO
        GameObject newChild = new GameObject(newName);

        // Attach the new GO to the prefab instance
        newChild.transform.parent = instancedParent.transform;

        GameObject corospondingObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefabInstance);

        // Update actual prefab
        //PrefabUtility.ReplacePrefab(prefabInstance, corospondingObject, ReplacePrefabOptions.ConnectToPrefab);

        PrefabUtility.SavePrefabAsset(corospondingObject);

        // Get the actual child under the updated prefab (not the one that will get destroyed)
        GameObject prefabChild = PrefabUtility.GetCorrespondingObjectFromSource(newChild) as GameObject;

        // Get rid of prefab instance
        GameObject.DestroyImmediate(prefabInstance);

        // Finally, return our creation
        return prefabChild;
    }

    public static bool RemoveChild(GameObject child) {
        if (PrefabUtility.GetPrefabAssetType(child.transform.root) == PrefabAssetType.Regular) {
            return RemoveChildFromPrefab(child);
        }
        else {
            GameObject.DestroyImmediate(child);
            return true;
        }
    }


    /// <summary> Deletes a prefab's (direct) child (can't do this directly to prefabs/assets, so this goes through the rigmaroll) </summary>
    public static bool RemoveChildFromPrefab(GameObject child) {
        // Find the root of child, i.e. the actual prefab
        GameObject parent = child.transform.root.gameObject;

        // Make a scene copy with prefab link
        GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(parent);

        // Find the child of the newly created instance that links to our to-delete child

        GameObject instanceChild = null;

        foreach (Transform tempChild in prefabInstance.transform) {
            if (PrefabUtility.GetCorrespondingObjectFromSource(tempChild.gameObject) as GameObject == child) {
                instanceChild = tempChild.gameObject;
                break;
            }

            foreach (Transform tempGrandchild in tempChild.transform) {
                if (PrefabUtility.GetCorrespondingObjectFromSource(tempGrandchild.gameObject) as GameObject == child) {
                    instanceChild = tempGrandchild.gameObject;
                    break; // Come on C#, break 2;
                }
            }

            if (instanceChild != null)
                break;
        }

        // If we found the child (we really should have unless it is very deep), remove it and update rpefab

        if (instanceChild != null) {
            // Destroy the child in the instance
            GameObject.DestroyImmediate(instanceChild);

            // Update actual prefab
            //PrefabUtility.ReplacePrefab(prefabInstance, PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance), ReplacePrefabOptions.ConnectToPrefab);
            PrefabUtility.SavePrefabAsset(PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance));

            // Get rid of prefab instance
            GameObject.DestroyImmediate(prefabInstance);

            return true;
        }
        else {
            // Get rid of prefab instance
            GameObject.DestroyImmediate(prefabInstance);

            return false;
        }
    }

    public static bool RemoveComponent(Component child) {
        if (PrefabUtility.GetPrefabAssetType(child.transform.root) == PrefabAssetType.Regular) {
            return RemoveComponentFromPrefab(child);
        }
        else {
            GameObject.DestroyImmediate(child);
            return true;
        }
    }

    /// <summary> Deletes a prefab's (direct) child (can't do this directly to prefabs/assets, so this goes through the rigmaroll) </summary>
    public static bool RemoveComponentFromPrefab(Component child) {
        // Find the root of child, i.e. the actual prefab
        GameObject parent = child.transform.root.gameObject;

        // Make a scene copy with prefab link
        GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(parent);

        // Find the child of the newly created instance that links to our to-delete child

        Component instanceChild = null;

        Component[] tempChildren = prefabInstance.GetComponentsInChildren<Component>();

        foreach (Component tempChild in tempChildren) {
            if (PrefabUtility.GetCorrespondingObjectFromSource(tempChild) as Component == child) {
                instanceChild = tempChild;
                break;
            }
        }

        // If we found the child (we really should have unless it is very deep), remove it and update prefab

        if (instanceChild != null) {
            // Destroy the child in the instance
            GameObject.DestroyImmediate(instanceChild);

            // Update actual prefab
            //PrefabUtility.ReplacePrefab(prefabInstance, PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance), ReplacePrefabOptions.ConnectToPrefab);
            PrefabUtility.SavePrefabAsset(PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance));

            // Get rid of prefab instance
            GameObject.DestroyImmediate(prefabInstance);

            return true;
        }
        else {
            // Get rid of prefab instance
            GameObject.DestroyImmediate(prefabInstance);

            return false;
        }
    }


    #region Draw Inspector Section

    /// <summary>
    /// Draws 'drawMethod' inside of an inspector section.
    /// </summary>
    public static void DrawInspectorSection(string caption, Action drawMethod, bool help = false, string helpText = null) {
        DrawInspectorSectionHeader(caption);
        if (help && helpText != null)
            EditorGUILayout.LabelField(helpText, HelpLabel);
        drawMethod();
        DrawInspectorSectionFooter();
    }

    /// <summary>
    /// Draws 'drawMethod' inside of an inspector section, where the caption serves as a foldout.
    /// </summary>
    public static void DrawInspectorSectionFoldout(ref bool foldout, string caption, Action drawMethod, bool help = false, string helpText = null) {
        DrawInspectorSectionHeaderFoldout(ref foldout, caption);
        if (foldout) {
            EditorGUI.indentLevel++;
            if (help && helpText != null)
                EditorGUILayout.LabelField(helpText, HelpLabel);
            drawMethod();
            EditorGUI.indentLevel--;
        }
        DrawInspectorSectionFooterFoldout();
    }

    //public static void DrawInspectorSectionFoldout(ref bool foldout, string caption, Action<SpecialAbility> drawMethod, SpecialAbility entry, bool help = false, string helpText = null) {
    //    DrawInspectorSectionHeaderFoldout(ref foldout, caption);
    //    if (foldout) {
    //        EditorGUI.indentLevel++;
    //        if (help && helpText != null)
    //            EditorGUILayout.LabelField(helpText, HelpLabel);
    //        drawMethod(entry);
    //        EditorGUI.indentLevel--;
    //    }
    //    DrawInspectorSectionFooterFoldout();
    //}

    /// <summary>
    /// Draws the default section header:
    /// Beginning a vertical group and displaying caption in bold.
    /// This also closed the previous one.
    /// </summary>
    public static void DrawInspectorSectionNextHeader(string caption) {
        DrawInspectorSectionFooter();
        DrawInspectorSectionHeader(caption);
    }

    /// <summary>
    /// Draws the default section header:
    /// Beginning a vertical group and displaying caption in bold.
    /// </summary>
    public static void DrawInspectorSectionHeader(string caption) {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        if (!string.IsNullOrEmpty(caption))
            GUILayout.Label(caption, EditorStyles.boldLabel);
    }

    /// <summary>
    /// Draws the default section header as a foldout:
    /// Beginning a vertical group and displaying caption in bold as a foldout,
    /// as well as increasing the indent level.
    /// </summary>
    public static void DrawInspectorSectionHeaderFoldout(ref bool foldout, string caption) {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        GUIStyle style = EditorStyles.foldout;
        FontStyle previousStyle = style.fontStyle;
        style.fontStyle = FontStyle.Bold;

        foldout = EditorGUILayout.Foldout(foldout, caption, style);

        style.fontStyle = previousStyle;
    }

    /// <summary>
    /// Draws the default section footer:
    /// Ending the vertical group.
    /// </summary>
    public static void DrawInspectorSectionFooter() {
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Draws the default section footer as a foldout:
    /// Returning indentation to normal and ending the vertical group.
    /// </summary>
    public static void DrawInspectorSectionFooterFoldout() {
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    #endregion


    public static Texture2D Make1x1Texure(Color color) {
        // http://forum.unity3d.com/threads/changing-the-background-color-for-beginhorizontal.66015/

        Texture2D result = new Texture2D(1, 1);
        result.SetPixels(new Color[1] { color });
        result.Apply();

        return result;
    }

    public static bool ButtonOkay(string tooltip = "Okay") {
        return GUILayout.Button(new GUIContent(IconOkayTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static bool ButtonWarning(string tooltip = "Warning") {
        return GUILayout.Button(new GUIContent(IconWarningTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static bool ButtonError(string tooltip = "Error") {
        return GUILayout.Button(new GUIContent(IconErrorTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static bool ButtonDisabled(string tooltip = "Disabled") {
        return GUILayout.Button(new GUIContent(IconDisabledTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static bool ButtonConfigurationProblem(string tooltip = "Configuration problem") {
        return GUILayout.Button(new GUIContent(IconConfigIssueTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static void IconOkay(string tooltip = "Okay") {
        GUILayout.Label(new GUIContent(IconOkayTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static void IconWarning(string tooltip = "Warning") {
        GUILayout.Label(new GUIContent(IconWarningTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static void IconError(string tooltip = "Error") {
        GUILayout.Label(new GUIContent(IconErrorTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static void IconDisabled(string tooltip = "Disabled") {
        GUILayout.Label(new GUIContent(IconDisabledTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    public static void IconConfigurationProblem(string tooltip = "Configuration problem") {
        GUILayout.Label(new GUIContent(IconConfigIssueTexture, tooltip), IconStyle, GUILayout.Width(16), GUILayout.Height(16));
    }

    private static object _lastReferenceObject = null;

    public static bool ConfirmButton(string label, ref bool confirmCheck, object referenceObject, params GUILayoutOption[] guiLayoutOptions) {
        return ConfirmButton(label, label, ref confirmCheck, referenceObject, guiLayoutOptions);
    }

    public static bool ConfirmButton(string label, string confirmLabel, ref bool confirmCheck, object referenceObject, params GUILayoutOption[] guiLayoutOptions) {
        bool result = false;

        if (confirmCheck && _lastReferenceObject == referenceObject) {
            GUI.backgroundColor = new Color(0.7f, 1.0f, 0.7f);
            if (GUILayout.Button(confirmLabel, guiLayoutOptions)) {
                confirmCheck = false;
                _lastReferenceObject = null;
                result = true;
            }
            GUI.backgroundColor = Color.white;
        }
        else {
            if (GUILayout.Button(label, guiLayoutOptions)) {
                _lastReferenceObject = referenceObject;
                confirmCheck = true;
            }
        }

        return result;
    }

    public static void GreenLabel(string text) {
        Color color = GUI.color;
        GUI.color = new Color(0f, 0.5f, 0f, 1f);
        GUILayout.Label(text, TintableLabel, GUILayout.ExpandWidth(false));
        GUI.color = color;
    }

    public static void RedLabel(string text) {
        Color color = GUI.color;
        GUI.color = new Color(0.5f, 0f, 0f, 1f);
        GUILayout.Label(text, TintableLabel, GUILayout.ExpandWidth(false));
        GUI.color = color;
    }

    /// <summary>
    /// 
    /// Note: Make sure to do "GUI.SetNextControlName("DefocusTabs");" beforehand on some random one for tab-switching defocusing needs
    /// </summary>
    public static void DrawTab<T>(T tab, string label, ref T currentTab, int width = 100) where T : struct {
        if (currentTab.Equals(tab)) GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
        if (GUILayout.Toggle(currentTab.Equals(tab), label, EditorStyles.toolbarButton, GUILayout.Width(width))) {
            if (!currentTab.Equals(tab)) {
                currentTab = tab;
                GUI.FocusControl("DefocusTabs"); // remove any glitchy focus from editor fields
            }
        }
        GUI.backgroundColor = Color.white;
    }

    /// <summary>
    /// 
    /// Returns whether there was a change
    /// </summary>
    public static bool DrawToggleTabs<T>(ref T value, string[] labels, T[] values) {
        EditorGUILayout.BeginHorizontal();

        bool changed = false;

        for (int i = 0; i < values.Length; i++) {
            bool current = value.Equals(values[i]);
            DrawToggleTab(ref current, labels[i], null);
            if (current != value.Equals(values[i])) {
                changed = true;
                if (current)
                    value = values[i];
            }
        }

        EditorGUILayout.EndHorizontal();

        return changed;
    }
    /// <summary>
    /// 
    /// Returns whether there was a change
    /// </summary>
    public static bool DrawStackedToggleTabs<T>(ref T value, string[] labels, T[] values, int rows) {
        EditorGUILayout.BeginHorizontal();

        bool changed = false;

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < values.Length; i++) {

            bool current = value.Equals(values[i]);
            DrawToggleTab(ref current, labels[i], null);
            if (current != value.Equals(values[i])) {
                changed = true;
                if (current)
                    value = values[i];
            }

            if (i % rows == 1 || i == values.Length) {
                EditorGUILayout.EndHorizontal();

                if (i != values.Length)
                    EditorGUILayout.BeginVertical();
            }
        }

        EditorGUILayout.EndHorizontal();

        return changed;
    }
    /// <summary>
    /// 
    /// Returns whether there was a change
    /// </summary>
    public static bool DrawToggleTabs<T>(ref T value, string[] labels, T[] values, int width) {
        EditorGUILayout.BeginHorizontal();

        bool changed = false;

        for (int i = 0; i < values.Length; i++) {
            bool current = value.Equals(values[i]);
            DrawToggleTab(ref current, labels[i], null, width);
            if (current != value.Equals(values[i])) {
                changed = true;
                if (current)
                    value = values[i];
            }
        }

        EditorGUILayout.EndHorizontal();

        return changed;
    }


    /// <summary>
    /// 
    /// Note: Make sure to do "GUI.SetNextControlName("DefocusTabs");" beforehand on some random one for tab-switching defocusing needs
    /// </summary>
    public static void DrawToggleTab(ref bool value, string label, string tooltip, int width = 120) {
        if (value) GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
        bool newValue = GUILayout.Toggle(value, new GUIContent(label, tooltip), EditorStyles.toolbarButton, GUILayout.Width(width));
        if (value != newValue) {
            value = newValue;
            GUI.FocusControl("DefocusTabs"); // remove any glitchy focus from editor fields
        }
        GUI.backgroundColor = Color.white;
    }

    /// <summary> Returns an array with the names of the derived classes of the specified type </summary>
    public static string[] GetImplementingClassNames(Type type) {
        Type[] mainTypes = type.Assembly.GetTypes();
        Type[] allTypes = mainTypes.Where(t => t.BaseType == type).ToArray();
        return allTypes.Select(t => t.Name).ToArray();
    }

    public static void DrawIconFromAsset(string fileName, string basePath, bool despace = false, string extension = "png", float maxHeight = 60f) {
        // Load the texture as asset from the given path
        Texture2D texture = GetIconFromAsset(fileName, basePath, despace, extension);

        DrawTexture(texture);
    }

    public static void DrawTexture(Texture2D texture, float maxHeight = 60f) {
        if (texture != null) {
            // Display the loaded texture as a label sized to fit
            float width = Mathf.Min(texture.width, maxHeight);
            GUILayout.Label(texture, GUILayout.Width(width), GUILayout.Height(texture.height * (width / texture.width)));
        }
    }

    private static readonly Dictionary<string, Texture2D> _cachedIconTextures = new Dictionary<string, Texture2D>();

    public static Texture2D GetIconFromAsset(string fileName, string basePath, bool despace = false, string extension = "png") {
        //// Convert the image to have spaces and such
        if (despace)
            fileName = Regex.Replace(fileName, @"([a-z](?![a-z])|[A-Z](?=[A-Z][^A-Z]))(?!$)", "$1 ").Replace(" N ", " N' ");

        if (!_cachedIconTextures.ContainsKey(fileName)) {
            // Start with the full path where the bonusDisplays are expected
            string path = Application.dataPath + basePath;

            if (!Directory.Exists(path))
                return null;

            // Scan all files and find out texture
            string textureLocation = Directory.GetFiles(path, fileName + "." + extension, SearchOption.AllDirectories).FirstOrDefault();

            // If not found, bail
            if (textureLocation == null)
                return null;

            // Trim the implicit data path from the location to use with load
            textureLocation = textureLocation.Substring(Application.dataPath.Length - 6);

            // Load the texture as asset from the given path and return it
            Texture2D texture2D = (Texture2D)AssetDatabase.LoadAssetAtPath(textureLocation, typeof(Texture2D));

            if (texture2D != null)
                _cachedIconTextures.Add(fileName, texture2D);
        }

        if (_cachedIconTextures.ContainsKey(fileName))
            return _cachedIconTextures[fileName];

        return null;
    }


    //private static Vector2 mousePosition;
    //private static bool lastKnobRect = false;
    //private static Rect knobRect;


    //public static float FloatAngle(string caption, float value, string suffix, float snap, float min, float max)
    //{
    //    Event currentEvent = Event.current;

    //    EditorGUILayout.BeginHorizontal();

    //    GUILayout.Label(caption, GUILayout.Width(EditorGUIUtility.labelWidth));

    //    //Rect knobRect = new Rect(rect.x, rect.y, rect.height, rect.height);

    //    float delta;
    //    if (min != max)
    //        delta = ((max - min) / 360);
    //    else
    //        delta = 1;

    //    GUI.matrix = GUI.matrix;

    //    Matrix4x4 matrix = GUI.matrix;

    //    if (lastKnobRect)
    //        GUIUtility.RotateAroundPivot(value * Mathf.Rad2Deg, knobRect.center);

    //    GUILayout.Label(DialTexture, GUILayout.Width(DialTexture.width), GUILayout.Height(DialTexture.height));

    //    if (currentEvent.type == EventType.repaint)
    //    {
    //        lastKnobRect = true;
    //        knobRect = GUILayoutUtility.GetLastRect();
    //        Debug.Log(knobRect);
    //    }

    //    int id = GUIUtility.GetControlID(FocusType.Native, knobRect); // really, Unity, really -- where is GetLastControlID

    //    GUI.matrix = matrix;


    //    if (currentEvent != null)
    //    {
    //        if (currentEvent.type == EventType.MouseDown && knobRect.Contains(currentEvent.mousePosition))
    //        {
    //            GUIUtility.hotControl = id;
    //            mousePosition = currentEvent.mousePosition;
    //        }
    //        else if (currentEvent.type == EventType.MouseUp && GUIUtility.hotControl == id)
    //            GUIUtility.hotControl = 0;
    //        else if (currentEvent.type == EventType.MouseDrag && GUIUtility.hotControl == id)
    //        {
    //            //Vector2 move = mousePosition - currentEvent.mousePosition;
    //            //value += delta * (-move.x - move.y);

    //            float diffX = mousePosition.x - knobRect.center.x;
    //            float diffY = mousePosition.y - knobRect.center.y;
    //            value = Mathf.Atan2(diffY, diffX) + Mathf.PI / 2f;

    //            if (snap > 0)
    //            {
    //                float mod = value % snap;

    //                if (mod < (delta * 3) || Mathf.Abs(mod - snap) < (delta * 3))
    //                    value = Mathf.Round(value / snap) * snap;
    //            }

    //            mousePosition = currentEvent.mousePosition;
    //            GUI.changed = true;
    //        }
    //    }


    //    value = EditorGUILayout.FloatField(value * Mathf.Rad2Deg) * Mathf.Deg2Rad;

    //    GUILayout.Label(suffix, GUILayout.ExpandWidth(false));

    //    EditorGUILayout.EndHorizontal();

    //    return value;
    //}

    public static int IntPopup(int value, int minValue, int maxValue, params GUILayoutOption[] options) {
        string[] list = new string[maxValue - minValue + 1];
        for (int i = minValue; i <= maxValue; i++)
            list[i - minValue] = i.ToString();

        value = Mathf.Clamp(value, minValue, maxValue);

        int index = EditorGUILayout.Popup(value - minValue, list, options);
        return minValue + index;
    }

    public static int IntPopupReverting(int minValue, int maxValue, params GUILayoutOption[] options) {
        string[] list = new string[maxValue - minValue + 2];
        list[0] = "(select)";
        for (int i = minValue; i <= maxValue; i++)
            list[i - minValue] = i.ToString();

        int index = EditorGUILayout.Popup(0, list, options);
        return minValue + index - 1;
    }


    private static Dictionary<string, object> _lastResourceObjects;

    /// <summary>
    /// Shows a smart resource location picker that uses an object field instead of a string if a resoruce is found
    /// and allows drag & dropping of existing resources that get converted into a path.
    /// This will show empty/null object if the path is not set.
    /// Invalid/non-found paths show as a text box until it is found again or cleared.
    /// </summary>
    public static string ResourceObjectStringField<T>(string mainLabel, string resourceLocationValue, string rawFieldLabel = "-> Resource location", bool allowWhileRunning = false) where T : Object {
        if (_lastResourceObjects == null)
            _lastResourceObjects = new Dictionary<string, object>();

        if (resourceLocationValue == null)
            resourceLocationValue = "";

        if (Application.isPlaying & !allowWhileRunning) {
            // During gameplay, don't mess with resources, just show the raw string
            resourceLocationValue = EditorGUILayout.TextField(mainLabel, resourceLocationValue);
        }
        else {
            T resourceObject;

            if (_lastResourceObjects.ContainsKey(resourceLocationValue)) {
                resourceObject = _lastResourceObjects[resourceLocationValue] as T;
            }
            // If the resource location has changed, then update our cache
            else {
                //Debug.Log("New location -- updating entry");
                resourceObject = Resources.Load<T>(resourceLocationValue);
                //Debug.Log("Now storing - " + (T)_lastResourceObjects[resourceLocationValue]);

                // Make sure our cache has the context key
                if (resourceObject != null)
                    if (!_lastResourceObjects.ContainsKey(resourceLocationValue))
                        _lastResourceObjects.Add(resourceLocationValue, resourceObject);
            }

            // Object field to drag the resource convieniently -- not actually linked to any data

            T newObject;

            if (mainLabel != null) {
                newObject = EditorGUILayout.ObjectField(new GUIContent(mainLabel, "\"" + resourceLocationValue + "\""), resourceObject, typeof(T), false) as T;
            }
            else {
                newObject = EditorGUILayout.ObjectField(resourceObject, typeof(T), false) as T;
            }

            // If the user dropped something new
            if (newObject != resourceObject) {
                if (newObject == null) {
                    resourceLocationValue = "";
                }
                else {
                    string assetPath = AssetDatabase.GetAssetPath(newObject);
                    //Debug.Log("Asset path " + assetPath);
                    if (!string.IsNullOrEmpty(assetPath)) {
                        int resourceIndex = assetPath.IndexOf("/Resources/", StringComparison.Ordinal);
                        //Debug.Log("Resource substring index " + resourceIndex);
                        if (resourceIndex != -1) {
                            resourceLocationValue = assetPath.Substring(resourceIndex + 11);
                            int extensionIndex = resourceLocationValue.LastIndexOf('.');
                            //Debug.Log("Extension substring start index " + extensionIndex);
                            if (extensionIndex != -1) // no extension? hmmm
                            {
                                resourceLocationValue = resourceLocationValue.Substring(0, extensionIndex); // trim extension
                            }
                            //Debug.Log("New location " + resourceLocationValue);
                        }
                        else {
                            Debug.LogError("The asset/prefab you dropped is not within a \"Resources/\" folder (\"" + assetPath + "\")!");
                        }
                    }
                    else {
                        Debug.LogError("The asset/prefab you dropped does not seem to return a valid location!!");
                    }
                }
            }

            // Raw location string: if it is not found -- let the user fix the issue
            if (resourceObject == null && resourceLocationValue != "")
                resourceLocationValue = EditorGUILayout.TextField(rawFieldLabel, resourceLocationValue);
        }

        return resourceLocationValue;
    }

    /// <summary>
    /// Like Unity's field but without the label
    /// </summary>
    public static Vector2 Vector2Field(Vector2 value) {
        GUILayout.Label("X", GUILayout.Width(13));
        float x = EditorGUILayout.FloatField(value.x, GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
        GUILayout.Label("Y", GUILayout.Width(13));
        float y = EditorGUILayout.FloatField(value.y, GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
        return new Vector2(x, y);
    }

    /// <summary>
    /// Like Unity's field but without the label
    /// </summary>
    public static Vector3 Vector3Field(Vector3 value) {
        GUILayout.Label("X", GUILayout.Width(13));
        float x = EditorGUILayout.FloatField(value.x, GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
        GUILayout.Label("Y", GUILayout.Width(13));
        float y = EditorGUILayout.FloatField(value.y, GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
        GUILayout.Label("Z", GUILayout.Width(13));
        float z = EditorGUILayout.FloatField(value.z, GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
        return new Vector3(x, y, z);
    }

    public static int ClippedScrollView<T>(List<T> list, int position, Action<T, int> drawCallback) {
        if (list == null)
            return 0;

        if (list.Count == 0) // inline?
        {
            GUILayout.Label("No entries");
            return 0;
        }

        position = Mathf.Min(list.Count - 1, Mathf.Max(0, position));

        int count = Mathf.Min(10, list.Count);


        EditorGUILayout.BeginHorizontal();

        position = Mathf.FloorToInt(GUILayout.VerticalScrollbar(position, count, 0, list.Count, GUILayout.Height(200)));

        // Note that the scrollbar has to be on the right side
        // If not, any content layout changes in the main area will cause it to lose focus and stop dragging/scrolling

        EditorGUILayout.BeginVertical(GUILayout.MinHeight(200));


        //Debug.Log("Drawing " + count + " items from " + position + " to " + (position + count) + " (total = " + list.Count + ")");

        for (int i = position; i < position + count; i++) {
            if (i >= 0 && i < list.Count) {
                drawCallback(list[i], i);
            }
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        return position;
    }

    //public static void SelectChildScriptButton<T>(Rect position, Transform parent, Action<T> callback, string tooltip = "Select child script") where T : MonoBehaviour {
    //    if (GUI.Button(position, new GUIContent(DropdownTexture, tooltip), IconButtonStyle)) {
    //        DoSelectChildScriptButton(parent, callback);
    //    }
    //}

    //public static void SelectChildScriptButton<T>(Transform parent, Action<T> callback, string tooltip = "Select child script", float width = 20, float height = 16) where T : MonoBehaviour {
    //    if (GUILayout.Button(new GUIContent(DropdownTexture, tooltip), IconButtonStyle, GUILayout.Width(width), GUILayout.Height(height))) {
    //        DoSelectChildScriptButton(parent, callback);
    //    }
    //}

    //private static void DoSelectChildScriptButton<T>(Transform parent, Action<T> callback) where T : MonoBehaviour {
    //    GUI.FocusControl(null);
    //    GenericMenu outputMenu = new GenericMenu();

    //    List<T> children = GetChildren<T>(parent);

    //    if (children.Count > 0) {
    //        for (int i = 0; i < children.Count; i++) {
    //            string title;

    //            if (children[i].GetType() == typeof(TweenGroup))
    //                title = i + ": " + children[i].gameObject.name + " - \"" + ((TweenGroup)(object)children[i]).friendlyName + "\"";
    //            else if (children[i].GetType().IsSubclassOf(typeof(Tween)))
    //                title = i + ": " + children[i].gameObject.name + " - \"" + ((Tween)(object)children[i]).friendlyName + "\"";
    //            else
    //                title = i + ": " + children[i].gameObject.name + " (" + children[i].gameObject + ")";

    //            outputMenu.AddItem(new GUIContent(title), false, SelectChildScriptCallback<T>, new SelectChildScriptData<T>(callback, children[i]));
    //        }
    //    }
    //    else {
    //        outputMenu.AddItem(new GUIContent("-- No child scripts found --"), false, null, null);
    //    }

    //    outputMenu.ShowAsContext();
    //}

    private class SelectChildScriptData<T> where T : MonoBehaviour {
        public readonly Action<T> callback;
        public readonly T value;

        public SelectChildScriptData(Action<T> callback, T value) {
            this.callback = callback;
            this.value = value;
        }
    }

    private static void SelectChildScriptCallback<T>(object userData) where T : MonoBehaviour {
        SelectChildScriptData<T> data = userData as SelectChildScriptData<T>;
        if (data == null)
            return;

        data.callback(data.value);
    }

    public static List<T> GetChildren<T>(Transform parent) where T : Component {
        List<T> list = parent.GetComponents<T>().ToList();

        foreach (Transform child in parent) {
            list.AddRange(GetChildren<T>(child));
        }

        return list;
    }

    /// <summary> 
    /// Draws a Unity object field for the given component type, but allows input and returns output as the owner game object.
    /// For example, if the source data is a generic object that could have different types, but an editor restricts it based on other selections.
    /// </summary>
    public static GameObject ComponentGameObjectField<T>(GameObject gameObject, bool allowSceneObject) where T : Component {
        T component = EditorGUILayout.ObjectField(gameObject != null ? gameObject.GetComponent<T>() : null, typeof(T), allowSceneObject) as T;
        return component != null ? component.gameObject : null;
    }

    public static string LayerMaskMaskToString(LayerMask original, string delimiter = ", ") {
        return string.Join(delimiter, LayerMaskMaskToNames(original));
    }

    private static string[] LayerMaskMaskToNames(LayerMask original) {
        var output = new List<string>();

        for (int i = 0; i < 32; ++i) {
            int shifted = 1 << i;
            if ((original & shifted) == shifted) {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName)) {
                    output.Add(layerName);
                }
            }
        }
        return output.ToArray();
    }

    static List<string> layers;
    static string[] layerNames;
    static long lastUpdateTick;

    public static LayerMask LayerMaskField(string label, LayerMask selected) {
        if (layers == null || (System.DateTime.UtcNow.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) {
            lastUpdateTick = System.DateTime.UtcNow.Ticks;
            if (layers == null) {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else {
                layers.Clear();
            }

            int emptyLayers = 0;
            for (int i = 0; i < 32; i++) {
                string layerName = LayerMask.LayerToName(i);

                if (layerName != "") {
                    for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                    layers.Add(layerName);
                }
                else {
                    emptyLayers++;
                }
            }

            if (layerNames.Length != layers.Count) {
                layerNames = new string[layers.Count];
            }
            for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];
        }

        selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);

        return selected;
    }

    public static T FindAsset<T>() where T : Object {
        string[] findAssets = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        if (findAssets.Length == 1)
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(findAssets[0]));
        return null;
    }

    public static List<T> FindPrefabsWithComponent<T>(bool showProgressBar = true, string progressBarTitle = null, string progressBarDescription = null) where T : Object {
        progressBarTitle = progressBarTitle ?? "Searching";
        progressBarDescription = progressBarDescription ?? "Searching project for all " + typeof(T) + " prefabs...";

        List<T> foundPrefabs = null;

        if (showProgressBar)
            EditorUtility.DisplayProgressBar(progressBarTitle, progressBarDescription, 0f);

        string[] assetGuids = AssetDatabase.FindAssets("t:Prefab");
        //Debug.Log("Found " + assetGuids.Length + " assets.");

        if (assetGuids.Length > 0) {
            foundPrefabs = new List<T>();

            for (int i = 0; i < assetGuids.Length; i++) {
                if (showProgressBar)
                    EditorUtility.DisplayProgressBar(progressBarTitle, progressBarDescription, 0.1f + 0.9f * i / assetGuids.Length); // 10% for initial asset GUID search

                T component = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGuids[i])).GetComponent<T>();

                if (component != null)
                    foundPrefabs.Add(component);
            }
        }

        if (showProgressBar)
            EditorUtility.ClearProgressBar();

        return foundPrefabs;
    }

    public static void MarkDirty(Object target, string message = null) {
        if (target is ScriptableObject || IsPrefab(target)) {
            Undo.RegisterCompleteObjectUndo(target, message ?? "Changed \"" + target.name + "\" prefab");
            EditorUtility.SetDirty(target); // doesn't happen automatically, like in scene
        }
        else {
            Undo.RegisterCompleteObjectUndo(target, message ?? "Changed \"" + target.name + "\" in scene");
            //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); - automatic
        }
    }
}

public enum Restriction {
    None,
    NonZero,
    Positive,
    Negative,
    NonPositive,
    NonNegative,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    NotValue,
}

#endif