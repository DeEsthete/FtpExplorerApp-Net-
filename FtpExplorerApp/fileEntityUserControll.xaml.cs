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

namespace FtpExplorerApp
{
    /// <summary>
    /// Логика взаимодействия для fileEntity.xaml
    /// </summary>
    public partial class FileEntityUserControll : UserControl
    {
        public FileEntity file;

        public FileEntityUserControll(FileEntity fileEntity)
        {
            InitializeComponent();
            file = fileEntity;

            fileNameTextBlock.Text = file.FileName;
            if (file.FileType == FileType.Directory)
            {
                fileImage.Source = BitmapFrame.Create(new Uri(Directory.GetCurrentDirectory() + @"\folder.png"));
            }
            else
            {
                fileImage.Source = BitmapFrame.Create(new Uri(Directory.GetCurrentDirectory() + @"\file.png"));
            }
        }
    }
}
