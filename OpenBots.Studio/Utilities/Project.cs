using Newtonsoft.Json;
using System;
using System.IO;
using OpenBots.Core.Script;

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

        public void SaveProject(string scriptPath, Script script)
        {
            //Looks through sequential parent directories to find one that matches the script's ProjectName and contains a Main
            string projectPath;
            string dirName;
            string configPath;
            do
            {
                projectPath = Path.GetDirectoryName(scriptPath);
                DirectoryInfo dirInfo = new DirectoryInfo(projectPath);
                dirName = dirInfo.Name;
                configPath = Path.Combine(projectPath, "project.config");
                scriptPath = projectPath;
            } while (dirName != script.ProjectName || !File.Exists(configPath));

            //If requirements are met, a project.config is created/updated
            if (dirName == script.ProjectName && File.Exists(configPath))
            {               
                File.WriteAllText(configPath, JsonConvert.SerializeObject(this));
            }
            else
            {
                throw new Exception("Project Directory Not Found");
            }
        }

        public static Project OpenProject(string configFilePath)
        {
            //Gets project path and project.config from main script

            //Loads project from project.config
            Project openProject = new Project();
            if (File.Exists(configFilePath))
            {
                string projectJSONString = File.ReadAllText(configFilePath);
                openProject = JsonConvert.DeserializeObject<Project>(projectJSONString);

                //updates project.config
                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(openProject));
                return openProject;
            }
            else
            {
                throw new Exception("project.config Not Found");
            }
        }
    }
}
