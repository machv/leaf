using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using Leaf.Web.LeafService;

namespace Leaf.Web
{
    public partial class _Default : System.Web.UI.Page
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

        }

        protected void ProcessUpload_Click(object sender, EventArgs e)
        {
            if (!LeafPhotoUpload.HasFile && !UrlWithImage.Text.Contains("http://"))
            {
                ShowFlashMessage("No image file specified.");
                return;
            }

            byte[] bytedata;

            if (LeafPhotoUpload.HasFile)
            {
                if (
                    System.IO.Path.GetExtension(LeafPhotoUpload.FileName).ToLower() != ".jpeg" &&
                    System.IO.Path.GetExtension(LeafPhotoUpload.FileName).ToLower() != ".jpg" &&
                    System.IO.Path.GetExtension(LeafPhotoUpload.FileName).ToLower() != ".png" &&
                    System.IO.Path.GetExtension(LeafPhotoUpload.FileName).ToLower() != ".gif"
                    )
                {
                    ShowFlashMessage("Only photos are accepted in JPEG, PNG of GIF format.");
                    return;
                }

                Stream stream = LeafPhotoUpload.FileContent;
                StreamReader reader = new StreamReader(stream);
 
                HttpPostedFile postFile = LeafPhotoUpload.PostedFile;
                int contentLength = postFile.ContentLength;//Storing file length
                bytedata = new byte[contentLength];//Initializing byte variable by passing image content length.
                postFile.InputStream.Read(bytedata, 0, contentLength); //Reading byte content
            }
            else
            {
                try
                {
                    using (System.Net.WebClient webClient = new System.Net.WebClient())
                        bytedata = webClient.DownloadData(UrlWithImage.Text);
                }
                catch
                {
                    ShowFlashMessage("Unable to load image from url.");
                    return;
                }
            }
            HideFlashMessage();

            Bitmap img = ImageUtils.BitmapFromBytes(bytedata);
            System.Drawing.Image NewImage = ImageUtils.Resize(img, 400, 400, true);
            byte[] data = ImageUtils.ImageToBytes(NewImage);

            string base64 = Convert.ToBase64String(data);
            
            LeafServiceClient client = new LeafServiceClient();
            //string reply = client.SayHello();
            Tree[] leafs = client.Recognize(base64, 5);

            //ShowFlashMessage("Leafs found: " + leafs.Length);

            DataTable dtTemp = new DataTable("TempImage");//Creating temprory data table which will store image information
            dtTemp.Columns.Add("Image", System.Type.GetType("System.Byte[]"));//Byte Image column

            DataRow dr = dtTemp.NewRow();
            dr[0] = bytedata;//storing binary image information in table.
            dtTemp.Rows.Add(dr);
            Session["dt"] = dtTemp;//storing temprory table in seesion.

            ImageUploaded.ImageUrl = "Image.aspx"; //Query string id we have passed

            //
            ResponsePlaceHolder.Visible = true;

            string leaf = leafs.Length > 1 ? "matches" : "match";
            LabelResults.Text = leafs.Length + " " + leaf + " found.";

            ResultsRepeater.DataSource = leafs;
            ResultsRepeater.DataBind();
        }
    }
}
