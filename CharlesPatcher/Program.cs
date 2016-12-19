using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.IO.Compression;

namespace CharlesPatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey charles = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\Charles.Document\shell\open\command");
            string lpPath = charles.GetValue("").ToString().Split('\"')[1];
            lpPath = Path.GetDirectoryName(lpPath) + @"\lib\charles.jar";

            string infile1 = "com/xk72/charles/gui/P.class";
            string infile2 = "com/xk72/charles/gui/Licence.class";
            string infile3 = "com/xk72/charles/License.class";

            using (FileStream fs = new FileStream(lpPath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry ofs;
                    ofs = za.GetEntry(infile1);
                    if (ofs != null && ofs.Length > 10000)
                        PatchCharles(za, ofs, Resource.P);
                    ofs = za.GetEntry(infile2);
                    if (ofs != null && ofs.Length > 10000)
                        PatchCharles(za, ofs, Resource.Licence);
                    ofs = za.GetEntry(infile3);
                    if (ofs != null && ofs.Length > 10000)
                        PatchCharles(za, ofs, Resource.License);
                }
            }
        }

        static void PatchCharles(ZipArchive _Zip, ZipArchiveEntry _File,  Byte[] _Resource)
        {
            using (MemoryStream ifs = new MemoryStream(_Resource))
            {
                _File.Delete();
                ifs.CopyTo(_Zip.CreateEntry(_File.FullName).Open());
            }
        }
    }
}
