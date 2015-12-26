using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace xpsviewerdecoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> xpsList = new List<string>();
        int index = 0;
        DrawingGroup drawgroup = new DrawingGroup();
        private DocumentPaginator paginator;
        private int pageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            xpsList.AddRange(Directory.GetFiles("xps").Where(f => f.EndsWith(".xps")));
            image.Source = new DrawingImage(drawgroup);
            

        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.D1)
            {
                pageIndex = 0;
                index++;
                if (index >= xpsList.Count)
                {
                    index = 0;
                }
                XpsDocument _xpsDocument = new XpsDocument(xpsList[index], System.IO.FileAccess.Read);

                paginator = _xpsDocument.GetFixedDocumentSequence().DocumentPaginator;
                Console.WriteLine("page size " + paginator.PageCount);
                Console.WriteLine("page size " + paginator.PageSize);
                var page = paginator.GetPage(pageIndex);
                
                DrawPage(page.Visual, page.Size);
                
            }
            if (e.Key == Key.D2)
            {
                pageIndex++;
                if(pageIndex >= paginator.PageCount)
                {
                    pageIndex = 0;
                }
                var page = paginator.GetPage(pageIndex);

                DrawPage(page.Visual, page.Size);
            }

        }

        private void DrawPage(Visual visual, Size size)
        {
            using (DrawingContext dc = drawgroup.Open())
            {
                dc.DrawRectangle(new VisualBrush(visual), null, new Rect(0, 0, size.Width, size.Height));
            }
        }
    }
}
