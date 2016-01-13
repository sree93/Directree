using System.Collections.Generic;
using System.IO;

namespace Directree.Logic.Cleaner
{
    class IOOperations
    {
        /// <summary>
        /// Deletes all files of specified extensions in a directory (and its subdirectories)
        /// </summary>
        /// <param name="sourceRoot">   The root directory to delete files from</param>
        /// <param name="extList">      The Extensions to delete. ******** KEEP NULL FOR ALL FILES ********</param>
        /// <param name="includeSub">   Recursively delete files in all sub directories</param>
        public static void Delete(string sourceRoot, List<string> extList, bool includeSub)
        {
            if (includeSub)
            {
                // implementing a queue based approach to avoid recursion
                Queue<string> folderQueue = new Queue<string>();
                folderQueue.Enqueue(sourceRoot);

                DirectoryInfo directoryInfo;
                while (folderQueue.Count > 0)
                {
                    string currFolder = folderQueue.Dequeue();
                    clearDirectory(currFolder, extList);

                    directoryInfo = new DirectoryInfo(currFolder);
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        folderQueue.Enqueue(dir.FullName);
                    }
                }
            }
            else
            {
                clearDirectory(sourceRoot, extList);
            }

            deleteEmptyDirectories(sourceRoot);
        }

        /// <summary>
        /// delete all files of specified extentions in a directory
        /// </summary>
        /// <param name="directory">    current directory</param>
        /// <param name="extList">      Extenstions of files that should be erased</param>
        private static void clearDirectory(string directory, List<string> extList)
        {
            DirectoryInfo di = new DirectoryInfo(directory);

            foreach (FileInfo file in di.GetFiles())
            {
                if (extList.Count == 0 || extList.Contains(file.Extension.ToLower()))
                    file.Delete();
            }
        }

        /// <summary>
        /// delete all empty directories after an action
        /// </summary>
        /// <param name="directory"> directory</param>
        private static void deleteEmptyDirectories(string directory)
        {
            // implementing a stack based approach to avoid recursion
            Stack<string> folderStack = new Stack<string>();

            // get to the deepest folder first, populating the stack simultaneously
            Queue<string> folderQueue = new Queue<string>();
            folderQueue.Enqueue(directory);

            DirectoryInfo directoryInfo;
            while (folderQueue.Count > 0)
            {
                string currFolder = folderQueue.Dequeue();
                folderStack.Push(currFolder);

                directoryInfo = new DirectoryInfo(currFolder);
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    folderQueue.Enqueue(dir.FullName);
                }
            }

            // here the stack is filled with folders such that the deepest folder is on stack top
            //now delete all folders by iteratively popping
            while (folderStack.Count > 0)
            {
                DirectoryInfo dir = new DirectoryInfo(folderStack.Pop());

                if (dir.GetDirectories().Length == 0 && dir.GetFiles().Length == 0)
                    dir.Delete();
            }
        }

        /// <summary>
        /// Move all files in a directory that have a specified extentions
        /// </summary>
        /// <param name="sourceRoot"> the root directory of source</param>
        /// <param name="destinationRoot"> the root directory of destination</param>
        /// <param name="extList">      The Extensions to move. ******** KEEP NULL FOR ALL FILES ********</param>
        /// <param name="includeSub">   Recursively move files in all sub directories</param>
        public static void Move(string sourceRoot, string destinationRoot, List<string> extList, bool includeSub)
        {
            List<MoveItem> moveList = new List<MoveItem>();

            if (includeSub)
            {
                // implementing a queue based approach to avoid recursion
                Queue<string> folderQueue = new Queue<string>();
                folderQueue.Enqueue(sourceRoot);

                DirectoryInfo directoryInfo;
                while (folderQueue.Count > 0)
                {
                    string currFolder = folderQueue.Dequeue();
                    moveList.AddRange(moveDirectory(currFolder, extList, sourceRoot, destinationRoot));

                    directoryInfo = new DirectoryInfo(currFolder);
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        folderQueue.Enqueue(dir.FullName);
                    }
                }
            }
            else
            {
                moveList.AddRange(moveDirectory(sourceRoot, extList, sourceRoot, destinationRoot));
            }

            foreach (MoveItem item in moveList)
            {
                item.move();
            }

            deleteEmptyDirectories(sourceRoot);
        }

        /// <summary>
        /// returns all files to move and its destination as a list of MoveItems
        /// this is implemented to resolve the problem of duplicate and infinite moving when source is the ancestor of destination directory or vice versa
        /// </summary>
        /// 
        /// <param name="sourceDirectory">current source directory</param>
        /// <param name="extList">the extension list of files that should be moved</param>
        /// <param name="sourceRoot"> the root source directory</param>
        /// <param name="destinationRoot"> the root destination directory</param>
        /// 
        /// <returns>List of all Moveitems that should be moved of this particular sourceDirectory</returns>
        private static List<MoveItem> moveDirectory(string sourceDirectory, List<string> extList, string sourceRoot, string destinationRoot)
        {
            List<MoveItem> list = new List<MoveItem>();

            DirectoryInfo di = new DirectoryInfo(sourceDirectory);

            string destinationDirectory;
            foreach (FileInfo file in di.GetFiles())
            {
                if (extList.Count == 0 || extList.Contains(file.Extension.ToLower()))
                {
                    destinationDirectory = file.DirectoryName.Replace(sourceRoot, destinationRoot);

                    if (!Directory.Exists(destinationDirectory))
                        Directory.CreateDirectory(destinationDirectory);

                    list.Add(new MoveItem(file, file.FullName.Replace(sourceRoot, destinationRoot)));
                }
            }

            return list;
        }

        /// <summary>
        /// A structure class for all the (source)files that should be moved to (destination) location 
        /// </summary>
        class MoveItem
        {
            private FileInfo source;
            private string destination;

            public MoveItem(FileInfo source, string destination)
            {
                this.source = source;
                this.destination = destination;
            }

            public void move()
            {
                source.MoveTo(destination);
            }
        }
    }
}
