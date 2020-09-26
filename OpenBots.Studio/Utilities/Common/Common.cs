//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenBots.Core.Command;
using OpenBots.Core.Attributes.ClassAttributes;

namespace OpenBots.Studio.Utilities.Common
{
    public static class Common
    {
        /// <summary>
        /// Returns commands from the AutomationCommands.cs file grouped by Custom 'Group' attribute.
        /// </summary>
        // remove it
        public static List<IGrouping<Attribute, Type>> GetGroupedCommands()
        {
            var groupedCommands = Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(t => t.Namespace == "OpenBots.Core.Automation.Commands")
                                          .Where(t => t.Name != "ScriptCommand")
                                          .Where(t => t.IsAbstract == false)
                                          .Where(t => t.BaseType.Name == "ScriptCommand")
                                          .Where(t => CommandEnabled(t))
                                          .GroupBy(t => t.GetCustomAttribute(typeof(Group)))
                                          .ToList();

            return groupedCommands;
        }

        /// <summary>
        /// Returns boolean indicating if the current command is enabled for use in automation.
        /// </summary>
        private static bool CommandEnabled(Type cmd)
        {
            var scriptCommand = (ScriptCommand)Activator.CreateInstance(cmd);
            return scriptCommand.CommandEnabled;
        }
    }
}

