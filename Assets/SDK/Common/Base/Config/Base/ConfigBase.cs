
using System.IO;

namespace SC.XR.Unity {
    
    /// <summary>
    /// 创建一个config文件，并提供操作API
    /// </summary>
    public abstract class ConfigBase : SCModule {

        string patch;
        public ConfigBase(string patch) {
            this.patch = patch;

            FileStream s2 = new FileStream(patch, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            s2.Close();

        }

        public virtual void SetLineValue(int line,string value) {
            if(File.Exists(patch)) {

                string[] lines = File.ReadAllLines(patch);
                if(lines.Length < line) {
                    string[] newLines = new string[line];
                    lines.CopyTo(newLines, 0);
                    newLines[line-1] = value;
                    File.WriteAllLines(patch, newLines);
                } else {
                    lines[line-1] = value;
                    File.WriteAllLines(patch, lines);
                }
            }
            
        }

        /// <summary>
        /// 读取某行内容
        /// </summary>
        /// <param name="line">起始1，1表示第一行</param>
        /// <returns></returns>
        public virtual string GetLineValue(int line) {
            string value = "";

            StreamReader fs = null;

            if(File.Exists(patch)) {
                fs = new StreamReader(patch);

                for(int i = 1; i < line; i++) {
                    fs.ReadLine();
                }
                value = fs.ReadLine();

                DebugMy.Log("config:" + patch + "  line:"+line +" "+ value,this);
                fs.Close();
            }

            return value;
        }


    }

}

