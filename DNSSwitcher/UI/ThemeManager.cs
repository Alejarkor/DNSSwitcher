using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DNSSwitcher.Properties;
using Newtonsoft.Json;

namespace DNSSwitcher.UI
{
    /// <summary>
    /// Class to manage app themes.
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Current app theme.
        /// </summary>
        public static Theme CurrentTheme
        {
            get => currentTheme;
            set
            {
                var themeJson = JsonConvert.SerializeObject(value.SerializableTheme);
                File.WriteAllText(ThemeFile, themeJson);
                currentTheme = value;
            }
        }

        /// <summary>
        /// Backfield for CurrentTheme.
        /// </summary>
        private static Theme currentTheme;

        /// <summary>
        /// Available themes.
        /// </summary>
        public static List<Theme> Themes
        {
            get
            {
                if (themes == null)
                    LoadThemes();
                return themes;
            }
        }

        /// <summary>
        /// Backfield for Themes.
        /// </summary>
        private static List<Theme> themes;

        /// <summary>
        /// Theme file.
        /// </summary>
        private const string ThemeFile = "theme.json";

        /// <summary>
        /// Load the themes the app has.
        /// </summary>
        private static void LoadThemes() =>
            themes = new List<Theme>
            {
                new Theme("Fruits", Resources.pera_Yxk_icon, Resources.limon_and_icon),
                new Theme("Emojis", Resources.happy2_GEs_icon, Resources.sad_lQk_icon),
                new Theme("Patri", Resources.panda_Patri, Resources.limon_Patri)
                
            };

        /// <summary>
        /// Initializes the theme manager.
        /// </summary>
        public static void Initialize() =>
            CurrentTheme = File.Exists(ThemeFile)
                ? GetThemeFromSerializable(
                    JsonConvert.DeserializeObject<SerializableTheme>(File.ReadAllText(ThemeFile)))
                : Themes[0];

        /// <summary>
        /// Change the theme.
        /// </summary>
        /// <param name="theme">The theme name.</param>
        public static void ChangeTheme(string theme) =>
            CurrentTheme = GetThemeFromSerializable(new SerializableTheme(theme));

        /// <summary>
        /// Gets the theme corresponding to the given serializable theme.
        /// </summary>
        /// <param name="serializableTheme">The serializable theme.</param>
        /// <returns>The full theme.</returns>
        private static Theme GetThemeFromSerializable(SerializableTheme serializableTheme)
        {
            foreach (var theme in Themes)
                if (theme.Name == serializableTheme.Name)
                    return theme;

            return Themes[0];
        }
    }

    /// <summary>
    /// Class that represents an app theme.
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Name of the theme.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Icon for the public dns.
        /// </summary>
        public readonly Icon PublicDnsIcon;

        /// <summary>
        /// Icon for the default dns.
        /// </summary>
        public readonly Icon DefaultDnsIcon;

        /// <summary>
        /// Error icon.
        /// </summary>
        public readonly Icon ErrorIcon;

        /// <summary>
        /// Constructor for the theme.
        /// </summary>
        /// <param name="name">Name for the theme.</param>
        /// <param name="publicDnsIcon">Icon for the public dns.</param>
        /// <param name="defaultDnsIcon">Icon for the default dns.</param>
        public Theme(string name, Icon publicDnsIcon, Icon defaultDnsIcon)
        {
            Name = name;
            PublicDnsIcon = publicDnsIcon;
            DefaultDnsIcon = defaultDnsIcon;
            ErrorIcon = Resources.sad2_xZP_icon;
        }

        /// <summary>
        /// Serializable theme corresponding to this one.
        /// </summary>
        public SerializableTheme SerializableTheme => new SerializableTheme(Name);

        /// <summary>
        /// Constructor for the theme.
        /// </summary>
        /// <param name="name">Name for the theme.</param>
        /// <param name="publicDnsIcon">Icon for the public dns.</param>
        /// <param name="defaultDnsIcon">Icon for the default dns.</param>
        /// <param name="errorIcon">Error icon.</param>
        public Theme(string name, Icon publicDnsIcon, Icon defaultDnsIcon, Icon errorIcon)
        {
            Name = name;
            PublicDnsIcon = publicDnsIcon;
            DefaultDnsIcon = defaultDnsIcon;
            ErrorIcon = errorIcon;
        }
    }

    /// <summary>
    /// Serializable version of a theme.
    /// </summary>
    public class SerializableTheme
    {
        /// <summary>
        /// Theme name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Theme name.</param>
        public SerializableTheme(string name) => Name = name;
    }
}