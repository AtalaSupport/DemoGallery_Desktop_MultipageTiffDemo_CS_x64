using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Atalasoft.Imaging.Metadata;
using Atalasoft.Imaging;
using Atalasoft.Imaging.Codec.Tiff;

namespace MultipageTiffDemo
{
	/// <summary>
	/// Summary description for TagInfo.
	/// </summary>
	public class TagInfo : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListView lvTags;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ColumnHeader lvId;
		private System.Windows.Forms.ColumnHeader lvTagData;
		private System.Windows.Forms.ColumnHeader lvType;
		
		
		//private MetadataContainer metadata = null;
		private TiffTagCollection tags = null;
		public TagInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		
		public TagInfo(string fileName, int frameIndex)
		{
			InitializeComponent();
			
			// Read the metadata.
			//metadata = new MetadataContainer(fileName, frameIndex);
			TiffFile theFile;
			TiffDirectory tiffImg;
			using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				theFile = new TiffFile();
				theFile.Read(fs);
				tiffImg = theFile.Images[frameIndex];
			
				// this may return some pointers instead of data.
				tags = tiffImg.Tags;
 			
				if ( tags == null) 
				{
					MessageBox.Show("This image doesn't contain any TIFF tags or has not been updated to a file.");
					this.Close();
				}

				DisplayTiffTags();
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lvTags = new System.Windows.Forms.ListView();
			this.btnClose = new System.Windows.Forms.Button();
			this.lvId = new System.Windows.Forms.ColumnHeader();
			this.lvTagData = new System.Windows.Forms.ColumnHeader();
			this.lvType = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvTags
			// 
			this.lvTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.lvId,
																					 this.lvType,
																					 this.lvTagData});
			this.lvTags.Dock = System.Windows.Forms.DockStyle.Top;
			this.lvTags.FullRowSelect = true;
			this.lvTags.MultiSelect = false;
			this.lvTags.Name = "lvTags";
			this.lvTags.Size = new System.Drawing.Size(359, 212);
			this.lvTags.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvTags.TabIndex = 0;
			this.lvTags.View = System.Windows.Forms.View.Details;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(138, 226);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(82, 25);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "&Close";
			// 
			// lvId
			// 
			this.lvId.Text = "Tag ID";
			this.lvId.Width = 120;
			// 
			// lvTagData
			// 
			this.lvTagData.Text = "Tag Data";
			this.lvTagData.Width = 150;
			// 
			// lvType
			// 
			this.lvType.Text = "Type";
			this.lvType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.lvType.Width = 50;
			// 
			// TagInfo
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(359, 266);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnClose,
																		  this.lvTags});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "TagInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TIFF Tag Information";
			this.ResumeLayout(false);

		}
		#endregion
		
		
		// Load the tag info into the listview.
		private void DisplayTiffTags()
		{
			if (tags == null)
				return;
			
			ListViewItem item = null;
			foreach (TiffTag tag in tags) {
				string tagName;
				if (Enum.IsDefined(typeof(TiffTagID), tag.ID))
					tagName = Enum.GetName(typeof(TiffTagID), tag.ID);
				else
					tagName = tag.ID.ToString();
				item = lvTags.Items.Add(tagName);
				item.SubItems.Add(tag.Data.GetType().Name);
				item.SubItems.Add(tag.Data.ToString());
			}
			
			
		}
	}
}
