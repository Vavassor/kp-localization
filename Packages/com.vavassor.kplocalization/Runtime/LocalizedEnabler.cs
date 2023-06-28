using UnityEngine;

namespace KPLocalization
{
    using UdonSharp;

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LocalizedEnabler : UdonSharpBehaviour
    {
        /// <summary>
        /// The objects to show when the locale is preferred.
        /// </summary>
        public GameObject[] gameObjects;

        /// <summary>
        /// The locale to enable objects for, as an IETF language tag.
        /// </summary>
        public string locale;

        /// <summary>
        /// The localization manager managing this enabler.
        /// </summary>
        [HideInInspector]
        public LocalizationManager localizationManager;

        public void OnChangePreferredLocales()
        {
            var isPreferred = IsLocalePreferred();

            foreach (var gameObj in gameObjects)
            {
                gameObj.SetActive(isPreferred);
            }
        }

        private bool IsLocalePreferred()
        {
            if (localizationManager != null)
            {
                foreach (var preferredLocale in localizationManager.PreferredLocales)
                {
                    if (IsLocaleMatch(preferredLocale))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsLocaleMatch(string searchLocale)
        {
            var parts = searchLocale.Split('-');
            var languageSubtag = parts.Length > 0 ? parts[0] : searchLocale;
            return locale.Equals(languageSubtag) || (locale.IndexOf('-') >= 0 && locale.StartsWith(languageSubtag));
        }
    }
}
