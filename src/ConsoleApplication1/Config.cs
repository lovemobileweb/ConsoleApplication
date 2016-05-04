using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    public class Config
    {
        private string mPath = Environment.CurrentDirectory + "\\config.xml";
        public string mSourceName = "";
        public string mTargetName = "";
        public string mAppPath = "";

        public Config(string path)
        {
            Log.WriteLine("Loading config file ...");
            try
            {
                mPath = path;
                using (FileStream fs = new FileStream(mPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string tagNameSourceName = "sourcename";
                        string tagNameTargetName = "targetname";
                        string tagNameAppPath = "apppath";
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            line = line.Trim();

                            string tagName;
                            string tagVal;
                            int nPos = line.IndexOf(':');
                            if (nPos >= 0)
                            {
                                tagName = line.Substring(0, nPos);
                                tagVal = line.Substring(nPos + 1);
                                tagName = tagName.Trim(new char[] { '"', ',', ' ', '\t', '\r', '\n' });
                                tagName = tagName.ToLower();
                                tagVal = tagVal.Trim(new char[] { '"', ',', ' ', '\t', '\r', '\n' });
                                if (tagNameSourceName == tagName)
                                {
                                    mSourceName = tagVal;
                                }
                                else if (tagNameTargetName == tagName)
                                {
                                    mTargetName = tagVal;
                                }
                                else if (tagNameAppPath == tagName)
                                {
                                    mAppPath = tagVal;
                                }
                            }
                        }
                        if (mSourceName == "")
                            throw new ApplicationException("SourceName is not defined! Please check config file");
                        if (mTargetName == "")
                            throw new ApplicationException("TargetName is not defined! Please check config file");
                        if (mAppPath == "")
                            throw new ApplicationException("AppPath is not defined! Please check config file");
                    }
                }
                Log.WriteLine("Loading config file success");
            }
            catch (Exception e)
            {
                Log.WriteLine("Inputed path is invalid or access denied!");
                throw e;
            }
        }
    }
}
