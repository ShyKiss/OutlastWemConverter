
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct OpenFileName
{
    public int lStructSize;
    public IntPtr hwndOwner;
    public IntPtr hInstance;
    public string lpstrFilter;
    public string lpstrCustomFilter;
    public int nMaxCustFilter;
    public int nFilterIndex;
    public string lpstrFile;
    public int nMaxFile;
    public string lpstrFileTitle;
    public int nMaxFileTitle;
    public string lpstrInitialDir;
    public string lpstrTitle;
    public int Flags;
    public short nFileOffset;
    public short nFileExtension;
    public string lpstrDefExt;
    public IntPtr lCustData;
    public IntPtr lpfnHook;
    public string lpTemplateName;
    public IntPtr pvReserved;
    public int dwReserved;
    public int flagsEx;
}
class OutlastWemConverter
{
    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName(ref OpenFileName ofn);
    private static string ShowDialog()
    {
        var ofn = new OpenFileName();
        ofn.lStructSize = Marshal.SizeOf(ofn);
        ofn.lpstrFilter = "SoundbanksInfo.xml\0SoundbanksInfo.xml\0";
        ofn.lpstrFile = new string(new char[256]);
        ofn.nMaxFile = ofn.lpstrFile.Length;
        ofn.lpstrFileTitle = new string(new char[64]);
        ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
        ofn.lpstrTitle = "Open SoundBanksInfo...";
        if (GetOpenFileName(ref ofn))
            return ofn.lpstrFile;
        return string.Empty;
    }
    static void Main(string[] args)
    {
        string wemsdir = AppDomain.CurrentDomain.BaseDirectory + "/wems";
        string wavsdir = AppDomain.CurrentDomain.BaseDirectory + "/wavs";
        if (!Directory.Exists(wemsdir)) Directory.CreateDirectory(wemsdir);
        if (!Directory.Exists(wavsdir)) Directory.CreateDirectory(wavsdir);
        Console.WriteLine("Press E -> Convert Wem ShortName to ID (Example: VO_02_SE680_0532_TRAG_300532.wem to 463274111.wem)\nPress R -> Convert Wem ID to ShortName (Example: 463274111.wem to VO_02_SE680_0532_TRAG_300532.wem)\nPress T -> Convert Wem ShortName_Hash to ShortName (Example: VO_02_SE860_0645_TRAG_300645_583901A4.wem to VO_02_SE860_0645_TRAG_300645.wem)\nPress D -> Convert Wav ShortName to ID (Example: VO_02_SE680_0532_TRAG_300532.wav to 463274111.wav)\nPress F -> Convert Wav ID to ShortName (Example: 463274111.wav to VO_02_SE680_0532_TRAG_300532.wav)");
        ConsoleKey Key = Console.ReadKey().Key;
        if (Key == ConsoleKey.E)
        {
            Console.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(ShowDialog());
            Console.WriteLine("\nScanning!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string name = node.LocalName;
                if (name == "SoundBanks")
                {
                    foreach (XmlNode curbank in node.ChildNodes) // All Soundbanks
                    {
                        foreach (XmlNode curbanksection in curbank.ChildNodes) // Only IncludedFullFiles Section
                        {
                            if (curbanksection.LocalName == "IncludedFullFiles")
                            {
                                foreach (XmlNode curbankfile in curbanksection.ChildNodes) // All Memory Files in soundbank
                                {
                                    if (File.Exists(wemsdir + "/" + curbankfile.ChildNodes[0].InnerText.Replace(".wav", ".wem")))
                                    {
                                        string sourcewem = wemsdir + "/" + curbankfile.ChildNodes[0].InnerText; // ShortName
                                        string outwem = wemsdir + "/" + curbankfile.Attributes[0].Value + ".wem"; // ID
                                        Console.WriteLine(sourcewem.Replace(".wav", ".wem").Replace(wemsdir + "/", "") + " -> " + outwem.Replace(wemsdir + "/", ""));
                                        File.Move(sourcewem.Replace(".wav", ".wem"), outwem); // Rename ShortName to ID
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\nCompleted!\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        else if (Key == ConsoleKey.R)
        {
            
            Console.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(ShowDialog());
            Console.WriteLine("\nScanning!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string name = node.LocalName;
                if (name == "SoundBanks")
                {
                    foreach (XmlNode curbank in node.ChildNodes) // All Soundbanks
                    {
                        foreach (XmlNode curbanksection in curbank.ChildNodes) // Only IncludedFullFiles Section
                        {
                            if (curbanksection.LocalName == "IncludedFullFiles")
                            {
                                foreach (XmlNode curbankfile in curbanksection.ChildNodes) // All Memory Files in soundbank
                                {
                                    if (File.Exists(wemsdir + "/" + curbankfile.Attributes[0].Value + ".wem"))
                                    {
                                        string sourcewem = wemsdir + "/" + curbankfile.Attributes[0].Value + ".wem"; // Memory File ID
                                        string outwem = wemsdir + "/" + curbankfile.ChildNodes[0].InnerText.Replace(".wav", ".wem"); // Memory File ShortName
                                        Console.WriteLine(sourcewem.Replace(wemsdir + "/", "") + " -> " + outwem.Replace(wemsdir + "/", ""));
                                        File.Move(sourcewem, outwem); // Rename ID to ShortName
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\nCompleted!\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        else if (Key == ConsoleKey.D)
        {
            Console.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(ShowDialog());
            Console.WriteLine("\nScanning!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string name = node.LocalName;
                if (name == "SoundBanks")
                {
                    foreach (XmlNode curbank in node.ChildNodes) // All Soundbanks
                    {
                        foreach (XmlNode curbanksection in curbank.ChildNodes) // Only IncludedFullFiles Section
                        {
                            if (curbanksection.LocalName == "IncludedFullFiles")
                            {
                                foreach (XmlNode curbankfile in curbanksection.ChildNodes) // All Memory Files in soundbank
                                {
                                    if (File.Exists(wavsdir + "/" + curbankfile.ChildNodes[0].InnerText))
                                    {
                                        string sourcewem = wavsdir + "/" + curbankfile.ChildNodes[0].InnerText; // ShortName
                                        string outwem = wavsdir + "/" + curbankfile.Attributes[0].Value + ".wav"; // ID
                                        Console.WriteLine(sourcewem.Replace(wavsdir + "/", "") + " -> " + outwem.Replace(wavsdir + "/", ""));
                                        File.Move(sourcewem, outwem); // Rename ShortName to ID
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\nCompleted!\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        else if (Key == ConsoleKey.F)
        {

            Console.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(ShowDialog());
            Console.WriteLine("\nScanning!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            foreach (XmlNode node in doc.DocumentElement)
            {
                string name = node.LocalName;
                if (name == "SoundBanks")
                {
                    foreach (XmlNode curbank in node.ChildNodes) // All Soundbanks
                    {
                        foreach (XmlNode curbanksection in curbank.ChildNodes) // Only IncludedFullFiles Section
                        {
                            if (curbanksection.LocalName == "IncludedFullFiles")
                            {
                                foreach (XmlNode curbankfile in curbanksection.ChildNodes) // All Memory Files in soundbank
                                {
                                    if (File.Exists(wavsdir + "/" + curbankfile.Attributes[0].Value + ".wav"))
                                    {
                                        string sourcewem = wavsdir + "/" + curbankfile.Attributes[0].Value + ".wav"; // Memory File ID
                                        string outwem = wavsdir + "/" + curbankfile.ChildNodes[0].InnerText; // Memory File ShortName
                                        Console.WriteLine(sourcewem.Replace(wavsdir + "/", "") + " -> " + outwem.Replace(wavsdir + "/", ""));
                                        File.Move(sourcewem, outwem); // Rename ID to ShortName
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\nCompleted!\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        else if (Key == ConsoleKey.T)
        {
            Console.Clear();
            Console.WriteLine("\nScanning!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            foreach (string file in Directory.EnumerateFiles(wemsdir, "*.wem", SearchOption.AllDirectories)) // All .wem Files
            {
                if (File.Exists(file))
                {
                    string sourcewem = file;
                    string outwem = file.Substring(0, file.LastIndexOf("_")) + ".wem";
                    Console.WriteLine(sourcewem.Replace(wemsdir + "\\", "") + " -> " + outwem.Replace(wemsdir + "\\", ""));
                    File.Move(sourcewem, outwem); // Rename ID to ShortName
                }
            }
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\nCompleted!\nPress any key to exit...");
            Console.ReadKey();
            return;
        }
        else return;
        
    }
}
    