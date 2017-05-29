using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MetadataExtractor;
using Directory = System.IO.Directory;

namespace ImageInfoWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void selectFilesButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            string folderName = "";

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                string[] dirs = Directory.GetFiles(folderName);
                Regex regex = new Regex("(.TIF)|(.BMP)|(.JPG)|(.GIF)|(.PNG)$", RegexOptions.IgnoreCase);
               
                if (dirs != null)
                {
                    int i = 0;
                    Image loadedImage;
                    foreach (String file in dirs)
                    {
                        if (regex.IsMatch(file) && i < dirs.Length)
                        {
                            loadedImage = Image.FromFile(file);
                            string[] row = GetMetaData(file);

                            if (row[4] == null || row[4].Length == 0)
                            {
                                row[4] = getCompression(loadedImage);
                            }

                            row[2] = Math.Round(loadedImage.HorizontalResolution).ToString();
                            row[3] = Math.Round(loadedImage.VerticalResolution).ToString();

                            dataGridView1.Rows.Add(row);
                            i++;
                        }
                    }
                }
                
            }
        }

        private string[] GetMetaData(string fileName)
        {
            var metadata = ImageMetadataReader.ReadMetadata(fileName);
            var result = new string[5];
            string height = "";
            string width = "";

            foreach (var directory in metadata)
            {
                foreach (var directoryTag in directory.Tags)
                {
                    switch (directoryTag.Name)
                    {
                    case "Compression Type":
                        result[4] = directoryTag.Description;
                        break;
                    case "File Name":
                        result[0] = directoryTag.Description;
                        break;
                    case "Image Height":
                        height = directoryTag.Description.Split(' ')[0];
                        break;
                    case "Image Width":
                        width = directoryTag.Description.Split(' ')[0];
                        break;
                    }
                }
            }

            result[1] = string.Format("{0}x{1}", width, height);

            return result;
        }

        string getCompression(Image image)
        {
            int compressionTagIndex = Array.IndexOf(image.PropertyIdList, 0x103);
            int Type = 0;
            if (compressionTagIndex > -1)
            {
                PropertyItem compressionTag = image.PropertyItems[compressionTagIndex];
                Type = BitConverter.ToUInt16(compressionTag.Value, 0);
            }
            string Result = "No compression";
            switch (Type)
            {
                case 2:
                    Result = "CCITT modified Huffman RLE";
                    break;
                case 3:
                    /* falls through */
                case 32771:
                    Result = "CCITT Group 3 fax encoding";
                    break;
                case 4:
                    Result = "CCITT Group 4 fax encoding";
                    break;
                case 5:
                    Result = "LZW";
                    break;
                case 6:
                    Result = "'old-style' JPEG";
                    break;
                case 7:
                    Result = "'new-style' JPEG";
                    break;
                case 32773:
                    Result = "Macintosh RLE";
                    break;
            }
            return Result;
        }
    }
}
