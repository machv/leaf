using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LeafWeb.LeafService;
using System.IO;
using System.Drawing;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownListTrees.Attributes.Add("onchange", "OnSelectedIndexChange(this);");

            if (!Page.IsPostBack)
            {
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
        }

        protected void ButtonSaveLeaf_Click(object sender, EventArgs e)
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

            byte[] data;
            HttpPostedFile postFile = fuLeafFile.PostedFile;
            int contentLength = postFile.ContentLength;
            data = new byte[contentLength];
            postFile.InputStream.Read(data, 0, contentLength);

            Bitmap img = ImageUtils.BitmapFromBytes(data);
            System.Drawing.Image NewImage = ImageUtils.Resize(img, 400, 400, true);
            data = ImageUtils.ImageToBytes(NewImage);

            string base64 = Convert.ToBase64String(data);

            LeafServiceClient client = new LeafServiceClient();

            int treeID = int.Parse(DropDownListTrees.SelectedValue);

            if (treeID == 0)
            {
                treeID = client.AddTree(txtTreeNameCzechRodove.Text, txtTreeNameCzechDruhove.Text, txtTreeNameLatinRodove.Text, txtTreeNameLatinDruhove.Text);
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
            }
            else
            {
                ShowFlashMessage("Error occured during learning proccess.");
            }
        }
    }

    class TreePair
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}