using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4b
{
  /*************************
  * AUTHOR: Mohamad Albazeai
  */
    public partial class Form1 : Form
    {
        List<char> readFile = new List<char>();                 // this array were used to store the text in the user file when we read the file to our application.
        List<string> tagList = new List<string>();             // taglist , its stores only the Html tag that came from the readFile array.
        Stack<string> checkTags = new Stack<string>();        // stack list, is comparing the tags that were stored in taglist array.

        //this is the list of the self closing tag / Void tags in html, we use this list when we compare the html tags using Stack list.
        string [] voidTages = { "<area>", "<base>", "<br>", "<col>", "<embed>", "<hr>", "<img>", "<input>", "<link>", "<meta>", "<param>", "<source>", "<track>", "<wbr>"};
        string fileName;   //storing file name to share it in some methods
        public Form1()
        {
            InitializeComponent();
            label.Text = "Load a File!" ;
            
        }

       /// <summary>
       /// this is the load option from the menu under the file selection.
       /// this method is allowing the user to select an Html file to validate the tags. 
       /// </summary>
       /// <param name="sender">object</param>
       /// <param name="e">eventArgs</param>

        private void LoadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tagList.Clear();
            readFile.Clear();
            checkTags.Clear();
            listBox.Items.Clear();
            label.Text = "";
            checkTagsToolStripMenuItem.Enabled = true;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open File";
            openFile.Filter = "|*.html";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFile.FileName);
                char ch;
                while (!reader.EndOfStream)
                {
                    try
                    {
                        ch = (char)reader.Read();

                        if (ch == '<' && ch != ' ' && ch != '>')
                        {
                            while (ch != ' ' && ch != '>')
                            {
                                readFile.Add(ch);
                                ch = (char)reader.Read();
                            }
                        }
                        if (ch == '>')
                        {
                            readFile.Add(ch);

                        }
                       
                    }
                    catch(Exception ex) { }
                   
                }
                label.Text = Path.GetFileName(openFile.ToString());
                fileName = Path.GetFileName(openFile.ToString());
            }
        }
        /// <summary>
        /// this is the check tag option on the menu under the process selection
        /// this method is reading and displaying the html tags that was read it from the user file.
        /// it is also checking the tags in the file that was added by the user,  and call the DisplayTag method
        /// to display the file tags.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">eventArgs</param>
        private void CheckTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                checkTagsToolStripMenuItem.Enabled = false;
                label.Text = "";
                listBox.Items.Clear();
                tagList.Clear();
                checkTags.Clear();
                string text = "";
                foreach (char s in readFile)
                {

                    if (s != '>' && s != ' ')
                    {
                        text += s;
                    }
                    else if (s == '>')
                    {

                        text += s;
                        tagList.Add(text);
                        string tag = text.ToLower();
                        text = "";
                        if (!tag.Contains("/") && !voidTages.Contains(tag))
                        {
                            checkTags.Push(tag.ToLower());
                        }


                        if (tag.Contains("/") && !voidTages.Contains(tag) && checkTags.Count > 0)
                        {
                            string f = checkTags.Peek().ToLower();
                            string t = tag.Replace("/", "").ToLower();
                            if (f == t)
                            {
                                checkTags.Pop();
                            }

                        }

                    }

                }
                 // Messages: shows if the file is good or bad
                if (checkTags.Count > 0)
                {
                    label.Text = fileName + " has a missing Tags!!";
                }
                else
                {
                    label.Text = fileName + " has a Balanced Tags. Good Work!";
                }

                DisplayTags();          // displaying the file tags.
            }catch(Exception ex) { }
        }

        /// <summary>
        /// displaying tag method is displaying the list of html tags that were retrived from the user file.
        /// </summary>
        private void DisplayTags()
        {
            foreach (string tag in tagList)
            {
                if (tag.Contains("/"))
                {
                    listBox.Items.Add("FOUND CLOSING TAG: " + tag);
                }
                else
                {
                    listBox.Items.Add("FOUND OPNING TAG: " + tag);
                }

            }
        }
       
        /// <summary>
        /// this is the exit option on the menu under the file selection.
        /// this method is closing the application.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">eventArgs</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    
    }
}
