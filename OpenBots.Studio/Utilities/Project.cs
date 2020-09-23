using Newtonsoft.Json;
using System;
using System.IO;

namespace OpenBots.Utilities
{
    public class Project
    {
        public Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Main { get; set; }

        public Project()
        {

        }

        public Project(string projectName)
        {
            ProjectID = Guid.NewGuid();
            ProjectName = projectName;
            Main = "Main.json";
        }

        public void SaveProject(string scriptPath)
        {
            //Looks through sequential parent directories to find one that matches the script's ProjectName and contains a Main
            string projectPath;
            string dirName;
            string configPath;

            try
            {
                do
                {
                    projectPath = Path.GetDirectoryName(scriptPath);
                    DirectoryInfo dirInfo = new DirectoryInfo(projectPath);
                    dirName = dirInfo.Name;
                    configPath = Path.Combine(projectPath, "project.config");
                    scriptPath = projectPath;
                } while (dirName != ProjectName || !File.Exists(configPath));

                //If requirements are met, a project.config is created/updated
                if (dirName == ProjectName && File.Exists(configPath))
                    File.WriteAllText(configPath, JsonConvert.SerializeObject(this));
            }
            catch (Exception)
            {
                throw new Exception("Project Directory Not Found. Saving File Externally.");
            }                       
        }

        public static Project OpenProject(string configFilePath)
        {
            //Loads project from project.config
            if (File.Exists(configFilePath))
            {
                string projectJSONString = File.ReadAllText(configFilePath);
                return JsonConvert.DeserializeObject<Project>(projectJSONString);
            }
            else
            {
                throw new Exception("project.config Not Found");
            }
        }
    }
}
