using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;
using Leaf.Web.LeafService;

namespace Leaf.Web.Admin
{
    public partial class AddLeaf : System.Web.UI.Page
    {
        public void ShowFlashMessage(string text)
        {
            flashMessageBox.Visible = true;
            flashMessageText.Text = text;
        }

        public void HideFlashMessage()
        {
            flashMessageBox.Visible = false;
            flashMessageText.Text = "";
        }

        protected void BindTreesList()
        {
            DropDownListTrees.Items.Clear();

            LeafServiceClient client = new LeafServiceClient();
            Tree[] trees = client.GetTrees();

            TreePair[] newTrees = new TreePair[trees.Length + 1];
            for (int i = 0; i < trees.Length; i++)
            {
                newTrees[i] = new TreePair();
                newTrees[i].ID = trees[i].ID;
                newTrees[i].Name = trees[i].RodoveCzech + " " + trees[i].DruhoveCzech;
            }

            newTrees[trees.Length] = new TreePair();
            newTrees[trees.Length].ID = 0;
            newTrees[trees.Length].Name = "-- New tree --";

            DropDownListTrees.DataSource = newTrees;
            DropDownListTrees.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownListTrees.Attributes.Add("onchange", "OnSelectedIndexChange(this);");

            if (!Page.IsPostBack)
            {
                BindTreesList();
            }

            if (Request.QueryString["s"] != null)
            {
                ShowFlashMessage("New leaf sucessfully added to database.");
            }
        }

        public bool IsPreview = false;
        public bool IsNewTreeInPreview = false;

        protected void ButtonSaveLeaf_Click(object sender, EventArgs e)
        {
            byte[] data = null;

            if (HiddenFieldIsPreview.Value == "1")
            {
                PlaceHolderPreview.Visible = false;
                PlaceHolderUpload.Visible = false;
                PlaceHolderImage.Visible = false;

                DataTable dt = new DataTable();
                dt = (DataTable)Session["dtPreview"];

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //storing image binary information in byte array variable.
                        data = ((byte[])dt.Rows[0]["Image"]);
                    }
                }
            }
            else
            {
                if (!fuLeafFile.HasFile)
                {
                    ShowFlashMessage("No image file uploaded.");
                    return;
                }

                if (System.IO.Path.GetExtension(fuLeafFile.FileName).ToLower() != ".jpeg" &&
                    System.IO.Path.GetExtension(fuLeafFile.FileName).ToLower() != ".jpg"
                    )
                {
                    ShowFlashMessage("Only JPEG photos are accepted.");
                    return;
                }

                HideFlashMessage();

                Stream stream = fuLeafFile.FileContent;
                StreamReader reader = new StreamReader(stream);

                HttpPostedFile postFile = fuLeafFile.PostedFile;
                int contentLength = postFile.ContentLength;
                data = new byte[contentLength];
                postFile.InputStream.Read(data, 0, contentLength);

            }

            if (CheckBoxPreview.Checked)
            {
                CheckBoxPreview.Checked = false;

                IsPreview = true;
                int treeID = int.Parse(DropDownListTrees.SelectedValue);
                if (treeID == 0)
                {
                    IsNewTreeInPreview = true;
                }
                PlaceHolderPreview.Visible = false;
                PlaceHolderUpload.Visible = false;
                PlaceHolderImage.Visible = true;

                DataTable dtTemp = new DataTable("TempImage");//Creating temprory data table which will store image information
                dtTemp.Columns.Add("Image", System.Type.GetType("System.Byte[]"));//Byte Image column

                DataRow dr = dtTemp.NewRow();
                dr[0] = data;//storing binary image information in table.
                dtTemp.Rows.Add(dr);
                Session["dtPreview"] = dtTemp;//storing temprory table in seesion.

                ImageUploaded.ImageUrl = "~/Admin/DescriptorPreview.aspx"; //Query string id we have passed

                HiddenFieldIsPreview.Value = "1";
            }
            else
            {
                Bitmap img = ImageUtils.BitmapFromBytes(data);
                System.Drawing.Image NewImage = ImageUtils.Resize(img, 400, 400, true);
                data = ImageUtils.ImageToBytes(NewImage);

                string base64 = Convert.ToBase64String(data);

                LeafServiceClient client = new LeafServiceClient();

                int treeID = int.Parse(DropDownListTrees.SelectedValue);
                bool newTree = false;
                if (treeID == 0)
                {
                    treeID = client.AddTree(txtTreeNameCzechRodove.Text, txtTreeNameCzechDruhove.Text, txtTreeNameLatinRodove.Text, txtTreeNameLatinDruhove.Text);
                    newTree = true;
                }

                bool status = client.Learn(treeID, base64);

                if (status)
                {
                    ShowFlashMessage("New leaf sucessfully added to database.");

                    txtTreeNameCzechRodove.Text = "";
                    txtTreeNameCzechDruhove.Text = "";
                    txtTreeNameLatinRodove.Text = "";
                    txtTreeNameLatinDruhove.Text = "";
                    DropDownListTrees.SelectedIndex = 0;
                    HiddenFieldIsPreview.Value = "0";
                    //CheckBoxPreview.Checked = false;
                    if (newTree)
                    {
                        // rebind data including new tree
                        BindTreesList();
                    }

                    Response.Redirect("~/Admin/AddLeaf.aspx?s=1");
                }
                else
                {
                    ShowFlashMessage("Error occured during learning proccess.");
                }
            }
        }
    }

    class TreePair
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}