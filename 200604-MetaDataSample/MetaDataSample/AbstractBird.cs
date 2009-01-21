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
            // "Bird"��� Ű���� ���� �������� �д´�.
            string birdType = ConfigurationManager.AppSettings[keyName];

            // �������� ��õ� IBird �ν��Ͻ��� �������� �����Ѵ�.
            string[] parts = birdType.Split(',');
            string typeName = parts[0].Trim();
            string assemblyName = parts[1].Trim();

            Assembly assembly = Assembly.LoadFrom(assemblyName);
            return (AbstractBird)assembly.CreateInstance(typeName);
        }

        public static void Save(AbstractBird bird, string assemblyName)
        {
            // ���������� �����´�.
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // �������� �����ϰ� �����Ѵ�.
            config.AppSettings.Settings.Remove(keyName);
            config.AppSettings.Settings.Add(keyName, bird.GetType().FullName + ", " + assemblyName);        
            config.Save(ConfigurationSaveMode.Modified);
            // ���������� �ٽ� �д´�.
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
