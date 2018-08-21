using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Web.UI;

namespace WhoSitsWhere
{
    class AD_class
    {
        public List<DirectoryEntry> AD_class_getusers()
        {
            string DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            List<DirectoryEntry> list_users = new List<DirectoryEntry>();
            using (var context = new PrincipalContext(ContextType.Domain, DomainName))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        list_users.Add(de);
                    }
                }
            }
            return list_users;
        }

        public void DrawLineInt(Bitmap bmp)
        {

            Pen blackPen = new Pen(Color.Black, 3);

            int x1 = 100;
            int y1 = 100;
            int x2 = 500;
            int y2 = 100;
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
            }
        }

        public string WriteoutHTML(List<string> list_users)
        {
            StringWriter stringWriter = new StringWriter();

            // Put HtmlTextWriter in using block because it needs to call Dispose.
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                // Loop over some strings.
                foreach (string user in list_users)
                {
                    // Some strings for the attributes.
                    string seat = GetSeat(user);

                    // The important part:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, user);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div); // Begin #1

                    writer.AddAttribute(HtmlTextWriterAttribute.Href, seat);
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // Begin #2

                    writer.AddAttribute(HtmlTextWriterAttribute.Src, "Seat");
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "60");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "60");
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");

                    writer.RenderBeginTag(HtmlTextWriterTag.Img); // Begin #3
                    writer.RenderEndTag(); // End #3



                    writer.RenderEndTag(); // End #2
                    writer.RenderEndTag(); // End #1
                }
            }
            // Return the result.
            return stringWriter.ToString();
        }


        private static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        private string GetSeat(string user)
        {
            string DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + DomainName);
            DirectorySearcher dSearch = new DirectorySearcher(entry);
            dSearch.Filter = "(&(objectClass=user)(l=" + user + "))";
            string ad = "";
            foreach (SearchResult sResultSet in dSearch.FindAll())
            {
                // Address
                string tempAddress = GetProperty(sResultSet, "OfficeLocation");

                if (tempAddress != string.Empty)
                {
                    string[] addressArray = tempAddress.Split(';');
                    string taddr1, taddr2;
                    taddr1 = addressArray[0];
                    ad = taddr1;
                }


            }

            return ad;

        }
    }

}
