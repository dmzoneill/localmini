using System.Windows.Forms;
using System.Drawing;

namespace LocalMiniTrayApp
{
    class WebPageBitmap
    {
        private WebBrowser webBrowser;
        private string url;
        private int width;
        private int height;
        private bool isReady;

        public WebPageBitmap(string url, int width, int height, bool scrollBarsEnabled)
        {
            this.url = url;
            this.width = width;
            this.height = height;
            webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(documentCompletedEventHandler);
            webBrowser.Size = new Size(width, height);
            webBrowser.ScrollBarsEnabled = scrollBarsEnabled;
        }
        
        public void Fetch()
        {
            webBrowser.Navigate(url);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            
        }
        private void documentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            isReady = true;
        }
        
        ~WebPageBitmap()
        {
/*            webBrowser.Dispose();
 */
        }

        internal Bitmap GetBitmap(int thumbwidth, int thumbheight)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Rectangle bitmapRect = new Rectangle(0, 0, width, height);
            webBrowser.DrawToBitmap(bitmap, bitmapRect);
            if (thumbheight == height && thumbwidth == width)
            {
                return bitmap;
            }
            else
            {
                Bitmap thumbnail = new Bitmap(thumbwidth, thumbheight);
                Graphics gfx = Graphics.FromImage(thumbnail);
                gfx.DrawImage(bitmap, new Rectangle(0,0, thumbwidth, thumbheight), bitmapRect,GraphicsUnit.Pixel);

                bitmap.Dispose();
                return thumbnail;
            }
        }
    }
}
