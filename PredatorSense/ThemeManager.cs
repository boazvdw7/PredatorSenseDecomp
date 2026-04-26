using System.Windows.Media;
using TsDotNetLib;

namespace PredatorSense
{
	internal static class ThemeManager
	{
		private const string AdvanceSettingsPath = "SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings";

		private const string ThemeModeValueName = "ThemeMode";

		public static bool IsDarkModeEnabled()
		{
			return Registry.CheckLM(AdvanceSettingsPath, ThemeModeValueName, 0U) == 1;
		}

		public static void SetDarkModeEnabled(bool enabled)
		{
			Registry.SetValueLM(AdvanceSettingsPath, ThemeModeValueName, enabled ? 1U : 0U);
		}

		public static void ApplyThemeResources(bool darkMode)
		{
			if (Startup.styled == null)
			{
				return;
			}
			SetBrush("UiWindowBackgroundBrush", darkMode ? "#1B1D21" : "#F3F3F3");
			SetBrush("UiWindowBorderBrush", darkMode ? "#30333A" : "#D0D0D0");
			SetBrush("UiPanelBackgroundBrush", darkMode ? "#242830" : "#FFFFFF");
			SetBrush("UiPanelBorderBrush", darkMode ? "#3A3F4B" : "#D0D0D0");
			SetBrush("UiTitleBarBackgroundBrush", darkMode ? "#20242B" : "#F3F4F5");
			SetBrush("UiTitleBarBorderBrush", darkMode ? "#353B45" : "#DADCE0");
			SetBrush("UiMainTextBrush", darkMode ? "#F1F3F5" : "#1F1F1F");
			SetBrush("UiMutedTextBrush", darkMode ? "#A9B0BC" : "#808080");
			SetBrush("UiSubtleTextBrush", darkMode ? "#9098A8" : "#666666");
			SetBrush("UiPopupBackgroundBrush", darkMode ? "#2A2F38" : "#FFFFFF");
			SetBrush("UiPopupBorderBrush", darkMode ? "#404754" : "#DDDDDD");
			SetBrush("UiButtonBackgroundBrush", darkMode ? "#2D333D" : "#F8F8F8");
			SetBrush("UiButtonBorderBrush", darkMode ? "#454D5B" : "#D0D0D0");
		}

		private static void SetBrush(string key, string hex)
		{
			Startup.styled[key] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
		}
	}
}
