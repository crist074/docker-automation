using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Newtonsoft.Json;


namespace Automation{
    public class Utility {

        static string currentDirectory = @$"{Environment.CurrentDirectory}/../../../";

        public string LoadJsonConfig(string filepath, string key){
            using (StreamReader r = new StreamReader(currentDirectory + filepath))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach(var item in array)
                {
                    if(item.Name == key){
                        return item.Value;
                    }
                }
            }
            return "";
        }
    }
}