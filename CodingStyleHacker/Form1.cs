using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodingStyleHacker
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> RndVariableList = new List<string>();
        string GetRandName()
        {
            string NewRandomVarName = "";
            do
            {
                NewRandomVarName = "v_" + new Random().Next(5, 1000).ToString();
            } while (RndVariableList.Contains(NewRandomVarName));
            return (NewRandomVarName);
        }

        string ForToWhileMethod(string SourceCode)
        {
            while (SourceCode.Contains("for"))
            {
                int LBrack_count = 0,Semicolon_count = 0;
                int ForPos = SourceCode.IndexOf("for");
                int LBrackPos = SourceCode.IndexOf("(", ForPos);
                int p = LBrackPos;
                string[] ForContent = new string[3] {"","",""};

                for (; ; p++)
                {
                    if (SourceCode[p] == ';')
                    {
                        Semicolon_count++;
                        continue;
                    }
                    else if (SourceCode[p] == '(')
                    {
                        LBrack_count++;
                        if (LBrack_count == 1) continue;
                    }
                    else if (SourceCode[p] == ')')
                    {
                        LBrack_count--;
                        if (LBrack_count == 0) break;
                    }
                    ForContent[Semicolon_count] += SourceCode[p];
                }

                if (ForContent[2] != "") SourceCode = SourceCode.Insert(SourceCode.IndexOf("}", p), ForContent[2] + ";\n");
                SourceCode = SourceCode.Remove(LBrackPos + 1, p - LBrackPos - 1);
                SourceCode = SourceCode.Insert(LBrackPos + 1, ForContent[1]);
                SourceCode = SourceCode.Remove(ForPos, 3);
                SourceCode = SourceCode.Insert(ForPos, "while");
                SourceCode = SourceCode.Insert(ForPos, ForContent[0] + ";\n");
           }
            return (SourceCode);
        }
        private string RemoveStr(string Source,string[] Patten)
        {
            foreach (string item in Patten) Source = Source.Replace(item, null);
            return (Source);
        }

        private string GetManyBlankStr(int count)
        {
            string Tempstr = "";
            for (; count > 0; count--) Tempstr += "\x09";
            return (Tempstr);
        }

        private string BeautyMyCode(string Source)
        {
            Source = RemoveStr(Source, new string[] { "\n", "\x09", "  " });
            Source = new Regex("#include <(.*?)>").Replace(Source, "$0\n");
            Source = new Regex(";").Replace(Source, "$0\n");
            Source = new Regex("{").Replace(Source, "\n$0\n");
            Source = new Regex("}").Replace(Source, "$0\n");
            int DeepCount = 0;
            for (int i = 0; i < Source.Length; i++)
            {
                if (Source[i] == '}') DeepCount--;
                else if (Source[i] == '{') DeepCount++;
                else if (Source[i]  == '\n')
                {
                    int NextNewLinePos = Source.IndexOf("\n", i + 1);
                    if (NextNewLinePos != -1)
                    {
                        Console.WriteLine(Source.Substring(i, NextNewLinePos - i));
                        if (Source.Substring(i, NextNewLinePos - i).Contains("}"))
                        {
                            Source = Source.Insert(i + 1, GetManyBlankStr(DeepCount -1));
                        }
                        else
                        {
                            Source = Source.Insert(i + 1, GetManyBlankStr(DeepCount));
                        }
                        
                    }
                }
            }
            return (Source);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string SourceCode = richTextBox1.Text;
            SourceCode = ForToWhileMethod(SourceCode);
            richTextBox2.Text = BeautyMyCode(SourceCode);
        }

    }
}
