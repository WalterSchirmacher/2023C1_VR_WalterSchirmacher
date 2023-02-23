using UnityEngine;
using UnityEngine.UI;
using Microlight.AssetUtility;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ****************************************************************************************************
// Controller for health bar.
// Supports Image version and SpriteRenderer
// Requires visuals to be SpriteRenderers with DrawMode = Sliced
// or to be Image with ImageType = Filled
// Health bar uses width parameter to display HP amount in SpriteRenderers
// and fillAmount parameter to display HP amount in Images
// ****************************************************************************************************
namespace Microlight.HealthBar_NoTweens {
    enum HealthBarType { SpriteRenderer, Image }

    public class HealthBar_NoTweens : MonoBehaviour {
        [Header("Type")]
        [SerializeField] HealthBarType _barType;

        [Header("SpriteRenderer")]
        [SerializeField] SpriteRenderer _srBackground;
        [SerializeField] SpriteRenderer _srPrimaryBar;

        [Header("Image")]
        [SerializeField] Image _uiBackground;
        [SerializeField] Image _uiPrimaryBar;

        [Header("Colors")]
        [SerializeField] Color _barPrimaryColor = new Color(0f, 1f, 0f);
        [SerializeField] Color _barSecondaryColor = new Color(1f, 0f, 0f);

        [Header("General Settings")]
        [SerializeField] bool _adaptiveColor = true;   // Does health bar uses adaptive color based on current hp or single color for each bar

        // Variables
        bool _isInitalized = false;   // Safety system that gives out warning if health bar has not been initialized
        float _value = 1f;   // Current value of HP
        float _maxValue = 1f;   // Max value of HP
        float _fillAmount = 1f;   // How much bar is filled (%)
        Color _backgroundBarColor;   // Stores color of background bar

        // Contains every method that is exposed 
        #region Public Methods
        /// <summary>
        /// Initializes health bar with max value
        /// </summary>
        /// <param name="maxValue">Max value</param>
        public void Initialize(float maxValue) {
            _isInitalized = true;
            _value = maxValue;
            _maxValue = maxValue;
            UpdateHealthBar(maxValue);
        }

        /// <summary>
        /// Updates max value of health bar
        /// </summary>
        /// <param name="maxValue">Max health value</param>
        public void SetMaxHealth(float maxValue) {
            _maxValue = maxValue;
            UpdateHealthBar(maxValue);
        }

        /// <summary>
        /// Updates health bar.
        /// </summary>
        /// <param name="value">Value of HP</param>
        public void UpdateHealthBar(float value) {
            if(!_isInitalized) Debug.LogWarning("Health bar has not been initalized.");   // Warn if bar has not been initialized        
            _value = Mathf.Clamp(value, 0f, _maxValue);   // Set values for health bar, without going over max or below min
            _fillAmount = _value / _maxValue;   // At which % does bar needs to be with new value

            SetBarValue();
        }

        /// <summary>
        /// Fades health bar in or out
        /// </summary>
        /// <param name="fadeIn">True if fades in, False if fades out</param>
        public void FadeBar(bool fadeIn) {
            if(!_isInitalized) Debug.LogWarning("Health bar has not been initalized.");
            float backgroundFadeTo = fadeIn ? _backgroundBarColor.a : 0f;
            float barFadeTo = fadeIn ? 1f : 0f;

            if(_barType == HealthBarType.SpriteRenderer) {   // SpriteRenderer
                SetAlpha(_srBackground, backgroundFadeTo);
                SetAlpha(_srPrimaryBar, barFadeTo);
            }
            else {   // UI
                SetAlpha(_uiBackground, backgroundFadeTo);
                SetAlpha(_uiPrimaryBar, barFadeTo);
            }
        }
        #endregion

        // At start sort some values
        private void Awake() {
            // Store background bar colors and check if all SpriteRenderers/Images are referenced
            if(_barType == HealthBarType.SpriteRenderer) {   // SpriteRenderer
                if(_srBackground != null) _backgroundBarColor = _srBackground.color;   // Store background bar color
                else Debug.LogWarning("SpriteRenderer for Background Bar missing!");
                if(_srPrimaryBar == null) Debug.LogWarning("SpriteRenderer for Bar missing!");
            }
            else {
                if(_uiBackground != null) _backgroundBarColor = _uiBackground.color;   // Store background bar color
                else Debug.LogWarning("Image for Background Bar missing!");
                if(_uiPrimaryBar == null) Debug.LogWarning("Image for Bar missing!");
            }

            SetBarsColor();   // Sets appropriate color for each bar
        }

        // Sets primary bar based on values
        void SetBarValue() {
            if(_barType == HealthBarType.SpriteRenderer) _srPrimaryBar.size = new Vector2(_fillAmount, 1f);   // Sets bar value
            else _uiPrimaryBar.fillAmount = _fillAmount;
            SetBarsColor();
        }

        // Updates bars colors based on settings
        void SetBarsColor() {
            if(_adaptiveColor) {   // If using adaptive colors
                                   // Primary bar
                if(_barType == HealthBarType.SpriteRenderer) _srPrimaryBar.color = Color.Lerp(_barSecondaryColor, _barPrimaryColor, _srPrimaryBar.size.x);
                else _uiPrimaryBar.color = Color.Lerp(_barSecondaryColor, _barPrimaryColor, _uiPrimaryBar.fillAmount);
            }
            else {
                // Primary bar
                if(_barType == HealthBarType.SpriteRenderer) _srPrimaryBar.color = _barPrimaryColor;
                else _uiPrimaryBar.color = _barPrimaryColor;
            }
        }

        // Sets alpha value of color
        void SetAlpha(Image image, float value) {
            Color color = image.color;
            color.a = value;
            image.color = color;
        }
        void SetAlpha(SpriteRenderer renderer, float value) {
            Color color = renderer.color;
            color.a = value;
            renderer.color = color;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Microlight/SpriteRenderer Health Bar (NoTweens)")]
        public static void AddSpriteRendererHealthBar() {
            // Get prefab
            GameObject go = AssetUtilities.GetPrefab("SpriteRendererHP(NoTweens)");
            if(go == null) return;

            go = Instantiate(go);   // Instantiate
            go.name = "HealthBar";   // Change name
            if(Selection.activeGameObject != null) {   // Make child if some object is selected
                go.transform.SetParent(Selection.activeGameObject.transform);
            }
        }

        [MenuItem("GameObject/Microlight/UI Image Health Bar (NoTweens)")]
        public static void AddImageHealthBar() {
            // Get prefab
            GameObject go = AssetUtilities.GetPrefab("UIImageHP(NoTweens)");
            if(go == null) return;

            go = Instantiate(go);   // Instantiate
            go.name = "HealthBar";   // Change name
            if(Selection.activeGameObject != null) {   // Make child if some object is selected
                go.transform.SetParent(Selection.activeGameObject.transform, false);
            }
        }
#endif
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor for HealthBar. Used only for editor
    // ****************************************************************************************************
    [CustomEditor(typeof(HealthBar_NoTweens))]
    public class HealthBar_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Store serialized properties
            SerializedProperty barType = serializedObject.FindProperty("_barType");
            SerializedProperty adaptiveColor = serializedObject.FindProperty("_adaptiveColor");

            EditorGUILayout.PropertyField(barType);   // Select type of health bar (SpriteRenderer or Image)

            // References to SpriteRenderers and UI Images
            switch((HealthBarType)barType.enumValueIndex) {
                case HealthBarType.SpriteRenderer:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_srBackground"), new GUIContent("SpriteRenderer Background"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_srPrimaryBar"), new GUIContent("SpriteRenderer Bar"));
                    break;
                case HealthBarType.Image:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_uiBackground"), new GUIContent("Image Background"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_uiPrimaryBar"), new GUIContent("Image Bar"));
                    break;
                default: break;
            }

            // Settings
            EditorGUILayout.PropertyField(adaptiveColor, new GUIContent("Adaptive Color",
                "With adaptive color lerps between full and empty color based on fill amount. Without adaptive color bar has same color whole time."));

            // Colors
            // Primary
            if(adaptiveColor.boolValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("_barPrimaryColor"), new GUIContent("Full Color", "Color of the bar when full."));
            else EditorGUILayout.PropertyField(serializedObject.FindProperty("_barPrimaryColor"), new GUIContent("Bar Color", "Color of the health bar."));
            // Secondary
            if(adaptiveColor.boolValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("_barSecondaryColor"), new GUIContent("Empty Color", "Color of the bar when empty."));

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
#endif
    #endregion
}