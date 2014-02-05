using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogTrailer {
    class RegexStyle {
        /// <summary>
        /// parse multi line text.
        /// 
        /// === format ===
        /// text : [text] def-line CRLF 
        /// def-line : regex-list { style-list }
        /// regex-list : regex , [regex-list]
        /// style-list : [style-list] style ; 
        /// regex : / string /
        /// style : name = value ;
        /// name : { color|background-color|font-weight }
        /// ex)
        /// /\bINFO\b/{color:blue;}
        /// /\bERROR\b/{color:red;}
        /// 
        /// </summary>
        /// <param name="styleSetting"></param>
        /// <returns></returns>
        public static RegexStyle[] Parse(string styleSetting) {
            List<RegexStyle> styles = new List<RegexStyle>();
            string[] strStyleSpecifierList = Regex.Split(styleSetting, "\\s*\\r*\\n\\s*");
            for (int i = 0; i < strStyleSpecifierList.Length; i++) {
                strStyleSpecifierList[i] = Trim(strStyleSpecifierList[i]);
                if (strStyleSpecifierList[i] == "") {
                    continue;
                }
                Match match = Regex.Match(strStyleSpecifierList[i], "^(.*){(.*)}$");
                if (match.Success && match.Groups.Count >= 2) {
                    RegexStyle regexStyle = new RegexStyle();
                    string[] strSelectorList = Regex.Split(match.Groups[1].Value, "\\s*,\\s*");
                    for (int j = 0; j < strSelectorList.Length; j++) {
                        strSelectorList[j] = Trim(strSelectorList[j]);
                        strSelectorList[j] = Regex.Replace(strSelectorList[j], "/$", "");
                        strSelectorList[j] = Regex.Replace(strSelectorList[j], "^/", "");
                        strSelectorList[j] = Trim(strSelectorList[j]);
                        if (strSelectorList[j] != "") {
                            regexStyle.AddPattern(strSelectorList[j]);
                        }
                    }
                    string[] strStyleList = Regex.Split(match.Groups[2].Value, "\\s*;\\s*");
                    for (int j = 0; j < strStyleList.Length; j++) {
                        strStyleList[j] = Trim(strStyleList[j]);
                        string[] keyValue = Regex.Split(strStyleList[j], "\\s*:\\s*");
                        if (keyValue.Length >= 2) {
                            string key = keyValue[0];
                            string value = "";
                            for (int k = 1; k < keyValue.Length; k++) {
                                value += keyValue[k];
                            }
                            regexStyle.AddStyle(key, value);
                        }
                    }
                    if (strSelectorList.Length > 0 && strStyleList.Length > 0) {
                        styles.Add(regexStyle);
                    }
                }
            }
            return styles.ToArray();
        }
        private static string Trim(string s) {
            s = Regex.Replace(s, "\\s+$", "");
            s = Regex.Replace(s, "^\\s+", "");
            return s;
        }
        List<string> patterns = new List<string>();
        Dictionary<string, string> styles = new Dictionary<string, string>();
        public void AddPattern(string pattern) {
            Console.WriteLine("[RegexStyle.AddPattern] pattern=" + pattern);
            patterns.Add(pattern);
        }
        public void AddStyle(string name, string value) {
            Console.WriteLine("[RegexStyle.AddStyle] name=" + name + ", value=" + value);
            styles.Add(name, value);
        }
        public bool IsMatch(string s) {
            foreach(string pat in patterns) {
                if (Regex.IsMatch(s, pat)) {
                    return true;
                }
            }
            return false;
        }
        public Color Color {
            get {
                Color color = Color.Black;
                string strColor = null;
                if (styles.TryGetValue("color", out strColor)) {
                    color = LibTakamin.Web.CssUtil.ParseColor(strColor);
                }
                return color;
            }
        }
        public Color BackgroundColor {
            get {
                Color color = Color.White;
                string strColor = null;
                if (styles.TryGetValue("background-color", out strColor)) {
                    color = LibTakamin.Web.CssUtil.ParseColor(strColor);
                }
                return color;
            }
        }
        public bool IsBold {
            get {
                string strFontWeight = null;
                if (styles.TryGetValue("background-color", out strFontWeight)) {
                    if (strFontWeight == "bold") {
                        return true;
                    }
                }
                return false; 
            }
        }
    }
}
