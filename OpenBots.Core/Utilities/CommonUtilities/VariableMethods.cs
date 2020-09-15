using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class VariableMethods
    {
        /// <summary>
        /// Replaces variable placeholders ({variable}) with variable text.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        public static string ConvertUserVariableToString(this string userInputString, IEngine engine, bool requiresMarkers = true)
        {
            if (string.IsNullOrEmpty(userInputString))
                return string.Empty;

            if (engine == null)
                return userInputString;

            if (userInputString.Length < 2)
                return userInputString;

            var variableList = engine.VariableList;
            var systemVariables = Common.Common.GenerateSystemVariables();

            var variableSearchList = new List<ScriptVariable>();
            variableSearchList.AddRange(variableList);
            variableSearchList.AddRange(systemVariables);

            //variable markers
            var startVariableMarker = "{";
            var endVariableMarker = "}";

            if ((!userInputString.Contains(startVariableMarker) || !userInputString.Contains(endVariableMarker)) && requiresMarkers)
            {
                return userInputString.CalculateVariables(engine);
            }
                
            //split by custom markers
            string[] potentialVariables = userInputString.Split(new string[] { startVariableMarker, endVariableMarker }, StringSplitOptions.None);

            foreach (var potentialVariable in potentialVariables)
            {
                if (potentialVariable.Length == 0) 
                    continue;

                string varcheckname = potentialVariable;
                bool isSystemVar = systemVariables.Any(vars => vars.VariableName == varcheckname);

                if (potentialVariable.Split('.').Length >= 2 && !isSystemVar)
                {
                    varcheckname = potentialVariable.Split('.')[0];
                }

                var varCheck = variableSearchList.Where(v => v.VariableName == varcheckname).FirstOrDefault();
                    
                if (potentialVariable == "OpenBots.EngineContext") 
                    varCheck.VariableValue = engine.GetEngineContext();

                if (varCheck != null)
                {
                    var searchVariable = startVariableMarker + potentialVariable + endVariableMarker;

                    if (userInputString.Contains(searchVariable))
                    {
                        if (varCheck.VariableValue is string)
                        {
                            userInputString = userInputString.Replace(searchVariable, (string)varCheck.VariableValue);
                        }
                        else if (varCheck.VariableValue is List<string> && potentialVariable.Split('.').Length == 2)
                        {
                            //get data from a string list using the index
                            string listIndexString = potentialVariable.Split('.')[1].ConvertUserVariableToString(engine, false);
                            var list = varCheck.VariableValue as List<string>;

                            string listItem;
                            if (int.TryParse(listIndexString, out int listIndex))
                                listItem = list[listIndex].ToString();
                            else
                                return userInputString;

                            userInputString = userInputString.Replace(searchVariable, listItem);
                        }
                        else if (varCheck.VariableValue is DataRow && potentialVariable.Split('.').Length == 2)
                        {
                            //get data from a datarow using the column name/index
                            string columnName = potentialVariable.Split('.')[1].ConvertUserVariableToString(engine, false);
                            var row = varCheck.VariableValue as DataRow;

                            string cellItem;
                            if (int.TryParse(columnName, out var columnIndex))
                                cellItem = row[columnIndex].ToString();
                            else
                                cellItem = row[columnName].ToString();

                            userInputString = userInputString.Replace(searchVariable, cellItem);
                        }
                        else if (varCheck.VariableValue is DataTable && potentialVariable.Split('.').Length == 3)
                        {
                            //get data from datatable using the row index and column name/index	
                            string rowIndexString = potentialVariable.Split('.')[1].ConvertUserVariableToString(engine, false);
                            string columnName = potentialVariable.Split('.')[2].ConvertUserVariableToString(engine, false);
                            var dt = varCheck.VariableValue as DataTable;
                            string cellItem;

                            if (int.TryParse(rowIndexString, out int rowIndex))
                            {                               
                                if (int.TryParse(columnName, out int columnIndex))
                                    cellItem = dt.Rows[rowIndex][columnIndex].ToString();
                                else
                                    cellItem = dt.Rows[rowIndex][columnName].ToString();
                            }
                            else
                                return userInputString;

                            userInputString = userInputString.Replace(searchVariable, cellItem);
                        }
                    }
                    else if (!requiresMarkers)
                    {
                        if (varCheck.VariableValue is string)
                            userInputString = userInputString.Replace(potentialVariable, (string)varCheck.VariableValue);
                    }
                }                           
            }
            return userInputString.CalculateVariables(engine);
        }

        public static object ConvertUserVariableToObject(this string variableName, IEngine engine)
        {
            ScriptVariable requiredVariable;

            if (variableName.StartsWith("{") && variableName.EndsWith("}"))
            {
                //reformat and attempt
                var reformattedVariable = variableName.Replace("{", "").Replace("}", "");
                requiredVariable = engine.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }
            else
                throw new Exception("Variable markers '{}' missing. Variable '" + variableName + "' could not be found.");

            if (requiredVariable != null)
                return requiredVariable.VariableValue;
            else
                return null;
        }

        private static string CalculateVariables(this string str, IEngine engine)
        {
            if (!engine.AutoCalculateVariables)
                return str;
            else
            {
                //track math chars
                var mathChars = new List<char>();
                mathChars.Add('*');
                mathChars.Add('+');
                mathChars.Add('-');
                mathChars.Add('=');
                mathChars.Add('/');

                //if the string matches the char then return
                //as the user does not want to do math
                if (mathChars.Any(f => f.ToString() == str) || (mathChars.Any(f => str.StartsWith(f.ToString()))))
                    return str;

                //bypass math for types that are dates
                DateTime dateTest;
                if (DateTime.TryParse(str, out dateTest) && (str.Split('/').Length == 3 || str.Split('-').Length == 3))
                    return str;

                //test if math is required
                if (mathChars.Any(f => str.Contains(f)))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        var v = dt.Compute(str, "");
                        return v.ToString();
                    }
                    catch (Exception)
                    {
                        return str;
                    }
                }
                else
                    return str;
            }
        }

        /// <summary>
        /// Stores value of the object to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this object variableValue, IEngine engine, string variableName)
        {
            if (variableName.StartsWith("{") && variableName.EndsWith("}"))
                variableName = variableName.Replace("{", "").Replace("}", "");           
            else
                throw new Exception("Variable markers '{}' missing. '" + variableName + "' is an invalid output variable name.");

            if (engine.VariableList.Any(f => f.VariableName == variableName))
            {
                //update existing variable
                var existingVariable = engine.VariableList.FirstOrDefault(f => f.VariableName == variableName);
                existingVariable.VariableName = variableName;
                existingVariable.VariableValue = variableValue;
            }
            else
            {
                //add new variable
                var newVariable = new ScriptVariable();
                newVariable.VariableName = variableName;
                newVariable.VariableValue = variableValue;
                engine.VariableList.Add(newVariable);
            }
        }       

        /// <summary>
        /// Converts a string to SecureString
        /// </summary>
        /// <param name="value">The string to be converted to SecureString</param>
        public static SecureString GetSecureString(this string value)
        {
            SecureString secureString = new NetworkCredential(string.Empty, value).SecurePassword;
            return secureString;
        }

        public static string ConvertSecureStringToString(this SecureString secureString)
        {
            string strValue = new NetworkCredential(string.Empty, secureString).Password;
            return strValue;
        }
    }
}
