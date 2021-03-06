// Copyright � 2009 Syterra Software Inc.
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 2.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

using System;
using System.Collections;
using System.IO;
using fitSharp.IO;

namespace fit.Test.NUnit {
    public class FolderTestModel: FolderModel, ProgressReporter {
        public FolderTestModel() {
            myFiles = new Hashtable();
        }

        public void MakeFile(string thePath, string theContent) {
            myFiles[thePath] = theContent;
        }

        public TextWriter MakeWriter(string thePath) {
            TextWriter writer = new StringWriter();
            myFiles[thePath] = writer;
            return writer;
        }

        public string FileContent(string thePath) {
            return (myFiles.ContainsKey(thePath) ? myFiles[thePath].ToString() : null);
        }

        public bool FileExists(string thePath) {
            return myFiles.ContainsKey(thePath);
        }

        public string[] GetFiles(string thePath) {
            if (thePath.Contains(".")) return new [] {thePath};
            var result = new ArrayList();
            foreach (string file in myFiles.Keys) {
                if (file.StartsWith(thePath + "\\") && file.Substring(thePath.Length + 1).IndexOf("\\") < 0) result.Add(file);
            }
            var files = (string[])result.ToArray(typeof(string));
            Array.Sort(files);
            return files;
        }

        public string[] GetFolders(string thePath) {
            var result = new ArrayList();
            foreach (string file in myFiles.Keys) {
                if (!file.StartsWith(thePath + "\\")) continue;
                int length = file.LastIndexOf("\\");
                if (length <= thePath.Length) continue;
                string folder = file.Substring(0, length);
                if (!result.Contains(folder)) result.Add(folder);
            }
            return (string[])result.ToArray(typeof(string));
        }

        public void CopyFile(string theInputPath, string theOutputPath) {
            MakeFile(theOutputPath, FileContent(theInputPath));
        }
        
        public void Write(string theMessage) {}

        private readonly Hashtable myFiles;
    }
}