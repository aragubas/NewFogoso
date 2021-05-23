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
using System.Collections.Generic;
using System.Threading;

namespace Fogoso.Taiyou
{
    public static class Event
    {
        // Lists
        public static List<EventObject> EventList = new List<EventObject>();
        public static List<string> EventListNames = new List<string>();

        private static string LastEventError = "";


        /// Event Dispatcher
        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="EventName">Event name added to Event Queue</param>
        public static void TriggerEvent(string EventName)
        {
            int EventID = EventListNames.IndexOf(EventName);
            if (EventID == -1)
            {
                // Don't flood the console with the same event error over and over again
                if (LastEventError != EventName)
                {
                    LastEventError = EventName;

                    Console.WriteLine("Error: No event with name [" + EventName + "] has been registred.");
                }
   

                return;
            }
            EventList[EventID].InterpreterInstance.Interpret();

        }

        /// <summary>
        /// Renames the event.
        /// </summary>
        /// <param name="EventName">Event name.</param>
        /// <param name="NewName">New name.</param>
        public static void RenameEvent(string EventName, string NewName)
        {
            // Check if event already exists
            int EventID = EventListNames.IndexOf(EventName);

            // If already exists, return
            if (EventID != -1)
            {
                Console.WriteLine("System.Taiyou.RenameEvent : Cannot find the event (" + EventName + ")");
                return;
            }
        }

        /// <summary>
        /// Registers an event.
        /// </summary>
        /// <param name="EventName">Event name.</param>
        /// <param name="EventScript">Event script.</param>
        public static void RegisterEvent(string EventName, string EventScript)
        {
            // Check if event already exists
            int EventID = EventListNames.IndexOf(EventName);
            // If already exists, return
            if (EventID != -1)
            {
                return;
            }


            // Add Event to Event List
            EventListNames.Add(EventName);
            EventList.Add(new EventObject(EventName, EventScript));
        }

        /// <summary>
        /// Removes an event.
        /// </summary>
        /// <param name="EventName">Event name.</param>
        public static void RemoveEvent(string EventName)
        {
            // Check if event already exists
            int EventID = EventListNames.IndexOf(EventName);
            // If already exists, remove it
            if (EventID != -1)
            {
                EventListNames.RemoveAt(EventID);
                EventList.RemoveAt(EventID);
            }
            else
            {
                Console.WriteLine("TaiyouEventHandler ; Cannot delete an event that does not exists, Event[" + EventName + "].");
            }

        }


    }
}
