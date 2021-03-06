﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FGECore.FileSystems;

namespace FGECore.CoreSystems
{
    /// <summary>
    /// A special helper class to assist with live multithreaded asset streaming.
    /// </summary>
    public class AssetStreamingEngine
    {
        /// <summary>
        /// The dedicated file reading thread (splitting across multiple threads is unlikely to benefit anything, and may even get in the way).
        /// </summary>
        public Thread FilesThread;

        /// <summary>
        /// The backing file engine.
        /// </summary>
        public FileEngine Files;

        /// <summary>
        /// The backing scheduler.
        /// </summary>
        public Scheduler Schedule;

        /// <summary>
        /// All currently waiting asset streaming goals.
        /// Controlled mainly by <see cref="AddGoal(string, bool, Action{byte[]}, Action, Action{string})"/>.
        /// </summary>
        public ConcurrentQueue<StreamGoal> Goals = new ConcurrentQueue<StreamGoal>();

        /// <summary>
        /// Reset event for when the files thread has more goals to process.
        /// Set mainly by <see cref="AddGoal(string, bool, Action{byte[]}, Action, Action{string})"/>.
        /// </summary>
        public AutoResetEvent GoalWaitingReset = new AutoResetEvent(false);

        /// <summary>
        /// Construct the <see cref="AssetStreamingEngine"/>. Call <see cref="Init"/> to actually start it.
        /// </summary>
        /// <param name="_files">The backing file engine.</param>
        /// <param name="_schedule">The backing scheduler.</param>
        public AssetStreamingEngine(FileEngine _files, Scheduler _schedule)
        {
            Files = _files;
            Schedule = _schedule;
        }

        /// <summary>
        /// Starts the asset streaming engine.
        /// </summary>
        public void Init()
        {
            FilesThread = new Thread(new ThreadStart(FilesMainLoop)) { Name = "assetstreamingfiles" };
            FilesThread.Start();
        }

        /// <summary>
        /// Shuts down the asset streaming engine.
        /// </summary>
        public void Shutdown()
        {
            FilesThread.Abort();
        }

        /// <summary>
        /// The asset streaming goal to be executed.
        /// </summary>
        public class StreamGoal
        {
            /// <summary>
            /// The name of the file to load. MUST be set.
            /// </summary>
            public string FileName;

            /// <summary>
            /// Action to call in the case of an error. The first parameter is the error message.
            /// If unset, errors go to the <see cref="SysConsole"/>.
            /// </summary>
            public Action<string> OnError = null;

            /// <summary>
            /// Action to call if the file is not present. If unset, will become an error message.
            /// </summary>
            public Action OnFileMissing = null;

            /// <summary>
            /// Called to process the file data once loaded. MUST be set.
            /// </summary>
            public Action<byte[]> ProcessData;

            /// <summary>
            /// A sync action that runs after <see cref="ProcessData"/> is finished.
            /// </summary>
            public Action SyncFollowUp;

            /// <summary>
            /// Whether to sync the process result call to the main thread (if not, runs async).
            /// Defaults to false.
            /// </summary>
            public bool ShouldSyncToMainThread = false;

            /// <summary>
            /// Handles a file-missing situation.
            /// </summary>
            public void HandleFileMissing()
            {
                try
                {
                    if (OnFileMissing != null)
                    {
                        OnFileMissing();
                    }
                    else
                    {
                        HandleError("File '" + FileName + "' not found.");
                    }
                }
                catch (Exception ex2)
                {
                    SysConsole.Output(OutputType.ERROR, "Exception in asset streaming error handler: " + ex2 + "\nCaused by: File not found.");
                }
            }

            /// <summary>
            /// Handles an error message.
            /// </summary>
            /// <param name="message">The error message.</param>
            public void HandleError(string message)
            {
                try
                {
                    if (OnError != null)
                    {
                        OnError(message);
                    }
                    else
                    {
                        SysConsole.Output(OutputType.ERROR, "Asset streaming engine encountered error: " + message);
                    }
                }
                catch (Exception ex2)
                {
                    SysConsole.Output(OutputType.ERROR, "Exception in asset streaming error handler: " + ex2 + "\nCaused by:\n" + message);
                }
            }
        }

        /// <summary>
        /// Entry point of the Files thread.
        /// </summary>
        public void FilesMainLoop()
        {
            while (true)
            {
                GoalWaitingReset.WaitOne();
                while (Goals.TryDequeue(out StreamGoal goal))
                {
                    ProcessGoal(goal);
                }
            }
        }

        /// <summary>
        /// Process a single asset streaming goal.
        /// </summary>
        public void ProcessGoal(StreamGoal goal)
        {
            try
            {
                if (!Files.TryReadFileData(goal.FileName, out byte[] data))
                {
                    goal.HandleFileMissing();
                    return;
                }
                void CallProcData()
                {
                    goal.ProcessData(data);
                }
                if (goal.ShouldSyncToMainThread)
                {
                    Schedule.ScheduleSyncTask(CallProcData);
                }
                else
                {
                    Schedule.StartAsyncTask(CallProcData);
                }
            }
            catch (Exception ex)
            {
                goal.HandleError(ex.ToString());
            }
        }

        /// <summary>
        /// Adds a new goal to the system.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        /// <param name="processOnMainThread">Whether to sync the process result call to the main thread (if not, runs async).</param>
        /// <param name="processAction">Called to process the file data once loaded.</param>
        /// <param name="onFileMissing">(Optional) called to handle a file-missing situation.</param>
        /// <param name="onError">(Optional) called to handle an error message. If unset, errors go to the <see cref="SysConsole"/>.</param>
        /// <returns>The created <see cref="StreamGoal"/>.</returns>
        public StreamGoal AddGoal(string fileName, bool processOnMainThread, Action<byte[]> processAction, Action onFileMissing = null, Action<string> onError = null)
        {
            StreamGoal goal = new StreamGoal()
            {
                FileName = fileName,
                ShouldSyncToMainThread = processOnMainThread,
                ProcessData = processAction,
                OnFileMissing = onFileMissing,
                OnError = onError
            };
            Goals.Enqueue(goal);
            GoalWaitingReset.Set();
            return goal;
        }
    }
}
