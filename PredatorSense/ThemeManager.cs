using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using TsDotNetLib;

namespace PredatorSense
{
	internal static class ThemeManager
	{
		private const string AdvanceSettingsPath = "SOFTWARE\\OEM\\PredatorSense\\AdvanceSettings";
		private const string ThemeModeValueName = "ThemeMode";

		private static readonly Dictionary<string, BitmapSource> OriginalImageResources = new Dictionary<string, BitmapSource>(StringComparer.Ordinal);
		private static readonly Dictionary<string, BitmapSource> LightenedImageResources = new Dictionary<string, BitmapSource>(StringComparer.Ordinal);

		private static readonly string[] ImageKeysToLightenInDarkMode = new string[]
		{
			"btn_gfe",
			"Img_monitor_chartshadow",
			"Img_monitor_chart",
			"Img_fan",
			"Img_fan_position",
			"Img_oc_dashboard_normal",
			"Img_oc_dashboard_faster",
			"Img_oc_dashboard_turbo",
			"Img_oc_indicator"
		};

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
			SetBrush("UiPngOverlayBrush", darkMode ? "#22FFFFFF" : "#00FFFFFF");

			Startup.styled["UiPngImageEffect"] = darkMode
				? (object)new DropShadowEffect
				{
					Color = (Color)ColorConverter.ConvertFromString("#DCE2EA"),
					BlurRadius = 8.0,
					ShadowDepth = 0.0,
					Opacity = 0.2
				}
				: null;

			ApplyImageBrightnessResources(darkMode);
		}

		private static void SetBrush(string key, string hex)
		{
			Startup.styled[key] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
		}

		private static void ApplyImageBrightnessResources(bool darkMode)
		{
			foreach (string key in ImageKeysToLightenInDarkMode)
			{
				if (!Startup.styled.Contains(key))
				{
					continue;
				}

				if (!(Startup.styled[key] is BitmapSource currentBitmap))
				{
					continue;
				}

				if (!OriginalImageResources.ContainsKey(key))
				{
					BitmapSource original = currentBitmap;
					if (original.CanFreeze && !original.IsFrozen)
					{
						original.Freeze();
					}
					OriginalImageResources[key] = original;
				}

				if (darkMode)
				{
					if (!LightenedImageResources.TryGetValue(key, out BitmapSource lightened))
					{
						lightened = CreateLightenedBitmap(OriginalImageResources[key], 0.82);
						LightenedImageResources[key] = lightened;
					}
					Startup.styled[key] = lightened;
				}
				else
				{
					Startup.styled[key] = OriginalImageResources[key];
				}
			}
		}

		private static BitmapSource CreateLightenedBitmap(BitmapSource source, double strength)
		{
			if (source == null)
			{
				return null;
			}

			BitmapSource converted = source.Format == PixelFormats.Bgra32
				? source
				: new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);

			int width = converted.PixelWidth;
			int height = converted.PixelHeight;
			int stride = width * 4;
			byte[] pixels = new byte[stride * height];
			converted.CopyPixels(pixels, stride, 0);

			for (int i = 0; i < pixels.Length; i += 4)
			{
				byte alpha = pixels[i + 3];
				if (alpha == 0)
				{
					continue;
				}

				pixels[i] = LightenChannel(pixels[i], strength);
				pixels[i + 1] = LightenChannel(pixels[i + 1], strength);
				pixels[i + 2] = LightenChannel(pixels[i + 2], strength);
			}

			WriteableBitmap result = new WriteableBitmap(width, height, converted.DpiX, converted.DpiY, PixelFormats.Bgra32, null);
			result.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
			result.Freeze();
			return result;
		}

		private static byte LightenChannel(byte channel, double strength)
		{
			double value = channel + (255.0 - channel) * strength;
			if (value < 0.0)
			{
				value = 0.0;
			}
			if (value > 255.0)
			{
				value = 255.0;
			}
			return (byte)value;
		}
	}
}
