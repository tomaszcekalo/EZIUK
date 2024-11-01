using HtmlAgilityPack;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Windows;

namespace EZIUK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Output.Text = @"@relation departments

@attribute document_name string

@attribute dokument_content string

@dane

";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var address = PageUrl.Text;
            
            //await WriteDocumentToFile(encoded + ".txt", document);
            var row=GetRow(address);
            Output.Text += row;
        }

        public string GetRow(string address)
        {
            var client = new WebClient();
            var content = client.DownloadString(address);

            //var config = Configuration.Default.WithDefaultLoader();
            //var context = BrowsingContext.New(config);
            //var document = await context.OpenAsync(address);
            // encode url in base64
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(address));
            var web = new HtmlWeb();
            var doc = web.Load(address);
            var sb = new StringBuilder();
            sb.Append(doc.DocumentNode.SelectSingleNode("//title").InnerText);
            sb.AppendLine(" \"");
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.Descendants().Where(n =>
                n.NodeType == HtmlNodeType.Text &&
                n.ParentNode.Name != "script" &&
                n.ParentNode.Name != "style");
            foreach (HtmlNode node in nodes)
            {
                if(string.IsNullOrWhiteSpace(node.InnerText)) 
                    continue;
                sb.AppendLine(node.InnerText);
            }
            sb.Replace("\n", " ");
            sb.Replace("\r", " ");

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {

            }
            sb.AppendLine("\"");
            return sb.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "ARFF Files(*.arff)|*.arff|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, Output.Text);
            }
        }
    }
}