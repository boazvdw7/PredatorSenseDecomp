using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using TsDotNetLib;

namespace PredatorSense
{
	// Token: 0x0200005D RID: 93
	public class HotkeyProfileXML
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x00022A28 File Offset: 0x00020C28
		public HotkeyProfileXML(string file_path)
		{
			this.doc = new XmlDocument();
			this.doc.Load(file_path + "\\Main.xml");
			XmlNode xmlNode = this.doc.SelectSingleNode("ROOT");
			XmlElement xmlElement = (XmlElement)xmlNode;
			this.root_name = xmlElement.GetAttribute("name");
			for (int i = 1; i <= 3; i++)
			{
				this.group_content.Add(new HotkeyProfileXML.GroupContent(this.doc, "ROOT/Group" + i.ToString()));
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00022AD0 File Offset: 0x00020CD0
		public static void ResetDefaultHotkeyProfile(string profile_name)
		{
			try
			{
				string text = CommonFunction.hotkey_profile_path + profile_name;
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
					XmlElement xmlElement = xmlDocument.CreateElement("ROOT");
					xmlElement.SetAttribute("name", profile_name);
					xmlDocument.AppendChild(xmlElement);
					for (int i = 1; i <= 3; i++)
					{
						XmlElement xmlElement2 = xmlDocument.CreateElement("Group" + i.ToString());
						xmlElement2.SetAttribute("name", Startup.langRd["MUI_Group"].ToString() + i.ToString());
						if (i == 1)
						{
							xmlElement2.SetAttribute("color", "#0096B4");
						}
						else if (i == 2)
						{
							xmlElement2.SetAttribute("color", "#00FF00");
						}
						else if (i == 3)
						{
							xmlElement2.SetAttribute("color", "#FF0000");
						}
						xmlElement.AppendChild(xmlElement2);
						for (int j = 1; j <= 5; j++)
						{
							XmlElement xmlElement3 = xmlDocument.CreateElement("KEY" + j.ToString());
							xmlElement3.SetAttribute("userDefineName", null);
							xmlElement3.SetAttribute("type", 6.ToString());
							xmlElement3.SetAttribute("option", "0");
							xmlElement2.AppendChild(xmlElement3);
						}
					}
					xmlDocument.Save(text + "\\Main.xml");
					Directory.CreateDirectory(text + "\\MacroKeyPool");
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00022C88 File Offset: 0x00020E88
		public static void ProfileChangeAction(string profile_name)
		{
			Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment", "HotkeyProfile", profile_name);
			HotkeyProfileXML hotkeyProfileXML = new HotkeyProfileXML(CommonFunction.hotkey_profile_path + profile_name);
			for (int i = 1; i <= 3; i++)
			{
				Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + i.ToString(), "color", hotkeyProfileXML.group_content[i - 1].group_color);
				Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + i.ToString(), "name", hotkeyProfileXML.group_content[i - 1].group_name);
				for (int j = 1; j <= 5; j++)
				{
					Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + i.ToString(), "type" + j.ToString(), (uint)hotkeyProfileXML.group_content[i - 1].key_content[j - 1].type);
					Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + i.ToString(), "option" + j.ToString(), (uint)hotkeyProfileXML.group_content[i - 1].key_content[j - 1].option);
					Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + i.ToString(), "userDefineName" + j.ToString(), hotkeyProfileXML.group_content[i - 1].key_content[j - 1].userDefineName);
				}
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00022E28 File Offset: 0x00021028
		public static void SaveHotkeyGroupColor(string profile_name, int group_index, Color color)
		{
			group_index++;
			string text = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("ROOT/Group" + group_index.ToString());
			if (xmlNode == null)
			{
				return;
			}
			XmlElement xmlElement = (XmlElement)xmlNode;
			XmlAttribute attributeNode = xmlElement.GetAttributeNode("color");
			attributeNode.Value = text;
			Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + group_index, "color", text);
			ThreadStart threadStart = delegate
			{
				CommonFunction.set_group_wmi_color(group_index, color);
			};
			new Thread(threadStart).Start();
			try
			{
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
			catch (Exception)
			{
				Task<int> task = CommonFunction.delete_file(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
				int result = task.Result;
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00022F90 File Offset: 0x00021190
		public static void SaveHotkeyKeyContent(string profile_name, HotkeyProfileXML.KeyContent key_content)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			XmlNode xmlNode = xmlDocument.SelectSingleNode(string.Concat(new object[]
			{
				"ROOT/Group",
				key_content.GroupIndex.ToString(),
				"/KEY",
				key_content.KeyIndex
			}));
			if (xmlNode == null)
			{
				return;
			}
			XmlElement xmlElement = (XmlElement)xmlNode;
			XmlAttribute xmlAttribute = xmlElement.GetAttributeNode("type");
			xmlAttribute.Value = key_content.type.ToString();
			xmlAttribute = xmlElement.GetAttributeNode("option");
			xmlAttribute.Value = key_content.option.ToString();
			xmlAttribute = xmlElement.GetAttributeNode("userDefineName");
			xmlAttribute.Value = key_content.userDefineName.ToString();
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + key_content.GroupIndex.ToString(), "type" + key_content.KeyIndex.ToString(), (uint)key_content.type);
			Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + key_content.GroupIndex.ToString(), "option" + key_content.KeyIndex.ToString(), (uint)key_content.option);
			Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + key_content.GroupIndex.ToString(), "userDefineName" + key_content.KeyIndex.ToString(), key_content.userDefineName);
			try
			{
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
			catch (Exception)
			{
				Task<int> task = CommonFunction.delete_file(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
				int result = task.Result;
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00023160 File Offset: 0x00021360
		public static void SaveHotkeyKeyContents(string profile_name, List<HotkeyProfileXML.KeyContent> key_content_list)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			foreach (HotkeyProfileXML.KeyContent keyContent in key_content_list)
			{
				XmlNode xmlNode = xmlDocument.SelectSingleNode(string.Concat(new object[]
				{
					"ROOT/Group",
					keyContent.GroupIndex.ToString(),
					"/KEY",
					keyContent.KeyIndex
				}));
				if (xmlNode == null)
				{
					return;
				}
				XmlElement xmlElement = (XmlElement)xmlNode;
				XmlAttribute xmlAttribute = xmlElement.GetAttributeNode("type");
				xmlAttribute.Value = keyContent.type.ToString();
				xmlAttribute = xmlElement.GetAttributeNode("option");
				xmlAttribute.Value = keyContent.option.ToString();
				xmlAttribute = xmlElement.GetAttributeNode("userDefineName");
				xmlAttribute.Value = keyContent.userDefineName.ToString();
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + keyContent.GroupIndex.ToString(), "type" + keyContent.KeyIndex.ToString(), (uint)keyContent.type);
				Registry.SetValueLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + keyContent.GroupIndex.ToString(), "option" + keyContent.KeyIndex.ToString(), (uint)keyContent.option);
				Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + keyContent.GroupIndex.ToString(), "userDefineName" + keyContent.KeyIndex.ToString(), keyContent.userDefineName);
			}
			try
			{
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
			catch (Exception)
			{
				Task<int> task = CommonFunction.delete_file(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
				int result = task.Result;
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00023390 File Offset: 0x00021590
		public static void SaveHotkeyGroupName(string profile_name, int group_index, string new_group_name)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("ROOT/Group" + group_index.ToString());
			if (xmlNode == null)
			{
				return;
			}
			XmlElement xmlElement = (XmlElement)xmlNode;
			XmlAttribute attributeNode = xmlElement.GetAttributeNode("name");
			attributeNode.Value = new_group_name;
			Registry.SetStringLM("SOFTWARE\\OEM\\PredatorSense\\KeyAssignment\\Group" + group_index, "name", new_group_name);
			try
			{
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
			catch (Exception)
			{
				Task<int> task = CommonFunction.delete_file(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
				int result = task.Result;
				xmlDocument.Save(CommonFunction.hotkey_profile_path + profile_name + "\\Main.xml");
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0002346C File Offset: 0x0002166C
		public static void ResetDefaultMacroProfile(string profile_name, string macro_name)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			XmlElement xmlElement = xmlDocument.CreateElement("ROOT");
			xmlElement.SetAttribute("name", "");
			xmlDocument.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument.CreateElement("DATA");
			xmlElement2.SetAttribute("type", "macro");
			xmlElement.AppendChild(xmlElement2);
			XmlElement xmlElement3 = xmlDocument.CreateElement("KEY");
			xmlElement.AppendChild(xmlElement3);
			xmlDocument.Save(CommonFunction.macro_profile_path.Replace(CommonFunction.macro_profile_path_replace_name, profile_name) + macro_name + ".xml");
		}

		// Token: 0x04000401 RID: 1025
		public string root_name = "";

		// Token: 0x04000402 RID: 1026
		private XmlDocument doc;

		// Token: 0x04000403 RID: 1027
		public List<HotkeyProfileXML.GroupContent> group_content = new List<HotkeyProfileXML.GroupContent>();

		// Token: 0x0200005E RID: 94
		public class GroupContent
		{
			// Token: 0x06000301 RID: 769 RVA: 0x00023514 File Offset: 0x00021714
			public GroupContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.group_name = xmlElement.GetAttribute("name");
				this.group_color = xmlElement.GetAttribute("color");
				for (int i = 1; i <= 5; i++)
				{
					this.key_content.Add(new HotkeyProfileXML.KeyContent(doc, node_name + "/KEY" + i.ToString()));
				}
			}

			// Token: 0x04000404 RID: 1028
			public string group_name = "";

			// Token: 0x04000405 RID: 1029
			public string group_color = "";

			// Token: 0x04000406 RID: 1030
			public List<HotkeyProfileXML.KeyContent> key_content = new List<HotkeyProfileXML.KeyContent>();
		}

		// Token: 0x0200005F RID: 95
		public class KeyContent
		{
			// Token: 0x06000302 RID: 770 RVA: 0x000235A8 File Offset: 0x000217A8
			public KeyContent()
			{
			}

			// Token: 0x06000303 RID: 771 RVA: 0x000235BC File Offset: 0x000217BC
			public KeyContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.userDefineName = xmlElement.GetAttribute("userDefineName");
				this.type = Convert.ToInt32(xmlElement.GetAttribute("type"));
				this.option = Convert.ToInt32(xmlElement.GetAttribute("option"));
			}

			// Token: 0x04000407 RID: 1031
			public string userDefineName = "";

			// Token: 0x04000408 RID: 1032
			public int type;

			// Token: 0x04000409 RID: 1033
			public int option;

			// Token: 0x0400040A RID: 1034
			public int KeyIndex;

			// Token: 0x0400040B RID: 1035
			public int GroupIndex;
		}
	}
}
