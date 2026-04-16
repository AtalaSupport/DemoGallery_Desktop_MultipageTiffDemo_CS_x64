using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Atalasoft.Imaging.WinControls;
using Atalasoft.Imaging;
using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Codec.Tiff;
using Atalasoft.Imaging.ImageProcessing;
using Atalasoft.Imaging.ImageProcessing.Document;
using Atalasoft.Imaging.ImageProcessing.Effects;
using Atalasoft.Imaging.ImageProcessing.Filters;
using Atalasoft.Imaging.ImageProcessing.Transforms;
using Atalasoft.Imaging.ImageProcessing.Channels;
using Atalasoft.Imaging.Metadata;
using Atalasoft.Imaging.Drawing;

namespace MultipageTiffDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		// Local storage.
		private bool _validLicense;
		private int pageCount = 0;
		private int currentPage = 1;	// 1-based index
		private string currentFile = "";
		private TiffCompression tiffCompression = TiffCompression.Group4FaxEncoding;
		private int maxPageMenus = 15;
		private int[] dragDropIndex = new int[0];
		private bool imageChanged = false;

		#region Private Designer Vars
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuOpen;
		private System.Windows.Forms.MenuItem menuSaveAs;
		private System.Windows.Forms.MenuItem menuFileSep1;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuPages;
		private System.Windows.Forms.MenuItem menuAddPage;
		private System.Windows.Forms.MenuItem menuRemovePage;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusMessage;
		private System.Windows.Forms.StatusBarPanel statusPage;
		private System.Windows.Forms.MenuItem menuPageSep2;
		private System.Windows.Forms.MenuItem menuAppend;
		private System.Windows.Forms.MenuItem menuModify;
		private System.Windows.Forms.MenuItem menuModifySep1;
		private System.Windows.Forms.MenuItem menuErosion;
		private System.Windows.Forms.MenuItem menuAntialiasDisplay;
		private System.Windows.Forms.MenuItem menuPageSep1;
		private System.Windows.Forms.MenuItem menuDilation;
		private System.Windows.Forms.MenuItem menuBoundaryExtraction;
		private System.Windows.Forms.MenuItem menuAutoDeskew;
		private System.Windows.Forms.MenuItem menuMorpho;
		private System.Windows.Forms.MenuItem menuFileSep2;
		private System.Windows.Forms.MenuItem menuAntiNone;
		private System.Windows.Forms.MenuItem menuAntiGray;
		private System.Windows.Forms.MenuItem menuAntiReduction;
		private System.Windows.Forms.MenuItem menuAntiFull;
		private System.Windows.Forms.MenuItem menuTiffCompression;
		private System.Windows.Forms.MenuItem menuCompressNone;
		private System.Windows.Forms.MenuItem menuCompressGroup3;
		private System.Windows.Forms.MenuItem menuCompressGroup4;
		private System.Windows.Forms.MenuItem menuCompressLzw;
		private System.Windows.Forms.MenuItem menuCompressDeflate;
		private System.Windows.Forms.MenuItem menuCompressJpeg;
		private System.Windows.Forms.MenuItem menuCompressHuffman;
		private System.Windows.Forms.MenuItem menuCompressMac;
		private System.Windows.Forms.MenuItem menuFlipHorizontal;
		private System.Windows.Forms.MenuItem menuRotate;
		private System.Windows.Forms.MenuItem menuRotate90;
		private System.Windows.Forms.MenuItem menuRotate180;
		private System.Windows.Forms.MenuItem menuRotate270;
		private System.Windows.Forms.MenuItem menuFlipVertical;
		private System.Windows.Forms.MenuItem menuMorphoOpen;
		private System.Windows.Forms.MenuItem menuMorphoClose;
		private System.Windows.Forms.MenuItem menuUndo;
		private System.Windows.Forms.MenuItem menuModifySep2;
		private System.EventHandler onPageMenuClick = null;
		private Atalasoft.Imaging.WinControls.WorkspaceViewer Viewer;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuPageSep3;
		private MenuItem pageMenus = null;
		private System.Windows.Forms.MenuItem menuInvert;
		private System.Windows.Forms.MenuItem menuInsertPage;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton tbOpen;
		private System.Windows.Forms.ToolBarButton tbSave;
		private System.Windows.Forms.ToolBarButton tbSep;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton tbArrow;
		private System.Windows.Forms.ToolBarButton tbSelectRectangle;
		private System.Windows.Forms.ToolBarButton tbSelectEllipse;
		private System.Windows.Forms.ToolBarButton tbPan;
		private System.Windows.Forms.ToolBarButton tbMagnifier;
		private System.Windows.Forms.ToolBarButton tbZoom;
		private System.Windows.Forms.ToolBarButton tbZoomSelection;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.ToolBarButton tbPrevious;
		private System.Windows.Forms.ToolBarButton tbNext;
		private Atalasoft.Imaging.WinControls.EllipseRubberband ellipseSelect;
		private Atalasoft.Imaging.WinControls.RectangleSelection rectangleSelect;
		private System.Windows.Forms.MenuItem menuRotateCustom;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.MenuItem mnuDeskew1;
		private System.Windows.Forms.MenuItem mnuDeskew2;
		private System.Windows.Forms.MenuItem mnuDeskew4;
		private System.Windows.Forms.MenuItem mnuDeskew10;
		private System.Windows.Forms.MenuItem menuTiffTags;
		private System.Windows.Forms.MenuItem menuAutoZoomNone;
		private System.Windows.Forms.MenuItem menuAutoZoomBestFit;
		private System.Windows.Forms.MenuItem menuAutoZoomBestFitShrink;
		private System.Windows.Forms.MenuItem menuAutoZoomFitWidth;
		private System.Windows.Forms.MenuItem menuAutoZoomFitHeight;
		private System.Windows.Forms.MenuItem menuMedian;
		private System.Windows.Forms.ComboBox cmbAutoZoom;
		private System.Windows.Forms.MenuItem menuDespeckle;
		private Atalasoft.Imaging.WinControls.ThumbnailView thumbnailView1;
		private Atalasoft.Imaging.WinControls.ImagePrintDocument imagePrintDocument1;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Windows.Forms.MenuItem menuPrint;
		private System.Windows.Forms.MenuItem menuThresholding;
		private System.Windows.Forms.MenuItem menuThresholdingAdaptive;
		private System.Windows.Forms.MenuItem menuThresholdingGlobal;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuBorderRemoval;
		private System.Windows.Forms.MenuItem menuBorderRemovalH;
		private System.Windows.Forms.MenuItem menuBorderRemovalV;
		private System.Windows.Forms.MenuItem menuBorderRemovalAll;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuThresholdingDynamic;
		private System.Windows.Forms.MenuItem menuThresholdingSimple;
		private System.Windows.Forms.MenuItem menuItem3;
		#endregion

		public Form1()
		{
			CheckLicenseFile();

			if (this._validLicense)
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				this.onPageMenuClick = new System.EventHandler(this.OnPageMenuClick);

				//display the correct checkmark in the antialias menu
				RemoveAntialiasCheckmarks();
				this.cmbAutoZoom.SelectedIndex = 0;
				menuAntialiasDisplay.MenuItems[(int)Viewer.AntialiasDisplay].Checked = true;
				Viewer.ImageBorderPen = new Atalasoft.Imaging.Drawing.AtalaPen(Color.Black, 1);

				//this.thumbnailView1.BackgroundThumb = new AtalaImage(79,102,PixelFormat.Pixel24bppBgr,Color.Black);
			
				// Create a temporary file for editing
				this.currentFile = System.IO.Path.GetTempFileName();
			}
		}

		#region Check for license code

		private void CheckLicenseFile()
		{
			try
			{
				AtalaImage img = new AtalaImage();
				img.Dispose();
			}
			catch (Atalasoft.Imaging.AtalasoftLicenseException ex)
			{
				LicenseCheckFailure("License validation failed:  " + ex.Message);
				return;
			}

			if (AtalaImage.Edition != LicenseEdition.Document)
			{
				LicenseCheckFailure("This demo requires a Document Imaging License.\r\nYour current license is for '" + AtalaImage.Edition.ToString() + "'.");
				return;
			}

			this._validLicense = true;
		}

		private void LicenseCheckFailure(string message)
		{
			this.Load += new System.EventHandler(this.Form1_Load);
			if (MessageBox.Show(this, message + "\r\n\r\nWould you like to request an evaluation license?", "License Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				// Locate the activation utility.
				string path = "";
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Atalasoft\dotImage\4.0");
				if (key != null)
				{
					path = Convert.ToString(key.GetValue("AssemblyBasePath"));
					if (path != null && path.Length > 5)
						path = path.Substring(0, path.Length - 3) + "AtalasoftToolkitActivation.exe";
					else
						path = Path.GetFullPath(@"..\..\..\..\..\AtalasoftToolkitActivation.exe");

					key.Close();
				}

				if (File.Exists(path))
					System.Diagnostics.Process.Start(path);
				else
					MessageBox.Show(this, "We were unable to location the DotImage activation utility.\r\nPlease run it from the Start menu shortcut.", "File Not Found");
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			if (!this._validLicense)
				Application.Exit();
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuOpen = new System.Windows.Forms.MenuItem();
			this.menuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuFileSep1 = new System.Windows.Forms.MenuItem();
			this.menuTiffCompression = new System.Windows.Forms.MenuItem();
			this.menuCompressNone = new System.Windows.Forms.MenuItem();
			this.menuCompressDeflate = new System.Windows.Forms.MenuItem();
			this.menuCompressGroup3 = new System.Windows.Forms.MenuItem();
			this.menuCompressGroup4 = new System.Windows.Forms.MenuItem();
			this.menuCompressJpeg = new System.Windows.Forms.MenuItem();
			this.menuCompressLzw = new System.Windows.Forms.MenuItem();
			this.menuCompressMac = new System.Windows.Forms.MenuItem();
			this.menuCompressHuffman = new System.Windows.Forms.MenuItem();
			this.menuPrint = new System.Windows.Forms.MenuItem();
			this.menuFileSep2 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuPages = new System.Windows.Forms.MenuItem();
			this.menuAntialiasDisplay = new System.Windows.Forms.MenuItem();
			this.menuAntiNone = new System.Windows.Forms.MenuItem();
			this.menuAntiGray = new System.Windows.Forms.MenuItem();
			this.menuAntiReduction = new System.Windows.Forms.MenuItem();
			this.menuAntiFull = new System.Windows.Forms.MenuItem();
			this.menuPageSep1 = new System.Windows.Forms.MenuItem();
			this.menuAddPage = new System.Windows.Forms.MenuItem();
			this.menuInsertPage = new System.Windows.Forms.MenuItem();
			this.menuRemovePage = new System.Windows.Forms.MenuItem();
			this.menuPageSep2 = new System.Windows.Forms.MenuItem();
			this.menuAppend = new System.Windows.Forms.MenuItem();
			this.menuPageSep3 = new System.Windows.Forms.MenuItem();
			this.menuTiffTags = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuModify = new System.Windows.Forms.MenuItem();
			this.menuUndo = new System.Windows.Forms.MenuItem();
			this.menuModifySep2 = new System.Windows.Forms.MenuItem();
			this.menuFlipHorizontal = new System.Windows.Forms.MenuItem();
			this.menuFlipVertical = new System.Windows.Forms.MenuItem();
			this.menuInvert = new System.Windows.Forms.MenuItem();
			this.menuRotate = new System.Windows.Forms.MenuItem();
			this.menuRotate90 = new System.Windows.Forms.MenuItem();
			this.menuRotate180 = new System.Windows.Forms.MenuItem();
			this.menuRotate270 = new System.Windows.Forms.MenuItem();
			this.menuRotateCustom = new System.Windows.Forms.MenuItem();
			this.menuModifySep1 = new System.Windows.Forms.MenuItem();
			this.menuAutoDeskew = new System.Windows.Forms.MenuItem();
			this.mnuDeskew1 = new System.Windows.Forms.MenuItem();
			this.mnuDeskew2 = new System.Windows.Forms.MenuItem();
			this.mnuDeskew4 = new System.Windows.Forms.MenuItem();
			this.mnuDeskew10 = new System.Windows.Forms.MenuItem();
			this.menuMorpho = new System.Windows.Forms.MenuItem();
			this.menuDilation = new System.Windows.Forms.MenuItem();
			this.menuErosion = new System.Windows.Forms.MenuItem();
			this.menuMorphoOpen = new System.Windows.Forms.MenuItem();
			this.menuMorphoClose = new System.Windows.Forms.MenuItem();
			this.menuBoundaryExtraction = new System.Windows.Forms.MenuItem();
			this.menuDespeckle = new System.Windows.Forms.MenuItem();
			this.menuMedian = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuThresholding = new System.Windows.Forms.MenuItem();
			this.menuThresholdingAdaptive = new System.Windows.Forms.MenuItem();
			this.menuThresholdingDynamic = new System.Windows.Forms.MenuItem();
			this.menuThresholdingGlobal = new System.Windows.Forms.MenuItem();
			this.menuThresholdingSimple = new System.Windows.Forms.MenuItem();
			this.menuBorderRemoval = new System.Windows.Forms.MenuItem();
			this.menuBorderRemovalH = new System.Windows.Forms.MenuItem();
			this.menuBorderRemovalV = new System.Windows.Forms.MenuItem();
			this.menuBorderRemovalAll = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusMessage = new System.Windows.Forms.StatusBarPanel();
			this.statusPage = new System.Windows.Forms.StatusBarPanel();
			this.Viewer = new Atalasoft.Imaging.WinControls.WorkspaceViewer();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.tbOpen = new System.Windows.Forms.ToolBarButton();
			this.tbSave = new System.Windows.Forms.ToolBarButton();
			this.tbSep = new System.Windows.Forms.ToolBarButton();
			this.tbPrevious = new System.Windows.Forms.ToolBarButton();
			this.tbNext = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.tbArrow = new System.Windows.Forms.ToolBarButton();
			this.tbSelectRectangle = new System.Windows.Forms.ToolBarButton();
			this.tbSelectEllipse = new System.Windows.Forms.ToolBarButton();
			this.tbPan = new System.Windows.Forms.ToolBarButton();
			this.tbMagnifier = new System.Windows.Forms.ToolBarButton();
			this.tbZoom = new System.Windows.Forms.ToolBarButton();
			this.tbZoomSelection = new System.Windows.Forms.ToolBarButton();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuAutoZoomNone = new System.Windows.Forms.MenuItem();
			this.menuAutoZoomBestFit = new System.Windows.Forms.MenuItem();
			this.menuAutoZoomBestFitShrink = new System.Windows.Forms.MenuItem();
			this.menuAutoZoomFitWidth = new System.Windows.Forms.MenuItem();
			this.menuAutoZoomFitHeight = new System.Windows.Forms.MenuItem();
			this.ellipseSelect = new Atalasoft.Imaging.WinControls.EllipseRubberband();
			this.rectangleSelect = new Atalasoft.Imaging.WinControls.RectangleSelection();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.cmbAutoZoom = new System.Windows.Forms.ComboBox();
			this.thumbnailView1 = new Atalasoft.Imaging.WinControls.ThumbnailView();
			this.imagePrintDocument1 = new Atalasoft.Imaging.WinControls.ImagePrintDocument();
			this.printDialog1 = new System.Windows.Forms.PrintDialog();
			((System.ComponentModel.ISupportInitialize)(this.statusMessage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusPage)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuFile,
																					  this.menuPages,
																					  this.menuModify,
																					  this.menuItem2});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuOpen,
																					 this.menuSaveAs,
																					 this.menuFileSep1,
																					 this.menuTiffCompression,
																					 this.menuPrint,
																					 this.menuFileSep2,
																					 this.menuExit});
			this.menuFile.Text = "&File";
			// 
			// menuOpen
			// 
			this.menuOpen.Index = 0;
			this.menuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuOpen.Text = "&Open";
			this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
			// 
			// menuSaveAs
			// 
			this.menuSaveAs.Index = 1;
			this.menuSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSaveAs.Text = "&Save As...";
			this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
			// 
			// menuFileSep1
			// 
			this.menuFileSep1.Index = 2;
			this.menuFileSep1.Text = "-";
			// 
			// menuTiffCompression
			// 
			this.menuTiffCompression.Index = 3;
			this.menuTiffCompression.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.menuCompressNone,
																								this.menuCompressDeflate,
																								this.menuCompressGroup3,
																								this.menuCompressGroup4,
																								this.menuCompressJpeg,
																								this.menuCompressLzw,
																								this.menuCompressMac,
																								this.menuCompressHuffman});
			this.menuTiffCompression.Text = "TIFF Compression";
			// 
			// menuCompressNone
			// 
			this.menuCompressNone.Index = 0;
			this.menuCompressNone.RadioCheck = true;
			this.menuCompressNone.Text = "None";
			this.menuCompressNone.Click += new System.EventHandler(this.menuCompressNone_Click);
			// 
			// menuCompressDeflate
			// 
			this.menuCompressDeflate.Index = 1;
			this.menuCompressDeflate.RadioCheck = true;
			this.menuCompressDeflate.Text = "Deflate";
			this.menuCompressDeflate.Click += new System.EventHandler(this.menuCompressDeflate_Click);
			// 
			// menuCompressGroup3
			// 
			this.menuCompressGroup3.Index = 2;
			this.menuCompressGroup3.RadioCheck = true;
			this.menuCompressGroup3.Text = "Group 3 Fax Encoding";
			this.menuCompressGroup3.Click += new System.EventHandler(this.menuCompressGroup3_Click);
			// 
			// menuCompressGroup4
			// 
			this.menuCompressGroup4.Checked = true;
			this.menuCompressGroup4.Index = 3;
			this.menuCompressGroup4.RadioCheck = true;
			this.menuCompressGroup4.Text = "Group 4 Fax Encoding";
			this.menuCompressGroup4.Click += new System.EventHandler(this.menuCompressGroup4_Click);
			// 
			// menuCompressJpeg
			// 
			this.menuCompressJpeg.Index = 4;
			this.menuCompressJpeg.RadioCheck = true;
			this.menuCompressJpeg.Text = "JPEG";
			this.menuCompressJpeg.Click += new System.EventHandler(this.menuCompressJpeg_Click);
			// 
			// menuCompressLzw
			// 
			this.menuCompressLzw.Index = 5;
			this.menuCompressLzw.RadioCheck = true;
			this.menuCompressLzw.Text = "LZW";
			this.menuCompressLzw.Click += new System.EventHandler(this.menuCompressLzw_Click);
			// 
			// menuCompressMac
			// 
			this.menuCompressMac.Index = 6;
			this.menuCompressMac.RadioCheck = true;
			this.menuCompressMac.Text = "Macintosh Packbits";
			this.menuCompressMac.Click += new System.EventHandler(this.menuCompressMac_Click);
			// 
			// menuCompressHuffman
			// 
			this.menuCompressHuffman.Index = 7;
			this.menuCompressHuffman.RadioCheck = true;
			this.menuCompressHuffman.Text = "Modified Huffman";
			this.menuCompressHuffman.Click += new System.EventHandler(this.menuCompressHuffman_Click);
			// 
			// menuPrint
			// 
			this.menuPrint.Index = 4;
			this.menuPrint.Text = "Print";
			this.menuPrint.Click += new System.EventHandler(this.menuPrint_Click);
			// 
			// menuFileSep2
			// 
			this.menuFileSep2.Index = 5;
			this.menuFileSep2.Text = "-";
			// 
			// menuExit
			// 
			this.menuExit.Index = 6;
			this.menuExit.Text = "E&xit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuPages
			// 
			this.menuPages.Index = 1;
			this.menuPages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuAntialiasDisplay,
																					  this.menuPageSep1,
																					  this.menuAddPage,
																					  this.menuInsertPage,
																					  this.menuRemovePage,
																					  this.menuPageSep2,
																					  this.menuAppend,
																					  this.menuPageSep3,
																					  this.menuTiffTags,
																					  this.menuItem3});
			this.menuPages.Text = "&Pages";
			// 
			// menuAntialiasDisplay
			// 
			this.menuAntialiasDisplay.Index = 0;
			this.menuAntialiasDisplay.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuAntiNone,
																								 this.menuAntiGray,
																								 this.menuAntiReduction,
																								 this.menuAntiFull});
			this.menuAntialiasDisplay.Text = "Antialias Display";
			// 
			// menuAntiNone
			// 
			this.menuAntiNone.Checked = true;
			this.menuAntiNone.Index = 0;
			this.menuAntiNone.RadioCheck = true;
			this.menuAntiNone.Text = "None";
			this.menuAntiNone.Click += new System.EventHandler(this.menuAnti_Click);
			// 
			// menuAntiGray
			// 
			this.menuAntiGray.Index = 1;
			this.menuAntiGray.RadioCheck = true;
			this.menuAntiGray.Text = "Scale To Gray";
			this.menuAntiGray.Click += new System.EventHandler(this.menuAnti_Click);
			// 
			// menuAntiReduction
			// 
			this.menuAntiReduction.Index = 2;
			this.menuAntiReduction.RadioCheck = true;
			this.menuAntiReduction.Text = "Reduction Only";
			this.menuAntiReduction.Click += new System.EventHandler(this.menuAnti_Click);
			// 
			// menuAntiFull
			// 
			this.menuAntiFull.Index = 3;
			this.menuAntiFull.RadioCheck = true;
			this.menuAntiFull.Text = "Full";
			this.menuAntiFull.Click += new System.EventHandler(this.menuAnti_Click);
			// 
			// menuPageSep1
			// 
			this.menuPageSep1.Index = 1;
			this.menuPageSep1.Text = "-";
			// 
			// menuAddPage
			// 
			this.menuAddPage.Index = 2;
			this.menuAddPage.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.menuAddPage.Text = "&Add...";
			this.menuAddPage.Click += new System.EventHandler(this.menuAddPage_Click);
			// 
			// menuInsertPage
			// 
			this.menuInsertPage.Index = 3;
			this.menuInsertPage.Text = "&Insert...";
			this.menuInsertPage.Click += new System.EventHandler(this.menuInsertPage_Click);
			// 
			// menuRemovePage
			// 
			this.menuRemovePage.Index = 4;
			this.menuRemovePage.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuRemovePage.Text = "&Remove";
			this.menuRemovePage.Click += new System.EventHandler(this.menuRemovePage_Click);
			// 
			// menuPageSep2
			// 
			this.menuPageSep2.Index = 5;
			this.menuPageSep2.Text = "-";
			// 
			// menuAppend
			// 
			this.menuAppend.Index = 6;
			this.menuAppend.Text = "Append to file...";
			this.menuAppend.Click += new System.EventHandler(this.menuAppend_Click);
			// 
			// menuPageSep3
			// 
			this.menuPageSep3.Index = 7;
			this.menuPageSep3.Text = "-";
			// 
			// menuTiffTags
			// 
			this.menuTiffTags.Index = 8;
			this.menuTiffTags.Text = "View TIFF Tags";
			this.menuTiffTags.Click += new System.EventHandler(this.menuTiffTags_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 9;
			this.menuItem3.Text = "-";
			// 
			// menuModify
			// 
			this.menuModify.Index = 2;
			this.menuModify.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuUndo,
																					   this.menuModifySep2,
																					   this.menuFlipHorizontal,
																					   this.menuFlipVertical,
																					   this.menuInvert,
																					   this.menuRotate,
																					   this.menuModifySep1,
																					   this.menuAutoDeskew,
																					   this.menuMorpho,
																					   this.menuDespeckle,
																					   this.menuMedian,
																					   this.menuItem1,
																					   this.menuThresholding,
																					   this.menuBorderRemoval});
			this.menuModify.Text = "&Modify";
			// 
			// menuUndo
			// 
			this.menuUndo.Index = 0;
			this.menuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuUndo.Text = "&Undo";
			this.menuUndo.Click += new System.EventHandler(this.menuUndo_Click);
			// 
			// menuModifySep2
			// 
			this.menuModifySep2.Index = 1;
			this.menuModifySep2.Text = "-";
			// 
			// menuFlipHorizontal
			// 
			this.menuFlipHorizontal.Index = 2;
			this.menuFlipHorizontal.Text = "&Flip Horizontal";
			this.menuFlipHorizontal.Click += new System.EventHandler(this.menuFlipHorizontal_Click);
			// 
			// menuFlipVertical
			// 
			this.menuFlipVertical.Index = 3;
			this.menuFlipVertical.Text = "Flip &Vertical";
			this.menuFlipVertical.Click += new System.EventHandler(this.menuFlipVertical_Click);
			// 
			// menuInvert
			// 
			this.menuInvert.Index = 4;
			this.menuInvert.Text = "Invert";
			this.menuInvert.Click += new System.EventHandler(this.menuInvert_Click);
			// 
			// menuRotate
			// 
			this.menuRotate.Index = 5;
			this.menuRotate.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuRotate90,
																					   this.menuRotate180,
																					   this.menuRotate270,
																					   this.menuRotateCustom});
			this.menuRotate.Text = "&Rotate";
			// 
			// menuRotate90
			// 
			this.menuRotate90.Index = 0;
			this.menuRotate90.Text = "90 Degrees";
			this.menuRotate90.Click += new System.EventHandler(this.menuRotate90_Click);
			// 
			// menuRotate180
			// 
			this.menuRotate180.Index = 1;
			this.menuRotate180.Text = "180 Degrees";
			this.menuRotate180.Click += new System.EventHandler(this.menuRotate180_Click);
			// 
			// menuRotate270
			// 
			this.menuRotate270.Index = 2;
			this.menuRotate270.Text = "270 Degrees";
			this.menuRotate270.Click += new System.EventHandler(this.menuRotate270_Click);
			// 
			// menuRotateCustom
			// 
			this.menuRotateCustom.Index = 3;
			this.menuRotateCustom.Text = "Custom";
			this.menuRotateCustom.Click += new System.EventHandler(this.menuRotateCustom_Click);
			// 
			// menuModifySep1
			// 
			this.menuModifySep1.Index = 6;
			this.menuModifySep1.Text = "-";
			// 
			// menuAutoDeskew
			// 
			this.menuAutoDeskew.Index = 7;
			this.menuAutoDeskew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuDeskew1,
																						   this.mnuDeskew2,
																						   this.mnuDeskew4,
																						   this.mnuDeskew10});
			this.menuAutoDeskew.Text = "Binary Auto Deskew";
			// 
			// mnuDeskew1
			// 
			this.mnuDeskew1.Index = 0;
			this.mnuDeskew1.Text = "1 degree Precision";
			this.mnuDeskew1.Click += new System.EventHandler(this.mnuDeskew_Click);
			// 
			// mnuDeskew2
			// 
			this.mnuDeskew2.Index = 1;
			this.mnuDeskew2.Text = "0.5 degree Precision";
			this.mnuDeskew2.Click += new System.EventHandler(this.mnuDeskew_Click);
			// 
			// mnuDeskew4
			// 
			this.mnuDeskew4.Index = 2;
			this.mnuDeskew4.Text = "0.25 degree Precision";
			this.mnuDeskew4.Click += new System.EventHandler(this.mnuDeskew_Click);
			// 
			// mnuDeskew10
			// 
			this.mnuDeskew10.Index = 3;
			this.mnuDeskew10.Text = "0.1 degree Precision";
			this.mnuDeskew10.Click += new System.EventHandler(this.mnuDeskew_Click);
			// 
			// menuMorpho
			// 
			this.menuMorpho.Index = 8;
			this.menuMorpho.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuDilation,
																					   this.menuErosion,
																					   this.menuMorphoOpen,
																					   this.menuMorphoClose,
																					   this.menuBoundaryExtraction});
			this.menuMorpho.Text = "Binary Morphological Filters";
			// 
			// menuDilation
			// 
			this.menuDilation.Index = 0;
			this.menuDilation.Text = "Dilation";
			this.menuDilation.Click += new System.EventHandler(this.menuMorpho_Click);
			// 
			// menuErosion
			// 
			this.menuErosion.Index = 1;
			this.menuErosion.Text = "Erosion";
			this.menuErosion.Click += new System.EventHandler(this.menuMorpho_Click);
			// 
			// menuMorphoOpen
			// 
			this.menuMorphoOpen.Index = 2;
			this.menuMorphoOpen.Text = "Open";
			this.menuMorphoOpen.Click += new System.EventHandler(this.menuMorpho_Click);
			// 
			// menuMorphoClose
			// 
			this.menuMorphoClose.Index = 3;
			this.menuMorphoClose.Text = "Close";
			this.menuMorphoClose.Click += new System.EventHandler(this.menuMorpho_Click);
			// 
			// menuBoundaryExtraction
			// 
			this.menuBoundaryExtraction.Index = 4;
			this.menuBoundaryExtraction.Text = "Boundary Extraction";
			this.menuBoundaryExtraction.Click += new System.EventHandler(this.menuMorpho_Click);
			// 
			// menuDespeckle
			// 
			this.menuDespeckle.Index = 9;
			this.menuDespeckle.Text = "Binary Despeckle";
			this.menuDespeckle.Click += new System.EventHandler(this.menuDespeckle_Click);
			// 
			// menuMedian
			// 
			this.menuMedian.Index = 10;
			this.menuMedian.Text = "Binary Remove Noise";
			this.menuMedian.Click += new System.EventHandler(this.menuMedian_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 11;
			this.menuItem1.Text = "-";
			// 
			// menuThresholding
			// 
			this.menuThresholding.Index = 12;
			this.menuThresholding.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.menuThresholdingAdaptive,
																							 this.menuThresholdingDynamic,
																							 this.menuThresholdingGlobal,
																							 this.menuThresholdingSimple});
			this.menuThresholding.Text = "Thresholding";
			// 
			// menuThresholdingAdaptive
			// 
			this.menuThresholdingAdaptive.Index = 0;
			this.menuThresholdingAdaptive.Text = "Adaptive";
			this.menuThresholdingAdaptive.Click += new System.EventHandler(this.menuThresholdingAdaptive_Click);
			// 
			// menuThresholdingDynamic
			// 
			this.menuThresholdingDynamic.Index = 1;
			this.menuThresholdingDynamic.Text = "Dynamic";
			this.menuThresholdingDynamic.Click += new System.EventHandler(this.menuThresholdingDynamic_Click);
			// 
			// menuThresholdingGlobal
			// 
			this.menuThresholdingGlobal.Index = 2;
			this.menuThresholdingGlobal.Text = "Global";
			this.menuThresholdingGlobal.Click += new System.EventHandler(this.menuThresholdingGlobal_Click);
			// 
			// menuThresholdingSimple
			// 
			this.menuThresholdingSimple.Index = 3;
			this.menuThresholdingSimple.Text = "Simple";
			this.menuThresholdingSimple.Click += new System.EventHandler(this.menuThresholdingSimple_Click);
			// 
			// menuBorderRemoval
			// 
			this.menuBorderRemoval.Index = 13;
			this.menuBorderRemoval.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuBorderRemovalH,
																							  this.menuBorderRemovalV,
																							  this.menuBorderRemovalAll});
			this.menuBorderRemoval.Text = "Border Removal";
			// 
			// menuBorderRemovalH
			// 
			this.menuBorderRemovalH.Index = 0;
			this.menuBorderRemovalH.Text = "Horizontal";
			this.menuBorderRemovalH.Click += new System.EventHandler(this.menuBorderRemovalH_Click);
			// 
			// menuBorderRemovalV
			// 
			this.menuBorderRemovalV.Index = 1;
			this.menuBorderRemovalV.Text = "Vertical";
			this.menuBorderRemovalV.Click += new System.EventHandler(this.menuBorderRemovalV_Click);
			// 
			// menuBorderRemovalAll
			// 
			this.menuBorderRemovalAll.Index = 2;
			this.menuBorderRemovalAll.Text = "All Sides";
			this.menuBorderRemovalAll.Click += new System.EventHandler(this.menuBorderRemovalAll_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuAbout});
			this.menuItem2.Text = "&Help";
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 0;
			this.menuAbout.Text = "About ...";
			this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 395);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusMessage,
																						  this.statusPage});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(456, 22);
			this.statusBar1.TabIndex = 1;
			// 
			// statusMessage
			// 
			this.statusMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusMessage.Text = "dotImage Multipage TIFF Demo";
			this.statusMessage.Width = 320;
			// 
			// statusPage
			// 
			this.statusPage.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.statusPage.MinWidth = 120;
			this.statusPage.Text = "Page 0  of 0";
			this.statusPage.Width = 120;
			// 
			// Viewer
			// 
			this.Viewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Viewer.AntialiasDisplay = Atalasoft.Imaging.WinControls.AntialiasDisplayMode.ScaleToGray;
			this.Viewer.DisplayProfile = null;
			this.Viewer.Location = new System.Drawing.Point(160, 24);
			this.Viewer.Magnifier.BackColor = System.Drawing.Color.White;
			this.Viewer.Magnifier.BorderColor = System.Drawing.Color.Black;
			this.Viewer.Magnifier.Size = new System.Drawing.Size(100, 100);
			this.Viewer.Name = "Viewer";
			this.Viewer.OutputProfile = null;
			this.Viewer.Selection = null;
			this.Viewer.Size = new System.Drawing.Size(295, 366);
			this.Viewer.TabIndex = 7;
			this.Viewer.UndoLevels = 3;
			this.Viewer.Progress += new Atalasoft.Imaging.ProgressEventHandler(this.Viewer_Progress);
			this.Viewer.ImageChanged += new Atalasoft.Imaging.ImageEventHandler(this.Viewer_ChangedImage);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.AutoSize = false;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.tbOpen,
																						this.tbSave,
																						this.tbSep,
																						this.tbPrevious,
																						this.tbNext,
																						this.toolBarButton1,
																						this.tbArrow,
																						this.tbSelectRectangle,
																						this.tbSelectEllipse,
																						this.tbPan,
																						this.tbMagnifier,
																						this.tbZoom,
																						this.tbZoomSelection});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.imageList1;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(456, 24);
			this.toolBar1.TabIndex = 3;
			this.toolBar1.Wrappable = false;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// tbOpen
			// 
			this.tbOpen.ImageIndex = 3;
			this.tbOpen.ToolTipText = "Open";
			// 
			// tbSave
			// 
			this.tbSave.ImageIndex = 4;
			this.tbSave.ToolTipText = "Save";
			// 
			// tbSep
			// 
			this.tbSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbPrevious
			// 
			this.tbPrevious.Enabled = false;
			this.tbPrevious.ImageIndex = 11;
			this.tbPrevious.ToolTipText = "Previous";
			// 
			// tbNext
			// 
			this.tbNext.Enabled = false;
			this.tbNext.ImageIndex = 12;
			this.tbNext.ToolTipText = "Next";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbArrow
			// 
			this.tbArrow.ImageIndex = 0;
			this.tbArrow.Pushed = true;
			this.tbArrow.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbArrow.ToolTipText = "Arrow";
			// 
			// tbSelectRectangle
			// 
			this.tbSelectRectangle.ImageIndex = 1;
			this.tbSelectRectangle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbSelectRectangle.ToolTipText = "Rectangle Selection";
			// 
			// tbSelectEllipse
			// 
			this.tbSelectEllipse.ImageIndex = 10;
			this.tbSelectEllipse.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbSelectEllipse.ToolTipText = "Ellipse Selection";
			// 
			// tbPan
			// 
			this.tbPan.ImageIndex = 2;
			this.tbPan.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbPan.ToolTipText = "Pan";
			// 
			// tbMagnifier
			// 
			this.tbMagnifier.ImageIndex = 6;
			this.tbMagnifier.ToolTipText = "Magnifier";
			// 
			// tbZoom
			// 
			this.tbZoom.ImageIndex = 7;
			this.tbZoom.ToolTipText = "Zoom";
			// 
			// tbZoomSelection
			// 
			this.tbZoomSelection.ImageIndex = 8;
			this.tbZoomSelection.ToolTipText = "Zoom Selection";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAutoZoomNone,
																						 this.menuAutoZoomBestFit,
																						 this.menuAutoZoomBestFitShrink,
																						 this.menuAutoZoomFitWidth,
																						 this.menuAutoZoomFitHeight});
			// 
			// menuAutoZoomNone
			// 
			this.menuAutoZoomNone.Index = 0;
			this.menuAutoZoomNone.Text = "None";
			this.menuAutoZoomNone.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// menuAutoZoomBestFit
			// 
			this.menuAutoZoomBestFit.Index = 1;
			this.menuAutoZoomBestFit.Text = "Best Fit";
			this.menuAutoZoomBestFit.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// menuAutoZoomBestFitShrink
			// 
			this.menuAutoZoomBestFitShrink.Index = 2;
			this.menuAutoZoomBestFitShrink.Text = "Best Fit Shrink Only";
			this.menuAutoZoomBestFitShrink.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// menuAutoZoomFitWidth
			// 
			this.menuAutoZoomFitWidth.Index = 3;
			this.menuAutoZoomFitWidth.Text = "Fit To Width";
			this.menuAutoZoomFitWidth.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// menuAutoZoomFitHeight
			// 
			this.menuAutoZoomFitHeight.Index = 4;
			this.menuAutoZoomFitHeight.Text = "Fit To Height";
			this.menuAutoZoomFitHeight.Click += new System.EventHandler(this.mnuAutoZoom_Click);
			// 
			// ellipseSelect
			// 
			this.ellipseSelect.ActiveButtons = System.Windows.Forms.MouseButtons.Left;
			this.ellipseSelect.AspectRatio = 0F;
			this.ellipseSelect.BackgroundColor = System.Drawing.Color.Transparent;
			this.ellipseSelect.ConstrainPosition = false;
			this.ellipseSelect.Fill = null;
			this.ellipseSelect.MoveCursor = System.Windows.Forms.Cursors.SizeAll;
			this.ellipseSelect.Parent = this.Viewer;
			this.ellipseSelect.Pen.Color = System.Drawing.Color.Black;
			this.ellipseSelect.Pen.CustomDashPattern = new int[] {
																	 8,
																	 8};
			this.ellipseSelect.Persist = true;
			this.ellipseSelect.SnapToPixelGrid = false;
			// 
			// rectangleSelect
			// 
			this.rectangleSelect.ActiveButtons = System.Windows.Forms.MouseButtons.Left;
			this.rectangleSelect.Animated = true;
			this.rectangleSelect.AspectRatio = 0F;
			this.rectangleSelect.BackgroundColor = System.Drawing.Color.White;
			this.rectangleSelect.Inverted = false;
			this.rectangleSelect.MoveCursor = System.Windows.Forms.Cursors.SizeAll;
			this.rectangleSelect.Parent = this.Viewer;
			this.rectangleSelect.Pen.Color = System.Drawing.Color.Black;
			this.rectangleSelect.Pen.CustomDashPattern = new int[] {
																	   8,
																	   8};
			this.rectangleSelect.Pen.LineStyle = Atalasoft.Imaging.Drawing.LineStyle.Custom;
			this.rectangleSelect.Persist = true;
			this.rectangleSelect.SelectionNESWCursor = System.Windows.Forms.Cursors.SizeNESW;
			this.rectangleSelect.SelectionNSCursor = System.Windows.Forms.Cursors.SizeNS;
			this.rectangleSelect.SelectionNWSECursor = System.Windows.Forms.Cursors.SizeNWSE;
			this.rectangleSelect.SelectionWECursor = System.Windows.Forms.Cursors.SizeWE;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(8, 400);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(304, 16);
			this.progressBar1.TabIndex = 5;
			// 
			// cmbAutoZoom
			// 
			this.cmbAutoZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAutoZoom.Items.AddRange(new object[] {
															 "None",
															 "Best Fit",
															 "Best Fit Shrink Only",
															 "Fit to Width",
															 "Fit to Height"});
			this.cmbAutoZoom.Location = new System.Drawing.Point(280, 2);
			this.cmbAutoZoom.Name = "cmbAutoZoom";
			this.cmbAutoZoom.Size = new System.Drawing.Size(121, 21);
			this.cmbAutoZoom.TabIndex = 6;
			this.cmbAutoZoom.SelectedIndexChanged += new System.EventHandler(this.cmbAutoZoom_SelectedIndexChanged);
			// 
			// thumbnailView1
			// 
			this.thumbnailView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.thumbnailView1.AutoDragDrop = true;
			this.thumbnailView1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.thumbnailView1.CaptionLines = 0;
			this.thumbnailView1.DisplayText = Atalasoft.Imaging.WinControls.ThumbViewAttribute.None;
			this.thumbnailView1.DragSelectionColor = System.Drawing.Color.Red;
			this.thumbnailView1.DropPositionIndicator = Atalasoft.Imaging.WinControls.ThumbnailDropPositionIndicator.IBeam;
			this.thumbnailView1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.thumbnailView1.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
			this.thumbnailView1.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
			this.thumbnailView1.LoadErrorMessage = "";
			this.thumbnailView1.LoadMethod = Atalasoft.Imaging.WinControls.ThumbLoadMethod.EntireFolder;
			this.thumbnailView1.Location = new System.Drawing.Point(0, 24);
			this.thumbnailView1.Margins = new Atalasoft.Imaging.WinControls.Margin(4, 4, 4, 4);
			this.thumbnailView1.SelectionMode = Atalasoft.Imaging.WinControls.ThumbnailSelectionMode.MultiSelect;
			this.thumbnailView1.Name = "thumbnailView1";
			this.thumbnailView1.SelectionRectangleBackColor = System.Drawing.Color.Transparent;
			this.thumbnailView1.SelectionRectangleDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			this.thumbnailView1.SelectionRectangleLineColor = System.Drawing.Color.Black;
			this.thumbnailView1.Size = new System.Drawing.Size(152, 366);
			this.thumbnailView1.TabIndex = 0;
			this.thumbnailView1.ThumbnailBackground = null;
			this.thumbnailView1.ThumbnailOffset = new System.Drawing.Point(0, 0);
			this.thumbnailView1.ThumbnailSize = new System.Drawing.Size(100, 100);
			this.thumbnailView1.ThumbnailDrop += new Atalasoft.Imaging.WinControls.ThumbnailDropEventHandler(this.thumbnailView1_ThumbnailDrop);
			this.thumbnailView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.thumbnailView1_ItemDrag);
			this.thumbnailView1.SelectedIndexChanged += new System.EventHandler(this.thumbnailView1_SelectedIndexChanged);
			// 
			// imagePrintDocument1
			// 
			this.imagePrintDocument1.ScaleMode = Atalasoft.Imaging.WinControls.PrintScaleMode.None;
			this.imagePrintDocument1.GetImage += new Atalasoft.Imaging.WinControls.PrintImageEventHandler(this.imagePrintDocument1_GetImage);
			// 
			// printDialog1
			// 
			this.printDialog1.Document = this.imagePrintDocument1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 417);
			this.Controls.Add(this.thumbnailView1);
			this.Controls.Add(this.cmbAutoZoom);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.Viewer);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Atalasoft dotImage Multipage TIFF Demo";
			((System.ComponentModel.ISupportInitialize)(this.statusMessage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusPage)).EndInit();
			this.ResumeLayout(false);

		}
		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		#endregion
		
		#region File Menu Code
		
		private void menuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuOpen_Click(object sender, System.EventArgs e)
		{
			OpenImage();
		}

		private void OpenImage()
		{
			// Display the open file dialog.
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "TIFF Images|*.tif;*.tiff";
			
			// try to locate images folder
			string imagesFolder = Application.ExecutablePath;
			// we assume we are running under the DotImage install folder
			int pos = imagesFolder.IndexOf("DotImage ");
			if (pos != -1)
			{
				imagesFolder = imagesFolder.Substring(0,imagesFolder.IndexOf(@"\",pos)) + @"\Images";
			}

			//use this folder as starting point			
			openFile.InitialDirectory = imagesFolder;

			if (openFile.ShowDialog() == DialogResult.OK) 
			{
				try
				{
					LoadPages(openFile.FileName);
				}			
				catch(Exception err)
				{
					MessageBox.Show(this, err.Message);
				}
			}
			openFile.Dispose();
		}

		private void LoadPages(string fileName)
		{
			//Copy image to temp file for instert/delete/remove functions.
			System.IO.File.Copy(fileName, this.currentFile, true);
			// use the temp file as the current file, so we don't change the opened file
			// until it is saved.

			//clear the thumb list
			this.thumbnailView1.Items.Clear();
			this.currentPage = 1;
			
			// Use the TiffDecoder for more control and information.
			TiffDecoder decoder = new TiffDecoder();
				
			using (AtalaFileStream fs = new AtalaFileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				if (fs == null) 
				{
					MessageBox.Show("We were unable to open the file.");
					return;
				}
				
				// Check to see if the file is a valid TIFF.
				if (decoder.IsValidFormat(fs) == false) 
				{
					MessageBox.Show("This is not a valid TIFF image.");
					fs.Close();
					return;
				}
				
				// Seek back to the beginning of the file.
				fs.Seek(0, SeekOrigin.Begin);
				
				// Get the page count.
				this.pageCount = decoder.GetFrameCount(fs);
				
				fs.Seek(0, SeekOrigin.Begin);
				// Load thumbs asynchronously and open first page in viewer.
				this.thumbnailView1.Asynchronous = true;
				for (int i = 0; i < this.pageCount; i++)
				{
					this.thumbnailView1.Items.Add(fileName, i, "Page " + (i + 1));
					if (i == 0)
					{
						this.thumbnailView1.Items[i].Selected = true;
						// Setting the thumbnail Selected property raises the
						// SelectedIndexChanged event.  We load the image there.
						//Viewer.Image = new AtalaImage(fs, i, null);
					}
				}
				fs.Close();
			}
			// Setup the Page menu.
			ResetPageMenuItems();
		}

		private void menuSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveImage();
		}

		private void SaveImage()
		{
			if(this.imageChanged) // save this image first
			{
				// open the file for edit
				string localTempFile = System.IO.Path.GetTempFileName();
				
				TiffFile editFile = new TiffFile();
				using (FileStream fs = new FileStream(this.currentFile, FileMode.Open, FileAccess.ReadWrite))
				{
					editFile.Read(fs);

					editFile.Images[this.currentPage-1] = new TiffDirectory(Viewer.Image, new TiffEncoder());
					using (FileStream tempFS = new FileStream(localTempFile, FileMode.Open, FileAccess.ReadWrite))
					{
						editFile.Save(tempFS);
					}
				}

				File.Copy(localTempFile,this.currentFile, true);
				this.imageChanged = false;
			}

			// Display the save editFile dialog.
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "TIFF Images|*.tif;*.tiff";
			saveFile.DefaultExt = "tif";
			saveFile.AddExtension = true;
			
			if (saveFile.ShowDialog() == DialogResult.OK) 
			{				
				// Copy from temp file to save file
				System.IO.File.Copy(this.currentFile,saveFile.FileName, true);
				this.statusMessage.Text = this.currentFile;
			}
			saveFile.Dispose();
		}
		
		//this code will get the frame index, either from the file, or from in memory
		private void imagePrintDocument1_GetImage(object sender, Atalasoft.Imaging.WinControls.PrintImageEventArgs e)
		{
			//only one image is loaded in memory at a time
			AtalaFileStream fs = null;  
			try
			{
				if (this.currentPage - 1 != e.ImageIndex)
				{
					//load the frame from a file
					fs = new AtalaFileStream(this.currentFile, FileMode.Open, FileAccess.Read);
					AtalaImage image = new AtalaImage(fs, e.ImageIndex, null);
					fs.Close();
					e.Image = image;
				}
				else
				{
					//can use the image in the viewer
					e.Image = Viewer.Image;
				}
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			//tell the print controller to print more images (or not)
			if (e.ImageIndex == this.pageCount - 1)
				e.HasMorePages = false;
			else
				e.HasMorePages = true;
		}

		private void menuPrint_Click(object sender, System.EventArgs e)
		{
			// Allow for a page range.
			this.printDialog1.AllowSomePages = true;
			this.printDialog1.PrinterSettings.FromPage = 1;
			this.printDialog1.PrinterSettings.ToPage = this.pageCount;
			this.printDialog1.PrinterSettings.MinimumPage = 1;
			this.printDialog1.PrinterSettings.MaximumPage = this.pageCount;

			if (this.printDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.imagePrintDocument1.ScaleMode = PrintScaleMode.FitToEdges;
				this.imagePrintDocument1.Print();
			}
		}

		#endregion
		
		#region AntialiasDisplayMode
		
		private void RemoveAntialiasCheckmarks()
		{
			this.menuAntiNone.Checked = false;
			this.menuAntiFull.Checked = false;
			this.menuAntiGray.Checked = false;
			this.menuAntiReduction.Checked = false;
		}
		
		private void menuAnti_Click(object sender, System.EventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			Atalasoft.Imaging.WinControls.AntialiasDisplayMode display = (Atalasoft.Imaging.WinControls.AntialiasDisplayMode)item.Index;
			Viewer.AntialiasDisplay = display;
			RemoveAntialiasCheckmarks();
			menuAntialiasDisplay.MenuItems[(int)display].Checked = true;
		}

		#endregion
		
		#region Pages Code
		
		// Rebuild the Pages menu.
		private void ResetPageMenuItems()
		{
			// Remove the current menu items.
			if (this.pageMenus != null) 
			{
				menuPages.MenuItems.Remove(this.pageMenus);
				this.pageMenus.MenuItems.Clear();
			}
			else
				this.pageMenus = new MenuItem("Current Page");
			
			// Add up to maxPageMenus menu items.
			int topItem = (this.pageCount > this.maxPageMenus ? this.maxPageMenus : this.pageCount);
			
			MenuItem item = null;
			for (int i = 1; i <= topItem; i++) {
				item = this.pageMenus.MenuItems.Add("Page " + i.ToString(), this.onPageMenuClick);
				item.RadioCheck = true;
			}
			
			// Check the first page.
			this.pageMenus.MenuItems[0].Checked = true;
			
			// Add the menu only if there is more than 1 page.
			if (topItem > 1)
				this.menuPages.MenuItems.Add(this.pageMenus);
		}
		
		// Update the radio checkmarks.
		private void SetPageMenu(int index)
		{
            if (pageMenus == null || pageMenus.MenuItems == null)
                return;

			if (this.pageMenus.MenuItems.Count < 1)
				return;
			
			for (int i = 0; i < this.pageMenus.MenuItems.Count; i++) {
				this.pageMenus.MenuItems[i].Checked = false;
			}
			
			this.pageMenus.MenuItems[index].Checked = true;
			
		}
		
		// Remove one menu item and rename the text.
		private void RemovePageMenu(int index)
		{
			if (this.pageMenus.MenuItems.Count < 1)
				return;
			
			// Because the page menu handler uses the menu text,
			// all we have to do is remove the last menu item.
			this.pageMenus.MenuItems.RemoveAt(this.pageCount - 1);
			
			// Remove the menu if there is only 1 item.
			if (this.pageMenus.MenuItems.Count < 2) {
				this.menuPages.MenuItems.Remove(this.pageMenus);
				this.pageMenus.MenuItems.Clear();
			}
		}
		
		// Event handler for the Pages menu.
		private void OnPageMenuClick(object sender, System.EventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			
			// Get the page to load.
			// Remember, the pages are zero based.
			int page = item.Index;

			// Update the statusbar.
			this.currentPage = page + 1;
			
			// Since we are loading one page at a time, replace the current page.
			Viewer.Images.Clear();
			Viewer.Open(this.currentFile, page);
            if (this.thumbnailView1.MultiSelect)
            {
                this.thumbnailView1.ClearSelection();
            }
			this.thumbnailView1.Items[page].Selected = true;
			this.thumbnailView1.EnsureVisible(page);
			
			// Update the menu.
			SetPageMenu(page);

		}
		
		private void menuAddPage_Click(object sender, System.EventArgs e)
		{
			// Add another page to the ImageCollection.
			TiffFile editFile = null;
			FileStream fs = null;
			FileStream tempFS = null;
			string localTempFile = System.IO.Path.GetTempFileName();
			try
			{
				fs = new FileStream(this.currentFile, FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				tempFS = new FileStream(localTempFile,FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				editFile = new TiffFile();
				editFile.Read(fs);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message);
				if (fs != null)
					fs.Close();
				if (tempFS != null)
					tempFS.Close();
				return;
			}
			
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "TIFF Images|*.tif;*.tiff";
			
			if (openFile.ShowDialog() == DialogResult.OK) 
			{
				AtalaImage image = new AtalaImage(openFile.FileName);
				if (image != null) 
				{
					//this.Viewer.Images.Add(image);
					editFile.Images.Add(new TiffDirectory(image));
					fs.Seek(0,SeekOrigin.Begin);
					editFile.Save(tempFS);

					this.pageCount++;
					this.currentPage = this.pageCount;
					
					this.thumbnailView1.Items.Add(image, "Page " + this.pageCount.ToString());
					
					// Make sure the menu has been added.
					if (this.pageMenus == null || this.pageCount == 2)
						ResetPageMenuItems();
					else
						// Add the menu item.
						if (this.pageCount <= this.maxPageMenus)
						this.pageMenus.MenuItems.Add("Page " + this.pageCount.ToString(), onPageMenuClick);
					
					fs.Close();
					tempFS.Close();

					// copy temp file
					System.IO.File.Copy(localTempFile,this.currentFile, true);

					// We want to view the image we just added.
					this.thumbnailView1.Items[this.currentPage - 1].Selected = true;
					//Viewer.Open(this.currentFile, this.currentPage - 1);
					
					// Update the Pages menu selection.
					SetPageMenu(this.currentPage - 1);


				}
				
			}
			
			openFile.Dispose();

			// Update the statusbar.
			this.statusPage.Text = "Page " + this.currentPage.ToString() + " of " + this.pageCount.ToString();
		}

		private void menuRemovePage_Click(object sender, System.EventArgs e)
		{
			// Remove the current page.
			FileStream fs= null;
			FileStream tempFS = null;
			TiffFile editFile = null;
			string localTempFile = System.IO.Path.GetTempFileName();
			try
			{
				// remove from temp image
				fs = new FileStream(this.currentFile, FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				tempFS = new FileStream(localTempFile,FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				editFile = new TiffFile();
				editFile.Read(fs);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message);
				if (fs != null)
					fs.Close();
				if (tempFS != null)
					fs.Close();
				return;
			}

			
			editFile.Images.RemoveAt(this.currentPage - 1);
			// don't save it if the last page was deleted
			if (editFile.Images.Count > 0)
				editFile.Save(tempFS);

			fs.Close();
			tempFS.Close();
			// copy from temp stream.
			System.IO.File.Copy(localTempFile,this.currentFile, true);

			this.thumbnailView1.Items.RemoveAt(this.currentPage - 1);
			
			// Remove that menu item.
			RemovePageMenu(this.currentPage - 1);
			
			this.pageCount--;
			this.currentPage--;
			
			// Make sure the current page and viewed page are correct.
			if (this.pageCount > 0) 
			{
				if (this.currentPage == 0)
					this.currentPage = 1;

				// The image is loaded in the SelectedIndexChanged event.
				this.thumbnailView1.Items[this.currentPage - 1].Selected = true;
				//Viewer.Open(this.currentFile,this.currentPage);
				SetPageMenu(this.currentPage - 1);
			} 
			else 
			{
				Viewer.Images.Clear();
				this.toolBar1.Buttons[3].Enabled = false;
			}
			
			// Update the statusbar.
			this.statusPage.Text = "Page " + this.currentPage.ToString() + " of " + this.pageCount.ToString();
		}

		private void menuAppend_Click(object sender, System.EventArgs e)
		{
			// This will add the current image to an existing TIFF image.
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "TIFF Images|*.tif;*.tiff";
			
			if (openFile.ShowDialog() == DialogResult.OK) 
				Viewer.Image.Save(openFile.FileName, new TiffEncoder(this.tiffCompression, true), null);
			
			openFile.Dispose();
		}
		
		private void menuInsertPage_Click(object sender, System.EventArgs e)
		{
			// Insert a page into the ImageCollection.
			FileStream fs = null;
			FileStream tempFS = null;
			TiffFile editFile = null;
			string localTempFile = System.IO.Path.GetTempFileName();
			try
			{
				fs = new FileStream(this.currentFile, FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				tempFS = new FileStream(localTempFile,FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.ReadWrite);
				editFile = new TiffFile();
				editFile.Read(fs);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message);
				if (fs != null)
					fs.Close();
				if (tempFS != null)
					fs.Close();
				return;
			}
			
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "TIFF Images|*.tif;*.tiff";
			
			if (openFile.ShowDialog() == DialogResult.OK) 
			{
				AtalaImage image = new AtalaImage(openFile.FileName);
				if (image != null) 
				{
					//Viewer.Images.Insert(this.currentPage - 1, image);
					editFile.Images.Insert(this.currentPage - 1, new TiffDirectory(image));
					editFile.Save(tempFS);
					fs.Close();
					tempFS.Close();
					// copy from temp stream.
					System.IO.File.Copy(localTempFile,this.currentFile, true);

					this.thumbnailView1.Items.Insert(this.currentPage - 1, openFile.FileName, 0, "Page " + this.currentPage.ToString(), "");
					this.pageCount++;
					
					// Make sure the menu has been added.
					if (this.pageMenus == null || this.pageCount == 2)
						ResetPageMenuItems();
					else
						// Add the menu item.
						if (this.pageCount <= this.maxPageMenus)
						this.pageMenus.MenuItems.Add("Page " + this.pageCount.ToString(), onPageMenuClick);
					
					// We want to view the image we just added.
					this.thumbnailView1.Items[this.currentPage - 1].Selected = true;
					//Viewer.Open(this.currentFile,this.currentPage - 1);

					// Update the Pages menu selection.
					SetPageMenu(this.currentPage - 1);
				}
				
			}
			
			openFile.Dispose();
			
			// Update the statusbar.
			this.statusPage.Text = "Page " + this.currentPage.ToString() + " of " + this.pageCount.ToString();
		}

		#endregion
		
		#region Metadata Code
		
		private void menuTiffTags_Click(object sender, System.EventArgs e)
		{
			// Display the tag info form.
			if (this.currentFile.Length == 0) {
				MessageBox.Show(this, "You need to load an image first.");
				return;
			}
			
			TagInfo frm = new TagInfo(this.currentFile, this.currentPage - 1);
			frm.ShowDialog();
			frm.Dispose();
		}

		#endregion
		
		#region TIFF Compression
		
		// Clear all of the radio checkmarks.
		private void RemoveTiffCompressionCheckmarks()
		{
			this.menuCompressDeflate.Checked = false;
			this.menuCompressGroup3.Checked = false;
			this.menuCompressGroup4.Checked = false;
			this.menuCompressHuffman.Checked = false;
			this.menuCompressJpeg.Checked = false;
			this.menuCompressLzw.Checked = false;
			this.menuCompressMac.Checked = false;
			this.menuCompressNone.Checked = false;
		}
		
		private void menuCompressNone_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.NoCompression;
			RemoveTiffCompressionCheckmarks();
			menuCompressNone.Checked = true;
		}

		private void menuCompressDeflate_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.Deflate;
			RemoveTiffCompressionCheckmarks();
			menuCompressDeflate.Checked = true;
		}

		private void menuCompressGroup3_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.Group3FaxEncoding;
			RemoveTiffCompressionCheckmarks();
			menuCompressGroup3.Checked = true;
		}

		private void menuCompressGroup4_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.Group4FaxEncoding;
			RemoveTiffCompressionCheckmarks();
			menuCompressGroup4.Checked = true;
		}

		private void menuCompressJpeg_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.JpegCompression;
			RemoveTiffCompressionCheckmarks();
			menuCompressJpeg.Checked = true;
		}

		private void menuCompressLzw_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.Lzw;
			RemoveTiffCompressionCheckmarks();
			menuCompressLzw.Checked = true;
		}

		private void menuCompressMac_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.MacintoshPackbits;
			RemoveTiffCompressionCheckmarks();
			menuCompressMac.Checked = true;
		}

		private void menuCompressHuffman_Click(object sender, System.EventArgs e)
		{
			this.tiffCompression = TiffCompression.ModifiedHuffman;
			RemoveTiffCompressionCheckmarks();
			menuCompressHuffman.Checked = true;
		}
		
		#endregion

		#region Modify Menu

        private void CheckAndApplyCommand(ImageCommand cmd, string name)
        {
            if (Viewer == null || Viewer.Image == null)
                return;
            if (!cmd.IsPixelFormatSupported(Viewer.Image.PixelFormat))
            {
                MessageBox.Show(cmd.GetType().Name + " can't be applied to an image of type " + Enum.GetName(typeof(PixelFormat), Viewer.Image.PixelFormat) + ".");
            }
            else
            {
                Viewer.ApplyCommand(cmd, name);
            }
        }

		private void menuFlipHorizontal_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			FlipCommand flip = new FlipCommand(FlipDirection.Horizontal);
			CheckAndApplyCommand(flip, "Horizontal Flip");
		}

		private void menuFlipVertical_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			FlipCommand flip = new FlipCommand(FlipDirection.Vertical);
			CheckAndApplyCommand(flip, "Vertical Flip");
		}

		private void menuRotate90_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			RotateCommand rotate = new RotateCommand(90);
			CheckAndApplyCommand(rotate, "Undo Rotate 90");
		}

		private void menuRotate180_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			RotateCommand rotate = new RotateCommand(180);
			CheckAndApplyCommand(rotate, "Undo Rotate 180");
		}

		private void menuRotate270_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			RotateCommand rotate = new RotateCommand(270);
			CheckAndApplyCommand(rotate, "Undo Rotate 270");
		}
		
		private void menuRotateCustom_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			RotateCommand rotate = new RotateCommand(1.5);
			CheckAndApplyCommand(rotate, "Undo Rotate 1.5");
		}

		private void menuMorpho_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;

			MenuItem item = (MenuItem)sender;
			
			MorphoDocumentMode mode = (MorphoDocumentMode)item.Index;
			
			MorphoDocumentCommand morpho = new MorphoDocumentCommand(mode);
			if (Viewer.Selection != null && Viewer.Selection.Visible)
				morpho.RegionOfInterest = new RegionOfInterest(Viewer.Selection.GetRegion());
			CheckAndApplyCommand(morpho, "Undo Morpho");
		}

		private void menuUndo_Click(object sender, System.EventArgs e)
		{
			Viewer.Undos.Undo();
		}
		
		private void menuInvert_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			InvertCommand invert = new InvertCommand();
			CheckAndApplyCommand(invert, "Undo Invert");
		}
		private void mnuDeskew_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			try
			{
			
				//get precision
				int precision = 1;
				MenuItem item = (MenuItem)sender;
				switch (item.Index)
				{
					case 0:
						precision = 1;
						break;
					case 1:
						precision = 2;
						break;
					case 2:
						precision = 4;
						break;
					case 3:
						precision = 10;
						break;
				}

				AutoDeskewCommand deskew = null;
				if (Viewer.Selection != null && Viewer.Selection.Visible)
					deskew = new AutoDeskewCommand(Viewer.Selection.Bounds);
				else
					deskew = new AutoDeskewCommand();

				deskew.Precision = precision;
			
				CheckAndApplyCommand(deskew, "Undo Deskew");
			}
			catch(Exception err)
			{
				MessageBox.Show(this, err.Message);
			}
		}

		private void menuDespeckle_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			DocumentDespeckleCommand despeckle = new DocumentDespeckleCommand();
			if (Viewer.Selection != null && Viewer.Selection.Visible)
				despeckle.RegionOfInterest = new RegionOfInterest(Viewer.Selection.GetRegion());
			CheckAndApplyCommand(despeckle, "Despeckle");
		}

		private void menuMedian_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			DocumentMedianCommand median = new DocumentMedianCommand(3);
			if (Viewer.Selection != null && Viewer.Selection.Visible)
				median.RegionOfInterest = new RegionOfInterest(Viewer.Selection.GetRegion());
			CheckAndApplyCommand(median, "Remove Noise");
		}
	
		private void menuThresholdingAdaptive_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				this.Cursor = Cursors.WaitCursor;
				AdaptiveThresholdCommand cmd = new AdaptiveThresholdCommand();
				
				if (this.Viewer.MouseTool == MouseToolType.Selection && this.Viewer.Selection.Visible)
					cmd.RegionOfInterest = new RegionOfInterest(this.Viewer.Selection.Bounds);

				CheckAndApplyCommand(cmd, "Adaptive Thresholding");
				this.Cursor = Cursors.Default;
			}
		}

		private void menuThresholdingGlobal_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				this.Cursor = Cursors.WaitCursor;
				GlobalThresholdCommand cmd = new GlobalThresholdCommand();
				
				if (this.Viewer.MouseTool == MouseToolType.Selection && this.Viewer.Selection.Visible)
					cmd.RegionOfInterest = new RegionOfInterest(this.Viewer.Selection.Bounds);

				CheckAndApplyCommand(cmd, "Global Thresholding");
				this.Cursor = Cursors.Default;
			}
		}

		private void menuThresholdingDynamic_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				this.Cursor = Cursors.WaitCursor;
				DynamicThresholdCommand cmd = new DynamicThresholdCommand();
				
				if (this.Viewer.MouseTool == MouseToolType.Selection && this.Viewer.Selection.Visible)
					cmd.RegionOfInterest = new RegionOfInterest(this.Viewer.Selection.Bounds);

				CheckAndApplyCommand(cmd, "Dynamic Thresholding");
				this.Cursor = Cursors.Default;
			}
		}

		private void menuThresholdingSimple_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				this.Cursor = Cursors.WaitCursor;
				SimpleThresholdCommand cmd = new SimpleThresholdCommand();
				
				if (this.Viewer.MouseTool == MouseToolType.Selection && this.Viewer.Selection.Visible)
					cmd.RegionOfInterest = new RegionOfInterest(this.Viewer.Selection.Bounds);
   				CheckAndApplyCommand(cmd, "Simple Thresholding");
				this.Cursor = Cursors.Default;
			}
		}

		private void menuBorderRemovalAll_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				BorderRemovalCommand cmd = new BorderRemovalCommand(BorderRemovalEdges.AllSides, 5, false);
                if (cmd.IsPixelFormatSupported(this.Viewer.Image.PixelFormat))
                    CheckAndApplyCommand(cmd, "Border Removal");
			}
		}

		private void menuBorderRemovalV_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				BorderRemovalCommand cmd = new BorderRemovalCommand(BorderRemovalEdges.Vertical, 5, false);
				CheckAndApplyCommand(cmd, "Border Removal (Vertical)");
			}
		}

		private void menuBorderRemovalH_Click(object sender, System.EventArgs e)
		{
			this.imageChanged = true;
			if (this.Viewer.Image != null)
			{
				BorderRemovalCommand cmd = new BorderRemovalCommand(BorderRemovalEdges.Horizontal, 5, false);
				CheckAndApplyCommand(cmd, "Border Removal (Horizontal)");
			}
		}

		
		#endregion

		#region Form Events

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(e.Button.ToolTipText)
			{
				case "Open":
					OpenImage();
					break;
				
				case "Save":
					SaveImage();
					break;
			
				case "Previous":
					if (this.currentPage > 1)
					{
						//load the next page from the file
						this.currentPage--;
						Viewer.Open(this.currentFile, this.currentPage - 1);
						// change thumnail selection.
						this.thumbnailView1.Items[this.currentPage].Selected = false;
						this.thumbnailView1.Items[this.currentPage-1].Selected = true;
						this.thumbnailView1.EnsureVisible(this.currentPage-1);
					}
					break;
			
				case "Next":

					if (this.currentPage < this.pageCount)
					{
						//load the next page from the file
						this.currentPage++;
						Viewer.Open(this.currentFile, this.currentPage - 1);
						// change thumnail selection.
						this.thumbnailView1.Items[this.currentPage-2].Selected = false;
						this.thumbnailView1.Items[this.currentPage-1].Selected = true;
						this.thumbnailView1.EnsureVisible(this.currentPage-1);
					}
					break;
				
				case "Arrow":
					Viewer.MouseTool = MouseToolType.None;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = false;
					tbPan.Pushed = false;
					tbArrow.Pushed = true;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = false;
					break;
				case "Rectangle Selection":
					Viewer.MouseTool = MouseToolType.Selection;
					Viewer.Selection = this.rectangleSelect;
					tbArrow.Pushed = false;
					tbPan.Pushed = false;
					tbSelectRectangle.Pushed = true;
					tbSelectEllipse.Pushed = false;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = false;
					break;
				case "Ellipse Selection":
					Viewer.MouseTool = MouseToolType.Selection;
					Viewer.Selection = this.ellipseSelect;
					tbArrow.Pushed = false;
					tbPan.Pushed = false;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = true;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = false;
					break;
				case "Pan":
					Viewer.MouseTool = MouseToolType.Pan;
					tbArrow.Pushed = false;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = false;
					tbPan.Pushed = true;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = false;
					break;
				case "Magnifier":
					Viewer.MouseTool = MouseToolType.Magnifier;
					tbArrow.Pushed = false;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = false;
					tbPan.Pushed = false;
					tbMagnifier.Pushed = true;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = false;
					break;
				case "Zoom":
					Viewer.MouseTool = MouseToolType.Zoom;
					tbArrow.Pushed = false;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = false;
					tbPan.Pushed = false;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = true;
					tbZoomSelection.Pushed = false;
					break;
				case "Zoom Selection":
					Viewer.MouseTool = MouseToolType.ZoomArea;
					tbArrow.Pushed = false;
					tbSelectRectangle.Pushed = false;
					tbSelectEllipse.Pushed = false;
					tbPan.Pushed = false;
					tbMagnifier.Pushed = false;
					tbZoom.Pushed = false;
					tbZoomSelection.Pushed = true;
					break;
			}
		}

		
		private void Viewer_Progress(object sender, Atalasoft.Imaging.ProgressEventArgs e)
		{
			if (e.Total == 0) 
				e.Total = 1;
			progressBar1.Value = e.Current * 100 / e.Total;
			if (progressBar1.Value == 100)
			{
				progressBar1.Visible = false;
				progressBar1.Value = 0;
			}
			else if (progressBar1.Value == 0)
			{
				progressBar1.Visible = true;
			}
		}

		private void Viewer_ChangedImage(object sender, Atalasoft.Imaging.ImageEventArgs e)
		{
			// Update the statusbar.
			statusPage.Text = "Page " + this.currentPage.ToString() + " of " + this.pageCount.ToString();
			statusMessage.Text = this.currentFile;

			//enable or disable previous/next buttons

			if (this.currentPage == 1)
				toolBar1.Buttons[3].Enabled = false;
			else
				toolBar1.Buttons[3].Enabled = true;

			if (this.currentPage == this.pageCount)
				toolBar1.Buttons[4].Enabled = false;
			else
				toolBar1.Buttons[4].Enabled = true;

			if (this.imageChanged) // image data changed, reload thumbs
			{
				this.thumbnailView1.Items.Update(this.thumbnailView1.Items[this.currentPage-1],e.Image);
			}
		}

		private void mnuAutoZoom_Click(object sender, System.EventArgs e)
		{
			foreach (MenuItem mitem in contextMenu1.MenuItems)
				mitem.Checked = false;

			//set the AutoZoom mode based on menu pick
			MenuItem item = (MenuItem)sender;
			AutoZoomMode zoomMode = (AutoZoomMode)item.Index;
			Viewer.AutoZoom = zoomMode;
			item.Checked = true;
		}

		
		private void cmbAutoZoom_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//set the AutoZoom mode based on menu pick
			AutoZoomMode zoomMode = (AutoZoomMode)cmbAutoZoom.SelectedIndex;
			Viewer.AutoZoom = zoomMode;
			if (zoomMode == AutoZoomMode.None) Viewer.Zoom = 1;
		}

		private void menuAbout_Click(object sender, System.EventArgs e)
		{
			AtalaDemos.AboutBox.About aboutBox = new AtalaDemos.AboutBox.About("About Atalasoft DotImage Multipage TIFF Demo",
				"DotImage Multipage TIFF Demo");
			aboutBox.Description = @"The Multipage Tiff Demo shows how to view and save multipage Tiff images using the TiffFile class, which allows editing of the image, without the need to open the entire image into memory or the need to re-encode the image data.  Some of this functionality includes adding, reordering, or removing pages from the image.  The demo also demonstrates many of the Document Imaging functions that are provided with DotImage.  In addition, this demo makes great use of the ThumbnailView control, displaying all pages in a multipage TIFF in the thumbnail control, as well as allowing the user to reorder and manipulate the individual pages with a nice GUI.";
			aboutBox.ShowDialog();
		}

		private void thumbnailView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (thumbnailView1.SelectedItems.Count > 0)
			{
				if(this.imageChanged) // save this image first
				{
					// open the file for edit
					string localTempFile = System.IO.Path.GetTempFileName();
					
					TiffFile editFile = new TiffFile();
					using (FileStream fs = new FileStream(this.currentFile, FileMode.Open, FileAccess.ReadWrite))
					{
						editFile.Read(fs);
						editFile.Images[this.currentPage-1] = new TiffDirectory(Viewer.Image, new TiffEncoder());
						using (FileStream tempFS = new FileStream(localTempFile, FileMode.Open, FileAccess.ReadWrite))
						{
							editFile.Save(tempFS);
						}
					}

					File.Copy(localTempFile,this.currentFile, true);
					this.imageChanged = false;
				}
				// load selected page
				this.currentPage = this.thumbnailView1.SelectedIndices[0] + 1;
				try
				{
					Viewer.Open(this.currentFile, this.currentPage - 1);
                    SetPageMenu(this.currentPage-1);
				}
				catch(Exception)
				{
					MessageBox.Show(this, "Page " + (this.currentPage - 1).ToString() + " is an invalid or corupt TIFF image.", "Error Loading Page");
				}
			}
		}

		#endregion

		#region Drag and Drop

		private void thumbnailView1_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			// This event fires when a thumbnail drag operation is starting.
			this.dragDropIndex = this.thumbnailView1.SelectedIndices;
		}

		private void thumbnailView1_ThumbnailDrop(object sender, Atalasoft.Imaging.WinControls.ThumbnailDropEventArgs e)
		{
			// This event fires when a thumbnail has been dropped on the control.
			
			// This demo doesn't handle dragging from one ThumbnailView to another.
			if (this.dragDropIndex.Length == 0) return;
			// make sure we have a valid location
			if (e.InsertIndex < 0 || e.InsertIndex > this.thumbnailView1.Items.Count)
				// do nothing
				return;

			// Move the image in the WorkspaceViewer so it will match the ThumbnailView.
			try
			{	// open the file for edit
				string localTempFile = System.IO.Path.GetTempFileName();
				
				TiffFile editFile = new TiffFile();
				using (FileStream fs = new FileStream(this.currentFile, FileMode.Open, FileAccess.ReadWrite))
				{
					editFile.Read(fs);

					// Remove original location
					TiffDirectory theImage = editFile.Images[this.dragDropIndex[0]];
					editFile.Images.RemoveAt(this.dragDropIndex[0]);
					// Insert at new location
					editFile.Images.Insert((e.InsertIndex > editFile.Images.Count ? editFile.Images.Count : e.InsertIndex), theImage);

					// save the editFile
					using (FileStream tempFS = new FileStream(localTempFile, FileMode.Open, FileAccess.ReadWrite))
					{
						editFile.Save(tempFS);
					}
				}
				File.Copy(localTempFile, this.currentFile, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error Updating ImageCollection");
			}

			this.dragDropIndex = new int[0];
		}

		#endregion



	}
}
