using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using GameEvent = Constants.GameEvent;
#if UNITY_EDITOR
using System.Text; // ToString() string builder stuff
#endif

namespace LL.Events {

    public class EventData {
        #region Fields and Properties

        /// <summary> The event (type) that originated (sent) this data package </summary>
        public GameEvent eventType;

        /// <summary> The main/default int data value, if any, sent with this event </summary>
        private int? _defaultInt;

        /// <summary> The main/default string data value, if any, sent with this event </summary>
        private string _defaultString;

        /// <summary> The main/default float data value, if any, sent with this event </summary>
        private float? _defaultFloat;

        /// <summary> The main/default bool data value, if any, sent with this event </summary>
        private bool? _defaultBool;

        /// <summary> The main/default Unity game object reference data value, if any, sent with this event </summary>
        private GameObject _defaultObj;

        /// <summary> The main/default Unity script reference data value, if any, sent with this event </summary>
        private MonoBehaviour _defaultMonoBehaviour;

        /// <summary> The main/default Unity Vector3 struct data value, if any, sent with this event </summary>
        private Vector3? _defaultVector;

        /// <summary> The extra/additional keyed int data values, if any, sent with this event </summary>
        private IDictionary<string, int> _intValues;

        /// <summary> The extra/additional keyed string data values, if any, sent with this event </summary>
        private IDictionary<string, string> _stringValues;

        /// <summary> The extra/additional keyed float data values, if any, sent with this event </summary>
        private IDictionary<string, float> _floatValues;

        /// <summary> The extra/additional keyed bool data values, if any, sent with this event </summary>
        private IDictionary<string, bool> _boolValues;

        /// <summary> The extra/additional keyed Unity game object reference data values, if any, sent with this event </summary>
        private IDictionary<string, GameObject> _gameObjectValues;

        /// <summary> The extra/additional keyed Unity script reference data values, if any, sent with this event </summary>
        private IDictionary<string, MonoBehaviour> _monoBehaviourValues;

        /// <summary> The extra/additional keyed Unity Vector3 struct data values, if any, sent with this event </summary>
        private IDictionary<string, Vector3> _vectors;

        /// <summary> The extra/additional keyed Unity Actions delegate data values, if any, sent with this event </summary>
        private IDictionary<string, Action> _actions;

        private IDictionary<string, Effect> _effects;

        private IDictionary<string, Ability> _abilities;

        #endregion


        #region Constructors

        public EventData() {
            __InitializeChecks();
        }

        public EventData(int value) {
            __InitializeChecks();
            _defaultInt = value;
        }

        public EventData(string value) {
            __InitializeChecks();
            _defaultString = value;
        }

        public EventData(float value) {
            __InitializeChecks();
            _defaultFloat = value;
        }

        public EventData(bool value) {
            __InitializeChecks();
            _defaultBool = value;
        }

        public EventData(GameObject value) {
            __InitializeChecks();
            _defaultObj = value;
        }

        public EventData(MonoBehaviour value) {
            __InitializeChecks();
            _defaultMonoBehaviour = value;
        }

        public EventData(Vector3 value) {
            __InitializeChecks();
            _defaultVector = value;
        }

        #endregion


        #region Adding Keyed Values


        public void AddAbility(string key, Ability value)
        {
            if (_abilities == null)
                _abilities = new Dictionary<string, Ability>();

            _abilities.Add(key, value);
        }

        public void AddInt(string key, int value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_intValues == null)
                _intValues = new Dictionary<string, int>();

            _intValues.Add(key, value);
        }

        public void AddString(string key, string value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_stringValues == null)
                _stringValues = new Dictionary<string, string>();

            _stringValues.Add(key, value);
        }

        public void AddFloat(string key, float value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_floatValues == null)
                _floatValues = new Dictionary<string, float>();

            _floatValues.Add(key, value);
        }

        public void AddBool(string key, bool value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_boolValues == null)
                _boolValues = new Dictionary<string, bool>();

            _boolValues.Add(key, value);
        }

        public void AddGameObject(string key, GameObject value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_gameObjectValues == null)
                _gameObjectValues = new Dictionary<string, GameObject>();

            _gameObjectValues.Add(key, value);
        }

        public void AddMonoBehaviour(string key, MonoBehaviour value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_monoBehaviourValues == null)
                _monoBehaviourValues = new Dictionary<string, MonoBehaviour>();

            _monoBehaviourValues.Add(key, value);
        }

        public void AddEffect(string key, Effect value)
        {
            __CheckWritable(key);

            if (_effects == null)
                _effects = new Dictionary<string, Effect>();

            _effects.Add(key, value);
        }

        public void AddAction(string key, Action value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_actions == null)
                _actions = new Dictionary<string, Action>();

            _actions.Add(key, value);
        }

        public void AddVector3(string key, Vector3 value) {
            // Debug-time verification that we can write the data at this time with the given key
            __CheckWritable(key);

            if (_vectors == null)
                _vectors = new Dictionary<string, Vector3>();

            _vectors.Add(key, value);
        }
        #endregion


        #region Getting Keyed Values

        public Ability GetAbility(string key)
        {
            Ability ability;
            if(_abilities == null || _abilities.TryGetValue(key, out ability) == false)
            {
                return null;
            }

            return ability;
        }

        public int GetInt(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            int i;
            if (_intValues == null || !_intValues.TryGetValue(key, out i)) {
                i = 0;
            }

            return i;
        }

        public string GetString(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            string s;
            if (_stringValues == null || !_stringValues.TryGetValue(key, out s)) {
                s = "";
            }

            return s;
        }

        public float GetFloat(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            float f;
            if (_floatValues == null || !_floatValues.TryGetValue(key, out f)) {
                f = 0f;
            }

            return f;
        }

        public bool GetBool(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            bool b;
            if (_boolValues == null || !_boolValues.TryGetValue(key, out b)) {
                b = false;
            }

            return b;
        }

        public GameObject GetGameObject(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            GameObject gameObject;
            if (_gameObjectValues == null || !_gameObjectValues.TryGetValue(key, out gameObject)) {
                gameObject = null;
            }

            return gameObject;
        }

        public MonoBehaviour GetMonoBehaviour(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            MonoBehaviour monoBehaviour;
            if (_monoBehaviourValues == null || !_monoBehaviourValues.TryGetValue(key, out monoBehaviour)) {
                return null;
            }

            return monoBehaviour;
        }

        public Effect GetEffect(string key)
        {
            __CheckReadable(key);

            Effect effect;
            if(_effects == null || _effects.TryGetValue(key, out effect) == false)
            {
                return null;
            }

            return effect;
        }

        public Action GetAction(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            Action action;
            if (_actions == null || !_actions.TryGetValue(key, out action)) {
                return null;
            }

            return action;
        }

        public Vector3 GetVector3(string key) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            Vector3 vector3;
            if (_vectors == null || !_vectors.TryGetValue(key, out vector3)) {
                vector3 = new Vector3();
            }

            return vector3;
        }

        public int GetIntOrDefault(string key, int defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            int i;
            if (_intValues == null || !_intValues.TryGetValue(key, out i)) {
                i = defaultValue;
            }

            return i;
        }

        public string GetStringOrDefault(string key, string defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            string s;
            if (_stringValues == null || !_stringValues.TryGetValue(key, out s)) {
                s = defaultValue;
            }

            return s;
        }

        public float GetFloatOrDefault(string key, float defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            float f;
            if (_floatValues == null || !_floatValues.TryGetValue(key, out f)) {
                f = defaultValue;
            }

            return f;
        }

        public bool GetBoolOrDefault(string key, bool defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            bool b;
            if (_boolValues == null || !_boolValues.TryGetValue(key, out b)) {
                b = defaultValue;
            }

            return b;
        }

        public GameObject GetGameObjectOrDefault(string key, GameObject defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            GameObject gameObject;
            if (_gameObjectValues == null || !_gameObjectValues.TryGetValue(key, out gameObject)) {
                gameObject = defaultValue;
            }

            return gameObject;
        }

        public MonoBehaviour GetMonoBehaviourOrDefault(string key, MonoBehaviour defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            MonoBehaviour monoBehaviour;
            if (_monoBehaviourValues == null || !_monoBehaviourValues.TryGetValue(key, out monoBehaviour)) {
                return defaultValue;
            }

            return monoBehaviour;
        }

        public Vector3 GetVector3OrDefault(string key, Vector3 defaultValue) {
            // Debug-time verification that we can read the data at this time with the given key
            __CheckReadable(key);

            Vector3 vector3;
            if (_vectors == null || !_vectors.TryGetValue(key, out vector3)) {
                vector3 = defaultValue;
            }

            return vector3;
        }



        #endregion


        #region Setting Keyless Values

        public void SetInt(int value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultInt = value;
        }

        public void SetString(string value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultString = value;
        }

        public void SetFloat(float value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultFloat = value;
        }

        public void SetBool(bool value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultBool = value;
        }

        public void SetGameObject(GameObject value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultObj = value;
        }

        public void SetMonoBehaviour(MonoBehaviour value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultMonoBehaviour = value;
        }

        public void SetVector(Vector3 value) {
            // Debug-time verification that we can write the data at this time
            __CheckWritable();

            _defaultVector = value;
        }

        #endregion


        #region Getting Keyless Values

        public int GetInt() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultInt.HasValue ? _defaultInt.Value : 0;
        }

        public string GetString() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultString ?? "";
        }

        public float GetFloat() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultFloat.HasValue ? _defaultFloat.Value : 0f;
        }

        public bool GetBool() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultBool.HasValue ? _defaultBool.Value : false;
        }

        public GameObject GetGameObject() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            if (_defaultObj != null) // Note: Unity ? and ?? are not the same as == or !=
                return _defaultObj;

            return null;
        }

        public MonoBehaviour GetMonoBehaviour() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultMonoBehaviour;
        }

        public Vector3 GetVector3() {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultVector.HasValue ? _defaultVector.Value : new Vector3();
        }

        public int GetIntOrDefault(int defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultInt.HasValue ? _defaultInt.Value : defaultValue;
        }

        public string GetStringOrDefault(string defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultString ?? defaultValue;
        }

        public float GetFloatOrDefault(float defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultFloat.HasValue ? _defaultFloat.Value : defaultValue;
        }

        public bool GetBoolOrDefault(bool defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultBool.HasValue ? _defaultBool.Value : defaultValue;
        }

        public GameObject GetGameObjectOrDefault(GameObject defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            if (_defaultObj != null) // Note: Unity ? and ?? are not the same as == or !=
                return _defaultObj;

            return defaultValue;
        }

        public MonoBehaviour GetMonoBehaviourOrDefault(MonoBehaviour defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            if (_defaultMonoBehaviour != null) // Note: Unity ? and ?? are not the same as == or !=
                return _defaultMonoBehaviour;

            return defaultValue;
        }

        public Vector3 GetVector3OrDefault(Vector3 defaultValue) {
            // Debug-time verification that we can read the data at this time
            __CheckReadable();

            return _defaultVector.HasValue ? _defaultVector.Value : defaultValue;
        }
        #endregion


        #region Debug

        [Conditional("UNITY_EDITOR")]
        private void __InitializeChecks() {
#if UNITY_EDITOR
            __keys = new List<string>();
            __frameCreated = Time.frameCount;
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void __CheckWritable(string key) {
#if UNITY_EDITOR
            __CheckKeyWritable(key);
            __CheckFrame();
            __CheckDelivered();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void __CheckWritable() {
#if UNITY_EDITOR
            __CheckFrame();
            __CheckDelivered();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void __CheckReadable(string key) {
#if UNITY_EDITOR
            __CheckFrame();
            __CheckKeyReadable(key);
            __CheckUnsent();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void __CheckReadable() {
#if UNITY_EDITOR
            __CheckFrame();
            __CheckUnsent();
#endif
        }

#if UNITY_EDITOR

        /// <summary> Debug-time check that a key is valid </summary>
        private void __CheckKeyReadable(string key) {
            if (key == null) {
                Debug.LogError("[EVENT DATA] Given key is null!");
                return;
            }
        }

        /// <summary> Debug-time check that a key is valid; plus records it for future checks </summary>
        private void __CheckKeyWritable(string key) {
            if (key == null) {
                Debug.LogError("[EVENT DATA] Given key is null!");
                return;
            }

            if (__keys.Contains(key)) {
                Debug.LogError("[EVENT DATA] EventData key \"" + key + "\" already exists. You cannot have multiple values assigned to a single key.");
                return;
            }

            if (!__keys.Contains(key))
                __keys.Add(key);
        }

        /// <summary> Debug-time check that we are written and accessed on the same frame as we are created </summary>
        private void __CheckFrame() {
            if (!__frameless && __frameCreated != Time.frameCount) {
                Debug.LogError("[EVENT DATA] EventData is being used out of sync (frame created: " + __frameCreated + ", current: " + Time.frameCount + "); " +
                                "make sure to only use it on the same frame as received and store the values otherwise. " +
                                "Copy the values from event data and don't store it other than on the frame received. " +
                                "Event system is not designed for any events to remain alive across multiple frames.");
            }
        }

        /// <summary> Debug-time check that we are not being written to after being sent </summary>
        private void __CheckDelivered() {
            if (!__frameless && __delivered) {
                Debug.LogError("[EVENT DATA] Data is being written to EventData after it has already been sent; " +
                                "make sure to only read the data and not write any when receiving the event. " +
                                "Event system is not designed for any events to change during delivery. " +
                                "Other scripts may use the data before or after and may receive invalid values.");
            }
        }

        /// <summary> Debug-time check that we are not being used without having been sent </summary>
        private void __CheckUnsent() {
            if (!__frameless && __frameCreated == Time.frameCount && !__delivered) {
                Debug.LogError("[EVENT DATA] Data is being read without it having been sent through the manager. " +
                               "Data should only be written before sending. " +
                               "Event data should not be created and used manually or passed to receivers directly; " +
                               "receiver methods should only be invoked by the manager and the data passed to it.");
            }
        }

        private List<string> __keys; // Optimized to not use key checks in release
        private int __frameCreated;
        public bool __frameless = false; // Default data is reusable, so it doesn't care about this
        public bool __delivered = false;

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("EventData [" + eventType + "] (");

            bool first = true;

            if (_defaultInt != null) { str.Append("int=" + _defaultInt); first = false; }
            if (_defaultString != null) { if (!first) str.Append(", "); str.Append("str=\"" + _defaultString + "\""); first = false; }
            if (_defaultFloat != null) { if (!first) str.Append(", "); str.Append("float=" + _defaultFloat); first = false; }
            if (_defaultBool != null) { if (!first) str.Append(", "); str.Append("bool=" + _defaultBool); first = false; }
            if (_defaultObj != null) { if (!first) str.Append(", "); str.Append("go={" + _defaultObj + "}"); first = false; }
            if (_defaultMonoBehaviour != null) { if (!first) str.Append(", "); str.Append("script={" + _defaultMonoBehaviour + "}"); first = false; }
            if (_defaultVector != null) { if (!first) str.Append(", "); str.Append("vector={" + _defaultVector + "}"); first = false; }

            if (_intValues != null) foreach (KeyValuePair<string, int> v in _intValues) { if (!first) str.Append(", "); str.Append("int[" + v.Key + "]=" + v.Value); first = false; }
            if (_stringValues != null) foreach (KeyValuePair<string, string> v in _stringValues) { if (!first) str.Append(", "); str.Append("str[" + v.Key + "]=\"" + v.Value + "\""); first = false; }
            if (_floatValues != null) foreach (KeyValuePair<string, float> v in _floatValues) { if (!first) str.Append(", "); str.Append("float[" + v.Key + "]=" + v.Value); first = false; }
            if (_boolValues != null) foreach (KeyValuePair<string, bool> v in _boolValues) { if (!first) str.Append(", "); str.Append("bool[" + v.Key + "]=" + v.Value); first = false; }
            if (_gameObjectValues != null) foreach (KeyValuePair<string, GameObject> v in _gameObjectValues) { if (!first) str.Append(", "); str.Append("go[" + v.Key + "]={" + v.Value + "}"); first = false; }
            if (_monoBehaviourValues != null) foreach (KeyValuePair<string, MonoBehaviour> v in _monoBehaviourValues) { if (!first) str.Append(", "); str.Append("script[" + v.Key + "]={" + v.Value + "}"); first = false; }
            if (_vectors != null) foreach (KeyValuePair<string, Vector3> v in _vectors) { if (!first) str.Append(", "); str.Append("vector[" + v.Key + "]={" + v.Value + "}"); first = false; }

            if (first) str.Append("-no data-");

            return str.Append(")").ToString();
        }
#endif
        #endregion
    }

}
