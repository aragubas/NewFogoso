/*
   ####### BEGIN APACHE 2.0 LICENSE #######
   Copyright 2020 Aragubas

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

   ####### END APACHE 2.0 LICENSE #######




   ####### BEGIN MONOGAME LICENSE #######
   THIS GAME-ENGINE WAS CREATED USING THE MONOGAME FRAMEWORK
   Github: https://github.com/MonoGame/MonoGame#license 

   MONOGAME WAS CREATED BY MONOGAME TEAM

   THE MONOGAME LICENSE IS IN THE MONOGAME_License.txt file on the root folder. 

   ####### END MONOGAME LICENSE ####### 


*/

using System;
using Fogoso.Taiyou;
namespace Fogoso.Taiyou.Command
{
    internal abstract class EventHandlerOperation
    {
        public virtual void Run()
        {

        }

    }

    internal class AddOperation : EventHandlerOperation
    {
        string EventName;
        string EventScript;


        public AddOperation(string pEventName, string pEventScript)
        {
            EventName = pEventName;
            EventScript = pEventScript;

        }


        public override void Run()
        {
            Event.EventListNames.Add(EventName);
            Event.EventList.Add(new EventObject(EventName, EventScript));

            base.Run();
        }

    }

    internal class RemoveOperation : EventHandlerOperation
    {
        string EventName;


        public RemoveOperation(string pEventName)
        {
            EventName = pEventName;

        }


        public override void Run()
        {
            Event.TriggerEvent(EventName);

            base.Run();
        }

    }

    internal class CallOperation : EventHandlerOperation
    {
        string EventName;


        public CallOperation(string pEventName)
        {
            EventName = pEventName;

        }


        public override void Run()
        {
            Event.TriggerEvent(EventName);

            base.Run();
        }

    }


    public class EventHandler : TaiyouCommand
    {
        bool Initialized = false;
        EventHandlerOperation Operator;

        public EventHandler(string[] pArguments, string pScriptCaller, TaiyouLine pRootTaiyouLine)
        {
            OriginalArguments = pArguments;

            ScriptCaller = pScriptCaller;
            Title = "EventHandler";
            RootTaiyouLine = pRootTaiyouLine;

        }

        // Event Handler
        //================
        // 1 - Operator (Add, Remove, Call)
        // 2 - Event Name
        // (optional) 3 - Event Script

        public override int Call()
        {
            string[] Arguments = ReplaceVarLiterals();

            if (!Initialized)
            {
                Initialized = true;
 
                string vOperator = GetArgument(Arguments, 0);
                string EventName = GetArgument(Arguments, 1);
                string EventScript = "null";
                // Optional Argument
                if (Arguments.Length >= 3)
                {
                    EventScript = GetArgument(Arguments, 2);
                }

                // Check if event already exists
                int EventNameIndex = Event.EventListNames.IndexOf(EventName);

                switch (vOperator)
                {
                    case "Add":
                        if (EventNameIndex != -1)
                        {
                            throw new TaiyouExecutionError(this, "Execution Error!", "Event [" + EventName + "] already exist");
                        }

                        Operator = new AddOperation(EventName, EventScript);
                        break;

                    case "Remove":
                        if (EventNameIndex == -1)
                        {
                            throw new TaiyouExecutionError(this, "Type Error!", "Event [" + EventName + "] dosen't exist.");
                        }

                        Operator = new RemoveOperation(EventName);
                        break;

                    case "Call":
                        if (EventNameIndex == -1)
                        {
                            throw new TaiyouExecutionError(this, "Type Error!", "Event [" + EventName + "] dosen't exist.");
                        }

                        Operator = new CallOperation(EventName);
                        break;

                }


            }

            Operator.Run();
            return 0;

        }




    }
}