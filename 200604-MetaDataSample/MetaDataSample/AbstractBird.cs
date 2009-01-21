using System;
using System.Configuration;
using System.Reflection;

namespace MetaDataSample
{
    public abstract class AbstractBird
    {
        private const string keyName = "Bird";

        public string KeyName { get { return keyName; } } 
        public abstract void Sing();
        
        public static AbstractBird Load()
        {
            // "Bird"라는 키값을 가진 구성값을 읽는다.
            string birdType = ConfigurationManager.AppSettings[keyName];

            // 구성값에 명시된 IBird 인스턴스를 동적으로 생성한다.
            string[] parts = birdType.Split(',');
            string typeName = parts[0].Trim();
            string assemblyName = parts[1].Trim();

            Assembly assembly = Assembly.LoadFrom(assemblyName);
            return (AbstractBird)assembly.CreateInstance(typeName);
        }

        public static void Save(AbstractBird bird, string assemblyName)
        {
            // 구성파일을 가져온다.
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 구성값을 변경하고 저장한다.
            config.AppSettings.Settings.Remove(keyName);
            config.AppSettings.Settings.Add(keyName, bird.GetType().FullName + ", " + assemblyName);        
            config.Save(ConfigurationSaveMode.Modified);
            // 구성파일을 다시 읽는다.
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
