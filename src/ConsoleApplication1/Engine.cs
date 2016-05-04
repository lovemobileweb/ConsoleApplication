using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    public class Engine
    {
        private Config mConfig = null;
        List<string>[] mRenameList = new List<string>[2];
        List<string>[] mRenameDirList = new List<string>[2];

        public Engine(string path)
        {
            mConfig = new Config(path);
            mRenameList[0] = new List<string>();
            mRenameList[1] = new List<string>();
            mRenameDirList[0] = new List<string>();
            mRenameDirList[1] = new List<string>();
        }

        public void Run()
        {
            Log.WriteLine("File Contents Processing ...");
            FileContentsProcess(mConfig.mAppPath);
            Log.WriteLine("File Name Processing ...");
            FileNameProcess();
        }

        private void FileContentsProcess(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    int n  = 0;
                    int nPos = 0;
                    n = d.LastIndexOf('\\');
                    if (n >= 0)
                    {
                        string oldDirName = d.Substring(n + 1);
                        string oldPathName = d.Substring(0, n + 1);

                        nPos = oldDirName.IndexOf(mConfig.mSourceName, StringComparison.CurrentCultureIgnoreCase);
                        if (nPos >= 0)
                        {
                            string newDirName = oldDirName.Replace(oldDirName.Substring(nPos, mConfig.mSourceName.Length), mConfig.mTargetName);
                            mRenameDirList[0].Add(d);
                            mRenameDirList[1].Add(oldPathName + newDirName);
                        }
                    }
                    foreach (string f in Directory.GetFiles(d))
                    {
                        try
                        {
                            string strContents = null;
                            {
                                using (FileStream fs = new FileStream(f, FileMode.Open))
                                {
                                    using (StreamReader sr = new StreamReader(fs))
                                    {
                                        strContents = sr.ReadToEnd();
                                        if (strContents.IndexOf(mConfig.mSourceName) >= 0)
                                            Log.WriteLine(string.Format("{0}", f));
                                        strContents = strContents.Replace(mConfig.mSourceName, mConfig.mTargetName);
                                    }
                                }
                            }
                            if (strContents != null)
                            {
                                FileInfo fi = new FileInfo(f);
                                fi.Delete();
                                using (FileStream fs = new FileStream(f, FileMode.CreateNew))
                                {
                                    using (StreamWriter sw = new StreamWriter(fs))
                                    {
                                        sw.Write(strContents);
                                    }
                                }
                                n = f.LastIndexOf('\\');
                                if (n >= 0)
                                {
                                    string oldFileName = f.Substring(n + 1);
                                    string oldPathName = f.Substring(0, n + 1);

                                    nPos = oldFileName.IndexOf(mConfig.mSourceName, StringComparison.CurrentCultureIgnoreCase);
                                    if (nPos >= 0)
                                    {
                                        string newFileName = oldFileName.Replace(oldFileName.Substring(nPos, mConfig.mSourceName.Length), mConfig.mTargetName);
                                        mRenameList[0].Add(f);
                                        mRenameList[1].Add(oldPathName + newFileName);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.WriteLine(string.Format("{0} : error {1}", f, e.Message));
                        }
                    }
                    FileContentsProcess(d);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FileNameProcess()
        {
            for (int i = 0; i < mRenameList[0].Count; i++)
            {
                try
                {
                    FileInfo fi = new FileInfo(mRenameList[0][i]);
                    fi.MoveTo(mRenameList[1][i]);
                    Log.WriteLine(string.Format("{0} to {1}", mRenameList[0][i], mRenameList[1][i]));
                }
                catch (Exception e)
                {
                    Log.WriteLine(string.Format("{0} to {1} : error {2}", mRenameList[0][i], mRenameList[1][i], e.Message));
                }
            }
            for (int i = 0; i < mRenameDirList[0].Count; i++)
            {
                try
                {
                    FileInfo fi = new FileInfo(mRenameDirList[0][i]);
                    fi.MoveTo(mRenameDirList[1][i]);
                    Log.WriteLine(string.Format("{0} to {1}", mRenameDirList[0][i], mRenameDirList[1][i]));
                }
                catch (Exception e)
                {
                    Log.WriteLine(string.Format("{0} to {1} : error {2}", mRenameDirList[0][i], mRenameDirList[1][i], e.Message));
                }
            }
        }
    }
}
