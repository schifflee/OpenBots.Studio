using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void SaveProject(string scriptPath, Script script, string mainName)
        {
            //Looks through sequential parent directories to find one that matches the script's ProjectName and contains a Main
            string projectPath;
            string dirName;
            string mainScriptPath;
            do
            {
                projectPath = Path.GetDirectoryName(scriptPath);
                DirectoryInfo dirInfo = new DirectoryInfo(projectPath);
                dirName = dirInfo.Name;
                mainScriptPath = Path.Combine(projectPath, mainName);
                scriptPath = projectPath;
            } while (dirName != script.ProjectName || !File.Exists(mainScriptPath));

            //If requirements are met, a project.config is created/updated
            if (dirName == script.ProjectName && File.Exists(mainScriptPath))
            {               
                string projectJSONFilePath = Path.Combine(projectPath, "project.config");
                File.WriteAllText(projectJSONFilePath, JsonConvert.SerializeObject(this));
            }
            else
            {
                throw new Exception("Project Directory Not Found");
            }
        }

        public static Project OpenProject(string mainFilePath)
        {
            //Gets project path and project.config from main script
            string projectPath = Path.GetDirectoryName(mainFilePath);
            string projectJSONPath = Path.Combine(projectPath, "project.config");

            //Loads project from project.config
            Project openProject = new Project();
            if (File.Exists(projectJSONPath))
            {
                string projectJSONString = File.ReadAllText(projectJSONPath);
                openProject = JsonConvert.DeserializeObject<Project>(projectJSONString);

                //updates project.config
                File.WriteAllText(projectJSONPath, JsonConvert.SerializeObject(openProject));
                return openProject;
            }
            else
            {
                throw new Exception("project.config Not Found");
            }
        }
    }
}
