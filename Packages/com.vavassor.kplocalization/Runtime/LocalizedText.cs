using UnityEngine;
using UnityEngine.UI;

namespace KPLocalization
{
    using UdonSharp;
    using VRC.Udon;

    /// <summary>
    /// Localization information for a UI text component.
    /// 
    /// <see cref="LocalizationManager" /> is responsible for most functionality.
    /// </summary>
    /// <remarks>
    /// Prefer using synchronization method "None". This isn't enforced with UdonBehaviourSyncMode
    /// because behaviours with other sync methods may need to be placed on the same GameObject.
    /// </remarks>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : UdonSharpBehaviour
    {
        /// <summary>
        /// A context for context-specific variants.
        /// <example>
        /// For example, the context could be gender for gender-specific translations.
        /// </example>
        /// </summary>
        [FieldChangeCallback(nameof(Context))]
        public string context;

        /// <summary>
        /// How many items are relevant for plural forms.
        /// </summary>
        [FieldChangeCallback(nameof(Count))]
        public int count;

        /// <summary>
        /// Keys for interpolation.
        /// <example>
        /// For example, "count" would be the key in the following translation.
        /// <code>
        /// {{count}} items
        /// </code>
        /// </example>
        /// </summary>
        public string[] interpolationKeys;

        /// <summary>
        /// Values for interpolation.
        /// <example>
        /// For example, suppose we had the following translation.
        /// <code>
        /// {{count}} items
        /// </code>
        /// A value of 100 would be interpolated into the text "100 items".
        /// </example>
        /// </summary>
        [FieldChangeCallback(nameof(InterpolationValues))]
        public string[] interpolationValues;

        public string key;

        /// <summary>
        /// The localization manager managing this text.
        /// </summary>
        [HideInInspector]
        public LocalizationManager localizationManager;

        /// <summary>
        /// Convert all text to uppercase.
        /// 
        /// Similar to the CSS property text-transform: uppercase.
        /// </summary>
        [FieldChangeCallback(nameof(ShouldTransformUppercase))]
        public bool shouldTransformUppercase;

        public bool shouldUseCount;

        [FieldChangeCallback(nameof(Text)), HideInInspector]
        public string text;

        public string Context
        {
            set
            {
                context = value;
                UpdateText();
            }
            get => context;
        }

        public int Count
        {
            set
            {
                count = value;
                UpdateText();
            }
            get => count;
        }

        public string[] InterpolationValues
        {
            set
            {
                interpolationValues = value;
                UpdateText();
            }
            get => interpolationValues;
        }

        public bool ShouldTransformUppercase
        {
            set
            {
                shouldTransformUppercase = value;
                UpdateText();
            }
            get => shouldTransformUppercase;
        }

        public string Text
        {
            set
            {
                text = value;
                textComponent.text = text;
            }
            get => text;
        }

        private Text textComponent;

        public void OnLocalizationManagerStart()
        {
            textComponent = GetComponent<Text>();
        }

        private void UpdateText()
        {
            if (localizationManager != null)
            {
                localizationManager.UpdateLocalizedText(this);
            }
        }
    }
}