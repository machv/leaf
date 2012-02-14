using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;

namespace Leaf.Web
{
    public partial class Image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["dt"];
            byte[] b = null;

            if (dt.Rows.Count > 0)
            {
                //storing image binary information in byte array variable.
                b = ((byte[])dt.Rows[0]["Image"]);
            }

            if (b != null)
            {
                Bitmap img = ImageUtils.BitmapFromBytes(b);
                System.Drawing.Image NewImage = ImageUtils.Resize(img, 200, 200, true);
                byte[] data = ImageUtils.ImageToBytes(NewImage);

                Response.ContentType = "image/jpeg";
                Response.BinaryWrite(data);
            }
        }
    }
}