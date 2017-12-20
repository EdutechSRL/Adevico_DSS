using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace lm.Comol.Core.DomainModel.Helpers { 
    public class Thumbnailer
    {
        const int TIMEOUT = 60000;
        const int START_WIDTH = 800;
        const int START_HEIGHT = 6000;

        Bitmap bmp = null;
        ManualResetEvent mre;

        public class ThumbnailerParameters
        {
            public String Url { get; set; }
            public String BaseUrlHttp { get; set; }
            public String BaseUrlHttps { get; set; }
            public Boolean HideImages { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public ThumbnailerParameters(String baseUrlHttp = "", String baseUrlHttps = "")
            {
                BaseUrlHttp= (String.IsNullOrEmpty(baseUrlHttp)) ? "" : baseUrlHttp.ToLower();
                BaseUrlHttps = (String.IsNullOrEmpty(baseUrlHttps)) ? "" : baseUrlHttps.ToLower();
            }
            public Boolean isInternalUrl(String url)
            {
                return (!String.IsNullOrEmpty(BaseUrlHttp) && url.Contains(BaseUrlHttp)) || (!String.IsNullOrEmpty(BaseUrlHttps) && url.Contains(BaseUrlHttps));
            }
            public Boolean isResizable()
            {
                return Width > 0 && Height > 0;
            }
        }
        public Bitmap GetThumbnailFromWeb(string url, Boolean hideImages, String baseUrlHttp="", String baseUrlHttps="", int width = 0, int height = 0)
        {
            mre = new ManualResetEvent(false);

            Thread t = new Thread(new ParameterizedThreadStart(BitmapThreadWorker));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(new ThumbnailerParameters(baseUrlHttp,baseUrlHttps) { Url = url, HideImages = hideImages, Width= width, Height=height });
            mre.WaitOne();

            if (!t.Join(TIMEOUT))
                t.Abort();

            //if (bmp == null)
            //    bmp = new Bitmap(START_WIDTH, START_HEIGHT);
            return bmp;

            //return Resize(bmp, width, height);
        }

        public Bitmap GetThumbnailFromFile(string path, int width, int height)
        {
            Bitmap input = (Bitmap)Bitmap.FromFile(path);
            return Resize(input, width, height);
        }

        public Bitmap GetThumbnailFromBitmap(Bitmap input, int width, int height)
        {
            return Resize(bmp, width, height);
        }

        public Bitmap Resize(Bitmap input, int width, int height)
        {
            Bitmap output = new Bitmap(width, height);
            Rectangle r = new Rectangle(0, 0, width, height);
            Graphics g = Graphics.FromImage(output);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(input, r);
            input.Dispose();
            return output;
        }

        public void BitmapThreadWorker(object parameters)
        {
            DateTime started = DateTime.Now;
            WebBrowser browser = new WebBrowser();
            browser.ScrollBarsEnabled = false;
            browser.ClientSize = new Size(START_WIDTH, START_HEIGHT);

            browser.ScriptErrorsSuppressed = true;
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(((ThumbnailerParameters)parameters).Url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ThumbnailerParameters settings = (ThumbnailerParameters)parameters;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string content = sr.ReadToEnd();

                content = HtmlSanitizer.SanitizeHtml(content);
                HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                hd.LoadHtml(content);

                //if (settings.HideImages)
                //{
                    List<string> xpaths = new List<string>();
                    foreach (HtmlNode node in hd.DocumentNode.Descendants())
                    {
                        if (node.Name.ToLower() == "img")
                        {
                            string src = node.Attributes["src"].Value;
                            if (!settings.HideImages && !settings.isInternalUrl(src))
                            {
                                xpaths.Add(node.XPath);
                            }
                            else
                                xpaths.Add(node.XPath);
                            continue;

                        }
                    }

                    foreach (string xpath in xpaths)
                    {
                        hd.DocumentNode.SelectSingleNode(xpath).Remove();
                    }
                //}
                content = hd.DocumentNode.OuterHtml;


                //var img_tags = HtmlDoc.DocumentNode.SelectNodes("//" + HTML.TAG_IMG + "[@" + HTML.TAG_IMG_SRC + "]");
                //var anchor_tags = hd.DocumentNode.SelectNodes("//" + HTML.TAG_ANCHOR + "[@" + HTML.ATTRIBUT_HREF + "]");
                //var embed_tags = hd.DocumentNode.SelectNodes("//" + HtmlAgilityPack.Crc32 .HTML.TAG_EMBED + "[@" + HTML.TAG_EMBED_SRC + "]");
                //var iframe_tags = hd.DocumentNode.SelectNodes("//" + HTML.TAG_IFRAME + "[@" + HTML.TAG_IFRAME_SRC + "]");
              
                //var audio_tags = hd.DocumentNode.SelectNodes("//" + HTML.TAG_AUDIO);       // may contain inner-html
                //var object_tags = hd.DocumentNode.SelectNodes("//" + HTML.TAG_OBJECT);     // may contain inner-html
                //var video_tags = hd.DocumentNode.SelectNodes("//" + HTML.TAG_VIDEO);       // may contain inner-html



                content = Regex.Replace(content, @"(<head>)", string.Format(@"$1<base href="" {0}="">", ((ThumbnailerParameters)parameters).Url));
                //if (((ThumbnailerParameters)parameters).HideImages)
                //    content = Regex.Replace(content, @"<\s*(script|object|embed|noscript|img)[>]*>(.*?)<\s*/$1\s*>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                //else
                    content = Regex.Replace(content, @"<\s*(script|object|embed|noscript|iframe)[>]*>(.*?)<\s*/$1\s*>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                browser.DocumentText = content;
            }

            while (bmp == null)
            {
                Thread.Sleep(1000);
                Application.DoEvents();
                TimeSpan elapsed = DateTime.Now.Subtract(started);
                if (elapsed.TotalMilliseconds > TIMEOUT)
                {
                    browser.Dispose();
                    mre.Set();
                    break;
                }
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;

            browser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
            //HtmlElementCollection imgs = browser.Document.GetElementsByTagName("img");
            //foreach (HtmlElement item in imgs)
            //{
            //    item.SetAttribute("src", "");

            //    item.Style = "display:none";
            //}

            Rectangle rec = browser.Document.Body.ScrollRectangle;
            if (rec.Width < START_WIDTH)
                rec.Width = START_WIDTH;
            if (rec.Height < START_HEIGHT)
                rec.Height = START_HEIGHT/10;
            bmp = new Bitmap(rec.Width, rec.Height);
            browser.DrawToBitmap(bmp, browser.Bounds);
            browser.Dispose();
            mre.Set();
        }

        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }
    }
}