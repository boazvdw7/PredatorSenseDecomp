using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace PredatorSense
{
	// Token: 0x02000060 RID: 96
	public class LightingProfileXML
	{
		// Token: 0x06000304 RID: 772 RVA: 0x00023628 File Offset: 0x00021828
		public LightingProfileXML(string file_path)
		{
			this.doc = new XmlDocument();
			this.doc.Load(file_path + "\\Main.xml");
			XmlNode xmlNode = this.doc.SelectSingleNode("ROOT");
			XmlElement xmlElement = (XmlElement)xmlNode;
			this.root_name = xmlElement.GetAttribute("name");
			this.key_group_content = new LightingProfileXML.KeyGroupContent(this.doc, "ROOT/Key");
			this.pattern_group_content = new LightingProfileXML.PatternGroupContent(this.doc, "ROOT/Pattern");
			this.zone_group_content = new LightingProfileXML.ZoneGroupContent(this.doc, "ROOT/Zone");
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000236D4 File Offset: 0x000218D4
		public static void SetLightingProfile_Key_status(string profile_name, int status)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Key");
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["status"].Value = status.ToString();
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0002376C File Offset: 0x0002196C
		public static void SetLightingProfile_Key(string profile_name, DataTable tag_color_datatable)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				for (int i = 0; i < tag_color_datatable.Rows.Count; i++)
				{
					try
					{
						string text2 = tag_color_datatable.Rows[i]["KeyTag"].ToString();
						XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Key/Tag" + text2);
						XmlNode xmlNode = xmlNodeList.Item(0);
						xmlNode.Attributes["color"].Value = tag_color_datatable.Rows[i]["KeyColor"].ToString();
					}
					catch (Exception)
					{
					}
				}
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0002386C File Offset: 0x00021A6C
		public static void SetLightingProfile_PatternActive(string profile_name, int selected, Color color)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Pattern");
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["selected"].Value = selected.ToString();
				xmlNode.Attributes["color"].Value = CommonFunction.StringFromColor(color);
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00023920 File Offset: 0x00021B20
		public static void SetLightingProfile_PatternProperty(string profile_name, int index, string speed, string direction)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Pattern/Pattern" + index.ToString());
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["speed"].Value = speed;
				xmlNode.Attributes["direction"].Value = direction;
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000239D4 File Offset: 0x00021BD4
		public static void SetLightingProfile_ZoneActive(string profile_name, bool effect, int speed)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Zone");
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["effect"].Value = Convert.ToInt32(effect).ToString();
				xmlNode.Attributes["speed"].Value = speed.ToString();
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00023A94 File Offset: 0x00021C94
		public static void SetLightingProfile_Zone(string profile_name, int index, bool status)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Zone/Zone" + index.ToString());
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["status"].Value = Convert.ToInt32(status).ToString();
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00023B40 File Offset: 0x00021D40
		public static void SetLightingProfile_Zone(string profile_name, int index, bool status, Color color)
		{
			string text = CommonFunction.lighting_profile_path + profile_name + "\\Main.xml";
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/ROOT/Zone/Zone" + index.ToString());
				XmlNode xmlNode = xmlNodeList.Item(0);
				xmlNode.Attributes["status"].Value = Convert.ToInt32(status).ToString();
				xmlNode.Attributes["color"].Value = CommonFunction.StringFromColor(color);
				try
				{
					xmlDocument.Save(text);
				}
				catch (Exception)
				{
					Task<int> task = CommonFunction.delete_file(text);
					int result = task.Result;
					xmlDocument.Save(text);
				}
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00023C08 File Offset: 0x00021E08
		public static void ResetDefaultLightProfile(string profile_name)
		{
			try
			{
				string text = CommonFunction.lighting_profile_path + profile_name;
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
					string text2 = "#0082D2";
					string text3 = "#008287";
					string text4 = "#00A087";
					string text5 = "#00B4B4";
					string text6 = "#00D282";
					string text7 = "5";
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
					XmlElement xmlElement = xmlDocument.CreateElement("ROOT");
					xmlElement.SetAttribute("name", profile_name);
					xmlDocument.AppendChild(xmlElement);
					XmlElement xmlElement2 = xmlDocument.CreateElement("Key");
					xmlElement2.SetAttribute("status", "0");
					xmlElement.AppendChild(xmlElement2);
					for (int i = 0; i < 125; i++)
					{
						XmlElement xmlElement3 = xmlDocument.CreateElement("Tag" + i.ToString());
						xmlElement3.SetAttribute("color", text2);
						xmlElement2.AppendChild(xmlElement3);
					}
					XmlElement xmlElement4 = xmlDocument.CreateElement("Pattern");
					xmlElement4.SetAttribute("selected", "0");
					xmlElement4.SetAttribute("color", text2);
					xmlElement.AppendChild(xmlElement4);
					for (int j = 0; j < 12; j++)
					{
						XmlElement xmlElement5 = xmlDocument.CreateElement("Pattern" + j.ToString());
						xmlElement5.SetAttribute("speed", text7);
						if (j == 6)
						{
							xmlElement5.SetAttribute("direction", "5");
						}
						else if (j == 7)
						{
							xmlElement5.SetAttribute("direction", "1");
						}
						else
						{
							xmlElement5.SetAttribute("direction", "0");
						}
						xmlElement4.AppendChild(xmlElement5);
					}
					XmlElement xmlElement6 = xmlDocument.CreateElement("Zone");
					xmlElement6.SetAttribute("effect", "0");
					xmlElement6.SetAttribute("speed", text7);
					xmlElement.AppendChild(xmlElement6);
					for (int k = 1; k <= 4; k++)
					{
						XmlElement xmlElement7 = xmlDocument.CreateElement("Zone" + k.ToString());
						xmlElement7.SetAttribute("status", "1");
						switch (k)
						{
						case 1:
							xmlElement7.SetAttribute("color", text3);
							break;
						case 2:
							xmlElement7.SetAttribute("color", text4);
							break;
						case 3:
							xmlElement7.SetAttribute("color", text5);
							break;
						case 4:
							xmlElement7.SetAttribute("color", text6);
							break;
						}
						xmlElement6.AppendChild(xmlElement7);
					}
					xmlDocument.Save(text + "\\Main.xml");
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0400040C RID: 1036
		public string root_name = "";

		// Token: 0x0400040D RID: 1037
		private XmlDocument doc;

		// Token: 0x0400040E RID: 1038
		public LightingProfileXML.KeyGroupContent key_group_content;

		// Token: 0x0400040F RID: 1039
		public LightingProfileXML.PatternGroupContent pattern_group_content;

		// Token: 0x04000410 RID: 1040
		public LightingProfileXML.ZoneGroupContent zone_group_content;

		// Token: 0x02000061 RID: 97
		public class KeyGroupContent
		{
			// Token: 0x0600030D RID: 781 RVA: 0x00023ED8 File Offset: 0x000220D8
			public KeyGroupContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.tab_status = Convert.ToInt32(xmlElement.GetAttribute("status"));
				for (int i = 0; i < 125; i++)
				{
					this.key_content.Add(new LightingProfileXML.KeyContent(doc, node_name + "/Tag" + i.ToString()));
				}
			}

			// Token: 0x04000411 RID: 1041
			public int tab_status;

			// Token: 0x04000412 RID: 1042
			public List<LightingProfileXML.KeyContent> key_content = new List<LightingProfileXML.KeyContent>();
		}

		// Token: 0x02000062 RID: 98
		public class KeyContent
		{
			// Token: 0x0600030E RID: 782 RVA: 0x00023F4B File Offset: 0x0002214B
			public KeyContent()
			{
			}

			// Token: 0x0600030F RID: 783 RVA: 0x00023F60 File Offset: 0x00022160
			public KeyContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.color = CommonFunction.ColorFromString(xmlElement.GetAttribute("color"));
			}

			// Token: 0x04000413 RID: 1043
			public Color color = Colors.White;
		}

		// Token: 0x02000063 RID: 99
		public class PatternGroupContent
		{
			// Token: 0x06000310 RID: 784 RVA: 0x00023FA4 File Offset: 0x000221A4
			public PatternGroupContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.pattern_select = Convert.ToInt32(xmlElement.GetAttribute("selected"));
				this.pattern_color = xmlElement.GetAttribute("color");
				for (int i = 0; i < 12; i++)
				{
					this.pattern_content.Add(new LightingProfileXML.PatternContent(doc, node_name + "/Pattern" + i.ToString()));
				}
			}

			// Token: 0x04000414 RID: 1044
			public int pattern_select;

			// Token: 0x04000415 RID: 1045
			public string pattern_color = "";

			// Token: 0x04000416 RID: 1046
			public List<LightingProfileXML.PatternContent> pattern_content = new List<LightingProfileXML.PatternContent>();
		}

		// Token: 0x02000064 RID: 100
		public class PatternContent
		{
			// Token: 0x06000311 RID: 785 RVA: 0x00024033 File Offset: 0x00022233
			public PatternContent()
			{
			}

			// Token: 0x06000312 RID: 786 RVA: 0x0002403C File Offset: 0x0002223C
			public PatternContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.speed = Convert.ToInt32(xmlElement.GetAttribute("speed"));
				this.direction = Convert.ToInt32(xmlElement.GetAttribute("direction"));
			}

			// Token: 0x04000417 RID: 1047
			public int speed;

			// Token: 0x04000418 RID: 1048
			public int direction;
		}

		// Token: 0x02000065 RID: 101
		public class ZoneGroupContent
		{
			// Token: 0x06000313 RID: 787 RVA: 0x0002408C File Offset: 0x0002228C
			public ZoneGroupContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.zone_effect = Convert.ToInt32(xmlElement.GetAttribute("effect"));
				this.zone_speed = Convert.ToInt32(xmlElement.GetAttribute("speed"));
				for (int i = 1; i <= 4; i++)
				{
					this.zone_content.Add(new LightingProfileXML.ZoneContent(doc, node_name + "/Zone" + i.ToString()));
				}
			}

			// Token: 0x04000419 RID: 1049
			public int zone_effect;

			// Token: 0x0400041A RID: 1050
			public int zone_speed;

			// Token: 0x0400041B RID: 1051
			public List<LightingProfileXML.ZoneContent> zone_content = new List<LightingProfileXML.ZoneContent>();
		}

		// Token: 0x02000066 RID: 102
		public class ZoneContent
		{
			// Token: 0x06000314 RID: 788 RVA: 0x00024114 File Offset: 0x00022314
			public ZoneContent()
			{
			}

			// Token: 0x06000315 RID: 789 RVA: 0x00024128 File Offset: 0x00022328
			public ZoneContent(XmlDocument doc, string node_name)
			{
				XmlNode xmlNode = doc.SelectSingleNode(node_name);
				XmlElement xmlElement = (XmlElement)xmlNode;
				this.status = Convert.ToInt32(xmlElement.GetAttribute("status"));
				this.color = xmlElement.GetAttribute("color");
			}

			// Token: 0x0400041C RID: 1052
			public int status;

			// Token: 0x0400041D RID: 1053
			public string color = "";
		}
	}
}
