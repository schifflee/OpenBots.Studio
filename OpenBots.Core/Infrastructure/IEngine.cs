using Newtonsoft.Json;
using RestSharp;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using OpenBots.Core.Command;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Model.ServerModel;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;

namespace OpenBots.Core.Infrastructure
{
    public interface IEngine
    {
        List<ScriptVariable> VariableList { get; set; }
        List<ScriptElement> ElementList { get; set; }
        Dictionary<string, object> AppInstances { get; set; }
        //ErrorHandlingCommand ErrorHandler;
        List<ScriptError> ErrorsOccured { get; set; }
        string ErrorHandlingAction { get; set; }
        bool ChildScriptFailed { get; set; }
        bool ChildScriptErrorCaught { get; set; }
        ScriptCommand LastExecutedCommand { get; set; }
        bool IsCancellationPending { get; set; }
        bool CurrentLoopCancelled { get; set; }
        bool CurrentLoopContinuing { get; set; }
        [JsonIgnore]
        IfrmScriptEngine ScriptEngineUI { get; set; }
        EngineSettings EngineSettings { get; set; }
        List<DataTable> DataTables { get; set; }
        string FileName { get; set; }
        Task TaskModel { get; set; }
        bool ServerExecution { get; set; }
        List<IRestResponse> ServiceResponses { get; set; }
        bool AutoCalculateVariables { get; set; }
        string TaskResult { get; set; }
        Logger EngineLogger { get; set; }

        event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        void ExecuteScriptAsync(IfrmScriptEngine scriptEngine, string filePath, List<ScriptVariable> variables = null,
                                       List<ScriptElement> elements = null, Dictionary<string, object> appInstances = null);
        void ExecuteScriptAsync(string filePath);
        void ExecuteScriptJson(string jsonData);
        void ExecuteCommand(ScriptAction command);
        string GetEngineContext();
    }
}
