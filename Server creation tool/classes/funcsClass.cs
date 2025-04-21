using Server_Creation_Tool;
using Server_Creation_Tool.myClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Server_creation_tool.classes
{
    internal class funcsClass
    {

        public Icon convertPNGtoICO(System.Drawing.Image img)
        {
            Bitmap bitmap = (Bitmap)img;
            return Icon.FromHandle(bitmap.GetHicon());
        }
        public Thread StartThread(Action action)
        {
            Thread thread = new Thread(() => { action(); });
            thread.Start();
            return thread;
        }
        public void InvokeIfRequired(Control control, MethodInvoker action)
        {
            try
            {
                if (control.InvokeRequired)
                { control.Invoke(action); }
                else { action(); }
            }
            catch { }
        }

        // this can be used with forms not just buttons                                                                      //if tag is empty it takes transparent as the color
        public void showMenuStripAtBtn(Control menuStrip, Control button, Form parentForm = null, bool showOnSide = false, bool normalColorInTag = true, bool doNotShowBtnPressed = false)//use parentForm only when using a form as a contex menu strip
        {

            //  Point ParentLoc = parentForm.PointToClient(parentForm.Parent.PointToScreen(parentForm.Location));
            Point btnLoc = button.FindForm().PointToClient(button.Parent.PointToScreen(button.Location));
            if (menuStrip is ContextMenuStrip)
            {
                if (showOnSide)
                {
                    (menuStrip as ContextMenuStrip).Show(button, new Point(btnLoc.X + button.Width, btnLoc.Y));//barely working
                }
                else
                {
                    (menuStrip as ContextMenuStrip).Show(button, new Point(button.Width - button.Width, button.Height));
                }

            }
            else
            {
                menuStrip.Show();
                int x;
                int y;
                if (showOnSide)
                {
                    x = btnLoc.X + button.Width;
                    y = btnLoc.Y;
                }
                else
                {
                    x = btnLoc.X;
                    y = btnLoc.Y + button.Height;
                }
                menuStrip.Location = new Point(parentForm.Location.X + x, parentForm.Location.Y + y);

            }
            if (!doNotShowBtnPressed)
            {
                if (button is customSmoothBtn)
                {
                    //make the button that was clicked appear pressed until the menu closes
                    // i avoid to simply take the colorNormal as its done exactly below, because sometimes it reads the pressed state as the normal color and the button looks pressed even after the contex menu strip closes
                    Color normalColor = (button as customSmoothBtn).ColorNormal;
                    if (button.Tag == null && normalColorInTag)
                    {
                        normalColor = Color.Transparent;
                    }
                    else if (button.Tag != null && normalColorInTag && button.Tag.ToString().Contains(','))
                    {
                        normalColor = convertToColor(button.Tag.ToString().Trim());
                    }
                    else if (button.Tag != null && normalColorInTag && !button.Tag.ToString().Contains(','))
                    {
                        normalColor = Color.FromName(button.Tag.ToString().Trim());
                    }

                    (button as customSmoothBtn).BackColor = (button as customSmoothBtn).ColorPressed;
                    (button as customSmoothBtn).ColorNormal = (button as customSmoothBtn).ColorPressed;
                    StartThread(() =>
                    {
                        while (menuStrip.Visible) Thread.Sleep(100);
                        InvokeIfRequired((button as customSmoothBtn), () => { (button as customSmoothBtn).BackColor = normalColor; (button as customSmoothBtn).ColorNormal = normalColor; });
                    });
                }
                else if (button is Button)//DOESN'T WORK PROPERLY
                {
                    //make the button that was clicked appear pressed until the menu closes
                    Color normalColor = button.BackColor;
                    button.BackColor = button.BackColor;//THIS IS WRONG
                    StartThread(() =>
                    {
                        while (menuStrip.Visible && !menuStrip.IsDisposed) Thread.Sleep(100);
                        InvokeIfRequired(button, () => { button.BackColor = normalColor; });
                    });
                }
            }

            menuStrip.BringToFront();
        }

        public static bool DeleteDirectory(string path)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteDirectory(directory);
                }
                try
                {
                    Directory.Delete(path, true);
                }
                catch (IOException)
                {
                    Directory.Delete(path, true);
                    Thread.Sleep(10);
                }
                catch (UnauthorizedAccessException)
                {
                    Directory.Delete(path, true);
                }
                return true;
            }
            catch (DirectoryNotFoundException ex) { log.Append("DIR NOT FOUND: " + ex.ToString()); return false; }

        }
        public Color convertToColor(string value)
        {
            string[] splitArray = value.Split(',');
            return Color.FromArgb(Int32.Parse(splitArray[0]), Int32.Parse(splitArray[1]), Int32.Parse(splitArray[2]));
        }
        public IEnumerable<Control> GetAllChildren(Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);
                yield return next;
            }
        }
        public void RemoveClickEvent(Control b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);

            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);

            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }
        public Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)//UNUSED
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
            .ToArray();
        }

        public void extractZip(string zipPath, string extractPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    entry.ExtractToFile(Path.Combine(extractPath, entry.FullName), true);
                }
            }

        }
        public bool writeTxtFile(string path, string content, string failLogMsg = "")
        {
            try
            { File.WriteAllText(path, content); return true; }
            catch (Exception a)
            {
                log.Append(failLogMsg + ". " + a.ToString()); MessageBox.Show(path);
                return false;
            }
        }
        public static long DirSize(DirectoryInfo d, ref bool cancel)
        {
            try
            {
                long size = 0;
                // Add file sizes.
                FileInfo[] fis;
                try
                {
                    fis = d.GetFiles();
                }
                catch { cancel = true; return 0; }//0 means dir not found
                foreach (FileInfo fi in fis)
                {
                    if (cancel) return 0;
                    size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    if (cancel) return 0;
                    size += DirSize(di, ref cancel);
                }
                return size;
            }
            catch
            {
                cancel = true;
                return -1;//-1 means some other kind of error. 
            }
        }
        public object readEmbeddedRes(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // string[] names = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }


        }
        public object Populate(object[] arr, object value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
            return arr;
        }
    }

}
