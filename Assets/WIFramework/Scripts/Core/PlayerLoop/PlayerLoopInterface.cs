using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace WIFramework
{
    public static class PlayerLoopInterface
    {

        private static List<PlayerLoopSystem> insertedSystems = new List<PlayerLoopSystem>();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {

            PlayerLoopQuitChecker.GameQuitCallback += () =>
            {
                foreach (var playerLoopSystem in insertedSystems)
                    TryRemoveSystem(playerLoopSystem.type);

                insertedSystems.Clear();
            };
            Hooker.Initialize();
        }

        private enum InsertType
        {
            Before,
            After
        }

        public static void InsertSystemAfter(Type newSystemMarker, PlayerLoopSystem.UpdateFunction newSystemUpdate, Type insertAfter)
        {
            var playerLoopSystem = new PlayerLoopSystem { type = newSystemMarker, updateDelegate = newSystemUpdate };
            InsertSystemAfter(playerLoopSystem, insertAfter);
        }
        public static void InsertSystemBefore(Type newSystemMarker, PlayerLoopSystem.UpdateFunction newSystemUpdate, Type insertBefore)
        {
            var playerLoopSystem = new PlayerLoopSystem { type = newSystemMarker, updateDelegate = newSystemUpdate };
            InsertSystemBefore(playerLoopSystem, insertBefore);
        }
        public static void InsertSystemAfter(PlayerLoopSystem toInsert, Type insertAfter)
        {
            if (toInsert.type == null)
                throw new ArgumentException("The inserted player loop system must have a marker type!", nameof(toInsert.type));
            if (toInsert.updateDelegate == null)
                throw new ArgumentException("The inserted player loop system must have an update delegate!", nameof(toInsert.updateDelegate));
            if (insertAfter == null)
                throw new ArgumentNullException(nameof(insertAfter));

            var rootSystem = PlayerLoop.GetCurrentPlayerLoop();

            InsertSystem(ref rootSystem, toInsert, insertAfter, InsertType.After, out var couldInsert);
            if (!couldInsert)
            {
                throw new ArgumentException($"When trying to insert the type {toInsert.type.Name} into the player loop after {insertAfter.Name}, " +
                                            $"{insertAfter.Name} could not be found in the current player loop!");
            }

            insertedSystems.Add(toInsert);
            PlayerLoop.SetPlayerLoop(rootSystem);
        }
        public static void InsertSystemBefore(PlayerLoopSystem toInsert, Type insertBefore)
        {
            if (toInsert.type == null)
                throw new ArgumentException("The inserted player loop system must have a marker type!", nameof(toInsert.type));
            if (toInsert.updateDelegate == null)
                throw new ArgumentException("The inserted player loop system must have an update delegate!", nameof(toInsert.updateDelegate));
            if (insertBefore == null)
                throw new ArgumentNullException(nameof(insertBefore));

            var rootSystem = PlayerLoop.GetCurrentPlayerLoop();
            InsertSystem(ref rootSystem, toInsert, insertBefore, InsertType.Before, out var couldInsert);
            if (!couldInsert)
            {
                throw new ArgumentException($"When trying to insert the type {toInsert.type.Name} into the player loop before {insertBefore.Name}, " +
                                            $"{insertBefore.Name} could not be found in the current player loop!");
            }

            insertedSystems.Add(toInsert);
            PlayerLoop.SetPlayerLoop(rootSystem);
        }

        public static bool TryRemoveSystem(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Trying to remove a null type!");

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            var couldRemove = TryRemoveTypeFrom(ref currentSystem, type);
            PlayerLoop.SetPlayerLoop(currentSystem);
            return couldRemove;
        }

        private static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, Type type)
        {
            var subSystems = currentSystem.subSystemList;
            if (subSystems == null)
                return false;

            for (int i = 0; i < subSystems.Length; i++)
            {
                if (subSystems[i].type == type)
                {
                    var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];

                    Array.Copy(subSystems, newSubSystems, i);
                    Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);

                    currentSystem.subSystemList = newSubSystems;

                    return true;
                }

                if (TryRemoveTypeFrom(ref subSystems[i], type))
                    return true;
            }

            return false;
        }

        public static PlayerLoopSystem CopySystem(PlayerLoopSystem system)
        {
            // PlayerLoopSystem is a struct.
            var copy = system;

            // but the sub system list is an array.
            if (system.subSystemList != null)
            {
                copy.subSystemList = new PlayerLoopSystem[system.subSystemList.Length];
                for (int i = 0; i < copy.subSystemList.Length; i++)
                {
                    copy.subSystemList[i] = CopySystem(system.subSystemList[i]);
                }
            }

            return copy;
        }

        private static void InsertSystem(ref PlayerLoopSystem currentLoopRecursive, PlayerLoopSystem toInsert, Type insertTarget, InsertType insertType,
                                         out bool couldInsert)
        {
            var currentSubSystems = currentLoopRecursive.subSystemList;
            if (currentSubSystems == null)
            {
                couldInsert = false;
                return;
            }

            int indexOfTarget = -1;
            for (int i = 0; i < currentSubSystems.Length; i++)
            {
                if (currentSubSystems[i].type == insertTarget)
                {
                    indexOfTarget = i;
                    break;
                }
            }

            if (indexOfTarget != -1)
            {
                var newSubSystems = new PlayerLoopSystem[currentSubSystems.Length + 1];

                var insertIndex = insertType == InsertType.Before ? indexOfTarget : indexOfTarget + 1;

                for (int i = 0; i < newSubSystems.Length; i++)
                {
                    if (i < insertIndex)
                        newSubSystems[i] = currentSubSystems[i];
                    else if (i == insertIndex)
                    {
                        newSubSystems[i] = toInsert;
                    }
                    else
                    {
                        newSubSystems[i] = currentSubSystems[i - 1];
                    }
                }

                couldInsert = true;
                currentLoopRecursive.subSystemList = newSubSystems;
            }
            else
            {
                for (var i = 0; i < currentSubSystems.Length; i++)
                {
                    var subSystem = currentSubSystems[i];
                    InsertSystem(ref subSystem, toInsert, insertTarget, insertType, out var couldInsertInInner);
                    if (couldInsertInInner)
                    {
                        currentSubSystems[i] = subSystem;
                        couldInsert = true;
                        return;
                    }
                }

                couldInsert = false;
            }
        }
        public static string CurrentLoopToString()
        {
            return PrintSystemToString(PlayerLoop.GetCurrentPlayerLoop());
        }

        private static string PrintSystemToString(PlayerLoopSystem s)
        {
            List<(PlayerLoopSystem, int)> systems = new List<(PlayerLoopSystem, int)>();

            AddRecursively(s, 0);
            void AddRecursively(PlayerLoopSystem system, int depth)
            {
                systems.Add((system, depth));
                if (system.subSystemList != null)
                    foreach (var subsystem in system.subSystemList)
                        AddRecursively(subsystem, depth + 1);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Systems");
            sb.AppendLine("=======");
            foreach (var (system, depth) in systems)
            {
                // root system has a null type, all others has a marker type.
                Append($"System Type: {system.type?.Name ?? "NULL"}");

                // This is a C# delegate, so it's only set for functions created on the C# side.
                Append($"Delegate: {system.updateDelegate}");

                // This is a pointer, probably to the function getting run internally. Has long values (like 140700263204024) for the builtin ones concrete ones,
                // while the builtin grouping functions has 0. So UnityEngine.PlayerLoop.Update has 0, while UnityEngine.PlayerLoop.Update.ScriptRunBehaviourUpdate
                // has a concrete value.
                Append($"Update Function: {system.updateFunction}");

                // The loopConditionFunction seems to be a red herring. It's set to a value for only UnityEngine.PlayerLoop.FixedUpdate, but setting a different
                // system to have the same loop condition function doesn't seem to do anything
                Append($"Loop Condition Function: {system.loopConditionFunction}");

                // null rather than an empty array when it's empty.
                Append($"{system.subSystemList?.Length ?? 0} subsystems");

                void Append(string s)
                {
                    for (int i = 0; i < depth; i++)
                        sb.Append("  ");
                    sb.AppendLine(s);
                }
            }

            return sb.ToString();
        }

    }
}