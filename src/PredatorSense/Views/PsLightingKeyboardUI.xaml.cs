using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PredatorSense
{
	// Token: 0x0200002D RID: 45
	public partial class PsLightingKeyboardUI : UserControl
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000200 RID: 512 RVA: 0x0001517C File Offset: 0x0001337C
		// (remove) Token: 0x06000201 RID: 513 RVA: 0x000151B4 File Offset: 0x000133B4
		public event PsLightingKeyboardUI.PsMouseEventHandler MouseSelectedKeyEvent;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000202 RID: 514 RVA: 0x000151EC File Offset: 0x000133EC
		// (remove) Token: 0x06000203 RID: 515 RVA: 0x00015224 File Offset: 0x00013424
		public event PsLightingKeyboardUI.PsRoutedEventHandler RoutedSelectedKeyEvent;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000204 RID: 516 RVA: 0x0001525C File Offset: 0x0001345C
		// (remove) Token: 0x06000205 RID: 517 RVA: 0x00015294 File Offset: 0x00013494
		public event PsLightingKeyboardUI.PsMouseDeselectAllKeysEventHandler MouseDeselectAllKeysEvent;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000206 RID: 518 RVA: 0x000152CC File Offset: 0x000134CC
		// (remove) Token: 0x06000207 RID: 519 RVA: 0x00015304 File Offset: 0x00013504
		public event PsLightingKeyboardUI.PsRoutedDeselectAllKeysEventHandler RoutedDeselectAllKeysEvent;

		// Token: 0x06000208 RID: 520 RVA: 0x0001533C File Offset: 0x0001353C
		public PsLightingKeyboardUI()
		{
			this.InitializeComponent();
			if (Startup._TTFont)
			{
				base.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Font/#Predator");
			}
			base.Resources = Startup.styled;
			this.PresetKeysMenu.Hide();
			this.IconInfoMenu.Hide();
			this.PsLightingKeyboardUIWindows.MouseLeftButtonDown += this.Window_DeselectKeys;
			this.PsLightingKeyboardUIWindows.MouseDown += this.Window_MouseDown;
			this.PsLightingKeyboardUIWindows.MouseUp += this.Window_MouseUp;
			this.PsLightingKeyboardUIWindows.MouseMove += this.Window_MouseMove;
			this.PsLightingKeyboardUIWindows.Loaded += this.Window_Loaded;
			this.US_listBox.MouseLeftButtonDown += this.ListBoxItem_MouseLeftButtonDown;
			this.UK_listBox.MouseLeftButtonDown += this.ListBoxItem_MouseLeftButtonDown;
			this.JP_listBox.MouseLeftButtonDown += this.ListBoxItem_MouseLeftButtonDown;
			this.ShowInformationicon.Click += this.ShowInformation;
			this.SelectPresetKeysicon.Click += this.SelectPresetKeys;
			//this.PresetKeysMenu.PresetKeysMenu_All.Click += this.CB_Checked;
			//this.PresetKeysMenu.PresetKeysMenu_WASD.Click += this.CB_Checked;
			//this.PresetKeysMenu.PresetKeysMenu_Arrowkeys.Click += this.CB_Checked;
			//this.PresetKeysMenu.PresetKeysMenu_Numberrows.Click += this.CB_Checked;
			//this.PresetKeysMenu.PresetKeysMenu_Fnkeys.Click += this.CB_Checked;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00015680 File Offset: 0x00013880
		public PsLightingKeyboardUI(bool isOff)
		{
			this.InitializeComponent();
			this.PresetKeysMenu.Hide();
			this.ShowInformationicon.Visibility = Visibility.Hidden;
			this.SelectPresetKeysicon.Visibility = Visibility.Hidden;
			this.KeyboardCanvasOff.Visibility = Visibility.Visible;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00015840 File Offset: 0x00013A40
		public void DrawKeyboard2Black()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			foreach (object obj in ((IEnumerable)listBox.Items))
			{
				int num = listBox.Items.IndexOf(obj);
				if (num == -1)
				{
					break;
				}
				ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(num) as ListBoxItem;
				if (listBoxItem == null)
				{
					break;
				}
				List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
				Rectangle rectangle = list[1];
				DrawingBrush drawingBrush = rectangle.Fill as DrawingBrush;
				GeometryDrawing geometryDrawing = drawingBrush.Drawing as GeometryDrawing;
				geometryDrawing.Brush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00015940 File Offset: 0x00013B40
		public void PassKeyboardtype(int keyboardtype)
		{
			this.Keyboardtype = keyboardtype;
			if (this.Keyboardtype == 0)
			{
				this.US_listBox.Visibility = Visibility.Visible;
				return;
			}
			if (this.Keyboardtype == 1)
			{
				this.UK_listBox.Visibility = Visibility.Visible;
				return;
			}
			if (this.Keyboardtype == 2)
			{
				this.JP_listBox.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00015994 File Offset: 0x00013B94
		public ListBox GetKeyboardlist()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			return listBox;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000159DC File Offset: 0x00013BDC
		public DataTable GetAllKeys()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("KeyTag");
			dataTable.Columns.Add("KeyColor");
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			foreach (object obj in ((IEnumerable)listBox.Items))
			{
				int num = listBox.Items.IndexOf(obj);
				if (num == -1)
				{
					return null;
				}
				ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(num) as ListBoxItem;
				if (listBoxItem == null)
				{
					return null;
				}
				List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
				Rectangle rectangle = list[1];
				DrawingBrush drawingBrush = rectangle.Fill as DrawingBrush;
				GeometryDrawing geometryDrawing = drawingBrush.Drawing as GeometryDrawing;
				dataTable.Rows.Add(new object[] { rectangle.Tag, geometryDrawing.Brush });
			}
			return dataTable;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00015B34 File Offset: 0x00013D34
		public int GetTagbyindex(int Listboxindex)
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(Listboxindex) as ListBoxItem;
			if (listBoxItem == null)
			{
				return int.MinValue;
			}
			List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
			Rectangle rectangle = list[1];
			return (int)rectangle.Tag;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00015BB4 File Offset: 0x00013DB4
		public void ChangeDefaultColorbyindex(int Listboxindex, Brush keycolor)
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(Listboxindex) as ListBoxItem;
			if (listBoxItem == null)
			{
				return;
			}
			List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
			Rectangle rectangle = list[1];
			DrawingBrush drawingBrush = rectangle.Fill as DrawingBrush;
			GeometryDrawing geometryDrawing = drawingBrush.Drawing as GeometryDrawing;
			geometryDrawing.Brush = keycolor;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00015C48 File Offset: 0x00013E48
		public void ChangeColortoSelectedKeys(Brush borderBrush)
		{
			if (this.PresetKeysMenu.Topmost && !this.PresetKeysMenu.IsActive)
			{
				this.SelectPresetKeysicon.IsChecked = new bool?(false);
				this.PresetKeysMenu.Topmost = false;
				this.PresetKeysMenu.Hide();
			}
			if (this.IconInfoMenu.Topmost && !this.IconInfoMenu.IsActive)
			{
				this.ShowInformationicon.IsChecked = new bool?(false);
				this.IconInfoMenu.Topmost = false;
				this.IconInfoMenu.Hide();
			}
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			foreach (object obj in listBox.SelectedItems)
			{
				int num = listBox.Items.IndexOf(obj);
				if (num == -1)
				{
					break;
				}
				ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(num) as ListBoxItem;
				if (listBoxItem == null)
				{
					break;
				}
				List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
				Rectangle rectangle = list[1];
				DrawingBrush drawingBrush = rectangle.Fill as DrawingBrush;
				GeometryDrawing geometryDrawing = drawingBrush.Drawing as GeometryDrawing;
				geometryDrawing.Brush = borderBrush;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00015DC0 File Offset: 0x00013FC0
		public void DeselectAllKeys()
		{
			if (!this.PresetKeysMenu.Topmost && !this.IconInfoMenu.Topmost && (this.US_listBox.SelectedItems.Count != 0 || this.UK_listBox.SelectedItems.Count != 0 || this.JP_listBox.SelectedItems.Count != 0) && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
			{
				if (this.IsUnselectedKeyswithPresetKeysMenuPopup)
				{
					if (this.Keyboardtype == 0)
					{
						this.US_listBox.SelectedItems.Clear();
					}
					else if (this.Keyboardtype == 1)
					{
						this.UK_listBox.SelectedItems.Clear();
					}
					else if (this.Keyboardtype == 2)
					{
						this.JP_listBox.SelectedItems.Clear();
					}
					this.PresetKeysMenu.UncheckAllPresetKeysMenu();
				}
				else
				{
					this.IsUnselectedKeyswithPresetKeysMenuPopup = true;
				}
			}
			if (this.PresetKeysMenu.Topmost && !this.PresetKeysMenu.IsActive)
			{
				this.SelectPresetKeysicon.IsChecked = new bool?(false);
				this.PresetKeysMenu.Topmost = false;
				this.PresetKeysMenu.Hide();
			}
			if (this.IconInfoMenu.Topmost && !this.IconInfoMenu.IsActive)
			{
				this.ShowInformationicon.IsChecked = new bool?(false);
				this.IconInfoMenu.Topmost = false;
				this.IconInfoMenu.Hide();
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00015F1C File Offset: 0x0001411C
		public bool HaveSelectedKeys()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			return listBox.SelectedItems.Count != 0;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00015F72 File Offset: 0x00014172
		private ViewModel ViewModel
		{
			get
			{
				return (ViewModel)base.DataContext;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00015F80 File Offset: 0x00014180
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.GetLightingKeyboardUICoordinate();
			this.GetFirstKeyCoordinate();
			this.Method1.Text = Startup.langRd["MUI_Method_Number"].ToString().Replace("%Number%", "1");
			this.Method2.Text = Startup.langRd["MUI_Method_Number"].ToString().Replace("%Number%", "2");
			this.Method1_Info2_up.Text = " " + Startup.langRd["MUI_Method1_Info2"].ToString();
			if (Startup.langRd["MUI_Method1_Info1"].ToString().IndexOf("%icon%") == 0)
			{
				this.SelectRec_R.Visibility = Visibility.Collapsed;
				this.SelectRec_L.Visibility = Visibility.Visible;
				this.Method1_Info1.Text = Startup.langRd["MUI_Method1_Info1"].ToString().Replace("%icon%", "");
			}
			else
			{
				this.SelectRec_R.Visibility = Visibility.Visible;
				this.SelectRec_L.Visibility = Visibility.Collapsed;
				this.Method1_Info1.Text = Startup.langRd["MUI_Method1_Info1"].ToString().Replace("%icon%", "");
			}
			if (42 - (6 + this.Method1_Info1.Text.Length) < this.Method1_Info2_up.Text.Length)
			{
				this.Method1_Info2_up.Visibility = Visibility.Collapsed;
				this.Method1_Info2_down.Visibility = Visibility.Visible;
			}
			else
			{
				this.Method1_Info2_up.Visibility = Visibility.Visible;
				this.Method1_Info2_down.Visibility = Visibility.Collapsed;
			}
			this.PresetKeysMenu_All.Click += this.PresetKeysMenu_All_Checked;
			this.PresetKeysMenu_WASD.Click += this.PresetKeysMenu_WASD_Checked;
			this.PresetKeysMenu_Arrowkeys.Click += this.PresetKeysMenu_Arrowkeys_Checked;
			this.PresetKeysMenu_Numberrows.Click += this.PresetKeysMenu_Numberrows_Checked;
			this.PresetKeysMenu_Fnkeys.Click += this.PresetKeysMenu_Fnkeys_Checked;
			if (this.Keyboardtype == 0)
			{
				this.keyListBox = this.US_listBox;
				return;
			}
			if (this.Keyboardtype == 1)
			{
				this.keyListBox = this.UK_listBox;
				return;
			}
			if (this.Keyboardtype == 2)
			{
				this.keyListBox = this.JP_listBox;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000161D8 File Offset: 0x000143D8
		private void GetLightingKeyboardUICoordinate()
		{
			Window window = Window.GetWindow(this);
			this.LightingKeyboardUICoordinate = base.TranslatePoint(new Point(window.Left, window.Top), Window.GetWindow(this));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00016210 File Offset: 0x00014410
		private void GetFirstKeyCoordinate()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
			List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
			Rectangle rectangle = list[1];
			Window window = Window.GetWindow(this);
			this.FirstKeypointCoordinate = rectangle.TranslatePoint(new Point(window.Left, window.Top), Window.GetWindow(rectangle));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000162AC File Offset: 0x000144AC
		private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
			{
				if (this.Keyboardtype == 0)
				{
					this.US_listBox.SelectedItems.Clear();
				}
				else if (this.Keyboardtype == 1)
				{
					this.UK_listBox.SelectedItems.Clear();
				}
				else if (this.Keyboardtype == 2)
				{
					this.JP_listBox.SelectedItems.Clear();
				}
				this.PresetKeysMenu.UncheckAllPresetKeysMenu();
			}
			if (this.PresetKeysMenu.Topmost && !this.PresetKeysMenu.IsActive)
			{
				this.SelectPresetKeysicon.IsChecked = new bool?(false);
				this.PresetKeysMenu.Topmost = false;
				this.PresetKeysMenu.Hide();
			}
			if (this.IconInfoMenu.Topmost && !this.IconInfoMenu.IsActive)
			{
				this.ShowInformationicon.IsChecked = new bool?(false);
				this.IconInfoMenu.Topmost = false;
				this.IconInfoMenu.Hide();
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000163A0 File Offset: 0x000145A0
		    private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.isDraggingSelectionRect)
                {
                    this.isDraggingSelectionRect = false;
                    this.dragSelectionCanvas.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                }
                if (this.US_listBox.SelectedItems.Count == 0 && this.UK_listBox.SelectedItems.Count == 0 && this.JP_listBox.SelectedItems.Count == 0)
                {
                    try
                    {
                        this.MouseDeselectAllKeysEvent(this, new MouseEventArgsWithDeselectAllKeys(e.MouseDevice, e.Timestamp, e.StylusDevice));
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    try
                    {
                        this.GetSelectedKeysList();
                        this.MouseSelectedKeyEvent(this, new MouseEventArgsWithKeyData(e.MouseDevice, e.Timestamp, e.StylusDevice, this._selectedTaglist, this._selectedColorlist));
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }

                if (this.isLeftMouseButtonDownOnWindow)
                {
                    this.isLeftMouseButtonDownOnWindow = false;
                    base.ReleaseMouseCapture();
                    e.Handled = true;
                }
            }
        }

		// Token: 0x06000219 RID: 537 RVA: 0x000164AC File Offset: 0x000146AC
		private void GetSelectedKeysList()
		{
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
			}
			int num = 0;
			ItemContainerGenerator itemContainerGenerator = listBox.ItemContainerGenerator;
			this._selectedTaglist = new int[listBox.SelectedItems.Count];
			this._selectedColorlist = new Brush[listBox.SelectedItems.Count];
			foreach (object obj in listBox.SelectedItems)
			{
				int num2 = listBox.Items.IndexOf(obj);
				if (num2 == -1)
				{
					break;
				}
				ListBoxItem listBoxItem = itemContainerGenerator.ContainerFromIndex(num2) as ListBoxItem;
				if (listBoxItem == null)
				{
					break;
				}
				List<Rectangle> list = this.FindVisualChild<Rectangle>(listBoxItem);
				Rectangle rectangle = list[1];
				DrawingBrush drawingBrush = rectangle.Fill as DrawingBrush;
				GeometryDrawing geometryDrawing = drawingBrush.Drawing as GeometryDrawing;
				this._selectedTaglist[num] = (int)rectangle.Tag;
				this._selectedColorlist[num] = geometryDrawing.Brush;
				num++;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000165F8 File Offset: 0x000147F8
		private void ListBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.isLeftMouseButtonDownOnWindow = true;
				this.origMouseDownPoint = e.GetPosition(this);
				this.origMouseDownPoint.X = this.origMouseDownPoint.X + this.LightingKeyboardUICoordinate.X;
				this.origMouseDownPoint.Y = this.origMouseDownPoint.Y + this.LightingKeyboardUICoordinate.Y;
				base.CaptureMouse();
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0001666C File Offset: 0x0001486C
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.GetLightingKeyboardUICoordinate();
			this.GetFirstKeyCoordinate();
			if (e.ChangedButton == MouseButton.Left)
			{
				this.isLeftMouseButtonDownOnWindow = true;
				this.origMouseDownPoint = e.GetPosition(this);
				this.origMouseDownPoint.X = this.origMouseDownPoint.X + this.LightingKeyboardUICoordinate.X;
				this.origMouseDownPoint.Y = this.origMouseDownPoint.Y + this.LightingKeyboardUICoordinate.Y;
				base.CaptureMouse();
				e.Handled = true;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000166F4 File Offset: 0x000148F4
		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.GetLightingKeyboardUICoordinate();
			this.GetFirstKeyCoordinate();
			if (e.ChangedButton == MouseButton.Left)
			{
				if (this.isDraggingSelectionRect)
				{
					this.isDraggingSelectionRect = false;
					this.dragSelectionCanvas.Visibility = Visibility.Collapsed;
					if (this.US_listBox.SelectedItems.Count == 0 && this.UK_listBox.SelectedItems.Count == 0)
					{
						if (this.JP_listBox.SelectedItems.Count == 0)
						{
							goto IL_00A5;
						}
					}
					try
					{
						this.GetSelectedKeysList();
						this.MouseSelectedKeyEvent(this, new MouseEventArgsWithKeyData(e.MouseDevice, e.Timestamp, e.StylusDevice, this._selectedTaglist, this._selectedColorlist));
					}
					catch (Exception)
					{
						return;
					}
					IL_00A5:
					e.Handled = true;
				}
				if (this.isLeftMouseButtonDownOnWindow)
				{
					this.isLeftMouseButtonDownOnWindow = false;
					base.ReleaseMouseCapture();
					e.Handled = true;
				}
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000167DC File Offset: 0x000149DC
		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			this.GetLightingKeyboardUICoordinate();
			this.GetFirstKeyCoordinate();
			if (this.isDraggingSelectionRect)
			{
				Point position = e.GetPosition(this);
				position.X += this.LightingKeyboardUICoordinate.X;
				position.Y += this.LightingKeyboardUICoordinate.Y;
				this.UpdateDragSelectionRect(this.origMouseDownPoint, position);
				Rect rect = new Rect(this.origMouseDownPoint, position);
				this.ApplyDragSelectionRect_Move(rect);
				e.Handled = true;
				return;
			}
			if (this.isLeftMouseButtonDownOnWindow)
			{
				Point position2 = e.GetPosition(this);
				position2.X += this.LightingKeyboardUICoordinate.X;
				position2.Y += this.LightingKeyboardUICoordinate.Y;
				double num = Math.Abs((position2 - this.origMouseDownPoint).Length);
				if (num > PsLightingKeyboardUI.DragThreshold)
				{
					this.isDraggingSelectionRect = true;
					this.InitDragSelectionRect(this.origMouseDownPoint, position2);
				}
				e.Handled = true;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000168E6 File Offset: 0x00014AE6
		private void InitDragSelectionRect(Point pt1, Point pt2)
		{
			this.UpdateDragSelectionRect(pt1, pt2);
			this.dragSelectionCanvas.Visibility = Visibility.Visible;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000168FC File Offset: 0x00014AFC
		private void UpdateDragSelectionRect(Point pt1, Point pt2)
		{
			double num;
			double num2;
			if (pt2.X < pt1.X)
			{
				num = pt2.X - this.LightingKeyboardUICoordinate.X;
				num2 = pt1.X - pt2.X;
			}
			else
			{
				num = pt1.X - this.LightingKeyboardUICoordinate.X;
				num2 = pt2.X - pt1.X;
			}
			double num3;
			double num4;
			if (pt2.Y < pt1.Y)
			{
				num3 = pt2.Y - this.LightingKeyboardUICoordinate.Y;
				num4 = pt1.Y - pt2.Y;
			}
			else
			{
				num3 = pt1.Y - this.LightingKeyboardUICoordinate.Y;
				num4 = pt2.Y - pt1.Y;
			}
			Canvas.SetLeft(this.dragSelectionBorder, num);
			Canvas.SetTop(this.dragSelectionBorder, num3);
			this.dragSelectionBorder.Width = num2;
			this.dragSelectionBorder.Height = num4;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000169F0 File Offset: 0x00014BF0
		private void ApplyDragSelectionRect_Move(Rect bandrect)
		{
			ObservableCollection<RectangleViewModel> observableCollection = new ObservableCollection<RectangleViewModel>();
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
				observableCollection = this.ViewModel.USRectangles;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
				observableCollection = this.ViewModel.UKRectangles;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
				observableCollection = this.ViewModel.JPRectangles;
			}
			foreach (RectangleViewModel rectangleViewModel in observableCollection)
			{
				Rect rect = new Rect(rectangleViewModel.Canvas_x + this.FirstKeypointCoordinate.X, rectangleViewModel.Canvas_y + this.FirstKeypointCoordinate.Y, rectangleViewModel.Outerwidth, rectangleViewModel.Outerheight);
				if (bandrect.IntersectsWith(rect))
				{
					listBox.SelectedItems.Add(rectangleViewModel);
				}
				else if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
				{
					listBox.SelectedItems.Remove(rectangleViewModel);
				}
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00016B00 File Offset: 0x00014D00
		private void ApplyDragSelectionRect_Up()
		{
			double left = Canvas.GetLeft(this.dragSelectionBorder);
			double top = Canvas.GetTop(this.dragSelectionBorder);
			double width = this.dragSelectionBorder.Width;
			double height = this.dragSelectionBorder.Height;
			Rect rect = new Rect(left + this.LightingKeyboardUICoordinate.X, top + this.LightingKeyboardUICoordinate.Y, width, height);
			ObservableCollection<RectangleViewModel> observableCollection = new ObservableCollection<RectangleViewModel>();
			ListBox listBox = new ListBox();
			if (this.Keyboardtype == 0)
			{
				listBox = this.US_listBox;
				observableCollection = this.ViewModel.USRectangles;
			}
			else if (this.Keyboardtype == 1)
			{
				listBox = this.UK_listBox;
				observableCollection = this.ViewModel.UKRectangles;
			}
			else if (this.Keyboardtype == 2)
			{
				listBox = this.JP_listBox;
				observableCollection = this.ViewModel.JPRectangles;
			}
			foreach (RectangleViewModel rectangleViewModel in observableCollection)
			{
				Rect rect2 = new Rect(rectangleViewModel.Canvas_x + this.FirstKeypointCoordinate.X, rectangleViewModel.Canvas_y + this.FirstKeypointCoordinate.Y, rectangleViewModel.Outerwidth, rectangleViewModel.Outerheight);
				if (rect.IntersectsWith(rect2))
				{
					listBox.SelectedItems.Add(rectangleViewModel);
				}
				else if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
				{
					listBox.SelectedItems.Remove(rectangleViewModel);
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00016C78 File Offset: 0x00014E78
		private void Window_DeselectKeys(object sender, MouseButtonEventArgs e)
		{
			if (!this.PresetKeysMenu.Topmost && !this.IconInfoMenu.Topmost)
			{
				if ((this.US_listBox.SelectedItems.Count != 0 || this.UK_listBox.SelectedItems.Count != 0 || this.JP_listBox.SelectedItems.Count != 0) && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
				{
					if (this.IsUnselectedKeyswithPresetKeysMenuPopup)
					{
						if (this.Keyboardtype == 0)
						{
							this.US_listBox.SelectedItems.Clear();
						}
						else if (this.Keyboardtype == 1)
						{
							this.UK_listBox.SelectedItems.Clear();
						}
						else if (this.Keyboardtype == 2)
						{
							this.JP_listBox.SelectedItems.Clear();
						}
						this.PresetKeysMenu.UncheckAllPresetKeysMenu();
					}
					else
					{
						this.IsUnselectedKeyswithPresetKeysMenuPopup = true;
					}
				}
				this.MouseDeselectAllKeysEvent(this, new MouseEventArgsWithDeselectAllKeys(e.MouseDevice, e.Timestamp, e.StylusDevice));
			}
			if (this.PresetKeysMenu.Topmost && !this.PresetKeysMenu.IsActive)
			{
				this.SelectPresetKeysicon.IsChecked = new bool?(false);
				this.PresetKeysMenu.Topmost = false;
				this.PresetKeysMenu.Hide();
			}
			if (this.IconInfoMenu.Topmost && !this.IconInfoMenu.IsActive)
			{
				this.ShowInformationicon.IsChecked = new bool?(false);
				this.IconInfoMenu.Topmost = false;
				this.IconInfoMenu.Hide();
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00016DF8 File Offset: 0x00014FF8
		private List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
		{
			List<T> list4;
			try
			{
				List<T> list = new List<T>();
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(obj, i);
					if (child != null && child is T)
					{
						list.Add((T)((object)child));
						List<T> list2 = this.FindVisualChild<T>(child);
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
					else
					{
						List<T> list3 = this.FindVisualChild<T>(child);
						if (list3 != null)
						{
							list.AddRange(list3);
						}
					}
				}
				list4 = list;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				list4 = null;
			}
			return list4;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00016E90 File Offset: 0x00015090
		private void ShowInformation(object sender, RoutedEventArgs e)
		{
			if (this.IconInfoMenuPopup.IsOpen)
			{
				this.IconInfoMenuPopup.IsOpen = false;
				return;
			}
			this.IconInfoMenuPopup.IsOpen = true;
			this.IsUnselectedKeyswithPresetKeysMenuPopup = false;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00016EBF File Offset: 0x000150BF
		private void SelectPresetKeys(object sender, RoutedEventArgs e)
		{
			if (this.PresetKeysMenuPopup.IsOpen)
			{
				this.PresetKeysMenuPopup.IsOpen = false;
			}
			else
			{
				this.PresetKeysMenuPopup.IsOpen = true;
				this.IsUnselectedKeyswithPresetKeysMenuPopup = false;
			}
			this.UpdatePresetKeysMenu();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00016EF8 File Offset: 0x000150F8
		    private void CB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.US_listBox.SelectedItems.Count == 0 && this.UK_listBox.SelectedItems.Count == 0 && this.JP_listBox.SelectedItems.Count == 0)
            {
                try
                {
                    this.RoutedDeselectAllKeysEvent(this, new RoutedEventArgsWithDeselectAllKeys(e.RoutedEvent, e.Source));
                }
                catch (Exception)
                {
                    // Handle exception if necessary
                }
            }
            else
            {
                try
                {
                    this.GetSelectedKeysList();
                    this.RoutedSelectedKeyEvent(this, new RoutedEventArgsWithKeyData(e.RoutedEvent, e.Source, this._selectedTaglist, this._selectedColorlist));
                }
                catch (Exception)
                {
                    // Handle exception if necessary
                }
            }
        }

		// Token: 0x06000227 RID: 551 RVA: 0x00016FB0 File Offset: 0x000151B0
		public void HideInformation()
		{
			if (this.ShowInformationicon.IsChecked == true)
			{
				this.PresetKeysMenu.Hide();
				this.PresetKeysMenu.Topmost = false;
				this.ShowInformationicon.IsChecked = new bool?(false);
			}
			if (this.SelectPresetKeysicon.IsChecked == true)
			{
				this.IconInfoMenu.Hide();
				this.IconInfoMenu.Topmost = false;
				this.SelectPresetKeysicon.IsChecked = new bool?(false);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00017050 File Offset: 0x00015250
		private void Common_Popup_Closed(object sender, EventArgs e)
		{
			this.SelectPresetKeysicon.IsChecked = new bool?(false);
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Closed(sender, e);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00017084 File Offset: 0x00015284
		private void Common_Popup_Opened(object sender, EventArgs e)
		{
			MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
			mainWindow.setting_Popup_Opened(sender, e);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000170A8 File Offset: 0x000152A8
		public void UncheckAllPresetKeysMenu()
		{
			this.PresetKeysMenu_WASD.IsChecked = new bool?(false);
			this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
			this.PresetKeysMenu_Numberrows.IsChecked = new bool?(false);
			this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000170FC File Offset: 0x000152FC
		private void UpdatePresetKeysMenu()
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
			this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
			this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
			this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
			if (this.item35.IsSelected && this.item51.IsSelected && this.item52.IsSelected && this.item53.IsSelected)
			{
				this.PresetKeysMenu_WASD.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_WASD.IsChecked = new bool?(false);
			}
			this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
			this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
			this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
			this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
			this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
			this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
			this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
			this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
			this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
			this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
			this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
			if (this.item75.IsSelected && this.item84.IsSelected && this.item85.IsSelected && this.item86.IsSelected)
			{
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
			}
			if (this.PresetKeysMenu_Arrowkeys.IsChecked == true && this.item3.IsSelected && this.item4.IsSelected && this.item5.IsSelected && this.item6.IsSelected && this.item7.IsSelected && this.item8.IsSelected && this.item77.IsSelected)
			{
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
			}
			this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
			this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
			this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
			this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
			this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
			this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
			this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
			this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
			this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
			this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
			if (this.item17.IsSelected && this.item18.IsSelected && this.item19.IsSelected && this.item20.IsSelected && this.item21.IsSelected && this.item22.IsSelected && this.item23.IsSelected && this.item24.IsSelected && this.item25.IsSelected && this.item26.IsSelected)
			{
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(true);
			}
			else
			{
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(false);
			}
			if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
			{
				this.PresetKeysMenu_All.IsChecked = new bool?(true);
				return;
			}
			this.PresetKeysMenu_All.IsChecked = new bool?(false);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00017528 File Offset: 0x00015728
		private void PresetKeysMenu_All_Checked(object sender, RoutedEventArgs e)
		{
			if (this.PresetKeysMenu_All.IsChecked == true)
			{
				this.keyListBox.SelectAll();
				this.PresetKeysMenu_WASD.IsChecked = new bool?(true);
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
				this.PresetKeysMenu_Numberrows.IsChecked = new bool?(true);
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
			}
			else
			{
				this.keyListBox.SelectedItems.Clear();
				this.UncheckAllPresetKeysMenu();
			}
			this.CB_Checked(sender, e);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000175C8 File Offset: 0x000157C8
		private void PresetKeysMenu_WASD_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_WASD.IsChecked == true)
			{
				this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
				this.item35.IsSelected = true;
				this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
				this.item51.IsSelected = true;
				this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
				this.item52.IsSelected = true;
				this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
				this.item53.IsSelected = true;
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
				}
			}
			else
			{
				this.item35 = itemContainerGenerator.ContainerFromIndex(35) as ListBoxItem;
				this.item35.IsSelected = false;
				this.item51 = itemContainerGenerator.ContainerFromIndex(51) as ListBoxItem;
				this.item51.IsSelected = false;
				this.item52 = itemContainerGenerator.ContainerFromIndex(52) as ListBoxItem;
				this.item52.IsSelected = false;
				this.item53 = itemContainerGenerator.ContainerFromIndex(53) as ListBoxItem;
				this.item53.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
			this.CB_Checked(sender, e);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00017754 File Offset: 0x00015954
		private void PresetKeysMenu_Arrowkeys_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Arrowkeys.IsChecked == true)
			{
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = true;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = true;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = true;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = true;
				if (this.item3.IsSelected && this.item4.IsSelected && this.item5.IsSelected && this.item6.IsSelected && this.item7.IsSelected && this.item8.IsSelected && this.item77.IsSelected)
				{
					this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(true);
				}
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
				}
			}
			else
			{
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = false;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = false;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = false;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
				this.PresetKeysMenu_Fnkeys.IsChecked = new bool?(false);
			}
			this.CB_Checked(sender, e);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001795C File Offset: 0x00015B5C
		private void PresetKeysMenu_Numberrows_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Numberrows.IsChecked == true)
			{
				this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
				this.item17.IsSelected = true;
				this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
				this.item18.IsSelected = true;
				this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
				this.item19.IsSelected = true;
				this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
				this.item20.IsSelected = true;
				this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
				this.item21.IsSelected = true;
				this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
				this.item22.IsSelected = true;
				this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
				this.item23.IsSelected = true;
				this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
				this.item24.IsSelected = true;
				this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
				this.item25.IsSelected = true;
				this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
				this.item26.IsSelected = true;
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
				}
			}
			else
			{
				this.item17 = itemContainerGenerator.ContainerFromIndex(17) as ListBoxItem;
				this.item17.IsSelected = false;
				this.item18 = itemContainerGenerator.ContainerFromIndex(18) as ListBoxItem;
				this.item18.IsSelected = false;
				this.item19 = itemContainerGenerator.ContainerFromIndex(19) as ListBoxItem;
				this.item19.IsSelected = false;
				this.item20 = itemContainerGenerator.ContainerFromIndex(20) as ListBoxItem;
				this.item20.IsSelected = false;
				this.item21 = itemContainerGenerator.ContainerFromIndex(21) as ListBoxItem;
				this.item21.IsSelected = false;
				this.item22 = itemContainerGenerator.ContainerFromIndex(22) as ListBoxItem;
				this.item22.IsSelected = false;
				this.item23 = itemContainerGenerator.ContainerFromIndex(23) as ListBoxItem;
				this.item23.IsSelected = false;
				this.item24 = itemContainerGenerator.ContainerFromIndex(24) as ListBoxItem;
				this.item24.IsSelected = false;
				this.item25 = itemContainerGenerator.ContainerFromIndex(25) as ListBoxItem;
				this.item25.IsSelected = false;
				this.item26 = itemContainerGenerator.ContainerFromIndex(26) as ListBoxItem;
				this.item26.IsSelected = false;
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
			this.CB_Checked(sender, e);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00017C5C File Offset: 0x00015E5C
		private void PresetKeysMenu_Fnkeys_Checked(object sender, RoutedEventArgs e)
		{
			ItemContainerGenerator itemContainerGenerator = this.keyListBox.ItemContainerGenerator;
			if (this.PresetKeysMenu_Fnkeys.IsChecked == true)
			{
				this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
				this.item3.IsSelected = true;
				this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
				this.item4.IsSelected = true;
				this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
				this.item5.IsSelected = true;
				this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
				this.item6.IsSelected = true;
				this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
				this.item7.IsSelected = true;
				this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
				this.item8.IsSelected = true;
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = true;
				this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
				this.item77.IsSelected = true;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = true;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = true;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = true;
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(true);
				if (this.keyListBox.SelectedItems.Count == this.keyListBox.Items.Count)
				{
					this.PresetKeysMenu_All.IsChecked = new bool?(true);
				}
			}
			else
			{
				this.item3 = itemContainerGenerator.ContainerFromIndex(3) as ListBoxItem;
				this.item3.IsSelected = false;
				this.item4 = itemContainerGenerator.ContainerFromIndex(4) as ListBoxItem;
				this.item4.IsSelected = false;
				this.item5 = itemContainerGenerator.ContainerFromIndex(5) as ListBoxItem;
				this.item5.IsSelected = false;
				this.item6 = itemContainerGenerator.ContainerFromIndex(6) as ListBoxItem;
				this.item6.IsSelected = false;
				this.item7 = itemContainerGenerator.ContainerFromIndex(7) as ListBoxItem;
				this.item7.IsSelected = false;
				this.item8 = itemContainerGenerator.ContainerFromIndex(8) as ListBoxItem;
				this.item8.IsSelected = false;
				this.item75 = itemContainerGenerator.ContainerFromIndex(75) as ListBoxItem;
				this.item75.IsSelected = false;
				this.item77 = itemContainerGenerator.ContainerFromIndex(77) as ListBoxItem;
				this.item77.IsSelected = false;
				this.item84 = itemContainerGenerator.ContainerFromIndex(84) as ListBoxItem;
				this.item84.IsSelected = false;
				this.item85 = itemContainerGenerator.ContainerFromIndex(85) as ListBoxItem;
				this.item85.IsSelected = false;
				this.item86 = itemContainerGenerator.ContainerFromIndex(86) as ListBoxItem;
				this.item86.IsSelected = false;
				this.PresetKeysMenu_Arrowkeys.IsChecked = new bool?(false);
				this.PresetKeysMenu_All.IsChecked = new bool?(false);
			}
			this.CB_Checked(sender, e);
		}

		//// Token: 0x06000233 RID: 563 RVA: 0x0001820C File Offset: 0x0001640C
		//[DebuggerNonUserCode]
		//[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		//[EditorBrowsable(EditorBrowsableState.Never)]
		//void IStyleConnector.Connect(int connectionId, object target)
		//{
		//	if (connectionId != 2)
		//	{
		//		return;
		//	}
		//	((Grid)target).MouseDown += this.ListBoxItem_MouseDown;
		//	((Grid)target).MouseLeftButtonDown += this.ListBoxItem_MouseLeftButtonDown;
		//	((Grid)target).MouseUp += this.ListBoxItem_MouseUp;
		//}

		// Token: 0x04000236 RID: 566
		private bool isLeftMouseButtonDownOnWindow;

		// Token: 0x04000237 RID: 567
		private bool isDraggingSelectionRect;

		// Token: 0x04000238 RID: 568
		private Point origMouseDownPoint;

		// Token: 0x04000239 RID: 569
		private static readonly double DragThreshold = 50.0;

		// Token: 0x0400023A RID: 570
		private ShowPresetKeys PresetKeysMenu = new ShowPresetKeys();

		// Token: 0x0400023B RID: 571
		private ShowIconInfo IconInfoMenu = new ShowIconInfo();

		// Token: 0x0400023C RID: 572
		private int Keyboardtype;

		// Token: 0x0400023D RID: 573
		private Point FirstKeypointCoordinate = new Point(double.NaN, double.NaN);

		// Token: 0x0400023E RID: 574
		private Point LightingKeyboardUICoordinate = new Point(double.NaN, double.NaN);

		// Token: 0x0400023F RID: 575
		private int[] _selectedTaglist;

		// Token: 0x04000240 RID: 576
		private Brush[] _selectedColorlist;

		// Token: 0x04000241 RID: 577
		private bool IsUnselectedKeyswithPresetKeysMenuPopup = true;

		// Token: 0x04000242 RID: 578
		private ListBox keyListBox = new ListBox();

		// Token: 0x04000243 RID: 579
		private ListBoxItem item35 = new ListBoxItem();

		// Token: 0x04000244 RID: 580
		private ListBoxItem item51 = new ListBoxItem();

		// Token: 0x04000245 RID: 581
		private ListBoxItem item52 = new ListBoxItem();

		// Token: 0x04000246 RID: 582
		private ListBoxItem item53 = new ListBoxItem();

		// Token: 0x04000247 RID: 583
		private ListBoxItem item75 = new ListBoxItem();

		// Token: 0x04000248 RID: 584
		private ListBoxItem item84 = new ListBoxItem();

		// Token: 0x04000249 RID: 585
		private ListBoxItem item85 = new ListBoxItem();

		// Token: 0x0400024A RID: 586
		private ListBoxItem item86 = new ListBoxItem();

		// Token: 0x0400024B RID: 587
		private ListBoxItem item17 = new ListBoxItem();

		// Token: 0x0400024C RID: 588
		private ListBoxItem item18 = new ListBoxItem();

		// Token: 0x0400024D RID: 589
		private ListBoxItem item19 = new ListBoxItem();

		// Token: 0x0400024E RID: 590
		private ListBoxItem item20 = new ListBoxItem();

		// Token: 0x0400024F RID: 591
		private ListBoxItem item21 = new ListBoxItem();

		// Token: 0x04000250 RID: 592
		private ListBoxItem item22 = new ListBoxItem();

		// Token: 0x04000251 RID: 593
		private ListBoxItem item23 = new ListBoxItem();

		// Token: 0x04000252 RID: 594
		private ListBoxItem item24 = new ListBoxItem();

		// Token: 0x04000253 RID: 595
		private ListBoxItem item25 = new ListBoxItem();

		// Token: 0x04000254 RID: 596
		private ListBoxItem item26 = new ListBoxItem();

		// Token: 0x04000255 RID: 597
		private ListBoxItem item3 = new ListBoxItem();

		// Token: 0x04000256 RID: 598
		private ListBoxItem item4 = new ListBoxItem();

		// Token: 0x04000257 RID: 599
		private ListBoxItem item5 = new ListBoxItem();

		// Token: 0x04000258 RID: 600
		private ListBoxItem item6 = new ListBoxItem();

		// Token: 0x04000259 RID: 601
		private ListBoxItem item7 = new ListBoxItem();

		// Token: 0x0400025A RID: 602
		private ListBoxItem item8 = new ListBoxItem();

		// Token: 0x0400025B RID: 603
		private ListBoxItem item77 = new ListBoxItem();

		// Token: 0x0200002E RID: 46
		// (Invoke) Token: 0x06000236 RID: 566
		public delegate void PsMouseEventHandler(object sender, MouseEventArgsWithKeyData e);

		// Token: 0x0200002F RID: 47
		// (Invoke) Token: 0x0600023A RID: 570
		public delegate void PsRoutedEventHandler(object sender, RoutedEventArgsWithKeyData e);

		// Token: 0x02000030 RID: 48
		// (Invoke) Token: 0x0600023E RID: 574
		public delegate void PsMouseDeselectAllKeysEventHandler(object sender, MouseEventArgsWithDeselectAllKeys e);

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x06000242 RID: 578
		public delegate void PsRoutedDeselectAllKeysEventHandler(object sender, RoutedEventArgsWithDeselectAllKeys e);

		// Token: 0x02000032 RID: 50
		private enum Keyboard_type
		{
			// Token: 0x04000277 RID: 631
			US,
			// Token: 0x04000278 RID: 632
			UK,
			// Token: 0x04000279 RID: 633
			JP
		}
	}
}
